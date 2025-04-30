using CommunityToolkit.Mvvm.ComponentModel;
using PowerKit.UI.Common;
using Sofar.BMSLib;
using Sofar.BMSUI;
using Sofar.BMSUI.Common;
using Sofar.ProtocolLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
      
        //private bool _isReadCountEnable = true;
        ///// <summary>
        ///// 读取条数是否可用
        ///// </summary>
        //public bool IsReadCountEnable
        //{
        //    get { return _isReadCountEnable; }
        //    set { _isReadCountEnable = value; OnPropertyChanged(); }
        //}

        //private bool _isStartLocalEnable = true;
        ///// <summary>
        ///// 文件起始位置是否可用
        ///// </summary>
        //public bool IsStartLocalEnable
        //{
        //    get { return _isStartLocalEnable; }
        //    set { _isStartLocalEnable = value; OnPropertyChanged(); }
        //}
       

        public static CancellationTokenSource cts = new CancellationTokenSource();
        private CancellationTokenSource _token = new CancellationTokenSource();
        BaseCanHelper baseCanHelper = null;
        //变量定义
        //BCU 5min特性数据文件表头
        private readonly string FiveMinHeadStr = "时间年,月,日,时,分,秒,电池簇累加电压(V),SOC(%),SOH(%),电池簇电流(A),继电器状态,风扇状态,继电器切断请求,BCU状态,充放电使能,保护信息1,保护信息2,保护信息3,保护信息4,告警信息1,告警信息2,最高pack电压(V),最低pack电压(V),最高pack电压序号,最低pack电压序号,簇号,簇内电池包个数,高压绝缘阻抗(V),保险丝后电压(V),母线侧电压(V),负载端电压(V),辅助电源电压(V),电池簇的充电电压(V),电池簇充电电流上限(A),电池簇放电电流上限(A),电池簇的放电截止电压(V),电池包均衡状态,最高功率端子温度(℃),环境温度(℃),累计充电安时(Ah),累计放电安时(Ah),累计充电瓦时(Wh),累计放电瓦时(Wh),BMU编号,单体电压1(mV),单体电压2(mV),单体电压3(mV),单体电压4(mV),单体电压5(mV),单体电压6(mV),单体电压7(mV),单体电压8(mV),单体电压9(mV),单体电压10(mV),单体电压11(mV),单体电压12(mV),单体电压13(mV),单体电压14(mV),单体电压15(mV),单体电压16(mV),PACK1最大单体电压(mV),PACK2最大单体电压(mV),PACK3最大单体电压(mV),PACK4最大单体电压(mV),PACK5最大单体电压(mV),PACK6最大单体电压(mV),PACK7最大单体电压(mV),PACK8最大单体电压(mV),PACK9最大单体电压(mV),PACK10最大单体电压(mV),PACK1最小单体电压(mV),PACK2最小单体电压(mV),PACK3最小单体电压(mV),PACK4最小单体电压(mV),PACK5最小单体电压(mV),PACK6最小单体电压(mV),PACK7最小单体电压(mV),PACK8最小单体电压(mV),PACK9最小单体电压(mV),PACK10最小单体电压(mV),PACK1平均单体电压(mV),PACK2平均单体电压(mV),PACK3平均单体电压(mV),PACK4平均单体电压(mV),PACK5平均单体电压(mV),PACK6平均单体电压(mV),PACK7平均单体电压(mV),PACK8平均单体电压(mV),PACK9平均单体电压(mV),PACK10平均单体电压(mV),PACK1最大单体温度(℃),PACK2最大单体温度(℃),PACK3最大单体温度(℃),PACK4最大单体温度(℃),PACK5最大单体温度(℃),PACK6最大单体温度(℃),PACK7最大单体温度(℃),PACK8最大单体温度(℃),PACK9最大单体温度(℃),PACK10最大单体温度(℃),PACK1最小单体温度(℃),PACK2最小单体温度(℃),PACK3最小单体温度(℃),PACK4最小单体温度(℃),PACK5最小单体温度(℃),PACK6最小单体温度(℃),PACK7最小单体温度(℃),PACK8最小单体温度(℃),PACK9最小单体温度(℃),PACK10最小单体温度(℃),告警信息3,告警信息4,RSV2\r\n";
       
        //BMU 5min特性数据文件表头
        private readonly string FiveMinHeadStr_BMU = "时间年,月,日,时,分,秒,电池采集电压(mV),电池累计电压(mV),SOC显示值(%),SOH显示值(%),SOC计算值,SOH计算值,电池电流(A),最高单体电压(mV),最低单体电压(mV),最高单体电压序号,最低单体电压序号,最高单体温度(℃),最低单体温度(℃),最高单体温度序号,最低单体温度序号,BMU编号,系统状态,充放电使能,切断请求,关机请求,充电电流上限(A),放电电流上限(A),保护1,保护2,告警1,告警2,故障1,故障2,故障1,故障2,故障3,故障4,主动均衡状态,均衡母线电压(mV),均衡母线电流(mA),辅助供电电压(mV),满充容量(Ah),循环次数,累计放电安时(Ah),累计充电安时(Ah),累计放电瓦时(Wh),累计充电瓦时(Wh),环境温度(℃),DCDC温度1(℃),均衡温度1(℃),均衡温度2(℃),功率端子温度1(℃),功率端子温度2(℃),其他温度1(℃),其他温度2(℃),其他温度3(℃),其他温度4(℃),1-16串均衡状态,单体电压1(mV),单体电压2(mV),单体电压3(mV),单体电压4(mV),单体电压5(mV),单体电压6(mV),单体电压7(mV),单体电压8(mV),单体电压9(mV),单体电压10(mV),单体电压11(mV),单体电压12(mV),单体电压13(mV),单体电压14(mV),单体电压15(mV),单体电压16(mV),单体温度1(℃),单体温度2(℃),单体温度3(℃),单体温度4(℃),单体温度5(℃),单体温度6(℃),单体温度7(℃),单体温度8(℃),单体温度9(℃),单体温度10(℃),单体温度11(℃),单体温度12(℃),单体温度13(℃),单体温度14(℃),单体温度15(℃),单体温度16(℃),RSV1,RSV2,RSV3,RSV4,RSV5,RSV6\r\n";
       
        //BCU、BMU 故障录波文件表头
        private readonly string FaultRecordStr = "电流(A),最大电压(mV),最小电压(mV),最大温度(℃),最小温度(℃)\r\n";
      
        //BCU、BMU 历史事件文件表头
        private readonly string HistoryEventStr = "时间年,月,日,时,分,秒,事件类型\r\n";

        //代号,数据类型,数据长度,精度,单位

        //BCU、BMU 故障录波文件内容
        private readonly string FaultRecordText = @"电流,I16,1,1,A
