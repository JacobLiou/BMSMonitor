using Sofar.ConnectionLibs.CAN;
using SofarBMS.Helper;
using SofarBMS.Model;
using System.Diagnostics;
using System.Text;

namespace SofarBMS.UI
{
    public partial class CBSUpgradeControl : UserControl
    {
        EcanHelper ecanHelper = EcanHelper.Instance;

        //定义校验CRC帮助类对象
        private static Crc16 _crc = new Crc16(Crc16Model.CcittKermit);
        //定义固件升级任务信号源对象
        public static CancellationTokenSource cts = new CancellationTokenSource();
        private CancellationTokenSource _token = new CancellationTokenSource();
        //定义固件升级工步枚举对象
        public StepFlag stepFlag = StepFlag.None;
        //定义芯片角色和固件编码
        private string chip_role = "";
        private string chip_code = "0xE0";
        private string slaveAddress = "0x1F";//常规0x1F，BCU为0x9F

        Dictionary<uint, int> DevState = new Dictionary<uint, int>();
        HashSet<uint> DeviceList = new HashSet<uint>();
        List<byte[]> ResultList = new List<byte[]>();
        List<int> ErrorList = new List<int>();
        byte[] upgradeTime = null;
        //int Flag = 0; //当前标识：步骤1 FB /步骤2 FC + FD /步骤4 FE /步骤5 FF
        int GroupIndex = 0;
        int MAX_RETRY_COUNT = 5;
        int TX_INTERVAL_TIME = 20;
        int TX_INTERVAL_TIME_Data = 3;
        private bool state = false;
        public bool State
        {
            get { return state; }
            set
            {
                state = value;
                this.BeginInvoke(new Action(() =>
                {
                    if (state)
                    {
                        btnUpgrade_04.Text = LanguageHelper.GetLanguage("Upgrade_09");
                    }
                    else
                    {
                        btnUpgrade_04.Text = LanguageHelper.GetLanguage("Upgrade_04");
                    }
                }));
            }
        }
        int file_size = 0;
        int file_length = -1;
        string file_name = string.Empty;
        byte[] file_data;
        List<FirmwareModel> firmwares = new List<FirmwareModel>();

        public int sendErrorCount { get; set; } = 0;

        public CBSUpgradeControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBSUpgradeControl_Load(object sender, EventArgs e)
        {
            this.Invoke(() =>
            {
                foreach (Control item in this.Controls)
                {
                    GetControls(item);
                }

                cbbChiprole.SelectedIndex = 1;
                txtChipcode.Text = "S3";
            });

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

        /// <summary>
        /// 导入文件包
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpgrade_03_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.bin;*.sofar;*.tar)|*.bin;*.sofar;*.tar";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _token = new CancellationTokenSource();
                DevState = new Dictionary<uint, int>();
                DeviceList = new HashSet<uint>();
                ResultList = new List<byte[]>();
                ErrorList = new List<int>();
                upgradeTime = null;
                stepFlag = 0;
                GroupIndex = 0;
                file_size = 0;
                file_length = -1;
                file_name = string.Empty;
                file_data = null;
                firmwares = new List<FirmwareModel>();

