namespace SofarBMS.Model
{
    public class BmuRealtimeData
    {
        // 基础信息
        public string SerialNumber { get; set; } = string.Empty;
        public string PackID { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }

        // 状态信息区
        public double BatteryVoltage { get; set; }
        public double LoadVoltage { get; set; }
        public double BatteryCurrent { get; set; }
        public double SOC { get; set; }
        public double SOH { get; set; }
        public double RemainingCapacity { get; set; }
        public double FullCapacity { get; set; }
        public ushort CycleTime { get; set; }
        public double CumulativeDischargeCapacity { get; set; }
        public double CumulativeChargeCapacity { get; set; }
        public double TotalDischargedCapacity { get; set; }
        public double TotalChargedCapacity { get; set; }
        public string BmsStatus { get; set; } = string.Empty;
        public double ChargeCurrentLimit { get; set; }
        public double DischargeCurrentLimit { get; set; }

        // 新增字段
        public double BalanceBusVoltage { get; set; }
        public double BalanceCurrent { get; set; }
        public double ActiveBalanceMaxCellVoltage { get; set; }
        public double AverageCellTemperature { get; set; }
        public double ActiveBalanceCellSoc { get; set; }
        public double ActiveBalanceAccumulatedCapacity { get; set; }
        public double ActiveBalanceRemainingCapacity { get; set; }
        public double AuxiliaryVoltage { get; set; }
        public double ChargeCurrentOffset { get; set; }
        public double DischargeCurrentOffset { get; set; }
        public string ResetMode { get; set; } = string.Empty;
        public double SyncFallSoc { get; set; }
        public string ActiveBalanceStatus { get; set; } = string.Empty;
        public double NominalCapacity { get; set; }
        public string RegisterName { get; set; } = string.Empty;
        public string BatteryType { get; set; } = string.Empty;
        public string ManufacturerName { get; set; } = string.Empty;

        // 电池数据
        public ushort MaxVoltageCellId { get; set; }
        public ushort MaxVoltage { get; set; }
        public ushort MinVoltageCellId { get; set; }
        public ushort MinVoltage { get; set; }
        public ushort VoltageDifference { get; set; }
        public uint CellVoltage1 { get; set; }
        public uint CellVoltage2 { get; set; }
        public uint CellVoltage3 { get; set; }
        public uint CellVoltage4 { get; set; }
        public uint CellVoltage5 { get; set; }
        public uint CellVoltage6 { get; set; }
        public uint CellVoltage7 { get; set; }
        public uint CellVoltage8 { get; set; }
        public uint CellVoltage9 { get; set; }
        public uint CellVoltage10 { get; set; }
        public uint CellVoltage11 { get; set; }
        public uint CellVoltage12 { get; set; }
        public uint CellVoltage13 { get; set; }
        public uint CellVoltage14 { get; set; }
        public uint CellVoltage15 { get; set; }
        public uint CellVoltage16 { get; set; }
        public double AmbientTemperature { get; set; }
        public double MosTemperature { get; set; }
        public double BalanceTemperature1 { get; set; }
        public double BalanceTemperature2 { get; set; }
        public double PositiveTerminalTemperature { get; set; }
        public double NegativeTerminalTemperature { get; set; }
        public double ActiveBalanceTemperature1 { get; set; }
        public double ActiveBalanceTemperature2 { get; set; }
        public ushort MaxTemperatureCellId { get; set; }
        public double MaxTemperature { get; set; }
        public ushort MinTemperatureCellId { get; set; }
        public double MinTemperature { get; set; }
        public double CellTemperature1 { get; set; }
        public double CellTemperature2 { get; set; }
        public double CellTemperature3 { get; set; }
        public double CellTemperature4 { get; set; }
        public double CellTemperature5 { get; set; }
        public double CellTemperature6 { get; set; }
        public double CellTemperature7 { get; set; }
        public double CellTemperature8 { get; set; }

        //系统状态
        public ushort ChargeEnabled { get; set; }
        public ushort DischargeEnabled { get; set; }
        public ushort RelayCutoffRequest { get; set; }
        public ushort PowerOffRequest { get; set; }
        public ushort ForceChargeRequest { get; set; }
        public ushort FullyCharged { get; set; }
        public ushort FullyDischarged { get; set; }
        public ushort DiIO { get; set; }
        public ushort ChargeIO { get; set; }

