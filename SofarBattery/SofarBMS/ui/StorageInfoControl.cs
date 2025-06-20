﻿using Sofar.ConnectionLibs.CAN;
using SofarBMS.Helper;
using SofarBMS.Model;
using System.Data;
using System.Reflection;
using System.Text;

namespace SofarBMS.UI
{
    public partial class StorageInfoControl : UserControl
    {
        // 取消令牌源
        public static CancellationTokenSource cts = null;
        private static CancellationTokenSource _cts = null;

        // ECAN助手实例
        private EcanHelper ecanHelper = EcanHelper.Instance;
        private StorageInfo storageInfo = new StorageInfo();

        DataRow channelDr = null;
        DataTable channelDt = null;
        byte[] bufferCode = new byte[21];
        ManualResetEvent resetEvent = new ManualResetEvent(false);

        byte[] canid = new byte[4] { 0xE0, FrmMain.BMS_ID, 0x2D, 0x10 };
        byte[] bytes = new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };



        public StorageInfoControl()
        {
            InitializeComponent();
        }

        private void StorageInfoControl_Load(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                foreach (Control item in this.Controls)
                {
                    GetControls(item);
                }
                splitContainer1.FixedPanel = FixedPanel.Panel1;
                BindData();
            }));

            cts = new CancellationTokenSource();
            ecanHelper.AnalysisDataInvoked += ServiceBase_AnalysisDataInvoked;
        }

        private void ServiceBase_AnalysisDataInvoked(object? sender, object e)
        {
            if (cts.IsCancellationRequested && ecanHelper.IsConnected)
            {
                ecanHelper.AnalysisDataInvoked -= ServiceBase_AnalysisDataInvoked;
                return;
            }

            var frameModel = e as CanFrameModel;
            if (frameModel != null)
            {
                this.Invoke(() => { AnalysisData(frameModel.CanID, frameModel.Data); });
            }
        }


        private void btn_01_Click(object sender, EventArgs e)
        {
            if (!ecanHelper.IsConnected)
            {
                MessageBox.Show("串口未打开，请先连接设备...");
                return;
            }

            BindData();

            Task.Factory.StartNew(async () =>
            {
                _cts = new CancellationTokenSource();

                //执行步骤一，等待响应后执行步骤二
                bytes[0] = 0x01;
                ecanHelper.Send(bytes, canid);
                resetEvent.WaitOne();

                while (true)
                {
                    if (_cts.IsCancellationRequested)
                        return;

                    //执行步骤二，等待响应AA后停止
                    bytes[0] = 0x02;
                    ecanHelper.Send(bytes, canid);
                    resetEvent.Reset();
                    resetEvent.WaitOne();
                    await Task.Delay(100);
                }
            });
        }

        private void BindData(int type = 0)
        {
            if (type == 1 && channelDt != null)
            {
                return;
            }

            channelDt = new DataTable();
            channelDr = channelDt.NewRow();
            PropertyInfo[] properties = typeof(StorageInfo).GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                channelDt.Columns.Add(propertyInfo.GetValue(storageInfo, null).ToString(), Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
            }
            Invoke((Action)delegate
            {
                dgvStorageInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvStorageInfo.DataSource = channelDt;
            });

            /*//重置表格数据
            channelDt = new DataTable();
            channelDr = channelDt.NewRow();
            channelDt.Columns.Add("发生时间");
            channelDt.Columns.Add("序列号");
            channelDt.Columns.Add("故障byte1");
            channelDt.Columns.Add("故障byte2");
            channelDt.Columns.Add("故障byte3");
            channelDt.Columns.Add("故障byte4");
            channelDt.Columns.Add("故障byte5");
            channelDt.Columns.Add("故障byte6");
            channelDt.Columns.Add("故障byte7");
            channelDt.Columns.Add("故障byte8");
            channelDt.Columns.Add("电池状态");
            channelDt.Columns.Add("充电MOS");
            channelDt.Columns.Add("放电MOS");
            channelDt.Columns.Add("预充MOS");
            channelDt.Columns.Add("充电急停");
            channelDt.Columns.Add("加热MOS");
            channelDt.Columns.Add("电池电压(V)");
            channelDt.Columns.Add("负载电压(V)");
            channelDt.Columns.Add("电池电流(A)");
            channelDt.Columns.Add("电池剩余容量(SOC)");
            channelDt.Columns.Add("电池健康程度(SOH)");
            channelDt.Columns.Add("充电电流上限(A)");
            channelDt.Columns.Add("放电电流上限(A)");
            channelDt.Columns.Add("累计充电容量(Ah)");
            channelDt.Columns.Add("累计放电容量(Ah)");
            channelDt.Columns.Add("最高单体电压(mV)");
            channelDt.Columns.Add("最高单体电压编号");
            channelDt.Columns.Add("最低单体电压(mV)");
            channelDt.Columns.Add("最低单体电压编号");
            channelDt.Columns.Add("电压1(mV)");
            channelDt.Columns.Add("电压2(mV)");
            channelDt.Columns.Add("电压3(mV)");
            channelDt.Columns.Add("电压4(mV)");
            channelDt.Columns.Add("电压5(mV)");
            channelDt.Columns.Add("电压6(mV)");
            channelDt.Columns.Add("电压7(mV)");
            channelDt.Columns.Add("电压8(mV)");
            channelDt.Columns.Add("电压9(mV)");
            channelDt.Columns.Add("电压10(mV)");
            channelDt.Columns.Add("电压11(mV)");
            channelDt.Columns.Add("电压12(mV)");
            channelDt.Columns.Add("电压13(mV)");
            channelDt.Columns.Add("电压14(mV)");
            channelDt.Columns.Add("电压15(mV)");
            channelDt.Columns.Add("电压16(mV)");
            channelDt.Columns.Add("环境温度(℃)");
            channelDt.Columns.Add("Mos温度(℃)");
            channelDt.Columns.Add("最高单体温度(℃)");
            channelDt.Columns.Add("最高单体温度编号");
            channelDt.Columns.Add("最低单体温度(℃)");
            channelDt.Columns.Add("最低单体温度编号");
            channelDt.Columns.Add("温度1(℃)");
            channelDt.Columns.Add("温度2(℃)");
            channelDt.Columns.Add("温度3(℃)");
            channelDt.Columns.Add("温度4(℃)");
            channelDt.Columns.Add("温度5(℃)");
            channelDt.Columns.Add("温度6(℃)");
            channelDt.Columns.Add("温度7(℃)");
            channelDt.Columns.Add("温度8(℃)");
            channelDt.Columns.Add("剩余容量(Ah)");
            channelDt.Columns.Add("满充容量(Ah)");
            channelDt.Columns.Add("均衡状态");
            channelDt.Columns.Add("均衡温度1(℃)");
            channelDt.Columns.Add("均衡温度2(℃)");
            this.Invoke(new Action(() =>
            {
                dgvStorageInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvStorageInfo.DataSource = channelDt;
            }));*/
        }

        private void btn_02_Click(object sender, EventArgs e)
        {
            if (!ecanHelper.IsConnected)
            {
                MessageBox.Show("串口未打开，请先连接设备...");
                return;
            }

            BindData(1);

            Task.Factory.StartNew(async () =>
            {
                _cts = new CancellationTokenSource();

                while (true)
                {
                    if (_cts.IsCancellationRequested)
                        return;

                    bytes[0] = 0x02;
                    ecanHelper.Send(bytes, canid);
                    resetEvent.Reset();
                    resetEvent.WaitOne();

                    await Task.Delay(200);
                }
            });
        }

        private void btn_AA_Click(object sender, EventArgs e)
        {
            if (!ecanHelper.IsConnected)
            {
                MessageBox.Show("串口未打开，请先连接设备...");
                return;
            }

            if (MessageBox.Show("是否清空存储的历史记录？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bytes[0] = 0xAA;
                ecanHelper.Send(bytes, canid);
            }
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            if (!ecanHelper.IsConnected)
            {
                MessageBox.Show("串口未打开，请先连接设备...");
                return;
            }

            resetEvent.Reset(); //变更为线程暂停运行
            if (_cts != null)
                _cts.Cancel();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataGridViewToExcel(dgvStorageInfo);
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            e.Row.HeaderCell.Value = string.Format("{0}", e.Row.Index + 1);
        }

        private void AnalysisData(uint canID, byte[] data)
        {
            byte[] canid = BitConverter.GetBytes(canID);

            switch (canid[2])
            {
                case 0x2D:
                    if (channelDr[storageInfo.SerialNumber].ToString() != "")
                    {
                        this.Invoke(new Action(() =>
                        {
                            channelDt.Rows.Add(channelDr);
                            dgvStorageInfo.DataSource = channelDt;

                            //定义新行对象
                            channelDr = channelDt.NewRow();
                        }));
                    }

                    if (data[0] == 0xAA)
                    {
                        resetEvent.Reset();

                        if (_cts != null)
                        {
                            _cts.Cancel();
                            //_cts.Dispose();
                        }
                    }
                    else
                    {
                        resetEvent.Set();
                    }
                    break;
                case 0x30:
                    StringBuilder date = new StringBuilder();
                    int[] numbers_bit = BytesToBit(data);
                    date.Append(numbers_bit[0] + 2000);
                    date.Append("-");
                    date.Append(numbers_bit[1]);
                    date.Append("-");
                    date.Append(numbers_bit[2]);
                    date.Append(" ");
                    date.Append(numbers_bit[3]);
                    date.Append(":");
                    date.Append(numbers_bit[4]);
                    date.Append(":");
                    date.Append(numbers_bit[5]);
                    channelDr[storageInfo.OccurredDate] = date;
                    break;
                case 0x31:
                    switch (data[0])
                    {
                        case 0x00: Buffer.BlockCopy(data, 1, bufferCode, 0, 7); break;
                        case 0x01: Buffer.BlockCopy(data, 1, bufferCode, 7, 7); break;
                        case 0x02:
                            Buffer.BlockCopy(data, 1, bufferCode, 14, 7);
                            channelDr[storageInfo.SerialNumber] = System.Text.Encoding.ASCII.GetString(bufferCode).Trim().Replace("\0", "");
                            break;
                        default:
                            bufferCode = new byte[21];
                            break;
                    }
                    break;
                case 0x33:
                    channelDr[storageInfo.BatteryStatus] = Convert.ToInt32(data[0].ToString("X2"), 16);
                    channelDr[storageInfo.ChargeCurrentLimit] = (Convert.ToInt32(data[2].ToString("X2") + data[1].ToString("X2"), 16) * 0.1).ToString();
                    channelDr[storageInfo.DischargeCurrentLimit] = (Convert.ToInt32(data[4].ToString("X2") + data[3].ToString("X2"), 16) * 0.1).ToString();
                    channelDr[storageInfo.ChargingMOS] = (GetBit(data[5], 0) == 0 ? LanguageHelper.GetString("off") : LanguageHelper.GetString("on"));
                    channelDr[storageInfo.DischargeMOS] = (GetBit(data[5], 1) == 0 ? LanguageHelper.GetString("off") : LanguageHelper.GetString("on"));
                    channelDr[storageInfo.PreChargedMOS] = (GetBit(data[5], 2) == 0 ? LanguageHelper.GetString("off") : LanguageHelper.GetString("on"));
                    channelDr[storageInfo.ChargingEmergencyStop] = (GetBit(data[5], 3) == 0 ? LanguageHelper.GetString("off") : LanguageHelper.GetString("on"));
                    channelDr[storageInfo.HeatedMOS] = (GetBit(data[5], 4) == 0 ? LanguageHelper.GetString("off") : LanguageHelper.GetString("on"));
                    break;
                case 0x34:
                    channelDr[storageInfo.BatteryVoltage] = (Convert.ToInt32(data[1].ToString("X2") + data[0].ToString("X2"), 16) * 0.1).ToString();
                    channelDr[storageInfo.LoadVoltage] = (Convert.ToInt32(data[3].ToString("X2") + data[2].ToString("X2"), 16) * 0.1).ToString();
                    channelDr[storageInfo.BatteryCurrent] = (Convert.ToInt16(data[5].ToString("x2") + data[4].ToString("x2"), 16) * 0.01).ToString();
                    channelDr[storageInfo.SOC] = (Convert.ToInt32(data[7].ToString("X2") + data[6].ToString("X2"), 16) * 0.1).ToString();
                    break;
                case 0x35:
                    channelDr[storageInfo.Maximumcellvoltage] = (Convert.ToInt32(data[1].ToString("X2") + data[0].ToString("X2"), 16) * 0.001).ToString();
                    channelDr[storageInfo.Maximumcellvoltagenumber] = Convert.ToInt32(data[2].ToString("X2"), 16).ToString();
                    channelDr[storageInfo.Minimumcellvoltage] = (Convert.ToInt32(data[4].ToString("X2") + data[3].ToString("X2"), 16) * 0.001).ToString();
                    channelDr[storageInfo.Minimumcellvoltagenumber] = Convert.ToInt32(data[5].ToString("X2"), 16).ToString();
                    break;
                case 0x36:
                    channelDr[storageInfo.Maximumcelltemperature] = (Convert.ToInt32(data[1].ToString("X2") + data[0].ToString("X2"), 16) * 0.1).ToString();
                    channelDr[storageInfo.Maximumcelltemperaturenumber] = Convert.ToInt32(data[2].ToString("X2"), 16).ToString();
                    channelDr[storageInfo.Minimumcelltemperature] = (Convert.ToInt32(data[4].ToString("X2") + data[3].ToString("X2"), 16) * 0.1).ToString();
                    channelDr[storageInfo.Minimumcelltemperaturenumber] = Convert.ToInt32(data[5].ToString("X2"), 16).ToString();
                    break;
                case 0x37:
                    channelDr[storageInfo.CumulativeChargingCapacity] = (((data[3] << 24) + (data[2] << 16) + (data[1] << 8) + (data[0] & 0xff)) * 0.001).ToString();
                    channelDr[storageInfo.Cumulativedischargecapacity] = (((data[7] << 24) + (data[6] << 16) + (data[5] << 8) + (data[4] & 0xff)) * 0.001).ToString();
                    break;
                case 0x38:
                    channelDr[storageInfo.FaultByte1] = Convert.ToInt32(data[0].ToString("X2"), 16);
                    channelDr[storageInfo.FaultByte2] = Convert.ToInt32(data[1].ToString("X2"), 16);
                    channelDr[storageInfo.FaultByte3] = Convert.ToInt32(data[2].ToString("X2"), 16);
                    channelDr[storageInfo.FaultByte4] = Convert.ToInt32(data[3].ToString("X2"), 16);
                    channelDr[storageInfo.FaultByte5] = Convert.ToInt32(data[4].ToString("X2"), 16);
                    channelDr[storageInfo.FaultByte6] = Convert.ToInt32(data[5].ToString("X2"), 16);
                    channelDr[storageInfo.FaultByte7] = Convert.ToInt32(data[6].ToString("X2"), 16);
                    channelDr[storageInfo.FaultByte8] = Convert.ToInt32(data[7].ToString("X2"), 16);
                    break;
                case 0x39:
                    channelDr[storageInfo.Voltage1] = (Convert.ToInt32(data[1].ToString("X2") + data[0].ToString("X2"), 16) * 0.001).ToString();
                    channelDr[storageInfo.Voltage2] = (Convert.ToInt32(data[3].ToString("X2") + data[2].ToString("X2"), 16) * 0.001).ToString();
                    channelDr[storageInfo.Voltage3] = (Convert.ToInt32(data[5].ToString("X2") + data[4].ToString("X2"), 16) * 0.001).ToString();
                    channelDr[storageInfo.Voltage4] = (Convert.ToInt32(data[7].ToString("X2") + data[6].ToString("X2"), 16) * 0.001).ToString();
                    break;
                case 0x3A:
                    channelDr[storageInfo.Voltage5] = (Convert.ToInt32(data[1].ToString("X2") + data[0].ToString("X2"), 16) * 0.001).ToString();
                    channelDr[storageInfo.Voltage6] = (Convert.ToInt32(data[3].ToString("X2") + data[2].ToString("X2"), 16) * 0.001).ToString();
                    channelDr[storageInfo.Voltage7] = (Convert.ToInt32(data[5].ToString("X2") + data[4].ToString("X2"), 16) * 0.001).ToString();
                    channelDr[storageInfo.Voltage8] = (Convert.ToInt32(data[7].ToString("X2") + data[6].ToString("X2"), 16) * 0.001).ToString();
                    break;
                case 0x3B:
                    channelDr[storageInfo.Voltage9] = (Convert.ToInt32(data[1].ToString("X2") + data[0].ToString("X2"), 16) * 0.001).ToString();
                    channelDr[storageInfo.Voltage10] = (Convert.ToInt32(data[3].ToString("X2") + data[2].ToString("X2"), 16) * 0.001).ToString();
                    channelDr[storageInfo.Voltage11] = (Convert.ToInt32(data[5].ToString("X2") + data[4].ToString("X2"), 16) * 0.001).ToString();
                    channelDr[storageInfo.Voltage12] = (Convert.ToInt32(data[7].ToString("X2") + data[6].ToString("X2"), 16) * 0.001).ToString();
                    break;
                case 0x3C:
                    channelDr[storageInfo.Voltage13] = (Convert.ToInt32(data[1].ToString("X2") + data[0].ToString("X2"), 16) * 0.001).ToString();
                    channelDr[storageInfo.Voltage14] = (Convert.ToInt32(data[3].ToString("X2") + data[2].ToString("X2"), 16) * 0.001).ToString();
                    channelDr[storageInfo.Voltage15] = (Convert.ToInt32(data[5].ToString("X2") + data[4].ToString("X2"), 16) * 0.001).ToString();
                    channelDr[storageInfo.Voltage16] = (Convert.ToInt32(data[7].ToString("X2") + data[6].ToString("X2"), 16) * 0.001).ToString();
                    break;
                case 0x3D:
                    channelDr[storageInfo.Temperature1] = (Convert.ToInt32(data[1].ToString("X2") + data[0].ToString("X2"), 16) * 0.1).ToString();
                    channelDr[storageInfo.Temperature2] = (Convert.ToInt32(data[3].ToString("X2") + data[2].ToString("X2"), 16) * 0.1).ToString();
                    channelDr[storageInfo.Temperature3] = (Convert.ToInt32(data[5].ToString("X2") + data[4].ToString("X2"), 16) * 0.1).ToString();
                    channelDr[storageInfo.Temperature4] = (Convert.ToInt32(data[7].ToString("X2") + data[6].ToString("X2"), 16) * 0.1).ToString();
                    break;
                case 0x3E:
                    channelDr[storageInfo.MosTemperature] = (Convert.ToInt32(data[1].ToString("X2") + data[0].ToString("X2"), 16) * 0.1).ToString();
                    channelDr[storageInfo.AmbientTemperature] = (Convert.ToInt32(data[3].ToString("X2") + data[2].ToString("X2"), 16) * 0.1).ToString();
                    channelDr[storageInfo.SOH] = (Convert.ToInt32(data[5].ToString("X2") + data[4].ToString("X2"), 16) * 0.1).ToString();
                    channelDr[storageInfo.EquilibriumState] = $"[1~16]:{Convert.ToString(data[6], 2).PadLeft(8, '0').Insert(4, " ")},{Convert.ToString(data[7], 2).PadLeft(8, '0').Insert(4, " ")}";
                    break;
                case 0x3F:
                    channelDr[storageInfo.RemainingCapacity] = (Convert.ToInt32(data[1].ToString("X2") + data[0].ToString("X2"), 16) * 0.1).ToString();
                    channelDr[storageInfo.FullChargeCapacity] = (Convert.ToInt32(data[3].ToString("X2") + data[2].ToString("X2"), 16) * 0.1).ToString();
                    break;
                case 0x50:
                    channelDr[storageInfo.Temperature5] = (Convert.ToInt32(data[1].ToString("X2") + data[0].ToString("X2"), 16) * 0.1).ToString();
                    channelDr[storageInfo.Temperature6] = (Convert.ToInt32(data[3].ToString("X2") + data[2].ToString("X2"), 16) * 0.1).ToString();
                    channelDr[storageInfo.Temperature7] = (Convert.ToInt32(data[5].ToString("X2") + data[4].ToString("X2"), 16) * 0.1).ToString();
                    channelDr[storageInfo.Temperature8] = (Convert.ToInt32(data[7].ToString("X2") + data[6].ToString("X2"), 16) * 0.1).ToString();
                    break;
                case 0x51:
                    channelDr[storageInfo.EquilibriumTemperature1] = (Convert.ToInt32(data[1].ToString("X2") + data[0].ToString("X2"), 16) * 0.1).ToString();
                    channelDr[storageInfo.EquilibriumTemperature2] = (Convert.ToInt32(data[3].ToString("X2") + data[2].ToString("X2"), 16) * 0.1).ToString();
                    break;
            }
        }

        private static int GetBit(byte b, short index)
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

        private int[] BytesToBit(byte[] data)
        {
            int[] numbers = new int[8];
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = Convert.ToInt32(data[i].ToString("X2"), 16);
            }
            return numbers;
        }

        public void DataGridViewToExcel(DataGridView dgv)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Execl files (*.csv)|*.csv";
            dlg.FilterIndex = 0;
            dlg.RestoreDirectory = true;
            dlg.CreatePrompt = true;
            dlg.Title = "保存为csv文件";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Stream myStream;
                myStream = dlg.OpenFile();
                StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));
                string columnTitle = "";
                try
                {
                    //写入列标题    
                    for (int i = 0; i < dgv.ColumnCount; i++)
                    {
                        if (i > 0)
                        {
                            columnTitle += ",";
                        }
                        columnTitle += dgv.Columns[i].HeaderText;//符号 ，的添加，在保存为Excel时就以 ，分成不同的列了
                    }

                    sw.WriteLine(columnTitle);//将内容写入文件流中

                    //写入列内容    
                    for (int j = 0; j < dgv.Rows.Count; j++)
                    {
                        string columnValue = "";
                        for (int k = 0; k < dgv.Columns.Count; k++)
                        {
                            if (k > 0)
                            {
                                columnValue += ",";
                            }
                            if (dgv.Rows[j].Cells[k].Value == null)
                                columnValue += "";
                            else if (dgv.Rows[j].Cells[k].Value.ToString().Contains(","))
                            {
                                columnValue += "\"" + dgv.Rows[j].Cells[k].Value.ToString().Trim() + "\"";//将单元格中的，号转义成文本
                            }
                            else
                            {
                                columnValue += dgv.Rows[j].Cells[k].Value.ToString().Trim() + "\t";//\t 横向跳格
                            }
                        }
                        sw.WriteLine(columnValue);
                    }
                    sw.Close();//关闭写入流
                    myStream.Close();//关闭流变量
                    MessageBox.Show("导出表格成功！");
                }
                catch (Exception)
                {
                    MessageBox.Show("导出表格失败！");
                }
                finally
                {
                    sw.Close();
                    myStream.Close();
                }
            }
            else
            {
                MessageBox.Show("取消导出表格操作!");
            }
        }


        #region 翻译函数
        private void GetControls(Control c)
        {
            if (c is GroupBox || c is TabControl)
            {
                c.Text = LanguageHelper.GetLanguage(c.Name.Remove(0, 2));

                foreach (Control item in c.Controls)
                {
                    this.GetControls(item);
                }
            }
            else
            {
                string name = c.Name;

                if (c is CheckBox)
                {
                    c.Text = LanguageHelper.GetLanguage(name.Remove(0, 2));

                    LTooltip(c as CheckBox, c.Text);
                }
                else if (c is RadioButton)
                {
                    c.Text = LanguageHelper.GetLanguage(name.Remove(0, 2));

                    LTooltip(c as RadioButton, c.Text);
                }
                else if (c is Label)
                {
                    c.Text = LanguageHelper.GetLanguage(name.Remove(0, 3));

                    LTooltip(c as Label, c.Text);
                }
                else if (c is Button)
                {
                    if (name.Contains("Set"))
                    {
                        c.Text = LanguageHelper.GetLanguage("Settings");
                    }
                    else if (name.Contains("_Close"))
                    {
                        c.Text = LanguageHelper.GetLanguage("Systemset_43");
                    }
                    else if (name.Contains("_Open"))
                    {
                        c.Text = LanguageHelper.GetLanguage("Systemset_44");
                    }
                    else if (name.Contains("_Lifted"))
                    {
                        c.Text = LanguageHelper.GetLanguage("Systemset_45");
                    }
                    else
                    {
                        c.Text = LanguageHelper.GetLanguage(name.Remove(0, 3));
                    }
                }
                else if (c is TabPage | c is Panel | c is SplitContainer)
                {
                    foreach (Control item in c.Controls)
                    {
                        this.GetControls(item);
                    }
                }
            }
        }

        public static void LTooltip(System.Windows.Forms.Control control, string value)
        {
            if (value.Length == 0) return;
            control.Text = Abbreviation(control, value);
            var tip = new ToolTip();
            tip.IsBalloon = false;
            tip.ShowAlways = true;
            tip.SetToolTip(control, value);
        }

        public static string Abbreviation(Control control, string str)
        {
            if (str == null)
            {
                return null;
            }
            int strWidth = FontWidth(control.Font, control, str);

            //获取label最长可以显示多少字符
            int len = control.Width * str.Length / strWidth;

            if (len > 20 && len < str.Length)
            {
                return str.Substring(0, 20) + "...";
            }
            else
            {
                return str;
            }
        }

        private static int FontWidth(Font font, Control control, string str)
        {
            using (Graphics g = control.CreateGraphics())
            {
                SizeF siF = g.MeasureString(str, font);
                return (int)siF.Width;
            }
        }
        #endregion
    }
}
