using CommunityToolkit.Mvvm.ComponentModel;
using Sofar.BMSLib;
using Sofar.BMSUI.Common;
using Sofar.ProtocolLib;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Sofar.HvBMSUI.ViewModels
{
    public class FileTransmit_BMS_ViewModel : ObservableObject
    {
        // 文件传输日志数据列表
        private ObservableCollection<FileTransmitLogData> _fileTransmitLogList;
        public ObservableCollection<FileTransmitLogData> FileTransmitLogList
        {
            get { return _fileTransmitLogList; }
            set
            {
                _fileTransmitLogList = value;
                OnPropertyChanged(nameof(FileTransmitLogList));
            }
        }

        private string _fileTransmitStatus = "启动传输";
        /// <summary>
        /// 文件传输按钮状态
        /// </summary>
        public string FileTransmitStatus
        {
            get { return _fileTransmitStatus; }
            set { _fileTransmitStatus = value; OnPropertyChanged(); }
        }

        private int _readCount = 0;
        /// <summary>
        /// 文件读取条数
        /// </summary>
        public int readCount
        {
            get { return _readCount; }
            set { _readCount = value; OnPropertyChanged(); }
        }

        private int _startLocal = 0;
        /// <summary>
        /// 文件读取起始位置
        /// </summary>
        public int StartLocal
        {
            get { return _startLocal; }
            set { _startLocal = value; OnPropertyChanged(); }
        }


        private bool _state = false;
        /// <summary>
        /// 文件传输状态
        /// </summary>
        public bool state
        {
            get { return _state; }
            set
            {
                _state = value;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (_state)
                    {
                        IsModeNameEnable = false;
                        FileTransmitStatus = "终止传输";
                    }
                    else
                    {
                        IsModeNameEnable = true;
                        FileTransmitStatus = "启动传输";
                    }
                });
            }
        }

        private int _slaveAddress = 0x81;
        /// <summary>
        /// BCU设备地址
        /// </summary>
        public int slaveAddress
        {
            get { return _slaveAddress; }
            set { _slaveAddress = value; OnPropertyChanged(); }
        }

        private int _subDeviceAddress = 0;
        /// <summary>
        /// BMU设备地址
        /// </summary>
        public int subDeviceAddress
        {
            get { return _subDeviceAddress; }
            set { _subDeviceAddress = value; OnPropertyChanged(); }
        }

        // 文件类型列表
        public ObservableCollection<string> FileNumberList { get; } = new ObservableCollection<string>
        {
            "0：故障录波文件1",
            "1：故障录波文件2",
            "2：故障录波文件3",
            "3：5min特性数据文件",
            "4：运行日志文件",
            "5：历史事件文件"
        };

        private string _selectedFileNumber = "3：5min特性数据文件";
        //文件类型 
        public string SelectedFileNumber
        {
            get { return _selectedFileNumber; }
            set
            {
                if (_selectedFileNumber != value)
                {
                    _selectedFileNumber = value;
                    OnPropertyChanged(nameof(SelectedFileNumber));                                 
                }
            }
        }

        // 模块列表
        public ObservableCollection<string> ModeNameList { get; } = new ObservableCollection<string>
        {
            "BCU",
            "BMU"
        };

        private string _selectedModeName = "BCU";
        //模块  BCU、BMU
        public string SelectedModeName
        {
            get { return _selectedModeName; }
            set
            {
                if (_selectedModeName != value)
                {
                    _selectedModeName = value;
                    OnPropertyChanged(nameof(SelectedModeName));
                    if(value == "BCU")
                    {
                        subDeviceAddress = 0;
                    }
                    else
                    {
                        subDeviceAddress = 1;
                    }
                } 
            }
        }

        private bool _isReadAll_Checked;
        /// <summary>
        /// 是否读取所有文件
        /// </summary>
        public bool IsReadAll_Checked
        {
            get { return _isReadAll_Checked; }
            set { _isReadAll_Checked = value;
                if (value)
                {
                    VisibleAttributes = "Collapsed";
                   
                }
                else
                {
                    VisibleAttributes = "Visible";
                   
                }
                OnPropertyChanged(); }
        }

        private bool _isTransmitEnable = true;
        /// <summary>
        /// 文件传输按钮是否可用
        /// </summary>
        public bool IsTransmitEnable
        {
            get { return _isTransmitEnable; }
            set { _isTransmitEnable = value; OnPropertyChanged(); }
        }

        private bool _isModeNameEnable = true;
        /// <summary>
        /// 模块选择按钮是否可用
        /// </summary>
        public bool IsModeNameEnable
        {
            get { return _isModeNameEnable; }
            set { _isModeNameEnable = value; OnPropertyChanged(); }
        }
       

        private string _visibleAttributes = "Visible";
        /// <summary>
        /// 文件传输按钮是否可用
        /// </summary>
        public string VisibleAttributes
        {
            get { return _visibleAttributes; }
            set { _visibleAttributes = value; OnPropertyChanged(); }
        }

        public static CancellationTokenSource cts = new CancellationTokenSource();
        private CancellationTokenSource _token = new CancellationTokenSource();
        BaseCanHelper baseCanHelper = null;
        //变量定义
        //5min特性数据文件表头
        private readonly string FiveMinHeadStr_BCU = "故障ID,时间年,月,日,时,分,秒,电池簇累加电压(V),SOC(%),SOH(%),电池簇电流(A),DI状态,DO状态,系统状态,其他状态,告警等级,告警信息1,告警信息2,告警信息3,告警信息4,告警信息5,告警信息6,告警信息7,告警信息8,告警信息9,告警信息10,告警信息11,告警信息12,告警信息13,告警信息14,告警信息15,告警信息16,最高pack电压(V),最低pack电压(V),最高pack电压序号,最低pack电压序号,组端电流2,高压绝缘阻抗(kΩ),母线侧电压(V),负载端电压(V),辅助电源电压(V),电池簇充电电流上限(A),电池簇放电电流上限(A),P+功率端子温度(℃),P-功率端子温度(℃),B+功率端子温度(℃),B-功率端子温度(℃),环境温度(℃),累计充电安时(Ah),累计放电安时(Ah),累计充电瓦时(Wh),累计放电瓦时(Wh),PACK1最大单体电压(mV),PACK2最大单体电压(mV),PACK3最大单体电压(mV),PACK4最大单体电压(mV),PACK5最大单体电压(mV),PACK6最大单体电压(mV),PACK7最大单体电压(mV),PACK8最大单体电压(mV),PACK1最小单体电压(mV),PACK2最小单体电压(mV),PACK3最小单体电压(mV),PACK4最小单体电压(mV),PACK5最小单体电压(mV),PACK6最小单体电压(mV),PACK7最小单体电压(mV),PACK8最小单体电压(mV),PACK1平均单体电压(mV),PACK2平均单体电压(mV),PACK3平均单体电压(mV),PACK4平均单体电压(mV),PACK5平均单体电压(mV),PACK6平均单体电压(mV),PACK7平均单体电压(mV),PACK8平均单体电压(mV),PACK1最大单体温度(℃),PACK2最大单体温度(℃),PACK3最大单体温度(℃),PACK4最大单体温度(℃),PACK5最大单体温度(℃),PACK6最大单体温度(℃),PACK7最大单体温度(℃),PACK8最大单体温度(℃),PACK1最小单体温度(℃),PACK2最小单体温度(℃),PACK3最小单体温度(℃),PACK4最小单体温度(℃),PACK5最小单体温度(℃),PACK6最小单体温度(℃),PACK7最小单体温度(℃),PACK8最小单体温度(℃),PACK1平均单体温度,PACK2平均单体温度,PACK3平均单体温度,PACK4平均单体温度,PACK5平均单体温度,PACK6平均单体温度,PACK7平均单体温度,PACK8平均单体温度,U32预留1,预留2,预留3,预留4,预留5,预留6,预留7,预留8,预留9,预留10,预留11,预留12,预留13,U8预留1";
        private readonly string FiveMinHeadStr_BMU = "故障ID,时间年,月,日,时,分,秒,电池采集电压(V),电池累计电压(V),SOC显示值(%),SOH显示值(%),SOC计算值,SOH计算值,电池电流(A),最高单体电压(mV),最低单体电压(mV),最高单体电压序号,最低单体电压序号,最高单体温度(℃),最低单体温度(℃),最高单体温度序号,最低单体温度序号,充电电流上限(A),放电电流上限(A),系统状态,其他状态,告警等级,告警信息1,告警信息2,告警信息3,告警信息4,告警信息5,告警信息6,告警信息7,告警信息8,告警信息9,告警信息10,告警信息11,告警信息12,告警信息13,告警信息14,告警信息15,告警信息16,告警信息17,告警信息18,告警信息19,告警信息20,辅助供电电压(V),累计放电安时(Ah),累计充电安时(Ah),累计放电瓦时(Wh),累计充电瓦时(Wh),环境温度(℃),均衡温度1(℃),均衡温度2(℃),均衡温度3(℃),均衡温度4(℃),均衡温度5(℃),均衡温度6(℃),均衡温度7(℃),均衡温度8(℃),功率端子温度1(℃),功率端子温度2(℃),1-16串均衡状态,17-32串均衡状态,33-48串均衡状态,49-64串均衡状态,单体电压1(mV),单体电压2(mV),单体电压3(mV),单体电压4(mV),单体电压5(mV),单体电压6(mV),单体电压7(mV),单体电压8(mV),单体电压9(mV),单体电压10(mV),单体电压11(mV),单体电压12(mV),单体电压13(mV),单体电压14(mV),单体电压15(mV),单体电压16(mV),单体电压17(mV),单体电压18(mV),单体电压19(mV),单体电压20(mV),单体电压21(mV),单体电压22(mV),单体电压23(mV),单体电压24(mV),单体电压25(mV),单体电压26(mV),单体电压27(mV),单体电压28(mV),单体电压29(mV),单体电压30(mV),单体电压31(mV),单体电压32(mV),单体电压33(mV),单体电压34(mV),单体电压35(mV),单体电压36(mV),单体电压37(mV),单体电压38(mV),单体电压39(mV),单体电压40(mV),单体电压41(mV),单体电压42(mV),单体电压43(mV),单体电压44(mV),单体电压45(mV),单体电压46(mV),单体电压47(mV),单体电压48(mV),单体电压49(mV),单体电压50(mV),单体电压51(mV),单体电压52(mV),单体电压53(mV),单体电压54(mV),单体电压55(mV),单体电压56(mV),单体电压57(mV),单体电压58(mV),单体电压59(mV),单体电压60(mV),单体电压61(mV),单体电压62(mV),单体电压63(mV),单体电压64(mV),单体温度1(℃),单体温度2(℃),单体温度3(℃),单体温度4(℃),单体温度5(℃),单体温度6(℃),单体温度7(℃),单体温度8(℃),单体温度9(℃),单体温度10(℃),单体温度11(℃),单体温度12(℃),单体温度13(℃),单体温度14(℃),单体温度15(℃),单体温度16(℃),单体温度17(℃),单体温度18(℃),单体温度19(℃),单体温度20(℃),单体温度21(℃),单体温度22(℃),单体温度23(℃),单体温度24(℃),单体温度25(℃),单体温度26(℃),单体温度27(℃),单体温度28(℃),单体温度29(℃),单体温度30(℃),单体温度31(℃),单体温度32(℃),U32预留1,预留2,预留3,预留4,U8预留1,预留2,预留3";

        //代号,数据类型,数据长度,精度,单位
        //5min特性数据文件内容，故障录波文件内容
        private readonly string FiveMinText_BCU = @"故障ID,U8,1,,