        //新增帧信息
        public string BMUSaftwareVersion { get; set; } = string.Empty; 
        public string BMUHardwareVersion { get; set; } = string.Empty;
        public string BMUCanVersion { get; set; } = string.Empty;

        public string Fault { get; set; } = string.Empty;
        public string Protection { get; set; } = string.Empty;
        public string Warning { get; set; } = string.Empty;
        public string Prompt { get; set; } = string.Empty;

        // SOC，SOH各10个
        public double[] SOC_Array { get; set; } = new double[10];
        public double SOC1 => SOC_Array[0];
        public double SOC2 => SOC_Array[1];
        public double SOC3 => SOC_Array[2];
        public double SOC4 => SOC_Array[3];
        public double SOC5 => SOC_Array[4];
        public double SOC6 => SOC_Array[5];
        public double SOC7 => SOC_Array[6];
        public double SOC8 => SOC_Array[7];
        public double SOC9 => SOC_Array[8];
        public double SOC10 => SOC_Array[9];

        public double[] SOH_Array { get; set; } = new double[10];
        public double SOH1 => SOH_Array[0];
        public double SOH2 => SOH_Array[1];
        public double SOH3 => SOH_Array[2];
        public double SOH4 => SOH_Array[3];
        public double SOH5 => SOH_Array[4];
        public double SOH6 => SOH_Array[5];
        public double SOH7 => SOH_Array[6];
        public double SOH8 => SOH_Array[7];
        public double SOH9 => SOH_Array[8];
        public double SOH10 => SOH_Array[9];


        // 列顺序定义（按实际业务顺序排列）
        private static readonly string[] CsvFields =
        {
            nameof(SerialNumber),
            nameof(CreateDate),
            nameof(PackID),
            nameof(Fault),
            nameof(Warning),
            nameof(Protection) ,
            nameof(Prompt),
            nameof(BmsStatus),
            nameof(ChargeEnabled),
            nameof(DischargeEnabled),
            nameof(RelayCutoffRequest),
            nameof(PowerOffRequest),
            nameof(ForceChargeRequest),
            nameof(FullyCharged),
            nameof(FullyDischarged),
            nameof(DiIO),
            nameof(ChargeIO),
            nameof(BatteryVoltage),
            nameof(LoadVoltage),
            nameof(BatteryCurrent),
            nameof(SOC),
            nameof(SOH),
            nameof(RemainingCapacity) ,
            nameof(FullCapacity),
            nameof(ChargeCurrentLimit),
            nameof(DischargeCurrentLimit),
            nameof(CumulativeDischargeCapacity),
            nameof(CumulativeChargeCapacity) ,
            nameof(TotalDischargedCapacity),
            nameof(TotalChargedCapacity),
            nameof(MaxVoltageCellId) ,
            nameof(MaxVoltage),
            nameof(MinVoltageCellId),
            nameof(MinVoltage),
            nameof(VoltageDifference) ,
            nameof(CellVoltage1),
            nameof(CellVoltage2),
            nameof(CellVoltage3),
            nameof(CellVoltage4),
            nameof(CellVoltage5),
            nameof(CellVoltage6),
            nameof(CellVoltage7),
            nameof(CellVoltage8),
            nameof(CellVoltage9),
            nameof(CellVoltage10),
            nameof(CellVoltage11),
            nameof(CellVoltage12),
            nameof(CellVoltage13),
            nameof(CellVoltage14),
            nameof(CellVoltage15),
            nameof(CellVoltage16),
            nameof(AmbientTemperature),
            nameof(MosTemperature),
            nameof(PositiveTerminalTemperature),
            nameof(NegativeTerminalTemperature),
            nameof(ActiveBalanceTemperature1),
            nameof(ActiveBalanceTemperature2),
            nameof(MaxTemperatureCellId),
            nameof(MaxTemperature),
            nameof(MinTemperatureCellId),
            nameof(MinTemperature),
            nameof(CellTemperature1),
            nameof(CellTemperature2),
            nameof(CellTemperature3),
            nameof(CellTemperature4),
            nameof(CellTemperature5),
            nameof(CellTemperature6),
            nameof(CellTemperature7),
            nameof(CellTemperature8),
            nameof(BalanceBusVoltage),
            nameof(BalanceCurrent),
            nameof(ActiveBalanceMaxCellVoltage),
            nameof(AverageCellTemperature),
            nameof(ActiveBalanceCellSoc),
            nameof(ActiveBalanceAccumulatedCapacity),
            nameof(ActiveBalanceRemainingCapacity),
            nameof(AuxiliaryVoltage),
            nameof(ChargeCurrentOffset),
            nameof(DischargeCurrentOffset),
            nameof(ResetMode),
            nameof(SyncFallSoc),
            nameof(ActiveBalanceStatus),
            nameof(NominalCapacity),
            nameof(RegisterName),
            nameof(BatteryType),
            nameof(BMUSaftwareVersion),
            nameof(BMUHardwareVersion),
            nameof(BMUCanVersion),
            nameof(SOC1),
            nameof(SOC2),
            nameof(SOC3),
            nameof(SOC4),
            nameof(SOC5),
            nameof(SOC6),
            nameof(SOC7),
            nameof(SOC8),
            nameof(SOC9),
            nameof(SOC10),
            nameof(SOH1),
            nameof(SOH2),
            nameof(SOH3),
            nameof(SOH4),
            nameof(SOH5),
            nameof(SOH6),
            nameof(SOH7),
            nameof(SOH8),
            nameof(SOH9),
            nameof(SOH10),
    };

