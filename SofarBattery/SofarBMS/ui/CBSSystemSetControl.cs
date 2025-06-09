using Sofar.BMS;
using Sofar.ConnectionLibs.CAN;
using SofarBMS.Helper;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace SofarBMS.UI
{
    public partial class CBSSystemSetControl : UserControl
    {
        public CBSSystemSetControl()
        {
            InitializeComponent();
        }

        // 取消令牌源
        public static CancellationTokenSource cts = null;

        // ECAN助手实例
        private EcanHelper ecanHelper = EcanHelper.Instance;

        public BMURealDataVM RealDataVM = new BMURealDataVM();

        string[] boardCode = new string[3];
        string[] bmsCode = new string[3];
        string[] bcuCode = new string[3];
        string[] pcuCode = new string[3];

        short[] bitResult { get; set; }
        short[] bitResult1 { get; set; }
        string strResult { get; set; }

        byte[] canid = new byte[] { 0xE0, FrmMain.BMS_ID, 0x00, 0x10 };

        byte[] bytes = new byte[8];

        bool flag = true;

        #region 读取数据
        private void SystemSetControl_Load(object sender, EventArgs e)
        {
            this.Invoke(() =>
            {
                foreach (Control item in this.Controls)
                {
                    GetControls(item);
                }

                foreach (Control item in this.gbControl0F0.Controls)
                {
                    if (item is ComboBox)
                    {
                        ComboBox cbb = item as ComboBox;
                        cbb.SelectedIndex = 0;
                    }
                }
            });

            cts = new CancellationTokenSource();
            Task.Run(async delegate
            {
                while (!cts.IsCancellationRequested)
                {
                    if (ecanHelper.IsConnected)
                    {
                        if (flag)
                        {
                            List<uint> DataLists = new List<uint>() { 0xAA11, 0xAA22, 0xAA33, 0xAA44, 0xAA55, 0xAA66, 0xAA77, 0xAA88 };

                            for (int i = 0; i < DataLists.Count; i++)
                            {
                                DataSelected(DataLists[i]);
                                await Task.Delay(100);
                            }

                            //首次进入读取一遍
                            byte[] bytes = new byte[8] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                            ecanHelper.Send(bytes, new byte[] { 0xE0, FrmMain.BMS_ID, 0x2E, 0x10 });
                            ecanHelper.Send(bytes, new byte[] { 0xE0, FrmMain.BMS_ID, 0x2C, 0x10 });

                            //BCU                          
                            ecanHelper.Send(bytes, new byte[] { 0xE0, FrmMain.BCU_ID, 0xF9, 0x10 });
                            //BMU
                            ecanHelper.Send(bytes, new byte[] { 0xE0, FrmMain.BMS_ID, 0x2E, 0x10 });

                            flag = false;
                            await Task.Delay(1000);
                        }
                    }
                }
            }, cts.Token);

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

        private void btnSystemset_46_Click(object sender, EventArgs e)
        {
            //CBS5000
            List<uint> DataLists = new List<uint>() { 0xAA11, 0xAA22, 0xAA33, 0xAA44, 0xAA55, 0xAA66, 0xAA77, 0xAA88 };

            for (int i = 0; i < DataLists.Count; i++)
            {
                DataSelected(DataLists[i]);

                Thread.Sleep(100);
            }

            byte[] bytes1 = new byte[8] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            // 0xF9||0x02E:一键操作标定参数0x102EXXE0
            if (!ecanHelper.Send(bytes1, new byte[] { 0xE0, FrmMain.BCU_ID, 0xF9, 0x10 }))
            {
                MessageBox.Show(FrmMain.GetString("keyReadFail"));
            }
            if (!ecanHelper.Send(bytes1, new byte[] { 0xE0, FrmMain.BMS_ID, 0x2E, 0x10 }))
            {
                MessageBox.Show(FrmMain.GetString("keyReadFail"));
            }
        }

        private void DataSelected(uint type)
        {
            byte[] id = new byte[4] { 0xE0, FrmMain.PCU_ID, 0x77, 0x0B };

            byte[] data = new byte[8];
            data[0] = (byte)(type & 0xff);
            data[1] = (byte)(type >> 8);

            ecanHelper.Send(data, id);
        }

        public void AnalysisData(uint canID, byte[] data)
        {
            byte[] canid = BitConverter.GetBytes(canID);

            if (!(((canID & 0xff) == FrmMain.BMS_ID) || ((canID & 0xff) == FrmMain.PCU_ID) || ((canID & 0xff) == FrmMain.BCU_ID)))
                return;

            // 数据接收打印
            Debug.WriteLine($"数据接收打印:{canID.ToString("X8")}");

            string[] strs;
            string[] controls;

            int[] numbers = BytesToUint16(data);
            int[] numbers_bit = BytesToBit(data);

            switch (BitConverter.ToUInt32(canid, 0) | 0xff)
            {
                case 0x101FE0FF:
                    if (data[0] == 0x00)
                    {
                        txtFlashData_BMU.Text = Encoding.Default.GetString(data).Substring(1);
                    }
                    else
                    {
                        txtFlashData_BMU.Text = "";
                    }
                    break;
                case 0x1020E0FF:
                    foreach (Control item in this.groupBox1.Controls)
                    {
                        if (item is ComboBox)
                        {
                            ComboBox cbb = item as ComboBox;
                            int index_BMU = 0;
                            int.TryParse(cbb.Name.Replace("cbbRequest20_", ""), out index_BMU);

                            int value = -1;
                            switch (data[index_BMU])
                            {
                                case 0x00:
                                    value = 0;
                                    break;
                                case 0xAA:
                                    value = 1;
                                    break;
                                case 0x55:
                                    value = 2;
                                    break;
                                default:
                                    break;
                            }
                            cbb.SelectedIndex = value;
                        }
                    }
                    int flag = data[6];
                    ckSystemset_BMU_60.Checked = (flag & 0x1) == 1;
                    ckSystemset_BMU_61.Checked = (flag & 0x2) == 1;
                    ckSystemset_BMU_62.Checked = (flag & 0x4) == 1;
                    ckSystemset_BMU_63.Checked = (flag & 0x8) == 1;
                    break;
                case 0x1021E0FF:
                    txtBalOpenVolt.Text = numbers[0].ToString();
                    txtBalOpenVoltDiff.Text = numbers[1].ToString();
                    txtFullChgVolt.Text = numbers[2].ToString();
                    txtHeatFilmOpenTemp.Text = $"{(short)data[6]}";
                    txtHeatFilmCloseTemp.Text = $"{(short)data[7]}";
                    break;

                case 0x1022E0FF:
                    txtPackStopVolt.Text = (Convert.ToInt32(data[1].ToString("X2") + data[0].ToString("X2"), 16) * 0.1).ToString();
                    txtPackStopCurrent.Text = (Convert.ToInt32(data[3].ToString("X2") + data[2].ToString("X2"), 16) * 0.01).ToString();
                    break;

                case 0x1023E0FF:
                    txtRetedCapacity.Text = (numbers[0] * 0.1).ToString();
                    txtCellVoltNum.Text = numbers[1].ToString();
                    txtCellTempNum.Text = numbers[2].ToString();
                    break;

                case 0x1024E0FF:
                    txtCumulativeChgCapacity.Text = ((data[3] << 24) + (data[2] << 16) + (data[1] << 8) + (data[0] & 0xff)).ToString();
                    txtCumulativeDsgCapactiy.Text = ((data[7] << 24) + (data[6] << 16) + (data[5] << 8) + (data[4] & 0xff)).ToString();

                    break;

                case 0x1025E0FF:
                    txtSOC.Text = (Convert.ToInt32(data[1].ToString("X2") + data[0].ToString("X2"), 16) * 0.1).ToString();
                    txtFullChgCapacity.Text = (Convert.ToInt32(data[3].ToString("X2") + data[2].ToString("X2"), 16) * 0.1).ToString();
                    txtSurplusCapacity.Text = (Convert.ToInt32(data[5].ToString("X2") + data[4].ToString("X2"), 16) * 0.1).ToString();
                    txtSOH.Text = (Convert.ToInt32(data[7].ToString("X2") + data[6].ToString("X2"), 16) * 0.1).ToString();

                    break;

                case 0x1026E0FF:
                    StringBuilder date = new StringBuilder();
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
                    dateTimePicker1.Text = date.ToString();
                    break;

                case 0x1027E0FF:
                    switch (data[0])
                    {
                        case 0:
                            bmsCode[0] = Encoding.Default.GetString(data).Substring(1);
                            break;
                        case 1:
                            bmsCode[1] = Encoding.Default.GetString(data).Substring(1);
                            break;
                        case 2:
                            bmsCode[2] = Encoding.Default.GetString(data).Substring(1);
                            txtPackSN_BMU.Text = String.Join("", bmsCode).Trim();
                            bmsCode = new string[3];
                            break;
                        default:
                            File.AppendAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/data.log", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + data[0].ToString() + Environment.NewLine);
                            break;
                    }
                    break;

                case 0x1028E0FF:
                    switch (data[0])
                    {
                        case 0:
                            boardCode[0] = Encoding.Default.GetString(data).Substring(1);
                            break;
                        case 1:
                            boardCode[1] = Encoding.Default.GetString(data).Substring(1);
                            break;
                        case 2:
                            boardCode[2] = Encoding.Default.GetString(data).Substring(1);
                            txtBoardSN_BMU.Text = String.Join("", boardCode).Trim();
                            boardCode = new string[3];
                            break;
                        default:
                            File.AppendAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/data.log", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + data[0].ToString() + Environment.NewLine);
                            break;
                    }
                    break;

                case 0x1029E0FF:
                    cbb_103.SelectedIndex = numbers_bit[0];

                    txt_104.Text = numbers_bit[1].ToString();
                    txtFlag.Text = numbers[1].ToString();
                    break;

                case 0x102BE0FF:
                    Dictionary<short, Button[]> buttons = new Dictionary<short, Button[]>();
                    buttons.Add(0, new Button[] { btnSystemset_45_BMU_Lifted1, btnSystemset_43_BMU_Close1, btnSystemset_44_BMU_Open1 });
                    buttons.Add(1, new Button[] { btnSystemset_45_BMU_Lifted2, btnSystemset_43_BMU_Close2, btnSystemset_44_BMU_Open2 });
                    buttons.Add(2, new Button[] { btnSystemset_45_BMU_Lifted3, btnSystemset_43_BMU_Close3, btnSystemset_44_BMU_Open3 });
                    buttons.Add(3, new Button[] { btnSystemset_45_BMU_Lifted4, btnSystemset_43_BMU_Close4, btnSystemset_44_BMU_Open4 });
                    buttons.Add(4, new Button[] { btnSystemset_45_BMU_Lifted5, btnSystemset_43_BMU_Close5, btnSystemset_44_BMU_Open5 });
                    buttons.Add(5, new Button[] { btnSystemset_45_BMU_Lifted6, btnSystemset_43_BMU_Close6, btnSystemset_44_BMU_Open6 });
                    buttons.Add(6, new Button[] { btnSystemset_45_BMU_Lifted7, btnSystemset_43_BMU_Close7, btnSystemset_44_BMU_Open7 });
                    buttons.Add(7, new Button[] { btnSystemset_45_BMU_Lifted8, btnSystemset_43_BMU_Close8, btnSystemset_44_BMU_Open8 });
                    buttons.Add(8, new Button[] { btnSystemset_45_BMU_Lifted9, btnSystemset_43_BMU_Close9, btnSystemset_44_BMU_Open9 });

                    //byte[]转为二进制字符串
                    strResult = string.Empty;
                    for (int i = 0; i <= 2; i++)
                    {
                        string strTemp = Convert.ToString(data[i], 2);
                        strTemp = strTemp.Insert(0, new string('0', 8 - strTemp.Length));
                        strResult = strTemp + strResult;
                    }

                    //二进制字符串转化为short[]
                    bitResult = new short[strResult.Length / 2];
                    int index = bitResult.Length - 1;
                    for (int i = 0; i < bitResult.Length; i++)
                    {
                        bitResult[index--] = (short)Convert.ToByte(Convert.ToInt32(strResult.Substring(i * 2, 2)) & 0x03);
                    }

                    //根据short得值来找到对应得button
                    foreach (var item in buttons.Keys)
                    {
                        Button btn = buttons[item][bitResult[item]];
                        string name = btn.Name;
                        btn.Enabled = false;
                    }
                    break;

                case 0x102EE0FF:
                    //0x02E:一键读取标定参数回复0x102EE0XX
                    break;

                case 0x102FE0FF:
                    //ATE强制控制指令
                    break;

                case 0x10F0E0FF:
                    //foreach (Control item in this.gbControl0F0.Controls)
                    //{
                    //    if (item is ComboBox)
                    //    {
                    //        ComboBox cbb = item as ComboBox;
                    //        int index_ = 0;
                    //        int.TryParse(cbb.Name.Replace("cbbRequestF0_", ""), out index);

                    //        int value = -1;
                    //        switch (data[index])
                    //        {
                    //            case 0x00:
                    //                value = 0;
                    //                break;
                    //            case 0xAA:
                    //                value = 1;
                    //                break;
                    //            case 0x55:
                    //                value = 2;
                    //                break;
                    //            default:
                    //                break;
                    //        }
                    //        cbb.SelectedIndex = value;
                    //    }
                    //}
                    break;
            }

            switch (canid[2])
            {
                //BCU Flash数据
                case 0xEF:
                    if (data[0] == 0x00)
                    {
                        txtFlashData.Text = Encoding.Default.GetString(data).Substring(1);
                    }
                    else
                    {
                        txtFlashData.Text = "";
                    }
                    break;

                //BCU时间
                case 0xF2:
                    StringBuilder date1 = new StringBuilder();
                    date1.Append(numbers_bit[0] + 2000);
                    date1.Append("-");
                    date1.Append(numbers_bit[1]);
                    date1.Append("-");
                    date1.Append(numbers_bit[2]);
                    date1.Append(" ");
                    date1.Append(numbers_bit[3]);
                    date1.Append(":");
                    date1.Append(numbers_bit[4]);
                    date1.Append(":");
                    date1.Append(numbers_bit[5]);
                    dateTimePicker2.Text = date1.ToString();
                    break;

                // BCU PACK_SN
                case 0xF3:
                    switch (data[0])
                    {
                        case 0:
                            bcuCode[0] = Encoding.Default.GetString(data).Substring(1);
                            break;
                        case 1:
                            bcuCode[1] = Encoding.Default.GetString(data).Substring(1);
                            break;
                        case 2:
                            bcuCode[2] = Encoding.Default.GetString(data).Substring(1);
                            txtbcuCode.Text = String.Join("", bcuCode).Trim();
                            bcuCode = new string[3];
                            break;
                        default:
                            File.AppendAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/data.log", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + data[0].ToString() + Environment.NewLine);
                            break;
                    }
                    break;

                //BCU Board_SN
                case 0xF4:
                    switch (data[0])
                    {
                        case 0:
                            boardCode[0] = Encoding.Default.GetString(data).Substring(1);
                            break;
                        case 1:
                            boardCode[1] = Encoding.Default.GetString(data).Substring(1);
                            break;
                        case 2:
                            boardCode[2] = Encoding.Default.GetString(data).Substring(1);
                            txtBCUBoardSN.Text = String.Join("", boardCode).Trim();
                            boardCode = new string[3];
                            break;
                        default:
                            File.AppendAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/data.log", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + data[0].ToString() + Environment.NewLine);
                            break;
                    }
                    break;
                case 0xF6:
                    Dictionary<short, Button[]> buttons_bcu1 = new Dictionary<short, Button[]>();
                    buttons_bcu1.Add(0, new Button[] { btnSystemset_45_BCU1_Lifted1, btnSystemset_43_BCU1_Close1, btnSystemset_44_BCU1_Open1 });
                    buttons_bcu1.Add(1, new Button[] { btnSystemset_45_BCU1_Lifted2, btnSystemset_43_BCU1_Close2, btnSystemset_44_BCU1_Open2 });

                    //byte[]转为二进制字符串
                    strResult = string.Empty;
                    for (int i = 0; i < 1; i++)
                    {
                        string strTemp = Convert.ToString(data[i], 2);
                        strTemp = strTemp.Insert(0, new string('0', 8 - strTemp.Length));
                        strResult = strTemp + strResult;
                    }

                    //二进制字符串转化为short[]
                    bitResult1 = new short[strResult.Length / 2];
                    int index4 = bitResult1.Length - 1;
                    for (int i = 0; i < bitResult1.Length; i++)
                    {
                        bitResult1[index4--] = (short)Convert.ToByte(Convert.ToInt32(strResult.Substring(i * 2, 2)) & 0x03);
                    }

                    //根据short得值来找到对应得button
                    foreach (var item in buttons_bcu1.Keys)
                    {
                        //解除->>将3该为0
                        for (int i = 0; i < bitResult1.Length; i++)
                        {
                            if (bitResult1[i] == 3)
                            {
                                bitResult1[i] = 0;
                            }
                        }
                        Button btn = buttons_bcu1[item][bitResult1[item]];
                        string name = btn.Name;
                        btn.Enabled = false;
                    }
                    break;

                case 0xFA:
                    Dictionary<short, Button[]> buttons_bcu = new Dictionary<short, Button[]>();
                    buttons_bcu.Add(0, new Button[] { btnSystemset_45_BCU_Lifted1, btnSystemset_43_BCU_Close1, btnSystemset_44_BCU_Open1 });
                    buttons_bcu.Add(1, new Button[] { btnSystemset_45_BCU_Lifted2, btnSystemset_43_BCU_Close2, btnSystemset_44_BCU_Open2 });
                    buttons_bcu.Add(2, new Button[] { btnSystemset_45_BCU_Lifted3, btnSystemset_43_BCU_Close3, btnSystemset_44_BCU_Open3 });
                    buttons_bcu.Add(3, new Button[] { btnSystemset_45_BCU_Lifted4, btnSystemset_43_BCU_Close4, btnSystemset_44_BCU_Open4 });
                    buttons_bcu.Add(4, new Button[] { btnSystemset_45_BCU_Lifted5, btnSystemset_43_BCU_Close5, btnSystemset_44_BCU_Open5 });
                    buttons_bcu.Add(5, new Button[] { btnSystemset_45_BCU_Lifted6, btnSystemset_43_BCU_Close6, btnSystemset_44_BCU_Open6 });
                    buttons_bcu.Add(6, new Button[] { btnSystemset_45_BCU_Lifted7, btnSystemset_43_BCU_Close7, btnSystemset_44_BCU_Open7 });
                    buttons_bcu.Add(7, new Button[] { btnSystemset_45_BCU_Lifted8, btnSystemset_43_BCU_Close8, btnSystemset_44_BCU_Open8 });
                    buttons_bcu.Add(8, new Button[] { btnSystemset_45_BCU_Lifted9, btnSystemset_43_BCU_Close9, btnSystemset_44_BCU_Open9 });
                    buttons_bcu.Add(9, new Button[] { btnSystemset_45_BCU_Lifted10, btnSystemset_43_BCU_Close10, btnSystemset_44_BCU_Open10 });
                    buttons_bcu.Add(10, new Button[] { btnSystemset_45_BCU_Lifted11, btnSystemset_43_BCU_Close11, btnSystemset_44_BCU_Open11 });
                    buttons_bcu.Add(11, new Button[] { btnSystemset_45_BCU_Lifted12, btnSystemset_43_BCU_Close12, btnSystemset_44_BCU_Open12 });
                    buttons_bcu.Add(12, new Button[] { btnSystemset_45_BCU_Lifted13, btnSystemset_43_BCU_Close13, btnSystemset_44_BCU_Open13 });
                    buttons_bcu.Add(13, new Button[] { btnSystemset_45_BCU_Lifted14, btnSystemset_43_BCU_Close14, btnSystemset_44_BCU_Open14 });
                    buttons_bcu.Add(14, new Button[] { btnSystemset_45_BCU_Lifted15, btnSystemset_43_BCU_Close15, btnSystemset_44_BCU_Open15 });
                    buttons_bcu.Add(15, new Button[] { btnSystemset_45_BCU_Lifted16, btnSystemset_43_BCU_Close16, btnSystemset_44_BCU_Open16 });

                    //byte[]转为二进制字符串
                    strResult = string.Empty;
                    //选取byte0-5数据
                    for (int i = 0; i <= 3; i++)
                    {
                        string strTemp = Convert.ToString(data[i], 2);
                        strTemp = strTemp.Insert(0, new string('0', 8 - strTemp.Length));
                        strResult = strTemp + strResult;
                    }

                    //二进制字符串转化为short[]
                    bitResult = new short[strResult.Length / 2];
                    int index5 = bitResult.Length - 1;
                    for (int i = 0; i < bitResult.Length; i++)
                    {
                        bitResult[index5--] = (short)Convert.ToByte(Convert.ToInt32(strResult.Substring(i * 2, 2)) & 0x03);
                    }

                    //根据short得值来找到对应得button
                    foreach (var item in buttons_bcu.Keys)
                    {
                        //解除->>将3该为0
                        for (int i = 0; i < bitResult.Length; i++)
                        {
                            if (bitResult[i] == 3)
                            {
                                bitResult[i] = 0;
                            }
                        }
                        Button btn = buttons_bcu[item][bitResult[item]];
                        string name = btn.Name;
                        btn.Enabled = false;
                    }
                    break;
                case 0x2B:
                    Dictionary<short, Button[]> buttons_bmu = new Dictionary<short, Button[]>();
                    buttons_bmu.Add(0, new Button[] { btnSystemset_45_BMU_Lifted1, btnSystemset_43_BMU_Close1, btnSystemset_44_BMU_Open1 });
                    buttons_bmu.Add(1, new Button[] { btnSystemset_45_BMU_Lifted2, btnSystemset_43_BMU_Close2, btnSystemset_44_BMU_Open2 });
                    buttons_bmu.Add(2, new Button[] { btnSystemset_45_BMU_Lifted3, btnSystemset_43_BMU_Close3, btnSystemset_44_BMU_Open3 });
                    buttons_bmu.Add(3, new Button[] { btnSystemset_45_BMU_Lifted4, btnSystemset_43_BMU_Close4, btnSystemset_44_BMU_Open4 });
                    buttons_bmu.Add(4, new Button[] { btnSystemset_45_BMU_Lifted5, btnSystemset_43_BMU_Close5, btnSystemset_44_BMU_Open5 });
                    buttons_bmu.Add(5, new Button[] { btnSystemset_45_BMU_Lifted6, btnSystemset_43_BMU_Close6, btnSystemset_44_BMU_Open6 });
                    buttons_bmu.Add(6, new Button[] { btnSystemset_45_BMU_Lifted7, btnSystemset_43_BMU_Close7, btnSystemset_44_BMU_Open7 });
                    buttons_bmu.Add(7, new Button[] { btnSystemset_45_BMU_Lifted8, btnSystemset_43_BMU_Close8, btnSystemset_44_BMU_Open8 });
                    buttons_bmu.Add(8, new Button[] { btnSystemset_45_BMU_Lifted9, btnSystemset_43_BMU_Close9, btnSystemset_44_BMU_Open9 });

                    //byte[]转为二进制字符串
                    strResult = string.Empty;
                    //选取byte0-5数据
                    for (int i = 0; i <= 3; i++)
                    {
                        string strTemp = Convert.ToString(data[i], 2);
                        strTemp = strTemp.Insert(0, new string('0', 8 - strTemp.Length));
                        strResult = strTemp + strResult;
                    }

                    //二进制字符串转化为short[]
                    bitResult = new short[strResult.Length / 2];
                    int index11 = bitResult.Length - 1;
                    for (int i = 0; i < bitResult.Length; i++)
                    {
                        bitResult[index11--] = (short)Convert.ToByte(Convert.ToInt32(strResult.Substring(i * 2, 2)) & 0x03);
                    }

                    //根据short得值来找到对应得button
                    foreach (var item in buttons_bmu.Keys)
                    {
                        //解除->>将3该为0
                        for (int i = 0; i < bitResult.Length; i++)
                        {
                            if (bitResult[i] == 3)
                            {
                                bitResult[i] = 0;
                            }
                        }
                        Button btn = buttons_bmu[item][bitResult[item]];
                        string name = btn.Name;
                        btn.Enabled = false;
                    }
                    break;
            }
        }

        #endregion

        #region 进入调试AND退出调试

        /// <summary>
        /// BCU调试-0xFA（进入/退出）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSystemset_debug_Click(object sender, EventArgs e)
        {
            byte[] canid = new byte[] { 0xE0, FrmMain.BCU_ID, 0xFA, 0x10 };

            if (btnSystemset_debug.Text == "结束调试" || btnSystemset_debug.Text == "End debugging")
            {
                DialogResult result = MessageBox.Show(LanguageHelper.GetLanguage("BmsDebug_Exit"), LanguageHelper.GetLanguage("BmsDebug_Tip"), MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    int num = 0x10 + 0x20 + FrmMain.BCU_ID + 0xFA + 0x00 + 0x00 + 0x55 + 0x00 + 0x00 + 0x00 + 0x00;
                    byte[] data = new byte[8] { 0x00, 0x00, 0x55, 0x00, 0x00, 0x00, 0x00, (byte)(num & 0xff) };

                    ecanHelper.Send(data, canid);

                    foreach (Control c in gb0FA.Controls)
                    {
                        if (c is Button)
                        {
                            c.Enabled = false;
                        }
                    }
                    btnSystemset_debug.Enabled = true;
                    btnSystemset_debug.Text = LanguageHelper.GetLanguage("BmsDebug_Start");
                    btnSystemset_debug.BackColor = Color.Green;
                }
            }
            else
            {
                DialogResult result = MessageBox.Show(LanguageHelper.GetLanguage("BmsDebug_Enter"), LanguageHelper.GetLanguage("BmsDebug_Tip"), MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    ReadCurrentState_BCU();

                    int num = 0x10 + 0x20 + FrmMain.BCU_ID + 0xFA + 0x00 + 0x00 + 0x00 + 0x00 + 0x00 + 0x00 + 0x00;
                    byte[] data = new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, (byte)(num & 0xff) };

                    ecanHelper.Send(data, canid);

                    foreach (Control c in gb0FA.Controls)
                    {
                        if (c is Panel)
                        {
                            foreach (Control item in c.Controls)
                            {
                                if (item is Button)
                                {
                                    item.Enabled = true;
                                }
                            }
                        }
                    }
                    btnSystemset_debug.Text = LanguageHelper.GetLanguage("BmsDebug_End");
                    btnSystemset_debug.BackColor = Color.Red;
                }
            }
        }

        /// <summary>
        /// 0xFA开关控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDebugCommandBCU_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                switch (btn.Name)
                {
                    case "btnSystemset_45_BCU_Lifted1":
                        bitResult[0] = 3;
                        break;
                    case "btnSystemset_43_BCU_Close1":
                        bitResult[0] = 1;
                        break;
                    case "btnSystemset_44_BCU_Open1":
                        bitResult[0] = 2;
                        break;
                    case "btnSystemset_45_BCU_Lifted2":
                        bitResult[1] = 3;
                        break;
                    case "btnSystemset_43_BCU_Close2":
                        bitResult[1] = 1;
                        break;
                    case "btnSystemset_44_BCU_Open2":
                        bitResult[1] = 2;
                        break;
                    case "btnSystemset_45_BCU_Lifted3":
                        bitResult[2] = 3;
                        break;
                    case "btnSystemset_43_BCU_Close3":
                        bitResult[2] = 1;
                        break;
                    case "btnSystemset_44_BCU_Open3":
                        bitResult[2] = 2;
                        break;
                    case "btnSystemset_45_BCU_Lifted4":
                        bitResult[3] = 3;
                        break;
                    case "btnSystemset_43_BCU_Close4":
                        bitResult[3] = 1;
                        break;
                    case "btnSystemset_44_BCU_Open4":
                        bitResult[3] = 2;
                        break;
                    case "btnSystemset_45_BCU_Lifted5":
                        bitResult[4] = 3;
                        break;
                    case "btnSystemset_43_BCU_Close5":
                        bitResult[4] = 1;
                        break;
                    case "btnSystemset_44_BCU_Open5":
                        bitResult[4] = 2;
                        break;
                    case "btnSystemset_45_BCU_Lifted6":
                        bitResult[5] = 3;
                        break;
                    case "btnSystemset_43_BCU_Close6":
                        bitResult[5] = 1;
                        break;
                    case "btnSystemset_44_BCU_Open6":
                        bitResult[5] = 2;
                        break;
                    case "btnSystemset_45_BCU_Lifted7":
                        bitResult[6] = 3;
                        break;
                    case "btnSystemset_43_BCU_Close7":
                        bitResult[6] = 1;
                        break;
                    case "btnSystemset_44_BCU_Open7":
                        bitResult[6] = 2;
                        break;
                    case "btnSystemset_45_BCU_Lifted8":
                        bitResult[7] = 3;
                        break;
                    case "btnSystemset_43_BCU_Close8":
                        bitResult[7] = 1;
                        break;
                    case "btnSystemset_44_BCU_Open8":
                        bitResult[7] = 2;
                        break;
                    case "btnSystemset_45_BCU_Lifted9":
                        bitResult[8] = 3;
                        break;
                    case "btnSystemset_43_BCU_Close9":
                        bitResult[8] = 1;
                        break;
                    case "btnSystemset_44_BCU_Open9":
                        bitResult[8] = 2;
                        break;
                    case "btnSystemset_45_BCU_Lifted10":
                        bitResult[9] = 3;
                        break;
                    case "btnSystemset_43_BCU_Close10":
                        bitResult[9] = 1;
                        break;
                    case "btnSystemset_44_BCU_Open10":
                        bitResult[9] = 2;
                        break;
                    case "btnSystemset_45_BCU_Lifted11":
                        bitResult[10] = 3;
                        break;
                    case "btnSystemset_43_BCU_Close11":
                        bitResult[10] = 1;
                        break;
                    case "btnSystemset_44_BCU_Open11":
                        bitResult[10] = 2;
                        break;
                    case "btnSystemset_45_BCU_Lifted12":
                        bitResult[11] = 3;
                        break;
                    case "btnSystemset_43_BCU_Close12":
                        bitResult[11] = 1;
                        break;
                    case "btnSystemset_44_BCU_Open12":
                        bitResult[11] = 2;
                        break;
                    case "btnSystemset_45_BCU_Lifted13":
                        bitResult[12] = 3;
                        break;
                    case "btnSystemset_43_BCU_Close13":
                        bitResult[12] = 1;
                        break;
                    case "btnSystemset_44_BCU_Open13":
                        bitResult[12] = 2;
                        break;
                    case "btnSystemset_45_BCU_Lifted14":
                        bitResult[13] = 3;
                        break;
                    case "btnSystemset_43_BCU_Close14":
                        bitResult[13] = 1;
                        break;
                    case "btnSystemset_44_BCU_Open14":
                        bitResult[13] = 2;
                        break;
                    case "btnSystemset_45_BCU_Lifted15":
                        bitResult[14] = 3;
                        break;
                    case "btnSystemset_43_BCU_Close15":
                        bitResult[14] = 1;
                        break;
                    case "btnSystemset_44_BCU_Open15":
                        bitResult[14] = 2;
                        break;
                    case "btnSystemset_45_BCU_Lifted16":
                        bitResult[15] = 3;
                        break;
                    case "btnSystemset_43_BCU_Close16":
                        bitResult[15] = 1;
                        break;
                    case "btnSystemset_44_BCU_Open16":
                        bitResult[15] = 2;
                        break;
                        //case "btnSystemset_45_BCU_Lifted17":
                        //    bitResult[16] = 0;
                        //    break;
                        //case "btnSystemset_43_BCU_Close17":
                        //    bitResult[16] = 1;
                        //    break;
                        //case "btnSystemset_44_BCU_Open17":
                        //    bitResult[16] = 2;
                        //    break;
                        //case "btnSystemset_45_BCU_Lifted18":
                        //    bitResult[17] = 0;
                        //    break;
                        //case "btnSystemset_43_BCU_Close18":
                        //    bitResult[17] = 1;
                        //    break;
                        //case "btnSystemset_44_BCU_Open18":
                        //    bitResult[17] = 2;
                        //    break;
                        //case "btnSystemset_45_BCU_Lifted19":
                        //    bitResult[18] = 0;
                        //    break;
                        //case "btnSystemset_43_BCU_Close19":
                        //    bitResult[18] = 1;
                        //    break;
                        //case "btnSystemset_44_BCU_Open19":
                        //    bitResult[18] = 2;
                        //    break;
                        //case "btnSystemset_45_BCU_Lifted20":
                        //    bitResult[19] = 0;
                        //    break;
                        //case "btnSystemset_43_BCU_Close20":
                        //    bitResult[19] = 1;
                        //    break;
                        //case "btnSystemset_44_BCU_Open20":
                        //    bitResult[19] = 2;
                        //    break;
                        //case "btnSystemset_45_BCU_Lifted21":
                        //    bitResult[20] = 0;
                        //    break;
                        //case "btnSystemset_43_BCU_Close21":
                        //    bitResult[20] = 1;
                        //    break;
                        //case "btnSystemset_44_BCU_Open21":
                        //    bitResult[20] = 2;
                        //    break;
                }
                //short[] 转为二级制数组
                strResult = string.Empty;
                for (int i = 0; i < bitResult.Length; i++)
                {
                    string strTemp = Convert.ToString(bitResult[i], 2);
                    strTemp = strTemp.Insert(0, new string('0', 2 - strTemp.Length));
                    strResult = strTemp + strResult;
                }

                //数组转byte[]:0x.... 0xAA CRC8
                byte[] data2 = new byte[strResult.Length / 8];
                for (int i = 0; i < data2.Length; i++)
                {
                    data2[i] = (byte)Convert.ToInt32(strResult.Substring(i * 8, 8), 2);
                }

                //byte数组组装
                int num = 0;
                byte[] data = new byte[7];
                data[num++] = data2[3];
                data[num++] = data2[2];
                data[num++] = data2[1];
                data[num++] = data2[0];
                data[num++] = 0x00;
                data[num++] = 0x00;
                data[num++] = 0x00;

                //0xAA：强制控制
                TestAte_BCU(data);

                Task.Run(new Action(() =>
                {
                    setUI_BCU(true);

                    Thread.Sleep(500);

                    //0x00：查询控制状态
                    TestAte_BCU();
                }));
            }
        }

        /// <summary>
        ///  BCU调试-0xF6（进入/退出）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSystemset_debug1_Click(object sender, EventArgs e)
        {
            byte[] canid = new byte[] { 0xE0, FrmMain.BCU_ID, 0xF6, 0x10 };

            if (btnSystemset_debug1.Text == "结束调试" || btnSystemset_debug1.Text == "End debugging")
            {
                DialogResult result = MessageBox.Show(LanguageHelper.GetLanguage("BmsDebug_Exit"), LanguageHelper.GetLanguage("BmsDebug_Tip"), MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    int num = 0x10 + 0x20 + FrmMain.BCU_ID + 0xF6 + 0x00 + 0x00 + 0x55 + 0x00 + 0x00 + 0x00 + 0x00;
                    byte[] data = new byte[8] { 0x00, 0x00, 0x55, 0x00, 0x00, 0x00, 0x00, (byte)(num & 0xff) };

                    ecanHelper.Send(data, canid);

                    foreach (Control c in gbControl0F6.Controls)
                    {
                        if (c is Button)
                        {
                            c.Enabled = false;
                        }
                    }
                    btnSystemset_debug1.Enabled = true;
                    btnSystemset_debug1.Text = LanguageHelper.GetLanguage("BmsDebug_Start");
                    btnSystemset_debug1.BackColor = Color.Green;
                }
            }
            else
            {
                DialogResult result = MessageBox.Show(LanguageHelper.GetLanguage("BmsDebug_Enter"), LanguageHelper.GetLanguage("BmsDebug_Tip"), MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    ReadCurrentState_BCU1();

                    int num = 0x10 + 0x20 + FrmMain.BCU_ID + 0xF6 + 0x00 + 0x00 + 0x00 + 0x00 + 0x00 + 0x00 + 0x00;
                    byte[] data = new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, (byte)(num & 0xff) };

                    ecanHelper.Send(data, canid);

                    foreach (Control c in gbControl0F6.Controls)
                    {
                        if (c is Button)
                        {
                            c.Enabled = true;
                        }
                    }
                    btnSystemset_debug1.Text = LanguageHelper.GetLanguage("BmsDebug_End");
                    btnSystemset_debug1.BackColor = Color.Red;
                }
            }
        }

        /// <summary>
        /// 0xF6开关控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDebugCommandBCU1_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                switch (btn.Name)
                {

                    case "btnSystemset_45_BCU1_Lifted1":
                        bitResult1[0] = 3;
                        break;
                    case "btnSystemset_43_BCU1_Close1":
                        bitResult1[0] = 1;
                        break;
                    case "btnSystemset_44_BCU1_Open1":
                        bitResult1[0] = 2;
                        break;
                    case "btnSystemset_45_BCU1_Lifted2":
                        bitResult1[1] = 3;
                        break;
                    case "btnSystemset_43_BCU1_Close2":
                        bitResult1[1] = 1;
                        break;
                    case "btnSystemset_44_BCU1_Open2":
                        bitResult1[1] = 2;
                        break;
                    //case "btnSystemset_45_BCU1_Lifted3":
                    //    bitResult1[2] = 0;
                    //    break;
                    //case "btnSystemset_43_BCU1_Close3":
                    //    bitResult1[2] = 1;
                    //    break;
                    //case "btnSystemset_44_BCU1_Open3":
                    //    bitResult1[2] = 2;
                    //    break;
                    //case "btnSystemset_45_BCU1_Lifted4":
                    //    bitResult1[3] = 0;
                    //    break;
                    //case "btnSystemset_43_BCU1_Close4":
                    //    bitResult1[3] = 1;
                    //    break;
                    //case "btnSystemset_44_BCU1_Open4":
                    //    bitResult1[3] = 2;
                    //    break;
                    default:
                        break;
                }
                //short[]转为二级制数组
                strResult = string.Empty;
                for (int i = 0; i < bitResult1.Length; i++)
                {
                    string strTemp = Convert.ToString(bitResult1[i], 2);
                    strTemp = strTemp.Insert(0, new string('0', 2 - strTemp.Length));
                    strResult = strTemp + strResult;
                }


                //数组转byte[]:0x.... 0xAA CRC8
                byte[] data2 = new byte[strResult.Length / 8];
                for (int i = 0; i < data2.Length; i++)
                {
                    data2[i] = (byte)Convert.ToInt32(strResult.Substring(i * 8, 8), 2);
                }

                //byte数组组装
                int num = 0;
                byte[] data = new byte[7];
                data[num++] = data2[0];
                data[num++] = 0x00;
                data[num++] = 0x00;
                data[num++] = 0x00;
                data[num++] = 0x00;
                data[num++] = 0x00;
                data[num++] = 0x00;//CBS不在使用0xAA作为查询功能码
                //ReadCurrentState_BCU1(data);
                TestAte_BCU1(data);

                Task.Run(new Action(() =>
                {
                    setUI_BCU1(true);

                    Thread.Sleep(500);
                    //ReadCurrentState_BCU1();
                    TestAte_BCU1();
                }));
            }
        }
        #endregion

        #region 读取当前开关机状态
        public void ReadCurrentState_BCU()
        {
            byte[] id = new byte[] { 0xE0, FrmMain.BCU_ID, 0xFA, 0x10 };

            byte[] data = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            byte[] crcData = new byte[11] { 0xE0, FrmMain.BCU_ID, 0xFA, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            data[7] = (byte)(Crc8_8210_nBytesCalculate(crcData, 11, 0) & 0xff);

            ecanHelper.Send(data, id);
        }
        public void ReadCurrentState_BCU1()
        {
            byte[] id = new byte[] { 0xE0, FrmMain.BCU_ID, 0xF6, 0x10 };

            byte[] data = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            byte[] crcData = new byte[11] { 0xE0, FrmMain.BCU_ID, 0xF6, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            data[7] = (byte)(Crc8_8210_nBytesCalculate(crcData, 11, 0) & 0xff);

            ecanHelper.Send(data, id);
        }
        public void ReadCurrentState_BMU()
        {
            byte[] id = new byte[] { 0xE0, FrmMain.BMS_ID, 0x2B, 0x10 };

            byte[] data = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            byte[] crcData = new byte[11] { 0xE0, FrmMain.BMS_ID, 0x2B, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            data[7] = (byte)(Crc8_8210_nBytesCalculate(crcData, 11, 0) & 0xff);

            ecanHelper.Send(data, id);
        }

        public void TestAte_BCU()
        {
            byte[] id = new byte[] { 0xE0, FrmMain.BCU_ID, 0xFA, 0x10 };

            byte[] data = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            byte[] crcData = new byte[11] { 0xE0, FrmMain.BCU_ID, 0xFA, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            data[7] = (byte)(Crc8_8210_nBytesCalculate(crcData, 11, 0) & 0xff);

            ecanHelper.Send(data, id);
        }
        public void TestAte_BCU(byte[] data)
        {
            byte[] id = new byte[] { 0xE0, FrmMain.BCU_ID, 0xFA, 0x10 };

            byte[] crcData = new byte[11] { 0xE0, FrmMain.BCU_ID, 0xFA, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            Array.Copy(data, 0, crcData, 4, data.Length);

            byte[] dataNew = new byte[8];
            Array.Copy(data, 0, dataNew, 0, data.Length);
            dataNew[7] = (byte)(Crc8_8210_nBytesCalculate(crcData, 11, 0) & 0xff);

            ecanHelper.Send(dataNew, id);
        }

        public void TestAte_BCU1()
        {
            byte[] id = new byte[] { 0xE0, FrmMain.BCU_ID, 0xF6, 0x10 };

            byte[] data = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            byte[] crcData = new byte[11] { 0xE0, FrmMain.BCU_ID, 0xF6, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            data[7] = (byte)(Crc8_8210_nBytesCalculate(crcData, 11, 0) & 0xff);

            ecanHelper.Send(data, id);
        }
        public void TestAte_BCU1(byte[] data)
        {
            byte[] id = new byte[] { 0xE0, FrmMain.BCU_ID, 0xF6, 0x10 };

            byte[] crcData = new byte[11] { 0xE0, FrmMain.BCU_ID, 0xF6, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            Array.Copy(data, 0, crcData, 4, data.Length);

            byte[] dataNew = new byte[8];
            Array.Copy(data, 0, dataNew, 0, data.Length);
            dataNew[7] = (byte)(Crc8_8210_nBytesCalculate(crcData, 11, 0) & 0xff);

            ecanHelper.Send(dataNew, id);
        }

        // NULL=表示查询，非空为设置帧
        public void TestAte_BMU(byte[] data = null)
        {
            byte[] canId = new byte[] { 0xE0, FrmMain.BMS_ID, 0x2B, 0x10 };

            if (data == null || data.Length < 1)
            {
                byte[] tempBuffer = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                byte[] crcData = new byte[11] { 0xE0, FrmMain.BMS_ID, 0x2B, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                tempBuffer[7] = (byte)(Crc8_8210_nBytesCalculate(crcData, 11, 0) & 0xff);

                ecanHelper.Send(tempBuffer, canId);
            }
            else
            {
                byte[] crcData = new byte[11] { 0xE0, FrmMain.BMS_ID, 0x2B, 0x10, 0x00, 0x00, 0xAA, 0x00, 0x00, 0x00, 0x00 };

                Array.Copy(data, 0, crcData, 4, data.Length);

                byte[] dataNew = new byte[8];
                Array.Copy(data, 0, dataNew, 0, data.Length);
                dataNew[7] = (byte)(Crc8_8210_nBytesCalculate(crcData, 11, 0) & 0xff);

                ecanHelper.Send(dataNew, canId);
            }
        }
        #endregion

        #region UI启用和禁用设置
        private void setUI_BCU(bool state)
        {
            this.Invoke(new Action(() =>
            {
                btnSystemset_44_BCU_Open1.Enabled = state;
                btnSystemset_43_BCU_Close1.Enabled = state;
                btnSystemset_45_BCU_Lifted1.Enabled = state;

                btnSystemset_44_BCU_Open2.Enabled = state;
                btnSystemset_43_BCU_Close2.Enabled = state;
                btnSystemset_45_BCU_Lifted2.Enabled = state;

                btnSystemset_44_BCU_Open3.Enabled = state;
                btnSystemset_43_BCU_Close3.Enabled = state;
                btnSystemset_45_BCU_Lifted3.Enabled = state;

                btnSystemset_44_BCU_Open4.Enabled = state;
                btnSystemset_43_BCU_Close4.Enabled = state;
                btnSystemset_45_BCU_Lifted4.Enabled = state;

                btnSystemset_44_BCU_Open5.Enabled = state;
                btnSystemset_43_BCU_Close5.Enabled = state;
                btnSystemset_45_BCU_Lifted5.Enabled = state;

                btnSystemset_44_BCU_Open6.Enabled = state;
                btnSystemset_43_BCU_Close6.Enabled = state;
                btnSystemset_45_BCU_Lifted6.Enabled = state;

                btnSystemset_44_BCU_Open7.Enabled = state;
                btnSystemset_43_BCU_Close7.Enabled = state;
                btnSystemset_45_BCU_Lifted7.Enabled = state;

                btnSystemset_44_BCU_Open8.Enabled = state;
                btnSystemset_43_BCU_Close8.Enabled = state;
                btnSystemset_45_BCU_Lifted8.Enabled = state;

                btnSystemset_44_BCU_Open9.Enabled = state;
                btnSystemset_43_BCU_Close9.Enabled = state;
                btnSystemset_45_BCU_Lifted9.Enabled = state;

                btnSystemset_44_BCU_Open10.Enabled = state;
                btnSystemset_43_BCU_Close10.Enabled = state;
                btnSystemset_45_BCU_Lifted10.Enabled = state;

                btnSystemset_44_BCU_Open11.Enabled = state;
                btnSystemset_43_BCU_Close11.Enabled = state;
                btnSystemset_45_BCU_Lifted11.Enabled = state;

                btnSystemset_44_BCU_Open12.Enabled = state;
                btnSystemset_43_BCU_Close12.Enabled = state;
                btnSystemset_45_BCU_Lifted12.Enabled = state;

                btnSystemset_44_BCU_Open13.Enabled = state;
                btnSystemset_43_BCU_Close13.Enabled = state;
                btnSystemset_45_BCU_Lifted13.Enabled = state;

                btnSystemset_44_BCU_Open14.Enabled = state;
                btnSystemset_43_BCU_Close15.Enabled = state;
                btnSystemset_45_BCU_Lifted15.Enabled = state;

                btnSystemset_44_BCU_Open16.Enabled = state;
                btnSystemset_43_BCU_Close16.Enabled = state;
                btnSystemset_45_BCU_Lifted16.Enabled = state;
            }));
        }

        private void setUI_BCU1(bool state)
        {
            this.Invoke(new Action(() =>
            {
                btnSystemset_44_BCU1_Open1.Enabled = state;
                btnSystemset_43_BCU1_Close1.Enabled = state;
                btnSystemset_45_BCU1_Lifted1.Enabled = state;


                btnSystemset_44_BCU1_Open2.Enabled = state;
                btnSystemset_43_BCU1_Close2.Enabled = state;
                btnSystemset_45_BCU1_Lifted2.Enabled = state;

            }));
        }

        private void setUI_BMU(bool state)
        {
            this.Invoke(new Action(() =>
            {
                btnSystemset_44_BMU_Open1.Enabled = state;
                btnSystemset_43_BMU_Close1.Enabled = state;
                btnSystemset_45_BMU_Lifted1.Enabled = state;

                btnSystemset_44_BMU_Open2.Enabled = state;
                btnSystemset_43_BMU_Close2.Enabled = state;
                btnSystemset_45_BMU_Lifted2.Enabled = state;

                btnSystemset_44_BMU_Open3.Enabled = state;
                btnSystemset_43_BMU_Close3.Enabled = state;
                btnSystemset_45_BMU_Lifted3.Enabled = state;

                btnSystemset_44_BMU_Open4.Enabled = state;
                btnSystemset_43_BMU_Close4.Enabled = state;
                btnSystemset_45_BMU_Lifted4.Enabled = state;

                btnSystemset_44_BMU_Open5.Enabled = state;
                btnSystemset_43_BMU_Close5.Enabled = state;
                btnSystemset_45_BMU_Lifted5.Enabled = state;

                btnSystemset_44_BMU_Open6.Enabled = state;
                btnSystemset_43_BMU_Close6.Enabled = state;
                btnSystemset_45_BMU_Lifted6.Enabled = state;

                btnSystemset_44_BMU_Open7.Enabled = state;
                btnSystemset_43_BMU_Close7.Enabled = state;
                btnSystemset_45_BMU_Lifted7.Enabled = state;

                btnSystemset_44_BMU_Open8.Enabled = state;
                btnSystemset_43_BMU_Close8.Enabled = state;
                btnSystemset_45_BMU_Lifted8.Enabled = state;

                btnSystemset_44_BMU_Open9.Enabled = state;
                btnSystemset_43_BMU_Close9.Enabled = state;
                btnSystemset_45_BMU_Lifted9.Enabled = state;
            }));
        }
        #endregion

        #region CBS5000
        /// <summary>
        /// 开关控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSet020_1_Click(object sender, EventArgs e)
        {
            byte[] can_id = new byte[4] { 0xE0, FrmMain.BMS_ID, 0x20, 0x10 };

            byte[] data = new byte[8];
            byte crc8 = (byte)(0x10 + 0x20 + data[6] + 0xE0);
            for (int i = 0; i < data.Length - 1; i++)
            {
                crc8 += data[i];
            }

            data[7] = crc8;
            if (ecanHelper.Send(data, can_id))
            {
                MessageBox.Show(FrmMain.GetString("keyWriteSuccess"));
            }
            else
            {
                MessageBox.Show(FrmMain.GetString("keyWriteFail"));
            }
        }

        /// <summary>
        /// 设置充电&放电状态参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetStateParam_Click(object sender, EventArgs e)
        {
            //List<byte> bytes = new List<byte>();

            byte[] can_id = new byte[4] { 0xE0, FrmMain.BMS_ID, 0x60, 0x10 };
            //bytes.AddRange(can_id);

            byte[] data = new byte[8];

            int value = cbbState.SelectedIndex & 0x03;

            if (ckSystemset_67.Checked)
            {
                value = value | 0x4;
            }
            if (ckSystemset_68.Checked)
            {
                value = value | 0x8;
            }
            if (ckSystemset_69.Checked)
            {
                value = value | 0x10;
            }
            if (ckSystemset_60.Checked)
            {
                value = value | 0x80;
            }

            int current = Convert.ToInt32(txtPackCurrent.Text.Trim());
            data[0] = (byte)(current & 0xff);
            data[1] = (byte)(current >> 8);
            data[2] = Convert.ToByte(value);
            data[3] = Convert.ToByte(Convert.ToInt32(txtSyncFallSoc.Text, 16));
            //bytes.AddRange(data);

            byte[] crcData = new byte[11] { 0xE0, FrmMain.BMS_ID, 0x60, 0x10, data[0], data[1], data[2], data[3], data[4], data[5], data[6] };

            data[7] = (byte)(Crc8_8210_nBytesCalculate(crcData, 11, 0) & 0xff);
            ecanHelper.Send(data, can_id);
        }

        /// <summary>
        /// 设置均衡信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetControlInfo_Click(object sender, EventArgs e)
        {
            //List<byte> bytes = new List<byte>();

            byte[] can_id = new byte[4] { 0xE0, FrmMain.BMS_ID, 0x61, 0x10 };
            //bytes.AddRange(canid);

            byte[] data = new byte[8];
            //主动均衡充电使能
            data[0] = (byte)Convert.ToByte(cbbActiveBalanceCtrl.SelectedIndex);
            //主动均衡电流
            int current = Convert.ToInt32(txtPackActiveBalanceCur.Text.Trim());
            data[1] = (byte)(current & 0xff);
            data[2] = (byte)(current >> 8);
            //主动均衡容量
            int capacity = Convert.ToInt32(txtPackActiveBalanceCap.Text.Trim());
            data[3] = (byte)(capacity & 0xff);
            data[4] = (byte)(capacity >> 8);
            //bytes.AddRange(data);

            byte[] crcData = new byte[11] { 0xE0, FrmMain.BMS_ID, 0x61, 0x10, data[0], data[1], data[2], data[3], data[4], data[5], data[6] };
            data[7] = (byte)(Crc8_8210_nBytesCalculate(crcData, 11, 0) & 0xff);

            ecanHelper.Send(data, can_id);
        }
        #endregion

        #region CBS-BCU模块
        /// <summary>
        /// 读取数据（未引用）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRead_Click(object sender, EventArgs e)
        {
            List<uint> DataLists = new List<uint>() { 0xAA11, 0xAA22, 0xAA33, 0xAA44, 0xAA55, 0xAA66, 0xAA77, 0xAA88 };

            for (int i = 0; i < DataLists.Count; i++)
            {
                DataSelected(DataLists[i]);

                Thread.Sleep(100);
            }

            byte[] bytes1 = new byte[8] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            if (ecanHelper.Send(bytes1, new byte[] { 0xE0, FrmMain.BCU_ID, 0xF9, 0x10 }))
            {
                MessageBox.Show(FrmMain.GetString("keyReadSuccess"));
            }
            else
            {
                MessageBox.Show(FrmMain.GetString("keyReadFail"));
            }
        }

        /// <summary>
        /// 设置BCU序列号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetBcuSN_Click(object sender, EventArgs e)
        {
            SetBCU_SN(txtbcuCode.Text.Trim());
        }

        /// <summary>
        /// 设置BCU单板序列号SN
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetBcuBoardSN_Click(object sender, EventArgs e)
        {
            SetBCU_SN(txtBCUBoardSN.Text.Trim(), true);
        }

        /// <summary>
        /// 设置BCU序列号函数
        /// </summary>
        /// <param name="bmsId"></param>
        /// <param name="packSn"></param>
        /// <param name="identity">false=SN号设置，true=单板SN设置</param>
        public void SetBCU_SN(string packSn, bool identity = false)
        {
            bool flag_WriteSuccess = false;
            if (packSn.Length != 20)
            {
                MessageBox.Show("SN序列号长度不等于20！输入的序列号长度为" + packSn.Length.ToString() + "位");
                return;
            }

            byte[] can_id = new byte[] { 0xE0, FrmMain.BCU_ID, 0xF3, 0x10 };
            if (identity)
            {
                can_id = new byte[] { 0xE0, FrmMain.BCU_ID, 0xF4, 0x10 };
            }

            string[] strs = new string[packSn.Length];
            for (int j = 0; j < packSn.Length; j++)
            {
                strs[j] = ((int)packSn[j]).ToString("X2");
            }

            for (int j = 0; j < strs.Length; j++)
            {
                int i = 0;
                byte[] data = new byte[8];

                data[i++] = Convert.ToByte((j / 7).ToString(), 16);
                data[i++] = Convert.ToByte(strs[j++], 16);
                data[i++] = Convert.ToByte(strs[j++], 16);
                data[i++] = Convert.ToByte(strs[j++], 16);
                data[i++] = Convert.ToByte(strs[j++], 16);
                data[i++] = Convert.ToByte(strs[j++], 16);
                data[i++] = Convert.ToByte(strs[j++], 16);
                data[i++] = j == strs.Length ? (byte)0x00 : Convert.ToByte(strs[j], 16);
                if (ecanHelper.Send(data, can_id))
                {
                    flag_WriteSuccess = true;
                }
            }
            if (flag_WriteSuccess)
            {
                MessageBox.Show(FrmMain.GetString("keyWriteSuccess"));
            }
            else
            {
                MessageBox.Show(FrmMain.GetString("keyWriteFail"));
            }
        }

        /// <summary>
        /// BCU时间校准
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetBcuTime_Click(object sender, EventArgs e)
        {
            byte[] canid_BCU = new byte[] { 0xE0, FrmMain.BCU_ID, 0xF2, 0x10 };
            string[] date = dateTimePicker2.Text.Split(new char[] { ' ', '-', ':' });
            bytes = new byte[] {
                                    Convert.ToByte(Convert.ToInt32(date[0]) - 2000),
                                    Convert.ToByte(Convert.ToInt32(date[1])),
                                    Convert.ToByte(Convert.ToInt32(date[2])),
                                    Convert.ToByte(Convert.ToInt32(date[3])),
                                    Convert.ToByte(Convert.ToInt32(date[4])),
                                    Convert.ToByte(Convert.ToInt32(date[5])),
                                    0x00, 0x00 };
            //发送指令
            if (ecanHelper.Send(bytes, canid_BCU))
            {
                MessageBox.Show(FrmMain.GetString("keyWriteSuccess"));
            }
            else
            {
                MessageBox.Show(FrmMain.GetString("keyWriteFail"));
            }
        }

        /// <summary>
        /// 设置FLASH数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetFlashData_Click(object sender, EventArgs e)
        {
            byte[] can_id = new byte[4] { 0xE0, FrmMain.BCU_ID, 0xEF, 0x10 };
            byte[] data = new byte[8];

            data[0] = 0x01;
            byte[] data1 = Encoding.Default.GetBytes(txtFlashData.Text);
            int lengthToCopy = Math.Min(data1.Length, data.Length - 1);
            Array.Copy(data1, 0, data, 1, lengthToCopy);

            if (ecanHelper.Send(data, can_id))
            {
                MessageBox.Show(FrmMain.GetString("keyWriteSuccess"));
            }
            else
            {
                MessageBox.Show(FrmMain.GetString("keyWriteFail"));
            }
        }

        /// <summary>
        /// 设置BCU校准系数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetCalibrationBCU_Click(object sender, EventArgs e)
        {
            int val;

            if (sender is Button btn)
            {
                switch (btn.Name)
                {
                    case "btnSetCalibration_BCU_01"://校准总压
                        val = Convert.ToInt32(Convert.ToDouble(txtBCU_Calibration01.Text.Trim()) / 0.1);

                        CalibrationBCU(0x01, val);
                        break;
                    case "btnSetCalibration_BCU_02"://校准负载电压
                        val = Convert.ToInt32(Convert.ToDouble(txtBCU_Calibration02.Text.Trim()) / 0.1);

                        CalibrationBCU(0x02, val);
                        break;
                    case "btnSetCalibration_BCU_03"://校准充电电流
                        val = Convert.ToInt32(Convert.ToDouble(txtBCU_Calibration03.Text.Trim()) / 0.01);

                        CalibrationBCU(0x03, val);
                        break;
                    case "btnSetCalibration_BCU_04"://校准充电小电流
                        val = Convert.ToInt32(Convert.ToDouble(txtBCU_Calibration04.Text.Trim()) / 0.01);

                        CalibrationBCU(0x04, val);
                        break;
                    case "btnSetCalibration_BCU_05"://校准放电电流
                        val = Convert.ToInt32(Convert.ToDouble(txtBCU_Calibration05.Text.Trim()) / 0.01);

                        CalibrationBCU(0x05, val);
                        break;
                    case "btnSetCalibration_BCU_06"://校准放电小电流
                        val = Convert.ToInt32(Convert.ToDouble(txtBCU_Calibration06.Text.Trim()) / 0.01);

                        CalibrationBCU(0x06, val);
                        break;
                    case "btnSetCalibration_BCU_07"://加热膜供电电压
                        val = Convert.ToInt32(Convert.ToDouble(txtBCU_Calibration07.Text.Trim()) / 0.1);

                        CalibrationBCU(0x07, val);
                        break;
                    case "btnSetCalibration_BCU_08"://绝缘阻抗电压
                        val = Convert.ToInt32(Convert.ToDouble(txtBCU_Calibration08.Text.Trim()) / 0.1);

                        CalibrationBCU(0x08, val);
                        break;
                    case "btnSetCalibration_BCU_09"://加热膜MOS电压
                        val = Convert.ToInt32(Convert.ToDouble(txtBCU_Calibration09.Text.Trim()) / 0.1);

                        CalibrationBCU(0x09, val);
                        break;
                    case "btnSetCalibration_BCU_0A"://加热膜电流
                        val = Convert.ToInt32(Convert.ToDouble(txtBCU_Calibration0A.Text.Trim()) / 0.01);

                        CalibrationBCU(0x0A, val);
                        break;
                }
            }
        }
        private void CalibrationBCU(byte firstData, int CalibrationVal)
        {
            byte[] can_id = new byte[] { 0xE0, FrmMain.BCU_ID, 0xF5, 0x10 };

            byte[] data = new byte[8] { firstData, Convert.ToByte(CalibrationVal & 0xff), Convert.ToByte(CalibrationVal >> 8), 0x00, 0x00, 0x00, 0x00, 0x00 };

            if (ecanHelper.Send(data, can_id))
            {
                MessageBox.Show(FrmMain.GetString("keyWriteSuccess"));
            }
            else
            {
                MessageBox.Show(FrmMain.GetString("keyWriteFail"));
            }
        }

        /// <summary>
        /// 设置DSP校准系数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetCalibrationDSP_Click(object sender, EventArgs e)
        {
            int val;

            if (sender is Button btn)
            {
                switch (btn.Name)
                {
                    case "btnSetCalibration_DSP_01"://电感电流采样1校准系数

                        CalibrationDSP(0x01, int.Parse(txtDSP_Calibration01.Text));
                        break;
                    case "btnSetCalibration_DSP_02"://电感电流采样2校准系数

                        CalibrationDSP(0x02, int.Parse(txtDSP_Calibration02.Text));
                        break;
                    case "btnSetCalibration_DSP_03"://电感电流采样3校准系数

                        CalibrationDSP(0x03, int.Parse(txtDSP_Calibration03.Text));
                        break;
                    case "btnSetCalibration_DSP_04"://电感电流采样4校准系数

                        CalibrationDSP(0x04, int.Parse(txtDSP_Calibration04.Text));
                        break;
                    case "btnSetCalibration_DSP_05"://放电大电流采样校准系数

                        CalibrationDSP(0x05, int.Parse(txtDSP_Calibration05.Text));
                        break;
                    case "btnSetCalibration_DSP_06"://充电大电流采样校准系数

                        CalibrationDSP(0x06, int.Parse(txtDSP_Calibration06.Text));
                        break;
                    case "btnSetCalibration_DSP_07"://接触器电压采样校准系数

                        CalibrationDSP(0x07, int.Parse(txtDSP_Calibration07.Text));
                        break;
                    case "btnSetCalibration_DSP_08"://电池电压采样1校准系数

                        CalibrationDSP(0x08, int.Parse(txtDSP_Calibration08.Text));
                        break;
                    case "btnSetCalibration_DSP_09"://高压母线电压采样校准系数

                        CalibrationDSP(0x09, int.Parse(txtDSP_Calibration09.Text));
                        break;
                    case "btnSetCalibration_DSP_0A"://加热膜供电电压采样校准系数

                        CalibrationDSP(0x0A, int.Parse(txtDSP_Calibration0A.Text));
                        break;
                    case "btnSetCalibration_DSP_0B"://电池电压采样2（过继电器）校准系数

                        CalibrationDSP(0x0B, int.Parse(txtDSP_Calibration0B.Text));
                        break;
                }
            }
        }
        private void CalibrationDSP(byte firstData, int CalibrationVal)
        {
            byte[] can_id = new byte[] { 0xE0, FrmMain.BCU_ID, 0xFB, 0x10 };

            byte[] data = new byte[8] { firstData, Convert.ToByte(CalibrationVal & 0xff), Convert.ToByte(CalibrationVal >> 8), 0x00, 0x00, 0x00, 0x00, 0x00 };

            if (ecanHelper.Send(data, can_id))
            {
                MessageBox.Show(FrmMain.GetString("keyWriteSuccess"));
            }
            else
            {
                MessageBox.Show(FrmMain.GetString("keyWriteFail"));
            }
        }


        /// <summary>
        /// 设置OxF0控制参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSet0F0_Click(object sender, EventArgs e)
        {
            byte[] can_id = new byte[4] { 0xE0, 0x9F, 0xF0, 0x10 };

            byte[] data = new byte[8];
            foreach (Control item in this.gbControl0F0.Controls)
            {
                if (item is ComboBox)
                {
                    ComboBox cbb = item as ComboBox;
                    int index = 0;
                    int.TryParse(cbb.Name.Replace("cbbRequestF0_", ""), out index);
                    int value = 0;
                    switch (cbb.SelectedIndex)
                    {
                        case 0: value = 0x00; break;
                        case 1: value = 0xAA; break;
                        case 2: value = 0x55; break;
                        default:
                            break;
                    }
                    data[index] = (byte)(value & 0xff);
                }
            }

            int flag = 0;
            if (ckSystemset_BMU_60.Checked) flag += 1;
            if (ckSystemset_BMU_61.Checked) flag += 2;
            if (ckSystemset_BMU_62.Checked) flag += 4;
            if (ckSystemset_BMU_63.Checked) flag += 8;
            data[6] = (byte)flag;

            byte crc8 = (byte)((0xE0 + 0x9F + 0xF0 + 0x10) & 0xFF);
            for (int i = 0; i < data.Length - 1; i++)
            {
                crc8 += data[i];
            }

            data[7] = (byte)(crc8 & 0xFF);
            if (ecanHelper.Send(data, can_id))
            {
                MessageBox.Show(FrmMain.GetString("keyWriteSuccess"));
            }
            else
            {
                MessageBox.Show(FrmMain.GetString("keyWriteFail"));
            }
        }
        #endregion

        #region CBS-BMU模块

        #endregion

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
                else if (c is TabPage | c is Panel)
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

        #region 数据类型转换
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

            return (b & _byte) == _byte ? 1 : 0;
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

        private byte[] Uint16ToBytes(TextBox t1, TextBox t2, TextBox t3, TextBox t4,
           double scaling1, double scaling2, double scaling3, double scaling4)
        {
            byte[] b1 = Uint16ToBytes(Convert.ToUInt32(float.Parse(t1.Text) / scaling1));
            byte[] b2 = Uint16ToBytes(Convert.ToUInt32(float.Parse(t2.Text) / scaling2));
            byte[] b3 = Uint16ToBytes(Convert.ToUInt32(float.Parse(t3.Text) / scaling3));
            byte[] b4 = Uint16ToBytes(Convert.ToUInt32(float.Parse(t4.Text) / scaling4));

            return new byte[] { b1[0], b1[1], b2[0], b2[1], b3[0], b3[1], b4[0], b4[1] };
        }

        private byte[] Uint16ToBytes(uint ivalue)
        {
            byte[] data = new byte[2];
            data[1] = (byte)(ivalue >> 8);
            data[0] = (byte)(ivalue & 0xff);

            return data;
        }

        private bool ConvertIntToByteArray(Int32 m, ref byte[] arry)
        {
            if (arry == null) return false;
            if (arry.Length < 4) return false;

            arry[0] = (byte)(m & 0xFF);
            arry[1] = (byte)((m & 0xFF00) >> 8);
            arry[2] = (byte)((m & 0xFF0000) >> 16);
            arry[3] = (byte)((m >> 24) & 0xFF);

            return true;
        }

        private int[] BytesToUint16(byte[] data)
        {
            int[] numbers = new int[4];
            for (int i = 0; i < data.Length; i += 2)
            {
                numbers[i / 2] = Convert.ToInt32(data[i + 1].ToString("X2") + data[i].ToString("X2"), 16);
            }
            return numbers;
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

        private byte[] Uint8ToBits(List<int> vals)
        {
            if (vals.Count > 8 || vals.Count <= 0)
                return null;

            byte[] bytes = new byte[8];
            for (int i = 0; i < vals.Count; i++)
            {
                bytes[i] = (byte)Convert.ToByte(vals[i].ToString("X2"), 16);
            }

            return bytes;
        }
        #endregion

        #region CRC校验
        /*******************************************************************************
        * Function Name  : Crc8_8210_nBytesCalculate
        * Description    : CRC校验,多项式为0x2F
        *******************************************************************************/
        public static uint Crc8_8210_nBytesCalculate(byte[] pBuff, uint bLen, uint bCrsMask)
        {
            uint i;
            for (int k = 0; k < bLen; k++)
            {
                for (i = 0x80; i > 0; i >>= 1)
                {
                    if (0 != (bCrsMask & 0x80))
                    {
                        bCrsMask <<= 1;
                        bCrsMask ^= 0x2F;
                    }
                    else
                    {
                        bCrsMask <<= 1;
                    }
                    if (0 != (pBuff[k] & i))
                    {
                        bCrsMask ^= 0x2F;
                    }
                }
            }
            return bCrsMask;
        }

        #endregion

        #region BMU模块

        //0x02B:ATE控制参数
        private void btnSystemset_debug2_Click(object sender, EventArgs e)
        {
            byte[] canid = new byte[] { 0xE0, FrmMain.BMS_ID, 0x2B, 0x10 };

            if (btnSystemset_debug1.Text == "结束调试" || btnSystemset_debug1.Text == "End debugging")
            {
                DialogResult result = MessageBox.Show(LanguageHelper.GetLanguage("BmsDebug_Exit"), LanguageHelper.GetLanguage("BmsDebug_Tip"), MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    int num = 0x10 + 0x20 + FrmMain.BMS_ID + 0x2B + 0x00 + 0x00 + 0x55 + 0x00 + 0x00 + 0x00 + 0x00;
                    byte[] data = new byte[8] { 0x00, 0x00, 0x55, 0x00, 0x00, 0x00, 0x00, (byte)(num & 0xff) };

                    ecanHelper.Send(data, canid);

                    foreach (Control c in panel2.Controls)
                    {
                        if (c is Button)
                        {
                            c.Enabled = false;
                        }
                    }
                    btnSystemset_debug1.Enabled = true;
                    btnSystemset_debug1.Text = LanguageHelper.GetLanguage("BmsDebug_Start");
                    btnSystemset_debug1.BackColor = Color.Green;
                }
            }
            else
            {
                DialogResult result = MessageBox.Show(LanguageHelper.GetLanguage("BmsDebug_Enter"), LanguageHelper.GetLanguage("BmsDebug_Tip"), MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    ReadCurrentState_BMU();

                    int num = 0x10 + 0x20 + FrmMain.BMS_ID + 0x2B + 0x00 + 0x00 + 0x00 + 0x00 + 0x00 + 0x00 + 0x00;
                    byte[] data = new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, (byte)(num & 0xff) };

                    ecanHelper.Send(data, canid);

                    foreach (Control c in panel2.Controls)
                    {
                        if (c is Button)
                        {
                            c.Enabled = true;
                        }
                    }
                    btnSystemset_debug1.Text = LanguageHelper.GetLanguage("BmsDebug_End");
                    btnSystemset_debug1.BackColor = Color.Red;
                }
            }
        }

        /// <summary>
        /// 0x2B开关控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDebugCommandBMU_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                switch (btn.Name)
                {
                    case "btnSystemset_45_BMU_Lifted1":
                        bitResult[0] = 3;
                        break;
                    case "btnSystemset_43_BMU_Close1":
                        bitResult[0] = 1;
                        break;
                    case "btnSystemset_44_BMU_Open1":
                        bitResult[0] = 2;
                        break;
                    case "btnSystemset_45_BMU_Lifted2":
                        bitResult[1] = 3;
                        break;
                    case "btnSystemset_43_BMU_Close2":
                        bitResult[1] = 1;
                        break;
                    case "btnSystemset_44_BMU_Open2":
                        bitResult[1] = 2;
                        break;
                    case "btnSystemset_45_BMU_Lifted3":
                        bitResult[2] = 3;
                        break;
                    case "btnSystemset_43_BMU_Close3":
                        bitResult[2] = 1;
                        break;
                    case "btnSystemset_44_BMU_Open3":
                        bitResult[2] = 2;
                        break;
                    case "btnSystemset_45_BMU_Lifted4":
                        bitResult[3] = 3;
                        break;
                    case "btnSystemset_43_BMU_Close4":
                        bitResult[3] = 1;
                        break;
                    case "btnSystemset_44_BMU_Open4":
                        bitResult[3] = 2;
                        break;
                    case "btnSystemset_45_BMU_Lifted5":
                        bitResult[4] = 3;
                        break;
                    case "btnSystemset_43_BMU_Close5":
                        bitResult[4] = 1;
                        break;
                    case "btnSystemset_44_BMU_Open5":
                        bitResult[4] = 2;
                        break;
                    case "btnSystemset_45_BMU_Lifted6":
                        bitResult[5] = 3;
                        break;
                    case "btnSystemset_43_BMU_Close6":
                        bitResult[5] = 1;
                        break;
                    case "btnSystemset_44_BMU_Open6":
                        bitResult[5] = 2;
                        break;
                    case "btnSystemset_45_BMU_Lifted7":
                        bitResult[6] = 3;
                        break;
                    case "btnSystemset_43_BMU_Close7":
                        bitResult[6] = 1;
                        break;
                    case "btnSystemset_44_BMU_Open7":
                        bitResult[6] = 2;
                        break;
                    case "btnSystemset_45_BMU_Lifted8":
                        bitResult[7] = 3;
                        break;
                    case "btnSystemset_43_BMU_Close8":
                        bitResult[7] = 1;
                        break;
                    case "btnSystemset_44_BMU_Open8":
                        bitResult[7] = 2;
                        break;
                    case "btnSystemset_45_BMU_Lifted9":
                        bitResult[8] = 3;
                        break;
                    case "btnSystemset_43_BMU_Close9":
                        bitResult[8] = 1;
                        break;
                    case "btnSystemset_44_BMU_Open9":
                        bitResult[8] = 2;
                        break;
                }
                //short[] 转为二级制数组
                strResult = string.Empty;
                for (int i = 0; i < bitResult.Length; i++)
                {
                    string strTemp = Convert.ToString(bitResult[i], 2);
                    strTemp = strTemp.Insert(0, new string('0', 2 - strTemp.Length));
                    strResult = strTemp + strResult;
                }

                //数组转byte[]:0x.... 0xAA CRC8
                byte[] data2 = new byte[strResult.Length / 8];
                for (int i = 0; i < data2.Length; i++)
                {
                    data2[i] = (byte)Convert.ToInt32(strResult.Substring(i * 8, 8), 2);
                }

                //byte数组组装
                int num = 0;
                byte[] data = new byte[7];
                data[num++] = data2[3];
                data[num++] = data2[2];
                data[num++] = data2[1];
                data[num++] = data2[0];
                data[num++] = 0x00;
                data[num++] = 0x00;
                data[num++] = 0xAA;

                //0xAA：强制控制
                TestAte_BMU(data);

                Task.Run(new Action(() =>
                {
                    setUI_BMU(true);

                    Thread.Sleep(500);

                    //0x00：查询控制状态
                    TestAte_BMU();
                }));
            }
        }

        /// <summary>
        /// 设置BMU校准系数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetCalibrationBMU_Click(object sender, EventArgs e)
        {
            int val;

            if (sender is Button btn)
            {
                switch (btn.Name)
                {
                    case "btnSetCalibration_BMU_01"://校准总压
                        val = Convert.ToInt32(Convert.ToDouble(txtBMU_Calibration01.Text.Trim()) / 0.1);

                        CalibrationBMU(0x01, val);
                        break;
                    case "btnSetCalibration_BMU_02"://校准负载电压
                        val = Convert.ToInt32(Convert.ToDouble(txtBMU_Calibration02.Text.Trim()) / 0.1);

                        CalibrationBMU(0x02, val);
                        break;
                    case "btnSetCalibration_BMU_03"://校准充电电流
                        val = Convert.ToInt32(Convert.ToDouble(txtBMU_Calibration03.Text.Trim()) / 0.01);

                        CalibrationBMU(0x03, val);
                        break;
                    case "btnSetCalibration_BMU_04"://校准充电小电流
                        val = Convert.ToInt32(Convert.ToDouble(txtBMU_Calibration04.Text.Trim()) / 0.01);

                        CalibrationBMU(0x04, val);
                        break;
                    case "btnSetCalibration_BMU_05"://校准放电电流
                        val = Convert.ToInt32(Convert.ToDouble(txtBMU_Calibration05.Text.Trim()) / 0.01);

                        CalibrationBMU(0x05, val);
                        break;
                    case "btnSetCalibration_BMU_06"://校准放电小电流
                        val = Convert.ToInt32(Convert.ToDouble(txtBMU_Calibration06.Text.Trim()) / 0.01);

                        CalibrationBMU(0x06, val);
                        break;
                }
            }
        }
        private void CalibrationBMU(byte firstData, int CalibrationVal)
        {
            byte[] can_id = new byte[] { 0xE0, FrmMain.BMS_ID, 0x2A, 0x10 };

            byte[] data = new byte[8] { firstData, Convert.ToByte(CalibrationVal & 0xff), Convert.ToByte(CalibrationVal >> 8), 0x00, 0x00, 0x00, 0x00, 0x00 };

            if (ecanHelper.Send(data, can_id))
            {
                MessageBox.Show(FrmMain.GetString("keyWriteSuccess"));
            }
            else
            {
                MessageBox.Show(FrmMain.GetString("keyWriteFail"));
            }
        }
        #endregion

        private void btnSetAteFlash_Click(object sender, EventArgs e)
        {
            RealDataVM.SelectedRequest7 = FrmMain.BMS_ID;

            string flashData = txtFlashData_BMU.Text.Trim();
            bool result = RealDataVM.WriteFlashData(flashData);
            Debug.WriteLine(result == true ? "写入成功" : "写入失败");
        }

        private void btnSetSN_Pack_Click(object sender, EventArgs e)
        {
            RealDataVM.SelectedRequest7 = FrmMain.BMS_ID;

            string sn = txtPackSN_BMU.Text.Trim();
            bool result = RealDataVM.SetSN(sn);
            Debug.WriteLine(result == true ? "写入成功" : "写入失败");
        }

        private void btnSetSN_Board_Click(object sender, EventArgs e)
        {
            RealDataVM.SelectedRequest7 = FrmMain.BMS_ID;

            string sn = txtBoardSN_BMU.Text.Trim();
            bool result = RealDataVM.SetSN(sn, true);
            Debug.WriteLine(result == true ? "写入成功" : "写入失败");
        }

        private void btnSetBatteryinfo_Click(object sender, EventArgs e)
        {
            RealDataVM.SelectedRequest7 = FrmMain.BMS_ID;

            RealDataVM.Write_0x29(cbb_103.SelectedIndex.ToString(), Convert.ToInt32(txt_104.Text), cbb_105.SelectedIndex.ToString(), cbb_106.SelectedIndex.ToString());
        }

        private void btnSet_0x21_Click(object sender, EventArgs e)
        {
            RealDataVM.SelectedRequest7 = FrmMain.BMS_ID;

            RealDataVM.Write_0x21(txtBalOpenVolt.Text.Trim(), txtBalOpenVoltDiff.Text.Trim(), txtFullChgVolt.Text.Trim(), txtHeatFilmOpenTemp.Text.Trim(), txtHeatFilmCloseTemp.Text.Trim());
        }

        private void btnSet_0x22_Click(object sender, EventArgs e)
        {
            RealDataVM.SelectedRequest7 = FrmMain.BMS_ID;

            RealDataVM.Write_0x22(txtPackStopVolt.Text.Trim(), txtPackStopCurrent.Text.Trim());
        }

        private void btnSet_0x23_Click(object sender, EventArgs e)
        {
            RealDataVM.SelectedRequest7 = FrmMain.BMS_ID;

            RealDataVM.Write_0x23(txtRetedCapacity.Text.Trim(), txtCellVoltNum.Text.Trim(), txtCellTempNum.Text.Trim());
        }

        private void btnSet_0x24_Click(object sender, EventArgs e)
        {
            RealDataVM.SelectedRequest7 = FrmMain.BMS_ID;

            RealDataVM.Write_0x24(txtCumulativeChgCapacity.Text.Trim(), txtCumulativeDsgCapactiy.Text.Trim());
        }

        private void btnSet_0x25_Click(object sender, EventArgs e)
        {
            RealDataVM.SelectedRequest7 = FrmMain.BMS_ID;

            RealDataVM.Write_0x25(txtSOC.Text.Trim(), txtFullChgCapacity.Text.Trim(), txtSurplusCapacity.Text.Trim(), txtSOH.Text.Trim());
        }

        private void btnSetDatetime_Click(object sender, EventArgs e)
        {
            RealDataVM.SelectedRequest7 = FrmMain.BMS_ID;

            RealDataVM.Write_0x26(dateTimePicker1.Value);
        }

        private void btnSet0F0_2_Click(object sender, EventArgs e)
        {
            byte[] can_id = new byte[4] { 0xE0, FrmMain.BMS_ID, 0x20, 0x10 };

            byte[] data = new byte[8];
            foreach (Control item in this.groupBox1.Controls)
            {
                if (item is ComboBox)
                {
                    ComboBox cbb = item as ComboBox;
                    int index = 0;
                    int.TryParse(cbb.Name.Replace("cbbRequest20_", ""), out index);
                    int value = 0;
                    switch (cbb.SelectedIndex)
                    {
                        case 0: value = 0x00; break;
                        case 1: value = 0xAA; break;
                        case 2: value = 0x55; break;
                        default:
                            break;
                    }
                    data[index] = (byte)(value & 0xff);
                }
            }

            int flag = 0;
            if (ckSystemset_BMU_60.Checked) flag += 1;
            if (ckSystemset_BMU_61.Checked) flag += 2;
            if (ckSystemset_BMU_62.Checked) flag += 4;
            if (ckSystemset_BMU_63.Checked) flag += 8;
            data[6] = (byte)flag;

            byte crc8 = (byte)((0xE0 + FrmMain.BMS_ID + 0x20 + 0x10) & 0xFF);
            for (int i = 0; i < data.Length - 1; i++)
            {
                crc8 += data[i];
            }

            data[7] = (byte)(crc8 & 0xFF);
            if (ecanHelper.Send(data, can_id))
            {
                MessageBox.Show(FrmMain.GetString("keyWriteSuccess"));
            }
            else
            {
                MessageBox.Show(FrmMain.GetString("keyWriteFail"));
            }
        }
    }
}
