using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Win32;
using Sofar.BMSLib;
using Sofar.BMSUI;
using Sofar.BMSUI.Common;
using Sofar.ProtocolLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace Sofar.HvBMSUI.ViewModels
{
    public partial class BCU_ParamControl_ViewModel : ObservableObject
    {

        public CancellationTokenSource cts = null;
        CommandOperation bmsOper = null;
        XmlDocument mDocument;
        public bool flag = true;
        public static int index = 1;
        public static Dictionary<string, string> keys = new Dictionary<string, string>();

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
                OnPropertyChanged(nameof(SelectedAddress_BCU));
            }
        }

        public ObservableCollection<string> NewAddress_BCU_List { get; } = new ObservableCollection<string>
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

        private string _selectedNewAddress_BCU = "E8";
        /// <summary>
        /// 设置新的BCU地址
        /// </summary>
        public string SelectedNewAddress_BCU
        {
            get { return _selectedNewAddress_BCU; }
            set
            {
                _selectedNewAddress_BCU = value;
                OnPropertyChanged(nameof(SelectedNewAddress_BCU));
            }
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

        private string _alarmParameter_9_1;
        public string AlarmParameter_9_1
        {
            get => _alarmParameter_9_1;
            set => SetProperty(ref _alarmParameter_9_1, value);
        }

        private string _alarmParameter_9_2;
        public string AlarmParameter_9_2
        {
            get => _alarmParameter_9_2;
            set => SetProperty(ref _alarmParameter_9_2, value);
        }

        private string _alarmParameter_9_3;
        public string AlarmParameter_9_3
        {
            get => _alarmParameter_9_3;
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

        private string _alarmParameter_11_5;
        public string AlarmParameter_11_5
        {
            get => _alarmParameter_11_5;
            set => SetProperty(ref _alarmParameter_11_5, value);
        }

        private string _alarmParameter_11_6;
        public string AlarmParameter_11_6
        {
            get => _alarmParameter_11_6;
            set => SetProperty(ref _alarmParameter_11_6, value);
        }

        private string _alarmParameter_11_7;
        public string AlarmParameter_11_7
        {
            get => _alarmParameter_11_7;
            set => SetProperty(ref _alarmParameter_11_7, value);
        }

        private string _alarmParameter_11_8;
        public string AlarmParameter_11_8
        {
            get => _alarmParameter_11_8;
            set => SetProperty(ref _alarmParameter_11_8, value);
        }

        private string _alarmParameter_11_9;
        public string AlarmParameter_11_9
        {
            get => _alarmParameter_11_9;
            set => SetProperty(ref _alarmParameter_11_9, value);
        }

        private string _alarmParameter_11_10;
        public string AlarmParameter_11_10
        {
            get => _alarmParameter_11_10;
            set => SetProperty(ref _alarmParameter_11_10, value);
        }

        private string _alarmParameter_11_11;
        public string AlarmParameter_11_11
        {
            get => _alarmParameter_11_11;
            set => SetProperty(ref _alarmParameter_11_11, value);
        }

        private string _alarmParameter_11_12;
        public string AlarmParameter_11_12
        {
            get => _alarmParameter_11_12;
            set => SetProperty(ref _alarmParameter_11_12, value);
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

        private string _alarmParameter_12_5;
        public string AlarmParameter_12_5
        {
            get => _alarmParameter_12_5;
            set => SetProperty(ref _alarmParameter_12_5, value);
        }

        private string _alarmParameter_12_6;
        public string AlarmParameter_12_6
        {
            get => _alarmParameter_12_6;
            set => SetProperty(ref _alarmParameter_12_6, value);
        }

        private string _alarmParameter_12_7;
        public string AlarmParameter_12_7
        {
            get => _alarmParameter_12_7;
            set => SetProperty(ref _alarmParameter_12_7, value);
        }

        private string _alarmParameter_12_8;
        public string AlarmParameter_12_8
        {
            get => _alarmParameter_12_8;
            set => SetProperty(ref _alarmParameter_12_8, value);
        }

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
        private string _alarmParameter_14_3;
        public string AlarmParameter_14_3
        {
            get => _alarmParameter_14_3;
            set => SetProperty(ref _alarmParameter_14_3, value);
        }

        private string _alarmParameter_14_4;
        public string AlarmParameter_14_4
        {
            get => _alarmParameter_14_4;
            set => SetProperty(ref _alarmParameter_14_4, value);
        }

        private string _alarmParameter_14_5;
        public string AlarmParameter_14_5
        {
            get => _alarmParameter_14_5;
            set => SetProperty(ref _alarmParameter_14_5, value);
        }

        private string _alarmParameter_14_6;
        public string AlarmParameter_14_6
        {
            get => _alarmParameter_14_6;
            set => SetProperty(ref _alarmParameter_14_6, value);
        }

        private string _alarmParameter_14_7;
        public string AlarmParameter_14_7
        {
            get => _alarmParameter_14_7;
            set => SetProperty(ref _alarmParameter_14_7, value);
        }

        private string _alarmParameter_14_8;
        public string AlarmParameter_14_8
        {
            get => _alarmParameter_14_8;
            set => SetProperty(ref _alarmParameter_14_8, value);
        }

        private string _alarmParameter_14_9;
        public string AlarmParameter_14_9
        {
            get => _alarmParameter_14_9;
            set => SetProperty(ref _alarmParameter_14_9, value);
        }

        private string _alarmParameter_14_10;
        public string AlarmParameter_14_10
        {
            get => _alarmParameter_14_10;
            set => SetProperty(ref _alarmParameter_14_10, value);
        }

        private string _alarmParameter_14_11;
        public string AlarmParameter_14_11
        {
            get => _alarmParameter_14_11;
            set => SetProperty(ref _alarmParameter_14_11, value);
        }

        private string _alarmParameter_14_12;
        public string AlarmParameter_14_12
        {
            get => _alarmParameter_14_12;
            set => SetProperty(ref _alarmParameter_14_12, value);

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

        private bool _isAllChecked;
        public bool IsAllChecked
        {
            get { return _isAllChecked; }
            set
            {
                _isAllChecked = value;
                if (value)
                {

                    for (int i = 1; i <= 15; i++)
                    {
                        GetType().GetProperty($"IsChecked_AlarmParameter_" + i).SetValue(this, true);
                    }

                }
                else
                {
                    for (int i = 1; i <= 15; i++)
                    {
                        GetType().GetProperty($"IsChecked_AlarmParameter_" + i).SetValue(this, false);
                    }
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





        public ObservableCollection<ParamViewModel> Parameters { get; set; } = new ObservableCollection<ParamViewModel>();
        public class ParamViewModel : ObservableObject
        {
            public string ControlName { get; set; }
            public string Value { get; set; }
        }


        public ICommand ImportCmd => new RelayCommand<Window>(Import);
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
                if (bReturn == true)
                {
                    MessageBoxHelper.Info("界面参数已导入，需下设后生效！", "提示", null, ButtonType.OK);
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
                    //{ "txtAlarmParameters_1_1",   value => AlarmParameter_1_1   = value },
                    //{ "txtAlarmParameters_1_2",   value => AlarmParameter_1_2   = value },
                    //{ "txtAlarmParameters_1_3",   value => AlarmParameter_1_3   = value },
                    { "txtAlarmParameters_1_4",   value => AlarmParameter_1_4   = value },
                    //{ "txtAlarmParameters_2_1",   value => AlarmParameter_2_1   = value },
                    //{ "txtAlarmParameters_2_2",   value => AlarmParameter_2_2   = value },
                    //{ "txtAlarmParameters_2_3",   value => AlarmParameter_2_3   = value },
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
                    { "txtAlarmParameters_8_1",   value => AlarmParameter_8_1   = value },
                    { "txtAlarmParameters_8_2",   value => AlarmParameter_8_2   = value },
                    { "txtAlarmParameters_8_3",   value => AlarmParameter_8_3   = value },
                    { "txtAlarmParameters_8_4",   value => AlarmParameter_8_4   = value },
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
                    { "txtAlarmParameters_11_5",  value => AlarmParameter_11_5  = value },
                    { "txtAlarmParameters_11_6",  value => AlarmParameter_11_6  = value },
                    { "txtAlarmParameters_11_7",  value => AlarmParameter_11_7  = value },
                    { "txtAlarmParameters_11_8",  value => AlarmParameter_11_8  = value },
                    { "txtAlarmParameters_11_9",  value => AlarmParameter_11_9  = value },
                    { "txtAlarmParameters_11_10", value => AlarmParameter_11_10 = value },
                    { "txtAlarmParameters_11_11", value => AlarmParameter_11_11 = value },
                    { "txtAlarmParameters_11_12", value => AlarmParameter_11_12 = value },
                    { "txtAlarmParameters_12_1",  value => AlarmParameter_12_1  = value },
                    { "txtAlarmParameters_12_2",  value => AlarmParameter_12_2  = value },
                    { "txtAlarmParameters_12_3",  value => AlarmParameter_12_3  = value },
                    { "txtAlarmParameters_12_4",  value => AlarmParameter_12_4  = value },
                    { "txtAlarmParameters_12_5",  value => AlarmParameter_12_5  = value },
                    { "txtAlarmParameters_12_6",  value => AlarmParameter_12_6  = value },
                    { "txtAlarmParameters_12_7",  value => AlarmParameter_12_7  = value },
                    { "txtAlarmParameters_12_8",  value => AlarmParameter_12_8  = value },
                    { "txtAlarmParameters_13_1",  value => AlarmParameter_13_1  = value },
                    { "txtAlarmParameters_13_2",  value => AlarmParameter_13_2  = value },
                    { "txtAlarmParameters_13_3",  value => AlarmParameter_13_3  = value },
                    { "txtAlarmParameters_13_4",  value => AlarmParameter_13_4  = value },
                    { "txtAlarmParameters_14_1",  value => AlarmParameter_14_1  = value },
                    { "txtAlarmParameters_14_2",  value => AlarmParameter_14_2  = value },
                    { "txtAlarmParameters_14_3",  value => AlarmParameter_14_3  = value },
                    { "txtAlarmParameters_14_4",  value => AlarmParameter_14_4  = value },
                    { "txtAlarmParameters_14_5",  value => AlarmParameter_14_5  = value },
                    { "txtAlarmParameters_14_6",  value => AlarmParameter_14_6  = value },
                    { "txtAlarmParameters_14_7",  value => AlarmParameter_14_7  = value },
                    { "txtAlarmParameters_14_8",  value => AlarmParameter_14_8  = value },
                    { "txtAlarmParameters_14_9",  value => AlarmParameter_14_9  = value },
                    { "txtAlarmParameters_14_10", value => AlarmParameter_14_10 = value },
                    { "txtAlarmParameters_14_11", value => AlarmParameter_14_11 = value },
                    { "txtAlarmParameters_14_12", value => AlarmParameter_14_12 = value },
                    { "txtAlarmParameters_15_1",  value => AlarmParameter_15_1  = value },
                    { "txtAlarmParameters_15_2",  value => AlarmParameter_15_2  = value },
                    { "txtAlarmParameters_15_3",  value => AlarmParameter_15_3  = value },
                    { "txtAlarmParameters_15_4",  value => AlarmParameter_15_4  = value },
                    { "txtAlarmParameters_15_5",  value => AlarmParameter_15_5  = value },
                    { "txtAlarmParameters_15_6",  value => AlarmParameter_15_6  = value },
                    { "txtAlarmParameters_15_7",  value => AlarmParameter_15_7  = value },
                    { "txtAlarmParameters_15_8",  value => AlarmParameter_15_8  = value }
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


            //查询从控模块基本参数(Byte1: 包序号0x01, 包序号0x02 - 0xFF暂时全部预留)
            Task.Run(() =>
            {
                int ret = 50;
                while (ConstantDef.BCU_ModuleNumber == -1 && ret-- > 0)
                {
                    byte Address_BCU = Convert.ToByte(Convert.ToInt32(SelectedAddress_BCU, 16));
                    byte[] bytes7 = new byte[8] { 0x14, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    bmsOper.Send(bytes7, new byte[] { 0xF4, Address_BCU, 0x1F, 0x18 });

                    //byte[] bytes0 = new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    //bmsOper.Send(bytes0, new byte[] { 0xF4, Address_BCU, 0x1F, 0x18 });
                    Thread.Sleep(100);
                }
            }).ContinueWith((obj) =>
            {
                AlarmParameter_1_1 = (Convert.ToDecimal(AlarmParameter_8_1) * ConstantDef.BCU_ModuleNumber).ToString();
                AlarmParameter_1_2 = (Convert.ToDecimal(AlarmParameter_8_2) * ConstantDef.BCU_ModuleNumber).ToString();
                AlarmParameter_1_3 = (Convert.ToDecimal(AlarmParameter_8_3) * ConstantDef.BCU_ModuleNumber).ToString();

                AlarmParameter_2_1 = (Convert.ToDecimal(AlarmParameter_9_1) * ConstantDef.BCU_ModuleNumber).ToString();
                AlarmParameter_2_2 = (Convert.ToDecimal(AlarmParameter_9_2) * ConstantDef.BCU_ModuleNumber).ToString();
                AlarmParameter_2_3 = (Convert.ToDecimal(AlarmParameter_9_3) * ConstantDef.BCU_ModuleNumber).ToString();

            });


            return true;
        }

        public ICommand ExportCmd => new RelayCommand<Window>(Export);
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
                    ExportParameters(sw, 1, 10, 4);   // 导出前10个参数组，每组4个
                    ExportParameters(sw, 11, 11, 12); // 导出第11组参数，12个
                    ExportParameters(sw, 12, 12, 8);  // 导出第12组参数，8个
                    ExportParameters(sw, 13, 13, 4);  // 导出第13组参数，4个
                    ExportParameters(sw, 14, 14, 12); // 导出第14组参数，12个
                    ExportParameters(sw, 15, 15, 8);  // 导出第16组参数，8个

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
                        "txtAlarmParameters_8_1" => AlarmParameter_8_1,
                        "txtAlarmParameters_8_2" => AlarmParameter_8_2,
                        "txtAlarmParameters_8_3" => AlarmParameter_8_3,
                        "txtAlarmParameters_8_4" => AlarmParameter_8_4,
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
                        "txtAlarmParameters_11_5" => AlarmParameter_11_5,
                        "txtAlarmParameters_11_6" => AlarmParameter_11_6,
                        "txtAlarmParameters_11_7" => AlarmParameter_11_7,
                        "txtAlarmParameters_11_8" => AlarmParameter_11_8,
                        "txtAlarmParameters_11_9" => AlarmParameter_11_9,
                        "txtAlarmParameters_11_10" => AlarmParameter_11_10,
                        "txtAlarmParameters_11_11" => AlarmParameter_11_11,
                        "txtAlarmParameters_11_12" => AlarmParameter_11_12,
                        "txtAlarmParameters_12_1" => AlarmParameter_12_1,
                        "txtAlarmParameters_12_2" => AlarmParameter_12_2,
                        "txtAlarmParameters_12_3" => AlarmParameter_12_3,
                        "txtAlarmParameters_12_4" => AlarmParameter_12_4,
                        "txtAlarmParameters_12_5" => AlarmParameter_12_5,
                        "txtAlarmParameters_12_6" => AlarmParameter_12_6,
                        "txtAlarmParameters_12_7" => AlarmParameter_12_7,
                        "txtAlarmParameters_12_8" => AlarmParameter_12_8,
                        "txtAlarmParameters_13_1" => AlarmParameter_13_1,
                        "txtAlarmParameters_13_2" => AlarmParameter_13_2,
                        "txtAlarmParameters_13_3" => AlarmParameter_13_3,
                        "txtAlarmParameters_13_4" => AlarmParameter_13_4,
                        "txtAlarmParameters_14_1" => AlarmParameter_14_1,
                        "txtAlarmParameters_14_2" => AlarmParameter_14_2,
                        "txtAlarmParameters_14_3" => AlarmParameter_14_3,
                        "txtAlarmParameters_14_4" => AlarmParameter_14_4,
                        "txtAlarmParameters_14_5" => AlarmParameter_14_5,
                        "txtAlarmParameters_14_6" => AlarmParameter_14_6,
                        "txtAlarmParameters_14_7" => AlarmParameter_14_7,
                        "txtAlarmParameters_14_8" => AlarmParameter_14_8,
                        "txtAlarmParameters_14_9" => AlarmParameter_14_9,
                        "txtAlarmParameters_14_10" => AlarmParameter_14_10,
                        "txtAlarmParameters_14_11" => AlarmParameter_14_11,
                        "txtAlarmParameters_14_12" => AlarmParameter_14_12,
                        "txtAlarmParameters_15_1" => AlarmParameter_15_1,
                        "txtAlarmParameters_15_2" => AlarmParameter_15_2,
                        "txtAlarmParameters_15_3" => AlarmParameter_15_3,
                        "txtAlarmParameters_15_4" => AlarmParameter_15_4,
                        "txtAlarmParameters_15_5" => AlarmParameter_15_5,
                        "txtAlarmParameters_15_6" => AlarmParameter_15_6,
                        "txtAlarmParameters_15_7" => AlarmParameter_15_7,
                        "txtAlarmParameters_15_8" => AlarmParameter_15_8,
                        _ => string.Empty // 对应不存在的情况
                    };

                    sw.WriteLine($" <param id=\"{controlName}\" name=\"{keys[controlName]}\">{val}</param>");
                }
            }
        }

        public ICommand InitCmd => new RelayCommand<Window>(Init);
        /// <summary>
        /// 恢复主控报警参数默认值
        /// </summary>
        /// <param name="window"></param>
        public void Init(Window window)
        {



            var xmlPath = AppDomain.CurrentDomain.BaseDirectory + "Configuration\\BCU主控参数默认值.xml";
            var bReturn = LoadXmlParam(xmlPath);
            if (bReturn)
            {
                MessageBoxHelper.Info("界面参数已恢复，需下设后生效", "提示", null, ButtonType.OK);
                return;
            }
            //byte[] commandCodes = new byte[] { 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x1C, 0x2B, 0x2C };

            //byte Address_BCU = Convert.ToByte(Convert.ToInt32(SelectedAddress_BCU, 16));
            //foreach (byte code in commandCodes)
            //{
            //    byte[] bytes = new byte[8] { code, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            //    baseCanHelper.Send(bytes, new byte[] { 0xF4, Address_BCU, 0x1F, 0x18 });
            //}
        }

        public ICommand WriteBCUAddressCmd => new RelayCommand(WriteBCUAddress);
        /// <summary>
        /// 设置主控(BCU)地址（命令码 0x2A）
        /// </summary>
        public void WriteBCUAddress()
        {
            byte Address_BCU = Convert.ToByte(Convert.ToInt32(SelectedAddress_BCU, 16));
            byte NewAddress_BCU = Convert.ToByte(Convert.ToInt32(SelectedNewAddress_BCU, 16));
            byte[] FrameID = new byte[] { 0xF4, Address_BCU, 0x2A, 0x18 };
            byte[] data = new byte[8];
            data[0] = NewAddress_BCU;

            if (bmsOper.Send(data, FrameID)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);
        }

        public ICommand ReadCmd => new RelayCommand(Read);

        /// <summary>
        /// 读取主控报警参数
        /// </summary>
        public async void Read()
        {
            if (!bmsOper.IsConnection)
            {
                MessageBoxHelper.Info("请先打开CAN口!", "提示", null, ButtonType.OK);
                return;
            }


            bmsOper.ReadBCUParam(Convert.ToInt32(SelectedAddress_BCU, 16));

        }
        public ICommand WriteCmd => new RelayCommand<Window>(Write);
        /// <summary>
        /// 下设主控报警参数   
        /// </summary>
        /// <param name="window"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Write(Window window)
        {
            if (!bmsOper.IsConnection)
            {
                MessageBoxHelper.Info("请先打开CAN口!", "提示", null, ButtonType.OK);
                return;
            }

            Dictionary<string, bool> sendResult = new Dictionary<string, bool>();
            byte Address_BCU = Convert.ToByte(Convert.ToInt32(SelectedAddress_BCU, 16));
            byte[] canid = new byte[] { 0xF4, Address_BCU, 0x04, 0x18 };

            // 存储多个bytes和canid
            List<byte[]> byteList = new List<byte[]>();
            List<byte[]> canidList = new List<byte[]>();

            List<byte[]> byteList_11 = new List<byte[]>();
            List<byte[]> canidList_11 = new List<byte[]>();

            List<byte[]> byteList_14 = new List<byte[]>();
            List<byte[]> canidList_14 = new List<byte[]>();

            if (IsChecked_AlarmParameter_1)
            {
                if (AlarmParameter_1_1 == null || AlarmParameter_1_2 == null || AlarmParameter_1_3 == null || AlarmParameter_1_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x05;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes_4(AlarmParameter_1_1, AlarmParameter_1_2, AlarmParameter_1_3, AlarmParameter_1_4, 0.1, 0.1, 0.1, 0.1));
            }

            if (IsChecked_AlarmParameter_2)
            {
                if (AlarmParameter_2_1 == null || AlarmParameter_2_2 == null || AlarmParameter_2_3 == null || AlarmParameter_2_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x04;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes_4(AlarmParameter_2_1, AlarmParameter_2_2, AlarmParameter_2_3, AlarmParameter_2_4, 0.1, 0.1, 0.1, 0.1));
            }

            if (IsChecked_AlarmParameter_3)
            {
                if (AlarmParameter_3_1 == null || AlarmParameter_3_2 == null || AlarmParameter_3_3 == null || AlarmParameter_3_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x0D;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes_4(AlarmParameter_3_1, AlarmParameter_3_2, AlarmParameter_3_3, AlarmParameter_3_4, 0.001, 0.001, 0.001, 0.001));
            }

            if (IsChecked_AlarmParameter_4)
            {
                if (AlarmParameter_4_1 == null || AlarmParameter_4_2 == null || AlarmParameter_4_3 == null || AlarmParameter_4_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x0C;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes_4(AlarmParameter_4_1, AlarmParameter_4_2, AlarmParameter_4_3, AlarmParameter_4_4, 0.001, 0.001, 0.001, 0.001));
            }

            if (IsChecked_AlarmParameter_5)
            {
                if (AlarmParameter_5_1 == null || AlarmParameter_5_2 == null || AlarmParameter_5_3 == null || AlarmParameter_5_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x0E;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes_4(AlarmParameter_5_1, AlarmParameter_5_2, AlarmParameter_5_3, AlarmParameter_5_4, 0.001, 0.001, 0.001, 0.001));
            }

            if (IsChecked_AlarmParameter_6)
            {
                if (AlarmParameter_6_1 == null || AlarmParameter_6_2 == null || AlarmParameter_6_3 == null || AlarmParameter_6_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x09;
                canidList.Add(clonedCanid);
                //-40->修改为0 无偏差
                byteList.Add(Uint16ToBytes_4_Offset(AlarmParameter_6_1, AlarmParameter_6_2, AlarmParameter_6_3, AlarmParameter_6_4, 1, 1, 1, 1, 0, 0, 0, 0));
            }

            if (IsChecked_AlarmParameter_7)
            {
                if (AlarmParameter_7_1 == null || AlarmParameter_7_2 == null || AlarmParameter_7_3 == null || AlarmParameter_7_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x0B;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes_4(AlarmParameter_7_1, AlarmParameter_7_2, AlarmParameter_7_3, AlarmParameter_7_4, 1, 1, 1, 1));
            }

            if (IsChecked_AlarmParameter_8)
            {
                if (AlarmParameter_8_1 == null || AlarmParameter_8_2 == null || AlarmParameter_8_3 == null || AlarmParameter_8_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x2C;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes_4(AlarmParameter_8_1, AlarmParameter_8_2, AlarmParameter_8_3, AlarmParameter_8_4, 0.1, 0.1, 0.1, 0.1));
            }

            if (IsChecked_AlarmParameter_9)
            {
                if (AlarmParameter_9_1 == null || AlarmParameter_9_2 == null || AlarmParameter_9_3 == null || AlarmParameter_9_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x2B;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes_4(AlarmParameter_9_1, AlarmParameter_9_2, AlarmParameter_9_3, AlarmParameter_9_4, 0.1, 0.1, 0.1, 0.1));
            }

            if (IsChecked_AlarmParameter_10)
            {
                if (AlarmParameter_10_1 == null || AlarmParameter_10_2 == null || AlarmParameter_10_3 == null || AlarmParameter_10_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x07;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes_4(AlarmParameter_10_1, AlarmParameter_10_2, AlarmParameter_10_3, AlarmParameter_10_4, 0.1, 0.1, 0.1, 0.1));
            }

            if (IsChecked_AlarmParameter_11)
            {
                if (AlarmParameter_11_1 == null || AlarmParameter_11_2 == null || AlarmParameter_11_3 == null || AlarmParameter_11_4 == null ||
                    AlarmParameter_11_5 == null || AlarmParameter_11_6 == null || AlarmParameter_11_7 == null || AlarmParameter_11_8 == null ||
                    AlarmParameter_11_9 == null || AlarmParameter_11_10 == null || AlarmParameter_11_11 == null || AlarmParameter_11_12 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x06;


                for (int k = 1; k <= 3; k++)
                {
                    // 每次处理 4 个
                    string[] parameters = new string[]
                    {
                        GetAlarmParameter(k, 1), GetAlarmParameter(k, 2),
                        GetAlarmParameter(k, 3), GetAlarmParameter(k, 4)
                    };

                    byte[] b1 = Uint16ToBytes(Convert.ToUInt32(double.Parse(parameters[0]) / 0.1));
                    byte[] b2 = Uint16ToBytes(Convert.ToUInt32(double.Parse(parameters[1]) / 0.1));
                    byte[] b3 = Uint16ToBytes(Convert.ToUInt32(double.Parse(parameters[2]) / 0.1));
                    byte byte8 = (byte)(Convert.ToUInt32(double.Parse(parameters[3])) / 0.1);
                    byte[] bytes = new byte[] { (byte)k, b1[1], b1[0], b2[1], b2[0], b3[1], b3[0], byte8 };
                    canidList_11.Add(clonedCanid);
                    byteList_11.Add(bytes);

                }

                try
                {
                    for (int i = 0; i < byteList_11.Count; i++)
                    {
                        bool result = bmsOper.Send(byteList_11[i], canidList_11[i]);
                        //sendResult.Add(i.ToString(), result);
                    }

                }
                catch (Exception ex)
                {
                    MessageBoxHelper.Error(ex.Message, "提示", null, ButtonType.OK);

                }

                // 取对应的AlarmParameter值
                string GetAlarmParameter(int groupIndex, int parameterIndex)
                {
                    switch ((groupIndex - 1) * 4 + parameterIndex)
                    {
                        case 1: return AlarmParameter_11_1;
                        case 2: return AlarmParameter_11_2;
                        case 3: return AlarmParameter_11_3;
                        case 4: return AlarmParameter_11_4;
                        case 5: return AlarmParameter_11_5;
                        case 6: return AlarmParameter_11_6;
                        case 7: return AlarmParameter_11_7;
                        case 8: return AlarmParameter_11_8;
                        case 9: return AlarmParameter_11_9;
                        case 10: return AlarmParameter_11_10;
                        case 11: return AlarmParameter_11_11;
                        case 12: return AlarmParameter_11_12;
                        default: throw new ArgumentOutOfRangeException();
                    }
                }
            }

            if (IsChecked_AlarmParameter_12)
            {
                if (AlarmParameter_12_1 == null || AlarmParameter_12_2 == null || AlarmParameter_12_3 == null || AlarmParameter_12_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x08;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes_8_Offset(AlarmParameter_12_1, AlarmParameter_12_2, AlarmParameter_12_3, AlarmParameter_12_4, AlarmParameter_12_5, AlarmParameter_12_6, AlarmParameter_12_7, AlarmParameter_12_8, 1, 1, 1, 1, 1, 1, 1, 1, -40, -40, -40, 0, -40, -40, -40, 0));
            }

            if (IsChecked_AlarmParameter_13)
            {
                if (AlarmParameter_13_1 == null || AlarmParameter_13_2 == null || AlarmParameter_13_3 == null || AlarmParameter_13_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x0F;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes_4_Offset(AlarmParameter_13_1, AlarmParameter_13_2, AlarmParameter_13_3, AlarmParameter_13_4, 1, 1, 1, 1, -40, -40, -40, 0));
            }

            if (IsChecked_AlarmParameter_14)
            {
                if (AlarmParameter_14_1 == null || AlarmParameter_14_2 == null || AlarmParameter_14_3 == null || AlarmParameter_14_4 == null ||
                   AlarmParameter_14_5 == null || AlarmParameter_14_6 == null || AlarmParameter_14_7 == null || AlarmParameter_14_8 == null ||
                   AlarmParameter_14_9 == null || AlarmParameter_14_10 == null || AlarmParameter_14_11 == null || AlarmParameter_14_12 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x0A;


                for (int k = 1; k <= 3; k++)
                {
                    // 每次处理 4 个
                    string[] parameters = new string[]
                    {
                        GetAlarmParameter(k, 1), GetAlarmParameter(k, 2),
                        GetAlarmParameter(k, 3), GetAlarmParameter(k, 4)
                    };

                    //修正下设数据的分辨率
                    byte byte2 = (byte)Convert.ToUInt16(parameters[0]);
                    byte byte3 = (byte)Convert.ToUInt16(parameters[1]);
                    byte byte4 = (byte)Convert.ToUInt16(parameters[2]);
                    byte byte5 = (byte)Convert.ToUInt16(parameters[3]);
                    byte[] bytes = new byte[] { (byte)k, byte2, byte3, byte4, byte5, 0xFF, 0xFF, 0xFF };
                    canidList_14.Add(clonedCanid);
                    byteList_14.Add(bytes);

                }

                try
                {
                    for (int i = 0; i < byteList_14.Count; i++)
                    {
                        bool result = bmsOper.Send(byteList_14[i], canidList_14[i]);
                        //sendResult.Add(i.ToString(), result);
                    }

                }
                catch (Exception ex)
                {
                    MessageBoxHelper.Error(ex.Message, "提示", null, ButtonType.OK);

                }

                // 取对应的AlarmParameter值
                string GetAlarmParameter(int groupIndex, int parameterIndex)
                {
                    switch ((groupIndex - 1) * 4 + parameterIndex)
                    {
                        case 1: return AlarmParameter_14_1;
                        case 2: return AlarmParameter_14_2;
                        case 3: return AlarmParameter_14_3;
                        case 4: return AlarmParameter_14_4;
                        case 5: return AlarmParameter_14_5;
                        case 6: return AlarmParameter_14_6;
                        case 7: return AlarmParameter_14_7;
                        case 8: return AlarmParameter_14_8;
                        case 9: return AlarmParameter_14_9;
                        case 10: return AlarmParameter_14_10;
                        case 11: return AlarmParameter_14_11;
                        case 12: return AlarmParameter_14_12;
                        default: throw new ArgumentOutOfRangeException();
                    }
                }
            }

            if (IsChecked_AlarmParameter_15)
            {
                if (AlarmParameter_15_1 == null || AlarmParameter_15_2 == null || AlarmParameter_15_3 == null || AlarmParameter_15_4 == null)
                {
                    MessageBoxHelper.Warning("写入参数不能为空!", "警告", null, ButtonType.OK);
                    return;
                }
                var clonedCanid = (byte[])canid.Clone();
                clonedCanid[2] = 0x1C;
                canidList.Add(clonedCanid);
                byteList.Add(Uint16ToBytes_8_Offset(AlarmParameter_15_1, AlarmParameter_15_2, AlarmParameter_15_3, AlarmParameter_15_4, AlarmParameter_15_5, AlarmParameter_15_6, AlarmParameter_15_7, AlarmParameter_15_8, 1, 1, 1, 1, 1, 1, 1, 1, -40, -40, -40, 0, -40, -40, -40, 0));
            }

            // 发送指令
            try
            {
                for (int i = 0; i < byteList.Count; i++)
                {
                    bool result = bmsOper.Send(byteList[i], canidList[i]);
                    Thread.Sleep(50);
                    sendResult.Add(i.ToString(), result);
                }

            }
            catch (Exception ex)
            {
                MessageBoxHelper.Error(ex.Message, "提示", null, ButtonType.OK);

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




        byte[] Uint16ToBytes_4(string t1, string t2, string t3, string t4,
         double scaling1, double scaling2, double scaling3, double scaling4)
        {
            byte[] b1 = Uint16ToBytes(Convert.ToUInt32(float.Parse(t1) / scaling1));
            byte[] b2 = Uint16ToBytes(Convert.ToUInt32(float.Parse(t2) / scaling2));
            byte[] b3 = Uint16ToBytes(Convert.ToUInt32(float.Parse(t3) / scaling3));

            byte byte7 = (byte)(Convert.ToUInt32(float.Parse(t4) / scaling4));
            return new byte[] { b1[1], b1[0], b2[1], b2[0], b3[1], b3[0], byte7, 0xFF };//高字节在前
        }

        byte[] Uint16ToBytes_4_Offset(string t1, string t2, string t3, string t4,
         double scaling1, double scaling2, double scaling3, double scaling4, int offset1, int offset2, int offset3, int offset4)
        {
            byte byte1 = (byte)(Convert.ToUInt32(float.Parse(t1) / scaling1 - offset1));
            byte byte2 = (byte)(Convert.ToUInt32(float.Parse(t2) / scaling2 - offset2));
            byte byte3 = (byte)(Convert.ToUInt32(float.Parse(t3) / scaling3 - offset3));
            byte byte4 = (byte)(Convert.ToUInt32(float.Parse(t4) / scaling4 - offset4));
            return new byte[] { byte1, byte2, byte3, byte4, 0xFF, 0xFF, 0xFF, 0xFF };
        }
        byte[] Uint16ToBytes_8_Offset(string t1, string t2, string t3, string t4, string t5, string t6, string t7, string t8,
         double scaling1, double scaling2, double scaling3, double scaling4, double scaling5, double scaling6, double scaling7, double scaling8,
         int offset1, int offset2, int offset3, int offset4, int offset5, int offset6, int offset7, int offset8)
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
        byte[] Uint16ToBytes(uint ivalue)
        {
            byte[] data = new byte[2];
            data[1] = (byte)(ivalue >> 8);
            data[0] = (byte)(ivalue & 0xff);
            return data;
        }

        public BCU_ParamControl_ViewModel()
        {


            bmsOper = new CommandOperation(BMSConfig.ConfigManager);


            cts = new CancellationTokenSource();
            #region 主控报警参数
            if (keys.Count == 0)
            {
                //添加界面TextBox控件名称对应控件文本
                keys.Add("txtAlarmParameters_1_1", "组端总电压轻微报警下限值(V)");
                keys.Add("txtAlarmParameters_1_2", "组端总电压一般报警下限值(V)");
                keys.Add("txtAlarmParameters_1_3", "组端总电压严重报警下限值(V)");
                keys.Add("txtAlarmParameters_1_4", "组端总电压报警下限回差(V)");
                keys.Add("txtAlarmParameters_2_1", "组端总电压轻微报警上限值(V)");
                keys.Add("txtAlarmParameters_2_2", "组端总电压一般报警上限值(V)");
                keys.Add("txtAlarmParameters_2_3", "组端总电压严重报警上限值(V)");
                keys.Add("txtAlarmParameters_2_4", "组端总电压报警上限回差(V)");
                keys.Add("txtAlarmParameters_3_1", "电池单体电压轻微报警下限值(V)");
                keys.Add("txtAlarmParameters_3_2", "电池单体电压一般报警下限值(V)");
                keys.Add("txtAlarmParameters_3_3", "电池单体电压严重报警下限值(V)");
                keys.Add("txtAlarmParameters_3_4", "电池单体电压报警下限回差值(V)");
                keys.Add("txtAlarmParameters_4_1", "电池单体电压轻微报警上限值(V)");
                keys.Add("txtAlarmParameters_4_2", "电池单体电压一般报警上限值(V)");
                keys.Add("txtAlarmParameters_4_3", "电池单体电压严重报警上限值(V)");
                keys.Add("txtAlarmParameters_4_4", "电池单体电压报警上限回差值(V)");
                keys.Add("txtAlarmParameters_5_1", "电池单体电压压差轻微报警上限值(V)");
                keys.Add("txtAlarmParameters_5_2", "电池单体电压压差一般报警上限值(V)");
                keys.Add("txtAlarmParameters_5_3", "电池单体电压压差严重报警上限值(V)");
                keys.Add("txtAlarmParameters_5_4", "电池单体电压压差报警上限回差值(V)");
                keys.Add("txtAlarmParameters_6_1", "单体电池温差轻微报警下限值(℃)");
                keys.Add("txtAlarmParameters_6_2", "单体电池温差一般报警下限值(℃)");
                keys.Add("txtAlarmParameters_6_3", "单体电池温差严重报警下限值(℃)");
                keys.Add("txtAlarmParameters_6_4", "单体电池温差报警回差值(℃)");
                keys.Add("txtAlarmParameters_7_1", "绝缘电阻正负对地电阻轻微报警值(KΩ)");
                keys.Add("txtAlarmParameters_7_2", "绝缘电阻正负对地电阻一般报警值(KΩ)");
                keys.Add("txtAlarmParameters_7_3", "绝缘电阻正负对地电阻严重报警值(KΩ)");
                keys.Add("txtAlarmParameters_7_4", "绝缘电阻正负对地电阻回差值(KΩ)");
                keys.Add("txtAlarmParameters_8_1", "电池模组电压轻微报警下限值(V)");
                keys.Add("txtAlarmParameters_8_2", "电池模组电压一般报警下限值(V)");
                keys.Add("txtAlarmParameters_8_3", "电池模组电压严重报警下限值(V)");
                keys.Add("txtAlarmParameters_8_4", "电池模组电压报警下限回差值(V)");
                keys.Add("txtAlarmParameters_9_1", "电池模组电压轻微报警上限值(V)");
                keys.Add("txtAlarmParameters_9_2", "电池模组电压一般报警上限值(V)");
                keys.Add("txtAlarmParameters_9_3", "电池模组电压严重报警上限值(V)");
                keys.Add("txtAlarmParameters_9_4", "电池模组电压报警上限回差值(V)");
                keys.Add("txtAlarmParameters_10_1", "放电电流过流轻微报警值(A)");
                keys.Add("txtAlarmParameters_10_2", "放电电流过流一般报警值(A)");
                keys.Add("txtAlarmParameters_10_3", "放电电流过流严重报警值(A)");
                keys.Add("txtAlarmParameters_10_4", "放电电流过流报警回差值(A)");
                keys.Add("txtAlarmParameters_11_1", "充电电流过流轻微报警值(A)");
                keys.Add("txtAlarmParameters_11_2", "充电电流过流一般报警值(A)");
                keys.Add("txtAlarmParameters_11_3", "充电电流过流严重报警值(A)");
                keys.Add("txtAlarmParameters_11_4", "充电电流过流报警回差值(A)");
                keys.Add("txtAlarmParameters_11_5", "功率端子过温轻微报警值(℃)");
                keys.Add("txtAlarmParameters_11_6", "功率端子过温一般报警值(℃)");
                keys.Add("txtAlarmParameters_11_7", "功率端子过温严重报警值(℃)");
                keys.Add("txtAlarmParameters_11_8", "功率端子过温报警回差值(℃)");
                keys.Add("txtAlarmParameters_11_9", "馈电充电电流过流轻微报警值(A)");
                keys.Add("txtAlarmParameters_11_10", "馈电充电电流过流一般报警值(A)");
                keys.Add("txtAlarmParameters_11_11", "馈电充电电流过流严重报警值(A)");
                keys.Add("txtAlarmParameters_11_12", "馈电充电电流过流报警回差值(A)");
                keys.Add("txtAlarmParameters_12_1", "充电单体电池温度轻微报警上限值(℃)");
                keys.Add("txtAlarmParameters_12_2", "充电单体电池温度一般报警上限值(℃)");
                keys.Add("txtAlarmParameters_12_3", "充电单体电池温度严重报警上限值(℃)");
                keys.Add("txtAlarmParameters_12_4", "充电单体电池温度报警上限回差值(℃)");
                keys.Add("txtAlarmParameters_12_5", "充电单体电池温度轻微报警下限值(℃)");
                keys.Add("txtAlarmParameters_12_6", "充电单体电池温度一般报警下限值(℃)");
                keys.Add("txtAlarmParameters_12_7", "充电单体电池温度严重报警下限值(℃)");
                keys.Add("txtAlarmParameters_12_8", "充电单体电池温度报警下限回差值(℃)");
                keys.Add("txtAlarmParameters_13_1", "模块温度轻微报警上限值(℃)");
                keys.Add("txtAlarmParameters_13_2", "模块温度一般报警上限值(℃)");
                keys.Add("txtAlarmParameters_13_3", "模块温度严重报警上限值(℃)");
                keys.Add("txtAlarmParameters_13_4", "模块温度报警回差值(℃)");
                keys.Add("txtAlarmParameters_14_1", "SOC轻微报警下限值(%)");
                keys.Add("txtAlarmParameters_14_2", "SOC一般报警下限值(%)");
                keys.Add("txtAlarmParameters_14_3", "SOC严重报警下限值(%)");
                keys.Add("txtAlarmParameters_14_4", "SOC报警下限回差值(%)");
                keys.Add("txtAlarmParameters_14_5", "SOC轻微报警上限值(%)");
                keys.Add("txtAlarmParameters_14_6", "SOC一般报警上限值(%)");
                keys.Add("txtAlarmParameters_14_7", "SOC严重报警上限值(%)");
                keys.Add("txtAlarmParameters_14_8", "SOC报警上限回差值(%)");
                keys.Add("txtAlarmParameters_14_9", "SOC差异轻微报警值(%)");
                keys.Add("txtAlarmParameters_14_10", "SOC差异一般报警值(%)");
                keys.Add("txtAlarmParameters_14_11", "SOC差异严重报警值(%)");
                keys.Add("txtAlarmParameters_14_12", "SOC差异报警回差值(%)");
                keys.Add("txtAlarmParameters_15_1", "放电单体电池温度轻微报警上限值(℃)");
                keys.Add("txtAlarmParameters_15_2", "放电单体电池温度一般报警上限值(℃)");
                keys.Add("txtAlarmParameters_15_3", "放电单体电池温度严重报警上限值(℃)");
                keys.Add("txtAlarmParameters_15_4", "放电单体电池温度上限回差值(℃)");
                keys.Add("txtAlarmParameters_15_5", "放电单体电池温度轻微报警下限值(℃)");
                keys.Add("txtAlarmParameters_15_6", "放电单体电池温度一般报警下限值(℃)");
                keys.Add("txtAlarmParameters_15_7", "放电单体电池温度严重报警下限值(℃)");
                keys.Add("txtAlarmParameters_15_8", "放电单体电池温度下限回差值(℃)");
            }
            #endregion


        }
        public static bool IsShowMessage = true;
        public void Load()
        {

            if (bmsOper.IsConnection)
            {
                IsShowMessage = false;
                bmsOper.ReadBCUParam(Convert.ToInt32(SelectedAddress_BCU, 16));
            }


            Task.Run(async delegate
            {
                while (!cts.IsCancellationRequested)
                {

                    if (bmsOper.CommunicationType == "Ecan")
                    {
                        lock (EcanHelper._locker)
                        {
                            while (EcanHelper._task.Count > 0
                                && !cts.IsCancellationRequested)
                            {
                                CAN_OBJ ch = (CAN_OBJ)EcanHelper._task.Dequeue();

                                Application.Current.Dispatcher.Invoke(() => { analysisData(ch.ID, ch.Data); });
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

                                Application.Current.Dispatcher.Invoke(() => { analysisData(ch.ID, ch.Data); });
                                //Log.Info($"{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")} 接收CAN数据:{BitConverter.ToString(ch.Data).Replace("-", " ")}  帧ID:{ch.ID.ToString("X8")}");

                            }
                        }
                    }


                    await Task.Delay(500);
                }
            }, cts.Token);
        }
        public void CancelOperation()
        {
            if (cts != null)
            {
                cts.Cancel();
                cts.Dispose();
            }
        }
        public void analysisData(uint canID, byte[] data)
        {
            byte Address_BCU = Convert.ToByte(Convert.ToInt32(SelectedAddress_BCU, 16));
            //例如 查询组端电压上限告警值
            //下发： CANID: 0x181FE8F4 0x04
            //主控回复 CANID:0x1804F4E8 0x01 0xF4 0x02 0x58 0x02 0xBC 0x32 0xFF
            byte[] canid = BitConverter.GetBytes(canID);
            if (canid[0] != Address_BCU || !(canid[0] == Address_BCU && canid[1] == 0xF4 && canid[3] == 0x18)) return;

            Parameters.Clear();
            Parameters.Add(new ParamViewModel { ControlName = "组端总电压轻微报警下限值(V)", Value = AlarmParameter_1_1 });
            Parameters.Add(new ParamViewModel { ControlName = "组端总电压一般报警下限值(V)", Value = AlarmParameter_1_2 });
            Parameters.Add(new ParamViewModel { ControlName = "组端总电压严重报警下限值(V)", Value = AlarmParameter_1_3 });
            Parameters.Add(new ParamViewModel { ControlName = "组端总电压报警下限回差(V)", Value = AlarmParameter_1_4 });
            Parameters.Add(new ParamViewModel { ControlName = "组端总电压轻微报警上限值(V)", Value = AlarmParameter_2_1 });
            Parameters.Add(new ParamViewModel { ControlName = "组端总电压一般报警上限值(V)", Value = AlarmParameter_2_2 });
            Parameters.Add(new ParamViewModel { ControlName = "组端总电压严重报警上限值(V)", Value = AlarmParameter_2_3 });
            Parameters.Add(new ParamViewModel { ControlName = "组端总电压报警上限回差(V)", Value = AlarmParameter_2_4 });
            Parameters.Add(new ParamViewModel { ControlName = "电池单体电压轻微报警下限值(V)", Value = AlarmParameter_3_1 });
            Parameters.Add(new ParamViewModel { ControlName = "电池单体电压一般报警下限值(V)", Value = AlarmParameter_3_2 });
            Parameters.Add(new ParamViewModel { ControlName = "电池单体电压严重报警下限值(V)", Value = AlarmParameter_3_3 });
            Parameters.Add(new ParamViewModel { ControlName = "电池单体电压报警下限回差值(V)", Value = AlarmParameter_3_4 });
            Parameters.Add(new ParamViewModel { ControlName = "电池单体电压轻微报警上限值(V)", Value = AlarmParameter_4_1 });
            Parameters.Add(new ParamViewModel { ControlName = "电池单体电压一般报警上限值(V)", Value = AlarmParameter_4_2 });
            Parameters.Add(new ParamViewModel { ControlName = "电池单体电压严重报警上限值(V)", Value = AlarmParameter_4_3 });
            Parameters.Add(new ParamViewModel { ControlName = "电池单体电压报警上限回差值(V)", Value = AlarmParameter_4_4 });
            Parameters.Add(new ParamViewModel { ControlName = "电池单体电压压差轻微报警上限值(V)", Value = AlarmParameter_5_1 });
            Parameters.Add(new ParamViewModel { ControlName = "电池单体电压压差一般报警上限值(V)", Value = AlarmParameter_5_2 });
            Parameters.Add(new ParamViewModel { ControlName = "电池单体电压压差严重报警上限值(V)", Value = AlarmParameter_5_3 });
            Parameters.Add(new ParamViewModel { ControlName = "电池单体电压压差报警上限回差值(V)", Value = AlarmParameter_5_4 });
            Parameters.Add(new ParamViewModel { ControlName = "单体电池温差轻微报警下限值(℃)", Value = AlarmParameter_6_1 });
            Parameters.Add(new ParamViewModel { ControlName = "单体电池温差一般报警下限值(℃)", Value = AlarmParameter_6_2 });
            Parameters.Add(new ParamViewModel { ControlName = "单体电池温差严重报警下限值(℃)", Value = AlarmParameter_6_3 });
            Parameters.Add(new ParamViewModel { ControlName = "单体电池温差报警回差值(℃)", Value = AlarmParameter_6_4 });
            Parameters.Add(new ParamViewModel { ControlName = "绝缘电阻正负对地电阻轻微报警值(KΩ)", Value = AlarmParameter_7_1 });
            Parameters.Add(new ParamViewModel { ControlName = "绝缘电阻正负对地电阻一般报警值(KΩ)", Value = AlarmParameter_7_2 });
            Parameters.Add(new ParamViewModel { ControlName = "绝缘电阻正负对地电阻严重报警值(KΩ)", Value = AlarmParameter_7_3 });
            Parameters.Add(new ParamViewModel { ControlName = "绝缘电阻正负对地电阻回差值(KΩ)", Value = AlarmParameter_7_4 });
            Parameters.Add(new ParamViewModel { ControlName = "电池模组电压轻微报警下限值(V)", Value = AlarmParameter_8_1 });
            Parameters.Add(new ParamViewModel { ControlName = "电池模组电压一般报警下限值(V)", Value = AlarmParameter_8_2 });
            Parameters.Add(new ParamViewModel { ControlName = "电池模组电压严重报警下限值(V)", Value = AlarmParameter_8_3 });
            Parameters.Add(new ParamViewModel { ControlName = "电池模组电压报警下限回差值(V)", Value = AlarmParameter_8_4 });
            Parameters.Add(new ParamViewModel { ControlName = "电池模组电压轻微报警上限值(V)", Value = AlarmParameter_9_1 });
            Parameters.Add(new ParamViewModel { ControlName = "电池模组电压一般报警上限值(V)", Value = AlarmParameter_9_2 });
            Parameters.Add(new ParamViewModel { ControlName = "电池模组电压严重报警上限值(V)", Value = AlarmParameter_9_3 });
            Parameters.Add(new ParamViewModel { ControlName = "电池模组电压报警上限回差值(V)", Value = AlarmParameter_9_4 });
            Parameters.Add(new ParamViewModel { ControlName = "放电电流过流轻微报警值(A)", Value = AlarmParameter_10_1 });
            Parameters.Add(new ParamViewModel { ControlName = "放电电流过流一般报警值(A)", Value = AlarmParameter_10_2 });
            Parameters.Add(new ParamViewModel { ControlName = "放电电流过流严重报警值(A)", Value = AlarmParameter_10_3 });
            Parameters.Add(new ParamViewModel { ControlName = "放电电流过流报警回差值(A)", Value = AlarmParameter_10_4 });
            Parameters.Add(new ParamViewModel { ControlName = "充电电流过流轻微报警值(A)", Value = AlarmParameter_11_1 });
            Parameters.Add(new ParamViewModel { ControlName = "充电电流过流一般报警值(A)", Value = AlarmParameter_11_2 });
            Parameters.Add(new ParamViewModel { ControlName = "充电电流过流严重报警值(A)", Value = AlarmParameter_11_3 });
            Parameters.Add(new ParamViewModel { ControlName = "充电电流过流报警回差值(A)", Value = AlarmParameter_11_4 });
            Parameters.Add(new ParamViewModel { ControlName = "功率端子过温轻微报警值(℃)", Value = AlarmParameter_11_5 });
            Parameters.Add(new ParamViewModel { ControlName = "功率端子过温一般报警值(℃)", Value = AlarmParameter_11_6 });
            Parameters.Add(new ParamViewModel { ControlName = "功率端子过温严重报警值(℃)", Value = AlarmParameter_11_7 });
            Parameters.Add(new ParamViewModel { ControlName = "功率端子过温报警回差值(℃)", Value = AlarmParameter_11_8 });
            Parameters.Add(new ParamViewModel { ControlName = "馈电充电电流过流轻微报警值(A)", Value = AlarmParameter_11_9 });
            Parameters.Add(new ParamViewModel { ControlName = "馈电充电电流过流一般报警值(A)", Value = AlarmParameter_11_10 });
            Parameters.Add(new ParamViewModel { ControlName = "馈电充电电流过流严重报警值(A)", Value = AlarmParameter_11_11 });
            Parameters.Add(new ParamViewModel { ControlName = "馈电充电电流过流报警回差值(A)", Value = AlarmParameter_11_12 });
            Parameters.Add(new ParamViewModel { ControlName = "充电单体电池温度轻微报警上限值(℃)", Value = AlarmParameter_12_1 });
            Parameters.Add(new ParamViewModel { ControlName = "充电单体电池温度一般报警上限值(℃)", Value = AlarmParameter_12_2 });
            Parameters.Add(new ParamViewModel { ControlName = "充电单体电池温度严重报警上限值(℃)", Value = AlarmParameter_12_3 });
            Parameters.Add(new ParamViewModel { ControlName = "充电单体电池温度报警上限回差值(℃)", Value = AlarmParameter_12_4 });
            Parameters.Add(new ParamViewModel { ControlName = "充电单体电池温度轻微报警下限值(℃)", Value = AlarmParameter_12_5 });
            Parameters.Add(new ParamViewModel { ControlName = "充电单体电池温度一般报警下限值(℃)", Value = AlarmParameter_12_6 });
            Parameters.Add(new ParamViewModel { ControlName = "充电单体电池温度严重报警下限值(℃)", Value = AlarmParameter_12_7 });
            Parameters.Add(new ParamViewModel { ControlName = "充电单体电池温度报警下限回差值(℃)", Value = AlarmParameter_12_8 });
            Parameters.Add(new ParamViewModel { ControlName = "模块温度轻微报警上限值(℃)", Value = AlarmParameter_13_1 });
            Parameters.Add(new ParamViewModel { ControlName = "模块温度一般报警上限值(℃)", Value = AlarmParameter_13_2 });
            Parameters.Add(new ParamViewModel { ControlName = "模块温度严重报警上限值(℃)", Value = AlarmParameter_13_3 });
            Parameters.Add(new ParamViewModel { ControlName = "模块温度报警回差值(℃)", Value = AlarmParameter_13_4 });
            Parameters.Add(new ParamViewModel { ControlName = "SOC轻微报警下限值(%)", Value = AlarmParameter_14_1 });
            Parameters.Add(new ParamViewModel { ControlName = "SOC一般报警下限值(%)", Value = AlarmParameter_14_2 });
            Parameters.Add(new ParamViewModel { ControlName = "SOC严重报警下限值(%)", Value = AlarmParameter_14_3 });
            Parameters.Add(new ParamViewModel { ControlName = "SOC报警下限回差值(%)", Value = AlarmParameter_14_4 });
            Parameters.Add(new ParamViewModel { ControlName = "SOC轻微报警上限值(%)", Value = AlarmParameter_14_5 });
            Parameters.Add(new ParamViewModel { ControlName = "SOC一般报警上限值(%)", Value = AlarmParameter_14_6 });
            Parameters.Add(new ParamViewModel { ControlName = "SOC严重报警上限值(%)", Value = AlarmParameter_14_7 });
            Parameters.Add(new ParamViewModel { ControlName = "SOC报警上限回差值(%)", Value = AlarmParameter_14_8 });
            Parameters.Add(new ParamViewModel { ControlName = "SOC差异轻微报警值(%)", Value = AlarmParameter_14_9 });
            Parameters.Add(new ParamViewModel { ControlName = "SOC差异一般报警值(%)", Value = AlarmParameter_14_10 });
            Parameters.Add(new ParamViewModel { ControlName = "SOC差异严重报警值(%)", Value = AlarmParameter_14_11 });
            Parameters.Add(new ParamViewModel { ControlName = "SOC差异报警回差值(%)", Value = AlarmParameter_14_12 });
            Parameters.Add(new ParamViewModel { ControlName = "放电单体电池温度轻微报警上限值(℃)", Value = AlarmParameter_15_1 });
            Parameters.Add(new ParamViewModel { ControlName = "放电单体电池温度一般报警上限值(℃)", Value = AlarmParameter_15_2 });
            Parameters.Add(new ParamViewModel { ControlName = "放电单体电池温度严重报警上限值(℃)", Value = AlarmParameter_15_3 });
            Parameters.Add(new ParamViewModel { ControlName = "放电单体电池温度上限回差值(℃)", Value = AlarmParameter_15_4 });
            Parameters.Add(new ParamViewModel { ControlName = "放电单体电池温度轻微报警下限值(℃)", Value = AlarmParameter_15_5 });
            Parameters.Add(new ParamViewModel { ControlName = "放电单体电池温度一般报警下限值(℃)", Value = AlarmParameter_15_6 });
            Parameters.Add(new ParamViewModel { ControlName = "放电单体电池温度严重报警下限值(℃)", Value = AlarmParameter_15_7 });
            Parameters.Add(new ParamViewModel { ControlName = "放电单体电池温度下限回差值(℃)", Value = AlarmParameter_15_8 });

            switch (canid[2])
            {
                //从控模块基本参数（命令码 0x14）
                case 0x14:
                    if (data[0] == 1)
                    {
                        ConstantDef.BCU_ModuleNumber = Convert.ToInt32(data[1]);
                    }
                    break;
                //case 0x00:
                //    if (data[0] == 4)
                //    {
                //        var ProductName = "PowerMagic " + Encoding.ASCII.GetString(data.Skip(1).Take(1).ToArray())
                //            + "." + Encoding.ASCII.GetString(data.Skip(2).Take(1).ToArray());

                //        if (ProductName == "PowerMagic 1.0")
                //        {
                //            ConstantDef.BatteryCellCount = 48;
                //            ConstantDef.BatteryTemperatureCount = 28;

                //        }
                //        else
                //        {
                //            ConstantDef.BatteryCellCount = 64;
                //            ConstantDef.BatteryTemperatureCount = 36;

                //        }
                //    }
                //    break;
                case 0x04://组端总电压上限报警值                       
                    AlarmParameter_2_1 = ((data[0] << 8 | data[1]) * 0.1).ToString("F1"); // 组端总电压轻微报警上限值  0.1V/bit 偏移量： 0 范围：0~800V
                    AlarmParameter_2_2 = ((data[2] << 8 | data[3]) * 0.1).ToString("F1"); // 组端总电压一般报警上限值  0.1V/bit 偏移量： 0 范围：0~800V
                    AlarmParameter_2_3 = ((data[4] << 8 | data[5]) * 0.1).ToString("F1"); // 组端总电压严重报警上限值  0.1V/bit 偏移量： 0 范围：0~800V
                    AlarmParameter_2_4 = (data[6] * 0.1).ToString();                      // 组端总电压报警上限回差    0.1V/bit 偏移量： 0 范围：0~25.5V                  
                    break;
                case 0x05://组端总电压下限报警值                                                                                          
                    AlarmParameter_1_1 = ((data[0] << 8 | data[1]) * 0.1).ToString("F1"); // 组端总电压轻微报警下限值  0.1V/bit 偏移量： 0 范围：0~800V
                    AlarmParameter_1_2 = ((data[2] << 8 | data[3]) * 0.1).ToString("F1"); // 组端总电压一般报警下限值  0.1V/bit 偏移量： 0 范围：0~800V
                    AlarmParameter_1_3 = ((data[4] << 8 | data[5]) * 0.1).ToString("F1"); // 组端总电压严重报警下限值  0.1V/bit 偏移量： 0 范围：0~800V
                    AlarmParameter_1_4 = (data[6] * 0.1).ToString();                      // 组端总电压报警下限回差    0.1V/bit 偏移量： 0 范围：0~25.5V
                    break;
                case 0x06://充电电流报警值
                    uint chargingType = data[0];//充电类型 1:快充 2:慢充/极柱过温 3:馈电
                    switch (chargingType)
                    {
                        case 1:
                            AlarmParameter_11_1 = ((data[1] << 8 | data[2]) * 0.1).ToString("F1");  // 充电电流过流轻微报警值  0.1A/bit 偏移量： 0 范围：0~500A
                            AlarmParameter_11_2 = ((data[3] << 8 | data[4]) * 0.1).ToString("F1");  // 充电电流过流一般报警值  0.1A/bit 偏移量： 0 范围：0~500A
                            AlarmParameter_11_3 = ((data[5] << 8 | data[6]) * 0.1).ToString("F1");  // 充电电流过流严重报警值  0.1A/bit 偏移量： 0 范围：0~500A
                            AlarmParameter_11_4 = (data[7] * 0.1).ToString();                       // 充电电流过流报警回差值  0.1A/bit 偏移量： 0 范围：0~25.5A
                            break;
                        case 2:
                            AlarmParameter_11_5 = ((data[1] << 8 | data[2]) * 0.1).ToString("F1");  // 功率端子过温轻微报警值  0.1℃/bit 偏移量： 0 ->替换慢充电流过流
                            AlarmParameter_11_6 = ((data[3] << 8 | data[4]) * 0.1).ToString("F1");  // 功率端子过温一般报警值  0.1℃/bit 偏移量： 0 ->替换慢充电流过流
                            AlarmParameter_11_7 = ((data[5] << 8 | data[6]) * 0.1).ToString("F1");  // 功率端子过温严重报警值  0.1℃/bit 偏移量： 0 ->替换慢充电流过流
                            AlarmParameter_11_8 = (data[7] * 0.1).ToString();                       // 功率端子过温报警回差值  0.1℃/bit 偏移量： 0 ->替换慢充电流过流
                            break;
                        case 3:
                            AlarmParameter_11_9 = ((data[1] << 8 | data[2]) * 0.1).ToString("F1"); // 馈电充电电流过流轻微报警值  0.1A/bit 偏移量： 0 范围：0~500A
                            AlarmParameter_11_10 = ((data[3] << 8 | data[4]) * 0.1).ToString("F1"); // 馈电充电电流过流一般报警值  0.1A/bit 偏移量： 0 范围：0~500A
                            AlarmParameter_11_11 = ((data[5] << 8 | data[6]) * 0.1).ToString("F1"); // 馈电充电电流过流严重报警值  0.1A/bit 偏移量： 0 范围：0~500A
                            AlarmParameter_11_12 = (data[7] * 0.1).ToString();                      // 馈电充电电流过流报警回差值  0.1A/bit 偏移量： 0 范围：0~25.5A
                            break;
                    }
                    break;
                case 0x07://放电电流报警值
                    AlarmParameter_10_1 = ((data[0] << 8 | data[1]) * 0.1).ToString("F1"); // 放电电流过流轻微报警下限值  0.1A/bit 偏移量： 0 范围：0~500A
                    AlarmParameter_10_2 = ((data[2] << 8 | data[3]) * 0.1).ToString("F1"); // 放电电流过流一般报警下限值  0.1A/bit 偏移量： 0 范围：0~500A
                    AlarmParameter_10_3 = ((data[4] << 8 | data[5]) * 0.1).ToString("F1"); // 放电电流过流严重报警下限值  0.1A/bit 偏移量： 0 范围：0~500A
                    AlarmParameter_10_4 = (data[6] * 0.1).ToString();                      // 放电电流过流报警回差值      0.1A/bit 偏移量： 0 范围：0~25.5A
                    break;
                case 0x08://充电单体电池温度报警值
                    AlarmParameter_12_1 = (data[0] - 40).ToString(); // 充电单体电池温度轻微报警上限值  1℃/bit 范围：0℃~140℃(-40~100) 偏移量： -40
                    AlarmParameter_12_2 = (data[1] - 40).ToString(); // 充电单体电池温度一般报警上限值  1℃/bit 范围：0℃~140℃(-40~100) 偏移量： -40
                    AlarmParameter_12_3 = (data[2] - 40).ToString(); // 充电单体电池温度严重报警上限值  1℃/bit 范围：0℃~140℃(-40~100) 偏移量： -40
                    AlarmParameter_12_4 = (data[3]).ToString();      // 充电单体电池温度上限报警回差值  1℃/bit 无偏差
                    AlarmParameter_12_5 = (data[4] - 40).ToString(); // 充电单体电池温度轻微报警下限值  1℃/bit 范围：0℃~140℃(-40~100) 偏移量： -40
                    AlarmParameter_12_6 = (data[5] - 40).ToString(); // 充电单体电池温度一般报警下限值  1℃/bit 范围：0℃~140℃(-40~100) 偏移量： -40
                    AlarmParameter_12_7 = (data[6] - 40).ToString(); // 充电单体电池温度严重报警下限值  1℃/bit 范围：0℃~140℃(-40~100) 偏移量： -40
                    AlarmParameter_12_8 = (data[7]).ToString();      // 充电单体电池温度上限报警回差值  1℃/bit 无偏差
                    break;
                case 0x09://单体电池温差报警值
                    AlarmParameter_6_1 = data[0].ToString(); // 单体电池温差轻微报警下限值  1℃/bit 范围：0℃~140℃(-40~100) 偏移量： -40->修改为0 无偏差
                    AlarmParameter_6_2 = data[1].ToString(); // 单体电池温差一般报警下限值  1℃/bit 范围：0℃~140℃(-40~100) 偏移量： -40->修改为0 无偏差
                    AlarmParameter_6_3 = data[2].ToString(); // 单体电池温差严重报警下限值  1℃/bit 范围：0℃~140℃(-40~100) 偏移量： -40->修改为0 无偏差
                    AlarmParameter_6_4 = data[3].ToString(); // 单体电池温差严重报警回差值  1℃/bit 无偏差
                    break;

                case 0x0A:// SOC报警值
                    uint alarmType = (uint)data[0];//SOC告警类型 1:下限值 2:上限值 3:差异值
                    switch (alarmType)
                    {
                        case 1:
                            AlarmParameter_14_1 = data[1].ToString();  // SOC轻微报警下限值  1/bit 范围：0-100
                            AlarmParameter_14_2 = data[2].ToString();  // SOC一般报警下限值  1/bit 范围：0-100
                            AlarmParameter_14_3 = data[3].ToString();  // SOC严重报警下限值  1/bit 范围：0-100
                            AlarmParameter_14_4 = data[4].ToString();  // SOC报警下限回差值  1/bit 范围：0-100
                            break;
                        case 2:
                            AlarmParameter_14_5 = data[1].ToString();  // SOC轻微报警上限值  1/bit 范围：0-100
                            AlarmParameter_14_6 = data[2].ToString();  // SOC一般报警上限值  1/bit 范围：0-100
                            AlarmParameter_14_7 = data[3].ToString();  // SOC严重报警上限值  1/bit 范围：0-100
                            AlarmParameter_14_8 = data[4].ToString();  // SOC报警上限回差值  1/bit 范围：0-100
                            break;
                        case 3:
                            AlarmParameter_14_9 = data[1].ToString();  // SOC差异轻微报警值  1/bit 范围：0-100
                            AlarmParameter_14_10 = data[2].ToString();  // SOC差异一般报警值  1/bit 范围：0-100
                            AlarmParameter_14_11 = data[3].ToString();  // SOC差异严重报警值  1/bit 范围：0-100
                            AlarmParameter_14_12 = data[4].ToString();  // SOC差异报警回差值  1/bit 范围：0-100
                            break;
                    }
                    break;
                case 0x0B:// 绝缘电阻报警值
                    AlarmParameter_7_1 = ((data[0] << 8 | data[1])).ToString(); // 绝缘电阻正负对地电阻轻微报警值 1KΩ/bit 范围：0--FFFFH
                    AlarmParameter_7_2 = ((data[2] << 8 | data[3])).ToString(); // 绝缘电阻正负对地电阻一般报警值 1KΩ/bit 范围：0--FFFFH
                    AlarmParameter_7_3 = ((data[4] << 8 | data[5])).ToString(); // 绝缘电阻正负对地电阻严重报警值 1KΩ/bit 范围：0--FFFFH
                    AlarmParameter_7_4 = (data[6]).ToString();                  // 绝缘电阻正负对地电阻回差值     1KΩ/bit 范围：0--FFFFH
                    break;
                case 0x0C:// 电池单体电压上限报警值
                    AlarmParameter_4_1 = ((data[0] << 8 | data[1]) * 0.001).ToString("F3"); // 电池单体电压轻微报警上限值 0.001V/bit 偏移量：0
                    AlarmParameter_4_2 = ((data[2] << 8 | data[3]) * 0.001).ToString("F3"); // 电池单体电压一般报警上限值 0.001V/bit 偏移量：0
                    AlarmParameter_4_3 = ((data[4] << 8 | data[5]) * 0.001).ToString("F3"); // 电池单体电压严重报警上限值 0.001V/bit 偏移量：0
                    AlarmParameter_4_4 = (data[6] * 0.001).ToString("F3");                  // 电池单体电压报警上限回差值 0.001V/bit 偏移量：0
                    break;
                case 0x0D:// 电池单体电压下限报警值
                    AlarmParameter_3_1 = ((data[0] << 8 | data[1]) * 0.001).ToString("F3"); // 电池单体电压轻微报警下限值 0.001V/bit 偏移量：0
                    AlarmParameter_3_2 = ((data[2] << 8 | data[3]) * 0.001).ToString("F3"); // 电池单体电压一般报警下限值 0.001V/bit 偏移量：0
                    AlarmParameter_3_3 = ((data[4] << 8 | data[5]) * 0.001).ToString("F3"); // 电池单体电压严重报警下限值 0.001V/bit 偏移量：0
                    AlarmParameter_3_4 = (data[6] * 0.001).ToString("F3");                  // 电池单体电压报警下限回差值 0.001V/bit 偏移量：0
                    break;
                case 0x0E:// 电池单体电压压差报警值
                    AlarmParameter_5_1 = ((data[0] << 8 | data[1]) * 0.001).ToString("F3"); // 电池单体电压压差轻微报警值 0.001V/bit 偏移量：0
                    AlarmParameter_5_2 = ((data[2] << 8 | data[3]) * 0.001).ToString("F3"); // 电池单体电压压差一般报警值 0.001V/bit 偏移量：0
                    AlarmParameter_5_3 = ((data[4] << 8 | data[5]) * 0.001).ToString("F3"); // 电池单体电压压差严重报警值 0.001V/bit 偏移量：0
                    AlarmParameter_5_4 = (data[6] * 0.001).ToString("F3");                  // 电池单体电压压差报警回差值 0.001V/bit 偏移量：0
                    break;
                case 0x0F:// 模块温度上限报警值
                    AlarmParameter_13_1 = (data[0] - 40).ToString(); // 模块温度轻微报警上限值  1℃/bit 范围：0℃~140℃(-40~100) 偏移量： -40 
                    AlarmParameter_13_2 = (data[1] - 40).ToString(); // 模块温度一般报警上限值  1℃/bit 范围：0℃~140℃(-40~100) 偏移量： -40 
                    AlarmParameter_13_3 = (data[2] - 40).ToString(); // 模块温度严重报警上限值  1℃/bit 范围：0℃~140℃(-40~100) 偏移量： -40 
                    AlarmParameter_13_4 = data[3].ToString();      // 模块温度报警回差值      1℃/bit 无偏差
                    break;
                case 0x1C:// 放电单体电池温度报警值
                    AlarmParameter_15_1 = (data[0] - 40).ToString(); // 放电单体电池温度轻微报警上限值  1℃/bit 范围：0℃~140℃(-40~100) 偏移量： -40
                    AlarmParameter_15_2 = (data[1] - 40).ToString(); // 放电单体电池温度一般报警上限值  1℃/bit 范围：0℃~140℃(-40~100) 偏移量： -40
                    AlarmParameter_15_3 = (data[2] - 40).ToString(); // 放电单体电池温度严重报警上限值  1℃/bit 范围：0℃~140℃(-40~100) 偏移量： -40
                    AlarmParameter_15_4 = data[3].ToString();        // 放电单体电池温度上限报警回差值  1℃/bit 无偏差
                    AlarmParameter_15_5 = (data[4] - 40).ToString(); // 放电单体电池温度轻微报警下限值  1℃/bit 范围：0℃~140℃(-40~100) 偏移量： -40
                    AlarmParameter_15_6 = (data[5] - 40).ToString(); // 放电单体电池温度一般报警下限值  1℃/bit 范围：0℃~140℃(-40~100) 偏移量： -40
                    AlarmParameter_15_7 = (data[6] - 40).ToString(); // 放电单体电池温度严重报警下限值  1℃/bit 范围：0℃~140℃(-40~100) 偏移量： -40
                    AlarmParameter_15_8 = data[7].ToString();        // 放电单体电池温度下限报警回差值  1℃/bit 无偏差
                    break;
                case 0x2B:// 电池模组电压上限报警值
                    AlarmParameter_9_1 = ((data[0] << 8 | data[1]) * 0.1).ToString("F1"); // 电池模组电压轻微报警上限值 0.1V/bit 偏移量：0
                    AlarmParameter_9_2 = ((data[2] << 8 | data[3]) * 0.1).ToString("F1"); // 电池模组电压一般报警上限值 0.1V/bit 偏移量：0
                    AlarmParameter_9_3 = ((data[4] << 8 | data[5]) * 0.1).ToString("F1"); // 电池模组电压严重报警上限值 0.1V/bit 偏移量：0
                    AlarmParameter_9_4 = (data[6] * 0.1).ToString();                      // 电池模组电压报警上限回差值 0.1V/bit 偏移量：0
                    break;
                case 0x2C:// 电池模组电压下限报警值
                    AlarmParameter_8_1 = ((data[0] << 8 | data[1]) * 0.1).ToString("F1"); // 电池模组电压轻微报警下限值 0.1V/bit 偏移量：0
                    AlarmParameter_8_2 = ((data[2] << 8 | data[3]) * 0.1).ToString("F1"); // 电池模组电压一般报警下限值 0.1V/bit 偏移量：0
                    AlarmParameter_8_3 = ((data[4] << 8 | data[5]) * 0.1).ToString("F1"); // 电池模组电压严重报警下限值 0.1V/bit 偏移量：0
                    AlarmParameter_8_4 = (data[6] * 0.1).ToString();                      // 电池模组电压报警下限回差值 0.1V/bit 偏移量：0

                    if (IsShowMessage)
                    {
                        MessageBoxHelper.Success("读取成功！", "提示", null, ButtonType.OK);
                    }
                    IsShowMessage = true;
                    break;
            }
        }

    }
}