最大电压,U16,1,1,V
最小电压,U16,1,1,V
最大温度,I8,1,1,℃
最小温度,I8,1,1,℃";

        //BCU 5min特性数据文件内容
        private readonly string FiveMinText = @"时间-年,U8,1,1,1,
时间-月,U8,1,1,1,
时间-日,U8,1,1,1,
时间-时,U8,1,1,1,
时间-分,U8,1,1,1,
时间-秒,U8,1,1,1,
电池簇累加电压,U16,1,0.1,V
电池簇SOC,U8,1,1,%
电池簇SOH,U8,1,1,%
电池簇电流,I16,1,0.01,A
继电器状态,U16,1,1,
风扇状态,U8,1,1,
继电器切断请求,U8,1,1,
BCU状态,U8,1,1,
充放电使能,U16,1,1,
保护信息1,U16,1,,
保护信息2,U16,1,,
保护信息3,U16,1,,
保护信息4,U16,1,,
告警信息1,U16,1,,
告警信息2,U16,1,,
最高pack电压,U16,1,0.1,V
最低pack电压,U16,1,0.1,V
最高pack电压序号,U8,1,1,号
最低pack电压序号,U8,1,1,号
簇号,U8,1,,号
簇内电池包个数,U8,1,1,个
高压绝缘阻抗,U16,1,0.1,V
保险丝后电压,U16,1,0,1,V
母线侧电压,U16,1,0.1,V
负载端电压,U16,1,0.1,V
辅助电源电压,U16,1,0.1,V
电池簇的充电电压,U16,1,0.1,V
电池簇充电电流上限,U16,1,0.1,A
电池簇放电电流上限,U16,1,0.1,A
电池簇的放电截止电压,U16,1,0.1,V
电池包均衡状态,U16,1,,
最高功率端子温度,I16,1,0.1,℃
环境温度,I16,1,0.1,℃
累计放电安时,U32,1,1,Ah
累计充电安时,U32,1,1,Ah
累计放电瓦时,U32,1,1,Wh
累计充电瓦时,U32,1,1,Wh
BMU编号,U8,1,,号
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
PACK1最大单体电压,U16,1,1,mV
PACK2最大单体电压,U16,1,1,mV
PACK3最大单体电压,U16,1,1,mV
PACK4最大单体电压,U16,1,1,mV
PACK5最大单体电压,U16,1,1,mV
PACK6最大单体电压,U16,1,1,mV
PACK7最大单体电压,U16,1,1,mV
PACK8最大单体电压,U16,1,1,mV
PACK9最大单体电压,U16,1,1,mV
PACK10最大单体电压,U16,1,1,mV
PACK1最小单体电压,U16,1,1,mV
PACK2最小单体电压,U16,1,1,mV
PACK3最小单体电压,U16,1,1,mV
PACK4最小单体电压,U16,1,1,mV
PACK5最小单体电压,U16,1,1,mV
PACK6最小单体电压,U16,1,1,mV
PACK7最小单体电压,U16,1,1,mV
PACK8最小单体电压,U16,1,1,mV
PACK9最小单体电压,U16,1,1,mV
PACK10最小单体电压,U16,1,1,mV
PACK1平均单体电压,U16,1,1,mV
PACK2平均单体电压,U16,1,1,mV
PACK3平均单体电压,U16,1,1,mV
PACK4平均单体电压,U16,1,1,mV
PACK5平均单体电压,U16,1,1,mV
PACK6平均单体电压,U16,1,1,mV
PACK7平均单体电压,U16,1,1,mV
PACK8平均单体电压,U16,1,1,mV
PACK9平均单体电压,U16,1,1,mV
PACK10平均单体电压,U16,1,1,mV
PACK1最大单体温度,I8,1,1,℃
PACK2最大单体温度,I8,1,1,℃
PACK3最大单体温度,I8,1,1,℃
PACK4最大单体温度,I8,1,1,℃
PACK5最大单体温度,I8,1,1,℃
PACK6最大单体温度,I8,1,1,℃
PACK7最大单体温度,I8,1,1,℃
PACK8最大单体温度,I8,1,1,℃
PACK9最大单体温度,I8,1,1,℃
PACK10最大单体温度,I8,1,1,℃
PACK1最小单体温度,I8,1,1,℃
PACK2最小单体温度,I8,1,1,℃
PACK3最小单体温度,I8,1,1,℃
PACK4最小单体温度,I8,1,1,℃
PACK5最小单体温度,I8,1,1,℃
PACK6最小单体温度,I8,1,1,℃
PACK7最小单体温度,I8,1,1,℃
PACK8最小单体温度,I8,1,1,℃
PACK9最小单体温度,I8,1,1,℃
PACK10最小单体温度,I8,1,1,℃
告警信息3,U16,1,,
告警信息4,U16,1,,
RSV2,U32,1,,";
        //BMU 5min特性数据文件内容
        private readonly string FiveMinText_BMU = @"时间-年,U8,1,1,1,