时间-年,U8,1,1,
时间-月,U8,1,1,
时间-日,U8,1,1,
时间-时,U8,1,1,
时间-分,U8,1,1,
时间-秒,U8,1,1,
电池簇累加电压,U16,1,0.1,V
电池簇SOC,U8,1,1,%
电池簇SOH,U8,1,1,%
电池簇电流,I16,1,0.01,A
DI状态,U8,1,1,HEX
DO状态,U8,1,1,HEX
系统状态,U8,1,1,HEX
其他状态,U8,1,1,HEX
告警等级,U8,1,1,HEX
告警信息1,U8,1,1,HEX
告警信息2,U8,1,1,HEX
告警信息3,U8,1,1,HEX
告警信息4,U8,1,1,HEX
告警信息5,U8,1,1,HEX
告警信息6,U8,1,1,HEX
告警信息7,U8,1,1,HEX
告警信息8,U8,1,1,HEX
告警信息9,U8,1,1,HEX
告警信息10,U8,1,1,HEX
告警信息11,U8,1,1,HEX
告警信息12,U8,1,1,HEX
告警信息13,U8,1,1,HEX
告警信息14,U8,1,1,HEX
告警信息15,U8,1,1,HEX
告警信息16,U8,1,1,HEX
最高pack电压,U16,1,0.1,V
最低pack电压,U16,1,0.1,V
最高pack电压包序号,U8,1,1,号
最低pack电压包序号,U8,1,1,号
组端电流2,I16,1,0.01,A
高压绝缘阻抗,U16,1,1,kΩ
母线侧电压,U16,1,0.1,V
负载端电压,U16,1,0.1,V
辅助电源电压,U16,1,0.1,V
电池簇充电电流上限,U16,1,0.1,A
电池簇放电电流上限,U16,1,0.1,A
P+功率端子温度,I8,1,1,℃
P-功率端子温度,I8,1,1,℃
B+功率端子温度,I8,1,1,℃
B-功率端子温度,I8,1,1,℃
环境温度,I8,1,1,℃
累计充电安时,U32,1,1,Ah
累计放电安时,U32,1,1,Ah
累计充电瓦时,U32,1,1,Wh
累计放电瓦时,U32,1,1,Wh
PACK1最大单体电压,U16,1,1,mV
PACK2最大单体电压,U16,1,1,mV
PACK3最大单体电压,U16,1,1,mV
PACK4最大单体电压,U16,1,1,mV
PACK5最大单体电压,U16,1,1,mV
PACK6最大单体电压,U16,1,1,mV
PACK7最大单体电压,U16,1,1,mV
PACK8最大单体电压,U16,1,1,mV
PACK1最小单体电压,U16,1,1,mV
PACK2最小单体电压,U16,1,1,mV
PACK3最小单体电压,U16,1,1,mV
PACK4最小单体电压,U16,1,1,mV
PACK5最小单体电压,U16,1,1,mV
PACK6最小单体电压,U16,1,1,mV
PACK7最小单体电压,U16,1,1,mV
PACK8最小单体电压,U16,1,1,mV
PACK1平均单体电压,U16,1,1,mV
PACK2平均单体电压,U16,1,1,mV
PACK3平均单体电压,U16,1,1,mV
PACK4平均单体电压,U16,1,1,mV
PACK5平均单体电压,U16,1,1,mV
PACK6平均单体电压,U16,1,1,mV
PACK7平均单体电压,U16,1,1,mV
PACK8平均单体电压,U16,1,1,mV
PACK1最大单体温度,I8,1,1,℃
PACK2最大单体温度,I8,1,1,℃
PACK3最大单体温度,I8,1,1,℃
PACK4最大单体温度,I8,1,1,℃
PACK5最大单体温度,I8,1,1,℃
PACK6最大单体温度,I8,1,1,℃
PACK7最大单体温度,I8,1,1,℃
PACK8最大单体温度,I8,1,1,℃
PACK1最小单体温度,I8,1,1,℃
PACK2最小单体温度,I8,1,1,℃
PACK3最小单体温度,I8,1,1,℃
PACK4最小单体温度,I8,1,1,℃
PACK5最小单体温度,I8,1,1,℃
PACK6最小单体温度,I8,1,1,℃
PACK7最小单体温度,I8,1,1,℃
PACK8最小单体温度,I8,1,1,℃
PACK1平均单体温度,I8,1,1,℃
PACK2平均单体温度,I8,1,1,℃
PACK3平均单体温度,I8,1,1,℃
PACK4平均单体温度,I8,1,1,℃
PACK5平均单体温度,I8,1,1,℃
PACK6平均单体温度,I8,1,1,℃
PACK7平均单体温度,I8,1,1,℃
PACK8平均单体温度,I8,1,1,℃
预留1,U32,1,,
预留2,U32,1,,
预留3,U32,1,,
预留4,U32,1,,
预留5,U32,1,,
预留6,U32,1,,
预留7,U32,1,,
预留8,U32,1,,
预留9,U32,1,,
预留10,U32,1,,
预留11,U32,1,,
预留12,U32,1,,
预留13,U32,1,,
预留,U8,1,,";
        private readonly string FiveMinText_BMU = @"故障ID,U8,1,1,