                file_name = openFileDialog.FileName;
                using (FileStream bin_file = new FileStream(file_name, FileMode.Open))
                {
                    file_length = (int)bin_file.Length;

                    file_data = new byte[file_length];
                    for (int i = 0; i < file_data.Length; i++)
                    {
                        file_data[i] = 0x00;
                    }
                    bin_file.Read(file_data, 0, file_length);
                    //this.AnalysisBinchar(file_data);
                    this.AnalysisBin(file_data);
                }
                txtPath.Text = file_name;
            }
            else
            {
                txtPath.Text = "";
            }
        }

        /// <summary>
        /// 开始升级
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpgrade_04_Click(object sender, EventArgs e)
        {
            try
            {
                //0.检查CAN连接
                if (!ecanHelper.Connect())
                {
                    MessageBox.Show("串口未打开，请先连接设备...");
                    return;
                }
                //1.判断文件是否为空
                if (string.IsNullOrEmpty(txtPath.Text.Trim()))
                {
                    MessageBox.Show(LanguageHelper.GetLanguage("BMSUpgrade_ImportNull"), LanguageHelper.GetLanguage("BmsDebug_Tip"));
                    return;
                }

                //2.判断是否定时升级
                if (ckUpgrade_06.Checked)
                {
                    TimeSpan interval = dateTimePicker1.Value - DateTime.Now;
                    if (interval.Minutes < 5)
                    {
                        MessageBox.Show(LanguageHelper.GetLanguage("BMSUpgrade_DateError"), LanguageHelper.GetLanguage("BmsDebug_Tip"));
                        return;
                    }

                    string[] strTime = dateTimePicker1.Value.ToString("yy-MM-dd HH:mm:ss").Split('-', ':', ' ');
                    upgradeTime = new byte[6];
                    upgradeTime[0] = Convert.ToByte(strTime[0]);
                    upgradeTime[1] = Convert.ToByte(strTime[1]);
                    upgradeTime[2] = Convert.ToByte(strTime[2]);
                    upgradeTime[3] = Convert.ToByte(strTime[3]);
                    upgradeTime[4] = Convert.ToByte(strTime[4]);
                    upgradeTime[5] = Convert.ToByte(strTime[5]);
                }

                //3.判断当前升级状态
                if (State == false)
                {
                    _token = new CancellationTokenSource();
                    stepFlag = StepFlag.FB升级文件传输开始帧;
                    GroupIndex = 0;
                    DevState.Clear();
                    DeviceList.Clear();
                    State = true;
                    int.TryParse(txtFC.Text.Trim(), out TX_INTERVAL_TIME);
                    int.TryParse(txtFD.Text.Trim(), out TX_INTERVAL_TIME_Data);


                    var progress = new Progress<int>(percent =>
                                    {
                                        progressBar1.Value = percent;
                                        progressBar1.Text = $"{percent}%";
                                    });

                    Task.Run(() =>
                    {
                        int retryCount = 0;
                        do
                        {
                            if (_token.IsCancellationRequested)
                            {
                                this.Invoke(new Action(() => { progressBar1.Value = 0; }));
                                return;
                            }

                            switch (stepFlag)
                            {
                                case StepFlag.None:
                                    break;
                                case StepFlag.FB升级文件传输开始帧:
                                    Thread.Sleep(1000);//待优化

                                    if (DeviceList.Count == 0)
                                    {
                                        if (retryCount < MAX_RETRY_COUNT)
                                        {
                                            file_size = Convert.ToInt32(file_length / 1024) + Convert.ToInt32((file_length % 1024) != 0 ? 1 : 0);//upgradeModel.FileLength
                                            startDownloadFlag1(chip_role, chip_code, file_size, 1024);
                                            retryCount++;
                                        }
                                        else
                                        {
                                            this.Invoke(new Action(() =>
                                            {
                                                lblUpgrade_05.Text = LanguageHelper.GetLanguage("Response_Timed");
                                            }));
                                            _token.Cancel();
                                        }
                                    }
                                    else
                                    {
                                        stepFlag = StepFlag.FC升级数据块开始帧;

                                        this.Invoke(new Action(() =>
                                        {
                                            lblUpgrade_05.Text = LanguageHelper.GetLanguage("Upgrade_Start") + DeviceList.Count;
                                            lblUpgrade_05.ForeColor = System.Drawing.Color.Black;
                                        }));
                                    }
                                    break;
                                case StepFlag.FC升级数据块开始帧:
                                case StepFlag.FD升级数据块数据帧:
                                    startDownloadPack2(GroupIndex, 1024);
                                    //5Thread.Sleep(TX_INTERVAL_TIME);
                                    AddLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), "FC", "PACK_ID" + GroupIndex);
                                    int offset = GroupIndex * 1024;
                                    for (int i = 0; i < 1024; i += 8)
                                    {
                                        Thread.Sleep(TX_INTERVAL_TIME_Data);
                                        startDownloadData3(offset + i);
                                    }
                                    Thread.Sleep(TX_INTERVAL_TIME);
                                    if (GroupIndex == file_size)
                                    {
                                        stepFlag = StepFlag.FE升级文件接收结果查询帧;
                                    }
                                    else
                                    {
                                        GroupIndex++;
                                    }

                                    decimal proVal = ((decimal)GroupIndex / file_size) * 100;
                                    ((IProgress<int>)progress).Report(Convert.ToInt32(proVal));
                                    //this.BeginInvoke(new Action(() =>
                                    //{
                                    //    progressBar1.Maximum = file_size;
                                    //    progressBar1.Value = GroupIndex;

                                    //    decimal proVal = ((decimal)GroupIndex / file_size) * 100;
                                    //    progressBar1.Text = $"正在升级，当前进度为：{Convert.ToInt32(proVal)}%";
                                    //}));

                                    break;
                                case StepFlag.FE升级文件接收结果查询帧:
                                    do
                                    {
                                        ErrorList.Clear();
                                        ResultList.Clear();
                                        Thread.Sleep(5000);

                                        if (ResultList.Count >= DeviceList.Count)
                                        {
                                            int[][] resultArray = new int[ResultList.Count][];
                                            for (int i = 0; i < ResultList.Count; i++)
                                            {
                                                byte[] rec = ResultList[i];
                                                int[] error = Check(rec).ToArray();
                                                resultArray[i] = error;
                                            }
                                            for (int i = 0; i < resultArray.Length; ++i)
                                            {
                                                foreach (int j in resultArray[i])
                                                {
                                                    if (!ErrorList.Contains(j))
                                                        ErrorList.Add(j);
                                                }
                                            }
                                            for (int i = 0; i < ErrorList.Count; i++)
                                            {
                                                startDownloadPack2(ErrorList[i] - 1, 1024);
                                                //Thread.Sleep(TX_INTERVAL_TIME);

                                                GroupIndex = ErrorList[i] % 24 == 0 ? ErrorList[i] / 24 - 1 : ErrorList[i] / 24;
                                                AddLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), "FE", $"等{GroupIndex}组异常，Pack为{ErrorList[i]} ");
                                                offset = (ErrorList[i] - 1) * 1024;

                                                for (int j = 0; j < 1024; j += 8)
                                                {
                                                    Thread.Sleep(TX_INTERVAL_TIME_Data);
                                                    startDownloadData3(offset + j);
                                                }
                                                Thread.Sleep(TX_INTERVAL_TIME);
                                            }
                                        }
                                        else
                                        {
                                            this.Invoke(new Action(() =>
                                           {
                                               AddLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), "FE", "PACK_ID" + GroupIndex);
                                               startDownloadCheck4(chip_role, chip_code, file_data);
                                           }));
                                        }
                                    } while (ErrorList.Count != 0 || ResultList.Count < DeviceList.Count);

                                    if (ckUpgrade_06.Checked && upgradeTime != null)
                                    {
                                        startDownloadFlag6(upgradeTime);
                                        Thread.Sleep(TX_INTERVAL_TIME);

                                        startDownloadState5(chip_role, chip_code, 03, 0x80);
                                    }
                                    else
                                    {
                                        startDownloadState5(chip_role, chip_code, 02, 0x80);
                                    }

                                    stepFlag = StepFlag.FF升级完成状态查询帧;
                                    break;
                                case StepFlag.FF升级完成状态查询帧:

                                    break;
                                default:
                                    break;
                            }
                        } while (stepFlag != StepFlag.FF升级完成状态查询帧);

                    }, _token.Token);

                    ////注册一个委托：这个委托将任务取消的时候调用
                    //cts.Token.Register(() =>
                    //{
                    //    State = false;
                    //    stepFlag = 0;
                    //});
                }
                else
                {
                    DeviceList.Clear();
                    DevState.Clear();
                    _token.Cancel();

                    lblUpgrade_05.Text = "";
                    progressBar1.Value = 0;
                    stepFlag = 0;
                    State = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss:ffff") + " 出现异常，错误信息：" + ex.Message);
            }
        }

        #region BIN文件处理-1K
        /// <summary>
        /// 导入Bin文件处理-1K
        /// </summary>
        /// <param name="binchar"></param>
        private void AnalysisBin(byte[] binchar)
        {
            ////清空旧数据
            //firmwareModels_bms.Clear();
            //FirmwareModel_BMS1500V_DataList.Clear();
            FirmwareModel_BMS1500V firmwareModel = new FirmwareModel_BMS1500V();
            //签名信息字节大小
            int SIGNATURE_SIZE = 1024;

            //签名信息字节数组
            byte[] SignatureBytes = new byte[SIGNATURE_SIZE];

            // 读取签名信息
            Array.Copy(binchar, binchar.Length - 1024, SignatureBytes, 0, 1024);

            //文件长度
            firmwareModel.FirmwareSize = (uint)(SignatureBytes[(1) + 0] |
                                       SignatureBytes[(1) + 1] << 8 |
                                       SignatureBytes[(1) + 2] << 16 |
                                       SignatureBytes[(1) + 3] << 24);
            //有效字节CRC
            uint Crc32 = (uint)(SignatureBytes[(1 + 4) + 0] |
                        SignatureBytes[(1 + 4) + 1] << 8 |
                        SignatureBytes[(1 + 4) + 2] << 16 |
                        SignatureBytes[(1 + 4) + 3] << 24);

            //芯片型号
            firmwareModel.ChipModel = Encoding.ASCII.GetString(SignatureBytes, (1 + 4 + 4), 30).Trim('\0');

            //软件版本
            firmwareModel.SoftwareVersion = Encoding.ASCII.GetString(SignatureBytes, (1 + 4 + 4 + 30), 20).Trim('\0');

            //硬件版本
            firmwareModel.HardwareVersion = Encoding.ASCII.GetString(SignatureBytes, (1 + 4 + 4 + 30 + 20), 20).Trim('\0');

            //工程名称
            firmwareModel.ProjectName = Encoding.ASCII.GetString(SignatureBytes, (1 + 4 + 4 + 30 + 20 + 20), 30).Trim('\0');

            firmwareModel.dateTimeString = AnalysisTime(SignatureBytes);


            //程序起始地址
            uint ProgramOffset = (uint)(SignatureBytes[(1 + 4 + 4 + 30 + 20 + 20 + 30 + 12) + 0] |
                          SignatureBytes[(1 + 4 + 4 + 30 + 20 + 20 + 30 + 12) + 1] << 8 |
                          SignatureBytes[(1 + 4 + 4 + 30 + 20 + 20 + 30 + 12) + 2] << 16 |
                          SignatureBytes[(1 + 4 + 4 + 30 + 20 + 20 + 30 + 12) + 3] << 24);


            // 芯片角色代号
            firmwareModel.FirmwareChipRoleCode = SignatureBytes[(1 + 4 + 4 + 30 + 20 + 20 + 30 + 12 + 4)];

            // 芯片角色
            firmwareModel.FirmwareChipRole = Enum.Parse(typeof(ChipRole), firmwareModel.FirmwareChipRoleCode.ToString()).ToString() + $"(0x{firmwareModel.FirmwareChipRoleCode:X2})";

            //文件类型代号
            string fileType = "0x" + SignatureBytes[(1 + 4 + 4 + 30 + 20 + 20 + 30 + 12 + 4 + 1 + 2)].ToString("X2");
            firmwareModel.FirmwareFileTypeCode = Convert.ToByte(fileType, 16);
            //文件类型
            firmwareModel.FirmwareFileType = Enum.Parse(typeof(FileType), (Convert.ToInt32(fileType, 16) & 0x0f).ToString()).ToString() + $"({fileType})";

            //芯片代号
            firmwareModel.ChipMark = Encoding.ASCII.GetString(SignatureBytes, (1 + 4 + 4 + 30 + 20 + 20 + 30 + 12 + 4 + 1), 2).Trim('\0');

            //整体(固件 + 签名)CRC32
            uint Crc32WithSignature = (uint)(SignatureBytes[SignatureBytes.Length - 4] |
                          SignatureBytes[SignatureBytes.Length - 3] << 8 |
                          SignatureBytes[SignatureBytes.Length - 2] << 16 |
                          SignatureBytes[SignatureBytes.Length - 1] << 24);

            txtChipcode.Text = firmwareModel.ChipMark;
            txtChiprole_val.Text = "0x" + firmwareModel.FirmwareChipRoleCode.ToString("X2");
            switch (txtChiprole_val.Text.Trim())
            {
                case "0x24":
                    cbbChiprole.Text = "BCU";
                    txtSlaveAddress.Text = slaveAddress = "0x9F";
                    break;
                case "0x25":
                    cbbChiprole.Text = "PCU";
                    txtSlaveAddress.Text = slaveAddress = "0x9F";
                    break;
                case "0x2D":
                    cbbChiprole.Text = "BMU";
                    txtSlaveAddress.Text = slaveAddress = "0x1F";
                    break;
                default:
                    break;
            }
            //firmwareModels.Add(firmwareModel);
        }

        public string AnalysisTime(byte[] binchar)
        {
            string TimeStampString = Encoding.ASCII.GetString(binchar, (1 + 4 + 4 + 30 + 20 + 20 + 30), 12).Trim('\0');

            int ParseTimeComponent(string timeString, int startIndex, int length, int minValue, int maxValue, string componentName)
            {
                if (!int.TryParse(timeString.Substring(startIndex, length), out int value) || value < minValue || value > maxValue)
                    throw new FormatException($"无法转换{componentName}部分");
                return value;
            }

            try
            {
                int year = ParseTimeComponent(TimeStampString, 0, 2, 0, 99, "年份") + 2000;
                int month = ParseTimeComponent(TimeStampString, 2, 2, 1, 12, "月份");
                int day = ParseTimeComponent(TimeStampString, 4, 2, 1, 31, "日期");
                int hour = ParseTimeComponent(TimeStampString, 6, 2, 0, 23, "小时");
                int minute = ParseTimeComponent(TimeStampString, 8, 2, 0, 59, "分钟");
                int second = ParseTimeComponent(TimeStampString, 10, 2, 0, 59, "秒");

                DateTime dateTime = new DateTime(year, month, day, hour, minute, second);
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (FormatException ex)
            {
                //throw new FormatException($"文件创建时间解析失败: {ex.Message}");
                return "";
            }
        }

        #endregion

        /// <summary>
        /// 导入SOFAR文件处理-2K
        /// </summary>
        /// <param name="binchar"></param>
        private void AnalysisBinchar(byte[] binchar)
        {
            firmwares.Clear();

            string firmwarePackName = Encoding.ASCII.GetString(binchar.Skip(binchar.Length - 2048 + 33).Take(32).ToArray());
            //lblFirmwarePackName.Text = "固件包名称 : " + firmwarePackName;

            byte[] dateBytes = binchar.Skip(binchar.Length - 2048 + 65).Take(6).ToArray();
            String targetDt = "20" + Convert.ToInt32(dateBytes[0]).ToString("00") + "-" + Convert.ToInt32(dateBytes[1]).ToString("00") + "-" +
                Convert.ToInt32(dateBytes[2]).ToString("00") + " " + Convert.ToInt32(dateBytes[3]).ToString("00") + ":" +
                Convert.ToInt32(dateBytes[4]).ToString("00") + ":" + Convert.ToInt32(dateBytes[5]).ToString("00");
            //lblFirmwarePackDate.Text = "固件包生产日期 : " + targetDt;

            //升级选中固件配置
            int byte1 = binchar[binchar.Length - 2048 + 72] << 8;
            int byte2 = binchar[binchar.Length - 2048 + 73] & 0xff;
            int checkValue = byte1 + byte2;

            //固件模块数量
            int count = binchar[binchar.Length - 2048 + 71];
            byte[] firmwareBytes = binchar.Skip(binchar.Length - 2048 + 138).Take(68 * count).ToArray();

            //固件状态
            int k = 0;
            for (int i = 0; i < firmwareBytes.Length; i += 68)
            {
                FirmwareModel firmwareModel = new FirmwareModel();
                firmwareModel.FirmwareType = Encoding.ASCII.GetString(firmwareBytes.Skip(i).Take(8).ToArray());
                firmwareModel.FirmwareName = Encoding.ASCII.GetString(firmwareBytes.Skip(i + 8).Take(32).ToArray());
                firmwareModel.StartAddress = BitConverter.ToInt32(firmwareBytes.Skip(i + 8 + 32).Take(4).ToArray(), 0);
                firmwareModel.Length = BitConverter.ToInt32(firmwareBytes.Skip(i + 8 + 32 + 4).Take(4).ToArray(), 0);
                firmwareModel.CheckFlg = (checkValue & (1 << k)) == (1 << k);
                k++;
                firmwares.Add(firmwareModel);
            }
        }

        /// <summary>
        /// 解析数据-出队
        /// </summary>
        /// <param name="obj_ID"></param>
        /// <param name="data"></param>
        private void AnalysisData(uint obj_ID, byte[] data)
        {
            uint id = obj_ID | 0xff;

            switch (stepFlag)
            {
                case StepFlag.None:
                    break;
                case StepFlag.FB升级文件传输开始帧:
                    if (data[0] == 0x01 && data[1] == 0x01 && (id == 0x07FB41FF || id == 0x07FBE0FF))
                        DeviceList.Add(obj_ID);
                    break;
                case StepFlag.FC升级数据块开始帧:
                case StepFlag.FD升级数据块数据帧:
                    break;
                case StepFlag.FE升级文件接收结果查询帧:
                    if (data[0] == 0x01 && (id == 0x07FE41ff || id == 0x07FEE0ff))
                        ResultList.Add(data);
                    break;
                case StepFlag.FF升级完成状态查询帧:
                    if (data[0] == 0x01 && (id == 0x07FF41FF || id == 0x07FF5FFF || id == 0x07FFE0FF))
                    {
                        switch (data[2])
                        {
                            case 0x00://升级成功
                                this.Invoke(new Action(() =>
                                {
                                    if (data[1] == 0x00 && !DevState.ContainsKey(obj_ID))//避免报错，先判断是否包含Key值
                                    {
                                        DevState.Add(obj_ID, 1);
                                        AddLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), obj_ID.ToString("X8"), LanguageHelper.GetLanguage("Upgrade_Success"));
                                    }
                                    else
                                    {
                                        AddLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), obj_ID.ToString("X8"), LanguageHelper.GetLanguage("Upgrade_Error"));
                                    }

                                    if (DeviceList.Count <= DevState.Count)
                                    {
                                        int totalSussces = 0;
                                        foreach (var item in DevState)
                                        {
                                            if (item.Value == 1)
                                                totalSussces++;
                                        }
                                        lblUpgrade_05.Text = $"{LanguageHelper.GetLanguage("Upgrade_Result")}" + totalSussces;
                                        lblUpgrade_05.ForeColor = System.Drawing.Color.Green;

                                        stepFlag = StepFlag.None;
                                        State = false;
                                        _token.Cancel();
                                    }
                                }));
                                break;
                            case 0x01://暂存成功
                                this.BeginInvoke(new Action(() =>
                                {
                                    AddLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), obj_ID.ToString("X8"), LanguageHelper.GetLanguage("Stag_Success"));
                                }));
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 升级文件传输开始帧
        /// </summary>
        /// <param name="chip_role">0:ARM 1:DSP_M 2:DSP_S 3:BMS</param>
        /// <param name="chip_code">芯片编码</param>
        /// <param name="file_size">文件数据块总数=文件大小/数据块的大小+1</param>
        /// <param name="data_size">默认1024Byte</param>
        /// <exception cref="Exception"></exception>
        public void startDownloadFlag1(string chip_role, string chip_code, int file_size, int data_size)
        {
            int i = 0;
            byte[] data = new byte[8];
            try
            {
                data[i++] = 0x00;//发送请求帧
                data[i++] = Convert.ToByte(chip_role, 16);
                data[i++] = ASCIIEncoding.Default.GetBytes(chip_code)[1];
                data[i++] = ASCIIEncoding.Default.GetBytes(chip_code)[0];
                data[i++] = Convert.ToByte(file_size & 0xff);
                data[i++] = Convert.ToByte(file_size >> 8);
                data[i++] = Convert.ToByte(data_size & 0xff);
                data[i++] = Convert.ToByte(data_size >> 8);

                AssembleInstruction(0xFB, data);
            }
            catch (Exception ex)
            {
                throw new Exception("升级文件传输开始帧0x7FB，ERROR：" + ex.Message);
            }
        }

        /// <summary>
        /// 升级数据块开始帧
        /// </summary>
        /// <param name="serial_number">当前传输的数据块的序号(0开始)</param>
        /// <param name="data_size">数据块的大小</param>
        /// <exception cref="Exception"></exception>
        public void startDownloadPack2(int serial_number, int data_size)
        {
            int i = 0;
            byte[] data = new byte[8];
            try
            {
                byte[] bytes = new byte[1024];
                readBinFile(serial_number * 1024, ref bytes);
                byte[] hexCRC = _crc.ComputeChecksumBytes(bytes);//数据块校验值，序号总CRC计算

                data[i++] = 0x00;//发送请求帧
                data[i++] = 0x00;//预留
                data[i++] = Convert.ToByte(serial_number & 0xff);
                data[i++] = Convert.ToByte(serial_number >> 8);
                data[i++] = Convert.ToByte(data_size & 0xff);
                data[i++] = Convert.ToByte(data_size >> 8);
                data[i++] = hexCRC[0];
                data[i++] = hexCRC[1];

                AssembleInstruction(0xFC, data);
            }
            catch (Exception ex)
            {
                throw new Exception("升级数据块开始帧0xFC，ERROR：" + ex.Message);
            }
        }

        /// <summary>
        /// 升级数据块数据帧
        /// </summary>
        /// <param name="offset">文件数据</param>
        /// <exception cref="Exception"></exception>
        public void startDownloadData3(int offset)
        {
            byte[] data = new byte[8];
            try
            {
                readBinFile(offset, ref data);//文件数据

                AssembleInstruction(0xFD, data);
            }
            catch (Exception ex)
            {
                throw new Exception("升级数据块数据帧0xFD，ERROR：" + ex.Message);
            }
        }

        /// <summary>
        /// 升级文件接收结果查询帧
        /// </summary>
        /// <param name="chip_role">0:ARM 1:DSP_M 2:DSP_S 3:BMS</param>
        /// <param name="chip_code">芯片编码</param>
        /// <param name="file_buffer"></param>
        /// <exception cref="Exception"></exception>
        public void startDownloadCheck4(string chip_role, string chip_code, byte[] file_buffer)
        {
            int i = 0;
            byte[] data = new byte[8];
            try
            {
                byte[] hexCRC = _crc.ComputeChecksumBytes(file_buffer);//序号总CRC计算

                data[i++] = 0x00;//发送请求帧
                data[i++] = Convert.ToByte(chip_role, 16);
                data[i++] = ASCIIEncoding.Default.GetBytes(chip_code)[1];
                data[i++] = ASCIIEncoding.Default.GetBytes(chip_code)[0];
                data[i++] = hexCRC[0];
                data[i++] = hexCRC[1];
                data[i++] = 0xAA;

                AssembleInstruction(0xFE, data);
            }
            catch (Exception ex)
            {
                throw new Exception("升级文件接收结果查询帧0xFE，ERROR：" + ex.Message);
            }
        }

        /// <summary>
        /// 升级完成状态查询帧
        /// </summary>
        /// <param name="chip_role">0:ARM 1:DSP_M 2:DSP_S 3:BMS</param>
        /// <param name="chip_code">芯片编码</param>
        /// <param name="function_code">01:查询 02:启动升级 03:暂存升级</param>
        /// <param name="file_type">00:APP 01:CORE</param>
        /// <exception cref="Exception"></exception>
        public void startDownloadState5(string chip_role, string chip_code, int function_code, int file_type = 0x00)
        {
            int i = 0;
            byte[] data = new byte[8];
            try
            {
                data[i++] = 0x00;//发送请求帧
                data[i++] = Convert.ToByte(chip_role, 16);
                data[i++] = ASCIIEncoding.Default.GetBytes(chip_code)[1];
                data[i++] = ASCIIEncoding.Default.GetBytes(chip_code)[0];
                data[i++] = Convert.ToByte(function_code);
                data[i++] = Convert.ToByte(file_type);

                AssembleInstruction(0xFF, data);
            }
            catch (Exception ex)
            {
                throw new Exception("升级完成状态查询帧0xFF，ERROR：" + ex.Message);
            }
        }

        /// <summary>
        /// 升级定时标识帧
        /// </summary>
        /// <param name="strDate">时间数据</param>
        /// <exception cref="Exception"></exception>
        public void startDownloadFlag6(byte[] strDate)
        {
            byte[] data = new byte[8];
            try
            {
                Buffer.BlockCopy(strDate, 0, data, 0, strDate.Length);

                AssembleInstruction(0xFA, data);
            }
            catch (Exception ex)
            {
                throw new Exception("升级数据块数据帧0xFD，ERROR：" + ex.Message);
            }
        }

        /// <summary>
        /// 获取Bin文件数据
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool readBinFile(int offset, ref byte[] data)
        {
            try
            {
                var temp = file_data.Skip(offset).Take(data.Length).ToArray();
                data = temp;
                return true;
            }
            catch (Exception ex)
            {
                Debug.Print("readBinFile异常:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 升级指令组装函数
        /// </summary>
        /// <param name="mark"></param>
        /// <param name="data"></param>
        private void AssembleInstruction(byte mark, byte[] data)
        {
            try
            {
                byte[] canid = new byte[] { 0xE0, Convert.ToByte(slaveAddress, 16), mark, 0x07 };

                //增加判断，确认是否发送成功；
                bool result = ecanHelper.Send(data, canid);
                if (!result)
                {
                    sendErrorCount++;
                    if (sendErrorCount >= 10)
                    {
                        //State = false;
                        //cts.Cancel();
                        //DevState.Clear();
                        //DeviceList.Clear();

                        this.Invoke(new Action(() =>
                        {
                            //lblUpgrade_05.Text = "升级失败，无法下载数据。请检查通讯重新连接！";
                            lblUpgrade_05.Text = "发送失败，请检查通讯重新连接！";
                            lblUpgrade_05.ForeColor = System.Drawing.Color.Red;
                            //progressBar1.Value = 0;
                        }));
                        Thread.Sleep(1000 * 10);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "下发数据异常：" + ex.Message);
            }
        }

        /// <summary>
        /// 检查Bit为1的错误数据
        /// </summary>
        /// <param name="responsedata">Byte数组</param>
        /// <returns></returns>
        public List<int> Check(byte[] responsedata)
        {
            List<int> lists = new List<int>();
            byte[] table = { 0x01, 0x02, 0x04, 0x8, 0x10, 0x20, 0x40, 0x80 };
            int cnt = 0;

            for (int i = 5; i < responsedata.Length; i++)
            {
                if (responsedata[i] != 0x00)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if ((table[j] & responsedata[i]) == table[j])
                        {
                            lists.Add(j + cnt + 1 + (responsedata[4] * 24));
                        }
                    }
                }
                cnt += 8;
            }
            return lists;
        }

        /// <summary>
        /// 打印日志
        /// </summary>
        /// <param name="date"></param>
        /// <param name="id"></param>
        /// <param name="context"></param>
        private void AddLog(string date, string id, string context)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Text = date;
            lvi.SubItems.Add(id);
            lvi.SubItems.Add(context);
            this.Invoke(new Action(() =>
            {
                this.listView1.Items.Insert(0, lvi);
            }));
        }

        /// <summary>
        /// 多语言翻译
        /// </summary>
        /// <param name="c"></param>
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

        private void LTooltip(Control control, string value)
        {
            if (value.Length == 0) return;
            control.Text = Abbreviation(control, value);
            var tip = new ToolTip();
            tip.IsBalloon = false;
            tip.ShowAlways = true;
            tip.SetToolTip(control, value);
        }

        private string Abbreviation(Control control, string str)
        {
            if (str == null)
            {
                return null;
            }
            int strWidth = FontWidth(control.Font, control, str);
            int len = control.Width * str.Length / strWidth;
            if (len > 3 && len < str.Length)
                return str.Substring(0, len - 3) + "...";
            else
                return str;
        }

        private int FontWidth(Font font, Control control, string str)
        {
            using (Graphics g = control.CreateGraphics())
            {
                SizeF siF = g.MeasureString(str, font);
                return (int)siF.Width;
            }
        }


        private void cbbChiprole_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Chiprole_val被导入文件时，识别芯片所取代（把芯片类型做成自动设别）
            switch (cbbChiprole.SelectedIndex)
            {
                case 0:
                case 2:
                    //txtChiprole_val.Text = "0x24";
                    txtSlaveAddress.Text = slaveAddress = "0x9F";
                    break;
                case 1:
                    //txtChiprole_val.Text = "0x2D";
                    txtSlaveAddress.Text = slaveAddress = "0x1F";
                    break;
                default:
                    break;
            }
        }

        private void cbbChipcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //chip_code = txtChipcode.Text.Trim();
            //slaveAddress = txtSlaveAddress.Text;
        }

        private void cbbChiprole_val_SelectedIndexChanged(object sender, EventArgs e)
        {
            //chip_role = cbbChiprole_val.Text;
        }

        private void txtChipcode_TextChanged(object sender, EventArgs e)
        {
            chip_code = txtChipcode.Text.Trim();
            slaveAddress = txtSlaveAddress.Text;
        }

        private void txtChiprole_val_TextChanged(object sender, EventArgs e)
        {
            chip_role = txtChiprole_val.Text;
        }
    }

    public enum StepFlag
    {
        None = 0,
        FB升级文件传输开始帧 = 1,
        FC升级数据块开始帧 = 2,
        FD升级数据块数据帧 = 3,
        FE升级文件接收结果查询帧 = 4,
        FF升级完成状态查询帧 = 5
    }

    public enum FileType : byte
    {
        app = 0x00,
        core = 0x01,
        kernel = 0x02,
        rootfs = 0x03,
        safety = 0x04,
        pack = 0x80,
    }

    public enum ChipRole : byte
    {
        ARM = 0x21,
        DSPM = 0x22,
        DSPS = 0x23,
        BMS_BCU = 0x24,
        PCU = 0x25,
        BDU = 0x26,
        DCDC = 0x27,
        DCDC_ARM = 0x28,
        DCDC_DSP = 0x29,
        BMU = 0x2D,
        PCS_M = 0x30,
        PCS_S = 0x31,
        PCS_CPLD = 0x32,
        CSU_MCU1 = 0x33,
        CSU_MCU2 = 0x34,
        DCDC_M = 0x38,
        DCDC_S = 0x39,
        DCDC_CPLD = 0x3A,
        CMU_MCU1 = 0x3B,
        CMU_MCU2 = 0x3C,
        FUSE = 0x41,
        AFCI = 0x42,
        MPPT = 0x43,
        PID = 0x44,
        WIFI = 0x61,
        BLE = 0x62,
        PLC_CCO = 0x63,
        PLC_STA = 0x64,
        SUB1G_GW = 0x65,
        SUB1G_STA = 0x66,
        HUB_MCU1 = 0x67,
        HUB_MCU2 = 0x68,
        CSU = 0x80,
        TFC = 0x81,
        SSMGFD = 0x82,
        COM = 0x88,
        D2D = 0x90,
        PFC = 0x91,
    }
}