时间-月,U8,1,1,1,
时间-日,U8,1,1,1,
时间-时,U8,1,1,1,
时间-分,U8,1,1,1,
时间-秒,U8,1,1,1,
电池采样电压,U16,1,1,mV
电池累计电压,U16,1,1,mV
SOC显示值,U8,1,1,%
SOH显示值,U8,1,1,%
SOC计算值,U32,1,0.001,%
SOH计算值,U32,1,0.001,%
电池电流,I16,1,0.01,A
最高单体电压,U16,1,1,mV
最低单体电压,U16,1,1,mV
最高单体电压序号,U8,1,,
最低单体电压序号,U8,1,,
最高单体温度,I16,1,0.1,℃
最低单体温度,I16,1,0.1,℃
最高单体温度序号,U8,1,,
最低单体温度序号,U8,1,,
BMU编号,U8,1,,
系统状态,U8,1,,
充放电使能,U16,1,,
切断请求,U8,1,,
关机请求,U8,1,,
充电电流上限,U16,1,1,A
放电电流上限,U16,1,1,A
保护1,U16,1,,
保护2,U16,1,,
告警1,U16,1,,
告警2,U16,1,,
故障1,U16,1,,
故障2,U16,1,,
告警3,U16,1,,
告警4,U16,1,,
故障3,U16,1,,
故障4,U16,1,,
主动均衡状态,U16,1,,
均衡母线电压,U16,1,,mV
均衡母线电流,I16,1,,mA
辅助供电电压,U16,1,,mV
满充容量,U16,1,,Ah
循环次数,U16,1,,
累计放电安时,U32,1,1,Ah
累计充电安时,U32,1,1,Ah
累计放电瓦时,U32,1,1,Wh
累计充电瓦时,U32,1,1,Wh
环境温度,I16,1,0.1,℃
dcdc温度1,I16,1,0.1,℃
均衡温度1,I16,1,0.1,℃
均衡温度2,I16,1,0.1,℃
功率端子温度1,I16,1,0.1,℃
功率端子温度2,I16,1,0.1,℃
其他温度1,I16,1,0.1,℃
其他温度2,I16,1,0.1,℃
其他温度3,I16,1,0.1,℃
其他温度4,I16,1,0.1,℃
1-16串均衡状态,U16,1,1,
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
单体温度1,I16,1,0.1,℃
单体温度2,I16,1,0.1,℃
单体温度3,I16,1,0.1,℃
单体温度4,I16,1,0.1,℃
单体温度5,I16,1,0.1,℃
单体温度6,I16,1,0.1,℃
单体温度7,I16,1,0.1,℃
单体温度8,I16,1,0.1,℃
单体温度9,I16,1,0.1,℃
单体温度10,I16,1,0.1,℃
单体温度11,I16,1,0.1,℃
单体温度12,I16,1,0.1,℃
单体温度13,I16,1,0.1,℃
单体温度14,I16,1,0.1,℃
单体温度15,I16,1,0.1,℃
单体温度16,I16,1,0.1,℃
RSV1,U16,1,,
RSV2,U32,1,,
RSV3,U32,1,,
RSV4,U32,1,,
RSV5,U32,1,,
RSV6,U32,1,,";
        //BCU、BMU 历史事件文件内容
        private readonly string HistoryEventText = @"时间-年,U8,1,1,1,
