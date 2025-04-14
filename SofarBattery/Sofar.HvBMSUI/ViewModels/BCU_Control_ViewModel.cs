using CommunityToolkit.Mvvm.ComponentModel;
using PowerKit.UI.Common;
using PowerKit.UI.Models;
using Sofar.BMSLib;
using Sofar.BMSUI;
using Sofar.BMSUI.Common;
using Sofar.HvBMSUI.Models;
using Sofar.ProtocolLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static PowerKit.UI.Models.RealtimeData_BMS1500V_BCU;

namespace Sofar.HvBMSUI.ViewModels
{
    public partial class BCU_Control_ViewModel : ObservableObject
    {
        private ObservableCollection<batteryVoltageData> batteryVoltageDataList;
        private ObservableCollection<batterySocData> batterySocDataList;
        private ObservableCollection<batterySohData> batterySohDataList;
        private ObservableCollection<batteryTemperatureData> batteryTemperatureDataList;
        private ObservableCollection<supplyVoltageData> supplyVoltageDataList;
        private ObservableCollection<poleTemperatureData> poleTemperatureDataList;
        private ObservableCollection<moduleTotalVoltageData> moduleTotalVoltageDataList;

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

        public ObservableCollection<string> ProductNameList { get; } = new ObservableCollection<string>(new[] { "PowerMagic 1.0", "PowerMagic 2.0" });

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
        public ObservableCollection<string> Address_BCU_List { get; } = new ObservableCollection<string>
        {
             "E8",
             "E9",
             "EA",
             "EB",
             "EC",
             "ED",
             "EE",
             "EF",
             "F0",
             "F1",
             "F2",
             "F3",
             "F4",
             "F5",
             "F6",
             "F7",
             "F8",
             "F9",
             "FA",
             "FB",
             "FC",
             "FD",
             "FE",
             "FF",
             "100",
             "101",
             "102",
             "103",
             "104",
             "105",
             "106",
             "107",
             "108",
             "109",
             "10A",
             "10B",
             "10C",
             "10D",
             "10E",
             "10F"
        };

        private string _selectedAddress_BCU = "E8";
        /// <summary>
        /// BCU地址设置
        /// </summary>
        public string SelectedAddress_BCU
        {
            get { return _selectedAddress_BCU; }
            set
            {
                _selectedAddress_BCU = value;
                AlarmMessageDataList.Clear();
                FaultInfo.FaultInfos4.ForEach(x => x.State = 0);
                OnPropertyChanged(nameof(SelectedAddress_BCU));
            }
        }

        public ObservableCollection<int> SlaveControlNumList { get; } = new ObservableCollection<int>(Enumerable.Range(1, 60));

        private int _SelectedSlaveControlNum = 1;
        /// <summary>
        /// 从控模块号
        /// </summary>
        public int SelectedSlaveControlNum
        {
            get { return _SelectedSlaveControlNum; }
            set
            {
                _SelectedSlaveControlNum = value;
                OnPropertyChanged(nameof(SelectedSlaveControlNum));
            }
        }

        private string _MainControlSoftwareVersion;
        /// <summary>
        /// 主控软件版本
        /// </summary>
        public string MainControlSoftwareVersion
        {
            get { return _MainControlSoftwareVersion; }
            set
            {
                _MainControlSoftwareVersion = value;
                OnPropertyChanged(nameof(MainControlSoftwareVersion));
            }
        }

        private string _SlaveControlSoftwareVersion;
        /// <summary>
        /// 从控软件版本
        /// </summary>
        public string SlaveControlSoftwareVersion
        {
            get { return _SlaveControlSoftwareVersion; }
            set
            {
                _SlaveControlSoftwareVersion = value;
                OnPropertyChanged(nameof(SlaveControlSoftwareVersion));
            }
        }

        private string _SlaveControlSoftwareVersion_1;
        /// <summary>
        /// 从控BMU1软件版本
        /// </summary>
        public string SlaveControlSoftwareVersion_1
        {
            get { return _SlaveControlSoftwareVersion_1; }
            set
            {
                _SlaveControlSoftwareVersion_1 = value;
                OnPropertyChanged(nameof(SlaveControlSoftwareVersion_1));
            }
        }
        private string _SlaveControlSoftwareVersion_2;
        /// <summary>
        /// 从控BMU2软件版本
        /// </summary>
        public string SlaveControlSoftwareVersion_2
        {
            get { return _SlaveControlSoftwareVersion_2; }
            set
            {
                _SlaveControlSoftwareVersion_2 = value;
                OnPropertyChanged(nameof(SlaveControlSoftwareVersion_2));
            }
        }

        private string _SlaveControlSoftwareVersion_3;
        /// <summary>
        /// 从控BMU3软件版本
        /// </summary>
        public string SlaveControlSoftwareVersion_3
        {
            get { return _SlaveControlSoftwareVersion_3; }
            set
            {
                _SlaveControlSoftwareVersion_3 = value;
                OnPropertyChanged(nameof(SlaveControlSoftwareVersion_3));
            }
        }

        private string _SlaveControlSoftwareVersion_4;
        /// <summary>
        /// 从控BMU4软件版本
        /// </summary>
        public string SlaveControlSoftwareVersion_4
        {
            get { return _SlaveControlSoftwareVersion_4; }
            set
            {
                _SlaveControlSoftwareVersion_4 = value;
                OnPropertyChanged(nameof(SlaveControlSoftwareVersion_4));
            }
        }

        private string _SlaveControlSoftwareVersion_5;
        /// <summary>
        /// 从控BMU5软件版本
        /// </summary>
        public string SlaveControlSoftwareVersion_5
        {
            get { return _SlaveControlSoftwareVersion_5; }
            set
            {
                _SlaveControlSoftwareVersion_5 = value;
                OnPropertyChanged(nameof(SlaveControlSoftwareVersion_5));
            }
        }

        private string _SlaveControlSoftwareVersion_6;
        /// <summary>
        /// 从控BMU6软件版本
        /// </summary>
        public string SlaveControlSoftwareVersion_6
        {
            get { return _SlaveControlSoftwareVersion_6; }
            set
            {
                _SlaveControlSoftwareVersion_6 = value;
                OnPropertyChanged(nameof(SlaveControlSoftwareVersion_6));
            }
        }

        private string _SlaveControlSoftwareVersion_7;
        /// <summary>
        /// 从控BMU7软件版本
        /// </summary>
        public string SlaveControlSoftwareVersion_7
        {
            get { return _SlaveControlSoftwareVersion_7; }
            set
            {
                _SlaveControlSoftwareVersion_7 = value;
                OnPropertyChanged(nameof(SlaveControlSoftwareVersion_7));
            }
        }

        private string _SlaveControlSoftwareVersion_8;
        /// <summary>
        /// 从控BMU8软件版本
        /// </summary>
        public string SlaveControlSoftwareVersion_8
        {
            get { return _SlaveControlSoftwareVersion_8; }
            set
            {
                _SlaveControlSoftwareVersion_8 = value;
                OnPropertyChanged(nameof(SlaveControlSoftwareVersion_8));
            }
        }

        private string _SlaveControlSoftwareVersion_9;
        /// <summary>
        /// 从控BMU9软件版本
        /// </summary>
        public string SlaveControlSoftwareVersion_9
        {
            get { return _SlaveControlSoftwareVersion_9; }
            set
            {
                _SlaveControlSoftwareVersion_9 = value;
                OnPropertyChanged(nameof(SlaveControlSoftwareVersion_9));
            }
        }

        private string _SlaveControlSoftwareVersion_10;
        /// <summary>
        /// 从控BMU10软件版本
        /// </summary>
        public string SlaveControlSoftwareVersion_10
        {
            get { return _SlaveControlSoftwareVersion_10; }
            set
            {
                _SlaveControlSoftwareVersion_10 = value;
                OnPropertyChanged(nameof(SlaveControlSoftwareVersion_10));
            }
        }

        private string _CommunicationProtocolVersion;
        /// <summary>
        /// 通讯协议版本
        /// </summary>
        public string CommunicationProtocolVersion
        {
            get { return _CommunicationProtocolVersion; }
            set
            {
                _CommunicationProtocolVersion = value;
                OnPropertyChanged(nameof(CommunicationProtocolVersion));
            }
        }

        private string _ProjectNum;
        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectNum
        {
            get { return _ProjectNum; }
            set
            {
                _ProjectNum = value;
                OnPropertyChanged(nameof(ProjectNum));
            }
        }

        public ObservableCollection<string> BatteryTypeList { get; } = new ObservableCollection<string>
        {
             "1： L--磷酸铁锂电池",
             "2： N—钛酸锂电池",
             "3： T—锰酸锂电池",
             "4： S--三元锂电池"
        };

        private string _selectedBatteryType;
        /// <summary>
        /// 电池类型
        /// </summary>
        public string SelectedBatteryType
        {
            get { return _selectedBatteryType; }
            set
            {
                _selectedBatteryType = value;
                OnPropertyChanged(nameof(SelectedBatteryType));
            }
        }

        private string _BatteryCapacity;
        /// <summary>
        /// 电池容量
        /// </summary>
        public string BatteryCapacity
        {
            get { return _BatteryCapacity; }
            set
            {
                _BatteryCapacity = value;
                OnPropertyChanged(nameof(BatteryCapacity));
            }
        }

        private string _BatteryModel;
        /// <summary>
        /// 电池型号
        /// </summary>
        public string BatteryModel
        {
            get { return _BatteryModel; }
            set
            {
                _BatteryModel = value;
                OnPropertyChanged(nameof(BatteryModel));
            }
        }

        private string _BatteryManufacturer;
        /// <summary>
        /// 电池厂家
        /// </summary>
        public string BatteryManufacturer
        {
            get { return _BatteryManufacturer; }
            set
            {
                _BatteryManufacturer = value;
                OnPropertyChanged(nameof(BatteryManufacturer));
            }
        }
        private string _ProductName;
        /// <summary>
        /// 硬件版本
        /// </summary>
        public string ProductName
        {
            get { return _ProductName; }
            set
            {
                _ProductName = value;
                OnPropertyChanged(nameof(ProductName));
            }
        }
        private string _SystemTotalModuleNumber;
        /// <summary>
        /// 系统总模块数
        /// </summary>
        public string SystemTotalModuleNumber
        {
            get { return _SystemTotalModuleNumber; }
            set
            {
                _SystemTotalModuleNumber = value;
                OnPropertyChanged(nameof(SystemTotalModuleNumber));
            }
        }

        private string _SystemTotalbatteryNumber;
        /// <summary>
        /// 系统总电池节数
        /// </summary>
        public string SystemTotalbatteryNumber
        {
            get { return _SystemTotalbatteryNumber; }
            set
            {
                _SystemTotalbatteryNumber = value;
                OnPropertyChanged(nameof(SystemTotalbatteryNumber));
            }
        }

        private string _SystemTotalTemperaturesNumber;
        /// <summary>
        /// 系统总温度个数
        /// </summary>
        public string SystemTotalTemperaturesNumber
        {
            get { return _SystemTotalTemperaturesNumber; }
            set
            {
                _SystemTotalTemperaturesNumber = value;
                OnPropertyChanged(nameof(SystemTotalTemperaturesNumber));
            }
        }

        public ObservableCollection<int> ModulAddressList { get; } = new ObservableCollection<int>(Enumerable.Range(1, 60));
        private int _SelectedModulAddress;
        /// <summary>
        /// 模块地址—发送强制均衡命令-主动均衡
        /// </summary>
        public int SelectedModulAddress
        {
            get { return _SelectedModulAddress; }
            set
            {
                _SelectedModulAddress = value;
                OnPropertyChanged(nameof(SelectedModulAddress));
            }
        }

        private string _EquilibriumTime;
        /// <summary>
        /// 均衡时间—发送强制均衡命令-主动均衡
        /// </summary>
        public string EquilibriumTime
        {
            get { return _EquilibriumTime; }
            set
            {
                _EquilibriumTime = value;
                OnPropertyChanged(nameof(EquilibriumTime));
            }
        }

        private string _RemainingEquilibriumTime;
        /// <summary>
        /// 剩余均衡时间—发送强制均衡命令-主动均衡
        /// </summary>
        public string RemainingEquilibriumTime
        {
            get { return _RemainingEquilibriumTime; }
            set
            {
                _RemainingEquilibriumTime = value;
                OnPropertyChanged(nameof(RemainingEquilibriumTime));
            }
        }


        public ObservableCollection<int> BatteryNumber_SOC_List { get; } = new ObservableCollection<int>(Enumerable.Range(1, 3480));
        private int _SelectedBatteryNumber_SOC;
        /// <summary>
        /// 电池序号—设置电池SOC
        /// </summary>
        public int SelectedBatteryNumber_SOC
        {
            get { return _SelectedBatteryNumber_SOC; }
            set
            {
                _SelectedBatteryNumber_SOC = value;
                OnPropertyChanged(nameof(SelectedBatteryNumber_SOC));
            }
        }

        public ObservableCollection<int> BatteryNumber_SOH_List { get; } = new ObservableCollection<int>(Enumerable.Range(1, 3480));
        private int _SelectedBatteryNumber_SOH;
        /// <summary>
        /// 电池序号—设置电池SOC
        /// </summary>
        public int SelectedBatteryNumber_SOH
        {
            get { return _SelectedBatteryNumber_SOH; }
            set
            {
                _SelectedBatteryNumber_SOH = value;
                OnPropertyChanged(nameof(SelectedBatteryNumber_SOH));
            }
        }

        private string _SOC;
        /// <summary>
        /// SOC设置值
        /// </summary>
        public string SOC
        {
            get { return _SOC; }
            set
            {
                _SOC = value;
                OnPropertyChanged(nameof(SOC));
            }
        }

        private string _SOH;
        /// <summary>
        /// SOH设置值
        /// </summary>
        public string SOH
        {
            get { return _SOH; }
            set
            {
                _SOH = value;
                OnPropertyChanged(nameof(SOH));
            }
        }


        /// <summary>
        /// 当选择的 BCU 地址改变时触发的操作
        /// </summary>
        private void OnSelectedAddressChanged()
        {
            // 执行跟选择的地址相关的操作
            // 例如更新显示或配置
        }


        private ObservableCollection<IBatteryData> _batteryDataList;
        // 通用电池数据
        public ObservableCollection<IBatteryData> BatteryDataList
        {
            get => _batteryDataList;
            set
            {
                _batteryDataList = value;
                OnPropertyChanged(nameof(BatteryDataList)); // 通知界面更新
            }
        }

        private bool _isChecked_DO1_Close = true;
        public bool IsChecked_DO1_Close
        {
            get { return _isChecked_DO1_Close; }
            set
            {
                _isChecked_DO1_Close = value;
                OnPropertyChanged(nameof(IsChecked_DO1_Close));
                if (value) _isChecked_DO1_Open = false; // 开关互斥
            }
        }

        private bool _isChecked_DO1_Open;
        public bool IsChecked_DO1_Open
        {
            get { return _isChecked_DO1_Open; }
            set
            {
                _isChecked_DO1_Open = value;
                OnPropertyChanged(nameof(IsChecked_DO1_Open));
                if (value) IsChecked_DO1_Close = false; // 开关互斥
            }
        }


        private bool _isChecked_DO2_Close = true;
        public bool IsChecked_DO2_Close
        {
            get { return _isChecked_DO2_Close; }
            set
            {
                _isChecked_DO2_Close = value;
                OnPropertyChanged(nameof(IsChecked_DO2_Close));
                if (value) _isChecked_DO2_Open = false; // 开关互斥
            }
        }

        private bool _isChecked_DO2_Open;
        public bool IsChecked_DO2_Open
        {
            get { return _isChecked_DO2_Open; }
            set
            {
                _isChecked_DO2_Open = value;
                OnPropertyChanged(nameof(IsChecked_DO2_Open));
                if (value) IsChecked_DO2_Close = false; // 开关互斥
            }
        }


        private bool _isChecked_DO3_Close = true;
        public bool IsChecked_DO3_Close
        {
            get { return _isChecked_DO3_Close; }
            set
            {
                _isChecked_DO3_Close = value;
                OnPropertyChanged(nameof(IsChecked_DO3_Close));
                if (value) _isChecked_DO3_Open = false; // 开关互斥
            }
        }

        private bool _isChecked_DO3_Open;
        public bool IsChecked_DO3_Open
        {
            get { return _isChecked_DO3_Open; }
            set
            {
                _isChecked_DO3_Open = value;
                OnPropertyChanged(nameof(IsChecked_DO3_Open));
                if (value) IsChecked_DO3_Close = false; // 开关互斥
            }
        }


        private bool _isChecked_DO4_Close = true;
        public bool IsChecked_DO4_Close
        {
            get { return _isChecked_DO4_Close; }
            set
            {
                _isChecked_DO4_Close = value;
                OnPropertyChanged(nameof(IsChecked_DO4_Close));
                if (value) _isChecked_DO4_Open = false; // 开关互斥
            }
        }

        private bool _isChecked_DO4_Open;
        public bool IsChecked_DO4_Open
        {
            get { return _isChecked_DO4_Open; }
            set
            {
                _isChecked_DO4_Open = value;
                OnPropertyChanged(nameof(IsChecked_DO4_Open));
                if (value) IsChecked_DO4_Close = false; // 开关互斥
            }
        }

        private bool _isChecked_DO5_Close = true;
        public bool IsChecked_DO5_Close
        {
            get { return _isChecked_DO5_Close; }
            set
            {
                _isChecked_DO5_Close = value;
                OnPropertyChanged(nameof(IsChecked_DO5_Close));
                if (value) _isChecked_DO5_Open = false; // 开关互斥
            }
        }

        private bool _isChecked_DO5_Open;
        public bool IsChecked_DO5_Open
        {
            get { return _isChecked_DO5_Open; }
            set
            {
                _isChecked_DO5_Open = value;
                OnPropertyChanged(nameof(IsChecked_DO5_Open));
                if (value) IsChecked_DO5_Close = false; // 开关互斥
            }
        }

        private bool _isChecked_DO6_Close = true;
        public bool IsChecked_DO6_Close
        {
            get { return _isChecked_DO6_Close; }
            set
            {
                _isChecked_DO6_Close = value;
                OnPropertyChanged(nameof(IsChecked_DO6_Close));
                if (value) _isChecked_DO6_Open = false; // 开关互斥
            }
        }

        private bool _isChecked_DO6_Open;
        public bool IsChecked_DO6_Open
        {
            get { return _isChecked_DO6_Open; }
            set
            {
                _isChecked_DO6_Open = value;
                OnPropertyChanged(nameof(IsChecked_DO6_Open));
                if (value) IsChecked_DO6_Close = false; // 开关互斥
            }
        }

        private bool _isChecked_DO7_Close = true;
        public bool IsChecked_DO7_Close
        {
            get { return _isChecked_DO7_Close; }
            set
            {
                _isChecked_DO7_Close = value;
                OnPropertyChanged(nameof(IsChecked_DO7_Close));
                if (value) _isChecked_DO7_Open = false; // 开关互斥
            }
        }

        private bool _isChecked_DO7_Open;
        public bool IsChecked_DO7_Open
        {
            get { return _isChecked_DO7_Open; }
            set
            {
                _isChecked_DO7_Open = value;
                OnPropertyChanged(nameof(IsChecked_DO7_Open));
                if (value) IsChecked_DO7_Close = false; // 开关互斥
            }
        }

        private bool _isChecked_DO8_Close = true;
        public bool IsChecked_DO8_Close
        {
            get { return _isChecked_DO8_Close; }
            set
            {
                _isChecked_DO8_Close = value;
                OnPropertyChanged(nameof(IsChecked_DO8_Close));
                if (value) _isChecked_DO8_Open = false; // 开关互斥
            }
        }

        private bool _isChecked_DO8_Open;
        public bool IsChecked_DO8_Open
        {
            get { return _isChecked_DO8_Open; }
            set
            {
                _isChecked_DO8_Open = value;
                OnPropertyChanged(nameof(IsChecked_DO8_Open));
                if (value) IsChecked_DO8_Close = false; // 开关互斥
            }
        }

        private string _ActiveEquilibriumStatus = "Collapsed";
        /// <summary>
        /// 主动均衡状态
        /// </summary>
        public string ActiveEquilibriumStatus
        {
            get { return _ActiveEquilibriumStatus; }
            set
            {
                _ActiveEquilibriumStatus = value;
                OnPropertyChanged(nameof(ActiveEquilibriumStatus));
            }
        }

        private ObservableCollection<ActiveEquilibriumCheckBoxGroup> _ActiveEquilibriumCheckBoxItems;
        /// <summary>
        /// 主动均衡显示结构
        /// </summary>
        public ObservableCollection<ActiveEquilibriumCheckBoxGroup> ActiveEquilibriumCheckBoxItems
        {
            get { return _ActiveEquilibriumCheckBoxItems; }
            set
            {
                _ActiveEquilibriumCheckBoxItems = value;
                OnPropertyChanged(nameof(ActiveEquilibriumCheckBoxItems));
            }
        }


        private string _PassiveEquilibriumStatus = "Visible";
        /// <summary>
        /// 被动均衡状态
        /// </summary>
        public string PassiveEquilibriumStatus
        {
            get { return _PassiveEquilibriumStatus; }
            set
            {
                _PassiveEquilibriumStatus = value;
                OnPropertyChanged(nameof(PassiveEquilibriumStatus));
            }
        }
        private ObservableCollection<CheckBoxItem> _PassiveEquilibriumCheckBoxItems;
        /// <summary>
        /// 被动均衡显示结构
        /// </summary>
        public ObservableCollection<CheckBoxItem> PassiveEquilibriumCheckBoxItems
        {
            get { return _PassiveEquilibriumCheckBoxItems; }
            set
            {
                _PassiveEquilibriumCheckBoxItems = value;
                OnPropertyChanged(nameof(PassiveEquilibriumCheckBoxItems));
            }
        }

        private string _PassiveEquilibriumState = "电芯1~64被动均衡状态";

        /// <summary>
        /// 电芯1~64被动均衡状态
        /// </summary>
        public string PassiveEquilibriumState
        {
            get { return _PassiveEquilibriumState; }
            set
            {
                _PassiveEquilibriumState = value;
                OnPropertyChanged(nameof(PassiveEquilibriumState));
            }
        }

        private ObservableCollection<ActiveEquilibriumCheckBoxGroup> _ActiveEquilibriumControlItems;
        /// <summary>
        /// 主动均衡控制结构
        /// </summary>
        public ObservableCollection<ActiveEquilibriumCheckBoxGroup> ActiveEquilibriumControlItems
        {
            get { return _ActiveEquilibriumControlItems; }
            set
            {
                _ActiveEquilibriumControlItems = value;
                OnPropertyChanged(nameof(ActiveEquilibriumControlItems));
            }
        }

        private ObservableCollection<PassiveEquilibriumCheckBoxGroup> _PassiveEquilibriumControlItems;

        /// <summary>
        /// 被动均衡控制结构
        /// </summary>
        public ObservableCollection<PassiveEquilibriumCheckBoxGroup> PassiveEquilibriumControlItems
        {
            get { return _PassiveEquilibriumControlItems; }
            set
            {
                _PassiveEquilibriumControlItems = value;
                OnPropertyChanged(nameof(PassiveEquilibriumControlItems));
            }
        }

        public ObservableCollection<int> PackageNumberList { get; } = new ObservableCollection<int>(Enumerable.Range(1, 60));
        private int _selectedPackageNumber = 1;
        /// <summary>
        /// 被动均衡状态设置-帧序号N
        /// </summary>
        public int SelectedPackageNumber
        {
            get { return _selectedPackageNumber; }
            set
            {
                if (_selectedPackageNumber != value)
                {
                    _selectedPackageNumber = value;
                    OnPropertyChanged(nameof(SelectedPackageNumber));
                    //PackageNumber_SelectedIndexChanged(value); // 处理选项变化
                }
            }
        }



        public ObservableCollection<string> EquilibriumTypeList { get; } = new ObservableCollection<string>
        {
             "0： 被动均衡",
             "1： 主动均衡"
        };

