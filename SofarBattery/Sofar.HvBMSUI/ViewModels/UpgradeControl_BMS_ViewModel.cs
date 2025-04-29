using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Win32;
using PowerKit.UI.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using PowerKit.UI.Models;
using Sofar.ProtocolLib;
using System.Globalization;
using System.Collections;
using Sofar.BMSLib;
using Sofar.BMSUI.Models;
using Sofar.BMSUI;
using Sofar.BMSUI.Common;
using System.Windows.Media;

namespace Sofar.HvBMSUI.ViewModels
{
    public class UpgradeControl_BMS_ViewModel : ObservableObject
    {

        /// <summary>
        /// 时
        /// </summary>
        public ObservableCollection<string> HourList { get; } = new ObservableCollection<string>(Enumerable.Range(0, 24).Select(x => x.ToString("D2")));

        /// <summary>
        /// 分
        /// </summary>
        public ObservableCollection<string> MinuteList { get; } = new ObservableCollection<string>(Enumerable.Range(0, 60).Select(x => x.ToString("D2")));

        /// <summary>
        /// 秒
        /// </summary>
        public ObservableCollection<string> SecondList { get; } = new ObservableCollection<string>(Enumerable.Range(0, 60).Select(x => x.ToString("D2")));


        // 升级日志数据列表
        private ObservableCollection<UpgradeLogData> _upgradeLogList;
        public ObservableCollection<UpgradeLogData> UpgradeLogList
        {
            get { return _upgradeLogList; }
            set
            {
                _upgradeLogList = value;
                OnPropertyChanged(nameof(UpgradeLogList));
            }
        }

        // 芯片角色列表
        public ObservableCollection<string> ChiproleList { get; } = new ObservableCollection<string>
        {
            "BCU",
            "BMU"
        };

        private string _selectedChiprole = "BMU";
        //芯片角色  BCU/BMU
        public string SelectedChiprole
        {
            get { return _selectedChiprole; }
            set
            {
                if (_selectedChiprole != value)
                {
                    _selectedChiprole = value;
                    OnPropertyChanged(nameof(SelectedChiprole));
                    ChipRole_SelectedIndexChanged(value); // 处理选项变化
                }
            }
        }
        // 芯片角色编码列表
        public ObservableCollection<string> Chiprole_val_List { get; } = new ObservableCollection<string>
        {
            "0x24",
            "0x2D"
        };

        private string _selectedChiprole_val = "0x2D";
        public string SelectedChiprole_val
        {
            get { return _selectedChiprole_val; }
            set
            {
                if (_selectedChiprole_val != value)
                {
                    _selectedChiprole_val = value;
                    OnPropertyChanged(nameof(SelectedChiprole_val));
                    Chiprole_val_SelectedIndexChanged(value); // 处理选项变化
                }
            }
        }

        private string _slaveAddress = "0x1F";
        public string SlaveAddress
        {
            get { return _slaveAddress; }
            set { _slaveAddress = value; OnPropertyChanged(); }
        }

        // 电池芯片编码列表
        public ObservableCollection<string> ChipcodeList { get; } = new ObservableCollection<string>
        {
            "E0",
            "S3",
            "N2"
        };

        private string _selectedChipcode;
        public string SelectedChipcode
        {
            get { return _selectedChipcode; }
            set
            {
                if (_selectedChipcode != value)
                {
                    _selectedChipcode = value;
                    OnPropertyChanged(nameof(SelectedChipcode));
                    Chipcode_SelectedIndexChanged(value); // 处理选项变化
                }
            }
        }



        private ObservableCollection<FirmwareModel_BMS1500V> _FirmwareModel_BMS1500V_DataList;

        public ObservableCollection<FirmwareModel_BMS1500V> FirmwareModel_BMS1500V_DataList
        {
            get => _FirmwareModel_BMS1500V_DataList;
            set
            {
                _FirmwareModel_BMS1500V_DataList = value;
                OnPropertyChanged(nameof(FirmwareModel_BMS1500V_DataList)); // 通知界面更新
            }
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = value; OnPropertyChanged(); }
        }