时间-月,U8,1,1,1,
时间-日,U8,1,1,1,
时间-时,U8,1,1,1,
时间-分,U8,1,1,1,
时间-秒,U8,1,1,1,
事件类型,U16,1,1,";



        StepRemark stepFlag;
        bool isResponse;

        //int slaveAddress = -1;
        //int subDeviceAddress = -1;
        int fileNumber = -1;
        int readType = -1;

        int questCycle = 0;
        int fileOffset = 200;
        int dataLength = 200;
        int fileOffset_BCU = 200;
        //int readCount = 0;
        int readIndex = 0;

        int fileSize = 0;
        string textStr = "";
        string headStr = "";
        string filePath = "";
        string fileName = "";
        List<byte> dataBuffer = new List<byte>();
        byte SlaveAddress =0x81;

        public FileTransmit_BMS_ViewModel()
        {

            baseCanHelper = new CommandOperation(BMSConfig.ConfigManager).baseCanHelper;
            cts = new CancellationTokenSource();
            if (FileTransmitLogList == null) FileTransmitLogList = new ObservableCollection<FileTransmitLogData>();
        }


        public void Load()
        {

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

       
        public ICommand FileTransmitCmd => new RelayCommand(FileTransmit);

        public void FileTransmit()
        { 
            if (SelectedModeName == "BCU") subDeviceAddress = 0;
            //FileTransmitLogList.Clear();//暂时不清空日志
            // 获取文件编号/索引
            fileNumber = FileNumberList.IndexOf(SelectedFileNumber);

            //设备地址
            SlaveAddress = SelectedModeName == "BCU" ? (byte)slaveAddress : (byte)subDeviceAddress;

            //读取方式—0，直接读取该文件所有内容；1，由发起方选择读取该文件内容
            readType = IsReadAll_Checked ? 0 : 1;
            //if (readType == 1)
            //{
            //    if (!int.TryParse(txtStartLocal.Text, out readIndex))
            //    {
            //        MessageBox.Show("起始地址错误，终止操作！");
            //        return;
            //    }

            //    fileOffset *= readIndex;//变更起始偏移地址
            //}

            StartTransmit();
        }

        private void StartTransmit()
        {
            int recount = 0;
            if (!state)
            {
                state = true;
                stepFlag = StepRemark.读文件指令帧;
                _token = new CancellationTokenSource();

                Task.Factory.StartNew(() =>
                {
                    while (!_token.IsCancellationRequested)
                    {
                        switch (stepFlag)
                        {
                            case StepRemark.None:
                                break;
                            case StepRemark.读文件指令帧:
                                if (!ReadFileCommand())
                                {
                                    recount++;
                                    Debug.WriteLine("Read file command:fail!");
                                }
                                break;
                            case StepRemark.读文件数据内容帧:
                                if (questCycle > 1)
                                {
                                    Thread.Sleep(100);
                                    if (!ReadFileDataContent())
                                    {
                                        recount++;
                                        Debug.WriteLine("Read file data content:fail!");

                                    }
                                    else
                                    {
                                        questCycle--;

                                        switch(SelectedModeName)
                                        {
                                            case "BCU":
                                                fileOffset_BCU += dataLength;
                                                break;
                                            case "BMU":
                                                fileOffset += dataLength;
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    Debug.WriteLine("End read file data content...");
                                    stepFlag = StepRemark.查询完成状态帧;
                                }
                                break;
                            case StepRemark.查询完成状态帧:
                                if (QueryCompletionStatus())
                                {
                                    Debug.WriteLine("query status:success!");
                                    EndTransmit();
                                }
                                else
                                {
                                    recount++;
                                }
                                break;
                            default:
                                Debug.WriteLine("error:not normal!");
                                break;
                        }

                        if (recount == 5)
                        {
                            string tag = "";
                            if (stepFlag == StepRemark.读文件指令帧)
                            {
                                tag = "F0";
                            }
                            else if (stepFlag == StepRemark.读文件数据内容帧)
                            {
                                tag = "F1";
                            }
                            else if (stepFlag == StepRemark.查询完成状态帧)
                            {
                                tag = "F4";
                            }

                            AddLog(System.DateTime.Now.ToString("HH:mm:ss:fff"), tag, "终止传输，重试执行5次失败！");
                            recount = 0;
                            EndTransmit();
                            break;
                        }

                        Thread.Sleep(20);
                    }
                });
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
                Application.Current.Dispatcher.Invoke(() =>
                {
                    IsTransmitEnable = false;
                });

                await Task.Delay(1000 * 15);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    IsTransmitEnable = true;
                });

            });

            //关闭线程，清除缓存
            _token.Cancel();
            dataBuffer.Clear();

            state = false;
            isResponse = false;
            stepFlag = StepRemark.None;

            //fileNumber = -1;
            //readType = -1;

            questCycle = 0;
            fileOffset = 200;
            dataLength = 200;

            fileSize = 0;
            textStr = "";
            headStr = "";
            filePath = "";
            fileName = "";
        }


        private bool ReadFileCommand()
        {
            
            byte[] canid = { 0xE0, SlaveAddress, 0xF0, 0x07 };

            byte[] data = new byte[8];
            data[0] = 0x0;
            data[1] = (byte)subDeviceAddress;
            data[2] = (byte)fileNumber;
            data[3] = (byte)readType;

            baseCanHelper.Send(data, canid);
            AddLog(System.DateTime.Now.ToString("HH:mm:ss:fff"), "F0", byteToHexString(data));
            return CheckResponse();
        }

        private bool ReadFileDataContent()
        {
            byte[] canid = { 0xE0, SlaveAddress, 0xF1, 0x07 };

            byte[] data = new byte[8];
            data[0] = 0x0;
            data[1] = (byte)subDeviceAddress;
            data[2] = (byte)fileNumber;
            data[3] = (byte)(fileOffset_BCU & 0xff);
            data[4] = (byte)(fileOffset_BCU >> 8);
            data[5] = (byte)(fileOffset_BCU >> 16);
            data[6] = (byte)(dataLength & 0xff);
            data[7] = (byte)(dataLength >> 8);

            baseCanHelper.Send(data, canid);
            AddLog(System.DateTime.Now.ToString("HH:mm:ss:fff"), "F1", byteToHexString(data));
            return CheckResponse();
        }

        private bool QueryCompletionStatus()
        {
            byte[] canid = { 0xE0, SlaveAddress, 0xF4, 0x07 };

            byte[] data = new byte[8];
            data[0] = 0x0;
            data[1] = (byte)subDeviceAddress;
            data[2] = (byte)fileNumber;
            data[3] = (byte)0x01;

            baseCanHelper.Send(data, canid);
            AddLog(System.DateTime.Now.ToString("HH:mm:ss:fff"), "F4", byteToHexString(data));
            return CheckResponse();
        }

        private void AnalysisData(uint id, byte[] data)
        {
            id = id | 0xff;

            switch (stepFlag)
            {
                case StepRemark.None:
                    break;
                case StepRemark.读文件指令帧:
                    if (id == 0x7F0E0FF)
                    {
                        AddLog(System.DateTime.Now.ToString("HH:mm:ss:fff"), id.ToString("X8"), byteToHexString(data));

                        if (data[3] == 0x0)
                        {
                            isResponse = true;
                            string headName = "";

                            if (fileNumber == 0 || fileNumber == 1 || fileNumber == 2)
                            {
                                headStr = FaultRecordStr;
                                headName = "故障录波";
                            }
                            else if (fileNumber == 3)
                            {
                                headStr = SelectedModeName == "BCU" ? FiveMinHeadStr : FiveMinHeadStr_BMU;
                                //headStr = FiveMinHeadStr;
                                headName = "五分钟特性数据";
                            }
                            else if (fileNumber == 4)
                            {
                                headStr = "none";
                                headName = "运行日志";
                            }
                            else if (fileNumber == 5)
                            {
                                headStr = HistoryEventStr;
                                headName = "历史事件";
                            }

                            fileSize = Convert.ToInt32(data[6].ToString("X2") + data[5].ToString("X2") + data[4].ToString("X2"), 16);

                            if (!string.IsNullOrEmpty(headStr))
                            {
                                fileName = string.Format("{0}_{1}_{2}_{3}", headName, SelectedModeName, subDeviceAddress, System.DateTime.Now.ToString("yy-MM-dd-HH-mm-ss"));
                                filePath = $"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}//Log//{fileName}.csv";

                                if ((fileNumber < 3 && fileNumber >= 0) || fileNumber == 5)
                                {
                                    fileOffset = 8;
                                    dataLength = 8;
                                }
                                else if (fileNumber == 3 || fileNumber == 4)
                                {
                                    if (fileNumber == 4)
                                    {
                                        filePath = $"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}//Log//{fileName}.txt";
                                    }


                                    fileOffset = 200;
                                    dataLength = 200;

                                }

                                switch(SelectedModeName)
                                {
                                    case "BCU":
                                        if (readType == 0)
                                        {
                                            fileOffset_BCU = fileOffset;
                                            questCycle = fileSize % fileOffset == 0 ? fileSize / fileOffset + 1 : (fileSize / fileOffset);//一般结果为true
                                            //questCycle = fileSize % fileOffset == 0 ? (fileSize / fileOffset - 1) : fileSize / fileOffset;//一般结果为true
                                        }
                                        else
                                        {
                                            fileOffset_BCU = ((fileSize - readCount * fileOffset) < 0) ? fileOffset : (fileSize - readCount * fileOffset);
                                            questCycle = (fileSize - fileOffset_BCU) % fileOffset == 0 ? ((fileSize - fileOffset_BCU) / fileOffset) + 1 : ((fileSize - fileOffset_BCU) / fileOffset);

                                        }                                      
                                        break;  
                                    case "BMU":
                                        questCycle = fileSize % fileOffset == 0 ? (fileSize / fileOffset - 1) : fileSize / fileOffset;//一般结果为true
                                        break;
                                }
                            }

                            if (!File.Exists(filePath))
                            {
                                File.AppendAllText(filePath, headStr);
                            }

                            stepFlag = StepRemark.读文件数据内容帧;
                        }
                    }
                    break;
                case StepRemark.读文件数据内容帧:
                    if (id == 0x7F2E0FF)
                    {
                        dataBuffer.AddRange(data);

                        if (fileNumber == 0 || fileNumber == 1 || fileNumber == 2)
                        {
                            if (dataBuffer.Count == 8)
                            {
                                textStr = FaultRecordText;
                            }
                        }
                        else if (fileNumber == 3)
                        {
                            if (dataBuffer.Count == 200)
                            {
                                textStr = FiveMinText;

                                //测试打印
                                Debug.WriteLine(byteToHexString(dataBuffer.ToArray()));
                            }
                        }
                        else if (fileNumber == 5)
                        {
                            textStr = HistoryEventText;
                        }
                    }
                    else if (id == 0x7F3E0FF)
                    {
                        isResponse = true;
                        AddLog(System.DateTime.Now.ToString("HH:mm:ss:fff"), id.ToString("X8"), byteToHexString(data));

                        if (!string.IsNullOrEmpty(textStr))
                        {
                            string getValue = ToAnalysis(textStr, dataBuffer.ToArray());
                            dataBuffer.Clear();

                            File.AppendAllText(filePath, getValue + "\r\n");
                        }
                        else
                        {
                            if (fileNumber == 4)
                            {
                                string sbContent = "";
                                for (int i = 0; i < dataBuffer.Count; i++)
                                {
                                    String asciiStr = ((char)dataBuffer[i]).ToString();//十六进制转ASCII码
                                    sbContent += asciiStr;
                                }
                                TxtHelper.FileWrite(filePath, sbContent);
                                Debug.WriteLine(sbContent);
                            }
                        }
                    }
                    break;
                case StepRemark.查询完成状态帧:
                    if (id == 0x7F4E0FF)
                    {
                        AddLog(System.DateTime.Now.ToString("HH:mm:ss:fff"), id.ToString("X8"), byteToHexString(data));

                        if (data[3] == 0x0)
                        {
                            AddLog(System.DateTime.Now.ToString("HH:mm:ss:fff"), id.ToString("X8"), "已完成本次读取...");
                            isResponse = true;
                            EndTransmit();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private string ToAnalysis(string textStr, byte[] dataBuffer)
        {
            StringBuilder valList = new StringBuilder();
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
                        case "U16":
                            byte[] bytes = new byte[2];

                            int byte1 = bytes[0] = dataBuffer[index++];
                            int byte2 = bytes[1] = dataBuffer[index++];
                            val = $"0x{(byte1 + (byte2 << 8)).ToString("X4")}";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (model.DataType)
                    {
                        case "U8":
                            val = Convert.ToByte(dataBuffer[index++]);
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

                valList.Append(model.Value + ",");

                // toHex=true 直接按16进制显示
                switch (SelectedModeName)
                {
                    case "BCU":

                        if (model.Name == "电池簇电流")
                        {
                            toHex = true;
                        }
                        else if (model.Name == "告警信息2")
                        {
                            toHex = false;
                        }
                        else if (model.Name == "PACK10最小单体温度")
                        {
                            toHex = true;
                        }
                        else if (model.Name == "告警信息4")
                        {
                            toHex = false;
                        }

                        break;
                    case "BMU":

                        if (model.Name == "BMU编号")
                        {
                            toHex = true;
                        }
                        else if (model.Name == "关机请求")
                        {
                            toHex = false;
                        }
                        else if (model.Name == "放电电流上限")
                        {
                            toHex = true;
                        }
                        else if (model.Name == "主动均衡状态")
                        {
                            toHex = false;
                        }
                        else if (model.Name == "其他温度4")
                        {
                            toHex = true;
                        }
                        else if (model.Name == "1-16串均衡状态")
                        {
                            toHex = false;
                        }
                        break;
                    default:
                        break;
                }
            }

            return valList.ToString();
        }

        private List<ProtocolModel> ToProtocol(string text)
        {
            List<ProtocolModel> protocols = new List<ProtocolModel>();

            string[] textStr = text.Split('\n');

            for (int i = 0; i < textStr.Length; i++)
            {
                string[] datas = textStr[i].Split(',');

                ProtocolModel model = new ProtocolModel();
                model.Name = datas[0];
                model.DataType = datas[1];
                model.DataLength = Convert.ToInt32(datas[2]);
                model.Accuracy = datas[3] == "" ? 1 : Convert.ToDouble(datas[3]);
                model.Unit = datas[4].ToString();

                protocols.Add(model);
            }

            return protocols;
        }

        /// <summary>
        /// 文件传输日志
        /// </summary>
        /// <param name="date"></param>
        /// <param name="id"></param>
        /// <param name="context"></param>
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


        private string byteToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder();

            foreach (byte b in data)
            {
                sb.Append(b.ToString("X2") + " ");
            }

            return sb.ToString();
        }

        private bool CheckResponse()
        {
            bool isOk = isResponse = false;
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (true)
            {
                if (isResponse)
                {
                    isOk = true;
                    break;
                }

                if (stopwatch.ElapsedMilliseconds > 1000)
                {
                    stopwatch.Stop();
                    break;
                }
            }

            return isOk;
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

        enum StepRemark
        {
            None,
            读文件指令帧,
            读文件数据内容帧,
            查询完成状态帧
        }

        class ProtocolModel
        {
            public string Name;
            public string DataType;
            public int DataLength;
            public double Accuracy;
            public string Unit;
            public object Value;
        }

        public class TxtHelper
        {
            public static void FileWrite(string path, string content)
            {
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8))
                    {
                        sw.Write(content);
                    }
                }
                else
                {
                    FileStream fs = new FileStream(path, FileMode.Append);
                    StreamWriter sw1 = new StreamWriter(fs, Encoding.UTF8);
                    sw1.Write(content);
                    sw1.Close();
                }
            }
        }


    }
}