        private string _selectedEquilibriumType = "0： 被动均衡";
        /// <summary>
        /// 均衡类型
        /// </summary>
        public string SelectedEquilibriumType
        {
            get { return _selectedEquilibriumType; }
            set
            {
                _selectedEquilibriumType = value;
                OnPropertyChanged(nameof(SelectedEquilibriumType));
                if (value == "0： 被动均衡")
                {
                    ActiveEquilibriumStatus = "Collapsed";
                    PassiveEquilibriumStatus = "Visible";
                }
                else if (value == "1： 主动均衡")
                {
                    ActiveEquilibriumStatus = "Visible";
                    PassiveEquilibriumStatus = "Collapsed";
                }
            }
        }

        private string _PassiveEquilibriumControl = "Visible";

        /// <summary>
        /// 均衡命令——被动均衡
        /// </summary>
        public string PassiveEquilibriumControl
        {
            get { return _PassiveEquilibriumControl; }
            set
            {
                _PassiveEquilibriumControl = value;
                OnPropertyChanged(nameof(PassiveEquilibriumControl));
            }
        }

        private string _ActiveEquilibriumControl = "Collapsed";

        /// <summary>
        /// 均衡命令——主动均衡
        /// </summary>
        public string ActiveEquilibriumControl
        {
            get { return _ActiveEquilibriumControl; }
            set
            {
                _ActiveEquilibriumControl = value;
                OnPropertyChanged(nameof(ActiveEquilibriumControl));
            }
        }

        public ObservableCollection<string> EquilibriumType_WriteList { get; } = new ObservableCollection<string>
        {
             "被动均衡",
             "主动均衡"
        };

        private string _selectedEquilibriumType_Write = "被动均衡";
        /// <summary>
        /// 均衡类型
        /// </summary>
        public string SelectedEquilibriumType_Write
        {
            get { return _selectedEquilibriumType_Write; }
            set
            {
                _selectedEquilibriumType_Write = value;
                OnPropertyChanged(nameof(SelectedEquilibriumType_Write));
                if (value == "被动均衡")
                {
                    PassiveEquilibriumControl = "Visible";
                    ActiveEquilibriumControl = "Collapsed";

                }
                else if (value == "主动均衡")
                {
                    ActiveEquilibriumControl = "Visible";
                    PassiveEquilibriumControl = "Collapsed";
                }
            }
        }






        public ObservableCollection<int> ModuleNumberList { get; } = new ObservableCollection<int>(Enumerable.Range(1, 60));
        private int _selectedModuleNumber = 1;
        /// <summary>
        /// 模块号
        /// </summary>
        public int SelectedModuleNumber
        {
            get { return _selectedModuleNumber; }
            set
            {
                _selectedModuleNumber = value;
                OnPropertyChanged(nameof(SelectedModuleNumber));
                PackageNumber_SelectedIndexChanged(value); // 处理选项变化
            }
        }

        public ObservableCollection<int> ChannelNumberList { get; } = new ObservableCollection<int>(Enumerable.Range(1, 12));
        private int _selectedChannelNumber = 1;
        /// <summary>
        /// 通道号
        /// </summary>
        public int SelectedChannelNumber
        {
            get { return _selectedChannelNumber; }
            set
            {
                _selectedChannelNumber = value;
                OnPropertyChanged(nameof(SelectedChannelNumber));
            }
        }

        private string _isChannelName = "通道1";
        /// <summary>
        /// 通道名称
        /// </summary>
        public string ChannelName
        {
            get { return _isChannelName; }
            set
            {
                _isChannelName = value;
                OnPropertyChanged(nameof(ChannelName));
            }
        }