        private string _selectedHour;
        public string SelectedHour
        {
            get { return _selectedHour; }
            set { _selectedHour = value; OnPropertyChanged(); }
        }

        private string _selectedMinute;
        public string SelectedMinute
        {
            get { return _selectedMinute; }
            set { _selectedMinute = value; OnPropertyChanged(); }
        }

        private string _selectedSecond;
        public string SelectedSecond
        {
            get { return _selectedSecond; }
            set { _selectedSecond = value; OnPropertyChanged(); }
        }

        private string _filePath;
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; OnPropertyChanged(); }
        }
        private string _upgradeState = "开始升级";
        /// <summary>
        /// 升级按钮状态
        /// </summary>
        public string UpgradeState
        {
            get { return _upgradeState; }
            set { _upgradeState = value; OnPropertyChanged(); }
        }
        private bool state = false;
        /// <summary>
        /// 升级状态
        /// </summary>
        public bool State
        {
            get { return state; }
            set
            {
                state = value;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (state)
                    {
                        UpgradeState = "结束升级";
                    }
                    else
                    {
                        UpgradeState = "开始升级";
                    }
                });
            }
        }

        private string _FC = "20";
        public string FC
        {
            get { return _FC; }
            set { _FC = value; OnPropertyChanged(); }
        }
        private string _FD = "3";
        public string FD
        {
            get { return _FD; }
            set { _FD = value; OnPropertyChanged(); }
        }

        private int _upgradeProgressBar;
        /// <summary>
        /// 升级进度条当前数值
        /// </summary>
        public int UpgradeProgressBar
        {
            get { return _upgradeProgressBar; }
            set { _upgradeProgressBar = value; OnPropertyChanged(); }
        }

        private int _upgradeProgressValue;
        /// <summary>
        /// 升级进度条百分数值
        /// </summary>
        public int UpgradeProgressValue
        {
            get { return _upgradeProgressValue; }
            set { _upgradeProgressValue = value; OnPropertyChanged(); }
        }

        private int _upgradeProgressMax;
        /// <summary>
        /// 升级进度条最大值
        /// </summary>
        public int UpgradeProgressMax
        {
            get { return _upgradeProgressMax; }
            set { _upgradeProgressMax = value; OnPropertyChanged(); }
        }



        private string _upgradeTips = "提示:";
        /// <summary>
        /// 升级提示
        /// </summary>
        public string UpgradeTips
        {
            get { return _upgradeTips; }
            set { _upgradeTips = value; OnPropertyChanged(); }
        }

        private Brush _upgradeTipsTextColor = Brushes.Black;
        /// <summary>
        /// 升级提示文本颜色
        /// </summary>
        public Brush UpgradeTipsTextColor
        {
            get { return _upgradeTipsTextColor; }
            set { _upgradeTipsTextColor = value; OnPropertyChanged(); }
        }


        private bool _isStartScheduledUpgrade_Checked;
        public bool IsStartScheduledUpgrade_Checked
        {
            get { return _isStartScheduledUpgrade_Checked; }
            set { _isStartScheduledUpgrade_Checked = value; OnPropertyChanged(); }
        }

        BaseCanHelper baseCanHelper = null;
        //定义校验CRC帮助类对象
        private static Crc16 _crc = new Crc16(Crc16Model.CcittKermit);
        //定义固件升级任务信号源对象
        public CancellationTokenSource cts = null;
        private CancellationTokenSource _token = new CancellationTokenSource();
        //定义固件升级工步枚举对象
        public StepFlag stepFlag = StepFlag.None;
        //定义芯片角色和固件编码
        private string chip_role = "0x2D";
        private string chip_code = "E0";
        private string slaveAddress = "0x1F";//BMU为0x1F，BCU为0xFF

        Dictionary<uint, int> DevState = new Dictionary<uint, int>();
        HashSet<uint> DeviceList = new HashSet<uint>();
        List<byte[]> ResultList = new List<byte[]>();
        List<int> ErrorList = new List<int>();
        byte[] upgradeTime = null;
        //int Flag = 0; //当前标识：步骤1 FB /步骤2 FC + FD /步骤4 FE /步骤5 FF
        int GroupIndex = 0;
        int MAX_RETRY_COUNT = 5;
        int TX_INTERVAL_TIME = 200;
        int TX_INTERVAL_TIME_Data = 3;

        int file_size = 0;
        int file_length = -1;
        string file_name = string.Empty;
        byte[] file_data;
        //List<FirmwareModel> firmwares = new List<FirmwareModel>();
        List<FirmwareModel> firmwareModels = new List<FirmwareModel>();
        List<FirmwareModel_BMS1500V> firmwareModels_bms = new List<FirmwareModel_BMS1500V>();
        public int sendErrorCount { get; set; } = 0;
        public UpgradeControl_BMS_ViewModel()
        {
            baseCanHelper = new CommandOperation(BMSConfig.ConfigManager).baseCanHelper;

            cts = new CancellationTokenSource();
            if (UpgradeLogList == null) UpgradeLogList = new ObservableCollection<UpgradeLogData>();
            if (FirmwareModel_BMS1500V_DataList == null) FirmwareModel_BMS1500V_DataList = new ObservableCollection<FirmwareModel_BMS1500V>();
        }
        public void Load()
        {
            Task.Run(() =>
            {
                if (baseCanHelper.CommunicationType == "Ecan")
                {
                    while (!cts.IsCancellationRequested)
                    {
                        lock (EcanHelper._locker)
                        {
                            while (EcanHelper._task.Count > 0
                                && !_token.IsCancellationRequested)
                            {
                                //出队
                                CAN_OBJ ch = (CAN_OBJ)EcanHelper._task.Dequeue();

                                //解析
                                Application.Current.Dispatcher.Invoke(() => { AnalysisData(ch.ID, ch.Data); });
                            }
                        }
                    }
                }
                else
                {
                    while (!cts.IsCancellationRequested)
                    {
                        lock (ControlcanHelper._locker)
                        {
                            while (ControlcanHelper._task.Count > 0
                                && !_token.IsCancellationRequested)
                            {
                                //出队
                                VCI_CAN_OBJ ch = (VCI_CAN_OBJ)ControlcanHelper._task.Dequeue();

                                //解析
                                Application.Current.Dispatcher.Invoke(() => { AnalysisData(ch.ID, ch.Data); });
                            }
                        }
                    }
                }
            }, cts.Token);
        }

        public void ChipRole_SelectedIndexChanged(string ChipRole)
        {
            switch (ChipRole)
            {
                case "BCU":
                    //SelectedChiprole_val = "0x24";
                    SlaveAddress = slaveAddress = "0xFF";
                    break;
                case "BMU":
                    //SelectedChiprole_val = "0x2D";
                    SlaveAddress = slaveAddress = "0x1F";
                    break;
                default:
                    break;
            }
        }

        public void Chiprole_val_SelectedIndexChanged(string Chiprole_val)
        {
            chip_role = SelectedChiprole_val;
            //switch (Chiprole_val)
            //{
            //    case "0x24":
            //        SelectedChiprole = "BCU";
            //        SlaveAddress = slaveAddress = "0xFF";
            //        break;
            //    case "0x2D":
            //        SelectedChiprole = "BMU";
            //        SlaveAddress = slaveAddress = "0x1F";
            //        break;
            //    default:
            //        break;
            //}
        }

        public void Chipcode_SelectedIndexChanged(string Chipcode)
        {
            chip_code = Chipcode;
            slaveAddress = SlaveAddress;
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
                    if (data[0] == 0x01 && data[1] == 0x01 && (id == 0x07FB41FF || id == 0x07FBE0FF || id == 0x07FBF4FF))
                        DeviceList.Add(obj_ID);
                    break;
                case StepFlag.FC升级数据块开始帧:
                case StepFlag.FD升级数据块数据帧:
                    break;
                case StepFlag.FE升级文件接收结果查询帧:
                    if (data[0] == 0x01 && (id == 0x07FE41ff || id == 0x07FEE0ff || id == 0x07FEF4FF))
                        ResultList.Add(data);
                    break;
                case StepFlag.FF升级完成状态查询帧:
                    if (data[0] == 0x01 && (id == 0x07FF41FF || id == 0x07FF5FFF || id == 0x07FFE0FF || id == 0x07FFF4FF))
                    {
                        switch (data[2])
                        {
                            case 0x00://暂存固件标志  00:无固件 
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    //data[1]  0：成功 1： 文件校验失败 2：其它原因失败 3：BCU升级中

                                    if (data[1] == 0x00 && !DevState.ContainsKey(obj_ID))//避免报错，先判断是否包含Key值
                                    {
                                        DevState.Add(obj_ID, 1);
                                        AddLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), obj_ID.ToString("X8"), "升级成功");
                                    }
                                    //else if(data[1] == 0x03)
                                    //{
                                    //    stepFlag = StepFlag.FF升级完成状态查询帧;

                                    //}
                                    else
                                    {
                                        AddLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), obj_ID.ToString("X8"), "升级完成，状态" + data[1]);
                                    }

                                    if (DeviceList.Count <= DevState.Count)
                                    {
                                        int totalSussces = 0;
                                        foreach (var item in DevState)
                                        {
                                            if (item.Value == 1)
                                                totalSussces++;
                                        }
                                        UpgradeTips = $"升级结束，成功数量：{totalSussces}";
                                        UpgradeTipsTextColor = Brushes.Green;

                                        stepFlag = StepFlag.None;
                                        State = false;
                                        _token.Cancel();
                                    }
                                });
                                break;
                            case 0x01://暂存固件标志  01:存在固件

                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    AddLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), obj_ID.ToString("X8"), "暂存成功");
                                });

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


        public ICommand ImportFirmwareCmd => new RelayCommand(ImportFirmware);
        /// <summary>
        /// 导入升级固件
        /// </summary>
        private void ImportFirmware()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.sofar;*.tar;*.bin)|*.sofar;*.tar;*.bin||";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == true)
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
                //Firmwares = new List<FirmwareModel>();
                firmwareModels = new List<FirmwareModel>();


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
                    //this.AnalysisSofar(file_data);//解析SOFAR文件
                    this.AnalysisBin(file_data);//解析Bin文件
                }
                FilePath = file_name;
            }
            else
            {
                FilePath = "";
            }
        }
        public ICommand StartUpgradeCmd => new RelayCommand(StartUpgrade);

        /// <summary>
        /// 启动升级
        /// </summary>
        private void StartUpgrade()
        {
            try
            {
                slaveAddress = SlaveAddress;
                //0.检查CAN连接
                if (!baseCanHelper.IsConnection)
                {
                    MessageBoxHelper.Warning("CAN设备未连接或者连接失败，请重新连接设备!", "警告", null, ButtonType.OK);

                    return;
                }
                //1.判断文件是否为空
                if (string.IsNullOrEmpty(FilePath.Trim()))
                {
                    MessageBoxHelper.Info("当前升级对象为空，请选择升级文件!", "提示", null, ButtonType.OK);
                    return;
                }

                //2.判断是否定时升级
                if (IsStartScheduledUpgrade_Checked)
                {
                    // 选择的定时升级时间ScheduledUpgradeTime
                    DateTime ScheduledUpgradeTime = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day,
                                                                 int.Parse(SelectedHour),
                                                                 int.Parse(SelectedMinute),
                                                                 int.Parse(SelectedSecond)
                    );

                    // 计算与当前时间的间隔
                    TimeSpan interval = ScheduledUpgradeTime - DateTime.Now;

                    if (interval.Minutes < 5)
                    {
                        MessageBoxHelper.Info("升级时间为非法数据，请重新选择!", "提示", null, ButtonType.OK);
                        return;
                    }

                    string[] strTime = ScheduledUpgradeTime.ToString("yy-MM-dd HH:mm:ss").Split('-', ':', ' ');
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
                    int.TryParse(FC.Trim(), out TX_INTERVAL_TIME);
                    int.TryParse(FD.Trim(), out TX_INTERVAL_TIME_Data);

                    Task.Factory.StartNew(() =>
                    {
                        int retryCount = 0;
                        do
                        {
                            if (_token.IsCancellationRequested)
                            {
                                Application.Current.Dispatcher.Invoke(() => { UpgradeProgressBar = 0; });
                                return;
                            }

                            switch (stepFlag)
                            {
                                case StepFlag.None:
                                    break;
                                case StepFlag.FB升级文件传输开始帧:
                                    Thread.Sleep(3000);//待优化

                                    if (DeviceList.Count == 0)
                                    {
                                        if (retryCount < MAX_RETRY_COUNT)
                                        {
                                            file_size = Convert.ToInt32(file_length / 1024) + Convert.ToInt32((file_length % 1024) != 0 ? 1 : 0);//upgradeModel.FileLength
                                            AddLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), "FB", "PACK_ID" + GroupIndex);
                                            startDownloadFlag1(chip_role, chip_code, file_size, 1024);
                                            retryCount++;
                                        }
                                        else
                                        {
                                            Application.Current.Dispatcher.Invoke(() => { UpgradeTips = "响应已超时!"; });
                                            _token.Cancel();
                                        }
                                    }
                                    else
                                    {
                                        stepFlag = StepFlag.FC升级数据块开始帧;
                                        Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            UpgradeTips = "已启动升级";
                                            UpgradeTipsTextColor = Brushes.Green;
                                        });
                                    }
                                    break;
                                case StepFlag.FC升级数据块开始帧:
                                case StepFlag.FD升级数据块数据帧:
                                    startDownloadPack2(GroupIndex, 1024);
                                    Thread.Sleep(TX_INTERVAL_TIME);
                                    AddLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), "FC", "PACK_ID" + (GroupIndex + 1));
                                    int offset = GroupIndex * 1024;
                                    for (int i = 0; i < 1024; i += 8)
                                    {
                                        Thread.Sleep(Math.Max(TX_INTERVAL_TIME_Data, 0));
                                        //AddLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), "FD", "PACK_ID" + GroupIndex);
                                        startDownloadData3(offset + i);
                                    }
                                    Thread.Sleep(TX_INTERVAL_TIME);

                                    GroupIndex++;

                                    Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        UpgradeProgressMax = file_size;
                                        decimal proVal = ((decimal)GroupIndex / file_size) * 100;
                                        UpgradeProgressValue = Convert.ToInt32(proVal);
                                        UpgradeProgressBar = GroupIndex;
                                    });

                                    if (GroupIndex == file_size)
                                    {
                                        stepFlag = StepFlag.FE升级文件接收结果查询帧;
                                    }
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
                                                AddLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), "FC", "PACK_ID" + GroupIndex);
                                                startDownloadPack2(ErrorList[i] - 1, 1024);
                                                Thread.Sleep(TX_INTERVAL_TIME);

                                                GroupIndex = ErrorList[i] % 24 == 0 ? ErrorList[i] / 24 - 1 : ErrorList[i] / 24;
                                                AddLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), "FE", $"第{GroupIndex}组异常，Pack为{ErrorList[i]} ");
                                                offset = (ErrorList[i] - 1) * 1024;

                                                for (int j = 0; j < 1024; j += 8)
                                                {
                                                    Thread.Sleep(Math.Max(TX_INTERVAL_TIME_Data, 0));
                                                    //AddLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), "FD", "PACK_ID" + GroupIndex);
                                                    startDownloadData3(offset + j);
                                                }
                                                Thread.Sleep(TX_INTERVAL_TIME);
                                            }
                                        }
                                        else
                                        {
                                            Application.Current.Dispatcher.Invoke(() =>
                                            {
                                                AddLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), "FE", "PACK_ID" + GroupIndex);
                                                startDownloadCheck4(chip_role, chip_code, file_data);
                                            });

                                        }
                                    } while (ErrorList.Count != 0 || ResultList.Count < DeviceList.Count);

                                    if (IsStartScheduledUpgrade_Checked && upgradeTime != null)
                                    {
                                        startDownloadFlag6(upgradeTime);
                                        Thread.Sleep(TX_INTERVAL_TIME);
                                        //功能吗03:暂存升级                                       
                                        startDownloadState5(chip_role, chip_code, 03, Convert.ToInt32(firmwareModels_bms[0].FirmwareFileTypeCode));
                                        AddLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), "FF", "PACK_ID" + GroupIndex);

                                        //startDownloadState5(chip_role, chip_code, 03, 0x80);
                                    }
                                    else
                                    {
                                        //功能码02:启动升级                                      
                                        startDownloadState5(chip_role, chip_code, 02, Convert.ToInt32(firmwareModels_bms[0].FirmwareFileTypeCode));
                                        AddLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), "FF", "PACK_ID" + GroupIndex);

                                        //startDownloadState5(chip_role, chip_code, 02, 0x80);
                                    }

                                    stepFlag = StepFlag.FF升级完成状态查询帧;
                                    break;
                                case StepFlag.FF升级完成状态查询帧:

                                    //BCU升级等待 继续发送升级完成状态查询
                                    //startDownloadState5(chip_role, chip_code, 02, Convert.ToInt32(firmwareModels_bms[0].FirmwareFileTypeCode));
                                    break;
                                default:
                                    break;
                            }
                        } /*while (stepFlag != StepFlag.None);*/
                        while (stepFlag != StepFlag.FF升级完成状态查询帧);

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

                    UpgradeTips = "";
                    UpgradeProgressBar = 0;

                    stepFlag = 0;
                    State = false;
                }
            }
            catch (Exception ex)
            {
                //Log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " 出现异常，错误信息：" + ex.Message);
                Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss:ffff") + " 出现异常，错误信息：" + ex.Message);
                throw ex;
            }
        }


        /// <summary>
        /// 导入SOFAR文件处理-2K
        /// </summary>
        /// <param name="binchar"></param>
        private void AnalysisSofar(byte[] binchar)
        {
            //清空旧数据
            firmwareModels.Clear();
            //dgvUpgradeProgress.Rows.Clear();
            //固件模块数量
            int count = binchar[binchar.Length - 2048 + 78];
            byte[] firmwareBytes = binchar.Skip(binchar.Length - 2048 + 137).Take(104 * count).ToArray();

            //固件状态
            int index = 0;
            for (int i = 0; i < firmwareBytes.Length; i += 104)
            {
                FirmwareModel firmwareModel = new FirmwareModel();
                //文件类型
                firmwareModel.FirmwareFileType = firmwareBytes[i];
                //芯片角色
                firmwareModel.FirmwareChipRole = firmwareBytes[i + 1];
                //名称
                firmwareModel.FirmwareName = Encoding.ASCII.GetString(firmwareBytes.Skip(i + 2).Take(56).ToArray()).Replace("\0", "");
                //起始偏移地址
                long startAddressByte1 = firmwareBytes[i + 58] & 0xFF;
                long startAddressByte2 = firmwareBytes[i + 59] << 8;
                long startAddressByte3 = firmwareBytes[i + 60] << 16;
                long startAddressByte4 = firmwareBytes[i + 61] << 24;
                firmwareModel.FirmwareStartAddress = startAddressByte1 + startAddressByte2 + startAddressByte3 + startAddressByte4;
                //长度
                long lengthByte1 = firmwareBytes[i + 62] & 0xFF;
                long lengthByte2 = firmwareBytes[i + 63] << 8;
                long lengthByte3 = firmwareBytes[i + 64] << 16;
                long lengthByte4 = firmwareBytes[i + 65] << 24;
                firmwareModel.FirmwareLength = lengthByte1 + lengthByte2 + lengthByte3 + lengthByte4;
                //版本号
                firmwareModel.FirmwareVersion = Encoding.ASCII.GetString(firmwareBytes.Skip(i + 66).Take(20).ToArray());
                firmwareModels.Add(firmwareModel);
                index++;
            }
        }

        /// <summary>
        /// 导入Bin文件处理-1K
        /// </summary>
        /// <param name="binchar"></param>
        private void AnalysisBin(byte[] binchar)
        {

            //清空旧数据
            firmwareModels_bms.Clear();
            FirmwareModel_BMS1500V_DataList.Clear();
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

            FirmwareModel_BMS1500V_DataList.Add(firmwareModel);
            SelectedChipcode = FirmwareModel_BMS1500V_DataList[0].ChipMark;
            SelectedChiprole_val = "0x" + FirmwareModel_BMS1500V_DataList[0].FirmwareChipRoleCode.ToString("X2");
            switch (SelectedChiprole_val)
            {
                case "0x24":
                    SelectedChiprole = "BCU";
                    SlaveAddress = slaveAddress = "0xFF";
                    break;
                case "0x2D":
                    SelectedChiprole = "BMU";
                    SlaveAddress = slaveAddress = "0x1F";
                    break;
                default:
                    break;
            }
            firmwareModels_bms.Add(firmwareModel);
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
                MessageBoxHelper.Info($"文件创建时间解析失败: {ex.Message}", "提示", null, ButtonType.OK);
                return "";
            }
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

        public enum BmsCanChipRole
        {
            ARM = 0x00,
            DSPM = 0x01,
            DSPS = 0x02,
            PCU = 0x01,
            BMS = 0x03,
            BDU = 0x04,
        }

        public class FirmwareModel_BMS1500V : ObservableObject
        {
            /// <summary>
            /// 工程名称
            /// </summary>
            public string ProjectName { get; set; }

            /// <summary>
            /// 文件大小
            /// </summary>
            public uint FirmwareSize { get; set; }

            /// <summary>
            /// 文件创建时间
            /// </summary>
            public string dateTimeString { get; set; }

            /// <summary>
            /// 文件类型
            /// </summary>
            public string FirmwareFileType { get; set; }

            /// <summary>
            /// 文件类型代号
            /// </summary>
            public byte FirmwareFileTypeCode { get; set; }

            /// <summary>
            /// 芯片角色
            /// </summary>
            public string FirmwareChipRole { get; set; }

            /// <summary>
            /// 芯片角色代号
            /// </summary>
            public byte FirmwareChipRoleCode { get; set; }

            /// <summary>
            /// 芯片代号
            /// </summary>
            public string ChipModel { get; set; }

            /// <summary>
            /// 芯片型号
            /// </summary>
            public string ChipMark { get; set; }


            /// <summary>
            ///软件版本号
            /// </summary>
            public string SoftwareVersion { get; set; }

            /// <summary>
            /// 硬件版本号
            /// </summary>
            public string HardwareVersion { get; set; }



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
                MessageBoxHelper.Error($"升级文件传输开始帧0x7FB，ERROR：{ex.Message}", "错误", null, ButtonType.OK);
                //throw new Exception("升级文件传输开始帧0x7FB，ERROR：" + ex.Message);
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
                MessageBoxHelper.Error($"升级数据块开始帧0xFC，ERROR：" + ex.Message, "错误", null, ButtonType.OK);
                //throw new Exception("升级数据块开始帧0xFC，ERROR：" + ex.Message);
            }
        }

        /// <summary>
        /// 升级数据块数据帧
        /// </summary>
        /// <param name="offset"></param>
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
                MessageBoxHelper.Error($"升级数据块数据帧0xFD，ERROR：" + ex.Message, "错误", null, ButtonType.OK);
                //throw new Exception("升级数据块数据帧0xFD，ERROR：" + ex.Message);
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
                MessageBoxHelper.Error($"升级文件接收结果查询帧0xFE，ERROR：" + ex.Message, "错误", null, ButtonType.OK);
                //throw new Exception("升级文件接收结果查询帧0xFE，ERROR：" + ex.Message);
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
                MessageBoxHelper.Error($"升级数据块数据帧0xFD，ERROR：" + ex.Message, "错误", null, ButtonType.OK);
                //throw new Exception("升级数据块数据帧0xFD，ERROR：" + ex.Message);
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
                MessageBoxHelper.Error($"升级完成状态查询帧0xFF，ERROR：" + ex.Message, "错误", null, ButtonType.OK);
                //throw new Exception("升级完成状态查询帧0xFF，ERROR：" + ex.Message);
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
                //Log.Info("readBinFile异常:" + ex.Message);
                Debug.Print("readBinFile异常:" + ex.Message);
                throw ex;
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
                byte[] canid = new byte[4];
                switch (SelectedChiprole)
                {
                    case "BCU":
                        canid = new byte[] { 0xF4, Convert.ToByte(slaveAddress, 16), mark, 0x07 };
                        break;
                    case "BMU":
                        canid = new byte[] { 0xE0, Convert.ToByte(slaveAddress, 16), mark, 0x07 };
                        break;
                    default:
                        break;
                }


                //增加判断，确认是否发送成功；
                bool result = baseCanHelper.Send(data, canid);
                if (!result)
                {
                    sendErrorCount++;
                    if (sendErrorCount >= 10)
                    {
                        //State = false;
                        //cts.Cancel();
                        //DevState.Clear();
                        //DeviceList.Clear();

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            UpgradeTips = "发送失败，请检查通讯重新连接！";
                            UpgradeTipsTextColor = Brushes.Red;
                            UpgradeProgressBar = 0;
                        });

                        Thread.Sleep(1000 * 10);
                    }
                }
            }
            catch (Exception ex)
            {
                //Log.Info(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "下发数据异常：" + ex.Message);
                Debug.Print(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "下发数据异常：" + ex.Message);
                throw ex;
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
        /// 升级日志
        /// </summary>
        /// <param name="date"></param>
        /// <param name="id"></param>
        /// <param name="context"></param>
        private void AddLog(string date, string id, string context)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                UpgradeLogList.Insert(0, (new UpgradeLogData
                {
                    LogTime = date,
                    FrameID = id,
                    LogData = context
                }));
            });
        }

        public void CancelOperation()
        {
            if (cts != null)
            {
                cts.Cancel();
                cts.Dispose();
            }
        }

        public void UpdateTime()
        {
            DateTime now = DateTime.Now;
            SelectedDate = now.Date;
            SelectedHour = now.Hour.ToString("D2");
            SelectedMinute = now.Minute.ToString("D2");
            SelectedSecond = now.Second.ToString("D2");
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

        /// <summary>
        /// 升级日志数据
        /// </summary>
        public class UpgradeLogData
        {
            public string LogTime { get; set; }  // 日志记录时间         
            public string FrameID { get; set; }  // 帧ID
            public string LogData { get; set; }  // 日志数据      
        }

        //public class FirmwareModel
        //{
        //    public FirmwareModel()
        //    {

        //    }
        //    public FirmwareModel(string firmwareName, string firmwareType, int startAddress, string firmwareVersion, int length)
        //    {
        //        this.FirmwareName = firmwareName;
        //        this.FirmwareType = firmwareType;
        //        this.StartAddress = startAddress;
        //        this.FirmwareVersion = firmwareVersion;
        //        this.Length = length;
        //    }

        //    public string FirmwareName { get; set; }
        //    public string FirmwareType { get; set; }
        //    public int StartAddress { get; set; }
        //    public string FirmwareVersion { get; set; }
        //    public int Length { get; set; }
        //    public bool CheckFlg { get; set; }
        //}

        //public class FirmwareModel
        //{
        //    public FirmwareModel()
        //    {

        //    }
        //    public FirmwareModel(string firmwareName, byte firmwareFileType, byte firmwareChipRole, int firmwareStartAddress, string firmwareVersion, int firmwareLength)
        //    {
        //        this.FirmwareName = firmwareName;
        //        this.FirmwareFileType = firmwareFileType;
        //        this.FirmwareChipRole = firmwareChipRole;
        //        this.FirmwareStartAddress = firmwareStartAddress;
        //        this.FirmwareVersion = firmwareVersion;
        //        this.FirmwareLength = firmwareLength;
        //    }

        //    //名称
        //    public string FirmwareName { get; set; }

        //    //文件类型
        //    public byte FirmwareFileType { get; set; }

        //    //芯片角色
        //    public byte FirmwareChipRole { get; set; }

        //    //起始偏移地址
        //    public long FirmwareStartAddress { get; set; }

        //    //版本号
        //    public string FirmwareVersion { get; set; }

        //    //长度
        //    public long FirmwareLength { get; set; }

        //}
    }
}

