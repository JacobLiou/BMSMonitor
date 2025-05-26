using Sofar.ConnectionLibs.CAN;
using SofarBMS.UI;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace SofarBMS.ui
{
    public partial class CBSFileTransmit : UserControl
    {
        public static CancellationTokenSource cts = new CancellationTokenSource();
        EcanHelper ecanHelper = EcanHelper.Instance;
        int slaveAddress = -1;
        int bcuAddress = -1;
        int bmuAddress = -1;
        int questCycle = -1;

        int dataLength = -1;//数据长度
        int fileNumber = -1;//文件编码
        int fileOffset = -1;//文件偏移量
        int readType = -1;//读取类型，全部读0，部分读1
        int readIndex = 0;//读取起始
        int readCount = 0;//读取数量
        int fileSize = -1;//文件大小
        string filePath = "";//文件路径
        string fileName = "";//文件名称
        string dataHeader = "";
        string dataText = "";
        List<byte> dataBuffer = new List<byte>();//缓存数据

        private bool state = false;
        private StepRemark stepFlag;
        private CancellationTokenSource _token;
        private readonly object _syncLock = new object();
        private int _recount = 0;


        #region //代号,数据类型,数据长度,精度,单位
        private readonly string HistoryEventStr = "时间年,月,日,时,分,秒,事件类型\r\n";
        private readonly string HistoryEventText = @"时间-年,U8,1,1,1,
时间-月,U8,1,1,1,
时间-日,U8,1,1,1,
时间-时,U8,1,1,1,
时间-分,U8,1,1,1,
时间-秒,U8,1,1,1,
事件类型,U16,1,1,";

        private readonly string FaultRecordStr = "电流(A),最大电压(mV),最小电压(mV),最大温度(℃),最小温度(℃)\r\n";
        private readonly string FaultRecordText = @"电流,I16,1,1,A
最大电压,U16,1,1,V
最小电压,U16,1,1,V
最大温度,I8,1,1,℃
最小温度,I8,1,1,℃";

        private readonly string BcuFiveMinHeadStr = "时间年,月,日,时,分,秒,电池簇累加电压(V),SOC(%),SOH(%),电池簇电流(A),继电器状态,风扇状态,继电器切断请求,BCU状态,保留,充放电使能,保护信息1,保护信息2,保护信息3,保护信息4,告警信息1,告警信息2,最高pack电压(V),最低pack电压(V),最高pack电压序号,最低pack电压序号,簇号,簇内电池包个数,高压绝缘阻抗(V),保险丝后电压(V),母线侧电压(V),负载端电压(V),辅助电源电压(V),电池簇的充电电压(V),电池簇充电电流上限(A),电池簇放电电流上限(A),电池簇的放电截止电压(V),电池包均衡状态,最高功率端子温度(℃),环境温度(℃),保留,保留,累计充电安时(Ah),累计放电安时(Ah),累计充电瓦时(Wh),累计放电瓦时(Wh),PACK1最大单体电压(mV),PACK2最大单体电压(mV),PACK3最大单体电压(mV),PACK4最大单体电压(mV),PACK5最大单体电压(mV),PACK6最大单体电压(mV),PACK7最大单体电压(mV),PACK8最大单体电压(mV),PACK9最大单体电压(mV),PACK10最大单体电压(mV),PACK11最大单体电压(mV),PACK12最大单体电压(mV),PACK1最小单体电压(mV),PACK2最小单体电压(mV),PACK3最小单体电压(mV),PACK4最小单体电压(mV),PACK5最小单体电压(mV),PACK6最小单体电压(mV),PACK7最小单体电压(mV),PACK8最小单体电压(mV),PACK9最小单体电压(mV),PACK10最小单体电压(mV),PACK11最小单体电压(mV),PACK12最小单体电压(mV),PACK1平均单体电压(mV),PACK2平均单体电压(mV),PACK3平均单体电压(mV),PACK4平均单体电压(mV),PACK5平均单体电压(mV),PACK6平均单体电压(mV),PACK7平均单体电压(mV),PACK8平均单体电压(mV),PACK9平均单体电压(mV),PACK10平均单体电压(mV),PACK11平均单体电压(mV),PACK12平均单体电压(mV),PACK1最大单体温度(℃),PACK2最大单体温度(℃),PACK3最大单体温度(℃),PACK4最大单体温度(℃),PACK5最大单体温度(℃),PACK6最大单体温度(℃),PACK7最大单体温度(℃),PACK8最大单体温度(℃),PACK9最大单体温度(℃),PACK10最大单体温度(℃),PACK11最大单体温度(℃),PACK12最大单体温度(℃),PACK1最小单体温度(℃),PACK2最小单体温度(℃),PACK3最小单体温度(℃),PACK4最小单体温度(℃),PACK5最小单体温度(℃),PACK6最小单体温度(℃),PACK7最小单体温度(℃),PACK8最小单体温度(℃),PACK9最小单体温度(℃),PACK10最小单体温度(℃),PACK11最小单体温度(℃),PACK12最小单体温度(℃),BMU保护信息1,BMU保护信息2,BMU故障信息1,BMU故障信息2\r\n";
        private readonly string BcuFiveMinText = @"时间-年,U8,1,1,1,
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
保留,U8,1,1,
充放电使能,U16,1,1,
告警信息1,U16,1,,
保护信息1,U16,1,,
故障信息1,U16,1,,
故障信息2,U16,1,,
BMU告警信息1,U16,1,,
BMU告警信息2,U16,1,,
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
最高功率端子温度,I8,1,1,℃
环境温度,I8,1,1,℃
保留,U8,1,1,
保留,U8,1,1,
累计放电安时,U32,1,1,Ah
累计充电安时,U32,1,1,Ah
累计放电瓦时,U32,1,1,Wh
累计充电瓦时,U32,1,1,Wh
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
PACK11最大单体电压,U16,1,1,mV
PACK12最大单体电压,U16,1,1,mV
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
PACK11最小单体电压,U16,1,1,mV
PACK12最小单体电压,U16,1,1,mV
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
PACK11平均单体电压,U16,1,1,mV
PACK12平均单体电压,U16,1,1,mV
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
PACK11最大单体温度,I8,1,1,℃
PACK12最大单体温度,I8,1,1,℃
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
PACK11最小单体温度,I8,1,1,℃
PACK12最小单体温度,I8,1,1,℃
BMU保护信息1,U16,1,,
BMU保护信息2,U16,1,,
BMU故障信息1,U16,1,,
BMU故障信息2,U16,1,,";

        private readonly string BmuFiveMinHeadStr = "时间年,月,日,时,分,秒,电池采集电压(mV),电池累计电压(mV),SOC显示值(%),SOH显示值(%),SOC计算值,SOH计算值,电池电流(A),最高单体电压(mV),最低单体电压(mV),最高单体电压序号,最低单体电压序号,最高单体温度(℃),最低单体温度(℃),最高单体温度序号,最低单体温度序号,BMU编号,系统状态,充放电使能,切断请求,关机请求,充电电流上限(A),放电电流上限(A),保护1,保护2,告警1,告警2,故障1,故障2,故障1,故障2,故障3,故障4,主动均衡状态,均衡母线电压(mV),均衡电池电流(mA),辅助供电电压(mV),满充容量(Ah),循环次数,累计放电安时(Ah),累计充电安时(Ah),累计放电瓦时(Wh),累计充电瓦时(Wh),环境温度(℃),DCDC温度1(℃),均衡温度1(℃),均衡温度2(℃),功率端子温度1(℃),功率端子温度2(℃),其他温度1(℃),其他温度2(℃),其他温度3(℃),其他温度4(℃),1-16串均衡状态,单体电压1(mV),单体电压2(mV),单体电压3(mV),单体电压4(mV),单体电压5(mV),单体电压6(mV),单体电压7(mV),单体电压8(mV),单体电压9(mV),单体电压10(mV),单体电压11(mV),单体电压12(mV),单体电压13(mV),单体电压14(mV),单体电压15(mV),单体电压16(mV),单体温度1(℃),单体温度2(℃),单体温度3(℃),单体温度4(℃),单体温度5(℃),单体温度6(℃),单体温度7(℃),单体温度8(℃),单体温度9(℃),单体温度10(℃),单体温度11(℃),单体温度12(℃),单体温度13(℃),单体温度14(℃),单体温度15(℃),单体温度16(℃),RSV1,RSV2,RSV3,RSV4,RSV5,RSV6\r\n";
        private readonly string BmuFiveMinText = @"时间-年,U8,1,1,1,
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
电芯电压温度告警保护,U16,1,,
过流环境温度告警保护,U16,1,,
电路异常告警保护1,U16,1,,
电路异常告警保护2,U16,1,,
主动均衡告警,U16,1,,
电芯失能故障和电芯温度提示,U16,1,,
采样线异常和加热膜异常,U16,1,,
严重锁死故障,U16,1,,
故障保留1,U16,1,,
故障保留2,U16,1,,
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
保留温度1,I16,1,0.1,℃
保留温度2,I16,1,0.1,℃
保留温度3,I16,1,0.1,℃
保留温度4,I16,1,0.1,℃
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

        #endregion

        private string currentMode = "";
        private Dictionary<int, FileTypeConfig> bcuFileTypeConfigs = new();
        private Dictionary<int, FileTypeConfig> bmuFileTypeConfigs = new();

        public CBSFileTransmit()
        {
            InitializeComponent();
            this.Load += CBSFileTransmit_Load;
        }

        private void CBSFileTransmit_Load(object? sender, EventArgs e)
        {
            List<Mode> modes = new List<Mode>()
            {
                new Mode{
                    Name ="BCU",
                    FileModels= new List<string>()
                    {
                        "BCU通讯板故障录波",
                        "BCU功率板故障录波",
                        "保留",
                        "5min特性数据文件",
                        "运行日志文件",
                        "历史事件文件"
                    }
                },
                new Mode{
                    Name = "BMU",
                    FileModels=new List<string>()
                        {
                        "故障录波文件1",
                        "保留",
                        "保留",
                        "5min特性数据文件",
                        "运行日志文件",
                        "历史事件文件"
                        }
                }
            };

            cbbModeName.DisplayMember = "Name";
            cbbModeName.DataSource = modes;

            // 触发初始选择
            cbbModeName_SelectedIndexChanged(null, null);

            // 创建文件类型配置字典
            bcuFileTypeConfigs = new Dictionary<int, FileTypeConfig>()
            {
                { 0, new FileTypeConfig() { HeadName = "BCU通讯板故障录波", Extension = ".csv", DefaultOffset = 200, DefaultLength = 200, HeadStr = BcuFiveMinHeadStr, Content = BcuFiveMinText } },
                { 1, new FileTypeConfig() { HeadName = "BCU功率板故障录波", Extension = ".csv", DefaultOffset = 8, DefaultLength = 8, HeadStr = FaultRecordStr, Content = FaultRecordText }},
                { 3, new FileTypeConfig() { HeadName = "5min特性数据文件", Extension = ".csv", DefaultOffset = 200, DefaultLength = 200, HeadStr = BcuFiveMinHeadStr, Content = BcuFiveMinText }},
                { 4, new FileTypeConfig() { HeadName = "运行日志文件", Extension = ".txt", DefaultOffset = 256, DefaultLength = 256, HeadStr = null, Content = null }},
                { 5, new FileTypeConfig() {HeadName = "历史事件文件", Extension = ".csv", DefaultOffset = 8, DefaultLength = 8, HeadStr = HistoryEventStr, Content = HistoryEventText}},
            };

            bmuFileTypeConfigs = new Dictionary<int, FileTypeConfig>()
            {
                { 0, new FileTypeConfig() { HeadName = "故障录波文件1", Extension = ".csv", DefaultOffset = 200, DefaultLength = 200, HeadStr = BmuFiveMinHeadStr, Content = BmuFiveMinText }},
                { 3, new FileTypeConfig() { HeadName = "5min特性数据文件", Extension = ".csv", DefaultOffset = 200, DefaultLength = 200, HeadStr = BmuFiveMinHeadStr, Content = BmuFiveMinText }},
                { 4, new FileTypeConfig() { HeadName = "运行日志文件", Extension = ".txt", DefaultOffset = 200, DefaultLength = 200, HeadStr = null, Content = null }},
                { 5, new FileTypeConfig() { HeadName = "历史事件文件", Extension = ".csv", DefaultOffset = 8, DefaultLength = 8, HeadStr = HistoryEventStr, Content = HistoryEventText }}
            };

            // 控件初始化
            this.txtBCUAddress.Text = FrmMain.BCU_ID.ToString();
            this.txtBMUAddress.Text = "0"; //FrmMain.BMS_ID.ToString();
            this.ckReadAll.Checked = true;

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

        private void AnalysisData(uint id, byte[] data)
        {
            id |= 0xFF;

            switch (stepFlag)
            {
                case StepRemark.读文件指令帧:
                    if (id == 0x7F0E0FF && data[3] == 0x0)
                    {
                        AddLog(System.DateTime.Now.ToString("HH:mm:ss:fff"), id.ToString("X8"), byteToHexString(data));

                        FileTypeConfig config = null;
                        // 获取文件配置
                        if (currentMode == "BCU")
                        {
                            if (!bcuFileTypeConfigs.TryGetValue(fileNumber, out config))
                            {
                                AddLog(DateTime.Now.ToString("HH:mm:ss"), id.ToString("X8"), $"ERROR:未知文件类型: {fileNumber}");
                                return;
                            }

                            
                        }
                        else if (currentMode == "BMU")
                        {
                            if (!bmuFileTypeConfigs.TryGetValue(fileNumber, out config))
                            {
                                AddLog(DateTime.Now.ToString("HH:mm:ss"), id.ToString("X8"), $"ERROR:未知文件类型: {fileNumber}");
                                return;
                            }
                        }

                        string headStr = config.HeadStr;
                        string headName = config.HeadName;

                        // 计算文件大小（改进字节处理）
                        fileSize = (data[6] << 16) | (data[5] << 8) | data[4];

                        // 安全构建文件路径
                        fileName = $"{headName}_{cbbModeName.Text.Trim()}_{slaveAddress}_{DateTime.Now:yy-MM-dd-HH-mm-ss}";
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
                                questCycle = fileSize / config.DefaultOffset;
                            }
                            else
                            {
                                questCycle = fileSize / config.DefaultOffset + 1;
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
                    if (id == 0x7F2E0FF)
                    {
                        dataBuffer.AddRange(data);
                    }
                    else if (id == 0x7F3E0FF)
                    {
                        AddLog(System.DateTime.Now.ToString("HH:mm:ss:fff"), id.ToString("X8"), byteToHexString(data));

                        FileTypeConfig config = null;
                        if (currentMode == "BCU")
                        {
                            // 获取文件配置
                            if (!bcuFileTypeConfigs.TryGetValue(fileNumber, out config))
                            {
                                AddLog(DateTime.Now.ToString("HH:mm:ss"), id.ToString("X8"), $"ERROR:未知文件类型: {fileNumber}");
                                return;
                            }

                            
                        }
                        else if (currentMode == "BMU")
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
            if (dataBuffer.Length == 0) return string.Empty;

            var protocols = ToProtocol(textStr);
            var result = new StringBuilder();
            int index = 0;

            for (int i = 0; i < protocols.Count; i++)
            {
                ProtocolModel model = protocols[i];
                object val = null;
                bool toHex = false;

                if (toHex)
                {
                    switch (model.DataType)
                    {
                        case "I8":
                            val = (sbyte)dataBuffer[index++];
                            break;
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
                        case "I8":
                            val = (sbyte)dataBuffer[index++];
                            break;
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

                result.Append(model.Value + ",");

                // toHex=true 直接按16进制显示
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
            }

            return result.ToString().TrimEnd(',');
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

        private void cbbModeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mode selectedCountry = cbbModeName.SelectedItem as Mode;
            if (selectedCountry != null)
            {
                cbbFileNumber.DataSource = selectedCountry.FileModels;
            }

            if (cbbModeName.SelectedIndex == 0)
            {
                txtBCUAddress.ReadOnly = false;
                txtBMUAddress.Text = "0";
            }
            else if (cbbModeName.SelectedIndex == 1)
            {
                txtBMUAddress.Text = FrmMain.BMS_ID.ToString();
                txtBCUAddress.ReadOnly = true;
            }
        }

        private void ckReadAll_CheckedChanged(object sender, EventArgs e)
        {
            if (ckReadAll.Checked)
            {
                txtReadCount.ReadOnly = true;
                txtStartLocal.ReadOnly = true;
            }
            else
            {
                txtReadCount.ReadOnly = false;
                txtStartLocal.ReadOnly = false;
            }
        }

        private void btnFileTransmit_Click(object sender, EventArgs e)
        {
            // 防呆，ID编号待确认范围
            bool selState = ckReadAll.Checked;
            if (!selState)
            {
                if (string.IsNullOrEmpty(txtReadCount.Text) || string.IsNullOrEmpty(txtStartLocal.Text))
                {
                    MessageBox.Show("部分读取必须填写读取的起始位置和条数值", "操作异常");
                    return;
                }
            }

            bool result1 = int.TryParse(txtBCUAddress.Text.Trim(), out bcuAddress);
            bool result2 = int.TryParse(txtBMUAddress.Text.Trim(), out bmuAddress);
            if (!result1 || !result2)
            {
                MessageBox.Show("BCU和BMU的地址必须为整数", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 赋值读取方式，文件类型
            readType = selState ? 0 : 1;
            fileNumber = cbbFileNumber.SelectedIndex;

            // 所用字段属性进行赋值操作
            currentMode = cbbModeName.Text.ToString();
            string currentFileName = cbbFileNumber.Text.ToString();
            if (currentMode == "BCU")
            {
                FileTypeConfig model = bcuFileTypeConfigs.Values.Where(b => b.HeadName == currentFileName).FirstOrDefault();
                if (model != null)
                {
                    dataLength = model.DefaultLength;
                    dataHeader = model.HeadStr;
                    dataText = model.Content;
                }

                if (currentFileName.Equals("5min特性数据文件") && txtBMUAddress.Text.Trim() != "0")
                {
                    dataHeader = BmuFiveMinHeadStr;
                }

                slaveAddress = bcuAddress;
            }
            else if (currentMode == "BMU")
            {
                FileTypeConfig model = bmuFileTypeConfigs.Values.Where(b => b.HeadName == currentFileName).FirstOrDefault();
                if (model != null)
                {
                    dataLength = model.DefaultLength;
                    dataHeader = model.HeadStr;
                    dataText = model.Content;
                }

                slaveAddress = bmuAddress;
            }

            // 启动文件传输流程
            if (!state)
            {
                lock (_syncLock)
                {
                    state = true;
                    stepFlag = StepRemark.读文件指令帧;
                    _token = new CancellationTokenSource();

                    btnFileTransmit.Text = "停止读取";
                }

                ReadFileCommand();
                /*while (!_token.Token.IsCancellationRequested && !isResponse)
                {
                    
                    Thread.Sleep(100);

                    if (_recount >= 5)//超过五次请求
                    {
                        EndTransmit();
                        return;
                    }
                }*/
            }
            else
            {
                EndTransmit();
            }
        }

        // 开始/结束文件传输
        /*private async void StartTransmit()
        {
            if (state) return;

            lock (_syncLock)
            {
                state = true;
                stepFlag = StepRemark.读文件指令帧;
                _token = new CancellationTokenSource();
            }

            try
            {
                while (!_token.Token.IsCancellationRequested)
                {
                    bool stepResult = false;
                    StepRemark currentStep;

                    lock (_syncLock)
                    {
                        currentStep = stepFlag;
                    }

                    switch (currentStep)
                    {
                        case StepRemark.读文件指令帧:
                            stepResult = await ReadFileCommandAsync();
                            break;
                        case StepRemark.读文件数据内容帧:
                            stepResult = await ProcessDataContentAsync();
                            break;
                        case StepRemark.查询完成状态帧:
                            stepResult = QueryCompletionStatus();
                            break;
                    }

                    lock (_syncLock)
                    {
                        if (stepResult)
                        {
                            _recount = 0; // 成功重置计数器
                                          // 更新状态逻辑...
                        }
                        else
                        {
                            Interlocked.Increment(ref _recount);
                            if (_recount >= 5)
                            {
                                HandleRetryFailure(currentStep);
                                return;
                            }
                        }
                    }

                    await Task.Delay(20, _token.Token);
                }
            }
            catch (OperationCanceledException)
            {
                // 正常取消
            }
            catch (Exception ex)
            {
                AddLog("SYSTEM", "ERROR", $"任务异常: {ex.Message}");
            }
            finally
            {
                EndTransmit();
            }
        }

        private async Task<bool> ProcessDataContentAsync()
        {
            if (questCycle <= 1) return false;

            await Task.Delay(100, _token.Token);
            var result = await ReadFileDataContentAsync();
            if (result)
            {
                fileOffset += dataLength;
                //Interlocked.Add(ref fileOffset, dataLength);
                questCycle--;
            }
            return result;
        }*/

        private void HandleRetryFailure(StepRemark step)
        {
            string tag = step switch
            {
                StepRemark.读文件指令帧 => "F0",
                StepRemark.读文件数据内容帧 => "F1",
                StepRemark.查询完成状态帧 => "F4",
                _ => "UNKNOWN"
            };
            AddLog(DateTime.Now.ToString("HH:mm:ss:fff"), tag, "终止传输，重试5次失败");
            EndTransmit();
        }

        private void EndTransmit()
        {
            lock (_syncLock)
            {
                _token?.Cancel();
                _token?.Dispose(); // 释放资源
                _token = null;
                state = false;
                stepFlag = StepRemark.None;

                fileSize = -1;
                fileOffset = -1;

                this.Invoke(() => { btnFileTransmit.Text = "开始读取"; });
            }

            //按钮禁用
            Stopwatch stopwatch = Stopwatch.StartNew();
            Task.Run(async delegate
            {
                this.Invoke(new Action(() => { btnFileTransmit.Enabled = false; }));
                await Task.Delay(10000);
                this.Invoke(new Action(() => { btnFileTransmit.Enabled = true; }));
            });
        }

        private void ReadFileCommand()
        {
            byte[] canid = { 0xE0, Convert.ToByte(slaveAddress), 0xF0, 0x07 };
            byte[] data = new byte[8];
            data[0] = 0x0;
            data[1] = (byte)bmuAddress;
            data[2] = (byte)fileNumber;
            data[3] = (byte)readType;

            bool result = ecanHelper.Send(data, canid);
            if (result)
            {
                AddLog(DateTime.Now.ToString("HH:mm:ss:fff"), $"{BitConverter.ToUInt32(canid, 0).ToString("X8")}", byteToHexString(data));
            }
            else
            {
                AddLog(DateTime.Now.ToString("HH:mm:ss:fff"), $"{BitConverter.ToUInt32(canid, 0).ToString("X8")}", "发生失败！");
            }
        }

        private void ReadFileDataContent()
        {
            byte[] canid = { 0xE0, Convert.ToByte(slaveAddress), 0xF1, 0x07 };

            byte[] data = new byte[8];
            data[0] = 0x0;
            data[1] = (byte)bmuAddress;
            data[2] = (byte)fileNumber;
            data[3] = (byte)(fileOffset & 0xff);
            data[4] = (byte)(fileOffset >> 8);
            data[5] = (byte)(fileOffset >> 16);
            data[6] = (byte)(dataLength & 0xff);
            data[7] = (byte)(dataLength >> 8);

            bool result = ecanHelper.Send(data, canid);
            if (result)
            {
                AddLog(DateTime.Now.ToString("HH:mm:ss:fff"), $"{BitConverter.ToUInt32(canid, 0).ToString("X8")}", byteToHexString(data));
            }
            else
            {
                AddLog(DateTime.Now.ToString("HH:mm:ss:fff"), $"{BitConverter.ToUInt32(canid, 0).ToString("X8")}", "发生失败！");
            }
        }

        private void QueryCompletionStatus()
        {
            byte[] canid = { 0xE0, Convert.ToByte(slaveAddress), 0xF4, 0x07 };

            byte[] data = new byte[8];
            data[0] = 0x0;
            data[1] = (byte)bmuAddress;
            data[2] = (byte)fileNumber;
            data[3] = (byte)0x01;

            bool result = ecanHelper.Send(data, canid);
            if (result)
            {
                AddLog(DateTime.Now.ToString("HH:mm:ss:fff"), $"{BitConverter.ToUInt32(canid, 0).ToString("X8")}", byteToHexString(data));
            }
            else
            {
                AddLog(DateTime.Now.ToString("HH:mm:ss:fff"), $"{BitConverter.ToUInt32(canid, 0).ToString("X8")}", "发生失败！");
            }
        }

        // 辅助函数
        private void AddLog(string date, string id, string context)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Text = date;
            lvi.SubItems.Add(id);
            lvi.SubItems.Add(context);
            this.Invoke(new Action(() => { this.lvPrintBlock.Items.Insert(0, lvi); }));
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
    }

    public class Mode
    {
        public string Name { get; set; }
        public List<string> FileModels { get; set; }
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
}
