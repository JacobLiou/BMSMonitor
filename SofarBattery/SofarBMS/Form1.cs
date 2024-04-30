using Microsoft.Office.Interop.Excel;
using SofarBMS.Helper;
using SofarBMS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = System.Windows.Forms.Application;
using DataTable = System.Data.DataTable;

namespace SofarBMS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //线程状态
        private bool flag = false;
        private int initCount = 0;
        private RealtimeData model = null;
        private Dictionary<uint, RealtimeData> allQueue = new Dictionary<uint, RealtimeData>();

        // 启动消费者线程，这里假设我们处理每个优先级的队列  
        CancellationTokenSource cts;

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!EcanHelper.IsConnection)
            {
                if (!Connect())
                    return;

                btnConnect.Text = "Disconnect";
                EcanHelper.IsConnection = true;
                Task.Run(() => { EcanHelper.Receive(); });

                Task.Run(() =>
                {
                    while (true)
                    {
                        if (!EcanHelper.IsConnection)
                            continue;

                        Consumer();
                    }
                });

                Task.Run(() =>
                {
                    while (true)
                    {
                        if (!EcanHelper.IsConnection)
                            continue;

                        //获取实时数据指令
                        EcanHelper.Send(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }
                                       , new byte[] { 0xE0, FrmMain.BMS_ID, 0x2C, 0x10 });

                        // 等待一段时间，然后停止消费者线程（仅为示例，实际应用中可能有其他停止条件）  
                        Task.Delay(1000 * 1).Wait();

                        SaveRealtimeData();
                    }
                });
            }
            else
            {
                ECANHelper.CloseDevice(0, 0);
                btnConnect.Text = "Connect";
                EcanHelper.IsConnection = false;
            }
        }

        private void SaveRealtimeData()
        {
            lock (allQueue)
            {

                //List<RealtimeData> lists = new List<RealtimeData>();
                foreach (var _queue in allQueue)
                {
                    uint id = _queue.Key;
                    RealtimeData item = _queue.Value;
                    //lists.Add(item);
                    
                    var filePath = $"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}//Log//BTS5K_{id}_{DateTime.Now.ToString("yyyy-MM-dd")}.csv";

                    //用于确定指定文件是否存在
                    if (!File.Exists(filePath))
                    {
                        File.AppendAllText(filePath, RealtimeData.GetHeader() + "\r\n");
                    }
                    File.AppendAllText(filePath, item.GetValue() + "\r\n");
                }


                //DataTable dt = ModelsToDataTable(lists);
                //List<DataTable> dtList = new List<DataTable>();
                //foreach (var item in dt.Rows)
                //{
                //    dtList.Add(item);
                //}

                //EPPlusHelpr.LoadFromDataTables(new List<DataTable>(){dt });
            }
        }
        /// <summary>
        /// List泛型转换DataTable.
        /// </summary>
        public static DataTable ModelsToDataTable<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }
            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                tb.Rows.Add(values);
            }
            return tb;
        }
        /// <summary>
         /// 如果类型可空，则返回基础类型，否则返回类型
         /// </summary>
        private static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }
        /// <summary>
         /// 指定类型是否可为空
         /// </summary>
        private static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        private bool Connect(string baud = "500Kbps")
        {
            if (ECANHelper.OpenDevice(1, 0, 0) != ECANStatus.STATUS_OK)
            {
                MessageBox.Show(LanguageHelper.GetLanguage("OpenCAN_Error"));
                return false;
            }

            INIT_CONFIG INITCONFIG = new INIT_CONFIG();
            INITCONFIG.AccCode = 0;
            INITCONFIG.AccMask = 0xffffffff;
            INITCONFIG.Filter = 0;
            switch (baud)
            {
                case "125Kbps":
                    INITCONFIG.Timing0 = 0x03;
                    INITCONFIG.Timing1 = 0x1C;
                    break;
                case "250Kbps":
                    INITCONFIG.Timing0 = 0x01;
                    INITCONFIG.Timing1 = 0x1C;
                    break;
                case "500Kbps":
                    INITCONFIG.Timing0 = 0x00;
                    INITCONFIG.Timing1 = 0x1C;
                    break;
                case "1000Kbps":
                    INITCONFIG.Timing0 = 0x00;
                    INITCONFIG.Timing1 = 0x14;
                    break;
            }
            INITCONFIG.Mode = 0;


            if (ECANHelper.InitCAN(1, 0, 0, ref INITCONFIG) != ECANStatus.STATUS_OK)
            {
                MessageBox.Show(LanguageHelper.GetLanguage("InitCAN_Error"));
                ECANHelper.CloseDevice(1, 0);
                return false;
            }

            if (ECANHelper.StartCAN(1, 0, 0) != ECANStatus.STATUS_OK)
            {
                MessageBox.Show(LanguageHelper.GetLanguage("StartCAN_Error"));
                ECANHelper.CloseDevice(1, 0);
                return false;
            }

            return true;
        }

        private void PrintInfo(CAN_OBJ coMsg)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new System.Action(() =>
                {
                    string ss = "";
                    for (int i = 0; i < coMsg.Data.Length; i++)
                    {
                        ss += " " + coMsg.Data[i].ToString("X2");
                    }

                    richTextBox1.AppendText($"{System.DateTime.Now.ToString("HH:mm:ss:fff")} Dequeu   CAN_ID:{coMsg.ID.ToString("X8")},Data：{ss.ToString()}\r\n");
                    richTextBox1.ScrollToCaret();
                }));
            }
            else
            {
                string ss = "";
                for (int i = 0; i < coMsg.Data.Length; i++)
                {
                    ss += " " + coMsg.Data[i].ToString("X2");
                }

                richTextBox1.AppendText($"{System.DateTime.Now.ToString("HH:mm:ss:fff")} Dequeu   CAN_ID:{coMsg.ID.ToString("X8")},Data：{ss.ToString()}\r\n");
            }
        }

        public void analysisData(uint canID, byte[] data)
        {
            uint devID = EcanHelper.AnalysisID(canID);

            if (!(((canID & 0xff) == FrmMain.BMS_ID)
                || ((canID & 0xff) == FrmMain.PCU_ID)
                || ((canID & 0xff) == FrmMain.BDU_ID)))
                return;

            if (model == null)
                model = new RealtimeData();

            model.PackID = FrmMain.BMS_ID.ToString("X2");

            string[] strs;
            switch (canID | 0xff)
            {
                case 0x1003FFFF:
                    initCount++;
                    string batteryStatus = "";
                    //switch (Convert.ToInt32(data[0].ToString("X2"), 16) & 0x0f)
                    //{
                    //    case 0: batteryStatus = LanguageHelper.GetLanguage("State_Standby"); break;
                    //    case 1: batteryStatus = LanguageHelper.GetLanguage("State_Charging"); break;
                    //    case 2: batteryStatus = LanguageHelper.GetLanguage("State_Discharge"); break;
                    //    case 3: batteryStatus = LanguageHelper.GetLanguage("State_Hibernate"); break;
                    //}
                    model.BatteryStatus = allQueue[devID].BatteryStatus = batteryStatus;

                    strs = new string[2] { "0.1", "0.1" };
                    for (int i = 0; i < strs.Length; i++)
                    {
                        strs[i] = BytesToIntger(data[i * 2 + 2], data[i * 2 + 1], Convert.ToDouble(strs[i]));
                    }
                    model.ChargeCurrentLimitation = allQueue[devID].ChargeCurrentLimitation = Convert.ToDouble(strs[0]);
                    model.DischargeCurrentLimitation = allQueue[devID].DischargeCurrentLimitation = Convert.ToDouble(strs[1]);

                    model.ChargeMosEnable = allQueue[devID].ChargeMosEnable = (ushort)GetBit(data[5], 0);
                    model.DischargeMosEnable = allQueue[devID].DischargeMosEnable = (ushort)GetBit(data[5], 1);
                    model.PrechgMosEnable = allQueue[devID].PrechgMosEnable = (ushort)GetBit(data[5], 2);
                    model.StopChgEnable = allQueue[devID].StopChgEnable = (ushort)GetBit(data[5], 3);
                    model.HeatEnable = allQueue[devID].HeatEnable = (ushort)GetBit(data[5], 4);
                    break;
                case 0x1004FFFF:
                    initCount++;
                    strs = new string[4] { "0.1", "0.1", "0.01", "0.1" };
                    for (int i = 0; i < strs.Length; i++)
                    {
                        strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                    }

                    model.BatteryVolt = allQueue[devID].BatteryVolt = Convert.ToDouble(strs[0]);
                    model.LoadVolt = allQueue[devID].LoadVolt = Convert.ToDouble(strs[1]);
                    model.BatteryCurrent = allQueue[devID].BatteryCurrent = Convert.ToDouble(strs[2]);
                    model.SOC = allQueue[devID].SOC = Convert.ToDouble(strs[3]);
                    break;
                case 0x1005FFFF:
                    initCount++;
                    strs = new string[5];
                    strs[0] = BytesToIntger(data[1], data[0]);
                    strs[1] = BytesToIntger(0x00, data[2]);
                    strs[2] = BytesToIntger(data[4], data[3]);
                    strs[3] = BytesToIntger(0x00, data[5]);
                    strs[4] = (Convert.ToInt32(strs[0]) - Convert.ToInt32(strs[2])).ToString();

                    model.BatMaxCellVolt = Convert.ToUInt16(strs[0]);
                    model.BatMaxCellVoltNum = Convert.ToUInt16(strs[1]);
                    model.BatMinCellVolt = Convert.ToUInt16(strs[2]);
                    model.BatMinCellVoltNum = Convert.ToUInt16(strs[3]);
                    model.BatDiffCellVolt = Convert.ToUInt16(strs[4]);
                    break;
                case 0x1006FFFF:
                    initCount++;
                    strs = new string[4] { "0.1", "1", "0.1", "1" };
                    strs[0] = BytesToIntger(data[1], data[0], 0.1);
                    strs[1] = BytesToIntger(0x00, data[2]);
                    strs[2] = BytesToIntger(data[4], data[3], 0.1);
                    strs[3] = BytesToIntger(0x00, data[5]);

                    model.BatMaxCellTemp = Convert.ToDouble(strs[0]);
                    model.BatMaxCellTempNum = Convert.ToUInt16(strs[1]);
                    model.BatMinCellTemp = Convert.ToDouble(strs[2]);
                    model.BatMinCellTempNum = Convert.ToUInt16(strs[3]);
                    break;
                case 0x1007FFFF:
                    initCount++;
                    model.TotalChgCap = Convert.ToDouble((((data[3] << 24) + (data[2] << 16) + (data[1] << 8) + (data[0] & 0xff)) * 0.001).ToString());
                    model.TotalDsgCap = Convert.ToDouble((((data[7] << 24) + (data[6] << 16) + (data[5] << 8) + (data[4] & 0xff)) * 0.001).ToString());
                    break;
                case 0x1008FFFF:
                    initCount++;
                    analysisLog(data);
                    break;
                case 0x1009FFFF:
                    initCount++;
                    strs = new string[4];
                    for (int i = 0; i < strs.Length; i++)
                    {
                        strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2]);
                    }

                    model.CellVoltage1 = Convert.ToUInt32(strs[0]);
                    model.CellVoltage2 = Convert.ToUInt32(strs[1]);
                    model.CellVoltage3 = Convert.ToUInt32(strs[2]);
                    model.CellVoltage4 = Convert.ToUInt32(strs[3]);
                    break;
                case 0x100AFFFF:
                    initCount++;
                    strs = new string[4];
                    for (int i = 0; i < strs.Length; i++)
                    {
                        strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2]);
                    }

                    model.CellVoltage5 = Convert.ToUInt32(strs[0]);
                    model.CellVoltage6 = Convert.ToUInt32(strs[1]);
                    model.CellVoltage7 = Convert.ToUInt32(strs[2]);
                    model.CellVoltage8 = Convert.ToUInt32(strs[3]);
                    break;
                case 0x100BFFFF:
                    initCount++;
                    strs = new string[4];
                    for (int i = 0; i < strs.Length; i++)
                    {
                        strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2]);
                    }

                    model.CellVoltage9 = Convert.ToUInt32(strs[0]);
                    model.CellVoltage10 = Convert.ToUInt32(strs[1]);
                    model.CellVoltage11 = Convert.ToUInt32(strs[2]);
                    model.CellVoltage12 = Convert.ToUInt32(strs[3]);
                    break;
                case 0x100CFFFF:
                    initCount++;
                    strs = new string[4];
                    for (int i = 0; i < strs.Length; i++)
                    {
                        strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2]);
                    }

                    model.CellVoltage13 = Convert.ToUInt32(strs[0]);
                    model.CellVoltage14 = Convert.ToUInt32(strs[1]);
                    model.CellVoltage15 = Convert.ToUInt32(strs[2]);
                    model.CellVoltage16 = Convert.ToUInt32(strs[3]);
                    break;
                case 0x100DFFFF:
                    initCount++;
                    strs = new string[4] { "0.1", "0.1", "0.1", "0.1" };
                    for (int i = 0; i < strs.Length; i++)
                    {
                        strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                    }

                    model.CellTemperature1 = Convert.ToDouble(strs[0]);
                    model.CellTemperature2 = Convert.ToDouble(strs[1]);
                    model.CellTemperature3 = Convert.ToDouble(strs[2]);
                    model.CellTemperature4 = Convert.ToDouble(strs[3]);
                    break;
                case 0x100EFFFF:
                    initCount++;
                    strs = new string[3] { "0.1", "0.1", "0.1" };
                    for (int i = 0; i < strs.Length; i++)
                    {
                        strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                    }

                    model.MosTemperature = Convert.ToDouble(strs[0]);
                    model.EnvTemperature = Convert.ToDouble(strs[1]);
                    model.SOH = Convert.ToDouble(strs[2]);
                    break;
            }
        }

        private string BytesToIntger(byte high, byte low = 0x00, double unit = 1)
        {
            string value = Convert.ToInt16(high.ToString("X2") + low.ToString("X2"), 16).ToString();
            if (unit == 10)
            {
                value = (long.Parse(value) * 10).ToString();
            }
            else if (unit == 0.1)
            {
                value = (long.Parse(value) / 10.0f).ToString();
            }
            else if (unit == 0.01)
            {
                value = (long.Parse(value) / 100.0f).ToString();
            }
            else if (unit == 0.001)
            {
                value = (long.Parse(value) / 1000.0f).ToString();
            }
            else if (unit == 0.0001)
            {
                value = (long.Parse(value) / 10000).ToString();
            }

            return value;
        }
        private int GetBit(byte b, short index)
        {
            byte _byte = 0x01;
            switch (index)
            {
                case 0: { _byte = 0x01; } break;
                case 1: { _byte = 0x02; } break;
                case 2: { _byte = 0x04; } break;
                case 3: { _byte = 0x08; } break;
                case 4: { _byte = 0x10; } break;
                case 5: { _byte = 0x20; } break;
                case 6: { _byte = 0x40; } break;
                case 7: { _byte = 0x80; } break;
                default: { return 0; }
            }
            int x = (b & _byte) == _byte ? 1 : 0;

            return (b & _byte) == _byte ? 1 : 0;
        }

        private void analysisLog(byte[] data)
        {
            string[] msg = new string[2];

            for (int i = 0; i < data.Length; i++)
            {
                for (short j = 0; j < 8; j++)
                {
                    if (GetBit(data[i], j) == 1)
                    {
                        getLog(out msg, i, j);
                        switch (msg[1])
                        {
                            case "1":
                                //richTextBox3.AppendText(msg[0] + "\r\n");
                                //model.Warning = richTextBox3.Text.Replace("\n", "，").Replace("\r", "，");
                                break;
                            case "2":
                                //richTextBox2.AppendText(msg[0] + "\r\n");
                                //model.Protection = richTextBox2.Text.Replace("\n", "，").Replace("\r", "，");
                                break;
                            case "3":
                                //richTextBox1.AppendText(msg[0] + "\r\n");
                                //model.Fault = richTextBox1.Text.Replace("\n", "，").Replace("\r", "，");
                                break;
                        }
                    }
                }
            }
        }
        public static string[] getLog(out string[] msg, int row, int column)
        {
            msg = new string[2];
            List<FaultInfo> faultInfos = FrmMain.FaultInfos;

            for (int i = 0; i < faultInfos.Count; i++)
            {
                if (faultInfos[i].Byte == row && faultInfos[i].Bit == column)
                {
                    int index = LanguageHelper.LanaguageIndex;
                    msg[0] = faultInfos[i].Content.Split(',')[index - 1];
                    msg[1] = faultInfos[i].Type.ToString();
                    break;
                }
            }
            return msg;
        }

        private void Consumer()
        {
            // 等待一段时间，然后停止消费者线程（仅为示例，实际应用中可能有其他停止条件）  
            if (cts == null)
            {
                cts = new CancellationTokenSource();
            }

            foreach (var queue in EcanHelper._queueManager._queues.Values)
            {
                Task.Run(() =>
                {
                    foreach (var item in queue.GetConsumingEnumerable())
                    {
                        if (item.Data == null)
                            continue;

                        CAN_OBJ v = (CAN_OBJ)item.Data;
                        analysisData(v.ID, v.Data);
                        PrintInfo(v);
                    }
                }, cts.Token);
            }
        }

        private void btnStartThread_Click(object sender, EventArgs e)
        {
            if (!flag)
            {
                cts = new CancellationTokenSource();
                btnStartThread.Text = "终止";
                flag = true;
            }
            else
            {
                cts.Cancel();
                btnStartThread.Text = "启动";
                flag = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SQLiteHelper.ConStr = "Data Source=" + Application.StartupPath + "\\DB\\RealtimeDataBase;Pooling=true;FailIfMissing=false";

            allQueue = new Dictionary<uint, RealtimeData>()
            {
                { 1, new RealtimeData() {}},
                { 2, new RealtimeData() {}}
            };
        }
    }
}