时间-年,U8,1,1,
时间-月,U8,1,1,
时间-日,U8,1,1,
时间-时,U8,1,1,
时间-分,U8,1,1,
时间-秒,U8,1,1,
电池采样电压,U16,1,0.1,V
电池累计电压,U16,1,0.1,V
SOC显示值,U8,1,1,%
SOH显示值,U8,1,1,%
SOC计算值,U32,1,0.001,%
SOH计算值,U32,1,0.001,%
电池电流,I16,1,0.01,A
最高单体电压,U16,1,1,mV
最低单体电压,U16,1,1,mV
最高单体电压序号,U8,1,,
最低单体电压序号,U8,1,,
最高单体温度,I8,1,1,℃
最低单体温度,I8,1,1,℃
最高单体温度序号,U8,1,,
最低单体温度序号,U8,1,,
电池包充电电流上限,U16,1,0.1,A
电池包放电电流上限,U16,1,0.1,A
系统状态,U8,1,,
其他状态,U8,1,,
告警等级,U8,1,,
告警信息1,U8,1,,HEX
告警信息2,U8,1,,HEX
告警信息3,U8,1,,HEX
告警信息4,U8,1,,HEX
告警信息5,U8,1,,HEX
告警信息6,U8,1,,HEX
告警信息7,U8,1,,HEX
告警信息8,U8,1,,HEX
告警信息9,U8,1,,HEX
告警信息10,U8,1,,HEX
告警信息11,U8,1,,HEX
告警信息12,U8,1,,HEX
告警信息13,U8,1,,HEX
告警信息14,U8,1,,HEX
告警信息15,U8,1,,HEX
告警信息16,U8,1,,HEX
告警信息17,U8,1,,HEX
告警信息18,U8,1,,HEX
告警信息19,U8,1,,HEX
告警信息20,U8,1,,HEX
辅助供电电压,U16,1,0.1,V
累计充电安时,U32,1,1,Ah
累计放电安时,U32,1,1,Ah
累计充电瓦时,U32,1,1,Wh
累计放电瓦时,U32,1,1,Wh
环境温度,I8,1,1,℃
均衡温度1,I8,1,1,℃
均衡温度2,I8,1,1,℃
均衡温度3,I8,1,1,℃
均衡温度4,I8,1,1,℃
均衡温度5,I8,1,1,℃
均衡温度6,I8,1,1,℃
均衡温度7,I8,1,1,℃
均衡温度8,I8,1,1,℃
功率端子温度1,I8,1,1,℃
功率端子温度2,I8,1,1,℃
1-16串均衡状态,U16,1,1,HEX
17-32串均衡状态,U16,1,1,HEX
33-48串均衡状态,U16,1,1,HEX
49-64串均衡状态,U16,1,1,HEX
单体电压1,U16,1,1,mV
单体电压2,U16,1,1,mV
单体电压3,U16,1,1,mV
单体电压4,U16,1,1,mV
单体电压5,U16,1,1,mV
单体电压6,U16,1,1,mV
单体电压7,U16,1,1,mV
单体电压8,U16,1,1,mV
单体电压9,U16,1,1,mV
单体电压10,U16,1,1,mV
单体电压11,U16,1,1,mV
单体电压12,U16,1,1,mV
单体电压13,U16,1,1,mV
单体电压14,U16,1,1,mV
单体电压15,U16,1,1,mV
单体电压16,U16,1,1,mV
单体电压17,U16,1,1,mV
单体电压18,U16,1,1,mV
单体电压19,U16,1,1,mV
单体电压20,U16,1,1,mV
单体电压21,U16,1,1,mV
单体电压22,U16,1,1,mV
单体电压23,U16,1,1,mV
单体电压24,U16,1,1,mV
单体电压25,U16,1,1,mV
单体电压26,U16,1,1,mV
单体电压27,U16,1,1,mV
单体电压28,U16,1,1,mV
单体电压29,U16,1,1,mV
单体电压30,U16,1,1,mV
单体电压31,U16,1,1,mV
单体电压32,U16,1,1,mV
单体电压33,U16,1,1,mV
单体电压34,U16,1,1,mV
单体电压35,U16,1,1,mV
单体电压36,U16,1,1,mV
单体电压37,U16,1,1,mV
单体电压38,U16,1,1,mV
单体电压39,U16,1,1,mV
单体电压40,U16,1,1,mV
单体电压41,U16,1,1,mV
单体电压42,U16,1,1,mV
单体电压43,U16,1,1,mV
单体电压44,U16,1,1,mV
单体电压45,U16,1,1,mV
单体电压46,U16,1,1,mV
单体电压47,U16,1,1,mV
单体电压48,U16,1,1,mV
单体电压49,U16,1,1,mV
单体电压50,U16,1,1,mV
单体电压51,U16,1,1,mV
单体电压52,U16,1,1,mV
单体电压53,U16,1,1,mV
单体电压54,U16,1,1,mV
单体电压55,U16,1,1,mV
单体电压56,U16,1,1,mV
单体电压57,U16,1,1,mV
单体电压58,U16,1,1,mV
单体电压59,U16,1,1,mV
单体电压60,U16,1,1,mV
单体电压61,U16,1,1,mV
单体电压62,U16,1,1,mV
单体电压63,U16,1,1,mV
单体电压64,U16,1,1,mV
单体温度1,I8,1,1,℃
单体温度2,I8,1,1,℃
单体温度3,I8,1,1,℃
单体温度4,I8,1,1,℃
单体温度5,I8,1,1,℃
单体温度6,I8,1,1,℃
单体温度7,I8,1,1,℃
单体温度8,I8,1,1,℃
单体温度9,I8,1,1,℃
单体温度10,I8,1,1,℃
单体温度11,I8,1,1,℃
单体温度12,I8,1,1,℃
单体温度13,I8,1,1,℃
单体温度14,I8,1,1,℃
单体温度15,I8,1,1,℃
单体温度16,I8,1,1,℃
单体温度17,I8,1,1,℃
单体温度18,I8,1,1,℃
单体温度19,I8,1,1,℃
单体温度20,I8,1,1,℃
单体温度21,I8,1,1,℃
单体温度22,I8,1,1,℃
单体温度23,I8,1,1,℃
单体温度24,I8,1,1,℃
单体温度25,I8,1,1,℃
单体温度26,I8,1,1,℃
单体温度27,I8,1,1,℃
单体温度28,I8,1,1,℃
单体温度29,I8,1,1,℃
单体温度30,I8,1,1,℃
单体温度31,I8,1,1,℃
单体温度32,I8,1,1,℃
预留1,U32,1,,
预留2,U32,1,,
预留3,U32,1,,
预留4,U32,1,,
预留1,U8,1,,
预留2,U8,1,,
预留3,U8,1,,";

        StepRemark stepFlag;

        int fileNumber = -1;
        int readType = -1;
        int questCycle = 0;
        int fileOffset = -1;
        int dataLength = -1;
        int fileSize = 0;
        string filePath = "";
        string fileName = "";
        List<byte> dataBuffer = new List<byte>();
        byte SlaveAddress = 0xE8;
        string dataText = "";
        string dataHeader = "";
        private Dictionary<int, FileTypeConfig> bcuFileTypeConfigs = new();
        private Dictionary<int, FileTypeConfig> bmuFileTypeConfigs = new();

        public ICommand FileTransmitCmd => new RelayCommand(StartFileTransmit);

        public FileTransmit_BMS_ViewModel()
        {
            baseCanHelper = new CommandOperation(BMSConfig.ConfigManager).baseCanHelper;
            cts = new CancellationTokenSource();
            if (FileTransmitLogList == null) 
                FileTransmitLogList = new ObservableCollection<FileTransmitLogData>();

            bcuFileTypeConfigs = new Dictionary<int, FileTypeConfig>()
            {
                { 0, new FileTypeConfig() { HeadName = "故障录波文件1", Extension = ".csv", DefaultOffset = 200, DefaultLength = 200, HeadStr = FiveMinHeadStr_BCU, Content = FiveMinText_BCU } },
                { 3, new FileTypeConfig() { HeadName = "5min特性数据文件", Extension = ".csv", DefaultOffset = 200, DefaultLength = 200, HeadStr = FiveMinHeadStr_BCU, Content = FiveMinText_BCU }},
                { 4, new FileTypeConfig() { HeadName = "运行日志文件", Extension = ".txt", DefaultOffset = 200, DefaultLength = 200, HeadStr = null, Content = null }},
            };

            bmuFileTypeConfigs = new Dictionary<int, FileTypeConfig>()
            {
                { 0, new FileTypeConfig() { HeadName = "故障录波文件1", Extension = ".csv", DefaultOffset = 280, DefaultLength = 280, HeadStr = FiveMinHeadStr_BMU, Content = FiveMinText_BMU }},
                { 3, new FileTypeConfig() { HeadName = "5min特性数据文件", Extension = ".csv", DefaultOffset = 280, DefaultLength = 280, HeadStr = FiveMinHeadStr_BMU, Content = FiveMinText_BMU }},
                { 4, new FileTypeConfig() { HeadName = "运行日志文件", Extension = ".txt", DefaultOffset = 256, DefaultLength = 256, HeadStr = null, Content = null }},
            };
        }

        public void Load()
        {
            cts = new CancellationTokenSource();

            Task.Factory.StartNew(() =>
            {
                if (baseCanHelper.CommunicationType == "Ecan")
                {
                    lock (EcanHelper._locker)
                    {
                        while (EcanHelper._task.Count > 0
                            && !cts.IsCancellationRequested)
                        {
                            CAN_OBJ ch = (CAN_OBJ)EcanHelper._task.Dequeue();

                            Application.Current.Dispatcher.Invoke(() => { AnalysisData(ch.ID, ch.Data); });
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
                        }
                    }
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

        public void StartFileTransmit()
        {
            FileTransmitLogList.Clear();

            // 获取文件编号/索引
            fileNumber = FileNumberList.IndexOf(SelectedFileNumber);

            //配置信息
            string currentFileName = SelectedFileNumber.Split("：")[1];
            if (SelectedModeName == "BCU")
            {
                FileTypeConfig model = bcuFileTypeConfigs.Values.Where(b => b.HeadName == currentFileName).FirstOrDefault();
                if (model != null)
                {
                    dataLength = model.DefaultLength;
                    dataHeader = model.HeadStr;
                    dataText = model.Content;
                }

                if (currentFileName.Equals("5min特性数据文件") && subDeviceAddress != 0)
                {
                    dataHeader = FiveMinHeadStr_BMU;
                    dataLength = 280;
                }

                SlaveAddress = (byte)slaveAddress;
            }
            else if (SelectedModeName == "BMU")
            {
                FileTypeConfig model = bmuFileTypeConfigs.Values.Where(b => b.HeadName == currentFileName).FirstOrDefault();
                if (model != null)
                {
                    dataLength = model.DefaultLength;
                    dataHeader = model.HeadStr;
                    dataText = model.Content;
                }

                SlaveAddress = (byte)subDeviceAddress;
            }

            //读取方式：0从零读取，1部分读取
            readType = IsReadAll_Checked ? 0 : 1;

            // 开始进行传输
            //int recount = 0;
            if (!state)
            {
                state = true;
                stepFlag = StepRemark.读文件指令帧;
                _token = new CancellationTokenSource();

                // 读文件指令
                ReadFileCommand();
            }
            else
            {
                stepFlag = StepRemark.读文件指令帧;
                EndTransmit();
            }
        }

        private void EndTransmit()
        {
            //按钮禁用15s
            Stopwatch stopwatch = Stopwatch.StartNew();
            Task.Run(async delegate
            {
                Application.Current.Dispatcher.Invoke(() => { IsTransmitEnable = false; });
                await Task.Delay(15000);
                Application.Current.Dispatcher.Invoke(() => { IsTransmitEnable = true; });
            });

            // 释放资源
            _token.Cancel();
            dataBuffer.Clear();
            state = false;
            stepFlag = StepRemark.None;

            fileSize = -1;
            fileOffset = -1;
            dataLength = -1;
        }

        private void ReadFileCommand()
        {
            byte tagSource = SelectedModeName == "BCU" ? (byte)0xF4 : (byte)0xE0;
            byte[] canid = { tagSource, SlaveAddress, 0xF0, 0x07 };

            byte[] data = new byte[8];
            data[0] = 0x0;
            data[1] = (byte)subDeviceAddress;
            data[2] = (byte)fileNumber;
            data[3] = (byte)readType;

            bool result = baseCanHelper.Send(data, canid);
            if (result) AddLog(System.DateTime.Now.ToString("HH:mm:ss:fff"), BitConverter.ToUInt32(canid, 0).ToString("X8"), byteToHexString(data));
        }

        private void ReadFileDataContent()
        {
            byte tagSource = SelectedModeName == "BCU" ? (byte)0xF4 : (byte)0xE0;
            byte[] canid = { tagSource, SlaveAddress, 0xF1, 0x07 };

            byte[] data = new byte[8];
            data[0] = 0x0;
            data[1] = (byte)subDeviceAddress;
            data[2] = (byte)fileNumber;
            data[3] = (byte)(fileOffset & 0xff);
            data[4] = (byte)(fileOffset >> 8);
            data[5] = (byte)(fileOffset >> 16);
            data[6] = (byte)(dataLength & 0xff);
            data[7] = (byte)(dataLength >> 8);

            bool result = baseCanHelper.Send(data, canid);
            if (result) AddLog(System.DateTime.Now.ToString("HH:mm:ss:fff"), BitConverter.ToUInt32(canid, 0).ToString("X8"), byteToHexString(data));
        }

        private void QueryCompletionStatus()
        {
            byte tagSource = SelectedModeName == "BCU" ? (byte)0xF4 : (byte)0xE0;
            byte[] canid = { tagSource, SlaveAddress, 0xF4, 0x07 };

            byte[] data = new byte[8];
            data[0] = 0x0;
            data[1] = (byte)subDeviceAddress;
            data[2] = (byte)fileNumber;
            data[3] = (byte)0x01;

            bool result = baseCanHelper.Send(data, canid);
            if (result) AddLog(System.DateTime.Now.ToString("HH:mm:ss:fff"), BitConverter.ToUInt32(canid, 0).ToString("X8"), byteToHexString(data));
        }

        private void AnalysisData(uint id, byte[] data)
        {
            try
            {
                id = id | 0xff;
                switch (stepFlag)
                {
                    case StepRemark.读文件指令帧:
                        if ((id == 0x7F0F4FF || id == 0x7F0E0FF) && data[3] == 0x0)
                        {
                            AddLog(System.DateTime.Now.ToString("HH:mm:ss:fff"), id.ToString("X8"), byteToHexString(data));

                            FileTypeConfig config = null;
                            // 获取文件配置
                            if (SelectedModeName == "BCU")
                            {
                                if (!bcuFileTypeConfigs.TryGetValue(fileNumber, out config))
                                {
                                    AddLog(DateTime.Now.ToString("HH:mm:ss"), id.ToString("X8"), $"ERROR:未知文件类型: {fileNumber}");
                                    return;
                                }
                            }
                            else if (SelectedModeName == "BMU")
                            {
                                if (!bmuFileTypeConfigs.TryGetValue(fileNumber, out config))
                                {
                                    AddLog(DateTime.Now.ToString("HH:mm:ss"), id.ToString("X8"), $"ERROR:未知文件类型: {fileNumber}");
                                    return;
                                }
                            }

                            if (config == null)
                                return;

                            string headStr = dataHeader;//config.HeadStr;
                            string headName = config.HeadName;

                            // 计算文件大小（改进字节处理）
                            fileSize = (data[6] << 16) | (data[5] << 8) | data[4];

                            fileName = $"{headName}_{SelectedModeName}_{slaveAddress}_{DateTime.Now:yy-MM-dd-HH-mm-ss}";
                            string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                            Directory.CreateDirectory(logDir); // 确保目录存在
                            filePath = Path.Combine(logDir, $"{fileName}{config.Extension}");

                            if (!string.IsNullOrEmpty(headStr))
                            {
                                // 写入文件头
                                if (!File.Exists(filePath))
                                {
                                    SafeWriteToFile(filePath, headStr);
                                }
                            }

                            // 计算任务周期（优化逻辑）
                            if (readType == 0)
                            {
                                fileOffset = 0;
                                if (fileSize % config.DefaultOffset == 0)
                                {
                                    questCycle = (fileSize / config.DefaultOffset) - 1;
                                }
                                else
                                {
                                    questCycle = fileSize / config.DefaultOffset;
                                }

                                //当bcu要传输bmu时
                                if (SelectedModeName == "BCU" && subDeviceAddress != 0)
                                {
                                    if (fileSize % dataLength == 0)
                                    {
                                        questCycle = (fileSize / dataLength) - 1;
                                    }
                                    else
                                    {
                                        questCycle = fileSize / dataLength;
                                    }
                                }
                                Debug.WriteLine($"读取次数{questCycle}");
                            }
                            else
                            {
                                if (fileSize - (readCount * config.DefaultOffset) < 0)
                                {
                                    fileOffset = config.DefaultOffset;
                                }
                                else
                                {
                                    fileOffset = fileSize - (readCount * config.DefaultOffset);
                                }

                                if ((fileSize - fileOffset) % config.DefaultOffset == 0)
                                {
                                    questCycle = ((fileSize - fileOffset) / config.DefaultOffset) + 1;
                                }
                                else
                                {
                                    questCycle = ((fileSize - fileOffset) / config.DefaultOffset);
                                }
                                Debug.WriteLine($"读取次数:{questCycle}，文件偏移起始值:{fileOffset}");
                            }

                            stepFlag = StepRemark.读文件数据内容帧;
                            {
                                ReadFileDataContent();
                                questCycle -= 1;
                            }

                        }
                        break;
                    case StepRemark.读文件数据内容帧:
                        if (id == 0x7F2F4FF || id == 0x7F2E0FF)
                        {
                            dataBuffer.AddRange(data);
                        }
                        else if (id == 0x7F3F4FF || id == 0x7F3E0FF)
                        {
                            AddLog(System.DateTime.Now.ToString("HH:mm:ss:fff"), id.ToString("X8"), byteToHexString(data));

                            FileTypeConfig config = null;
                            if (SelectedModeName == "BCU")
                            {
                                // 获取文件配置
                                if (!bcuFileTypeConfigs.TryGetValue(fileNumber, out config))
                                {
                                    AddLog(DateTime.Now.ToString("HH:mm:ss"), id.ToString("X8"), $"ERROR:未知文件类型: {fileNumber}");
                                    return;
                                }
                            }
                            else if (SelectedModeName == "BMU")
                            {
                                // 获取文件配置
                                if (!bmuFileTypeConfigs.TryGetValue(fileNumber, out config))
                                {
                                    AddLog(DateTime.Now.ToString("HH:mm:ss"), id.ToString("X8"), $"ERROR:未知文件类型: {fileNumber}");
                                    return;
                                }
                            }

                            for (int i = 0; i < dataBuffer.ToArray().Length; i++)
                            {
                                Debug.Write(dataBuffer[i].ToString("X2") + " ");
                            }
                            Debug.Write("\r\n");

                            // 通过crc进行数据校验，如果数据不对需要重新读取
                            ushort crc = Crc16Ccitt(dataBuffer.ToArray());
                            if (crc == BitConverter.ToUInt16(new byte[] { data[3], data[4] }, 0))
                            {
                                if (fileNumber == 4)
                                {
                                    string sbContent = "";
                                    for (int i = 0; i < dataBuffer.Count; i++)
                                    {
                                        String asciiStr = ((char)dataBuffer[i]).ToString();//十六进制转ASCII码
                                        sbContent += asciiStr;
                                    }

                                    //Debug.WriteLine($"缓存个数：{dataBuffer.Count}，数据：{sbContent}");
                                    dataBuffer.Clear();
                                    if (!File.Exists(filePath))
                                    {
                                        using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.UTF8))
                                        {
                                            sw.Write(sbContent);
                                        }
                                    }
                                    else
                                    {
                                        FileStream fs = new FileStream(filePath, FileMode.Append);
                                        StreamWriter sw1 = new StreamWriter(fs, Encoding.UTF8);
                                        sw1.Write(sbContent);
                                        sw1.Close();
                                    }
                                }
                                else
                                {
                                    string getValue = ToAnalysis(dataText, dataBuffer.ToArray());
                                    dataBuffer.Clear();

                                    File.AppendAllText(filePath, getValue + "\r\n");
                                }

                                Interlocked.Add(ref fileOffset, dataLength);
                                if (questCycle <= 0)
                                {
                                    Debug.WriteLine("End read file data content...");
                                    stepFlag = StepRemark.查询完成状态帧;
                                    QueryCompletionStatus();
                                }
                                else if (questCycle > 0)
                                {
                                    ReadFileDataContent();
                                    questCycle--;

                                    Debug.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff")} 所剩读取次数：{questCycle}");
                                }
                            }
                            else
                            {
                                Debug.WriteLine($"CRC校验比对失败，重新请求！ pc：{crc.ToString("X")},response:{data[3].ToString("X2")} {data[4].ToString("X2")}");
                                dataBuffer.Clear();

                                if (questCycle <= 0)
                                {
                                    Debug.WriteLine("End read file data content...");
                                    stepFlag = StepRemark.查询完成状态帧;
                                    QueryCompletionStatus();
                                }
                                else if (questCycle > 0)
                                {
                                    questCycle += 1;
                                    ReadFileDataContent();
                                    questCycle--;

                                    Debug.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff")} 所剩读取次数：{questCycle}");
                                    if (questCycle == 0)
                                    {

                                    }
                                }
                            }
                        }
                        break;
                    case StepRemark.查询完成状态帧:
                        if (id == 0x7F4F4FF || id == 0x7F4E0FF)
                        {
                            AddLog(System.DateTime.Now.ToString("HH:mm:ss:fff"), id.ToString("X8"), byteToHexString(data));

                            if (data[3] == 0x0)
                            {
                                AddLog(System.DateTime.Now.ToString("HH:mm:ss:fff"), id.ToString("X8"), "已完成本次读取...");
                                EndTransmit();
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private string ToAnalysis(string textStr, byte[] dataBuffer)
        {
            if (dataBuffer.Length == 0) return string.Empty;

            var protocols = ToProtocol(textStr);
            var result = new StringBuilder();
            int index = 0;
            bool toHex = false;

            List<ProtocolModel> _protocols = ToProtocol(textStr);
            for (int i = 0; i < _protocols.Count; i++)
            {
                ProtocolModel model = _protocols[i];
                object val = null;

                if (toHex)
                {
                    switch (model.DataType)
                    {
                        case "U8":
                            val = $"0x{dataBuffer[index++].ToString("X2")}";
                            break;
                        case "I8":
                            val = (sbyte)dataBuffer[index++];
                            break;
                        case "U16":
                            byte[] bytes = new byte[2];

                            int byte1 = bytes[0] = dataBuffer[index++];
                            int byte2 = bytes[1] = dataBuffer[index++];
                            val = $"0x{(byte1 + (byte2 << 8)).ToString("X4")}";
                            break;
                        default:
                            break;
                    }

                    model.Value = val;
                }
                else
                {
                    switch (model.DataType)
                    {
                        case "U8":
                            val = Convert.ToByte(dataBuffer[index++]);
                            break;
                        case "I8":
                            val = (sbyte)dataBuffer[index++];
                            break;
                        case "U16":
                            byte[] bytes = new byte[2];

                            int byte1 = bytes[0] = dataBuffer[index++];
                            int byte2 = bytes[1] = dataBuffer[index++];
                            val = byte1 + (byte2 << 8);
                            break;
                        case "I16":
                            string byteStr1 = dataBuffer[index++].ToString("X2");
                            string byteStr2 = dataBuffer[index++].ToString("X2");
                            val = Convert.ToInt16(byteStr2 + byteStr1, 16);
                            break;
                        case "U32":
                            val = (dataBuffer[index++] & 0xff) + (dataBuffer[index++] << 8) + (dataBuffer[index++] << 16) + (dataBuffer[index++] << 24);
                            break;
                        default:
                            break;
                    }

                    model.Value = (model.Accuracy * Convert.ToDouble(val)).ToString();
                }

                result.Append(model.Value + ",");

                // toHex=true 直接按16进制显示
                if (SelectedModeName == "BMU" || (SelectedModeName == "BCU" && subDeviceAddress != 0))
                {
                    if (model.Name == "系统状态")
                    {
                        toHex = true;
                    }
                    else if (model.Name == "告警信息20")
                    {
                        toHex = false;
                    }
                    else if (model.Name == "功率端子温度2")
                    {
                        toHex = true;
                    }
                    else if (model.Name == "49-64串均衡状态")
                    {
                        toHex = false;
                    }
                }
                else if (SelectedModeName == "BCU")
                {
                    if (model.Name == "电池簇电流")
                    {
                        toHex = true;
                    }
                    else if (model.Name == "告警信息16")
                    {
                        toHex = false;
                    }
                }
            }

            return result.ToString();
        }

        // 辅助函数
        private void AddLog(string date, string id, string context)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                FileTransmitLogList.Insert(0, (new FileTransmitLogData
                {
                    LogTime = date,
                    FrameID = id,
                    LogData = context
                }));
            });
        }

        private List<ProtocolModel> ToProtocol(string text)
        {
            return text.Split('\n')
                 .Where(line => !string.IsNullOrWhiteSpace(line))
                 .Select(line =>
                 {
                     var parts = line.Split(',');
                     return new ProtocolModel
                     {
                         Name = parts[0].Trim(),
                         DataType = parts[1].Trim(),
                         DataLength = int.Parse(parts[2]),
                         Accuracy = string.IsNullOrEmpty(parts[3]) ? 1 : double.Parse(parts[3]),
                         Unit = parts.Length > 4 ? parts[4].Trim() : ""
                     };
                 }).ToList();
        }

        private string byteToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in data) sb.Append(b.ToString("X2") + " ");
            return sb.ToString();
        }

        // CRC16-CCITT 校验函数
        public static ushort Crc16Ccitt(byte[] data)
        {
            ushort crc = 0x0000;
            const ushort polynomial = 0x1021;

            foreach (byte b in data)
            {
                // 反转字节位序 (MSB->LSB to LSB->MSB)
                byte reversedByte = ReverseBits(b);

                crc ^= (ushort)(reversedByte << 8);

                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x8000) != 0)
                    {
                        crc = (ushort)((crc << 1) ^ polynomial);
                    }
                    else
                    {
                        crc <<= 1;
                    }
                }
            }

            // 反转16位CRC结果的位序
            return ReverseUInt16Bits(crc);
        }

        // 反转字节位序 (8位)
        private static byte ReverseBits(byte value)
        {
            byte result = 0;
            for (int i = 0; i < 8; i++)
            {
                result <<= 1;
                result |= (byte)(value & 1);
                value >>= 1;
            }
            return result;
        }

        // 反转ushort位序 (16位)
        private static ushort ReverseUInt16Bits(ushort value)
        {
            ushort result = 0;
            for (int i = 0; i < 16; i++)
            {
                result <<= 1;
                result |= (ushort)(value & 1);
                value >>= 1;
            }
            return result;
        }

        //安全文件写入方法
        private void SafeWriteToFile(string path, string content)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read))
                using (var writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.WriteLine(content);
                }
            }
            catch (IOException ex)
            {
                AddLog(DateTime.Now.ToString("HH:mm:ss"), "FILE", $"写入失败: {ex.Message}");
            }
        }

    }

    /// <summary>
    /// 文件传输日志数据
    /// </summary>
    public class FileTransmitLogData
    {
        public string LogTime { get; set; }  // 日志记录时间         
        public string FrameID { get; set; }  // 帧ID
        public string LogData { get; set; }  // 日志数据      
    }

    /// <summary>
    /// 点位格式
    /// </summary>
    public class ProtocolModel
    {
        public string Name;
        public string DataType;
        public int DataLength;
        public double Accuracy;
        public string Unit;
        public object Value;
    }

    // 定义文件类型配置类
    public class FileTypeConfig
    {
        public string HeadName { get; set; }
        public string HeadStr { get; set; }
        public string Content { get; set; }
        public string Extension { get; set; }
        public int DefaultOffset { get; set; }
        public int DefaultLength { get; set; }
    }

    public class Mode
    {
        public string Name { get; set; }
        public List<string> FileModels { get; set; }
    }

    public enum StepRemark
    {
        None,
        读文件指令帧,
        读文件数据内容帧,
        查询完成状态帧
    }
}
