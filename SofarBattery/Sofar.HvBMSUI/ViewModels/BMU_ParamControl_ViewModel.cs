using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Win32;
using Sofar.BMSLib;
using Sofar.BMSUI;
using Sofar.BMSUI.Common;
using Sofar.ProtocolLib;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace Sofar.HvBMSUI.ViewModels
{
    public partial class BMU_ParamControl_ViewModel : ObservableObject
    {
        BaseCanHelper baseCanHelper = null;
        public CancellationTokenSource cts = null;
        public static Dictionary<string, string> keys = new Dictionary<string, string>();

        public ObservableCollection<int> Address_BMU_List { get; } = new ObservableCollection<int>(Enumerable.Range(1, 255));
        private int _selectedAddress_BMU = 1;
        /// <summary>
        /// BMU地址
        /// </summary>
        public int SelectedAddress_BMU
        {
            get { return _selectedAddress_BMU; }
            set
            {
                _selectedAddress_BMU = value;
                OnPropertyChanged(nameof(SelectedAddress_BMU));
            }
        }

        private string _SoftwareVersion;
        /// <summary>
        /// 软件版本
        /// </summary>
        public string SoftwareVersion
        {
            get => _SoftwareVersion;
            set => SetProperty(ref _SoftwareVersion, value);
        }

        private string _HardwareVersion;
        /// <summary>
        /// 硬件版本
        /// </summary>
        public string HardwareVersion
        {
            get => _HardwareVersion;
            set => SetProperty(ref _HardwareVersion, value);
        }

        private string _alarmParameter_1_1;
        public string AlarmParameter_1_1
        {
            get => _alarmParameter_1_1;
            set => SetProperty(ref _alarmParameter_1_1, value);
        }

        private string _alarmParameter_1_2;
        public string AlarmParameter_1_2
        {
            get => _alarmParameter_1_2;
            set => SetProperty(ref _alarmParameter_1_2, value);
        }

        private string _alarmParameter_1_3;
        public string AlarmParameter_1_3
        {
            get => _alarmParameter_1_3;
            set => SetProperty(ref _alarmParameter_1_3, value);
        }

        private string _alarmParameter_1_4;
        public string AlarmParameter_1_4
        {
            get => _alarmParameter_1_4;
            set => SetProperty(ref _alarmParameter_1_4, value);
        }

        private string _alarmParameter_2_1;
        public string AlarmParameter_2_1
        {
            get => _alarmParameter_2_1;
            set => SetProperty(ref _alarmParameter_2_1, value);
        }

        private string _alarmParameter_2_2;
        public string AlarmParameter_2_2
        {
            get => _alarmParameter_2_2;
            set => SetProperty(ref _alarmParameter_2_2, value);
        }

        private string _alarmParameter_2_3;
        public string AlarmParameter_2_3
        {
            get => _alarmParameter_2_3;
            set => SetProperty(ref _alarmParameter_2_3, value);
        }

        private string _alarmParameter_2_4;
        public string AlarmParameter_2_4
        {
            get => _alarmParameter_2_4;
            set => SetProperty(ref _alarmParameter_2_4, value);
        }

        private string _alarmParameter_3_1;
        public string AlarmParameter_3_1
        {
            get => _alarmParameter_3_1;
            set => SetProperty(ref _alarmParameter_3_1, value);
        }

        private string _alarmParameter_3_2;
        public string AlarmParameter_3_2
        {
            get => _alarmParameter_3_2;
            set => SetProperty(ref _alarmParameter_3_2, value);
        }

        private string _alarmParameter_3_3;
        public string AlarmParameter_3_3
        {
            get => _alarmParameter_3_3;
            set => SetProperty(ref _alarmParameter_3_3, value);
        }

        private string _alarmParameter_3_4;
        public string AlarmParameter_3_4
        {
            get => _alarmParameter_3_4;
            set => SetProperty(ref _alarmParameter_3_4, value);
        }

        private string _alarmParameter_4_1;
        public string AlarmParameter_4_1
        {
            get => _alarmParameter_4_1;
            set => SetProperty(ref _alarmParameter_4_1, value);
        }

        private string _alarmParameter_4_2;
        public string AlarmParameter_4_2
        {
            get => _alarmParameter_4_2;
            set => SetProperty(ref _alarmParameter_4_2, value);
        }

        private string _alarmParameter_4_3;
        public string AlarmParameter_4_3
        {
            get => _alarmParameter_4_3;
            set => SetProperty(ref _alarmParameter_4_3, value);
        }

        private string _alarmParameter_4_4;
        public string AlarmParameter_4_4
        {
            get => _alarmParameter_4_4;
            set => SetProperty(ref _alarmParameter_4_4, value);
        }

        private string _alarmParameter_5_1;
        public string AlarmParameter_5_1
        {
            get => _alarmParameter_5_1;
            set => SetProperty(ref _alarmParameter_5_1, value);
        }

        private string _alarmParameter_5_2;
        public string AlarmParameter_5_2
        {
            get => _alarmParameter_5_2;
            set => SetProperty(ref _alarmParameter_5_2, value);
        }

        private string _alarmParameter_5_3;
        public string AlarmParameter_5_3
        {
            get => _alarmParameter_5_3;
            set => SetProperty(ref _alarmParameter_5_3, value);
        }

        private string _alarmParameter_5_4;
        public string AlarmParameter_5_4
        {
            get => _alarmParameter_5_4;
            set => SetProperty(ref _alarmParameter_5_4, value);
        }

        private string _alarmParameter_6_1;
        public string AlarmParameter_6_1
        {
            get => _alarmParameter_6_1;
            set => SetProperty(ref _alarmParameter_6_1, value);
        }

        private string _alarmParameter_6_2;
        public string AlarmParameter_6_2
        {
            get => _alarmParameter_6_2;
            set => SetProperty(ref _alarmParameter_6_2, value);
        }

        private string _alarmParameter_6_3;
        public string AlarmParameter_6_3
        {
            get => _alarmParameter_6_3;
            set => SetProperty(ref _alarmParameter_6_3, value);
        }

        private string _alarmParameter_6_4;
        public string AlarmParameter_6_4
        {
            get => _alarmParameter_6_4;
            set => SetProperty(ref _alarmParameter_6_4, value);
        }

        private string _alarmParameter_7_1;
        public string AlarmParameter_7_1
        {
            get => _alarmParameter_7_1;
            set => SetProperty(ref _alarmParameter_7_1, value);
        }

        private string _alarmParameter_7_2;
        public string AlarmParameter_7_2
        {
            get => _alarmParameter_7_2;
            set => SetProperty(ref _alarmParameter_7_2, value);
        }

        private string _alarmParameter_7_3;
        public string AlarmParameter_7_3
        {
            get => _alarmParameter_7_3;
            set => SetProperty(ref _alarmParameter_7_3, value);
        }

        private string _alarmParameter_7_4;
        public string AlarmParameter_7_4
        {
            get => _alarmParameter_7_4;
            set => SetProperty(ref _alarmParameter_7_4, value);
        }

        private string _alarmParameter_7_5;
        public string AlarmParameter_7_5
        {
            get => _alarmParameter_7_5;
            set => SetProperty(ref _alarmParameter_7_5, value);
        }

        private string _alarmParameter_7_6;
        public string AlarmParameter_7_6
        {
            get => _alarmParameter_7_6;
            set => SetProperty(ref _alarmParameter_7_6, value);
        }

        private string _alarmParameter_7_7;
        public string AlarmParameter_7_7
        {
            get => _alarmParameter_7_7;
            set => SetProperty(ref _alarmParameter_7_7, value);
        }

        private string _alarmParameter_7_8;
        public string AlarmParameter_7_8
        {
            get => _alarmParameter_7_8;
            set => SetProperty(ref _alarmParameter_7_8, value);
        }

        private string _alarmParameter_8_1;
        public string AlarmParameter_8_1
        {
            get => _alarmParameter_8_1;
            set => SetProperty(ref _alarmParameter_8_1, value);
        }

        private string _alarmParameter_8_2;
        public string AlarmParameter_8_2
        {
            get => _alarmParameter_8_2;
            set => SetProperty(ref _alarmParameter_8_2, value);
        }

        private string _alarmParameter_8_3;
        public string AlarmParameter_8_3
        {
            get => _alarmParameter_8_3;
            set => SetProperty(ref _alarmParameter_8_3, value);
        }

        private string _alarmParameter_8_4;
        public string AlarmParameter_8_4
        {
            get => _alarmParameter_8_4;
            set => SetProperty(ref _alarmParameter_8_4, value);
        }

        private string _alarmParameter_8_5;
        public string AlarmParameter_8_5
        {
            get => _alarmParameter_8_5;
            set => SetProperty(ref _alarmParameter_8_5, value);
        }

        private string _alarmParameter_8_6;
        public string AlarmParameter_8_6
        {
            get => _alarmParameter_8_6;
            set => SetProperty(ref _alarmParameter_8_6, value);
        }

        private string _alarmParameter_8_7;
        public string AlarmParameter_8_7
        {
            get => _alarmParameter_8_7;
            set => SetProperty(ref _alarmParameter_8_7, value);
        }

        private string _alarmParameter_8_8;
        public string AlarmParameter_8_8
        {
            get => _alarmParameter_8_8;
            set => SetProperty(ref _alarmParameter_8_8, value);
        }

        private string _alarmParameter_9_1;
        public string AlarmParameter_9_1
        {
            get => _alarmParameter_9_1;
            set => SetProperty(ref _alarmParameter_9_1, value);
        }

        private string _alarmParameter_9_2;
        public string AlarmParameter_9_2
        {
            get => _alarmParameter_3_2;
            set => SetProperty(ref _alarmParameter_9_2, value);
        }

        private string _alarmParameter_9_3;
        public string AlarmParameter_9_3
        {
            get => _alarmParameter_3_3;
            set => SetProperty(ref _alarmParameter_9_3, value);
        }

        private string _alarmParameter_9_4;
        public string AlarmParameter_9_4
        {
            get => _alarmParameter_9_4;
            set => SetProperty(ref _alarmParameter_9_4, value);
        }

        private string _alarmParameter_10_1;
        public string AlarmParameter_10_1
        {
            get => _alarmParameter_10_1;
            set => SetProperty(ref _alarmParameter_10_1, value);
        }

        private string _alarmParameter_10_2;
        public string AlarmParameter_10_2
        {
            get => _alarmParameter_10_2;
            set => SetProperty(ref _alarmParameter_10_2, value);
        }

        private string _alarmParameter_10_3;
        public string AlarmParameter_10_3
        {
            get => _alarmParameter_10_3;
            set => SetProperty(ref _alarmParameter_10_3, value);
        }

        private string _alarmParameter_10_4;
        public string AlarmParameter_10_4
        {
            get => _alarmParameter_10_4;
            set => SetProperty(ref _alarmParameter_10_4, value);
        }

        private string _alarmParameter_11_1;
        public string AlarmParameter_11_1
        {
            get => _alarmParameter_11_1;
            set => SetProperty(ref _alarmParameter_11_1, value);
        }

        private string _alarmParameter_11_2;
        public string AlarmParameter_11_2
        {
            get => _alarmParameter_11_2;
            set => SetProperty(ref _alarmParameter_11_2, value);
        }

        private string _alarmParameter_11_3;
        public string AlarmParameter_11_3
        {
            get => _alarmParameter_11_3;
            set => SetProperty(ref _alarmParameter_11_3, value);
        }

        private string _alarmParameter_11_4;
        public string AlarmParameter_11_4
        {
            get => _alarmParameter_11_4;
            set => SetProperty(ref _alarmParameter_11_4, value);
        }

        private string _alarmParameter_12_1;
        public string AlarmParameter_12_1
        {
            get => _alarmParameter_12_1;
            set => SetProperty(ref _alarmParameter_12_1, value);
        }

        private string _alarmParameter_12_2;
        public string AlarmParameter_12_2
        {
            get => _alarmParameter_12_2;
            set => SetProperty(ref _alarmParameter_12_2, value);
        }

        private string _alarmParameter_12_3;
        public string AlarmParameter_12_3
        {
            get => _alarmParameter_12_3;
            set => SetProperty(ref _alarmParameter_12_3, value);
        }

        private string _alarmParameter_12_4;
        public string AlarmParameter_12_4
        {
            get => _alarmParameter_12_4;
            set => SetProperty(ref _alarmParameter_12_4, value);
        }

        //private string _alarmParameter_12_5;
        //public string AlarmParameter_12_5
        //{
        //    get => _alarmParameter_12_5;
        //    set => SetProperty(ref _alarmParameter_12_5, value);
        //}

        private string _alarmParameter_13_1;
        public string AlarmParameter_13_1
        {
            get => _alarmParameter_13_1;
            set => SetProperty(ref _alarmParameter_13_1, value);
        }

        private string _alarmParameter_13_2;
        public string AlarmParameter_13_2
        {
            get => _alarmParameter_13_2;
            set => SetProperty(ref _alarmParameter_13_2, value);
        }

        private string _alarmParameter_13_3;
        public string AlarmParameter_13_3
        {
            get => _alarmParameter_13_3;
            set => SetProperty(ref _alarmParameter_13_3, value);
        }

        private string _alarmParameter_13_4;
        public string AlarmParameter_13_4
        {
            get => _alarmParameter_13_4;
            set => SetProperty(ref _alarmParameter_13_4, value);
        }

        private string _alarmParameter_14_1;
        public string AlarmParameter_14_1
        {
            get => _alarmParameter_14_1;
            set => SetProperty(ref _alarmParameter_14_1, value);
        }

        private string _alarmParameter_14_2;
        public string AlarmParameter_14_2
        {
            get => _alarmParameter_14_2;
            set => SetProperty(ref _alarmParameter_14_2, value);
        }

        private string _alarmParameter_15_1;
        public string AlarmParameter_15_1
        {
            get => _alarmParameter_15_1;
            set => SetProperty(ref _alarmParameter_15_1, value);
        }

        private string _alarmParameter_15_2;
        public string AlarmParameter_15_2
        {
            get => _alarmParameter_15_2;
            set => SetProperty(ref _alarmParameter_15_2, value);
        }

        private string _alarmParameter_15_3;
        public string AlarmParameter_15_3
        {
            get => _alarmParameter_15_3;
            set => SetProperty(ref _alarmParameter_15_3, value);
        }

        private string _alarmParameter_15_4;
        public string AlarmParameter_15_4
        {
            get => _alarmParameter_15_4;
            set => SetProperty(ref _alarmParameter_15_4, value);
        }

        private string _alarmParameter_15_5;
        public string AlarmParameter_15_5
        {
            get => _alarmParameter_15_5;
            set => SetProperty(ref _alarmParameter_15_5, value);
        }

        private string _alarmParameter_15_6;
        public string AlarmParameter_15_6
        {
            get => _alarmParameter_15_6;
            set => SetProperty(ref _alarmParameter_15_6, value);
        }

        private string _alarmParameter_15_7;
        public string AlarmParameter_15_7
        {
            get => _alarmParameter_15_7;
            set => SetProperty(ref _alarmParameter_15_7, value);
        }

        private string _alarmParameter_15_8;
        public string AlarmParameter_15_8
        {
            get => _alarmParameter_15_8;
            set => SetProperty(ref _alarmParameter_15_8, value);
        }

        private string _alarmParameter_16_1;
        public string AlarmParameter_16_1
        {
            get => _alarmParameter_16_1;
            set => SetProperty(ref _alarmParameter_16_1, value);
        }

        private string _alarmParameter_16_2;
        public string AlarmParameter_16_2
        {
            get => _alarmParameter_16_2;
            set => SetProperty(ref _alarmParameter_16_2, value);
        }

        private string _alarmParameter_16_3;
        public string AlarmParameter_16_3
        {
            get => _alarmParameter_16_3;
            set => SetProperty(ref _alarmParameter_16_3, value);
        }

        private string _alarmParameter_16_4;
        public string AlarmParameter_16_4
        {
            get => _alarmParameter_16_4;
            set => SetProperty(ref _alarmParameter_16_4, value);
        }

        private string _alarmParameter_16_5;
        public string AlarmParameter_16_5
        {
            get => _alarmParameter_16_5;
            set => SetProperty(ref _alarmParameter_16_5, value);
        }

        private string _alarmParameter_16_6;
        public string AlarmParameter_16_6
        {
            get => _alarmParameter_16_6;
            set => SetProperty(ref _alarmParameter_16_6, value);
        }

        private string _alarmParameter_17_1;
        public string AlarmParameter_17_1
        {
            get => _alarmParameter_17_1;
            set => SetProperty(ref _alarmParameter_17_1, value);
        }

        private string _alarmParameter_17_2;
        public string AlarmParameter_17_2
        {
            get => _alarmParameter_17_2;
            set => SetProperty(ref _alarmParameter_17_2, value);
        }

        private string _alarmParameter_17_3;
        public string AlarmParameter_17_3
        {
            get => _alarmParameter_17_3;
            set => SetProperty(ref _alarmParameter_17_3, value);
        }

        private string _alarmParameter_17_4;
        public string AlarmParameter_17_4
        {
            get => _alarmParameter_17_4;
            set => SetProperty(ref _alarmParameter_17_4, value);
        }

        private bool _isAllChecked;
        public bool IsAllChecked
        {
            get { return _isAllChecked; }
            set
            {
                _isAllChecked = value;
                if (value)
                {
                    IsChecked_AlarmParameter_1 = true;
                    IsChecked_AlarmParameter_2 = true;
                    IsChecked_AlarmParameter_3 = true;
                    IsChecked_AlarmParameter_4 = true;
                    IsChecked_AlarmParameter_5 = true;
                    IsChecked_AlarmParameter_6 = true;
                    IsChecked_AlarmParameter_7 = true;
                    IsChecked_AlarmParameter_8 = true;
                    IsChecked_AlarmParameter_9 = true;
                    IsChecked_AlarmParameter_10 = true;
                    IsChecked_AlarmParameter_11 = true;
                    IsChecked_AlarmParameter_12 = true;
                    IsChecked_AlarmParameter_13 = true;
                    IsChecked_AlarmParameter_14 = true;
                    IsChecked_AlarmParameter_15 = true;
                    IsChecked_AlarmParameter_16 = true;
                    IsChecked_AlarmParameter_17 = true;
                }
                else
                {
                    IsChecked_AlarmParameter_1 = false;
                    IsChecked_AlarmParameter_2 = false;
                    IsChecked_AlarmParameter_3 = false;
                    IsChecked_AlarmParameter_4 = false;
                    IsChecked_AlarmParameter_5 = false;
                    IsChecked_AlarmParameter_6 = false;
                    IsChecked_AlarmParameter_7 = false;
                    IsChecked_AlarmParameter_8 = false;
                    IsChecked_AlarmParameter_9 = false;
                    IsChecked_AlarmParameter_10 = false;
                    IsChecked_AlarmParameter_11 = false;
                    IsChecked_AlarmParameter_12 = false;
                    IsChecked_AlarmParameter_13 = false;
                    IsChecked_AlarmParameter_14 = false;
                    IsChecked_AlarmParameter_15 = false;
                    IsChecked_AlarmParameter_16 = false;
                    IsChecked_AlarmParameter_17 = false;

                }

                OnPropertyChanged();
            }
        }
        private bool _IsChecked_AlarmParameter_1;
        public bool IsChecked_AlarmParameter_1
        {
            get { return _IsChecked_AlarmParameter_1; }
            set
            {
                _IsChecked_AlarmParameter_1 = value;
                OnPropertyChanged(nameof(IsChecked_AlarmParameter_1));
            }
        }
        private bool _IsChecked_AlarmParameter_2;
        public bool IsChecked_AlarmParameter_2
        {
            get { return _IsChecked_AlarmParameter_2; }
            set
            {
                _IsChecked_AlarmParameter_2 = value;
                OnPropertyChanged(nameof(IsChecked_AlarmParameter_2));
            }
        }
        private bool _IsChecked_AlarmParameter_3;
        public bool IsChecked_AlarmParameter_3
        {
            get { return _IsChecked_AlarmParameter_3; }
            set
            {
                _IsChecked_AlarmParameter_3 = value;
                OnPropertyChanged(nameof(IsChecked_AlarmParameter_3));
            }
        }
        private bool _IsChecked_AlarmParameter_4;
        public bool IsChecked_AlarmParameter_4
        {
            get { return _IsChecked_AlarmParameter_4; }
            set
            {
                _IsChecked_AlarmParameter_4 = value;
                OnPropertyChanged(nameof(IsChecked_AlarmParameter_4));
            }
        }
        private bool _IsChecked_AlarmParameter_5;
        public bool IsChecked_AlarmParameter_5
        {
            get { return _IsChecked_AlarmParameter_5; }
            set
            {
                _IsChecked_AlarmParameter_5 = value;
                OnPropertyChanged(nameof(IsChecked_AlarmParameter_5));
            }
        }
        private bool _IsChecked_AlarmParameter_6;
        public bool IsChecked_AlarmParameter_6
        {
            get { return _IsChecked_AlarmParameter_6; }
            set
            {
                _IsChecked_AlarmParameter_6 = value;
                OnPropertyChanged(nameof(IsChecked_AlarmParameter_6));
            }
        }
        private bool _IsChecked_AlarmParameter_7;
        public bool IsChecked_AlarmParameter_7
        {
            get { return _IsChecked_AlarmParameter_7; }
            set
            {
                _IsChecked_AlarmParameter_7 = value;
                OnPropertyChanged(nameof(IsChecked_AlarmParameter_7));
            }
        }
        private bool _IsChecked_AlarmParameter_8;
        public bool IsChecked_AlarmParameter_8
        {
            get { return _IsChecked_AlarmParameter_8; }
            set
            {
                _IsChecked_AlarmParameter_8 = value;
                OnPropertyChanged(nameof(IsChecked_AlarmParameter_8));
            }
        }
        private bool _IsChecked_AlarmParameter_9;
        public bool IsChecked_AlarmParameter_9
        {
            get { return _IsChecked_AlarmParameter_9; }
            set
            {
                _IsChecked_AlarmParameter_9 = value;
                OnPropertyChanged(nameof(IsChecked_AlarmParameter_9));
            }
        }
        private bool _IsChecked_AlarmParameter_10;
        public bool IsChecked_AlarmParameter_10
        {
            get { return _IsChecked_AlarmParameter_10; }
            set
            {
                _IsChecked_AlarmParameter_10 = value;
                OnPropertyChanged(nameof(IsChecked_AlarmParameter_10));
            }
        }
        private bool _IsChecked_AlarmParameter_11;
        public bool IsChecked_AlarmParameter_11
        {
            get { return _IsChecked_AlarmParameter_11; }
            set
            {
                _IsChecked_AlarmParameter_11 = value;
                OnPropertyChanged(nameof(IsChecked_AlarmParameter_11));
            }
        }
        private bool _IsChecked_AlarmParameter_12;
        public bool IsChecked_AlarmParameter_12
        {
            get { return _IsChecked_AlarmParameter_12; }
            set
            {
                _IsChecked_AlarmParameter_12 = value;
                OnPropertyChanged(nameof(IsChecked_AlarmParameter_12));
            }
        }
        private bool _IsChecked_AlarmParameter_13;
        public bool IsChecked_AlarmParameter_13
        {
            get { return _IsChecked_AlarmParameter_13; }
            set
            {
                _IsChecked_AlarmParameter_13 = value;
                OnPropertyChanged(nameof(IsChecked_AlarmParameter_13));
            }
        }
        private bool _IsChecked_AlarmParameter_14;
        public bool IsChecked_AlarmParameter_14
        {
            get { return _IsChecked_AlarmParameter_14; }
            set
            {
                _IsChecked_AlarmParameter_14 = value;
                OnPropertyChanged(nameof(IsChecked_AlarmParameter_14));
            }
        }
        private bool _IsChecked_AlarmParameter_15;
        public bool IsChecked_AlarmParameter_15
        {
            get { return _IsChecked_AlarmParameter_15; }
            set
            {
                _IsChecked_AlarmParameter_15 = value;
                OnPropertyChanged(nameof(IsChecked_AlarmParameter_15));
            }
        }
        private bool _IsChecked_AlarmParameter_16;
        public bool IsChecked_AlarmParameter_16
        {
            get { return _IsChecked_AlarmParameter_16; }
            set
            {
                _IsChecked_AlarmParameter_16 = value;
                OnPropertyChanged(nameof(IsChecked_AlarmParameter_16));
            }
        }
        private bool _IsChecked_AlarmParameter_17;
        public bool IsChecked_AlarmParameter_17
        {
            get { return _IsChecked_AlarmParameter_17; }
            set
            {
                _IsChecked_AlarmParameter_17 = value;
                OnPropertyChanged(nameof(IsChecked_AlarmParameter_17));
            }
        }

        public ObservableCollection<ParamViewModel> Parameters { get; set; } = new ObservableCollection<ParamViewModel>();

        public BMU_ParamControl_ViewModel()
        {
            baseCanHelper = new CommandOperation(BMSConfig.ConfigManager).baseCanHelper;

            cts = new CancellationTokenSource();
            if (keys.Count == 0)
            {
                //从控报警参数，添加界面TextBox控件名称对应控件文本
                keys.Add("txtAlarmParameters_1_1", "单体过充保护(mV)");
                keys.Add("txtAlarmParameters_1_2", "单体过充解除(mV)");
                keys.Add("txtAlarmParameters_1_3", "单体过充告警(mV)");
                keys.Add("txtAlarmParameters_1_4", "单体过充解除(mV)");
                keys.Add("txtAlarmParameters_2_1", "总体过充保护(V)");
                keys.Add("txtAlarmParameters_2_2", "总体过充解除(V)");
                keys.Add("txtAlarmParameters_2_3", "总体过充告警(V)");
                keys.Add("txtAlarmParameters_2_4", "总体过充解除(V)");
                keys.Add("txtAlarmParameters_3_1", "单体过放保护(mV)");
                keys.Add("txtAlarmParameters_3_2", "单体过放解除(mV)");
                keys.Add("txtAlarmParameters_3_3", "单体过放告警(mV)");
                keys.Add("txtAlarmParameters_3_4", "单体过放解除(mV)");
                keys.Add("txtAlarmParameters_4_1", "总体过放保护(V)");
                keys.Add("txtAlarmParameters_4_2", "总体过放解除(V)");
                keys.Add("txtAlarmParameters_4_3", "总体过放告警(V)");
                keys.Add("txtAlarmParameters_4_4", "总体过放解除(V)");
                keys.Add("txtAlarmParameters_5_1", "充电过流保护(A)");
                keys.Add("txtAlarmParameters_5_2", "充电过流解除(A)");
                keys.Add("txtAlarmParameters_5_3", "充电过流告警(A)");
                keys.Add("txtAlarmParameters_5_4", "充电过流解除(A)");
                keys.Add("txtAlarmParameters_6_1", "放电过流保护(A)");
                keys.Add("txtAlarmParameters_6_2", "放电过流解除(A)");
                keys.Add("txtAlarmParameters_6_3", "放电过流告警(A)");
                keys.Add("txtAlarmParameters_6_4", "放电过流解除(A)");
                keys.Add("txtAlarmParameters_7_1", "充电高温保护(℃)");
                keys.Add("txtAlarmParameters_7_2", "充电高温解除(℃)");
                keys.Add("txtAlarmParameters_7_3", "充电高温告警(℃)");
                keys.Add("txtAlarmParameters_7_4", "充电高温解除(℃)");
                keys.Add("txtAlarmParameters_7_5", "放电高温保护(℃)");
                keys.Add("txtAlarmParameters_7_6", "放电高温解除(℃)");
                keys.Add("txtAlarmParameters_7_7", "放电高温告警(℃)");
                keys.Add("txtAlarmParameters_7_8", "放电高温解除(℃)");
                keys.Add("txtAlarmParameters_8_1", "充电低温保护(℃)");
                keys.Add("txtAlarmParameters_8_2", "充电低温解除(℃)");
                keys.Add("txtAlarmParameters_8_3", "充电低温告警(℃)");
                keys.Add("txtAlarmParameters_8_4", "充电低温解除(℃)");
                keys.Add("txtAlarmParameters_8_5", "放电低温保护(℃)");
                keys.Add("txtAlarmParameters_8_6", "放电低温解除(℃)");
                keys.Add("txtAlarmParameters_8_7", "放电低温告警(℃)");
                keys.Add("txtAlarmParameters_8_8", "放电低温解除(℃)");
                keys.Add("txtAlarmParameters_9_1", "充电过流二级保护(A)");
                keys.Add("txtAlarmParameters_9_2", "充电过流二级保护解除(A)");
                keys.Add("txtAlarmParameters_9_3", "放电过流二级保护(A)");
                keys.Add("txtAlarmParameters_9_4", "放电过流二级保护解除(A)");
                keys.Add("txtAlarmParameters_10_1", "单体超限产生阈值(mV)");
                keys.Add("txtAlarmParameters_10_2", "单体超限解除阈值(mV)");
                keys.Add("txtAlarmParameters_10_3", "单体超低产生阈值(mV)");
                keys.Add("txtAlarmParameters_10_4", "单体超低解除阈值(mV)");
                keys.Add("txtAlarmParameters_11_1", "低电量保护(%)");
                keys.Add("txtAlarmParameters_11_2", "低电量解除(%)");
                keys.Add("txtAlarmParameters_11_3", "低电量告警(%)");
                keys.Add("txtAlarmParameters_11_4", "低电量解除(%)");
                keys.Add("txtAlarmParameters_12_1", "单体过充提示(mV)");
                keys.Add("txtAlarmParameters_12_2", "单体过充提示解除(mV)");
                keys.Add("txtAlarmParameters_12_3", "单体过放提示(mV)");
                keys.Add("txtAlarmParameters_12_4", "单体过放提示解除(mV)");

                keys.Add("txtAlarmParameters_13_1", "单体压差过大提示(mV)");
                keys.Add("txtAlarmParameters_13_2", "单体压差过大提示解除(mV)");
                keys.Add("txtAlarmParameters_13_3", "单体压差过大告警(mV)");
                keys.Add("txtAlarmParameters_13_4", "单体压差过大告警解除(mV)");

                keys.Add("txtAlarmParameters_14_1", "单体压差过大故障(mV)");
                keys.Add("txtAlarmParameters_14_2", "单体压差过大故障解除(mV)");

                keys.Add("txtAlarmParameters_15_1", "充电高温提示(℃)");
                keys.Add("txtAlarmParameters_15_2", "充电高温提示解除(℃)");
                keys.Add("txtAlarmParameters_15_3", "充电低温提示(℃)");
                keys.Add("txtAlarmParameters_15_4", "充电低温提示解除(℃)");
                keys.Add("txtAlarmParameters_15_5", "放电高温提示(℃)");
                keys.Add("txtAlarmParameters_15_6", "放电高温提示解除(℃)");
                keys.Add("txtAlarmParameters_15_7", "放电低温提示(℃)");
                keys.Add("txtAlarmParameters_15_8", "放电低温提示解除(℃)");

                keys.Add("txtAlarmParameters_16_1", "电芯温差过大提示(℃)");
                keys.Add("txtAlarmParameters_16_2", "电芯温差过大提示解除(℃)");
                keys.Add("txtAlarmParameters_16_3", "电芯温差过大告警(℃)");
                keys.Add("txtAlarmParameters_16_4", "电芯温差过大告警解除(℃)");
                keys.Add("txtAlarmParameters_16_5", "电芯温差过大保护(℃)");
                keys.Add("txtAlarmParameters_16_6", "电芯温差过大保护解除(℃)");

                keys.Add("txtAlarmParameters_17_1", "总体过充提示(V)");
                keys.Add("txtAlarmParameters_17_2", "总体过充提示解除(V)");
                keys.Add("txtAlarmParameters_17_3", "总体过放提示(V)");
                keys.Add("txtAlarmParameters_17_4", "总体过放提示解除(V)");
            }
        }
        public static bool IsShowMessage = true;
        public void Load()
        {
            if (baseCanHelper.IsConnection)
            {
                IsShowMessage = true;
                byte[] bytes = new byte[8] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                byte Address_BMU = Convert.ToByte(SelectedAddress_BMU);
                // 17个故障报警参数

                //0x01:读取所有参数
                //0x02:恢复所有默认设置
                //0x03:故障参数标定回复0x1010E0XX ✅️
                //0x04:故障参数标定回复0x1011E0XX ✅️
                //0x05:故障参数标定回复0x1012E0XX ✅️
                //0x06:故障参数标定回复0x1013E0XX ✅️
                //0x07:故障参数标定回复0x1014E0XX ✅️
                //0x08:故障参数标定回复0x1015E0XX ✅️
                //0x09:故障参数标定回复0x1016E0XX ✅️
                //0x0A:故障参数标定回复0x1017E0XX ✅️
                //0x0B:故障参数标定回复0x1018E0XX 
                //0x0C:故障参数标定回复0x1019E0XX
                //0x0D:故障参数标定回复0x101AE0XX ✅️
                //0x0E:参数标定回复0x1021E0XX
                //0x0F:参数标定回复0x1022E0XX
                //0x10:参数标定回复0x1023E0XX
                //0x11:参数标定回复0x1024E0XX
                //0x12:参数标定回复0x1025E0XX
                //0x13:时间标定回复0x1026E0XX
                //0x14:PACK_SN标定回复0x1027E0XX
                //0x15:BOARD_SN标定回复0x1028E0XX
                //0x16:一键读取标定参数回复0x102EE0XX
                //0x17:电芯品牌回复0x1029E0XX
                //0x18: 0x01B 回复充放电二级故障参数标定   ✅️
                //0x19: 0x01C 回复单体超限超低故障参数标定 ✅️
                //0x1A: 0x01F 回复ATE_FLASH测试
                //0x1B: 0x020 回复上位机控制老化状态
                //0x1C: 0x02B 回复BMU功能开关
                //0x1D: 0x02F 回复ATE强制控制指令
                //0x1E: 0x01E 回复ATE强制控制指令2
                //0x1F: 0x080 回复单体电压过充、过放提示标定      ✅️
                //0x20: 0x081 回复单体电压压差过大提示、告警标定  ✅️
                //0x21: 0x082 回复单体电压压差过大故障标定        ✅️
                //0x22: 0x083 回复电芯温度充放电高低温提示标定    ✅️
                //0x23: 0x084 回复单体电压过充、过放提示标定      ✅️
                //0x24: 0x085 回复总压过压欠压提示标定            ✅️
                baseCanHelper.Send(bytes, new byte[] { 0xE0, Address_BMU, 0x2E, 0x10 });
            }

            Task.Run(delegate
            {
                while (!cts.IsCancellationRequested)
                {
                    if (baseCanHelper.CommunicationType == "Ecan")
                    {
                        lock (EcanHelper._locker)
                        {
                            while (EcanHelper._task.Count > 0
                                && !cts.IsCancellationRequested)
                            {
                                CAN_OBJ ch = (CAN_OBJ)EcanHelper._task.Dequeue();
                                Application.Current.Dispatcher.Invoke(() => { analysisData(ch.ID, ch.Data); });
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
                                Application.Current.Dispatcher.Invoke(() => { analysisData(ch.ID, ch.Data); });
                            }
                        }
                    }


                }
            }, cts.Token);
        }
        public void analysisData(uint canID, byte[] data)
        {
            byte Address_BMU = Convert.ToByte(SelectedAddress_BMU);

            byte[] canid = BitConverter.GetBytes(canID);
            if (canid[0] != Address_BMU || !(canid[0] == Address_BMU && (canid[1] == 0xE0 || canid[1] == 0xFF) && canid[3] == 0x10)) return;
            int[] numbers = BytesToUint16(data);
            int[] numbers_bit = BytesToBit(data);
            Parameters.Clear();
            Parameters.Add(new ParamViewModel { ControlName = "单体过充保护(mV)", Value = AlarmParameter_1_1 });
            Parameters.Add(new ParamViewModel { ControlName = "单体过充解除(mV)", Value = AlarmParameter_1_2 });
            Parameters.Add(new ParamViewModel { ControlName = "单体过充告警(mV)", Value = AlarmParameter_1_3 });
            Parameters.Add(new ParamViewModel { ControlName = "单体过充解除(mV)", Value = AlarmParameter_1_4 });
            Parameters.Add(new ParamViewModel { ControlName = "总体过充保护(V)", Value = AlarmParameter_2_1 });
            Parameters.Add(new ParamViewModel { ControlName = "总体过充解除(V)", Value = AlarmParameter_2_2 });
            Parameters.Add(new ParamViewModel { ControlName = "总体过充告警(V)", Value = AlarmParameter_2_3 });
            Parameters.Add(new ParamViewModel { ControlName = "总体过充解除(V)", Value = AlarmParameter_2_4 });
            Parameters.Add(new ParamViewModel { ControlName = "单体过放保护(mV)", Value = AlarmParameter_3_1 });
            Parameters.Add(new ParamViewModel { ControlName = "单体过放解除(mV)", Value = AlarmParameter_3_2 });
            Parameters.Add(new ParamViewModel { ControlName = "单体过放告警(mV)", Value = AlarmParameter_3_3 });
            Parameters.Add(new ParamViewModel { ControlName = "单体过放解除(mV)", Value = AlarmParameter_3_4 });
            Parameters.Add(new ParamViewModel { ControlName = "总体过放保护(V)", Value = AlarmParameter_4_1 });
            Parameters.Add(new ParamViewModel { ControlName = "总体过放解除(V)", Value = AlarmParameter_4_2 });
            Parameters.Add(new ParamViewModel { ControlName = "总体过放告警(V)", Value = AlarmParameter_4_3 });
            Parameters.Add(new ParamViewModel { ControlName = "总体过放解除(V)", Value = AlarmParameter_4_4 });
            Parameters.Add(new ParamViewModel { ControlName = "充电过流保护(A)", Value = AlarmParameter_5_1 });
            Parameters.Add(new ParamViewModel { ControlName = "充电过流解除(A)", Value = AlarmParameter_5_2 });
            Parameters.Add(new ParamViewModel { ControlName = "充电过流告警(A)", Value = AlarmParameter_5_3 });
            Parameters.Add(new ParamViewModel { ControlName = "充电过流解除(A)", Value = AlarmParameter_5_4 });
            Parameters.Add(new ParamViewModel { ControlName = "放电过流保护(A)", Value = AlarmParameter_6_1 });
            Parameters.Add(new ParamViewModel { ControlName = "放电过流解除(A)", Value = AlarmParameter_6_2 });
            Parameters.Add(new ParamViewModel { ControlName = "放电过流告警(A)", Value = AlarmParameter_6_3 });
            Parameters.Add(new ParamViewModel { ControlName = "放电过流解除(A)", Value = AlarmParameter_6_4 });
            Parameters.Add(new ParamViewModel { ControlName = "充电高温保护(℃)", Value = AlarmParameter_7_1 });
            Parameters.Add(new ParamViewModel { ControlName = "充电高温解除(℃)", Value = AlarmParameter_7_2 });
            Parameters.Add(new ParamViewModel { ControlName = "充电高温告警(℃)", Value = AlarmParameter_7_3 });
            Parameters.Add(new ParamViewModel { ControlName = "充电高温解除(℃)", Value = AlarmParameter_7_4 });
            Parameters.Add(new ParamViewModel { ControlName = "放电高温保护(℃)", Value = AlarmParameter_7_5 });
            Parameters.Add(new ParamViewModel { ControlName = "放电高温解除(℃)", Value = AlarmParameter_7_6 });
            Parameters.Add(new ParamViewModel { ControlName = "放电高温告警(℃)", Value = AlarmParameter_7_7 });
            Parameters.Add(new ParamViewModel { ControlName = "放电高温解除(℃)", Value = AlarmParameter_7_8 });
            Parameters.Add(new ParamViewModel { ControlName = "充电低温保护(℃)", Value = AlarmParameter_8_1 });
            Parameters.Add(new ParamViewModel { ControlName = "充电低温解除(℃)", Value = AlarmParameter_8_2 });
            Parameters.Add(new ParamViewModel { ControlName = "充电低温告警(℃)", Value = AlarmParameter_8_3 });
            Parameters.Add(new ParamViewModel { ControlName = "充电低温解除(℃)", Value = AlarmParameter_8_4 });
            Parameters.Add(new ParamViewModel { ControlName = "放电低温保护(℃)", Value = AlarmParameter_8_5 });
            Parameters.Add(new ParamViewModel { ControlName = "放电低温解除(℃)", Value = AlarmParameter_8_6 });
            Parameters.Add(new ParamViewModel { ControlName = "放电低温告警(℃)", Value = AlarmParameter_8_7 });
            Parameters.Add(new ParamViewModel { ControlName = "放电低温解除(℃)", Value = AlarmParameter_8_8 });
            Parameters.Add(new ParamViewModel { ControlName = "充电过流二级保护(A)", Value = AlarmParameter_9_1 });
            Parameters.Add(new ParamViewModel { ControlName = "充电过流二级保护解除(A)", Value = AlarmParameter_9_2 });
            Parameters.Add(new ParamViewModel { ControlName = "放电过流二级保护(A)", Value = AlarmParameter_9_3 });
            Parameters.Add(new ParamViewModel { ControlName = "放电过流二级保护解除(A)", Value = AlarmParameter_10_1 });
            Parameters.Add(new ParamViewModel { ControlName = "单体超限产生阈值(mV)", Value = AlarmParameter_10_2 });
            Parameters.Add(new ParamViewModel { ControlName = "单体超限解除阈值(mV)", Value = AlarmParameter_10_3 });
            Parameters.Add(new ParamViewModel { ControlName = "单体超低产生阈值(mV)", Value = AlarmParameter_10_4 });
            Parameters.Add(new ParamViewModel { ControlName = "单体超低解除阈值(mV)", Value = AlarmParameter_10_1 });
            Parameters.Add(new ParamViewModel { ControlName = "低电量保护(%)", Value = AlarmParameter_11_1 });
            Parameters.Add(new ParamViewModel { ControlName = "低电量解除(%)", Value = AlarmParameter_11_2 });
            Parameters.Add(new ParamViewModel { ControlName = "低电量告警(%)", Value = AlarmParameter_11_3 });
            Parameters.Add(new ParamViewModel { ControlName = "低电量解除(%)", Value = AlarmParameter_11_4 });
            Parameters.Add(new ParamViewModel { ControlName = "单体过充提示(mV)", Value = AlarmParameter_12_1 });
            Parameters.Add(new ParamViewModel { ControlName = "单体过充提示解除(mV)", Value = AlarmParameter_12_2 });
            Parameters.Add(new ParamViewModel { ControlName = "单体过放提示(mV)", Value = AlarmParameter_12_3 });
            Parameters.Add(new ParamViewModel { ControlName = "单体过放提示解除(mV)", Value = AlarmParameter_12_4 });
            Parameters.Add(new ParamViewModel { ControlName = "单体压差过大提示(mV)", Value = AlarmParameter_13_1 });
            Parameters.Add(new ParamViewModel { ControlName = "单体压差过大提示解除(mV)", Value = AlarmParameter_13_2 });
            Parameters.Add(new ParamViewModel { ControlName = "单体压差过大告警(mV)", Value = AlarmParameter_13_3 });
            Parameters.Add(new ParamViewModel { ControlName = "单体压差过大告警解除(mV)", Value = AlarmParameter_13_4 });
            Parameters.Add(new ParamViewModel { ControlName = "单体压差过大故障(mV)", Value = AlarmParameter_14_1 });
            Parameters.Add(new ParamViewModel { ControlName = "单体压差过大故障解除(mV)", Value = AlarmParameter_14_2 });
            Parameters.Add(new ParamViewModel { ControlName = "充电高温提示(℃)", Value = AlarmParameter_15_1 });
            Parameters.Add(new ParamViewModel { ControlName = "充电高温提示解除(℃)", Value = AlarmParameter_15_2 });
            Parameters.Add(new ParamViewModel { ControlName = "充电低温提示(℃)", Value = AlarmParameter_15_3 });
            Parameters.Add(new ParamViewModel { ControlName = "充电低温提示解除(℃)", Value = AlarmParameter_15_4 });
            Parameters.Add(new ParamViewModel { ControlName = "放电高温提示(℃)", Value = AlarmParameter_15_5 });
            Parameters.Add(new ParamViewModel { ControlName = "放电高温提示解除(℃)", Value = AlarmParameter_15_6 });
            Parameters.Add(new ParamViewModel { ControlName = "放电低温提示(℃)", Value = AlarmParameter_15_7 });
            Parameters.Add(new ParamViewModel { ControlName = "放电低温提示解除(℃)", Value = AlarmParameter_15_8 });
            Parameters.Add(new ParamViewModel { ControlName = "电芯温差过大提示(℃)", Value = AlarmParameter_16_1 });
            Parameters.Add(new ParamViewModel { ControlName = "电芯温差过大提示解除(℃)", Value = AlarmParameter_16_2 });
            Parameters.Add(new ParamViewModel { ControlName = "电芯温差过大告警(℃)", Value = AlarmParameter_16_3 });
            Parameters.Add(new ParamViewModel { ControlName = "电芯温差过大告警解除(℃)", Value = AlarmParameter_16_4 });
            Parameters.Add(new ParamViewModel { ControlName = "电芯温差过大保护(℃)", Value = AlarmParameter_16_5 });
            Parameters.Add(new ParamViewModel { ControlName = "电芯温差过大保护解除(℃)", Value = AlarmParameter_16_6 });
            Parameters.Add(new ParamViewModel { ControlName = "总体过充提示(V)", Value = AlarmParameter_17_1 });
            Parameters.Add(new ParamViewModel { ControlName = "总体过充提示解除(V)", Value = AlarmParameter_17_2 });
            Parameters.Add(new ParamViewModel { ControlName = "总体过放提示(V)", Value = AlarmParameter_17_3 });
            Parameters.Add(new ParamViewModel { ControlName = "总体过放提示解除(V)", Value = AlarmParameter_17_4 });

            switch (canid[2])
            {
                case 0x2E:
                    //BMS软件版本
                    string[] bsm_soft = new string[3];
                    for (int i = 0; i < 3; i++)
                    {
                        bsm_soft[i] = data[i + 2].ToString().PadLeft(2, '0');
                    }
                    SoftwareVersion = Encoding.ASCII.GetString(new byte[] { data[1] }) + string.Join("", bsm_soft);

                    //BMS硬件版本
                    string[] bsm_HW = new string[2];
                    for (int i = 0; i < 2; i++)
                    {
                        bsm_HW[i] = data[i + 5].ToString().PadLeft(2, '0');
                    }
                    HardwareVersion = string.Join("", bsm_HW);
                    if (IsShowMessage)
                    {
                        IsShowMessage = false; //防止重复提示
                        MessageBoxHelper.Success("读取成功", "提示", null, ButtonType.OK);
                    }

                    break;
                case 0x10://单体过充                      
                    AlarmParameter_1_1 = numbers[0].ToString();
                    AlarmParameter_1_2 = numbers[1].ToString();
                    AlarmParameter_1_3 = numbers[2].ToString();
                    AlarmParameter_1_4 = numbers[3].ToString();
                    break;
                case 0x11://总体过充                                                                                      
                    AlarmParameter_2_1 = (numbers[0] * 0.1).ToString("F1");
                    AlarmParameter_2_2 = (numbers[1] * 0.1).ToString("F1");
                    AlarmParameter_2_3 = (numbers[2] * 0.1).ToString("F1");
                    AlarmParameter_2_4 = (numbers[3] * 0.1).ToString("F1");
                    break;
                case 0x12://单体过放                                                                                  
                    AlarmParameter_3_1 = numbers[0].ToString();
                    AlarmParameter_3_2 = numbers[1].ToString();
                    AlarmParameter_3_3 = numbers[2].ToString();
                    AlarmParameter_3_4 = numbers[3].ToString();
                    break;
                case 0x13://总体过放                                                                                    
                    AlarmParameter_4_1 = (numbers[0] * 0.1).ToString();
                    AlarmParameter_4_2 = (numbers[1] * 0.1).ToString();
                    AlarmParameter_4_3 = (numbers[2] * 0.1).ToString();
                    AlarmParameter_4_4 = (numbers[3] * 0.1).ToString();
                    break;
                case 0x14://充电过流                                                                                
                    AlarmParameter_5_1 = (numbers[0] * 0.01).ToString();
                    AlarmParameter_5_2 = (numbers[1] * 0.01).ToString();
                    AlarmParameter_5_3 = (numbers[2] * 0.01).ToString();
                    AlarmParameter_5_4 = (numbers[3] * 0.01).ToString();
                    break;
                case 0x15://放电过流                                                                             
                    AlarmParameter_6_1 = (numbers[0] * 0.01).ToString();
                    AlarmParameter_6_2 = (numbers[1] * 0.01).ToString();
                    AlarmParameter_6_3 = (numbers[2] * 0.01).ToString();
                    AlarmParameter_6_4 = (numbers[3] * 0.01).ToString();
                    break;
                case 0x16://充电、放电高温                                                                        
                    AlarmParameter_7_1 = (numbers_bit[0] - 40).ToString();
                    AlarmParameter_7_2 = (numbers_bit[1] - 40).ToString();
                    AlarmParameter_7_3 = (numbers_bit[2] - 40).ToString();
                    AlarmParameter_7_4 = (numbers_bit[3] - 40).ToString();
                    AlarmParameter_7_5 = (numbers_bit[4] - 40).ToString();
                    AlarmParameter_7_6 = (numbers_bit[5] - 40).ToString();
                    AlarmParameter_7_7 = (numbers_bit[6] - 40).ToString();
                    AlarmParameter_7_8 = (numbers_bit[7] - 40).ToString();
                    break;
                case 0x17://充电、放电低温                                                                        
                    AlarmParameter_8_1 = (numbers_bit[0] - 40).ToString();
                    AlarmParameter_8_2 = (numbers_bit[1] - 40).ToString();
                    AlarmParameter_8_3 = (numbers_bit[2] - 40).ToString();
                    AlarmParameter_8_4 = (numbers_bit[3] - 40).ToString();
                    AlarmParameter_8_5 = (numbers_bit[4] - 40).ToString();
                    AlarmParameter_8_6 = (numbers_bit[5] - 40).ToString();
                    AlarmParameter_8_7 = (numbers_bit[6] - 40).ToString();
                    AlarmParameter_8_8 = (numbers_bit[7] - 40).ToString();
                    break;
                case 0x1B://充放电过流                                                                             
                    AlarmParameter_9_1 = (numbers[0] * 0.01).ToString();
                    AlarmParameter_9_2 = (numbers[1] * 0.01).ToString();
                    AlarmParameter_9_3 = (numbers[2] * 0.01).ToString();
                    AlarmParameter_9_4 = (numbers[3] * 0.01).ToString();
                    break;
                case 0x1C://单体超限、超低
                    AlarmParameter_10_1 = numbers[0].ToString();
                    AlarmParameter_10_2 = numbers[1].ToString();
                    AlarmParameter_10_3 = numbers[2].ToString();
                    AlarmParameter_10_4 = numbers[3].ToString();
                    break;
                case 0x19://低电量
                    AlarmParameter_11_1 = numbers[0].ToString();
                    AlarmParameter_11_2 = numbers[1].ToString();
                    AlarmParameter_11_3 = numbers[2].ToString();
                    AlarmParameter_11_4 = numbers[3].ToString();
                    break;
                //case 0x21://混合（均衡-满充电压-加热膜）
                //    AlarmParameter_12_1 = numbers[0].ToString();
                //    AlarmParameter_12_2 = numbers[1].ToString();
                //    AlarmParameter_12_3 = numbers[2].ToString();
                //    AlarmParameter_12_4 = (numbers_bit[6] - 40).ToString();
                //    AlarmParameter_12_5 = (numbers_bit[7] - 40).ToString();
                //    break;
                //case 0x22://电池包
                //    AlarmParameter_13_1 = (numbers[0] * 0.1).ToString();
                //    AlarmParameter_13_2 = (numbers[1] * 0.01).ToString();
                //    break;
                //单体过充、过放提示
                case 0x80:
                    AlarmParameter_12_1 = numbers[0].ToString();
                    AlarmParameter_12_2 = numbers[1].ToString();
                    AlarmParameter_12_3 = numbers[2].ToString();
                    AlarmParameter_12_4 = numbers[3].ToString();
                    break;
                //单体压差过大提示、告警
                case 0x81:
                    AlarmParameter_13_1 = numbers[0].ToString();
                    AlarmParameter_13_2 = numbers[1].ToString();
                    AlarmParameter_13_3 = numbers[2].ToString();
                    AlarmParameter_13_4 = numbers[3].ToString();
                    break;
                //单体压差过大故障
                case 0x82:
                    AlarmParameter_14_1 = numbers[0].ToString();
                    AlarmParameter_14_2 = numbers[1].ToString();
                    break;
                //充放电高低温提示
                case 0x83:
                    AlarmParameter_15_1 = (numbers_bit[0] - 40).ToString();
                    AlarmParameter_15_2 = (numbers_bit[1] - 40).ToString();
                    AlarmParameter_15_3 = (numbers_bit[2] - 40).ToString();
                    AlarmParameter_15_4 = (numbers_bit[3] - 40).ToString();
                    AlarmParameter_15_5 = (numbers_bit[4] - 40).ToString();
                    AlarmParameter_15_6 = (numbers_bit[5] - 40).ToString();
                    AlarmParameter_15_7 = (numbers_bit[6] - 40).ToString();
                    AlarmParameter_15_8 = (numbers_bit[7] - 40).ToString();
                    break;
                //电芯温差过大
                case 0x84:
                    AlarmParameter_16_1 = numbers_bit[0].ToString();//Offset：-40;修改为0，无偏差
                    AlarmParameter_16_2 = numbers_bit[1].ToString();//Offset：-40;修改为0，无偏差
                    AlarmParameter_16_3 = numbers_bit[2].ToString();//Offset：-40;修改为0，无偏差
                    AlarmParameter_16_4 = numbers_bit[3].ToString();//Offset：-40;修改为0，无偏差
                    AlarmParameter_16_5 = numbers_bit[4].ToString();//Offset：-40;修改为0，无偏差
                    AlarmParameter_16_6 = numbers_bit[5].ToString();//Offset：-40;修改为0，无偏差                
                    break;
                //总体过充、过放提示
                case 0x85:
                    AlarmParameter_17_1 = (numbers[0] * 0.1).ToString();
                    AlarmParameter_17_2 = (numbers[1] * 0.1).ToString();
                    AlarmParameter_17_3 = (numbers[2] * 0.1).ToString();
                    AlarmParameter_17_4 = (numbers[3] * 0.1).ToString();
                    break;
            }
        }


        public ICommand InitCmd => new RelayCommand<Window>(Init);
        public ICommand ReadCmd => new RelayCommand<Window>(Read);
        public ICommand WriteCmd => new RelayCommand<Window>(Write);
        public ICommand ImportCmd => new RelayCommand<Window>(Import);
        public ICommand ExportCmd => new RelayCommand<Window>(Export);

        /// <summary>
        /// 恢复主控报警参数默认值
        /// </summary>
        /// <param name="window"></param>
        public void Init(Window window)
        {

            var xmlPath = AppDomain.CurrentDomain.BaseDirectory + "Configuration\\BMU从控参数默认值.xml";
            var bReturn = LoadXmlParam(xmlPath);
            if (bReturn)
            {
                MessageBoxHelper.Info("界面参数已恢复，需下设后生效", "提示", null, ButtonType.OK);
                return;
            }
            //byte[] bytes = new byte[8] { 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            //byte Address_BMU = Convert.ToByte(SelectedAddress_BMU);
            //baseCanHelper.Send(bytes, new byte[] { 0xE0, Address_BMU, 0x2E, 0x10 });

            //Thread.Sleep(1000);
            //baseCanHelper.Send(new byte[8] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, new byte[] { 0xE0, Address_BMU, 0x2E, 0x10 });
        }

        /// <summary>
        /// 读取从控报警参数
        /// </summary>
        public void Read(Window window)
        {
            if (!baseCanHelper.IsConnection)
            {
                MessageBoxHelper.Info("请先打开CAN口!", "提示", null, ButtonType.OK);
                return;
            }

            byte[] bytes = new byte[8] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte Address_BMU = Convert.ToByte(SelectedAddress_BMU);
            
            IsShowMessage = true;
            if (!baseCanHelper.Send(bytes, new byte[] { 0xE0, Address_BMU, 0x2E, 0x10 }))
            {
                MessageBoxHelper.Success("读取失败", "提示", null, ButtonType.OK);
            }
        }

        /// <summary>
        /// 下设主控报警参数   
        /// </summary>
        /// <param name="window"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Write(Window window)
        {
            if (!baseCanHelper.IsConnection)
            {
                MessageBoxHelper.Info("请先打开CAN口!", "提示", null, ButtonType.OK);
                return;
            }

            Dictionary<string, bool> sendResult = new Dictionary<string, bool>();
            byte Address_BMU = Convert.ToByte(SelectedAddress_BMU);
            byte[] canid = new byte[] { 0xE0, Address_BMU, 0x00, 0x10 };
            byte[] bytes = new byte[8];


            // 存储多个bytes和canid
            List<byte[]> byteList = new List<byte[]>();
            List<byte[]> canidList = new List<byte[]>();

            if (IsChecked_AlarmParameter_1)
            {
                if (AlarmParameter_1_1 == null || AlarmParameter_1_2 == null || AlarmParameter_1_3 == null || AlarmParameter_1_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x10;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes(AlarmParameter_1_1, AlarmParameter_1_2, AlarmParameter_1_3, AlarmParameter_1_4, 1, 1, 1, 1));
            }

            if (IsChecked_AlarmParameter_2)
            {
                if (AlarmParameter_2_1 == null || AlarmParameter_2_2 == null || AlarmParameter_2_3 == null || AlarmParameter_2_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x11;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes(AlarmParameter_2_1, AlarmParameter_2_2, AlarmParameter_2_3, AlarmParameter_2_4, 0.1, 0.1, 0.1, 0.1));
            }

            if (IsChecked_AlarmParameter_3)
            {
                if (AlarmParameter_3_1 == null || AlarmParameter_3_2 == null || AlarmParameter_3_3 == null || AlarmParameter_3_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x12;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes(AlarmParameter_3_1, AlarmParameter_3_2, AlarmParameter_3_3, AlarmParameter_3_4, 1, 1, 1, 1));
            }

            if (IsChecked_AlarmParameter_4)
            {
                if (AlarmParameter_4_1 == null || AlarmParameter_4_2 == null || AlarmParameter_4_3 == null || AlarmParameter_4_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x13;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes(AlarmParameter_4_1, AlarmParameter_4_2, AlarmParameter_4_3, AlarmParameter_4_4, 0.1, 0.1, 0.1, 0.1));
            }

            if (IsChecked_AlarmParameter_5)
            {
                if (AlarmParameter_5_1 == null || AlarmParameter_5_2 == null || AlarmParameter_5_3 == null || AlarmParameter_5_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x14;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes(AlarmParameter_5_1, AlarmParameter_5_2, AlarmParameter_5_3, AlarmParameter_5_4, 0.01, 0.01, 0.01, 0.01));
            }

            if (IsChecked_AlarmParameter_6)
            {
                if (AlarmParameter_6_1 == null || AlarmParameter_6_2 == null || AlarmParameter_6_3 == null || AlarmParameter_6_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x15;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes(AlarmParameter_6_1, AlarmParameter_6_2, AlarmParameter_6_3, AlarmParameter_6_4, 0.01, 0.01, 0.01, 0.01));
            }

            if (IsChecked_AlarmParameter_7)
            {
                if (AlarmParameter_7_1 == null || AlarmParameter_7_2 == null || AlarmParameter_7_3 == null || AlarmParameter_7_4 == null ||
                    AlarmParameter_7_5 == null || AlarmParameter_7_6 == null || AlarmParameter_7_7 == null || AlarmParameter_7_8 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x16;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes_8_Offset(AlarmParameter_7_1, AlarmParameter_7_2, AlarmParameter_7_3, AlarmParameter_7_4, AlarmParameter_7_5, AlarmParameter_7_6, AlarmParameter_7_7, AlarmParameter_7_8, 1, 1, 1, 1, 1, 1, 1, 1, -40, -40, -40, -40, -40, -40, -40, -40));
            }

            if (IsChecked_AlarmParameter_8)
            {
                if (AlarmParameter_8_1 == null || AlarmParameter_8_2 == null || AlarmParameter_8_3 == null || AlarmParameter_8_4 == null ||
                    AlarmParameter_8_5 == null || AlarmParameter_8_6 == null || AlarmParameter_8_7 == null || AlarmParameter_8_8 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x17;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes_8_Offset(AlarmParameter_8_1, AlarmParameter_8_2, AlarmParameter_8_3, AlarmParameter_8_4, AlarmParameter_8_5, AlarmParameter_8_6, AlarmParameter_8_7, AlarmParameter_8_8, 1, 1, 1, 1, 1, 1, 1, 1, -40, -40, -40, -40, -40, -40, -40, -40));
            }

            if (IsChecked_AlarmParameter_9)
            {
                if (AlarmParameter_9_1 == null || AlarmParameter_9_2 == null || AlarmParameter_9_3 == null || AlarmParameter_9_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x1B;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes(AlarmParameter_9_1, AlarmParameter_9_2, AlarmParameter_9_3, AlarmParameter_9_4, 0.01, 0.01, 0.01, 0.01));
            }

            if (IsChecked_AlarmParameter_10)
            {
                if (AlarmParameter_10_1 == null || AlarmParameter_10_2 == null || AlarmParameter_10_3 == null || AlarmParameter_10_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x1C;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes(AlarmParameter_10_1, AlarmParameter_10_2, AlarmParameter_10_3, AlarmParameter_10_4, 1, 1, 1, 1));
            }

            if (IsChecked_AlarmParameter_11)
            {
                if (AlarmParameter_11_1 == null || AlarmParameter_11_2 == null || AlarmParameter_11_3 == null || AlarmParameter_11_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x19;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes(AlarmParameter_11_1, AlarmParameter_11_2, AlarmParameter_11_3, AlarmParameter_11_4, 1, 1, 1, 1));
            }

            //if (IsChecked_AlarmParameter_12)
            //{
            //    if (AlarmParameter_12_1 == null || AlarmParameter_12_2 == null || AlarmParameter_12_3 == null || AlarmParameter_12_4 == null)
            //    {
            //        MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
            //        return;
            //    }
            //    var clonedCanid = (byte[])canid.Clone();
            //    clonedCanid[2] = 0x21;
            //    canidList.Add(clonedCanid);
            //    byteList.Add(Uint16ToBytes(AlarmParameter_12_1, AlarmParameter_12_2, AlarmParameter_12_3, AlarmParameter_12_4, 1, 1, 1, 1));
            //    //bytes[6] = Convert.ToByte(Convert.ToInt32(txt_56.Text) + 40);
            //    //bytes[7] = Convert.ToByte(Convert.ToInt32(txt_57.Text) + 40);
            //}

            //if (IsChecked_AlarmParameter_13)
            //{
            //    if (AlarmParameter_13_1 == null || AlarmParameter_13_2 == null )
            //    {
            //        MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
            //        return;
            //    }
            //    var clonedCanid = (byte[])canid.Clone();
            //    clonedCanid[2] = 0x22;
            //    canidList.Add(clonedCanid);
            //    byteList.Add(Uint16ToBytes(AlarmParameter_13_1, AlarmParameter_13_2, AlarmParameter_13_1, AlarmParameter_13_2, 0.1, 0.01, 1, 1));
            //}

            if (IsChecked_AlarmParameter_12)
            {
                if (AlarmParameter_12_1 == null || AlarmParameter_12_2 == null || AlarmParameter_12_3 == null || AlarmParameter_12_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x80;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes(AlarmParameter_12_1, AlarmParameter_12_2, AlarmParameter_12_3, AlarmParameter_12_4, 1, 1, 1, 1));
            }

            if (IsChecked_AlarmParameter_13)
            {
                if (AlarmParameter_13_1 == null || AlarmParameter_13_2 == null || AlarmParameter_13_3 == null || AlarmParameter_13_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x81;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes(AlarmParameter_13_1, AlarmParameter_13_2, AlarmParameter_13_3, AlarmParameter_13_4, 1, 1, 1, 1));
            }

            if (IsChecked_AlarmParameter_14)
            {
                if (AlarmParameter_14_1 == null || AlarmParameter_14_2 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x82;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes(AlarmParameter_14_1, AlarmParameter_14_2, AlarmParameter_14_1, AlarmParameter_14_2, 1, 1, 1, 1));
            }

            if (IsChecked_AlarmParameter_15)
            {
                if (AlarmParameter_15_1 == null || AlarmParameter_15_2 == null || AlarmParameter_15_3 == null || AlarmParameter_15_4 == null ||
                    AlarmParameter_15_5 == null || AlarmParameter_15_6 == null || AlarmParameter_15_7 == null || AlarmParameter_15_8 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x83;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes_8_Offset(AlarmParameter_15_1, AlarmParameter_15_2, AlarmParameter_15_3, AlarmParameter_15_4, AlarmParameter_15_5, AlarmParameter_15_6, AlarmParameter_15_7, AlarmParameter_15_8, 1, 1, 1, 1, 1, 1, 1, 1, -40, -40, -40, -40, -40, -40, -40, -40));
            }

            if (IsChecked_AlarmParameter_16)
            {
                if (AlarmParameter_16_1 == null || AlarmParameter_16_2 == null || AlarmParameter_16_3 == null || AlarmParameter_16_4 == null ||
                    AlarmParameter_16_5 == null || AlarmParameter_16_6 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x84;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes_8_Offset(AlarmParameter_16_1, AlarmParameter_16_2, AlarmParameter_16_3, AlarmParameter_16_4, AlarmParameter_16_5, AlarmParameter_16_6, AlarmParameter_16_1, AlarmParameter_16_2, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0));
            }

            if (IsChecked_AlarmParameter_17)
            {
                if (AlarmParameter_17_1 == null || AlarmParameter_17_2 == null || AlarmParameter_17_3 == null || AlarmParameter_17_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x85;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes(AlarmParameter_17_1, AlarmParameter_17_2, AlarmParameter_17_3, AlarmParameter_17_4, 0.1, 0.1, 0.1, 0.1));
            }
            // 发送指令
            try
            {
                for (int i = 0; i < byteList.Count; i++)
                {
                    bool result = baseCanHelper.Send(byteList[i], canidList[i]);
                    sendResult.Add(i.ToString(), result);
                }

            }
            catch (Exception ex)
            {
                MessageBoxHelper.Error(ex.Message, "异常捕获", null, ButtonType.OK);
            }

            if (sendResult.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in sendResult)
                {
                    if (!item.Value)
                    {
                        sb.AppendLine(item.Key + "写入失败");
                    }
                }

                string msgInfo = string.IsNullOrEmpty(sb.ToString()) ? "写入成功" : sb.ToString();
                MessageBoxHelper.Info(msgInfo, "提示", null, ButtonType.OK);
            }
        }

        public void Import(Window window)
        {
            try
            {
                var fileDialog = new OpenFileDialog
                {
                    Multiselect = false,
                    Title = "请选择文件",
                    Filter = "XML文件|*.xml"
                };

                bool? result = fileDialog.ShowDialog();
                if (result != true) return;

                string filePath = fileDialog.FileName;
                var bReturn = LoadXmlParam(filePath);
                if (bReturn)
                {
                    MessageBoxHelper.Success("导入成功！", "提示", null, ButtonType.OK);
                }

            }
            catch (XmlException xmlEx)
            {
                MessageBoxHelper.Error($"XML格式错误: {xmlEx.Message}", "错误", null, ButtonType.OK);
            }
            catch (Exception ex)
            {
                MessageBoxHelper.Error($"导入过程中发生错误: {ex.Message}", "错误", null, ButtonType.OK);
            }
        }

        private bool LoadXmlParam(string filePath)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filePath);

            XmlNodeList nodeList = document.DocumentElement.SelectNodes("//param");
            if (nodeList == null || nodeList.Count == 0)
            {
                MessageBox.Show("未找到参数节点。", "导入错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            //// 清空现有的参数
            //Parameters.Clear();

            // 创建字典以映射参数名称到相应的属性设置
            var propertyMap = new Dictionary<string, Action<string>>()
                {
                    { "txtAlarmParameters_1_1",   value => AlarmParameter_1_1   = value },
                    { "txtAlarmParameters_1_2",   value => AlarmParameter_1_2   = value },
                    { "txtAlarmParameters_1_3",   value => AlarmParameter_1_3   = value },
                    { "txtAlarmParameters_1_4",   value => AlarmParameter_1_4   = value },
                    { "txtAlarmParameters_2_1",   value => AlarmParameter_2_1   = value },
                    { "txtAlarmParameters_2_2",   value => AlarmParameter_2_2   = value },
                    { "txtAlarmParameters_2_3",   value => AlarmParameter_2_3   = value },
                    { "txtAlarmParameters_2_4",   value => AlarmParameter_2_4   = value },
                    { "txtAlarmParameters_3_1",   value => AlarmParameter_3_1   = value },
                    { "txtAlarmParameters_3_2",   value => AlarmParameter_3_2   = value },
                    { "txtAlarmParameters_3_3",   value => AlarmParameter_3_3   = value },
                    { "txtAlarmParameters_3_4",   value => AlarmParameter_3_4   = value },
                    { "txtAlarmParameters_4_1",   value => AlarmParameter_4_1   = value },
                    { "txtAlarmParameters_4_2",   value => AlarmParameter_4_2   = value },
                    { "txtAlarmParameters_4_3",   value => AlarmParameter_4_3   = value },
                    { "txtAlarmParameters_4_4",   value => AlarmParameter_4_4   = value },
                    { "txtAlarmParameters_5_1",   value => AlarmParameter_5_1   = value },
                    { "txtAlarmParameters_5_2",   value => AlarmParameter_5_2   = value },
                    { "txtAlarmParameters_5_3",   value => AlarmParameter_5_3   = value },
                    { "txtAlarmParameters_5_4",   value => AlarmParameter_5_4   = value },
                    { "txtAlarmParameters_6_1",   value => AlarmParameter_6_1   = value },
                    { "txtAlarmParameters_6_2",   value => AlarmParameter_6_2   = value },
                    { "txtAlarmParameters_6_3",   value => AlarmParameter_6_3   = value },
                    { "txtAlarmParameters_6_4",   value => AlarmParameter_6_4   = value },
                    { "txtAlarmParameters_7_1",   value => AlarmParameter_7_1   = value },
                    { "txtAlarmParameters_7_2",   value => AlarmParameter_7_2   = value },
                    { "txtAlarmParameters_7_3",   value => AlarmParameter_7_3   = value },
                    { "txtAlarmParameters_7_4",   value => AlarmParameter_7_4   = value },
                    { "txtAlarmParameters_7_5",   value => AlarmParameter_7_5   = value },
                    { "txtAlarmParameters_7_6",   value => AlarmParameter_7_6   = value },
                    { "txtAlarmParameters_7_7",   value => AlarmParameter_7_7   = value },
                    { "txtAlarmParameters_7_8",   value => AlarmParameter_7_8   = value },
                    { "txtAlarmParameters_8_1",   value => AlarmParameter_8_1   = value },
                    { "txtAlarmParameters_8_2",   value => AlarmParameter_8_2   = value },
                    { "txtAlarmParameters_8_3",   value => AlarmParameter_8_3   = value },
                    { "txtAlarmParameters_8_4",   value => AlarmParameter_8_4   = value },
                    { "txtAlarmParameters_8_5",   value => AlarmParameter_8_5   = value },
                    { "txtAlarmParameters_8_6",   value => AlarmParameter_8_6   = value },
                    { "txtAlarmParameters_8_7",   value => AlarmParameter_8_7   = value },
                    { "txtAlarmParameters_8_8",   value => AlarmParameter_8_8   = value },
                    { "txtAlarmParameters_9_1",   value => AlarmParameter_9_1   = value },
                    { "txtAlarmParameters_9_2",   value => AlarmParameter_9_2   = value },
                    { "txtAlarmParameters_9_3",   value => AlarmParameter_9_3   = value },
                    { "txtAlarmParameters_9_4",   value => AlarmParameter_9_4   = value },
                    { "txtAlarmParameters_10_1",  value => AlarmParameter_10_1  = value },
                    { "txtAlarmParameters_10_2",  value => AlarmParameter_10_2  = value },
                    { "txtAlarmParameters_10_3",  value => AlarmParameter_10_3  = value },
                    { "txtAlarmParameters_10_4",  value => AlarmParameter_10_4  = value },
                    { "txtAlarmParameters_11_1",  value => AlarmParameter_11_1  = value },
                    { "txtAlarmParameters_11_2",  value => AlarmParameter_11_2  = value },
                    { "txtAlarmParameters_11_3",  value => AlarmParameter_11_3  = value },
                    { "txtAlarmParameters_11_4",  value => AlarmParameter_11_4  = value },
                    { "txtAlarmParameters_12_1",  value => AlarmParameter_12_1  = value },
                    { "txtAlarmParameters_12_2",  value => AlarmParameter_12_2  = value },
                    { "txtAlarmParameters_12_3",  value => AlarmParameter_12_3  = value },
                    { "txtAlarmParameters_12_4",  value => AlarmParameter_12_4  = value },
                    { "txtAlarmParameters_13_1",  value => AlarmParameter_13_1  = value },
                    { "txtAlarmParameters_13_2",  value => AlarmParameter_13_2  = value },
                    { "txtAlarmParameters_13_3",  value => AlarmParameter_13_3  = value },
                    { "txtAlarmParameters_13_4",  value => AlarmParameter_13_4  = value },
                    { "txtAlarmParameters_14_1",  value => AlarmParameter_14_1  = value },
                    { "txtAlarmParameters_14_2",  value => AlarmParameter_14_2  = value },
                    { "txtAlarmParameters_15_1",  value => AlarmParameter_15_1  = value },
                    { "txtAlarmParameters_15_2",  value => AlarmParameter_15_2  = value },
                    { "txtAlarmParameters_15_3",  value => AlarmParameter_15_3  = value },
                    { "txtAlarmParameters_15_4",  value => AlarmParameter_15_4  = value },
                    { "txtAlarmParameters_15_5",  value => AlarmParameter_15_5  = value },
                    { "txtAlarmParameters_15_6",  value => AlarmParameter_15_6  = value },
                    { "txtAlarmParameters_15_7",  value => AlarmParameter_15_7  = value },
                    { "txtAlarmParameters_15_8",  value => AlarmParameter_15_8  = value },
                    { "txtAlarmParameters_16_1",  value => AlarmParameter_16_1  = value },
                    { "txtAlarmParameters_16_2",  value => AlarmParameter_16_2  = value },
                    { "txtAlarmParameters_16_3",  value => AlarmParameter_16_3  = value },
                    { "txtAlarmParameters_16_4",  value => AlarmParameter_16_4  = value },
                    { "txtAlarmParameters_16_5",  value => AlarmParameter_16_5  = value },
                    { "txtAlarmParameters_16_6",  value => AlarmParameter_16_6  = value },
                    { "txtAlarmParameters_17_1",  value => AlarmParameter_17_1  = value },
                    { "txtAlarmParameters_17_2",  value => AlarmParameter_17_2  = value },
                    { "txtAlarmParameters_17_3",  value => AlarmParameter_17_3  = value },
                    { "txtAlarmParameters_17_4",  value => AlarmParameter_17_4  = value },
                };

            // 遍历每个参数节点并更新对应的值
            foreach (XmlNode xmlNode in nodeList)
            {
                XmlElement element = (XmlElement)xmlNode;
                string controlName = element.GetAttribute("id");
                string paramValue = element.InnerText;

                // 更新参数
                if (propertyMap.TryGetValue(controlName, out Action<string> setParameter))
                {
                    setParameter(paramValue);
                }
                //// 将参数添加到 ObservableCollection
                //Parameters.Add(new ParamViewModel { ControlName = controlName, Value = paramValue });
            }
            return true;
        }

        public void Export(Window window)
        {
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Title = "保存文件",
                    Filter = "XML文件|*.xml"
                };

                bool? result = saveFileDialog.ShowDialog();
                if (result != true) return;

                string filePath = saveFileDialog.FileName;
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine("<documentParams>");

                    // 导出参数
                    ExportParameters(sw, 1, 6, 4);   // 导出1-6参数组，    每组4个
                    ExportParameters(sw, 7, 8, 8);   // 导出第7-8组参数，  每组8个
                    ExportParameters(sw, 9, 11, 4);  // 导出第9-11组参数,  每组4个
                    ExportParameters(sw, 12, 13, 4); // 导出第12-13组参数，每组4个
                    ExportParameters(sw, 14, 14, 2); // 导出第14组参数，   2个
                    ExportParameters(sw, 15, 15, 8); // 导出第15组参数，   8个
                    ExportParameters(sw, 16, 16, 6); // 导出第16组参数，   6个
                    ExportParameters(sw, 17, 17, 4); // 导出第17组参数，   4个

                    sw.WriteLine("</documentParams>");
                }
                MessageBoxHelper.Success("导出成功！", "提示", null, ButtonType.OK);
            }
            catch (Exception ex)
            {
                MessageBoxHelper.Error($"导出过程中发生错误: {ex.Message}", "导出错误", null, ButtonType.OK);
            }

        }

        private void ExportParameters(StreamWriter sw, int startGroup, int endGroup, int parameterCount)
        {
            for (int i = startGroup; i <= endGroup; i++)
            {
                for (int k = 1; k <= parameterCount; k++)
                {
                    string controlName = $"txtAlarmParameters_{i}_{k}";
                    string val = controlName switch
                    {
                        "txtAlarmParameters_1_1" => AlarmParameter_1_1,
                        "txtAlarmParameters_1_2" => AlarmParameter_1_2,
                        "txtAlarmParameters_1_3" => AlarmParameter_1_3,
                        "txtAlarmParameters_1_4" => AlarmParameter_1_4,
                        "txtAlarmParameters_2_1" => AlarmParameter_2_1,
                        "txtAlarmParameters_2_2" => AlarmParameter_2_2,
                        "txtAlarmParameters_2_3" => AlarmParameter_2_3,
                        "txtAlarmParameters_2_4" => AlarmParameter_2_4,
                        "txtAlarmParameters_3_1" => AlarmParameter_3_1,
                        "txtAlarmParameters_3_2" => AlarmParameter_3_2,
                        "txtAlarmParameters_3_3" => AlarmParameter_3_3,
                        "txtAlarmParameters_3_4" => AlarmParameter_3_4,
                        "txtAlarmParameters_4_1" => AlarmParameter_4_1,
                        "txtAlarmParameters_4_2" => AlarmParameter_4_2,
                        "txtAlarmParameters_4_3" => AlarmParameter_4_3,
                        "txtAlarmParameters_4_4" => AlarmParameter_4_4,
                        "txtAlarmParameters_5_1" => AlarmParameter_5_1,
                        "txtAlarmParameters_5_2" => AlarmParameter_5_2,
                        "txtAlarmParameters_5_3" => AlarmParameter_5_3,
                        "txtAlarmParameters_5_4" => AlarmParameter_5_4,
                        "txtAlarmParameters_6_1" => AlarmParameter_6_1,
                        "txtAlarmParameters_6_2" => AlarmParameter_6_2,
                        "txtAlarmParameters_6_3" => AlarmParameter_6_3,
                        "txtAlarmParameters_6_4" => AlarmParameter_6_4,
                        "txtAlarmParameters_7_1" => AlarmParameter_7_1,
                        "txtAlarmParameters_7_2" => AlarmParameter_7_2,
                        "txtAlarmParameters_7_3" => AlarmParameter_7_3,
                        "txtAlarmParameters_7_4" => AlarmParameter_7_4,
                        "txtAlarmParameters_7_5" => AlarmParameter_7_5,
                        "txtAlarmParameters_7_6" => AlarmParameter_7_6,
                        "txtAlarmParameters_7_7" => AlarmParameter_7_7,
                        "txtAlarmParameters_7_8" => AlarmParameter_7_8,
                        "txtAlarmParameters_8_1" => AlarmParameter_8_1,
                        "txtAlarmParameters_8_2" => AlarmParameter_8_2,
                        "txtAlarmParameters_8_3" => AlarmParameter_8_3,
                        "txtAlarmParameters_8_4" => AlarmParameter_8_4,
                        "txtAlarmParameters_8_5" => AlarmParameter_8_5,
                        "txtAlarmParameters_8_6" => AlarmParameter_8_6,
                        "txtAlarmParameters_8_7" => AlarmParameter_8_7,
                        "txtAlarmParameters_8_8" => AlarmParameter_8_8,
                        "txtAlarmParameters_9_1" => AlarmParameter_9_1,
                        "txtAlarmParameters_9_2" => AlarmParameter_9_2,
                        "txtAlarmParameters_9_3" => AlarmParameter_9_3,
                        "txtAlarmParameters_9_4" => AlarmParameter_9_4,
                        "txtAlarmParameters_10_1" => AlarmParameter_10_1,
                        "txtAlarmParameters_10_2" => AlarmParameter_10_2,
                        "txtAlarmParameters_10_3" => AlarmParameter_10_3,
                        "txtAlarmParameters_10_4" => AlarmParameter_10_4,
                        "txtAlarmParameters_11_1" => AlarmParameter_11_1,
                        "txtAlarmParameters_11_2" => AlarmParameter_11_2,
                        "txtAlarmParameters_11_3" => AlarmParameter_11_3,
                        "txtAlarmParameters_11_4" => AlarmParameter_11_4,
                        "txtAlarmParameters_12_1" => AlarmParameter_12_1,
                        "txtAlarmParameters_12_2" => AlarmParameter_12_2,
                        "txtAlarmParameters_12_3" => AlarmParameter_12_3,
                        "txtAlarmParameters_12_4" => AlarmParameter_12_4,
                        "txtAlarmParameters_13_1" => AlarmParameter_13_1,
                        "txtAlarmParameters_13_2" => AlarmParameter_13_2,
                        "txtAlarmParameters_13_3" => AlarmParameter_13_3,
                        "txtAlarmParameters_13_4" => AlarmParameter_13_4,
                        "txtAlarmParameters_14_1" => AlarmParameter_14_1,
                        "txtAlarmParameters_14_2" => AlarmParameter_14_2,
                        "txtAlarmParameters_15_1" => AlarmParameter_15_1,
                        "txtAlarmParameters_15_2" => AlarmParameter_15_2,
                        "txtAlarmParameters_15_3" => AlarmParameter_15_3,
                        "txtAlarmParameters_15_4" => AlarmParameter_15_4,
                        "txtAlarmParameters_15_5" => AlarmParameter_15_5,
                        "txtAlarmParameters_15_6" => AlarmParameter_15_6,
                        "txtAlarmParameters_15_7" => AlarmParameter_15_7,
                        "txtAlarmParameters_15_8" => AlarmParameter_15_8,
                        "txtAlarmParameters_16_1" => AlarmParameter_16_1,
                        "txtAlarmParameters_16_2" => AlarmParameter_16_2,
                        "txtAlarmParameters_16_3" => AlarmParameter_16_3,
                        "txtAlarmParameters_16_4" => AlarmParameter_16_4,
                        "txtAlarmParameters_16_5" => AlarmParameter_16_5,
                        "txtAlarmParameters_16_6" => AlarmParameter_16_6,
                        "txtAlarmParameters_17_1" => AlarmParameter_17_1,
                        "txtAlarmParameters_17_2" => AlarmParameter_17_2,
                        "txtAlarmParameters_17_3" => AlarmParameter_17_3,
                        "txtAlarmParameters_17_4" => AlarmParameter_17_4,

                        _ => string.Empty // 对应不存在的情况
                    };

                    sw.WriteLine($" <param id=\"{controlName}\" name=\"{keys[controlName]}\">{val}</param>");
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

        private byte[] Uint16ToBytes_8_Offset(string t1, string t2, string t3, string t4, string t5, string t6, string t7, string t8, double scaling1, double scaling2, double scaling3, double scaling4, double scaling5, double scaling6, double scaling7, double scaling8,int offset1, int offset2, int offset3, int offset4, int offset5, int offset6, int offset7, int offset8)
        {
            byte byte1 = (byte)(Convert.ToUInt32(float.Parse(t1) / scaling1 - offset1));
            byte byte2 = (byte)(Convert.ToUInt32(float.Parse(t2) / scaling2 - offset2));
            byte byte3 = (byte)(Convert.ToUInt32(float.Parse(t3) / scaling3 - offset3));
            byte byte4 = (byte)(Convert.ToUInt32(float.Parse(t4) / scaling4 - offset4));
            byte byte5 = (byte)(Convert.ToUInt32(float.Parse(t5) / scaling5 - offset5));
            byte byte6 = (byte)(Convert.ToUInt32(float.Parse(t6) / scaling6 - offset6));
            byte byte7 = (byte)(Convert.ToUInt32(float.Parse(t7) / scaling7 - offset7));
            byte byte8 = (byte)(Convert.ToUInt32(float.Parse(t8) / scaling8 - offset8));
            return new byte[] { byte1, byte2, byte3, byte4, byte5, byte6, byte7, byte8 };
        }
        private byte[] Uint16ToBytes(uint ivalue)
        {
            byte[] data = new byte[2];
            data[1] = (byte)(ivalue >> 8);
            data[0] = (byte)(ivalue & 0xff);
            return data;
        }
        private byte[] Uint16ToBytes(string t1, string t2, string t3, string t4, double scaling1, double scaling2, double scaling3, double scaling4)
        {
            byte[] b1 = Uint16ToBytes(Convert.ToUInt32(float.Parse(t1) / scaling1));
            byte[] b2 = Uint16ToBytes(Convert.ToUInt32(float.Parse(t2) / scaling2));
            byte[] b3 = Uint16ToBytes(Convert.ToUInt32(float.Parse(t3) / scaling3));
            byte[] b4 = Uint16ToBytes(Convert.ToUInt32(float.Parse(t4) / scaling4));

            return new byte[] { b1[0], b1[1], b2[0], b2[1], b3[0], b3[1], b4[0], b4[1] };
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
    }
}