        // 属性到中文列名的映射
        private static readonly Dictionary<string, string> PropertyHeaderMap = new()
        {
            [nameof(SerialNumber)] = "序列号",
            [nameof(CreateDate)] = "记录时间",
            [nameof(PackID)] = "电池ID",
            [nameof(Fault)] = "故障信息",
            [nameof(Warning)] = "告警信息",
            [nameof(Protection)] = "保护信息",
            [nameof(Prompt)] = "提示信息",
            [nameof(BmsStatus)] = "电池状态",
            [nameof(ChargeEnabled)] = "允许充电",
            [nameof(DischargeEnabled)] = "允许放电",
            [nameof(RelayCutoffRequest)] = "切断继电器",
            [nameof(PowerOffRequest)] = "BMU关机",
            [nameof(ForceChargeRequest)] = "请求强充",
            [nameof(FullyCharged)] = "充满",
            [nameof(FullyDischarged)] = "放空",
            [nameof(DiIO)] = "编址输入电平",
            [nameof(ChargeIO)] = "补电输入电平",
            [nameof(BatteryVoltage)] = "电池电压",
            [nameof(LoadVoltage)] = "负载电压",
            [nameof(BatteryCurrent)] = "电池电流",
            [nameof(SOC)] = "电池剩余容量",
            [nameof(SOH)] = "电池健康程度",
            [nameof(RemainingCapacity)] = "剩余容量",
            [nameof(FullCapacity)] = "满充容量",
            [nameof(ChargeCurrentLimit)] = "放电电流上限",
            [nameof(DischargeCurrentLimit)] = "放电电流上限",
            [nameof(CumulativeDischargeCapacity)] = "累计放电量",
            [nameof(CumulativeChargeCapacity)] = "累计充电量",
            [nameof(TotalDischargedCapacity)] = "累计充电容量",
            [nameof(TotalChargedCapacity)] = "累计放电容量",

            [nameof(MaxVoltageCellId)] = "最高单体电压编号",
            [nameof(MaxVoltage)] = "最高单体电压",
            [nameof(MinVoltageCellId)] = "最低单体电压编号",
            [nameof(MinVoltage)] = "单体电压压差",
            [nameof(VoltageDifference)] = "单体电压差",
            [nameof(CellVoltage1)] = "电压1",
            [nameof(CellVoltage2)] = "电压2",
            [nameof(CellVoltage3)] = "电压3",
            [nameof(CellVoltage4)] = "电压4",
            [nameof(CellVoltage5)] = "电压5",
            [nameof(CellVoltage6)] = "电压6",
            [nameof(CellVoltage7)] = "电压7",
            [nameof(CellVoltage8)] = "电压8",
            [nameof(CellVoltage9)] = "电压9",
            [nameof(CellVoltage10)] = "电压10",
            [nameof(CellVoltage11)] = "电压11",
            [nameof(CellVoltage12)] = "电压12",
            [nameof(CellVoltage13)] = "电压13",
            [nameof(CellVoltage14)] = "电压14",
            [nameof(CellVoltage15)] = "电压15",
            [nameof(CellVoltage16)] = "电压16",

            [nameof(AmbientTemperature)] = "环境温度",
            [nameof(MosTemperature)] = "Mos温度",
            [nameof(PositiveTerminalTemperature)] = "P+端子温度",
            [nameof(NegativeTerminalTemperature)] = "P-端子温度",
            [nameof(ActiveBalanceTemperature1)] = "主动均衡温度1",
            [nameof(ActiveBalanceTemperature2)] = "主动均衡温度2",
            [nameof(MaxTemperatureCellId)] = "最高单体温度编号",
            [nameof(MaxTemperature)] = "最高单体温度",
            [nameof(MinTemperatureCellId)] = "最低单体温度编号",
            [nameof(MinTemperature)] = "最低单体温度",
            [nameof(CellTemperature1)] = "温度1",
            [nameof(CellTemperature2)] = "温度2",
            [nameof(CellTemperature3)] = "温度3",
            [nameof(CellTemperature4)] = "温度4",
            [nameof(CellTemperature5)] = "温度5",
            [nameof(CellTemperature6)] = "温度6",
            [nameof(CellTemperature7)] = "温度7",
            [nameof(CellTemperature8)] = "温度8",

            [nameof(BalanceBusVoltage)] = "均衡母线电压",
            [nameof(BalanceCurrent)] = "均衡电流",
            [nameof(ActiveBalanceMaxCellVoltage)] = "主动均衡最大单体电压",
            [nameof(AverageCellTemperature)] = "电芯平均温度",
            [nameof(ActiveBalanceCellSoc)] = "主动均衡查表SOC",
            [nameof(ActiveBalanceAccumulatedCapacity)] = "主动均衡累计容量",
            [nameof(ActiveBalanceRemainingCapacity)] = "主动均衡剩余容量",
            [nameof(AuxiliaryVoltage)] = "辅源电压",
            [nameof(ChargeCurrentOffset)] = "充电电流偏压",
            [nameof(DischargeCurrentOffset)] = "放电电流偏压",
            [nameof(ResetMode)] = "复位方式",
            [nameof(SyncFallSoc)] = "放空同步下降SOC",
            [nameof(ActiveBalanceStatus)] = "主动均衡状态",
            [nameof(NominalCapacity)] = "标定容量",
            [nameof(RegisterName)] = "供应商名称",
            [nameof(BatteryType)] = "锂电池类型",
            [nameof(BMUSaftwareVersion)] = "软件版本号",
            [nameof(BMUHardwareVersion)] = "硬件版本号",
            [nameof(BMUCanVersion)] = "CAN协议版本号",

            [nameof(SOC1)] = "SOC1",
            [nameof(SOC2)] = "SOC2",
            [nameof(SOC3)] = "SOC3",
            [nameof(SOC4)] = "SOC4",
            [nameof(SOC5)] = "SOC5",
            [nameof(SOC6)] = "SOC6",
            [nameof(SOC7)] = "SOC7",
            [nameof(SOC8)] = "SOC8",
            [nameof(SOC9)] = "SOC9",
            [nameof(SOC10)] = "SOC10",

            [nameof(SOH1)] = "SOH1",
            [nameof(SOH2)] = "SOH2",
            [nameof(SOH3)] = "SOH3",
            [nameof(SOH4)] = "SOH4",
            [nameof(SOH5)] = "SOH5",
            [nameof(SOH6)] = "SOH6",
            [nameof(SOH7)] = "SOH7",
            [nameof(SOH8)] = "SOH8",
            [nameof(SOH9)] = "SOH9",
            [nameof(SOH10)] = "SOH10",
        };



        public string GetHeader() => string.Join(",", CsvFields.Select(f => PropertyHeaderMap[f]));

        public string GetValue()
        {
            var values = new List<object>();
            foreach (var field in CsvFields)
            {
                var prop = GetType().GetProperty(field);
                values.Add(prop?.GetValue(this)?.ToString() ?? string.Empty);
            }

            return string.Join(",", values);
        }
    }
}