        private bool _isOutOfContact_ControlModule_1 = true;
        /// <summary>
        /// 从控模块1是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_1
        {
            get { return _isOutOfContact_ControlModule_1; }
            set
            {
                _isOutOfContact_ControlModule_1 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_1));
            }
        }
        private bool _isOutOfContact_ControlModule_2 = true;
        /// <summary>
        /// 从控模块2是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_2
        {
            get { return _isOutOfContact_ControlModule_2; }
            set
            {
                _isOutOfContact_ControlModule_2 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_2));
            }
        }
        private bool _isOutOfContact_ControlModule_3 = true;
        /// <summary>
        /// 从控模块3是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_3
        {
            get { return _isOutOfContact_ControlModule_3; }
            set
            {
                _isOutOfContact_ControlModule_3 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_3));
            }
        }
        private bool _isOutOfContact_ControlModule_4 = true;
        /// <summary>
        /// 从控模块4是否失联
        /// </summary>  
        public bool IsOutOfContact_ControlModule_4
        {
            get { return _isOutOfContact_ControlModule_4; }
            set
            {
                _isOutOfContact_ControlModule_4 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_4));
            }
        }
        private bool _isOutOfContact_ControlModule_5 = true;
        /// <summary>
        /// 从控模块5是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_5
        {
            get { return _isOutOfContact_ControlModule_5; }
            set
            {
                _isOutOfContact_ControlModule_5 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_5));
            }
        }
        private bool _isOutOfContact_ControlModule_6 = true;
        /// <summary>
        /// 从控模块6是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_6
        {
            get { return _isOutOfContact_ControlModule_6; }
            set
            {
                _isOutOfContact_ControlModule_6 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_6));
            }
        }
        private bool _isOutOfContact_ControlModule_7 = true;
        /// <summary>
        /// 从控模块7是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_7
        {
            get { return _isOutOfContact_ControlModule_7; }
            set
            {
                _isOutOfContact_ControlModule_7 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_7));
            }
        }
        private bool _isOutOfContact_ControlModule_8 = true;
        /// <summary>
        /// 从控模块8是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_8
        {
            get { return _isOutOfContact_ControlModule_8; }
            set
            {
                _isOutOfContact_ControlModule_8 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_8));
            }
        }
        private bool _isOutOfContact_ControlModule_9 = true;
        /// <summary>
        /// 从控模块9是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_9
        {
            get { return _isOutOfContact_ControlModule_9; }
            set
            {
                _isOutOfContact_ControlModule_9 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_9));
            }
        }
        private bool _isOutOfContact_ControlModule_10 = true;
        /// <summary>
        /// 从控模块10是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_10
        {
            get { return _isOutOfContact_ControlModule_10; }
            set
            {
                _isOutOfContact_ControlModule_10 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_10));
            }
        }
        private bool _isOutOfContact_ControlModule_11 = true;
        /// <summary>
        /// 从控模块11是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_11
        {
            get { return _isOutOfContact_ControlModule_11; }
            set
            {
                _isOutOfContact_ControlModule_11 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_11));
            }
        }
        private bool _isOutOfContact_ControlModule_12 = true;
        /// <summary>
        /// 从控模块12是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_12
        {
            get { return _isOutOfContact_ControlModule_12; }
            set
            {
                _isOutOfContact_ControlModule_12 = value;
            }
        }
        private bool _isOutOfContact_ControlModule_13 = true;
        /// <summary>
        /// 从控模块13是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_13
        {
            get { return _isOutOfContact_ControlModule_13; }
            set
            {
                _isOutOfContact_ControlModule_13 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_13));
            }
        }
        private bool _isOutOfContact_ControlModule_14 = true;
        /// <summary>
        /// 从控模块14是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_14
        {
            get { return _isOutOfContact_ControlModule_14; }
            set
            {
                _isOutOfContact_ControlModule_14 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_14));
            }
        }
        private bool _isOutOfContact_ControlModule_15 = true;
        /// <summary>
        /// 从控模块15是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_15
        {
            get { return _isOutOfContact_ControlModule_15; }
            set
            {
                _isOutOfContact_ControlModule_15 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_15));
            }
        }
        private bool _isOutOfContact_ControlModule_16 = true;
        /// <summary>
        /// 从控模块16是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_16
        {
            get { return _isOutOfContact_ControlModule_16; }
            set
            {
                _isOutOfContact_ControlModule_16 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_16));
            }
        }
        private bool _isOutOfContact_ControlModule_17 = true;
        /// <summary>
        /// 从控模块17是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_17
        {
            get { return _isOutOfContact_ControlModule_17; }
            set
            {
                _isOutOfContact_ControlModule_17 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_17));
            }
        }
        private bool _isOutOfContact_ControlModule_18 = true;
        /// <summary>
        /// 从控模块18是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_18
        {
            get { return _isOutOfContact_ControlModule_18; }
            set
            {
                _isOutOfContact_ControlModule_18 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_18));
            }
        }
        private bool _isOutOfContact_ControlModule_19 = true;
        /// <summary>
        /// 从控模块19是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_19
        {
            get { return _isOutOfContact_ControlModule_19; }
            set
            {
                _isOutOfContact_ControlModule_19 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_19));
            }
        }
        private bool _isOutOfContact_ControlModule_20 = true;
        /// <summary>
        /// 从控模块20是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_20
        {
            get { return _isOutOfContact_ControlModule_20; }
            set
            {
                _isOutOfContact_ControlModule_20 = value;
            }
        }
        private bool _isOutOfContact_ControlModule_21 = true;
        /// <summary>
        /// 从控模块21是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_21
        {
            get { return _isOutOfContact_ControlModule_21; }
            set
            {
                _isOutOfContact_ControlModule_21 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_21));
            }
        }
        private bool _isOutOfContact_ControlModule_22 = true;
        /// <summary>
        /// 从控模块22是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_22
        {
            get { return _isOutOfContact_ControlModule_22; }
            set
            {
                _isOutOfContact_ControlModule_22 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_22));
            }
        }
        private bool _isOutOfContact_ControlModule_23 = true;
        /// <summary>
        /// 从控模块23是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_23
        {
            get { return _isOutOfContact_ControlModule_23; }
            set
            {
                _isOutOfContact_ControlModule_23 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_23));
            }
        }
        private bool _isOutOfContact_ControlModule_24 = true;
        /// <summary>
        /// 从控模块24是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_24
        {
            get { return _isOutOfContact_ControlModule_24; }
            set
            {
                _isOutOfContact_ControlModule_24 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_24));
            }
        }
        private bool _isOutOfContact_ControlModule_25 = true;
        /// <summary>
        /// 从控模块25是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_25
        {
            get { return _isOutOfContact_ControlModule_25; }
            set
            {
                _isOutOfContact_ControlModule_25 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_25));
            }
        }
        private bool _isOutOfContact_ControlModule_26 = true;
        /// <summary>
        /// 从控模块26是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_26
        {
            get { return _isOutOfContact_ControlModule_26; }
            set
            {
                _isOutOfContact_ControlModule_26 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_26));
            }
        }
        private bool _isOutOfContact_ControlModule_27 = true;
        /// <summary>
        /// 从控模块27是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_27
        {
            get { return _isOutOfContact_ControlModule_27; }
            set
            {
                _isOutOfContact_ControlModule_27 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_27));
            }
        }
        private bool _isOutOfContact_ControlModule_28 = true;
        /// <summary>
        /// 从控模块28是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_28
        {
            get { return _isOutOfContact_ControlModule_28; }
            set
            {
                _isOutOfContact_ControlModule_28 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_28));
            }
        }
        private bool _isOutOfContact_ControlModule_29 = true;
        /// <summary>
        /// 从控模块29是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_29
        {
            get { return _isOutOfContact_ControlModule_29; }
            set
            {
                _isOutOfContact_ControlModule_29 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_29));
            }
        }
        private bool _isOutOfContact_ControlModule_30 = true;
        /// <summary>
        /// 从控模块30是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_30
        {
            get { return _isOutOfContact_ControlModule_30; }
            set
            {
                _isOutOfContact_ControlModule_30 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_30));
            }
        }
        private bool _isOutOfContact_ControlModule_31 = true;
        /// <summary>
        /// 从控模块31是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_31
        {
            get { return _isOutOfContact_ControlModule_31; }
            set
            {
                _isOutOfContact_ControlModule_31 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_31));
            }
        }
        private bool _isOutOfContact_ControlModule_32 = true;
        /// <summary>
        /// 从控模块32是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_32
        {
            get { return _isOutOfContact_ControlModule_32; }
            set
            {
                _isOutOfContact_ControlModule_32 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_32));
            }
        }
        private bool _isOutOfContact_ControlModule_33 = true;
        /// <summary>
        /// 从控模块33是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_33
        {
            get { return _isOutOfContact_ControlModule_33; }
            set
            {
                _isOutOfContact_ControlModule_33 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_33));
            }
        }
        private bool _isOutOfContact_ControlModule_34 = true;
        /// <summary>
        /// 从控模块34是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_34
        {
            get { return _isOutOfContact_ControlModule_34; }
            set
            {
                _isOutOfContact_ControlModule_34 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_34));
            }
        }
        private bool _isOutOfContact_ControlModule_35 = true;
        /// <summary>
        /// 从控模块35是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_35
        {
            get { return _isOutOfContact_ControlModule_35; }
            set
            {
                _isOutOfContact_ControlModule_35 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_35));
            }
        }
        private bool _isOutOfContact_ControlModule_36 = true;
        /// <summary>
        /// 从控模块36是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_36
        {
            get { return _isOutOfContact_ControlModule_36; }
            set
            {
                _isOutOfContact_ControlModule_36 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_36));
            }
        }
        private bool _isOutOfContact_ControlModule_37 = true;
        /// <summary>
        /// 从控模块37是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_37
        {
            get { return _isOutOfContact_ControlModule_37; }
            set
            {
                _isOutOfContact_ControlModule_37 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_37));
            }
        }
        private bool _isOutOfContact_ControlModule_38 = true;
        /// <summary>
        /// 从控模块38是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_38
        {
            get { return _isOutOfContact_ControlModule_38; }
            set
            {
                _isOutOfContact_ControlModule_38 = value;
            }
        }
        private bool _isOutOfContact_ControlModule_39 = true;
        /// <summary>
        /// 从控模块39是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_39
        {
            get { return _isOutOfContact_ControlModule_39; }
            set
            {
                _isOutOfContact_ControlModule_39 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_39));
            }
        }
        private bool _isOutOfContact_ControlModule_40 = true;
        /// <summary>
        /// 从控模块40是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_40
        {
            get { return _isOutOfContact_ControlModule_40; }
            set
            {
                _isOutOfContact_ControlModule_40 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_40));
            }
        }
        private bool _isOutOfContact_ControlModule_41 = true;
        /// <summary>
        /// 从控模块41是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_41
        {
            get { return _isOutOfContact_ControlModule_41; }
            set
            {
                _isOutOfContact_ControlModule_41 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_41));
            }
        }
        private bool _isOutOfContact_ControlModule_42 = true;
        /// <summary>
        /// 从控模块42是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_42
        {
            get { return _isOutOfContact_ControlModule_42; }
            set
            {
                _isOutOfContact_ControlModule_42 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_42));
            }
        }
        private bool _isOutOfContact_ControlModule_43 = true;
        /// <summary>
        /// 从控模块43是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_43
        {
            get { return _isOutOfContact_ControlModule_43; }
            set
            {
                _isOutOfContact_ControlModule_43 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_43));
            }
        }
        private bool _isOutOfContact_ControlModule_44 = true;
        /// <summary>
        /// 从控模块44是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_44
        {
            get { return _isOutOfContact_ControlModule_44; }
            set
            {
                _isOutOfContact_ControlModule_44 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_44));
            }
        }
        private bool _isOutOfContact_ControlModule_45 = true;
        /// <summary>
        /// 从控模块45是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_45
        {
            get { return _isOutOfContact_ControlModule_45; }
            set
            {
                _isOutOfContact_ControlModule_45 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_45));
            }
        }
        private bool _isOutOfContact_ControlModule_46 = true;
        /// <summary>
        /// 从控模块46是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_46
        {
            get { return _isOutOfContact_ControlModule_46; }
            set
            {
                _isOutOfContact_ControlModule_46 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_46));
            }
        }
        private bool _isOutOfContact_ControlModule_47 = true;
        /// <summary>
        /// 从控模块47是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_47
        {
            get { return _isOutOfContact_ControlModule_47; }
            set
            {
                _isOutOfContact_ControlModule_47 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_47));
            }
        }
        private bool _isOutOfContact_ControlModule_48 = true;
        /// <summary>
        /// 从控模块48是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_48
        {
            get { return _isOutOfContact_ControlModule_48; }
            set
            {
                _isOutOfContact_ControlModule_48 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_48));
            }
        }
        private bool _isOutOfContact_ControlModule_49 = true;
        /// <summary>
        /// 从控模块49是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_49
        {
            get { return _isOutOfContact_ControlModule_49; }
            set
            {
                _isOutOfContact_ControlModule_49 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_49));
            }
        }
        private bool _isOutOfContact_ControlModule_50 = true;
        /// <summary>
        /// 从控模块50是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_50
        {
            get { return _isOutOfContact_ControlModule_50; }
            set
            {
                _isOutOfContact_ControlModule_50 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_50));
            }
        }
        private bool _isOutOfContact_ControlModule_51 = true;
        /// <summary>
        /// 从控模块51是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_51
        {
            get { return _isOutOfContact_ControlModule_51; }
            set
            {
                _isOutOfContact_ControlModule_51 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_51));
            }
        }
        private bool _isOutOfContact_ControlModule_52 = true;
        /// <summary>
        /// 从控模块52是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_52
        {
            get { return _isOutOfContact_ControlModule_52; }
            set
            {
                _isOutOfContact_ControlModule_52 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_52));
            }
        }
        private bool _isOutOfContact_ControlModule_53 = true;
        /// <summary>
        /// 从控模块53是否失联
        /// </summary>  
        public bool IsOutOfContact_ControlModule_53
        {
            get { return _isOutOfContact_ControlModule_53; }
            set
            {
                _isOutOfContact_ControlModule_53 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_53));
            }
        }
        private bool _isOutOfContact_ControlModule_54 = true;
        /// <summary>
        /// 从控模块54是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_54
        {
            get { return _isOutOfContact_ControlModule_54; }
            set
            {
                _isOutOfContact_ControlModule_54 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_54));
            }
        }
        private bool _isOutOfContact_ControlModule_55 = true;
        /// <summary>
        /// 从控模块55是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_55
        {
            get { return _isOutOfContact_ControlModule_55; }
            set
            {
                _isOutOfContact_ControlModule_55 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_55));
            }
        }
        private bool _isOutOfContact_ControlModule_56 = true;
        /// <summary>
        /// 从控模块56是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_56
        {
            get { return _isOutOfContact_ControlModule_56; }
            set
            {
                _isOutOfContact_ControlModule_56 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_56));
            }
        }
        private bool _isOutOfContact_ControlModule_57 = true;
        /// <summary>
        /// 从控模块57是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_57
        {
            get { return _isOutOfContact_ControlModule_57; }
            set
            {
                _isOutOfContact_ControlModule_57 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_57));
            }
        }
        private bool _isOutOfContact_ControlModule_58 = true;
        /// <summary>
        /// 从控模块58是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_58
        {
            get { return _isOutOfContact_ControlModule_58; }
            set
            {
                _isOutOfContact_ControlModule_58 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_58));
            }
        }
        private bool _isOutOfContact_ControlModule_59 = true;
        /// <summary>
        /// 从控模块59是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_59
        {
            get { return _isOutOfContact_ControlModule_59; }
            set
            {
                _isOutOfContact_ControlModule_59 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_59));
            }
        }
        private bool _isOutOfContact_ControlModule_60 = true;
        /// <summary>
        /// 从控模块60是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_60
        {
            get { return _isOutOfContact_ControlModule_60; }
            set
            {
                _isOutOfContact_ControlModule_60 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_60));
            }
        }
        private bool _isOutOfContact_ControlModule_61 = true;
        /// <summary>
        /// 从控模块61是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_61
        {
            get { return _isOutOfContact_ControlModule_61; }
            set
            {
                _isOutOfContact_ControlModule_61 = value;
            }
        }
        private bool _isOutOfContact_ControlModule_62 = true;
        /// <summary>
        /// 从控模块62是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_62
        {
            get { return _isOutOfContact_ControlModule_62; }
            set
            {
                _isOutOfContact_ControlModule_62 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_62));
            }
        }
        private bool _isOutOfContact_ControlModule_63 = true;
        /// <summary>
        /// 从控模块63是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_63
        {
            get { return _isOutOfContact_ControlModule_63; }
            set
            {
                _isOutOfContact_ControlModule_63 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_63));
            }
        }
        private bool _isOutOfContact_ControlModule_64 = true;
        /// <summary>
        /// 从控模块64是否失联
        /// </summary>
        public bool IsOutOfContact_ControlModule_64
        {
            get { return _isOutOfContact_ControlModule_64; }
            set
            {
                _isOutOfContact_ControlModule_64 = value;
                OnPropertyChanged(nameof(IsOutOfContact_ControlModule_64));
            }
        }

        private bool _isDisconnect_Bat_1 = true;
        /// <summary>
        /// 电池1是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_1
        {
            get { return _isDisconnect_Bat_1; }
            set
            {
                _isDisconnect_Bat_1 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_1));
            }
        }
        private bool _isDisconnect_Bat_2 = true;
        /// <summary>
        /// 电池2是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_2
        {
            get { return _isDisconnect_Bat_2; }
            set
            {
                _isDisconnect_Bat_2 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_2));
            }
        }
        private bool _isDisconnect_Bat_3 = true;
        /// <summary>
        /// 电池3是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_3
        {
            get { return _isDisconnect_Bat_3; }
            set
            {
                _isDisconnect_Bat_3 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_3));
            }
        }
        private bool _isDisconnect_Bat_4 = true;
        /// <summary>
        /// 电池4是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_4
        {
            get { return _isDisconnect_Bat_4; }
            set
            {
                _isDisconnect_Bat_4 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_4));
            }
        }
        private bool _isDisconnect_Bat_5 = true;
        /// <summary>
        /// 电池5是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_5
        {
            get { return _isDisconnect_Bat_5; }
            set
            {
                _isDisconnect_Bat_5 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_5));
            }
        }
        private bool _isDisconnect_Bat_6 = true;
        /// <summary>
        /// 电池6是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_6
        {
            get { return _isDisconnect_Bat_6; }
            set
            {
                _isDisconnect_Bat_6 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_6));
            }
        }
        private bool _isDisconnect_Bat_7 = true;
        /// <summary>
        /// 电池7是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_7
        {
            get { return _isDisconnect_Bat_7; }
            set
            {
                _isDisconnect_Bat_7 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_7));
            }
        }
        private bool _isDisconnect_Bat_8 = true;
        /// <summary>
        /// 电池8是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_8
        {
            get { return _isDisconnect_Bat_8; }
            set
            {
                _isDisconnect_Bat_8 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_8));
            }
        }
        private bool _isDisconnect_Bat_9 = true;
        /// <summary>
        /// 电池9是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_9
        {
            get { return _isDisconnect_Bat_9; }
            set
            {
                _isDisconnect_Bat_9 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_9));
            }
        }
        private bool _isDisconnect_Bat_10 = true;
        /// <summary>
        /// 电池10是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_10
        {
            get { return _isDisconnect_Bat_10; }
            set
            {
                _isDisconnect_Bat_10 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_10));
            }
        }
        private bool _isDisconnect_Bat_11 = true;
        /// <summary>
        /// 电池11是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_11
        {
            get { return _isDisconnect_Bat_11; }
            set
            {
                _isDisconnect_Bat_11 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_11));
            }
        }
        private bool _isDisconnect_Bat_12 = true;
        /// <summary>
        /// 电池12是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_12
        {
            get { return _isDisconnect_Bat_12; }
            set
            {
                _isDisconnect_Bat_12 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_12));
            }
        }
        private bool _isDisconnect_Bat_13 = true;
        /// <summary>
        /// 电池13是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_13
        {
            get { return _isDisconnect_Bat_13; }
            set
            {
                _isDisconnect_Bat_13 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_13));
            }
        }
        private bool _isDisconnect_Bat_14 = true;
        /// <summary>
        /// 电池14是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_14
        {
            get { return _isDisconnect_Bat_14; }
            set
            {
                _isDisconnect_Bat_14 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_14));
            }
        }
        private bool _isDisconnect_Bat_15 = true;
        /// <summary>
        /// 电池15是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_15
        {
            get { return _isDisconnect_Bat_15; }
            set
            {
                _isDisconnect_Bat_15 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_15));
            }
        }
        private bool _isDisconnect_Bat_16 = true;
        /// <summary>
        /// 电池16是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_16
        {
            get { return _isDisconnect_Bat_16; }
            set
            {
                _isDisconnect_Bat_16 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_16));
            }
        }
        private bool _isDisconnect_Bat_17 = true;
        /// <summary>
        /// 电池17是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_17
        {
            get { return _isDisconnect_Bat_17; }
            set
            {
                _isDisconnect_Bat_17 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_17));
            }
        }
        private bool _isDisconnect_Bat_18 = true;
        /// <summary>
        /// 电池18是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_18
        {
            get { return _isDisconnect_Bat_18; }
            set
            {
                _isDisconnect_Bat_18 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_18));
            }
        }
        private bool _isDisconnect_Bat_19 = true;
        /// <summary>
        /// 电池19是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_19
        {
            get { return _isDisconnect_Bat_19; }
            set
            {
                _isDisconnect_Bat_19 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_19));
            }
        }
        private bool _isDisconnect_Bat_20 = true;
        /// <summary>
        /// 电池20是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_20
        {
            get { return _isDisconnect_Bat_20; }
            set
            {
                _isDisconnect_Bat_20 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_20));
            }
        }
        private bool _isDisconnect_Bat_21 = true;
        /// <summary>
        /// 电池21是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_21
        {
            get { return _isDisconnect_Bat_21; }
            set
            {
                _isDisconnect_Bat_21 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_21));
            }
        }
        private bool _isDisconnect_Bat_22 = true;
        /// <summary>
        /// 电池22是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_22
        {
            get { return _isDisconnect_Bat_22; }
            set
            {
                _isDisconnect_Bat_22 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_22));
            }
        }
        private bool _isDisconnect_Bat_23 = true;
        /// <summary>
        /// 电池23是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_23
        {
            get { return _isDisconnect_Bat_23; }
            set
            {
                _isDisconnect_Bat_23 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_23));
            }
        }
        private bool _isDisconnect_Bat_24 = true;
        /// <summary>
        /// 电池24是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_24
        {
            get { return _isDisconnect_Bat_24; }
            set
            {
                _isDisconnect_Bat_24 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_24));
            }
        }
        private bool _isDisconnect_Bat_25 = true;
        /// <summary>
        /// 电池25是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_25
        {
            get { return _isDisconnect_Bat_25; }
            set
            {
                _isDisconnect_Bat_25 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_25));
            }
        }
        private bool _isDisconnect_Bat_26 = true;
        /// <summary>
        /// 电池26是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_26
        {
            get { return _isDisconnect_Bat_26; }
            set
            {
                _isDisconnect_Bat_26 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_26));
            }
        }
        private bool _isDisconnect_Bat_27 = true;
        /// <summary>
        /// 电池27是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_27
        {
            get { return _isDisconnect_Bat_27; }
            set
            {
                _isDisconnect_Bat_27 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_27));
            }
        }
        private bool _isDisconnect_Bat_28 = true;
        /// <summary>
        /// 电池28是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_28
        {
            get { return _isDisconnect_Bat_28; }
            set
            {
                _isDisconnect_Bat_28 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_28));
            }
        }
        private bool _isDisconnect_Bat_29 = true;
        /// <summary>
        /// 电池29是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_29
        {
            get { return _isDisconnect_Bat_29; }
            set
            {
                _isDisconnect_Bat_29 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_29));
            }
        }
        private bool _isDisconnect_Bat_30 = true;
        /// <summary>
        /// 电池30是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_30
        {
            get { return _isDisconnect_Bat_30; }
            set
            {
                _isDisconnect_Bat_30 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_30));
            }
        }
        private bool _isDisconnect_Bat_31 = true;
        /// <summary>
        /// 电池31是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_31
        {
            get { return _isDisconnect_Bat_31; }
            set
            {
                _isDisconnect_Bat_31 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_31));
            }
        }
        private bool _isDisconnect_Bat_32 = true;
        /// <summary>
        /// 电池32是否掉线
        /// </summary>
        public bool IsDisconnect_Bat_32
        {
            get { return _isDisconnect_Bat_32; }
            set
            {
                _isDisconnect_Bat_32 = value;
                OnPropertyChanged(nameof(IsDisconnect_Bat_32));
            }
        }

        public ObservableCollection<int> ModuleNumList { get; } = new ObservableCollection<int>(Enumerable.Range(1, 60));
        private int _selectedModuleNum;
        /// <summary>
        /// 从控模块号
        /// </summary>
        public int SelectedModuleNum
        {
            get { return _selectedModuleNum; }
            set
            {
                _selectedModuleNum = value;
                OnPropertyChanged(nameof(SelectedModuleNum));
            }
        }

        private string _ICType;
        /// <summary>
        /// IC 类型
        /// </summary>
        public string ICType
        {
            get { return _ICType; }
            set
            {
                _ICType = value;
                OnPropertyChanged(nameof(ICType));
            }
        }

        private string _ICNum;
        /// <summary>
        /// IC 序号
        /// </summary>
        public string ICNum
        {
            get { return _ICNum; }
            set
            {
                _ICNum = value;
                OnPropertyChanged(nameof(ICNum));
            }
        }

        private string _BatteryVoltageCollectionHarness;
        /// <summary>
        /// 电池电压采集线束
        /// </summary>
        public string BatteryVoltageCollectionHarness
        {
            get { return _BatteryVoltageCollectionHarness; }
            set
            {
                _BatteryVoltageCollectionHarness = value;
                OnPropertyChanged(nameof(BatteryVoltageCollectionHarness));
            }
        }

        private string _BatteryNumber;
        /// <summary>
        /// 电池节号
        /// </summary>
        public string BatteryNumber
        {
            get { return _BatteryNumber; }
            set
            {
                _BatteryNumber = value;
                OnPropertyChanged(nameof(BatteryNumber));
            }
        }

        private string _BatteryTemperatureCollectionHarness;
        /// <summary>
        /// 电池温度采集线束
        /// </summary>
        public string BatteryTemperatureCollectionHarness
        {
            get { return _BatteryTemperatureCollectionHarness; }
            set
            {
                _BatteryTemperatureCollectionHarness = value;
                OnPropertyChanged(nameof(BatteryTemperatureCollectionHarness));
            }
        }

        private string _TemperaturesNumber;
        /// <summary>
        /// 采集线温度个数
        /// </summary>
        public string TemperaturesNumber
        {
            get { return _TemperaturesNumber; }
            set
            {
                _TemperaturesNumber = value;
                OnPropertyChanged(nameof(TemperaturesNumber));
            }
        }

        private string _BatteryTotalNumber;
        /// <summary>
        /// 总电池节数
        /// </summary>
        public string BatteryTotalNumber
        {
            get { return _BatteryTotalNumber; }
            set
            {
                _BatteryTotalNumber = value;
                OnPropertyChanged(nameof(BatteryTotalNumber));
            }
        }




        public ObservableCollection<string> PowerOnAndOffList { get; } = new ObservableCollection<string>
        {
             "1：上电",
             "2：下电",
             "3：所有簇故障下电",
             "其他：无效"
        };

        private string _selectedPowerOnAndOff;
        /// <summary>
        /// 控制主控上下电
        /// 1：上电
        /// 2：下电
        /// 3：所有簇故障下电
        /// 其他：无效
        /// </summary>
        public string SelectedPowerOnAndOff
        {
            get { return _selectedPowerOnAndOff; }
            set
            {
                _selectedPowerOnAndOff = value;
                OnPropertyChanged(nameof(SelectedPowerOnAndOff));
            }
        }

        public ObservableCollection<string> OperatingInstructionsList { get; } = new ObservableCollection<string>
        {
             "0：所有簇执行上电或者下电指令",
             "1：簇1执行上电或者下电指令",
             "2：簇2执行上电或者下电指令",
             "3：簇3执行上电或者下电指令",
             "4：簇4执行上电或者下电指令",
             "5：簇5执行上电或者下电指令",
             "6：簇6执行上电或者下电指令",
             "7：簇7执行上电或者下电指令",
             "8：簇8执行上电或者下电指令",
             "9：簇9执行上电或者下电指令",
             "10：簇10执行上电或者下电指令",
             "11：簇11执行上电或者下电指令",
             "12：簇12执行上电或者下电指令",
             "13：簇13执行上电或者下电指令",
             "14：簇14执行上电或者下电指令",
             "15：簇15执行上电或者下电指令",
             "16：簇16执行上电或者下电指令",
             "17：簇17执行上电或者下电指令",
             "18：簇18执行上电或者下电指令",
             "19：簇19执行上电或者下电指令",
             "20：簇20执行上电或者下电指令"

        };

        private string _selectedOperatingInstructions;
        /// <summary>
        /// 控制主控上下电说明
        /// 0：所有簇执行上电或者下电指令
        /// 1~20：对应簇执行上电或者下电指令
        /// </summary>
        public string SelectedOperatingInstructions
        {
            get { return _selectedOperatingInstructions; }
            set
            {
                _selectedOperatingInstructions = value;
                OnPropertyChanged(nameof(SelectedOperatingInstructions));
            }
        }

        public ObservableCollection<string> FaultResetList { get; } = new ObservableCollection<string>
        {
             "0x55：故障复归有效",
             "其他：无效"
        };

        private string _selectedFaultReset;
        /// <summary>
        /// 故障复归指令
        /// 0x55：故障复归有效
        /// 其他：无效
        /// </summary>
        public string SelectedFaultReset
        {
            get { return _selectedFaultReset; }
            set
            {
                _selectedFaultReset = value;
                OnPropertyChanged(nameof(SelectedFaultReset));
            }
        }

        public ObservableCollection<string> InsulationDetectionList { get; } = new ObservableCollection<string>
        {
             "1：开启绝缘检测",
             "2：关闭绝缘检测",
             "其他：无效"
        };

        private string _selectedInsulationDetection;
        /// <summary>
        /// 绝缘检测控制指令
        /// 1：开启绝缘检测
        /// 2：关闭绝缘检测
        /// 其他：无效
        /// </summary>
        public string SelectedInsulationDetection
        {
            get { return _selectedInsulationDetection; }
            set
            {
                _selectedInsulationDetection = value;
                OnPropertyChanged(nameof(SelectedInsulationDetection));
            }
        }

        public ObservableCollection<string> InsulationDetectionStatusList { get; } = new ObservableCollection<string>
        {
             "1：绝缘检测开启",
             "2：绝缘检测关闭"
        };

        private string _InsulationDetectionStatus;
        /// <summary>
        /// 绝缘检测状态
        /// 1：绝缘检测开启
        /// 2：绝缘检测关闭
        /// </summary>
        public string InsulationDetectionStatus
        {
            get { return _InsulationDetectionStatus; }
            set
            {
                _InsulationDetectionStatus = value;
                OnPropertyChanged(nameof(InsulationDetectionStatus));
            }
        }

        private bool _isChecked_Hall1_effective;
        /// <summary>
        /// 霍尔1有效
        /// </summary>
        public bool IsChecked_Hall1_effective
        {
            get { return _isChecked_Hall1_effective; }
            set
            {
                _isChecked_Hall1_effective = value;
                OnPropertyChanged(nameof(IsChecked_Hall1_effective));
                if (value) _isChecked_Hall1_invalid = false; // 开关互斥
            }
        }

        private bool _isChecked_Hall1_invalid;
        /// <summary>
        /// 霍尔1无效
        /// </summary>
        public bool IsChecked_Hall1_invalid
        {
            get { return _isChecked_Hall1_invalid; }
            set
            {
                _isChecked_Hall1_invalid = value;
                OnPropertyChanged(nameof(IsChecked_Hall1_invalid));
                if (value) _isChecked_Hall1_effective = false; // 开关互斥
            }
        }

        private bool _isChecked_Hall2_effective;
        /// <summary>
        /// 霍尔2有效
        /// </summary>
        public bool IsChecked_Hall2_effective
        {
            get { return _isChecked_Hall2_effective; }
            set
            {
                _isChecked_Hall2_effective = value;
                OnPropertyChanged(nameof(IsChecked_Hall2_effective));
                if (value) _isChecked_Hall2_invalid = false; // 开关互斥
            }
        }

        private bool _isChecked_Hall2_invalid;
        /// <summary>
        /// 霍尔2无效
        /// </summary>
        public bool IsChecked_Hall2_invalid
        {
            get { return _isChecked_Hall2_invalid; }
            set
            {
                _isChecked_Hall2_invalid = value;
                OnPropertyChanged(nameof(IsChecked_Hall2_invalid));
                if (value) _isChecked_Hall2_effective = false; // 开关互斥
            }
        }

        private bool _isChecked_Hall3_effective;
        /// <summary>
        /// 霍尔3有效
        /// </summary>
        public bool IsChecked_Hall3_effective
        {
            get { return _isChecked_Hall3_effective; }
            set
            {
                _isChecked_Hall3_effective = value;
                OnPropertyChanged(nameof(IsChecked_Hall3_effective));
                if (value) _isChecked_Hall3_invalid = false; // 开关互斥
            }
        }
        private bool _isChecked_Hall3_invalid;
        /// <summary>
        /// 霍尔3无效
        /// </summary>
        public bool IsChecked_Hall3_invalid
        {
            get { return _isChecked_Hall3_invalid; }
            set
            {
                _isChecked_Hall3_invalid = value;
                OnPropertyChanged(nameof(IsChecked_Hall3_invalid));
                if (value) _isChecked_Hall3_effective = false; // 开关互斥
            }
        }

        private bool _isChecked_DoubleHall;
        /// <summary>
        /// 双霍尔传感器
        /// </summary>
        public bool IsChecked_DoubleHall
        {
            get { return _isChecked_DoubleHall; }
            set
            {
                _isChecked_DoubleHall = value;
                OnPropertyChanged(nameof(IsChecked_DoubleHall));
                if (value) _isChecked_NonDoubleHall = false; // 开关互斥
            }
        }

        private bool _isChecked_NonDoubleHall;
        /// <summary>
        /// 非双霍尔传感器
        /// </summary>
        public bool IsChecked_NonDoubleHall
        {
            get { return _isChecked_NonDoubleHall; }
            set
            {
                _isChecked_NonDoubleHall = value;
                OnPropertyChanged(nameof(IsChecked_NonDoubleHall));
                if (value) _isChecked_DoubleHall = false; // 开关互斥
            }
        }


        private bool _isBatteryVoltageChecked;
        /// <summary>
        /// 电池电压
        /// </summary>
        public bool IsChecked_BatteryVoltage
        {
            get => _isBatteryVoltageChecked;
            set
            {
                SetProperty(ref _isBatteryVoltageChecked, value);
                if (value)
                {
                    // 更新 DataGrid 数据为电池电压
                    batteryVoltageTimer = new System.Threading.Timer(TimerCallBack_batteryVoltage, 0, 1000, Convert.ToInt32(DataCollectionInterval));
                }
                else
                {
                    batteryVoltageTimer.Dispose();
                }
            }
        }



        private bool _isBatterySOCChecked;

        /// <summary>
        /// 电池SOC
        /// </summary>
        public bool IsChecked_BatterySOC
        {
            get => _isBatterySOCChecked;
            set
            {
                SetProperty(ref _isBatterySOCChecked, value);
                if (value)
                {
                    // 更新 DataGrid 数据为电池SOC
                    batterySocTimer = new System.Threading.Timer(TimerCallBack_batterySoc, 0, 1000, Convert.ToInt32(DataCollectionInterval));
                }
                else
                {
                    batterySocTimer.Dispose();
                }

            }
        }



        private bool _isBatterySOHChecked;
        /// <summary>
        /// 电池SOH
        /// </summary>
        public bool IsChecked_BatterySOH
        {
            get => _isBatterySOHChecked;
            set
            {
                SetProperty(ref _isBatterySOHChecked, value);
                if (value)
                {
                    // 更新 DataGrid 数据为电池SOH
                    batterySohTimer = new System.Threading.Timer(TimerCallBack_batterySoh, 0, 1000, Convert.ToInt32(DataCollectionInterval));
                }
                else
                {
                    batterySohTimer.Dispose();
                }
            }
        }

        private bool _isBatteryTemperatureChecked;
        /// <summary>
        /// 电池温度
        /// </summary>
        public bool IsChecked_BatteryTemperature
        {
            get => _isBatteryTemperatureChecked;
            set
            {
                SetProperty(ref _isBatteryTemperatureChecked, value);
                if (value)
                {
                    // 更新 DataGrid 数据为电池温度
                    batteryTemperatureTimer = new System.Threading.Timer(TimerCallBack_batteryTemperature, 0, 1000, Convert.ToInt32(DataCollectionInterval));
                }
                else
                {
                    batteryTemperatureTimer.Dispose();
                }
            }
        }

        private bool _isChecked_SupplyVoltage;
        /// <summary>
        /// 供电电压
        /// </summary>
        public bool IsChecked_SupplyVoltage
        {
            get => _isChecked_SupplyVoltage;
            set
            {
                SetProperty(ref _isChecked_SupplyVoltage, value);
                if (value)
                {
                    // 更新 DataGrid 数据为供电电压
                    supplyVoltageTimer = new System.Threading.Timer(TimerCallBack_supplyVoltage, 0, 1000, Convert.ToInt32(DataCollectionInterval));
                }
                else
                {
                    supplyVoltageTimer.Dispose();
                }
            }
        }

        private bool _isChecked_PoleTemperature;
        /// <summary>
        /// 极柱温度
        /// </summary>
        public bool IsChecked_PoleTemperature
        {
            get => _isChecked_PoleTemperature;
            set
            {
                SetProperty(ref _isChecked_PoleTemperature, value);
                if (value)
                {
                    // 更新 DataGrid 数据为极柱温度
                    poleTemperatureTimer = new System.Threading.Timer(TimerCallBack_poleTemperature, 0, 1000, Convert.ToInt32(DataCollectionInterval));
                }
                else
                {
                    poleTemperatureTimer.Dispose();
                }
            }
        }

        private bool _isChecked_ModuleTotalVoltage;
        /// <summary>
        /// 模块总电压
        /// </summary>
        public bool IsChecked_ModuleTotalVoltage
        {
            get => _isChecked_ModuleTotalVoltage;
            set
            {
                SetProperty(ref _isChecked_ModuleTotalVoltage, value);
                if (value)
                {
                    // 更新 DataGrid 数据为模块总电压
                    moduleTotalVoltageTimer = new System.Threading.Timer(TimerCallBack_moduleTotalVoltage, 0, 1000, Convert.ToInt32(DataCollectionInterval));
                }
                else
                {
                    moduleTotalVoltageTimer.Dispose();
                }
            }
        }

        // 记录报警信息的数据列表
        private ObservableCollection<AlarmMessageData> _alarmMessageDataList;
        public ObservableCollection<AlarmMessageData> AlarmMessageDataList
        {
            get { return _alarmMessageDataList; }
            set
            {
                _alarmMessageDataList = value;
                OnPropertyChanged(nameof(AlarmMessageDataList));
            }
        }
        RealtimeData_BMS1500V_BCU model = new RealtimeData_BMS1500V_BCU();


        private string _dataName;
        /// <summary>
        /// 表第1列 列标题名称
        /// </summary>
        public string DataName
        {
            get => _dataName;
            set => SetProperty(ref _dataName, value);
        }

        private string _indexName;
        /// <summary>
        /// 表第2列 列标题名称
        /// </summary>
        public string IndexName
        {
            get => _indexName;
            set => SetProperty(ref _indexName, value);
        }

        // 表第3-18列 列标题1-16
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string Value4 { get; set; }
        public string Value5 { get; set; }
        public string Value6 { get; set; }
        public string Value7 { get; set; }
        public string Value8 { get; set; }
        public string Value9 { get; set; }
        public string Value10 { get; set; }
        public string Value11 { get; set; }
        public string Value12 { get; set; }
        public string Value13 { get; set; }
        public string Value14 { get; set; }
        public string Value15 { get; set; }
        public string Value16 { get; set; }

        private string _dataCollectionInterval = "2000";
        /// <summary>
        /// 数据采集间隔(ms)
        /// </summary>
        public string DataCollectionInterval
        {
            get => _dataCollectionInterval;
            set => SetProperty(ref _dataCollectionInterval, value);
        }

        private bool _IsStartDataCollectionEnabled;
        /// <summary>
        /// 是否启用启动数据采集按钮
        /// </summary>
        public bool IsStartDataCollectionEnabled
        {
            get => _IsStartDataCollectionEnabled;
            set => SetProperty(ref _IsStartDataCollectionEnabled, value);
        }

        private bool _IsStopDataCollectionEnabled = true;
        /// <summary>
        /// 是否启用停止数据采集按钮
        /// </summary>
        public bool IsStopDataCollectionEnabled
        {
            get => _IsStopDataCollectionEnabled;
            set => SetProperty(ref _IsStopDataCollectionEnabled, value);
        }

        /// <summary>
        /// 是否启动数据保存
        /// </summary>
        public bool _isChecked_StartDataSaving;
        public bool IsChecked_StartDataSaving
        {
            get => _isChecked_StartDataSaving;
            set
            {
                SetProperty(ref _isChecked_StartDataSaving, value);
                if (value)
                {
                    DataSavingTimer = new System.Threading.Timer(TimerCallBack_DataSaving, 0, 1000, Convert.ToInt32(DataStorageInterval));
                }
                else
                {
                    DataSavingTimer.Dispose();
                }
            }
        }

        private string _dataStorageInterval = "1000";
        /// <summary>
        /// 数据保存间隔(ms)
        /// </summary>
        public string DataStorageInterval
        {
            get => _dataStorageInterval;
            set => SetProperty(ref _dataStorageInterval, value);
        }

        private string _textColor = "Default";
        /// <summary>
        /// DI、DO 文字颜色
        /// </summary>
        public string TextColor
        {
            get => _textColor;
            set => SetProperty(ref _textColor, value);
        }

        private string _groupTerminalVoltage;
        /// <summary>
        /// 组端电压(V)
        /// </summary>
        public string GroupTerminalVoltage
        {
            get => _groupTerminalVoltage;
            set => SetProperty(ref _groupTerminalVoltage, value);
        }

        private string _groupEndCurrent_1;
        /// <summary>
        /// 组端电流1(A)
        /// </summary>
        public string GroupEndCurrent_1
        {
            get => _groupEndCurrent_1;
            set => SetProperty(ref _groupEndCurrent_1, value);
        }

        private string _groupEndCurrent_2;
        /// <summary>
        /// 组端电流2(A)
        /// </summary>
        public string GroupEndCurrent_2
        {
            get => _groupEndCurrent_2;
            set => SetProperty(ref _groupEndCurrent_2, value);
        }

        private string _groupEndCurrent_3;
        /// <summary>
        /// 组端电流3(A)
        /// </summary>
        public string GroupEndCurrent_3
        {
            get => _groupEndCurrent_3;
            set => SetProperty(ref _groupEndCurrent_3, value);
        }

        private string _preChargeVoltage;
        /// <summary>
        /// 预充电压(V)
        /// </summary>
        public string PreChargeVoltage
        {
            get => _preChargeVoltage;
            set => SetProperty(ref _preChargeVoltage, value);
        }

        private string _insulationResistance_R_Positive;
        /// <summary>
        /// 绝缘电阻R+(KΩ)
        /// </summary>
        public string InsulationResistance_R_Positive
        {
            get => _insulationResistance_R_Positive;
            set => SetProperty(ref _insulationResistance_R_Positive, value);
        }


        private string _insulationResistance_R_Negative;
        /// <summary>
        /// 绝缘电阻R-(KΩ)
        /// </summary>
        public string InsulationResistance_R_Negative
        {
            get => _insulationResistance_R_Negative;
            set => SetProperty(ref _insulationResistance_R_Negative, value);
        }


        private string _moduleTemperature;
        /// <summary>
        /// 模块温度(℃)
        /// </summary>
        public string ModuleTemperature
        {
            get => _moduleTemperature;
            set => SetProperty(ref _moduleTemperature, value);
        }

        private string _supplyVoltage;
        /// <summary>
        /// 供电电压(V)
        /// </summary>
        public string SupplyVoltage
        {
            get => _supplyVoltage;
            set => SetProperty(ref _supplyVoltage, value);
        }

        private string _groupEndSOC;
        /// <summary>
        /// 组端SOC(%)
        /// </summary>
        public string GroupEndSOC
        {
            get => _groupEndSOC;
            set => SetProperty(ref _groupEndSOC, value);
        }

        private string _groupEndTemperature_1;
        /// <summary>
        /// 组端温度1(℃)
        /// </summary>
        public string GroupEndTemperature_1
        {
            get => _groupEndTemperature_1;
            set => SetProperty(ref _groupEndTemperature_1, value);
        }

        private string _groupEndTemperature_2;
        /// <summary>
        /// 组端温度2(℃)
        /// </summary>
        public string GroupEndTemperature_2
        {
            get => _groupEndTemperature_2;
            set => SetProperty(ref _groupEndTemperature_2, value);
        }

        private string _groupEndTemperature_3;
        /// <summary>
        /// 组端温度3(℃)
        /// </summary>
        public string GroupEndTemperature_3
        {
            get => _groupEndTemperature_3;
            set => SetProperty(ref _groupEndTemperature_3, value);
        }

        private string _groupEndTemperature_4;
        /// <summary>
        /// 组端温度4(℃)
        /// </summary>
        public string GroupEndTemperature_4
        {
            get => _groupEndTemperature_4;
            set => SetProperty(ref _groupEndTemperature_4, value);
        }



        private string _beforeTheMaximumValue;
        /// <summary>
        /// 最大值前一
        /// </summary>
        public string BeforeTheMaximumValue
        {
            get => _beforeTheMaximumValue;
            set => SetProperty(ref _beforeTheMaximumValue, value);
        }

        private string _topTwoMaximumValue;
        /// <summary>
        /// 最大值前二
        /// </summary>
        public string TopTwoMaximumValue
        {
            get => _topTwoMaximumValue;
            set => SetProperty(ref _topTwoMaximumValue, value);
        }


        private string _topThreeMaximumValue;
        /// <summary>
        /// 最大值前三
        /// </summary>
        public string TopThreeMaximumValue
        {
            get => _topThreeMaximumValue;
            set => SetProperty(ref _topThreeMaximumValue, value);
        }

        private string _rangeValue;
        /// <summary>
        /// 极差值
        /// </summary>
        public string RangeValue
        {
            get => _rangeValue;
            set => SetProperty(ref _rangeValue, value);
        }

        private string _averageValue;
        /// <summary>
        /// 平均值
        /// </summary>
        public string AverageValue
        {
            get => _averageValue;
            set => SetProperty(ref _averageValue, value);
        }


        private string _chargingBalance;
        /// <summary>
        /// 充电均衡
        /// </summary>
        public string ChargingBalance
        {
            get => _chargingBalance;
            set => SetProperty(ref _chargingBalance, value);
        }

        private string _dischargingBalance;
        /// <summary>
        /// 放电均衡
        /// </summary>
        public string DischargingBalance
        {
            get => _dischargingBalance;
            set => SetProperty(ref _dischargingBalance, value);
        }

        private string _beforeTheMinimumValue;
        /// <summary>
        /// 最小值前一
        /// </summary>
        public string BeforeTheMinimumValue
        {
            get => _beforeTheMinimumValue;
            set => SetProperty(ref _beforeTheMinimumValue, value);
        }

        private string _topTwoMinimumValue;
        /// <summary>
        /// 最小值前二
        /// </summary>
        public string TopTwoMinimumValue
        {
            get => _topTwoMinimumValue;
            set => SetProperty(ref _topTwoMinimumValue, value);
        }

        private string _topThreeMinimumValue;
        /// <summary>
        /// 最小值前三
        /// </summary>
        public string TopThreeMinimumValue
        {
            get => _topThreeMinimumValue;
            set => SetProperty(ref _topThreeMinimumValue, value);
        }

        private string _moduleNumber = "8";
        /// <summary>
        /// 模块个数
        /// </summary>
        public string ModuleNumber
        {
            get => _moduleNumber;
            set => SetProperty(ref _moduleNumber, value);
        }

        private string _batteryCellsNumber = "384";
        /// <summary>
        /// 电池节数
        /// </summary>
        public string BatteryCellsNumber
        {
            get => _batteryCellsNumber;
            set => SetProperty(ref _batteryCellsNumber, value);
        }

        private string _temperatureNumber = "224";
        /// <summary>
        /// 温度个数
        /// </summary>
        public string TemperatureNumber
        {
            get => _temperatureNumber;
            set => SetProperty(ref _temperatureNumber, value);
        }

        private string _poleNumber = "16";
        /// <summary>
        /// 极柱个数
        /// </summary>
        public string PoleNumber
        {
            get => _poleNumber;
            set => SetProperty(ref _poleNumber, value);
        }

        private string _dIStatus_DI1;
        /// <summary>
        /// DI1
        /// </summary>
        public string DIStatus_DI1
        {
            get => _dIStatus_DI1;
            set => SetProperty(ref _dIStatus_DI1, value);
        }


        private string _dIStatus_DI2;
        /// <summary>
        /// DI2
        /// </summary>
        public string DIStatus_DI2
        {
            get => _dIStatus_DI2;
            set => SetProperty(ref _dIStatus_DI2, value);
        }

        private string _dIStatus_DI3;
        /// <summary>
        /// DI3
        /// </summary>
        public string DIStatus_DI3
        {
            get => _dIStatus_DI3;
            set => SetProperty(ref _dIStatus_DI3, value);
        }

        private string _dIStatus_DI4;
        /// <summary>
        /// DI4
        /// </summary>
        public string DIStatus_DI4
        {
            get => _dIStatus_DI4;
            set => SetProperty(ref _dIStatus_DI4, value);
        }

        private string _dIStatus_DI5;
        /// <summary>
        /// DI5
        /// </summary>
        public string DIStatus_DI5
        {
            get => _dIStatus_DI5;
            set => SetProperty(ref _dIStatus_DI5, value);
        }

        private string _dIStatus_DI6;
        /// <summary>
        /// DI6
        /// </summary>
        public string DIStatus_DI6
        {
            get => _dIStatus_DI6;
            set => SetProperty(ref _dIStatus_DI6, value);
        }

        private string _dIStatus_DI7;
        /// <summary>
        /// DI7
        /// </summary>
        public string DIStatus_DI7
        {
            get => _dIStatus_DI7;
            set => SetProperty(ref _dIStatus_DI7, value);
        }

        private string _dIStatus_DI8;
        /// <summary>
        /// DI8
        /// </summary>
        public string DIStatus_DI8
        {
            get => _dIStatus_DI8;
            set => SetProperty(ref _dIStatus_DI8, value);
        }

        private string _dIStatus_DI9;
        /// <summary>
        /// DI9
        /// </summary>
        public string DIStatus_DI9
        {
            get => _dIStatus_DI9;
            set => SetProperty(ref _dIStatus_DI9, value);
        }

        private string _dIStatus_DI10;
        /// <summary>
        /// DI10
        /// </summary>
        public string DIStatus_DI10
        {
            get => _dIStatus_DI10;
            set => SetProperty(ref _dIStatus_DI10, value);
        }
        private string _dOStatus_DO1;
        /// <summary>
        /// DO1
        /// </summary>
        public string DOStatus_DO1
        {
            get => _dOStatus_DO1;
            set => SetProperty(ref _dOStatus_DO1, value);
        }

        private string _dOStatus_DO2;
        /// <summary>
        /// DO2
        /// </summary>
        public string DOStatus_DO2
        {
            get => _dOStatus_DO2;
            set => SetProperty(ref _dOStatus_DO2, value);
        }

        private string _dOStatus_DO3;
        /// <summary>
        /// DO3
        /// </summary>
        public string DOStatus_DO3
        {
            get => _dOStatus_DO3;
            set => SetProperty(ref _dOStatus_DO3, value);
        }

        private string _dOStatus_DO4;
        /// <summary>
        /// DO4
        /// </summary>
        public string DOStatus_DO4
        {
            get => _dOStatus_DO4;
            set => SetProperty(ref _dOStatus_DO4, value);
        }

        private string _dOStatus_DO5;
        /// <summary>
        /// DO5
        /// </summary>
        public string DOStatus_DO5
        {
            get => _dOStatus_DO5;
            set => SetProperty(ref _dOStatus_DO5, value);
        }

        private string _dOStatus_DO6;
        /// <summary>
        /// DO6
        /// </summary>
        public string DOStatus_DO6
        {
            get => _dOStatus_DO6;
            set => SetProperty(ref _dOStatus_DO6, value);
        }

        private string _dOStatus_DO7;
        /// <summary>
        /// DO7
        /// </summary>
        public string DOStatus_DO7
        {
            get => _dOStatus_DO7;
            set => SetProperty(ref _dOStatus_DO7, value);
        }

        private string _dOStatus_DO8;
        /// <summary>
        /// DO8
        /// </summary>
        public string DOStatus_DO8
        {
            get => _dOStatus_DO8;
            set => SetProperty(ref _dOStatus_DO8, value);
        }

        private string _ChargeStatus;
        /// <summary>
        /// 系统状态—充电
        /// </summary>
        public string ChargeStatus
        {
            get => _ChargeStatus;
            set => SetProperty(ref _ChargeStatus, value);
        }

        private string _DischargeStatus;
        /// <summary>
        /// 系统状态—放电
        /// </summary>
        public string DischargeStatus
        {
            get => _DischargeStatus;
            set => SetProperty(ref _DischargeStatus, value);
        }

        private string _SeriousFaultStatus;
        /// <summary>
        /// 系统状态—严重故障状态
        /// </summary>
        public string SeriousFaultStatus
        {
            get => _SeriousFaultStatus;
            set => SetProperty(ref _SeriousFaultStatus, value);
        }

        private string _ClusterMode;
        /// <summary>
        /// 系统状态—并簇模式
        /// </summary>
        public string ClusterMode
        {
            get => _ClusterMode;
            set => SetProperty(ref _ClusterMode, value);
        }

        private string _SingleClusterMode;
        /// <summary>
        /// 系统状态—单簇模式
        /// </summary>
        public string SingleClusterMode
        {
            get => _SingleClusterMode;
            set => SetProperty(ref _SingleClusterMode, value);
        }

        private string _TypeOfCurrentSensor;
        /// <summary>
        /// 电流传感器类型
        /// </summary>
        public string TypeOfCurrentSensor
        {
            get => _TypeOfCurrentSensor;
            set => SetProperty(ref _TypeOfCurrentSensor, value);
        }

        private string _CurrentSensorRange_Hall_1;
        /// <summary>
        /// 霍尔1电流传感器量程
        /// </summary>
        public string CurrentSensorRange_Hall_1
        {
            get => _CurrentSensorRange_Hall_1;
            set => SetProperty(ref _CurrentSensorRange_Hall_1, value);
        }

        private string _CurrentSensorRange_Hall_2;
        /// <summary>
        /// 霍尔2电流传感器量程
        /// </summary>
        public string CurrentSensorRange_Hall_2
        {
            get => _CurrentSensorRange_Hall_2;
            set => SetProperty(ref _CurrentSensorRange_Hall_2, value);
        }

        private string _CurrentSensorRange_Hall_3;
        /// <summary>
        /// 霍尔3电流传感器量程
        /// </summary>
        public string CurrentSensorRange_Hall_3
        {
            get => _CurrentSensorRange_Hall_3;
            set => SetProperty(ref _CurrentSensorRange_Hall_3, value);
        }

        private string _TotalNumberofSystemModules;
        /// <summary>
        /// 系统模块总个数—自动分配地址使用
        /// </summary>
        public string TotalNumberofSystemModules
        {
            get => _TotalNumberofSystemModules;
            set => SetProperty(ref _TotalNumberofSystemModules, value);
        }

        private CancellationTokenSource cancellationTokenSource;
        public CancellationTokenSource cts = null;

        int initCount = 0;
        private Timer timer = null;
        private Timer DataSavingTimer = null;
        private Timer batteryVoltageTimer = null;
        private Timer batteryTemperatureTimer = null;
        private Timer batterySocTimer = null;
        private Timer supplyVoltageTimer = null;
        private Timer poleTemperatureTimer = null;
        private Timer moduleTotalVoltageTimer = null;
        private Timer batterySohTimer = null;
        BaseCanHelper baseCanHelper = null;
        public BCU_Control_ViewModel()
        {

            switch (BMSConfig.ConfigManager.CAN_DevType)
            {
                case "USBCAN-I/I+":
                case "USBCAN-II/II+":
                    baseCanHelper = EcanHelper.Instance();
                    break;
                case "USBCAN-E-U":
                case "USBCAN-2E-U":
                    baseCanHelper = ControlcanHelper.Instance();
                    break;

            }



            cts = new CancellationTokenSource();

            ChangeEquilibriumList(64);

            ChangeBatteryList(512);

            //初始选择更新电池电压数据
            IsChecked_BatteryVoltage = true;


           
        }

        private void ChangeBatteryList(int totalCell)
        {

            if (batteryVoltageDataList != null && batteryVoltageDataList.Count == totalCell)
            {
                return;
            }

            batteryVoltageDataList = new ObservableCollection<RealtimeData_BMS1500V_BCU.batteryVoltageData>();
            batteryTemperatureDataList = new ObservableCollection<RealtimeData_BMS1500V_BCU.batteryTemperatureData>();
            batterySocDataList = new ObservableCollection<RealtimeData_BMS1500V_BCU.batterySocData>();
            batterySohDataList = new ObservableCollection<RealtimeData_BMS1500V_BCU.batterySohData>();
            supplyVoltageDataList = new ObservableCollection<RealtimeData_BMS1500V_BCU.supplyVoltageData>();
            poleTemperatureDataList = new ObservableCollection<RealtimeData_BMS1500V_BCU.poleTemperatureData>();
            moduleTotalVoltageDataList = new ObservableCollection<RealtimeData_BMS1500V_BCU.moduleTotalVoltageData>();
            AlarmMessageDataList = new ObservableCollection<RealtimeData_BMS1500V_BCU.AlarmMessageData>();


            //添加电池电压编号
            for (int i = 1; i <= totalCell; i++)
            {
                batteryVoltageDataList.Add(new RealtimeData_BMS1500V_BCU.batteryVoltageData
                {
                    SectionNumber = i.ToString() + "#"
                });
            }

            // 添加SOC编号
            for (int i = 1; i <= totalCell; i++)
            {
                batterySocDataList.Add(new RealtimeData_BMS1500V_BCU.batterySocData
                {
                    SectionNumber = i.ToString() + "#"
                });
            }

            //添加SOH编号
            for (int i = 1; i <= totalCell; i++)
            {
                batterySohDataList.Add(new RealtimeData_BMS1500V_BCU.batterySohData
                {
                    SectionNumber = i.ToString() + "#"
                });
            }

            //添加温度编号
            for (int i = 1; i <= Convert.ToInt32(TemperatureNumber); i++)
            {
                batteryTemperatureDataList.Add(new RealtimeData_BMS1500V_BCU.batteryTemperatureData
                {
                    CellNumber = i.ToString() + "#"
                });
            }

            //添加极柱温度编号
            for (int i = 1; i <= Convert.ToInt32(PoleNumber); i++)
            {
                poleTemperatureDataList.Add(new RealtimeData_BMS1500V_BCU.poleTemperatureData
                {
                    CellNumber = i.ToString() + "#"
                });
            }

            //添加供电电压编号
            for (int i = 1; i < 2; i++)
            {
                supplyVoltageDataList.Add(new RealtimeData_BMS1500V_BCU.supplyVoltageData
                {
                    CellNumber = i.ToString() + "#"
                });
            }
        }

        private void ChangeEquilibriumList(int batterySeries)
        {
            if (PassiveEquilibriumCheckBoxItems != null && PassiveEquilibriumCheckBoxItems.Count == batterySeries)
            {
                return;
            }

            PassiveEquilibriumCheckBoxItems = new ObservableCollection<CheckBoxItem>();
            ActiveEquilibriumCheckBoxItems = new ObservableCollection<ActiveEquilibriumCheckBoxGroup>();

            PassiveEquilibriumControlItems = new ObservableCollection<PassiveEquilibriumCheckBoxGroup>();
            ActiveEquilibriumControlItems = new ObservableCollection<ActiveEquilibriumCheckBoxGroup>();

            // 动态生成 64 个 CheckBoxItem
            for (int i = 0; i < batterySeries; i++)
            {
                PassiveEquilibriumCheckBoxItems.Add(new CheckBoxItem
                {
                    Label = $"电芯 {i + 1}",
                    IsChecked = false // 默认未选中
                });

            }


            PassiveEquilibriumCheckBoxGroup currentSingleGroup = null;
            for (int i = 0; i < batterySeries; i++)
            {
                // 每 16 个 CheckBoxItem 为一组
                if (i % 16 == 0)
                {
                    currentSingleGroup = new PassiveEquilibriumCheckBoxGroup();
                    currentSingleGroup.ALL = new CheckBoxItem()
                    {
                        Label = $"全选",
                        IsChecked = false // 默认未选中
                    };
                    currentSingleGroup.Label = "均衡通道_" + (i / 16 + 1).ToString();
                    currentSingleGroup.Items = new ObservableCollection<CheckBoxItem>();
                    currentSingleGroup.SelectCommand = new RelayCommand<bool>(PassiveEquilibriumCheckAll);
                    PassiveEquilibriumControlItems.Add(currentSingleGroup);
                }

                CheckBoxItem A = new CheckBoxItem
                {
                    Label = $" {i + 1}#电芯",
                    IsChecked = false // 默认未选中
                };
                currentSingleGroup.Items.Add(A);
            }


            // 主动均衡暂时不根据串数调整
            ActiveEquilibriumCheckBoxGroup currentGroup1 = null;
            for (int i = 0; i < 24; i++)
            {
                // 每 12 个 CheckBoxItem 为一组
                if (i % 6 == 0)
                {
                    currentGroup1 = new ActiveEquilibriumCheckBoxGroup();
                    currentGroup1.ALL = new CheckBoxItem()
                    {
                        Label = $"均衡关闭",
                        IsChecked = false // 默认未选中
                    };
                    currentGroup1.Label = "均衡通道_" + (i / 6 + 1).ToString();
                    currentGroup1.Items = new ObservableCollection<BoxGruop>();
                    ActiveEquilibriumCheckBoxItems.Add(currentGroup1);
                }

                CheckBoxItem A = new CheckBoxItem
                {
                    Label = $" {i + 1}#充电均衡",
                    IsChecked = false // 默认未选中
                };
                CheckBoxItem B = new CheckBoxItem
                {
                    Label = $" {i + 1}#放电均衡",
                    IsChecked = false // 默认未选中
                };

                currentGroup1.Items.Add(new BoxGruop()
                {
                    A = A,
                    B = B
                });

            }

            ActiveEquilibriumCheckBoxGroup currentGroup = null;
            for (int i = 0; i < 24; i++)
            {
                // 每 12 个 CheckBoxItem 为一组
                if (i % 6 == 0)
                {
                    currentGroup = new ActiveEquilibriumCheckBoxGroup();
                    currentGroup.ALL = new CheckBoxItem()
                    {
                        Label = $"均衡关闭",
                        IsChecked = false // 默认未选中
                    };
                    currentGroup.Label = "均衡通道_" + (i / 6 + 1).ToString();
                    currentGroup.Items = new ObservableCollection<BoxGruop>();
                    ActiveEquilibriumControlItems.Add(currentGroup);
                }

                CheckBoxItem A = new CheckBoxItem
                {
                    Label = $" {i + 1}#充电均衡",
                    IsChecked = false // 默认未选中
                };
                CheckBoxItem B = new CheckBoxItem
                {
                    Label = $" {i + 1}#放电均衡",
                    IsChecked = false // 默认未选中
                };

                currentGroup.Items.Add(new BoxGruop()
                {
                    A = A,
                    B = B
                });

            }

        }

        /// <summary>
        /// 更新数据表网格数据(泛型方法)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="headers"></param>
        /// <param name="getPrimaryValue"></param>
        /// <param name="getSecondaryValue"></param>
        /// <param name="maxSectionsPerGroup"></param>
        /// <returns></returns>
        private List<T> UpdateDataGrid<T>(List<T> dataList, string[] headers, Func<T, string> getPrimaryValue, Func<T, string> getSecondaryValue, int maxSectionsPerGroup = 48) where T : IBatteryData, new()
        {
            var tempDataList = new List<T>(dataList);
            dataList.Clear();

            if (tempDataList.Count == 0)
            {
                return dataList;
            }

            //16列
            int totalColumns = 16;
            //64行
            int totalRows = 64;
            string[,] values = new string[totalColumns, totalRows];
            string[] dataName = new string[totalRows];

            for (int i = 0; i < totalRows; i++)
            {
                int bmmIndex = (i / 8) + 1;
                dataName[i] = $"BMU{bmmIndex}";
            }

            for (int currentIndex = 0; currentIndex < tempDataList.Count; currentIndex++)
            {
                var data = tempDataList[currentIndex];
                int groupIndex = currentIndex / maxSectionsPerGroup;
                int startingRow = groupIndex * 8;
                int rowInGroup = (currentIndex % maxSectionsPerGroup) / totalColumns;
                int col = (currentIndex % maxSectionsPerGroup) % totalColumns;

                try
                {
                    var setRowIndex = startingRow + rowInGroup * 2;
                    if (setRowIndex < totalRows)
                    {
                        values[col, setRowIndex] = getPrimaryValue(data);
                        values[col, setRowIndex + 1] = getSecondaryValue(data);
                    }
                }
                catch (Exception ex)
                {
                    //调试用,正常if (setRowIndex < totalRows)后，不会出现
                    //throw;
                }
            }

            for (int i = 0; i < totalRows; i++)
            {
                dataList.Add(new T
                {
                    DataName = dataName[i],
                    IndexName = (i % 2 == 0) ? headers[0] : headers[1],
                    Value1 = values[0, i],
                    Value2 = values[1, i],
                    Value3 = values[2, i],
                    Value4 = values[3, i],
                    Value5 = values[4, i],
                    Value6 = values[5, i],
                    Value7 = values[6, i],
                    Value8 = values[7, i],
                    Value9 = values[8, i],
                    Value10 = values[9, i],
                    Value11 = values[10, i],
                    Value12 = values[11, i],
                    Value13 = values[12, i],
                    Value14 = values[13, i],
                    Value15 = values[14, i],
                    Value16 = values[15, i]
                });
            }

            return dataList;
        }

        /// <summary>
        /// 启动数据存储
        /// </summary>
        /// <param name="obj"></param>
        private void TimerCallBack_DataSaving(object obj)
        {

            model.BCU_ID = SelectedAddress_BCU;
            model.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (model != null)
            {
                var folderPath = $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}//StoreData_BMS1500V//BCU";
                var filePath = $"{folderPath}//BCU实时数据_地址{SelectedAddress_BCU}_{DateTime.Now.ToString("yyyy-MM-dd")}.csv";

                // 检查文件夹是否存在，不存在则创建
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                // 创建临时电池电压数据列表的副本，同时保留原SectionNumber取值
                var tempbatteryVoltageDataList = batteryVoltageDataList.Select(b => new batteryVoltageData
                {
                    Voltage = b.Voltage,
                    SectionNumber = b.SectionNumber
                }).ToList();

                // 修改列标题
                for (int i = 0; i < tempbatteryVoltageDataList.Count; i++)
                {
                    // 计算当前组号
                    int groupIndex = i / 48; // 每48个为一组

                    // 确保组号在有效范围内（即小于8，表示最多384个电芯电压）
                    if (groupIndex < 8)
                    {
                        // 根据组号重新设置电池电压列标题
                        tempbatteryVoltageDataList[i].SectionNumber = $"BMU{(groupIndex + 1)}电池电压(V) " + tempbatteryVoltageDataList[i].SectionNumber;
                    }
                }

                // 创建临时电池温度数据列表的副本，同时保留原CellNumber取值
                var tempbatteryTemperatureDataList = batteryTemperatureDataList.Select(b => new batteryTemperatureData
                {
                    Temperature = b.Temperature,
                    CellNumber = b.CellNumber
                }).ToList();

                // 遍历 tempbatteryTemperatureDataList，修改CellNumber
                for (int i = 0; i < tempbatteryTemperatureDataList.Count; i++)
                {
                    // 计算当前组号
                    int groupIndex = i / 28; // 每28个为一组

                    // 确保组号在有效范围内（即小于8，最多224个电芯温度）
                    if (groupIndex < 8)
                    {
                        // 根据组号重新设置电池温度列标题
                        tempbatteryTemperatureDataList[i].CellNumber = $"BMU{(groupIndex + 1)}电池温度(℃) " + tempbatteryTemperatureDataList[i].CellNumber;
                    }
                }

                #region 测试版本增加SOH存储
                //// 创建临时电池SOH数据列表的副本，同时保留原SectionNumber取值
                //var tempbatterySohDataList = batterySohDataList.Select(b => new batterySohData
                //{
                //    SOH = b.SOH,
                //    SectionNumber = b.SectionNumber
                //}).ToList();

                //// 修改列标题
                //for (int i = 0; i < tempbatterySohDataList.Count; i++)
                //{
                //    // 计算当前组号
                //    int groupIndex = i / 48; // 每48个为一组

                //    // 确保组号在有效范围内（即小于8，表示最多384个电芯SOH）
                //    if (groupIndex < 8)
                //    {
                //        // 根据组号重新设置电池电压列标题
                //        tempbatterySohDataList[i].SectionNumber = $"BMU{(groupIndex + 1)}电池SOH(%) " + tempbatterySohDataList[i].SectionNumber;
                //    }
                //}
                #endregion

                #region 增加SOC存储
                // 创建临时电池SOC数据列表的副本，同时保留原SectionNumber取值
                var tempbatterySocDataList = batterySocDataList.Select(b => new batterySocData
                {
                    SOC = b.SOC,
                    SectionNumber = b.SectionNumber
                }).ToList();

                // 修改列标题
                for (int i = 0; i < tempbatterySocDataList.Count; i++)
                {
                    // 计算当前组号
                    int groupIndex = i / 48; // 每48个为一组

                    // 确保组号在有效范围内（即小于8，表示最多384个电芯SOH）
                    if (groupIndex < 8)
                    {
                        // 根据组号重新设置电池电压列标题
                        tempbatterySocDataList[i].SectionNumber = $"BMU{(groupIndex + 1)}电池SOC(%) " + tempbatterySocDataList[i].SectionNumber;
                    }
                }
                #endregion

                // 如果文件不存在，则写入表头
                if (!File.Exists(filePath))
                {
                    // 写入表头，拼接各部分
                    string header = model.GetHeader() + "," +// 分隔符                                 
                                    string.Join(",", tempbatteryVoltageDataList.Select(b => b.SectionNumber)) + "," + string.Join(",", tempbatteryTemperatureDataList.Select(b => b.CellNumber)) + "," + string.Join(",", tempbatterySocDataList.Select(b => b.SectionNumber)) + "\r\n"; //测试版本增加SOH存储
                    File.AppendAllText(filePath, header, Encoding.UTF8);
                }

                // 写入数据
                var values = model.GetValue() + "," +
                             string.Join(",", tempbatteryVoltageDataList.Select(b => b.Voltage)) + "," + string.Join(",", tempbatteryTemperatureDataList.Select(b => b.Temperature)) + "," + string.Join(",", tempbatterySocDataList.Select(b => b.SOC)) + "\r\n";//测试版本增加SOH存储
                File.AppendAllText(filePath, values, Encoding.UTF8);


            }
        }


        /// <summary>
        /// 更新电池电压数据
        /// </summary>
        /// <param name="obj"></param>
        private void TimerCallBack_batteryVoltage(object obj)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
                    {
                        BatteryDataList = new ObservableCollection<IBatteryData>(
                            UpdateDataGrid(batteryVoltageDataList.ToList(),
                            new[] { "节号", "电压" },
                            data => data.SectionNumber,
                            data => data.Voltage,
                            ConstantDef.BatteryCellNumber));
                    });
        }

        /// <summary>
        /// 更新电池温度数据
        /// </summary>
        /// <param name="obj"></param>
        private void TimerCallBack_batteryTemperature(object obj)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                BatteryDataList = new ObservableCollection<IBatteryData>(
                    UpdateDataGrid(batteryTemperatureDataList.ToList(),
                    new[] { "序号", "温度" },
                    data => data.CellNumber,
                    data => data.Temperature,
                   ConstantDef.BatteryTemperatureNumber));
            });
        }

        /// <summary>
        /// 更新电池SOC数据
        /// </summary>
        /// <param name="obj"></param>
        private void TimerCallBack_batterySoc(object obj)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                BatteryDataList = new ObservableCollection<IBatteryData>(
                    UpdateDataGrid(batterySocDataList.ToList(),
                    new[] { "节号", "SOC" },
                    data => data.SectionNumber,
                    data => data.SOC,
                    ConstantDef.BatteryCellNumber));
            });
        }

        /// <summary>
        /// 更新电池SOH数据
        /// </summary>
        /// <param name="obj"></param>
        private void TimerCallBack_batterySoh(object obj)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                BatteryDataList = new ObservableCollection<IBatteryData>(
                    UpdateDataGrid(batterySohDataList.ToList(),
                    new[] { "节号", "SOH" },
                    data => data.SectionNumber,
                    data => data.SOH,
                    ConstantDef.BatteryCellNumber));
            });
        }

        /// <summary>
        /// 更新供电电压数据
        /// </summary>
        /// <param name="obj"></param>
        private void TimerCallBack_supplyVoltage(object obj)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                BatteryDataList = new ObservableCollection<IBatteryData>(
                    UpdateDataGrid(supplyVoltageDataList.ToList(),
                    new[] { "序号", "电压" },
                    data => data.CellNumber,
                    data => data.SupplyVoltage,
                    1));
            });
        }

        /// <summary>
        /// 更新极柱温度数据
        /// </summary>
        /// <param name="obj"></param>
        private void TimerCallBack_poleTemperature(object obj)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                BatteryDataList = new ObservableCollection<IBatteryData>(
                    UpdateDataGrid(poleTemperatureDataList.ToList(),
                    new[] { "序号", "温度" },
                    data => data.CellNumber,
                    data => data.PoleTemperature,
                    2));
            });
        }

        /// <summary>
        /// 更新模块总电压数据
        /// </summary>
        /// <param name="obj"></param>
        private void TimerCallBack_moduleTotalVoltage(object obj)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                BatteryDataList = new ObservableCollection<IBatteryData>(
                    UpdateDataGrid(moduleTotalVoltageDataList.ToList(),
                    new[] { "序号", "电压" },
                    data => data.CellNumber,
                    data => data.ModuleTotalVoltage,
                    1));
            });
        }


        public ICommand StartDataCollectionCmd => new RelayCommand(StartDataCollection);
        /// <summary>
        /// 进入界面或者手动点击启动数据采集按钮
        /// </summary>
        public void StartDataCollection()
        {
            //禁用启动数据采集按钮
            IsStartDataCollectionEnabled = false;
            //启用停止数据采集按钮
            IsStopDataCollectionEnabled = true;
            //启动数据采集定时器
            timer = new System.Threading.Timer(TimerCallBack, 0, 500, Convert.ToInt32(DataCollectionInterval));
            //默认选择展示电池电压数据
            batteryVoltageTimer = new System.Threading.Timer(TimerCallBack_batteryVoltage, 0, 1000, Convert.ToInt32(DataCollectionInterval));

            IsShowMessage = false;
            ReadAll();

        }

        public ICommand StopDataCollectionCmd => new RelayCommand(StopDataCollection);
        /// <summary>
        /// 离开界面或者手动点击停止数据采集按钮
        /// </summary>
        public void StopDataCollection()
        {
            //启用启动数据采集按钮
            IsStartDataCollectionEnabled = true;
            //禁用停止数据采集按钮
            IsStopDataCollectionEnabled = false;

            //关闭数据采集定时器
            if (timer != null) timer.Dispose();
            //关闭数据存储定时器
            if (DataSavingTimer != null) DataSavingTimer.Dispose();
            //关闭系统概要相关定时器
            if (batteryVoltageTimer != null) batteryVoltageTimer.Dispose();
            if (batterySocTimer != null) batterySocTimer.Dispose();
            if (batterySohTimer != null) batterySohTimer.Dispose();
            if (batteryTemperatureTimer != null) batteryTemperatureTimer.Dispose();
            if (supplyVoltageTimer != null) supplyVoltageTimer.Dispose();
            if (poleTemperatureTimer != null) poleTemperatureTimer.Dispose();
            if (moduleTotalVoltageTimer != null) moduleTotalVoltageTimer.Dispose();
        }

        public void UpdateTime()
        {
            DateTime now = DateTime.Now;
            SelectedDate = now.Date;
            SelectedHour = now.Hour.ToString("D2");
            SelectedMinute = now.Minute.ToString("D2");
            SelectedSecond = now.Second.ToString("D2");
        }

        public ICommand StartBalance_Cmd => new RelayCommand(StartBalance);
        /// <summary>
        /// 开始发送强制均衡命令-主动均衡（命令码 0x52）、被动均衡（命令码 0x42）
        /// </summary>
        public void StartBalance()
        {
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            byte addressBCU = Convert.ToByte(Convert.ToInt32(SelectedAddress_BCU, 16));

            //选择均衡类型
            switch (SelectedEquilibriumType_Write)
            {
                case "主动均衡":
                    StartEquilibrium(token, addressBCU, 0x52, true);
                    break;
                case "被动均衡":
                    StartEquilibrium(token, addressBCU, 0x42, false);
                    break;
            }
        }

        /// <summary>
        /// 开始均衡操作
        /// </summary>
        private void StartEquilibrium(CancellationToken token, byte addressBCU, byte commandCode, bool isActive)
        {
            if (ConstantDef.BatteryCellNumber <= 48 || isActive)
            {
                byte[] frameId = { 0xF4, addressBCU, commandCode, 0x18 };
                byte[] frame = PrepareEquilibriumData(isActive);
                var data = new List<byte[]>() { frame };
                //启动均衡倒计时
                StartCountdown(token, data, frameId);
            }
            else
            {
                byte[] frameId = { 0xF4, addressBCU, commandCode, 0x18 };
                var address = Convert.ToInt32(SelectedModulAddress);
                byte[] frame1 = PrepareEquilibriumData8(isActive, address * 2 - 1);
                byte[] frame2 = PrepareEquilibriumData8(isActive, address * 2);
                var data = new List<byte[]>() { frame1, frame2 };

                //启动均衡倒计时
                StartCountdown(token, data, frameId);
            }
        }

        /// <summary>
        /// 准备均衡data数据
        /// </summary>
        private byte[] PrepareEquilibriumData(bool isActive)
        {
            byte[] data = new byte[8];
            if (isActive)
            {
                data[0] = Convert.ToByte(Convert.ToInt32(SelectedModulAddress));
                data[1] = 0xFF; // 默认发0xFF
                SetActiveEquilibriumData(1, data);
                SetActiveEquilibriumData(2, data);
                SetActiveEquilibriumData(3, data);
                SetActiveEquilibriumData(4, data);
            }
            else
            {

                data[0] = 0xFF; // 默认发0xFF
                data[1] = Convert.ToByte(Convert.ToInt32(SelectedModulAddress));
                SetPassiveEquilibriumData(2, 1, 9, data);
                SetPassiveEquilibriumData(3, 1, 1, data);
                SetPassiveEquilibriumData(4, 2, 25, data);
                SetPassiveEquilibriumData(5, 2, 17, data);
                SetPassiveEquilibriumData(6, 3, 41, data);
                SetPassiveEquilibriumData(7, 3, 33, data);

            }


            return data;
        }


        /// <summary>
        /// 启动倒计时线程并发送数据
        /// </summary>
        private void StartCountdown(CancellationToken token, List<byte[]> data, byte[] frameId)
        {
            //new Thread(() =>
            //{
            //    try
            //    {
            //        int countdownTime = Convert.ToInt32(EquilibriumTime);
            //        while (countdownTime > 0)
            //        {
            //            baseCanHelper.Send(data, frameId);
            //            token.ThrowIfCancellationRequested();
            //            Thread.Sleep(1000);
            //            countdownTime--;
            //            //更新界面剩余均衡时间
            //            UpdateRemainingTime(countdownTime);
            //        }
            //    }
            //    catch (OperationCanceledException)
            //    {
            //        ClearAndSendData(data, frameId);
            //    }

            //    if (!token.IsCancellationRequested) SendCompletionMessage(data, frameId);

            //}).Start();
            Task.Run(async () =>
            {
                try
                {
                    int countdownTime = Convert.ToInt32(EquilibriumTime);
                    while (countdownTime > 0)
                    {
                        for (int i = 0; i < data.Count; i++)
                        {
                            baseCanHelper.Send(data[i], frameId);
                            await Task.Delay(10, token);
                        }

                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1000, token);
                        countdownTime--;
                        UpdateRemainingTime(countdownTime); // 更新界面剩余时间
                    }
                }
                catch (OperationCanceledException)
                {
                    MessageBoxHelper.Warning($"{SelectedEquilibriumType_Write}均衡命令下发失败！", "提示", null, ButtonType.OK);
                }

                if (!token.IsCancellationRequested)
                {

                    for (int i = 0; i < data.Count; i++)
                    {
                        ClearAndSendData(data[i], frameId);// 倒计时结束发送完成消息命令data2-7清零
                        await Task.Delay(10, token);
                    }
                }
            }, token);

        }


        /// <summary>
        /// 准备均衡data数据
        /// </summary>
        private byte[] PrepareEquilibriumData8(bool isActive, int frameIndex)
        {
            byte[] data = new byte[8];
            if (isActive)
            {
                data[0] = Convert.ToByte(Convert.ToInt32(SelectedModulAddress));
                data[1] = 0xFF; // 默认发0xFF
                SetActiveEquilibriumData(1, data);
                SetActiveEquilibriumData(2, data);
                SetActiveEquilibriumData(3, data);
                SetActiveEquilibriumData(4, data);
            }
            else
            {
                if (frameIndex % 2 == 1)
                {
                    data[0] = 0xFF; // 默认发0xFF
                    data[1] = Convert.ToByte(frameIndex);
                    SetPassiveEquilibriumData(2, 1, 9, data);
                    SetPassiveEquilibriumData(3, 1, 1, data);
                    SetPassiveEquilibriumData(4, 2, 25, data);
                    SetPassiveEquilibriumData(5, 2, 17, data);
                    SetPassiveEquilibriumData(6, 3, 41, data);
                    SetPassiveEquilibriumData(7, 3, 33, data);
                }
                else
                {
                    data[0] = 0xFF; // 默认发0xFF
                    data[1] = Convert.ToByte(frameIndex);
                    SetPassiveEquilibriumData(2, 4, 57, data);
                    SetPassiveEquilibriumData(3, 4, 49, data);

                }





            }


            return data;
        }


        /// <summary>
        /// 更新剩余均衡时间
        /// </summary>
        private void UpdateRemainingTime(int time)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                RemainingEquilibriumTime = time.ToString();
            });
        }

        /// <summary>
        /// 清除数据并发送
        /// </summary>
        private bool ClearAndSendData(byte[] data, byte[] frameId)
        {
            var isOk = true;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Array.Fill(data, (byte)0, 2, 6); // 清除数据：byte3-8置0
                isOk = baseCanHelper.Send(data, frameId);
                RemainingEquilibriumTime = "0";// 更新界面剩余均衡时间为0
            });
            return isOk;
        }



        /// <summary>
        /// 设置主动均衡数据
        /// </summary>
        private void SetActiveEquilibriumData(int groupIndex, byte[] data)
        {

            if (ActiveEquilibriumControlItems[groupIndex - 1].ALL.IsChecked == true)
            {
                data[groupIndex + 1] = 0x00;
            }
            else
            {
                for (int i = 1; i <= 6; i++)
                {

                    bool? isCheckedChg = ActiveEquilibriumControlItems[groupIndex - 1].Items[i - 1].A.IsChecked;
                    bool? isCheckedDsg = ActiveEquilibriumControlItems[groupIndex - 1].Items[i - 1].B.IsChecked;
                    if (isCheckedChg == true) data[groupIndex + 1] |= (byte)(1 << (i - 1));// 将第 1-6 位设置为 1
                    else
                    if (isCheckedDsg == true) data[groupIndex + 1] |= (byte)((1 << (i - 1)) | (1 << 7));// 将第 1-6位设置为 1,并 将第 8 位设为 1
                }
            }
        }

        /// <summary>
        /// 设置被动均衡数据
        /// </summary>
        private void SetPassiveEquilibriumData(int groupIndex, int channelIndex, int startIndex, byte[] data)
        {
            for (int k = 0; k < 8; k++)
            {

                bool isChecked = PassiveEquilibriumControlItems[channelIndex - 1].Items[k].IsChecked;
                if (isChecked) data[groupIndex] |= (byte)(1 << k);//勾选则设置bit位为1
            }
        }

        private void PassiveEquilibriumCheckAll(bool ischecked)
        {
            foreach (var item in PassiveEquilibriumControlItems)
            {
                foreach (var check in item.Items)
                {
                    check.IsChecked = item.ALL.IsChecked;
                }
            }
        }
        /// <summary>
        /// 获取属性值
        /// </summary>
        private bool? GetPropertyValue(string propertyName) => GetType().GetProperty(propertyName)?.GetValue(this) as bool?;

        public ICommand StopBalance_Cmd => new RelayCommand(StopBalance);

        /// <summary>
        /// 停止发送强制均衡命令
        /// </summary>
        public void StopBalance()
        {
            cancellationTokenSource?.Cancel();
        }


        public ICommand PowerOnAndOffControlCmd => new RelayCommand(PowerOnAndOffControl);
        /// <summary>
        /// 控制主控上下电指令（ID: 0x0880FFF4）
        /// </summary>
        public void PowerOnAndOffControl()
        {
            byte Address_BCU = Convert.ToByte(Convert.ToInt32(SelectedAddress_BCU, 16));
            byte[] FrameID = new byte[] { 0xF4, 0xFF, 0x80, 0x08 };
            byte[] data = new byte[8];

            data[0] = (byte)(PowerOnAndOffList.IndexOf(SelectedPowerOnAndOff) + 1);             //上电或者下电指令
            data[1] = (byte)(OperatingInstructionsList.IndexOf(SelectedOperatingInstructions)); //对所有或者相应簇执行上电或者下电指令
            data[2] = (byte)(FaultResetList.IndexOf(SelectedFaultReset) + 85);                  //0x55：故障复归有效

            if (baseCanHelper.Send(data, FrameID)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);
        }

        public ICommand InsulationDetectionCmd => new RelayCommand(InsulationDetection);
        /// <summary>
        /// 控制主控绝缘检测功能指令（ID: 0x0881XXF4(XX:0xE8~0xFB)）
        /// </summary>
        public void InsulationDetection()
        {
            byte Address_BCU = Convert.ToByte(Convert.ToInt32(SelectedAddress_BCU, 16));
            byte[] FrameID = new byte[] { 0xF4, Address_BCU, 0x81, 0x08 };
            byte[] data = new byte[8];

            data[0] = (byte)(InsulationDetectionList.IndexOf(SelectedInsulationDetection) + 1);//绝缘检测控制指令
            if (baseCanHelper.Send(data, FrameID)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);
        }

        public ICommand AutomaticAllocationAddrCmd => new RelayCommand(AutomaticAllocationAddr);
        /// <summary>
        /// 自动分配地址指令（ID: 0x1867XXF4 (XX:0xE8~0xFB)）
        /// </summary>
        public void AutomaticAllocationAddr()
        {
            byte Address_BCU = Convert.ToByte(Convert.ToInt32(SelectedAddress_BCU, 16));
            byte[] FrameID = new byte[] { 0xF4, Address_BCU, 0x67, 0x18 };
            byte[] data = new byte[8];
            data[0] = 0x02;
            data[1] = 0x80;
            data[2] = Convert.ToByte(Convert.ToInt32(TotalNumberofSystemModules));
            if (baseCanHelper.Send(data, FrameID)) MessageBoxHelper.Success("地址分配成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("地址分配失败！", "提示", null, ButtonType.OK);
        }


        private static readonly Random random = new Random();

        private readonly object lockObject = new object();
        private void ClearBatteryList()
        {
            if (ConstantDef.BatteryCellNumber <= 48)
            {

                for (int i = 48; i < batteryVoltageDataList.Count; i++)
                {
                    if (i % 64 >= 48 && i % 64 <= 64)
                        batteryVoltageDataList[i].Voltage = "";
                }
                for (int i = 48; i < batterySocDataList.Count; i++)
                {
                    if (i % 64 >= 48 && i % 64 <= 64)
                        batterySocDataList[i].SOC = "";
                }
                for (int i = 48; i < batterySohDataList.Count; i++)
                {
                    if (i % 64 >= 48 && i % 64 <= 64)
                        batterySohDataList[i].SOH = "";
                }
                for (int i = 48; i < batteryTemperatureDataList.Count; i++)
                {
                    if (i % 64 >= 48 && i % 64 <= 64)
                        batteryTemperatureDataList[i].Temperature = "";
                }
                for (int i = 48; i < supplyVoltageDataList.Count; i++)
                {
                    if (i % 64 >= 48 && i % 64 <= 64)
                        supplyVoltageDataList[i].SupplyVoltage = "";
                }
                for (int i = 48; i < poleTemperatureDataList.Count; i++)
                {
                    if (i % 64 >= 48 && i % 64 <= 64)
                        poleTemperatureDataList[i].PoleTemperature = "";
                }
                for (int i = 48; i < moduleTotalVoltageDataList.Count; i++)
                {
                    if (i % 64 >= 48 && i % 64 <= 64)
                        moduleTotalVoltageDataList[i].ModuleTotalVoltage = "";
                }
            }



        }
        private void TimerCallBack(object obj)
        {


            if (baseCanHelper.IsConnection)
            {
                //ClearBatteryList();
                // 主控(BCU)地址
                byte Address_BCU = Convert.ToByte(Convert.ToInt32(SelectedAddress_BCU, 16));
                // PC 请求电池采集消息（命令码 0x30）
                byte[] commandCodes = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
                byte[] FrameID_Request = new byte[] { 0xF4, Address_BCU, 0x30, 0x18 };
                foreach (byte code in commandCodes)
                {
                    byte[] subCommandCodes;

                    switch (code)
                    {
                        case 0x01:
                            // 电池单体电压
                            subCommandCodes = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 };//恢复从1开始
                            //subCommandCodes = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };//修改从0开始
                            break;
                        case 0x02:
                        case 0x03:
                        case 0x04:
                            // 电池单体温度/极柱温度、电池单体SOC、电池单体SOH
                            subCommandCodes = new byte[] { 0x01, 0x02, 0x03, 0x04 };//恢复从1开始
                            //subCommandCodes = new byte[] { 0x00, 0x01, 0x02, 0x03 };//修改从0开始
                            break;
                        default:
                            // 其他—主控采集信息、系统概要信息、模块电池节数、组端报警信息、DI/DO 状态信息、模块温度个数、均衡状态、从控失联状态、NC(未定义)
                            byte[] defaultBytes = new byte[8] { code, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                            baseCanHelper.Send(defaultBytes, FrameID_Request);
                            continue; // 跳出当前循环
                    }

                    foreach (byte subCode in subCommandCodes)
                    {
                        byte[] bytes = new byte[8] { code, subCode, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                        baseCanHelper.Send(bytes, FrameID_Request);
                        Thread.Sleep(200);
                    }



                    if (baseCanHelper.CommunicationType == "Ecan")
                    {
                        lock (EcanHelper._locker)
                        {
                            while (EcanHelper._task.Count > 0
                                && !cts.IsCancellationRequested)
                            {
                                CAN_OBJ ch = (CAN_OBJ)EcanHelper._task.Dequeue();

                                Application.Current.Dispatcher.Invoke(() => { AnalysisData(ch.ID, ch.Data); });
                                //Log.Info($"{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")} 接收CAN数据:{BitConverter.ToString(ch.Data).Replace("-", " ")}  帧ID:{ch.ID.ToString("X8")}");

                            }
                        }
                    }
                    else
                    {
                        lock (ControlcanHelper._locker)
                        {
                            while (ControlcanHelper._task.Count > 0
                                && !cts.IsCancellationRequested)
                            {
                                VCI_CAN_OBJ ch = (VCI_CAN_OBJ)ControlcanHelper._task.Dequeue();

                                Application.Current.Dispatcher.Invoke(() => { AnalysisData(ch.ID, ch.Data); });
                                //Log.Info($"{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")} 接收CAN数据:{BitConverter.ToString(ch.Data).Replace("-", " ")}  帧ID:{ch.ID.ToString("X8")}");

                            }
                        }
                    }
                }
            }
        }

        public static bool IsShowMessage = true;
        public ICommand ReadAllCmd => new RelayCommand(ReadAll);

        public void ReadAll()
        {
            if (baseCanHelper.IsConnection)
            {
                try
                {
                    // 主控(BCU)地址
                    byte Address_BCU = Convert.ToByte(Convert.ToInt32(SelectedAddress_BCU, 16));

                    byte[] bytes0 = new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    baseCanHelper.Send(bytes0, new byte[] { 0xF4, Address_BCU, 0x1F, 0x18 });

                    //查询传感器报文（命令码 0x03）
                    byte[] bytes1 = new byte[8] { 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    baseCanHelper.Send(bytes1, new byte[] { 0xF4, Address_BCU, 0x1F, 0x18 });

                    //查询RTC 数据（命令码 0x12）
                    byte[] bytes2 = new byte[8] { 0x12, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    baseCanHelper.Send(bytes2, new byte[] { 0xF4, Address_BCU, 0x1F, 0x18 });

                    //查询电池主参数（命令码 0x16）
                    byte[] bytes3 = new byte[8] { 0x16, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    baseCanHelper.Send(bytes3, new byte[] { 0xF4, Address_BCU, 0x1F, 0x18 });

                    //查询SOC 参数（命令码 0x1A）
                    byte[] bytes4 = new byte[8] { 0x1A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    baseCanHelper.Send(bytes4, new byte[] { 0xF4, Address_BCU, 0x1F, 0x18 });

                    //查询询版本号消息（命令码 0x40）
                    //查询主控版本号
                    byte[] bytes5 = new byte[8] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    baseCanHelper.Send(bytes5, new byte[] { 0xF4, Address_BCU, 0x40, 0x18 });

                    //查询从控模块(1-10)版本号
                    for (int i = 1; i < 11; i++)
                    {
                        byte[] bytes6 = new byte[8] { 0x02, (byte)i, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                        baseCanHelper.Send(bytes6, new byte[] { 0xF4, Address_BCU, 0x40, 0x18 });
                    }

                    //查询从控模块基本参数(Byte1: 包序号0x01, 包序号0x02 - 0xFF暂时全部预留)                  
                    byte[] bytes7 = new byte[8] { 0x14, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    baseCanHelper.Send(bytes7, new byte[] { 0xF4, Address_BCU, 0x1F, 0x18 });


                }
                catch
                {
                    MessageBoxHelper.Warning("读取失败！", "警告", null, ButtonType.OK);
                }
            }
        }



        public void CancelOperation()
        {
            if (cts != null)
            {
                cts.Cancel();
                cts.Dispose();
            }
        }

        public ICommand FullyOpen_DO_Cmd => new RelayCommand(FullyOpen_DO);
        public void FullyOpen_DO()
        {
            for (int i = 1; i <= 8; i++)
            {
                GetType().GetProperty($"IsChecked_DO{i}_Open").SetValue(this, true);
            }
        }

        public ICommand FullyClose_DO_Cmd => new RelayCommand(FullyClose_DO);
        public void FullyClose_DO()
        {
            for (int i = 1; i <= 8; i++)
            {
                GetType().GetProperty($"IsChecked_DO{i}_Close").SetValue(this, true);
            }
        }

        public ICommand DistributeControlCmd => new RelayCommand(DistributeControl);
        public void DistributeControl()
        {
            if (!baseCanHelper.IsConnection)
            {
                MessageBoxHelper.Info("请先打开CAN口!", "提示", null, ButtonType.OK);
                return;
            }

            byte[] canid = new byte[] { 0xF4, 0xE8, 0x57, 0x18 };

            byte DO_byte1 = 0x00; // 初始化 DO_byte1

            if (IsChecked_DO1_Open == true) DO_byte1 |= 0x01; // Bit1：DO1 状态标志（0：输出无效；1：输出有效）
            if (IsChecked_DO2_Open == true) DO_byte1 |= 0x02; // Bit2：DO2 状态标志（0：输出无效；1：输出有效）
            if (IsChecked_DO3_Open == true) DO_byte1 |= 0x04; // Bit3：DO3 状态标志（0：输出无效；1：输出有效）
            if (IsChecked_DO4_Open == true) DO_byte1 |= 0x08; // Bit4：DO4 状态标志（0：输出无效；1：输出有效）
            if (IsChecked_DO5_Open == true) DO_byte1 |= 0x10; // Bit5：DO5 状态标志（0：输出无效；1：输出有效）
            if (IsChecked_DO6_Open == true) DO_byte1 |= 0x20; // Bit6：DO6 状态标志（0：输出无效；1：输出有效）
            if (IsChecked_DO7_Open == true) DO_byte1 |= 0x40; // Bit7：DO7 状态标志（0：输出无效；1：输出有效）
            if (IsChecked_DO8_Open == true) DO_byte1 |= 0x80; // Bit8：DO8 状态标志（0：输出无效；1：输出有效）

            byte[] data = new byte[] { DO_byte1, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            try
            {
                bool result = baseCanHelper.Send(data, canid);
            }
            catch (Exception ex)
            {
                MessageBoxHelper.Error(ex.Message, "提示", null, ButtonType.OK);
            }
        }

        public ICommand RefreshAlarmMessageCmd => new RelayCommand(RefreshAlarmMessage);

        public void RefreshAlarmMessage()
        {
            //if (AlarmMessageDataList != null) AlarmMessageDataList.Clear();

            if (AlarmMessageDataList != null)
            {
                // 使用 LINQ 筛选出已结束的报警信息
                var endedAlarms = AlarmMessageDataList.Where(a => a.isEnd == "是").ToList();

                // 移除所有已结束的报警信息
                foreach (var alarm in endedAlarms)
                {
                    AlarmMessageDataList.Remove(alarm);
                }
            }
        }

        public void AnalysisData(uint canID, byte[] data)
        {
            byte Address_BCU = Convert.ToByte(Convert.ToInt32(SelectedAddress_BCU, 16));
            byte[] canid = BitConverter.GetBytes(canID);
            if (canid[0] != Address_BCU || !(canid[0] == Address_BCU && canid[1] == 0xF4 /*&& canid[3] == 0x18*/)) return;

            string[] batteryVoltages = new string[3840];
            string[] batterySocs = new string[3840];
            string[] batterySohs = new string[3840];
            string[] batteryTemperatures = new string[3840];
            string[] supplyVoltages = new string[1];
            string[] poleTemperatures = new string[3840];
            string[] moduleTotalVoltages = new string[3840];

            // 初始化标志变量和计数器
            bool isVersionInfoFetched = false;
            int validDataFrameCount = 0; // 记录有效数据帧数量
            int SlaveControlNum = 1;

            try
            {
                switch (canid[2])
                {

                    case 0x00:
                        if (data[0] == 4)
                        {
                            ProductName = "PowerMagic " + Encoding.ASCII.GetString(data.Skip(1).Take(1).ToArray())
                                + "." + Encoding.ASCII.GetString(data.Skip(2).Take(1).ToArray());

                            if (ProductName == "PowerMagic 1.0")
                            {
                                ConstantDef.BatteryCellNumber = 48;
                                ConstantDef.BatteryTemperatureNumber = 28;
                                ChangeEquilibriumList(48);
                                ChangeBatteryList(384);
                            }
                            else
                            {
                                ConstantDef.BatteryCellNumber = 64;
                                ConstantDef.BatteryTemperatureNumber = 36;
                                ChangeEquilibriumList(64);
                                ChangeBatteryList(512);
                            }


                            if (IsShowMessage)
                            {
                                MessageBoxHelper.Success("读取成功！", "提示", null, ButtonType.OK);
                            }
                            IsShowMessage = true;
                        }
                        break;
                    //从控模块基本参数（命令码 0x14）
                    case 0x14:
                        if (data[0] == 1)
                        {
                            SystemTotalModuleNumber = data[1].ToString();
                            SystemTotalbatteryNumber = (data[2] << 8 | data[3]).ToString();
                            SystemTotalTemperaturesNumber = (data[4] << 8 | data[5]).ToString();

                            ConstantDef.BCU_ModuleNumber = Convert.ToInt32(SystemTotalModuleNumber);

                        }
                        break;
                    //传感器
                    case 0x03:
                        // Byte 1: 包序号（1~0xFE，0xFF表示最后一包）
                        int PackNumber = data[0];
                        TypeOfCurrentSensor = data[1].ToString();

                        // 获取霍尔1当前状态
                        if (GetBit(data[1], 0) == 1) IsChecked_Hall1_effective = true; else IsChecked_Hall1_invalid = true;

                        // 获取霍尔2当前状态
                        if (GetBit(data[1], 1) == 1) IsChecked_Hall2_effective = true; else IsChecked_Hall2_invalid = true;

                        // 获取霍尔3当前状态
                        if (GetBit(data[1], 2) == 1) IsChecked_Hall3_effective = true; else IsChecked_Hall3_invalid = true;
                        //获取霍尔传感器当前状态
                        if (GetBit(data[1], 7) == 1) IsChecked_DoubleHall = true; else IsChecked_NonDoubleHall = true;

                        CurrentSensorRange_Hall_1 = (data[2] << 8 | data[3]).ToString();
                        CurrentSensorRange_Hall_2 = (data[4] << 8 | data[5]).ToString();
                        CurrentSensorRange_Hall_3 = (data[6] << 8 | data[7]).ToString();

                        break;
                    //RTC 数据（命令码 0x12）
                    case 0x12:
                        SelectedDate = new DateTime(data[0] << 8 | data[1], data[2], data[3]);
                        SelectedHour = data[4].ToString("D2");
                        SelectedMinute = data[5].ToString("D2");
                        SelectedSecond = data[6].ToString("D2");
                        break;

                    //电池主参数（命令码 0x16）
                    case 0x16:
                        SelectedBatteryType = BatteryTypeList[Convert.ToInt32(data[0]) - 1];
                        BatteryCapacity = (data[1] << 8 | data[2]).ToString();
                        BatteryModel = data[3].ToString("");
                        BatteryManufacturer = data[4].ToString(""); ;
                        break;
                    //SOC 参数（命令码 0x1A）
                    case 0x1A:
                        switch (data[0])
                        {
                            case 0x00:
                                SelectedBatteryNumber_SOC = Convert.ToInt32(data[1] << 8 | data[2]);
                                SOC = (data[3] << 8 | data[4]).ToString();
                                break;
                            case 0x01:
                                SelectedBatteryNumber_SOH = Convert.ToInt32(data[1] << 8 | data[2]);
                                SOH = (data[3] << 8 | data[4]).ToString();
                                break;
                        }
                        break;

                    //EVBCM 应答 PC 请求模块均衡状态信息消息（命令码 0x3B）
                    case 0x3B:

                        // 提取data[0]的bit1-2
                        int bit1_2 = (data[0] >> 0) & 0x03;
                        // 均衡类型
                        SelectedEquilibriumType = EquilibriumTypeList[bit1_2];
                        // 提取data[0]的bit3-8
                        int bit3_8 = (data[0] >> 2) & 0x3F;
                        // 模块号
                        //SelectedModuleNumber = ModuleNumberList[bit3_8 - 1];修改为手动设置

                        if (SelectedEquilibriumType == " 1： 主动均衡")
                        {
                            // 处理主动均衡模块状态
                            for (int i = 1; i <= 4; i++)
                            {
                                HandleEquilibriumStatus(i);
                            }

                            // 处理均衡状态
                            void HandleEquilibriumStatus(int index)
                            {
                                // 提取均衡状态
                                int bits_6_7 = (data[index] >> 6) & 0b11;
                                // 通道号 电芯编号
                                int bits_0_5 = data[index] & 0b111111;

                                switch (bits_6_7)
                                {
                                    // Bit7..Bit6 = 00 表示均衡关闭
                                    case 0:
                                        ActiveEquilibriumCheckBoxItems[index - 1].ALL.IsChecked = true;
                                        break;
                                    // Bit7..Bit6 = 01 表示充电均衡
                                    case 1:
                                        ActiveEquilibriumCheckBoxItems[index - 1].Items[bits_0_5 - 1].A.IsChecked = true;
                                        break;
                                    // Bit7..Bit6 = 10 表示放电均衡
                                    case 2:
                                        ActiveEquilibriumCheckBoxItems[index - 1].Items[bits_0_5 - 1].B.IsChecked = true;
                                        break;
                                }
                            }
                        }
                        else if (SelectedEquilibriumType == "0： 被动均衡")
                        {
                            SelectedPackageNumber = PackageNumberList[data[1] - 1];

                            if (ConstantDef.BatteryCellNumber <= 48)
                            {

                                if (SelectedModuleNumber == ModuleNumberList[bit3_8 - 1]) //显示手动选择的模块号对应状态
                                {
                                    //Byte3 ~ Byte4：第一片电压采集芯片均衡通道。低通道号在高字节，高通道号在低字节。
                                    //Byte5 ~ Byte6：第二片电压采集芯片均衡通道。低通道号在高字节，高通道号在低字节。
                                    //Byte7 ~ Byte8：第三片电压采集芯片均衡通道。低通道号在高字节，高通道号在低字节。
                                    SetCellChecked(2, 8);   // 对应 data[2] 和 IsChecked_cell_9  到 IsChecked_cell_16
                                    SetCellChecked(3, 0);   // 对应 data[3] 和 IsChecked_cell_1  到 IsChecked_cell_8
                                    SetCellChecked(4, 24);  // 对应 data[4] 和 IsChecked_cell_25 到 IsChecked_cell_32
                                    SetCellChecked(5, 16);  // 对应 data[5] 和 IsChecked_cell_17 到 IsChecked_cell_24
                                    SetCellChecked(6, 40);  // 对应 data[6] 和 IsChecked_cell_41 到 IsChecked_cell_48
                                    SetCellChecked(7, 32);  // 对应 data[7] 和 IsChecked_cell_33 到 IsChecked_cell_40
                                }

                            }
                            else
                            {
                                if (SelectedModuleNumber == ModuleNumberList[bit3_8 - 1]) //显示手动选择的模块号对应状态
                                {
                                    if (SelectedPackageNumber % 2 == 1)
                                    {
                                        //Byte3 ~ Byte4：第一片电压采集芯片均衡通道。低通道号在高字节，高通道号在低字节。
                                        //Byte5 ~ Byte6：第二片电压采集芯片均衡通道。低通道号在高字节，高通道号在低字节。
                                        //Byte7 ~ Byte8：第三片电压采集芯片均衡通道。低通道号在高字节，高通道号在低字节。
                                        SetCellChecked(2, 8);   // 对应 data[2] 和 IsChecked_cell_9  到 IsChecked_cell_16
                                        SetCellChecked(3, 0);   // 对应 data[3] 和 IsChecked_cell_1  到 IsChecked_cell_8
                                        SetCellChecked(4, 24);  // 对应 data[4] 和 IsChecked_cell_25 到 IsChecked_cell_32
                                        SetCellChecked(5, 16);  // 对应 data[5] 和 IsChecked_cell_17 到 IsChecked_cell_24
                                        SetCellChecked(6, 40);  // 对应 data[6] 和 IsChecked_cell_41 到 IsChecked_cell_48
                                        SetCellChecked(7, 32);  // 对应 data[7] 和 IsChecked_cell_33 到 IsChecked_cell_40
                                    }
                                    else
                                    {
                                        SetCellChecked(2, 56);   // 对应 data[2] 和 IsChecked_cell_57  到 IsChecked_cell_64
                                        SetCellChecked(3, 48);   // 对应 data[3] 和 IsChecked_cell_49  到 IsChecked_cell_56
                                    }

                                }
                            }

                            void SetCellChecked(int dataIndex, int cellBase)
                            {
                                for (int k = 8; k > 0; k--)
                                {
                                    // 获取指定data的第8-k位
                                    bool isChecked = GetBit(data[dataIndex], (short)(8 - k)) == 1;
                                    // 设置对应属性值
                                    //GetType().GetProperty($"IsChecked_cell_{cellBase + k}").SetValue(this, isChecked);
                                    PassiveEquilibriumCheckBoxItems[cellBase + k - 1].IsChecked = isChecked;
                                }
                            }
                        }
                        break;

                    //EVBCM 应答 PC 请求从控模块失联状态信息消息（命令码 0x3C）
                    case 0x3C:

                        for (int j = 0; j < 8; j++)
                        {
                            for (int k = 0; k < 8; k++)
                            {
                                //1 表示正常，0 表示失联
                                if (GetBit(data[j], (short)k) == 1)
                                {
                                    GetType().GetProperty($"IsOutOfContact_ControlModule_{j * 8 + k + 1}").SetValue(this, false);
                                }
                                else
                                {
                                    GetType().GetProperty($"IsOutOfContact_ControlModule_{j * 8 + k + 1}").SetValue(this, true);
                                }
                            }
                        }
                        break;

                    case 0x3D:
                        //从控模块号
                        SelectedModuleNum = Convert.ToInt32(data[0]) - 1;
                        //IC 类型：默认 1
                        ICType = "1";
                        //IC 序号
                        ICNum = Convert.ToInt32(data[1]).ToString();
                        //采集线束类型
                        int LineType = GetBit(data[2], (short)0);
                        switch (LineType)
                        {
                            case 0:
                                BatteryTemperatureCollectionHarness = data[2].ToString();
                                TemperaturesNumber = ((data[2] >> 1) & 0b1111111).ToString();//bit1~7
                                break;
                            case 1:
                                BatteryVoltageCollectionHarness = data[2].ToString();
                                BatteryNumber = ((data[2] >> 1) & 0b1111111).ToString();//bit1~7
                                break;
                        }
                        BatteryTotalNumber = Convert.ToInt32(data[3]).ToString();

                        for (int j = 4; j < 8; j++)
                        {
                            for (int k = 0; k < 8; k++)
                            {
                                //掉线状态 1 表示异常，0 表示正常
                                if (GetBit(data[j], (short)k) == 1)
                                {
                                    GetType().GetProperty($"IsDisconnect_Bat_{(j - 4) * 8 + k + 1}").SetValue(this, true);
                                }
                                else
                                {
                                    GetType().GetProperty($"IsDisconnect_Bat_{(j - 4) * 8 + k + 1}").SetValue(this, false);
                                }
                            }
                        }
                        break;
                    //主控绝缘检测状态ID: 0x0881F4XX (XX:0xE8~0xFB)
                    case 0x81:
                        InsulationDetectionStatus = InsulationDetectionStatusList[Convert.ToInt32(data[0]) + 1];
                        break;


                    // 返回版本号消息（命令码 0x41）
                    case 0x41:

                        string MainControlSoftwareVersion_1_6 = "";
                        string MainControlSoftwareVersion_7_10 = "";
                        string SlaveControlSoftwareVersion_1_6 = "";
                        string SlaveControlSoftwareVersion_7_10 = "";
                        string Year = "";
                        string Month = "";
                        string Day = "";
                        string ProjectMajorVersion = "";
                        string ProjectMiniVersion = "";
                        string ProjectTestVersion = "";

                        switch (data[0])
                        {
                            case 0x01:
                                if (!isVersionInfoFetched) // 检查是否已获取版本信息
                                {
                                    MainControlSoftwareVersion_1_6 = Encoding.ASCII.GetString(data, 2, 1) + "-" + data[3].ToString() + "-" + data[4].ToString() + "." + data[5].ToString() + "." + data[6].ToString() + "." + data[7].ToString();
                                    validDataFrameCount++;
                                }
                                break;
                            case 0x02:
                                if (!isVersionInfoFetched)
                                {
                                    MainControlSoftwareVersion_7_10 = Encoding.ASCII.GetString(data, 2, 1) + Encoding.ASCII.GetString(data, 3, 1) + data[4].ToString() + "." + data[5].ToString();
                                    CommunicationProtocolVersion = (data[6] << 8 | data[7]).ToString();
                                    validDataFrameCount++;
                                }
                                break;
                            case 0x03:
                                SlaveControlSoftwareVersion_1_6 = Encoding.ASCII.GetString(data, 2, 1) + "-" + data[3].ToString() + "-" + data[4].ToString() + "." + data[5].ToString() + "." + data[6].ToString() + "." + data[7].ToString();
                                SlaveControlNum = data[1];
                                break;
                            case 0x04:
                                SlaveControlSoftwareVersion_7_10 = Encoding.ASCII.GetString(data, 2, 1) + Encoding.ASCII.GetString(data, 3, 1) + data[4].ToString() + "." + data[5].ToString();
                                SlaveControlNum = data[1];
                                break;
                            case 0x05:
                                if (!isVersionInfoFetched)
                                {
                                    Year = (data[1] + 2000).ToString();
                                    Month = data[2].ToString("D2");
                                    Day = data[3].ToString("D2");
                                    ProjectMajorVersion = data[4].ToString("D2");
                                    ProjectMiniVersion = data[5].ToString("D2");
                                    ProjectTestVersion = data[6].ToString("D2");
                                    validDataFrameCount++;
                                }
                                break;
                        }

                        // 当有效数据帧数量达到3且数据完整时，停止更新两个版本
                        if (validDataFrameCount >= 3)
                        {
                            isVersionInfoFetched = true; // 设置标志为已获取
                        }

                        // 累加版本信息
                        if (!string.IsNullOrEmpty(MainControlSoftwareVersion_1_6)) MainControlSoftwareVersion = MainControlSoftwareVersion_1_6;
                        if (!string.IsNullOrEmpty(MainControlSoftwareVersion_7_10)) MainControlSoftwareVersion += ((MainControlSoftwareVersion == "" ? "" : " - ") + MainControlSoftwareVersion_7_10);

                        ProjectNum = Year + Month + Day + "-" + ProjectMajorVersion + "." + ProjectMiniVersion + "-" + ProjectTestVersion;


                        if (!string.IsNullOrEmpty(SlaveControlSoftwareVersion_1_6)) SlaveControlSoftwareVersion = SlaveControlSoftwareVersion_1_6;
                        if (!string.IsNullOrEmpty(SlaveControlSoftwareVersion_7_10))
                        {
                            SlaveControlSoftwareVersion += (SlaveControlSoftwareVersion == "" ? "" : " - ") + SlaveControlSoftwareVersion_7_10;
                            var propertyName = $"SlaveControlSoftwareVersion_{SlaveControlNum}";
                            var property = this.GetType().GetProperty(propertyName);
                            if (property != null && property.CanWrite)
                            {
                                property.SetValue(this, SlaveControlSoftwareVersion);
                            }
                        }
                        break;

                    //  EVBCM 应答 PC 请求电池单体电压消息（命令码 0x31）
                    //case 0x1831F4E8:
                    case 0x31:

                        // Byte 1:帧序号（n） 20帧 一帧3个电池数据 20帧共60个电池数据
                        int frameNumber = data[0];
                        // Byte 2: 包序号（N） 7包 1包20帧    420-384=36
                        int sequenceNumber = data[1];


                        //实际调试384个电池电压数据
                        if (sequenceNumber >= 0x01 && sequenceNumber <= 0x06)
                        {
                            if (frameNumber >= 0x01 && frameNumber <= 0x14)
                            {

                                int startBatteryIndex = GetBatteryStartIndex(sequenceNumber, frameNumber);
                                ProcessBatteryData(startBatteryIndex, data);
                            }

                        }
                        if (sequenceNumber == 0x07)
                        {
                            if (frameNumber >= 0x01 && frameNumber <= 0x08)
                            {

                                int startBatteryIndex = GetBatteryStartIndex(sequenceNumber, frameNumber);
                                ProcessBatteryData(startBatteryIndex, data);
                            }
                        }

                        int GetBatteryStartIndex(int sequenceNumber, int frameNumber)
                        {
                            // 每个包有60个电池电压数据，每帧3个电池电压数据                        
                            return (sequenceNumber - 1) * 60 + (frameNumber - 1) * 3;
                            //return sequenceNumber * 60 + (frameNumber - 1) * 3;
                        }


                        void ProcessBatteryData(int startBatteryIndex, byte[] data)
                        {

                            var batteryVoltages = new string[3 + startBatteryIndex];
                            for (int k = 0; k < 3; k++)
                            {
                                int byteIndex = 2 + k * 2;
                                double voltageRaw = (data[byteIndex] << 8) | data[byteIndex + 1];

                                // 电压数据 0.001V/bit 范围：0~5V
                                batteryVoltages[startBatteryIndex + k] = (voltageRaw * 0.001).ToString("F3");// 保留3位小数

                            }

                            // 修改或新增电压数据
                            for (int i = startBatteryIndex; i < batteryVoltages.Length; i++)
                            {
                                string sectionNumber = (i + 1).ToString() + "#";

                                // 查找是否已存在该编号的记录
                                var existingData = batteryVoltageDataList.FirstOrDefault(data => data.SectionNumber == sectionNumber);
                                if (existingData != null)
                                {
                                    // 更新已有记录的电压值
                                    existingData.Voltage = batteryVoltages[i];
                                }
                                else
                                {
                                    // 新增记录
                                    batteryVoltageDataList.Add(new RealtimeData_BMS1500V_BCU.batteryVoltageData
                                    {
                                        SectionNumber = sectionNumber,
                                        Voltage = batteryVoltages[i]
                                    });
                                }
                            }
                        }

                        //batteryVoltageDataList.Clear();
                        initCount++;
                        break;

                    //  EVBCM 应答 PC 请求电池单体温度消息（命令码 0x32）
                    //case 0x1832F4E8:
                    case 0x32:

                        // Byte 1:帧序号 1~20（n） 20帧 1帧6个电池温度数据
                        frameNumber = data[0];
                        // Byte 2: 包序号 1~4（N）（电压按 120 节电池划分的包数） 4*20*6=480
                        sequenceNumber = data[1];

                        // 实际调试224个电池温度数据
                        if (sequenceNumber == 0x01)
                        {
                            if (frameNumber >= 0x01 && frameNumber <= 0x14)
                            {
                                int startBatteryIndex = GetbatteryTemperaturesStartIndex(sequenceNumber, frameNumber);
                                ProcessBatteryTemperature(startBatteryIndex, data);
                            }
                        }

                        if (sequenceNumber == 0x02)
                        {
                            if (frameNumber >= 0x01 && frameNumber <= 0x12)
                            {
                                int startBatteryIndex = GetbatteryTemperaturesStartIndex(sequenceNumber, frameNumber);
                                ProcessBatteryTemperature(startBatteryIndex, data);
                            }
                        }
                        int GetbatteryTemperaturesStartIndex(int sequenceNumber, int frameNumber)
                        {
                            // 每个包有120个电池温度，每帧 6个电池数据                       
                            return (sequenceNumber - 1) * 120 + (frameNumber - 1) * 6;
                            //return sequenceNumber * 120 + (frameNumber - 1) * 6;
                        }

                        void ProcessBatteryTemperature(int startBatteryIndex, byte[] data)
                        {
                            var batteryTemperatures = new string[6 + startBatteryIndex];
                            for (int k = 0; k < 6; k++)
                            {
                                int byteIndex = k + 2;
                                double temperatureRaw = data[byteIndex];
                                // 温度数据 1/bit 偏移量-40
                                batteryTemperatures[startBatteryIndex + k] = (temperatureRaw - 40).ToString();
                            }

                            // 修改或新增温度数据
                            for (int i = startBatteryIndex; i < batteryTemperatures.Length; i++)
                            {
                                if (i <= Convert.ToInt32(TemperatureNumber) - 1)//去掉大于224编号
                                {
                                    string cellNumber = (i + 1).ToString() + "#";

                                    // 查找是否已存在该编号的记录
                                    var existingData = batteryTemperatureDataList.FirstOrDefault(data => data.CellNumber == cellNumber);
                                    if (existingData != null)
                                    {
                                        // 更新已有记录的温度值
                                        existingData.Temperature = batteryTemperatures[i];
                                    }
                                    else
                                    {
                                        // 新增记录
                                        batteryTemperatureDataList.Add(new RealtimeData_BMS1500V_BCU.batteryTemperatureData
                                        {
                                            CellNumber = cellNumber,
                                            Temperature = batteryTemperatures[i]
                                        });
                                    }
                                }
                            }

                        }
                        initCount++;
                        break;
                    //EVBCM 应答 PC 请求极柱温度消息（命令码（ 0x32+0x80））
                    //case 0x18B2F4E8:
                    case 0xB2:
                        // Byte 1:从控模块号（1~48）
                        frameNumber = data[0];
                        // Byte 2: 从控采集极柱个数 1~6

                        if (frameNumber >= 0x01 && frameNumber <= 0x08 && data[1] == 0x02)
                        {
                            model.PoleNumber = "16";
                            PoleNumber = model.PoleNumber;
                        }

                        int startIndex = (frameNumber - 1) * Convert.ToInt32(data[1]);

                        ProcesspoleTemperature(startIndex, data);

                        void ProcesspoleTemperature(int startIndex, byte[] data)
                        {
                            var poleTemperatures = new string[Convert.ToInt32(model.PoleNumber)];
                            for (int i = 0; i < 2; i++)
                            {
                                double poleTemperatureRaw = data[i + 2];
                                // 温度数据 1/bit 偏移量 -40
                                poleTemperatures[startIndex + i] = (poleTemperatureRaw - 40).ToString();
                            }

                            // 修改或新增极柱温度数据
                            for (int i = startIndex; i < poleTemperatures.Length; i++)
                            {
                                string cellNumber = (i + 1).ToString() + "#";

                                // 查找是否已存在该编号的记录
                                var existingData = poleTemperatureDataList.FirstOrDefault(data => data.CellNumber == cellNumber);
                                if (existingData != null)
                                {
                                    // 更新已有记录的极柱温度值
                                    existingData.PoleTemperature = poleTemperatures[i];
                                }
                                else
                                {
                                    // 新增记录
                                    poleTemperatureDataList.Add(new RealtimeData_BMS1500V_BCU.poleTemperatureData
                                    {
                                        CellNumber = cellNumber,
                                        PoleTemperature = poleTemperatures[i]
                                    });
                                }
                            }

                        }
                        initCount++;
                        break;
                    // EVBCM 应答 PC 请求电池单体 SOC 消息（命令码 0x33）
                    //case 0x1833F4E8:
                    case 0x33:
                        // Byte 1:帧序号 1~20（n）20帧 1帧6个电池SOC数据
                        frameNumber = data[0];
                        // Byte 2: 包序号 1~4（N）  共4包480个电池SOC数据
                        sequenceNumber = data[1];

                        // 实际调试384个电池SOC数据
                        //1-360
                        if (sequenceNumber >= 0x01 && sequenceNumber <= 0x03)
                        {
                            if (frameNumber >= 0x01 && frameNumber <= 0x14)
                            {
                                int startBatteryIndex = GetbatterySOCStartIndex(sequenceNumber, frameNumber);
                                ProcessBatterySOC(startBatteryIndex, data);
                            }
                        }

                        //360-384
                        if (sequenceNumber == 0x04)
                        {
                            if (frameNumber >= 0x01 && frameNumber <= 0x04)
                            {
                                int startBatteryIndex = GetbatterySOCStartIndex(sequenceNumber, frameNumber);
                                ProcessBatterySOC(startBatteryIndex, data);
                            }
                        }

                        int GetbatterySOCStartIndex(int sequenceNumber, int frameNumber)
                        {
                            // 每个包有120个电池SOC数据，每帧 6个电池SOC数据                       
                            return (sequenceNumber - 1) * 120 + (frameNumber - 1) * 6;
                            //return sequenceNumber * 120 + (frameNumber - 1) * 6;
                        }

                        void ProcessBatterySOC(int startBatteryIndex, byte[] data)
                        {
                            var batterySocs = new string[6 + startBatteryIndex];
                            for (int k = 0; k < 6; k++)
                            {
                                int byteIndex = k + 2;
                                double SocRaw = data[byteIndex];
                                // SOC数据 1/bit 范围：0-100
                                batterySocs[startBatteryIndex + k] = SocRaw.ToString();
                            }

                            //for (int i = startBatteryIndex; i < batterySocs.Length; i++)
                            //{
                            //    batterySocDataList.Add(new RealtimeData_BMS1500V_BCU.batterySocData
                            //    {
                            //        SectionNumber = (i + 1).ToString() + "#",
                            //        SOC = batterySohs[i]
                            //    });
                            //}

                            // 修改或新增SOC数据
                            for (int i = startBatteryIndex; i < batterySocs.Length; i++)
                            {
                                string sectionNumber = (i + 1).ToString() + "#";

                                // 查找是否已存在该编号的记录
                                var existingData = batterySocDataList.FirstOrDefault(data => data.SectionNumber == sectionNumber);
                                if (existingData != null)
                                {
                                    // 更新已有记录的电压值
                                    existingData.SOC = batterySocs[i];
                                }
                                else
                                {
                                    // 新增记录
                                    batterySocDataList.Add(new RealtimeData_BMS1500V_BCU.batterySocData
                                    {
                                        SectionNumber = sectionNumber,
                                        SOC = batterySocs[i]
                                    });
                                }
                            }
                        }
                        initCount++;
                        break;
                    // EVBCM 应答 PC 请求电池单体 SOH 消息（命令码 0x34）
                    //case 0x1834F4E8:
                    case 0x34:

                        // Byte 1:帧序号 1~20（n）
                        frameNumber = data[0];
                        // Byte 2: 包序号 1~4（N）（同请求包序号）                   
                        sequenceNumber = data[1];
                        if (sequenceNumber >= 0x01 && sequenceNumber <= 0x03)
                        {
                            if (frameNumber >= 0x01 && frameNumber <= 0x14)
                            {
                                int startBatteryIndex = GetbatterySOHStartIndex(sequenceNumber, frameNumber);
                                ProcessBatterySOH(startBatteryIndex, data);
                            }
                        }

                        if (sequenceNumber == 0x04)
                        {
                            if (frameNumber >= 0x01 && frameNumber <= 0x04)
                            {
                                int startBatteryIndex = GetbatterySOHStartIndex(sequenceNumber, frameNumber);
                                ProcessBatterySOH(startBatteryIndex, data);
                            }
                        }

                        int GetbatterySOHStartIndex(int sequenceNumber, int frameNumber)
                        {
                            // 每个包有120个电池SOH数据，每帧 6个电池SOH数据                       
                            return (sequenceNumber - 1) * 120 + (frameNumber - 1) * 6;
                            //return sequenceNumber * 120 + (frameNumber - 1) * 6;
                        }

                        void ProcessBatterySOH(int startBatteryIndex, byte[] data)
                        {
                            var batterySohs = new string[6 + startBatteryIndex];
                            for (int k = 0; k < 6; k++)
                            {
                                int byteIndex = k + 2;
                                double SohRaw = data[byteIndex];
                                // SOH数据 1/bit 范围：0-100
                                batterySohs[startBatteryIndex + k] = SohRaw.ToString();
                            }


                            //for (int i = startBatteryIndex; i < batterySohs.Length; i++)
                            //{
                            //    batterySohDataList.Add(new RealtimeData_BMS1500V_BCU.batterySohData
                            //    {
                            //        SectionNumber = (i + 1).ToString() + "#",
                            //        SOH = batterySohs[i]
                            //    });
                            //}

                            // 修改或新增SOH数据
                            for (int i = startBatteryIndex; i < batterySohs.Length; i++)
                            {
                                string sectionNumber = (i + 1).ToString() + "#";

                                // 查找是否已存在该编号的记录
                                var existingData = batterySohDataList.FirstOrDefault(data => data.SectionNumber == sectionNumber);
                                if (existingData != null)
                                {
                                    // 更新已有记录的电压值
                                    existingData.SOH = batterySohs[i];
                                }
                                else
                                {
                                    // 新增记录
                                    batterySohDataList.Add(new RealtimeData_BMS1500V_BCU.batterySohData
                                    {
                                        SectionNumber = sectionNumber,
                                        SOH = batterySocs[i]
                                    });
                                }
                            }


                        }
                        initCount++;
                        break;

                    // EVBCM 应答 PC 请求主控采集信息消息（命令码 0x35）
                    //case 0x1835F4E8:
                    case 0x35:

                        // Byte 1:包序号 1~2
                        sequenceNumber = data[0];
                        switch (sequenceNumber)
                        {
                            case 0x01:
                                //模块温度 分辨率 1℃/bit
                                model.ModuleTemperature = (Convert.ToInt32(data[1]) - 40).ToString();
                                ModuleTemperature = model.ModuleTemperature;

                                //组端电压 分辨率为 0.1V/bit
                                model.GroupTerminalVoltage = ((data[2] << 8 | data[3]) * 0.1).ToString("F1");
                                GroupTerminalVoltage = model.GroupTerminalVoltage;

                                //组端电流1 分辨率 0.1A/bit 偏移量：-1600
                                model.GroupEndCurrent_1 = ((decimal)(data[4] << 8 | data[5]) * 0.1m - 1600).ToString("F1");//保留精度                            
                                GroupEndCurrent_1 = model.GroupEndCurrent_1;

                                //绝缘电阻R+ 
                                model.InsulationResistance_R_Positive = (data[6] << 8 | data[7]).ToString();
                                InsulationResistance_R_Positive = model.InsulationResistance_R_Positive;
                                break;

                            case 0x02:
                                //绝缘电阻R- 
                                model.InsulationResistance_R_Negative = (data[1] << 8 | data[2]).ToString();
                                InsulationResistance_R_Negative = model.InsulationResistance_R_Negative;

                                //组端SOC 分辨率 1%/bit
                                model.GroupEndSOC = data[3].ToString();
                                GroupEndSOC = model.GroupEndSOC;

                                //供电电压 分辨率 0.1V/bit
                                model.SupplyVoltage = ((data[4] << 8 | data[5]) * 0.1).ToString("F1");
                                SupplyVoltage = model.SupplyVoltage;
                                supplyVoltages[0] = model.SupplyVoltage;// 供电电压一个

                                // 目前只有一个供电电压数据
                                for (int i = 0; i < 1; i++)
                                {
                                    string cellNumber = (i + 1).ToString() + "#";

                                    // 查找是否已存在该编号的记录
                                    var existingData = supplyVoltageDataList.FirstOrDefault(data => data.CellNumber == cellNumber);
                                    if (existingData != null)
                                    {
                                        // 更新已有记录的电压值
                                        existingData.SupplyVoltage = supplyVoltages[i];
                                    }
                                    else
                                    {
                                        // 新增记录
                                        supplyVoltageDataList.Add(new RealtimeData_BMS1500V_BCU.supplyVoltageData
                                        {
                                            CellNumber = cellNumber,
                                            SupplyVoltage = supplyVoltages[i]
                                        });
                                    }
                                }

                                //预充电压 分辨率 0.1V/bit
                                model.PreChargeVoltage = ((data[6] << 8 | data[7]) * 0.1).ToString("F1");
                                PreChargeVoltage = model.PreChargeVoltage;
                                break;

                            case 0x04:
                                //组端温度 1 
                                model.GroupEndTemperature_1 = (Convert.ToInt32(data[3]) - 40).ToString();
                                GroupEndTemperature_1 = model.GroupEndTemperature_1;

                                //组端温度 2 
                                model.GroupEndTemperature_2 = (Convert.ToInt32(data[4]) - 40).ToString();
                                GroupEndTemperature_2 = model.GroupEndTemperature_2;

                                //组端温度 3 
                                model.GroupEndTemperature_3 = (Convert.ToInt32(data[5]) - 40).ToString();
                                GroupEndTemperature_3 = model.GroupEndTemperature_3;

                                //组端温度 4 
                                model.GroupEndTemperature_4 = (Convert.ToInt32(data[6]) - 40).ToString();
                                GroupEndTemperature_4 = model.GroupEndTemperature_4;
                                break;
                            case 0x05:
                                //组端电流2 分辨率 0.1A/bit 偏移量：-1600                              
                                model.GroupEndCurrent_2 = ((decimal)(data[1] << 8 | data[2]) * 0.1m - 1600).ToString("F1");//保留精度    
                                GroupEndCurrent_2 = model.GroupEndCurrent_2;

                                //组端电流3 分辨率 0.1A/bit 偏移量：-1600
                                model.GroupEndCurrent_3 = ((decimal)(data[3] << 8 | data[4]) * 0.1m - 1600).ToString("F1");//保留精度    
                                GroupEndCurrent_3 = model.GroupEndCurrent_3;
                                break;
                        }



                        initCount++;
                        break;

                    // EVBCM 应答 PC 请求系统概要信息消息（命令码 0x36）
                    //case 0x1836F4E8:
                    case 0x36:
                        // Byte 1:包序号 1~7
                        sequenceNumber = data[0];

                        switch (sequenceNumber)
                        {
                            case 0x01:
                                //电压最大值（前一）                              
                                model.BeforeTheMaximumValue_Voltage = ((data[2] << 8 | data[3]) * 0.001).ToString("F3");

                                //电压最大值（前二）                              
                                model.TopTwoMaximumValue_Voltage = ((data[4] << 8 | data[5]) * 0.001).ToString("F3");

                                //电压最大值（前三）                              
                                model.TopThreeMaximumValue_Voltage = ((data[6] << 8 | data[7]) * 0.001).ToString("F3");

                                break;
                            case 0x02:
                                //电压最小值（前一）                              
                                model.BeforeTheMinimumValue_Voltage = ((data[2] << 8 | data[3]) * 0.001).ToString("F3");

                                //电压最小值（前二）                              
                                model.TopTwoMinimumValue_Voltage = ((data[4] << 8 | data[5]) * 0.001).ToString("F3");

                                //电压最小值（前三）                              
                                model.TopThreeMinimumValue_Voltage = ((data[6] << 8 | data[7]) * 0.001).ToString("F3");
                                break;

                            case 0x03:
                                //电压平均值                              
                                model.AverageValue_Voltage = ((data[2] << 8 | data[3]) * 0.001).ToString("F3");

                                //电压极差值                              
                                model.RangeValue_Voltage = ((data[4] << 8 | data[5]) * 0.001).ToString("F3");

                                //温度最大值（前一）                             
                                model.BeforeTheMaximumValue_Temperature = (Convert.ToInt32(data[6]) - 40).ToString();

                                //温度最大值（前二）                             
                                model.TopTwoMaximumValue_Temperature = (Convert.ToInt32(data[7]) - 40).ToString();

                                break;
                            case 0x04:
                                //温度最大值（前三）                             
                                model.TopThreeMaximumValue_Temperature = (Convert.ToInt32(data[1]) - 40).ToString();

                                //温度最小值（前一）                              
                                model.BeforeTheMinimumValue_Temperature = (Convert.ToInt32(data[2]) - 40).ToString();

                                //温度最小值（前二）                              
                                model.TopTwoMinimumValue_Temperature = (Convert.ToInt32(data[3]) - 40).ToString();

                                //温度最小值（前三）                              
                                model.TopThreeMinimumValue_Temperature = (Convert.ToInt32(data[4]) - 40).ToString();

                                //温度平均值                              
                                model.AverageValue_Temperature = (Convert.ToInt32(data[5]) - 40).ToString();

                                //温度极差值                             
                                model.RangeValue_Temperature = Convert.ToInt32(data[6]).ToString();//无需进行偏移-40

                                //SOC最大值（前一）                             
                                model.BeforeTheMaximumValue_SOC = data[7].ToString();

                                break;
                            case 0x05:
                                //SOC最大值（前二）                             
                                model.TopTwoMaximumValue_SOC = data[1].ToString();

                                //SOC最大值（前三）                             
                                model.TopThreeMaximumValue_SOC = data[2].ToString();

                                //SOC最小值（前一）                              
                                model.BeforeTheMinimumValue_SOC = data[3].ToString();

                                //SOC最小值（前二）                              
                                model.TopTwoMinimumValue_SOC = data[4].ToString();

                                //SOC最小值（前三）                              
                                model.TopThreeMinimumValue_SOC = data[5].ToString();

                                //SOC平均值                              
                                model.AverageValue_SOC = data[6].ToString();

                                //SOC极差值                             
                                model.RangeValue_SOC = data[7].ToString();

                                break;

                            case 0x06:
                                //SOH最大值（前一）                             
                                model.BeforeTheMaximumValue_SOH = data[1].ToString();

                                //SOH最大值（前二）                             
                                model.TopTwoMaximumValue_SOH = data[2].ToString();

                                //SOH最大值（前三）                             
                                model.TopThreeMaximumValue_SOH = data[3].ToString();

                                //SOH最小值（前一）                              
                                model.BeforeTheMinimumValue_SOH = data[4].ToString();

                                //SOH最小值（前二）                              
                                model.TopTwoMinimumValue_SOH = data[5].ToString();

                                //SOH最小值（前三）                              
                                model.TopThreeMinimumValue_SOH = data[6].ToString();

                                //SOH平均值                              
                                model.AverageValue_SOH = data[7].ToString();

                                break;
                            case 0x07:
                                //SOH极差值                             
                                model.RangeValue_SOH = data[1].ToString();
                                break;

                            case 0x28:
                                //电压最大值节号                           
                                model.VoltageMaxNum = (data[2] << 8 | data[3]).ToString();

                                //电压最小值节号                           
                                model.VoltageMinNum = (data[4] << 8 | data[5]).ToString();

                                //温度最大值节号                           
                                model.TemperatureMaxNum = (data[6] << 8 | data[7]).ToString();
                                break;
                            case 0x29:
                                //温度最小值节号                           
                                model.TemperatureMinNum = (data[2] << 8 | data[3]).ToString();

                                //SOC最大值节号                           
                                model.SOCMaxNum = (data[4] << 8 | data[5]).ToString();

                                //SOC最小值节号                           
                                model.SOCMinNum = (data[6] << 8 | data[7]).ToString();
                                break;
                            case 0x2A:
                                //SOH最大值节号                           
                                model.SOHMaxNum = (data[2] << 8 | data[3]).ToString();

                                //SOH最小值节号                           
                                model.SOHMinNum = (data[4] << 8 | data[5]).ToString();
                                break;

                        }

                        if (IsChecked_BatteryVoltage)
                        {
                            BeforeTheMaximumValue = model.BeforeTheMaximumValue_Voltage + " " + model.VoltageMaxNum + "#";
                            TopTwoMaximumValue = model.TopTwoMaximumValue_Voltage;//+ " " + model.VoltageMaxNum + "#";
                            TopThreeMaximumValue = model.TopThreeMaximumValue_Voltage; //+ " " + model.VoltageMaxNum + "#";
                            BeforeTheMinimumValue = model.BeforeTheMinimumValue_Voltage + " " + model.VoltageMinNum + "#";
                            TopTwoMinimumValue = model.TopTwoMinimumValue_Voltage;//+ " " + model.VoltageMinNum + "#";
                            TopThreeMinimumValue = model.TopThreeMinimumValue_Voltage;// + " " + model.VoltageMinNum + "#";
                            AverageValue = model.AverageValue_Voltage;
                            RangeValue = model.RangeValue_Voltage;
                        }

                        else if (IsChecked_BatteryTemperature)
                        {
                            BeforeTheMaximumValue = model.BeforeTheMaximumValue_Temperature + " " + model.TemperatureMaxNum + "#";
                            TopTwoMaximumValue = model.TopTwoMaximumValue_Temperature + " " + model.TemperatureMaxNum + "#";
                            TopThreeMaximumValue = model.TopThreeMaximumValue_Temperature + " " + model.TemperatureMaxNum + "#";
                            BeforeTheMinimumValue = model.BeforeTheMinimumValue_Temperature + " " + model.TemperatureMinNum + "#";
                            TopTwoMinimumValue = model.TopTwoMinimumValue_Temperature + " " + model.TemperatureMinNum + "#";
                            TopThreeMinimumValue = model.TopThreeMinimumValue_Temperature + " " + model.TemperatureMinNum + "#";
                            AverageValue = model.AverageValue_Temperature;
                            RangeValue = model.RangeValue_Temperature;
                        }

                        else if (IsChecked_BatterySOH)
                        {
                            BeforeTheMaximumValue = model.BeforeTheMaximumValue_SOH + " " + model.SOHMaxNum + "#";
                            TopTwoMaximumValue = model.TopTwoMaximumValue_SOH + " " + model.SOHMaxNum + "#";
                            TopThreeMaximumValue = model.TopThreeMaximumValue_SOH + " " + model.SOCMaxNum + "#";
                            BeforeTheMinimumValue = model.BeforeTheMinimumValue_SOH + " " + model.SOCMinNum + "#";
                            TopTwoMinimumValue = model.TopTwoMinimumValue_SOH + " " + model.SOCMinNum + "#";
                            TopThreeMinimumValue = model.TopThreeMinimumValue_SOH + " " + model.SOCMinNum + "#";
                            AverageValue = model.AverageValue_SOH;
                            RangeValue = model.RangeValue_SOH;
                        }

                        else if (IsChecked_BatterySOC)
                        {
                            BeforeTheMaximumValue = model.BeforeTheMaximumValue_SOC + " " + model.SOCMaxNum + "#";
                            TopTwoMaximumValue = model.TopTwoMaximumValue_SOC + " " + model.SOCMaxNum + "#";
                            TopThreeMaximumValue = model.TopThreeMaximumValue_SOC + " " + model.SOCMaxNum + "#";
                            BeforeTheMinimumValue = model.BeforeTheMinimumValue_SOC + " " + model.SOCMinNum + "#";
                            TopTwoMinimumValue = model.TopTwoMinimumValue_SOC + " " + model.SOCMinNum + "#";
                            TopThreeMinimumValue = model.TopThreeMinimumValue_SOC + " " + model.SOCMinNum + "#";
                            AverageValue = model.AverageValue_SOC;
                            RangeValue = model.RangeValue_SOC;
                        }
                        initCount++;
                        break;

                    // EVBCM 应答 PC 请求模块电池节数信息消息（命令码 0x37）
                    //case 0x1837F4E8:
                    case 0x37:
                        // Byte 1 0~N/7
                        sequenceNumber = data[0];
                        switch (sequenceNumber)
                        {
                            case 0x00:
                                //模块总数 N ( 0 ~ 60 )个
                                model.ModuleNumber = data[1].ToString();
                                ModuleNumber = model.ModuleNumber;

                                //总电池节数 ( 0 ~ 400 )节
                                model.BatteryCellsNumber = (data[2] << 8 | data[3]).ToString();
                                BatteryCellsNumber = model.BatteryCellsNumber;
                                break;
                        }
                        initCount++;
                        break;

                    // EVBCM 应答 PC 请求报警信息应答消息（命令码 0x38）
                    //case 0x1838F4E8:
                    case 0x38:
                        // Byte 1 0~N/7                       
                        sequenceNumber = data[0];
                        string[] msg = new string[2];//报警信息、中文/英文（英文暂为null）
                        //报警等级/类型
                        string AlarmLevel = GetAlarmLevelDescription(data[0].ToString());
                        //动作值
                        string ActionValue = ((data[4] << 8) | data[5]).ToString();
                        //动作节号
                        string BatterySectionNumber = ((data[6] << 8) | data[7]).ToString();
                        if (((data[6] << 8) | data[7]) == 0xFF) BatterySectionNumber = "-";//0xff:无效

                        switch (sequenceNumber)
                        {
                            case 0x00: // 报警等级 0：无报警
                                if (data[1] == 0x00 && data[2] == 0x00 && data[3] == 0x00 && data[4] == 0x00)
                                {
                                    // 报警信息为空
                                    break;
                                }
                                break;
                            case 0x01: // 报警等级 1：严重报警                             
                            case 0x02: // 报警等级 2：一般报警                               
                            case 0x03: // 报警等级 3：轻微报警
                                AnalyzeAlarm(4, sequenceNumber, data, msg, AlarmLevel, BatterySectionNumber);
                                break;
                            case 0x04: // 报警等级 4：设备硬件故障
                                AnalyzeAlarm(5, sequenceNumber, data, msg, AlarmLevel, BatterySectionNumber);
                                break;
                        }
                        initCount++;
                        break;
                    // EVBCM 应答 PC 请求 DI/DO 信息应答消息（命令码 0x39）
                    //case 0x1839F4E8:
                    case 0x39:

                        // Byte 1 序号                    
                        sequenceNumber = data[0];

                        // 定义DI/DO 状态描述
                        string[] DIStatusMessages = {"主正接触器反馈-有效"  ,"辅助开关反馈-有效"    ,"主负接触器反馈-有效","紧急停机信号-有效","上层监控故障信号-有效",
                                                     "高压箱断路器反馈-有效","DI7-有效"             ,"DI8-有效"           ,"DI9-有效"         ,"DI10-有效"            };
                        string[] DOStatusMessages = {"主正接触器-吸合"      ,"环流接触器-吸合"      ,"主负接触器-吸合"    ,"DO4-吸合"         ,
                                                     "指示灯-绿-吸合"       ,"指示灯-红-吸合"       ,"辅助接触器-吸合"    ,"DO8-吸合"         };

                        // 解析 DI
                        if (sequenceNumber == 0x00)
                        {
                            // BYTE2-DI1~DI8
                            for (int i = 0; i < 8; i++)
                            {
                                string status = GetBit(data[1], (short)i).ToString() == "1" ? DIStatusMessages[i] : DIStatusMessages[i].Replace("有效", "无效");
                                switch (i)
                                {
                                    case 0: DIStatus_DI1 = status; break;
                                    case 1: DIStatus_DI2 = status; break;
                                    case 2: DIStatus_DI3 = status; break;
                                    case 3: DIStatus_DI4 = status; break;
                                    case 4: DIStatus_DI5 = status; break;
                                    case 5: DIStatus_DI6 = status; break;
                                    case 6: DIStatus_DI7 = status; break;
                                    case 7: DIStatus_DI8 = status; break;
                                }
                            }

                            // BYTE3-DI9、DI10
                            for (int i = 0; i < 2; i++)
                            {
                                string status = GetBit(data[2], (short)i).ToString() == "1" ? DIStatusMessages[i + 8] : DIStatusMessages[i + 8].Replace("有效", "无效");
                                switch (i)
                                {
                                    case 0: DIStatus_DI9 = status; break;
                                    case 1: DIStatus_DI10 = status; break;
                                }
                            }
                        }

                        // 解析 DO
                        else if (sequenceNumber == 0x01)
                        {
                            // BYTE2-DO1~DO8
                            for (int i = 0; i < 8; i++)
                            {
                                bool isActive = GetBit(data[1], (short)i) == 1; // 获取当前位的状态
                                string status = isActive ? DOStatusMessages[i] : DOStatusMessages[i].Replace("吸合", "断开");

                                // 数组或列表来储存 DO 状态
                                string[] doStatuses = new string[8] { DOStatus_DO1, DOStatus_DO2, DOStatus_DO3, DOStatus_DO4,
                                                          DOStatus_DO5, DOStatus_DO6, DOStatus_DO7, DOStatus_DO8 };
                                doStatuses[i] = status;
                                // 更新 DO 状态
                                DOStatus_DO1 = doStatuses[0];
                                DOStatus_DO2 = doStatuses[1];
                                DOStatus_DO3 = doStatuses[2];
                                DOStatus_DO4 = doStatuses[3];
                                DOStatus_DO5 = doStatuses[4];
                                DOStatus_DO6 = doStatuses[5];
                                DOStatus_DO7 = doStatuses[6];
                                DOStatus_DO8 = doStatuses[7];

                                //禁用
                                //switch (i)
                                //{
                                //    case 0:
                                //        if (isActive) IsChecked_DO1_Open = true; else IsChecked_DO1_Close = true;
                                //        break;
                                //    case 1:
                                //        if (isActive) IsChecked_DO2_Open = true; else IsChecked_DO2_Close = true;
                                //        break;
                                //    case 2:
                                //        if (isActive) IsChecked_DO3_Open = true; else IsChecked_DO3_Close = true;
                                //        break;
                                //    case 3:
                                //        if (isActive) IsChecked_DO4_Open = true; else IsChecked_DO4_Close = true;
                                //        break;
                                //    case 4:
                                //        if (isActive) IsChecked_DO5_Open = true; else IsChecked_DO5_Close = true;
                                //        break;
                                //    case 5:
                                //        if (isActive) IsChecked_DO6_Open = true; else IsChecked_DO6_Close = true;
                                //        break;
                                //    case 6:
                                //        if (isActive) IsChecked_DO7_Open = true; else IsChecked_DO7_Close = true;
                                //        break;
                                //    case 7:
                                //        if (isActive) IsChecked_DO8_Open = true; else IsChecked_DO8_Close = true;
                                //        break;
                                //}
                            }

                            // BYTE3-系统状态                          
                            ChargeStatus = GetBit(data[2], 0).ToString() == "1" ? $"禁止(1)" : "允许(0)";
                            DischargeStatus = GetBit(data[2], 1).ToString() == "1" ? "禁止(1)" : "允许(0)";
                            SeriousFaultStatus = GetBit(data[2], 2).ToString() == "1" ? "有(1)" : "无(0)";
                            ClusterMode = GetBit(data[2], 3).ToString() == "1" ? "使能(1)" : "失能(0)";
                            SingleClusterMode = GetBit(data[2], 4).ToString() == "1" ? "使能(1)" : "失能(0)";

                        }
                        initCount++;
                        break;

                    // EVBCM 应答 PC 请求模块温度个数信息消息（命令码 0x3A）
                    //case 0x183AF4E8:
                    case 0x3A:
                        // Byte 1 0~N/7
                        sequenceNumber = data[0];
                        switch (sequenceNumber)
                        {
                            case 0x00:
                                ////模块总数 N ( 0 ~ 60 )个
                                //model.ModuleNumber = data[1].ToString();
                                //ModuleNumber = model.ModuleNumber;

                                //温度总数 ( 0 ~ 400 )节
                                model.TemperatureNumber = (data[2] << 8 | data[3]).ToString();
                                TemperatureNumber = model.TemperatureNumber;
                                break;
                        }
                        initCount++;
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        public void AnalyzeAlarm(int ByteNum, int sequenceNumber, byte[] data, string[] msg, string alarmLevel, string BatterySectionNumber)
        {
            for (int i = 1; i < ByteNum; i++)
            {
                for (short j = 0; j < 8; j++)
                {
                    int alarmType = sequenceNumber == 0x04 ? 2 : 1;
                    if (GetBit(data[i], j) == 1)
                    {
                        // 报警状态为激活
                        UpdateAlarmState(out msg, i, j, alarmType, 0, sequenceNumber);//state=0 未激活
                        string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        // 检查是否已有激活报警记录
                        if (msg[0] != null)
                        {
                            if (!AlarmMessageDataList.Any(x => x.AlarmMessage.Contains(msg[0]) && x.isEnd == "否" && x.AlarmLevel == alarmLevel))
                            {
                                AlarmMessageDataList.Add(new RealtimeData_BMS1500V_BCU.AlarmMessageData
                                {
                                    AlarmNumber = (AlarmMessageDataList.Count + 1).ToString(),
                                    AlarmStartTime = StartTime,
                                    AlarmLevel = alarmLevel,
                                    BatterySectionNumber = BatterySectionNumber,
                                    AlarmMessage = $"【异常报警🚨】 {msg[0]}",
                                    isEnd = "否"
                                });
                            }
                        }
                    }
                    else
                    {
                        // 解除报警状态
                        UpdateAlarmState(out msg, i, j, alarmType, 1, sequenceNumber);//state=1 可解除

                        string StopTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        // 查找激活报警记录以确认解除
                        if (msg[0] != null)
                        {
                            var activeAlarm = AlarmMessageDataList.FirstOrDefault(x => x.AlarmMessage.Contains(msg[0]) && x.isEnd == "否" && x.AlarmLevel == alarmLevel);
                            if (activeAlarm != null)
                            {
                                activeAlarm.AlarmStopTime = StopTime;
                                activeAlarm.AlarmMessage = $"【报警解除🆗】 {msg[0]}";
                                activeAlarm.isEnd = "是";
                            }
                        }
                    }
                }
            }

            var alarmMessageDataList = AlarmMessageDataList.Where(x => x.AlarmLevel == "一般报警" ||
                                                                       x.AlarmLevel == "轻微报警" ||
                                                                       x.AlarmLevel == "严重报警" ||
                                                                       x.AlarmLevel == "设备硬件故障").ToList();
            if (alarmMessageDataList.Any())
            {
                foreach (var alarmMessageData in alarmMessageDataList)
                {
                    switch (alarmMessageData.AlarmLevel)
                    {
                        case "轻微报警":
                            model.MinorAlarm = alarmMessageData.AlarmMessage;
                            break;
                        case "一般报警":
                            model.GeneralAlarm = alarmMessageData.AlarmMessage;
                            break;
                        case "严重报警":
                            model.SevereAlarm = alarmMessageData.AlarmMessage;
                            break;
                        case "设备硬件故障":
                            model.EquipmentHardwareFailureAlarm = alarmMessageData.AlarmMessage;
                            break;
                    }
                }
            }
        }

        public ICommand Write_0x26_Cmd => new RelayCommand(Write_0x26);
        /// <summary>
        /// 设置传感器报文（命令码 0x03）
        /// </summary>
        public void Write_0x26()
        {
            byte[] data = new byte[8];
            byte Address_BCU = Convert.ToByte(Convert.ToInt32(SelectedAddress_BCU, 16));
            //包序号（1~0xFE，0xFF 表示最后一包）
            data[0] = 0x01;
            data[1] = 0x00;
            if (IsChecked_Hall1_effective == true) data[1] |= 0x01; // Bit0：1-霍尔 1 有效，0-霍尔 1 无效
            if (IsChecked_Hall2_effective == true) data[1] |= 0x02; // Bit1：1-霍尔 2 有效，0-霍尔 2 无效
            if (IsChecked_Hall3_effective == true) data[1] |= 0x04; // Bit2：1-霍尔 3 有效，0-霍尔 3 无效
            if (IsChecked_DoubleHall == true) data[1] |= 0x80;      // Bit7：1-双霍尔传感器，0-非双霍尔传感器

            data[2] = (byte)((Convert.ToInt32(CurrentSensorRange_Hall_1) >> 8) & 0xFF);
            data[3] = (byte)(Convert.ToInt32(CurrentSensorRange_Hall_1) & 0xFF);

            data[4] = (byte)((Convert.ToInt32(CurrentSensorRange_Hall_2) >> 8) & 0xFF);
            data[5] = (byte)(Convert.ToInt32(CurrentSensorRange_Hall_2) & 0xFF);

            data[6] = (byte)((Convert.ToInt32(CurrentSensorRange_Hall_3) >> 8) & 0xFF);
            data[7] = (byte)(Convert.ToInt32(CurrentSensorRange_Hall_3) & 0xFF);

            byte[] can_id = new byte[] { 0xF4, Address_BCU, 0x03, 0x18 };
            if (baseCanHelper.Send(data, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);
        }
        public ICommand Write_0x00_Cmd => new RelayCommand(Write_0x00);
        /// <summary>
        /// 设置RTC 数据（命令码 0x00）
        /// </summary>
        public void Write_0x00()
        {
            byte Address_BCU = Convert.ToByte(Convert.ToInt32(SelectedAddress_BCU, 16));
            byte[] can_id = new byte[] { 0xF4, Address_BCU, 0x00, 0x18 };
            var strArray = Encoding.ASCII.GetBytes(ProductName.Replace("PowerMagic ", ""));
            byte[] bytes = new byte[] {0x04,
                                       strArray[0],
                                       strArray[2],
                                       0,
                                       0,
                                       0,
                                       0,
                                       0};

            if (ProductName == "PowerMagic 1.0")
            {
                ConstantDef.BatteryCellNumber = 48;
                ConstantDef.BatteryTemperatureNumber = 28;
            }
            else
            {
                ConstantDef.BatteryCellNumber = 64;
                ConstantDef.BatteryTemperatureNumber = 36;
            }

            if (baseCanHelper.Send(bytes, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);
        }
        public ICommand Write_0x12_Cmd => new RelayCommand(Write_0x12);
        /// <summary>
        /// 设置RTC 数据（命令码 0x12）
        /// </summary>
        public void Write_0x12()
        {
            byte Address_BCU = Convert.ToByte(Convert.ToInt32(SelectedAddress_BCU, 16));
            byte[] can_id = new byte[] { 0xF4, Address_BCU, 0x12, 0x18 };

            DateTime SystemTime = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day,
                                  int.Parse(SelectedHour), int.Parse(SelectedMinute), int.Parse(SelectedSecond));
            string[] date = SystemTime.ToString("yyyy-MM-dd HH:mm:ss").Split(new char[] { ' ', '-', ':' });

            byte[] bytes = new byte[] { Convert.ToByte((Convert.ToInt32(date[0]) >> 8) & 0xFF),
                                        Convert.ToByte(Convert.ToInt32(date[0]) & 0xFF),
                                        Convert.ToByte(Convert.ToInt32(date[1])),
                                        Convert.ToByte(Convert.ToInt32(date[2])),
                                        Convert.ToByte(Convert.ToInt32(date[3])),
                                        Convert.ToByte(Convert.ToInt32(date[4])),
                                        Convert.ToByte(Convert.ToInt32(date[5])),
                                        0x00 };

            if (baseCanHelper.Send(bytes, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);
        }


        public ICommand Write_0x16_Cmd => new RelayCommand(Write_0x16);
        /// <summary>
        /// 电池主参数（命令码 0x16）
        /// </summary>
        public void Write_0x16()
        {
            byte Address_BCU = Convert.ToByte(Convert.ToInt32(SelectedAddress_BCU, 16));
            byte[] can_id = new byte[] { 0xF4, Address_BCU, 0x16, 0x18 };
            byte[] data = new byte[8];
            data[0] = (byte)(BatteryTypeList.IndexOf(SelectedBatteryType) + 1);       // 电池类型
            data[1] = Convert.ToByte((Convert.ToInt32(BatteryCapacity) >> 8) & 0xFF); // 电池容量高字节
            data[2] = Convert.ToByte(Convert.ToInt32(BatteryCapacity) & 0xFF);        // 电池容量低字节
            data[3] = Convert.ToByte(Convert.ToInt32(BatteryModel));                  // 电池型号
            data[4] = Convert.ToByte(Convert.ToInt32(BatteryManufacturer));           // 电池厂家

            if (baseCanHelper.Send(data, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);

        }

        public ICommand Write_0x14_Cmd => new RelayCommand(Write_0x14);
        /// <summary>
        /// 主控模块基本参数（命令码 0x14）
        /// </summary>
        public void Write_0x14()
        {

            byte Address_BCU = Convert.ToByte(Convert.ToInt32(SelectedAddress_BCU, 16));
            byte[] can_id = new byte[] { 0xF4, Address_BCU, 0x14, 0x18 };
            byte[] data = new byte[8];
            data[0] = 0x01;//其他包序号预留
            data[1] = Convert.ToByte(Convert.ToInt32(SystemTotalModuleNumber));                     // 系统总模块数
            data[2] = Convert.ToByte((Convert.ToInt32(SystemTotalbatteryNumber) >> 8) & 0xFF);      // 系统总电池节数高字节
            data[3] = Convert.ToByte(Convert.ToInt32(SystemTotalbatteryNumber) & 0xFF);             // 系统总电池节数低字节
            data[4] = Convert.ToByte((Convert.ToInt32(SystemTotalTemperaturesNumber) >> 8) & 0xFF); // 系统总温度个数高字节
            data[5] = Convert.ToByte(Convert.ToInt32(SystemTotalTemperaturesNumber) & 0xFF);        // 系统总温度个数低字节

            if (baseCanHelper.Send(data, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);
        }

        public ICommand Write_0x1A_SOC_Cmd => new RelayCommand(Write_0x1A_SOC);
        /// <summary>
        /// 设置SOC参数（命令码 0x1A）
        /// </summary>
        public void Write_0x1A_SOC()
        {
            byte Address_BCU = Convert.ToByte(Convert.ToInt32(SelectedAddress_BCU, 16));
            byte[] can_id = new byte[] { 0xF4, Address_BCU, 0x1A, 0x18 };
            byte[] data = new byte[8];

            data[0] = 0x00; //参数类型—SOC
            data[1] = Convert.ToByte((Convert.ToInt32(SelectedBatteryNumber_SOC) >> 8) & 0xFF); // 设置SOC电池序号高字节
            data[2] = Convert.ToByte(Convert.ToInt32(SelectedBatteryNumber_SOC) & 0xFF);        // 设置SOC电池序号低字节
            data[3] = Convert.ToByte((Convert.ToInt32(SOC) >> 8) & 0xFF);                       // SOC值高字节
            data[4] = Convert.ToByte(Convert.ToInt32(SOC) & 0xFF);                              // SOC值低字节

            if (baseCanHelper.Send(data, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);
        }

        public ICommand Write_0x1A_SOH_Cmd => new RelayCommand(Write_0x1A_SOH);
        /// <summary>
        /// 设置SOH参数（命令码 0x1A）
        /// </summary>
        public void Write_0x1A_SOH()
        {
            byte Address_BCU = Convert.ToByte(Convert.ToInt32(SelectedAddress_BCU, 16));
            byte[] can_id = new byte[] { 0xF4, Address_BCU, 0x1A, 0x18 };
            byte[] data = new byte[8];

            data[0] = 0x01; //参数类型—SOH
            data[1] = Convert.ToByte((Convert.ToInt32(SelectedBatteryNumber_SOH) >> 8) & 0xFF); // 设置SOH电池序号高字节
            data[2] = Convert.ToByte(Convert.ToInt32(SelectedBatteryNumber_SOH) & 0xFF);        // 设置SOH电池序号低字节
            data[3] = Convert.ToByte(Convert.ToInt32(SOH));                                    // SOH值


            if (baseCanHelper.Send(data, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);
        }

        /// <summary>
        /// 激活报警/解除报警
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="type"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public string[] UpdateAlarmState(out string[] msg, int row, int column, int type, int state, int alarmLevel)
        {
            msg = new string[2];
            FaultInfo faultInfo = FaultInfo.FaultInfos4.FirstOrDefault(f => f.Byte == row && f.Bit == column && f.Type == type && f.State == state && f.Value == alarmLevel);

            if (faultInfo != null)
            {
                msg[0] = faultInfo.Content.Trim();
                faultInfo.State = state == 0 ? 1 : 0; // 更新报警状态
            }

            return msg;
        }

        /// <summary>
        /// 获取指定位的值
        /// </summary>
        /// <param name="b"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetBit(byte b, short index)
        {
            return (b >> index) & 0x01;
        }

        /// <summary>
        /// 获取报警等级描述
        /// </summary>
        /// <param name="Alarm"></param>
        /// <returns></returns>
        public string GetAlarmLevelDescription(string Alarm)
        {
            switch (Alarm)
            {
                case "0": return "无报警";
                case "1": return "严重报警";
                case "2": return "一般报警";
                case "3": return "轻微报警";
                case "4": return "设备硬件故障";
                default: return "未知报警描述";
            }
        }
        public void PackageNumber_SelectedIndexChanged(int index)
        {
            int offset = 64 * (index - 1);

            PassiveEquilibriumState = $"电芯{1 + offset}~{64 + offset}的被动均衡状态";

            for (int i = 1; i <= 64; i++)
            {
                PassiveEquilibriumCheckBoxItems[i - 1].Label = $"电芯{i + offset}";

            }
        }
    }
}