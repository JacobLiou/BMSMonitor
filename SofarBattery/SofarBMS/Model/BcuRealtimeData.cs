namespace SofarBMS.Model
{
    public class BcuRealtimeData
    {
        // 列顺序定义（按实际业务顺序排列）
        private static readonly string[] CsvFields = {
            nameof(BCUSytemTime),
            nameof(SN),
            nameof(BCUSoftwareVersion),
            nameof(BCUHardwareVersion),
            nameof(PackID),
            nameof(Fault),
            nameof(Protection),
            nameof(Warning),
            nameof(Prompt),
            nameof(PCUFault),
            nameof(CCDetectionStatus),
            nameof(Relay1Status),
            nameof(Relay2Status),
            nameof(ButtonWakeupStatus),
            nameof(InverterWakeupStatus),
            nameof(DCRelayControlSignal),
            nameof(ChagreStatus),
            nameof(DischargeStatus),
            nameof(ForceChargeStatus),
            nameof(FullyCharged),
            nameof(FullyDischarged),
            nameof(BatteryChargeVoltage),
            nameof(ChargeCurrentLimitation),
            nameof(DischargeCurrentLimitation),
            nameof(BatteryDischargeVoltage),
            nameof(ClusterCurrent),
            nameof(BmsState),
            nameof(ClusterSOC),
            nameof(ClusterSOH),
            nameof(ClusterBatPackNum),
            nameof(PowerTerminalTemperature1),
            nameof(PowerTerminalTemperature2),
            nameof(PowerTerminalTemperature3),
            nameof(PowerTerminalTemperature4),
            nameof(AmbientTemperature),
            nameof(BatAverageTemp),
            nameof(Cycles),
            nameof(RemainingTotalCapacity),
            nameof(ClusterRatePower),
            nameof(BatterySamplingVoltage1),
            nameof(BusSamplingVoltage),
            nameof(BatterySummingVoltage),
            nameof(HeatingFilmVoltage),
            nameof(HeatingFilmMosVoltage),
            nameof(InsulationResistance),
            nameof(AuxiliaryPowerSupplyVoltage),
            nameof(ClusterMaxCellVolt),
            nameof(ClusterMaxCellVoltPack),
            nameof(ClusterMaxCellVoltNum),
            nameof(ClusterMinCellVolt),
            nameof(ClusterMinCellVoltPack),
            nameof(ClusterMinCellVoltNum),
            nameof(ClusterMaxCellTemp),
            nameof(ClusterMaxCellTempPack),
            nameof(ClusterMaxCellTempNum),
            nameof(ClusterMinCellTemp),
            nameof(ClusterMinCellTempPack),
            nameof(ClusterMinCellTempNum),
            nameof(PCUWorkState),
            nameof(PCUBatteryState),
            nameof(InductiveCurrentSampling1),
            nameof(InductiveCurrentSampling2),
            nameof(InductiveCurrentSampling3),
            nameof(InductiveCurrentSampling4),
            nameof(DischargeCurrentLimitation),
            nameof(ChargeCurrentLimitation),
            nameof(ContactorVoltage),
            nameof(BatteryClusterVoltage1),
            nameof(HighBusVoltage),
            nameof(HeatingFilmPowerSupplyVoltage),
            nameof(BatterySamplingVoltage2),
            nameof(RadiatorTemperature1),
            nameof(RadiatorTemperature2),
            nameof(RadiatorTemperature3),
            nameof(RadiatorTemperature4),
            nameof(DischargeLimitCurrent),
            nameof(ChargeLimitCurrent),
            nameof(PCUSoftwareVersion),
            nameof(PCUHardwareVersion),
            nameof(SCIProtocolVersion),
            nameof(CANProtocolVersion)};

        // 属性到中文列名的映射
        private static readonly Dictionary<string, string> PropertyHeaderMap = new()
        {
            [nameof(CreateDate)] = "记录时间",
            [nameof(SN)] = "序列号",
            [nameof(BCUSytemTime)] = "系统时间",
            [nameof(BCUSoftwareVersion)] = "BCU软件版本号",
            [nameof(BCUHardwareVersion)] = "BCU硬件版本号",
            [nameof(PackID)] = "电池ID",
            [nameof(Fault)] = "故障信息",
            [nameof(Protection)] = "保护信息",
            [nameof(Warning)] = "告警信息",
            [nameof(Prompt)] = "提示信息",
            [nameof(PCUFault)] = "PCU故障信息",
            [nameof(CCDetectionStatus)] = "CC检测状态",
            [nameof(Relay1Status)] = "继电器1状态",
            [nameof(Relay2Status)] = "继电器2状态",
            [nameof(ButtonWakeupStatus)] = "按钮唤醒状态",
            [nameof(InverterWakeupStatus)] = "逆变器唤醒状态",
            [nameof(DCRelayControlSignal)] = "直流继电器控制信号传输",
            [nameof(ChagreStatus)] = "充电允许",
            [nameof(DischargeStatus)] = "放电允许",
            [nameof(ForceChargeStatus)] = "强制充电",
            [nameof(FullyCharged)] = "满充",
            [nameof(FullyDischarged)] = "放空",

            [nameof(BatteryChargeVoltage)] = "充电电压",
            [nameof(ChargeCurrentLimitation)] = "充电电流上限",
            [nameof(DischargeCurrentLimitation)] = "放电电流上限",
            [nameof(BatteryDischargeVoltage)] = "放电截止电压",
            [nameof(ClusterCurrent)] = "簇电流",
            [nameof(BmsState)] = "BMS状态",
            [nameof(ClusterSOC)] = "电池簇SOC",
            [nameof(ClusterSOH)] = "电池簇SOH",
            [nameof(ClusterBatPackNum)] = "簇内电池包数量",
            [nameof(PowerTerminalTemperature1)] = "P+端子温度",
            [nameof(PowerTerminalTemperature2)] = "P-端子温度",
            [nameof(PowerTerminalTemperature3)] = "B+端子温度",
            [nameof(PowerTerminalTemperature4)] = "B-端子温度",
            [nameof(AmbientTemperature)] = "环境温度",
            [nameof(BatAverageTemp)] = "电池平均温度",
            [nameof(Cycles)] = "循环次数",
            [nameof(RemainingTotalCapacity)] = "总剩余容量",
            [nameof(ClusterRatePower)] = "额定功率",
            [nameof(BatterySamplingVoltage1)] = "电池采样电压1",
            [nameof(BusSamplingVoltage)] = "母线采样电压",
            [nameof(BatterySummingVoltage)] = "电池累加和电压",
            [nameof(HeatingFilmVoltage)] = "加热膜供电电压",
            [nameof(HeatingFilmMosVoltage)] = "加热膜MOS电压",
            [nameof(InsulationResistance)] = "绝缘电阻电压",
            [nameof(AuxiliaryPowerSupplyVoltage)] = "辅源电压",

            [nameof(ClusterMaxCellVolt)] = "最高单体电压",
            [nameof(ClusterMaxCellVoltPack)] = "最高单体电压所在pack",
            [nameof(ClusterMaxCellVoltNum)] = "最高单体电压所在pack的第几个位置",
            [nameof(ClusterMinCellVolt)] = "最低单体电压",
            [nameof(ClusterMinCellVoltPack)] = "最低单体电压所在pack",
            [nameof(ClusterMinCellVoltNum)] = "最低单体电压所在pack的第几位置",
            [nameof(ClusterMaxCellTemp)] = "最高单体温度",
            [nameof(ClusterMaxCellTempPack)] = "最高单体温度所在pack",
            [nameof(ClusterMaxCellTempNum)] = "最高单体温度所在pack的第几个位置",
            [nameof(ClusterMinCellTemp)] = "最低单体温度",
            [nameof(ClusterMinCellTempPack)] = "最低单体温度所在pack",
            [nameof(ClusterMinCellTempNum)] = "最低单体温度所在pack的第几个位置",

            [nameof(PCUWorkState)] = "PCU工作状态",
            [nameof(PCUBatteryState)] = "PCU运行状态",
            [nameof(InductiveCurrentSampling1)] = "电感电流采样1",
            [nameof(InductiveCurrentSampling2)] = "电感电流采样2",
            [nameof(InductiveCurrentSampling3)] = "电感电流采样3",
            [nameof(InductiveCurrentSampling4)] = "电感电流采样4",
            [nameof(DischargeCurrentLimitation)] = "放电大电流采样",
            [nameof(ChargeCurrentLimitation)] = "充电大电流采样",
            [nameof(ContactorVoltage)] = "接触器电压",
            [nameof(BatteryClusterVoltage1)] = "电池簇电压1",
            [nameof(HighBusVoltage)] = "高压母线电压",
            [nameof(HeatingFilmPowerSupplyVoltage)] = "加热膜供电电压",
            [nameof(BatterySamplingVoltage2)] = "电池采样电压2",
            [nameof(RadiatorTemperature1)] = "散热器温度1",
            [nameof(RadiatorTemperature2)] = "散热器温度2",
            [nameof(RadiatorTemperature3)] = "散热器温度3",
            [nameof(RadiatorTemperature4)] = "散热器温度4",
            [nameof(DischargeLimitCurrent)] = "充电限载电流值",
            [nameof(ChargeLimitCurrent)] = "放电限载电流值",
            [nameof(PCUSoftwareVersion)] = "PCU软件版本号",
            [nameof(PCUHardwareVersion)] = "PCU硬件版本号",
            [nameof(SCIProtocolVersion)] = "SCI协议版本号",
            [nameof(CANProtocolVersion)] = "CAN协议版本号",
        };

        private static List<string> customizeFields = new List<string>();
        private static Dictionary<string, string> CustomizePropertyHeaderMap = new Dictionary<string, string>();//readonly

        public BcuRealtimeData()
        {
            CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //单簇12个BMU，16个电压
            customizeFields = new List<string>();
            CustomizePropertyHeaderMap = new Dictionary<string, string>();
            for (int i = 1; i <= 12; i++)
            {
                for (int j = 1; j <= 16; j++)
                {
                    string voltageName = $"Voltage_BMU{i}_{j}";
                    customizeFields.Add(voltageName);
                    CustomizePropertyHeaderMap.Add(voltageName, $"BMU{i}电池电压(V) {j}#");
                }
            }

            //单簇12个BMU，8个电压
            for (int i = 1; i <= 12; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    string tempeartrueName = $"Tempeartrue_BMU{i}_{j}";
                    customizeFields.Add(tempeartrueName);
                    CustomizePropertyHeaderMap.Add(tempeartrueName, $"BMU{i}电池温度(℃) {j}#");
                }
            }

            //单簇12个BMU，16个电压
            for (int i = 1; i <= 12; i++)
            {
                for (int j = 1; j <= 16; j++)
                {
                    string socName = $"SOC_BMU{i}_{j}";
                    customizeFields.Add(socName);
                    CustomizePropertyHeaderMap.Add(socName, $"BMU{i}电池SOC(%) {j}#");
                }
            }

            //单簇12个BMU，16个电压
            for (int i = 1; i <= 12; i++)
            {
                for (int j = 1; j <= 16; j++)
                {
                    string sohName = $"SOH_BMU{i}_{j}";
                    customizeFields.Add(sohName);
                    CustomizePropertyHeaderMap.Add(sohName, $"BMU{i}电池SOH(%) {j}#");
                }
            }


        }

        public string PackID { get; set; } = string.Empty;
        public string CreateDate { get; } // 只读，构造时初始化

        // 0x0B6:BCU遥信数据上报1：
        public double CCDetectionStatus { get; set; }
        public double Relay1Status { get; set; }
        public double Relay2Status { get; set; }
        public double ButtonWakeupStatus { get; set; }
        public double InverterWakeupStatus { get; set; }
        public double DCRelayControlSignal { get; set; }
        //充放电相关状态
        public double ChagreStatus { get; set; }
        public double DischargeStatus { get; set; }
        public double ForceChargeStatus { get; set; }
        public double FullyCharged { get; set; }
        public double FullyDischarged { get; set; }

        //0x0B7:BCU遥信数据上报2
        public double PowerTerminalTemperature1 { get; set; }
        public double PowerTerminalTemperature2 { get; set; }
        public double PowerTerminalTemperature3 { get; set; }
        public double PowerTerminalTemperature4 { get; set; }
        public double AmbientTemperature { get; set; }

        //0x0B8:BCU遥信数据上报3
        public double BatterySamplingVoltage1 { get; set; }
        public double BusSamplingVoltage { get; set; }
        public double BatterySummingVoltage { get; set; }
        public double HeatingFilmVoltage { get; set; }
        public double HeatingFilmMosVoltage { get; set; }
        public double InsulationResistance { get; set; }
        public double AuxiliaryPowerSupplyVoltage { get; set; }
        //public double FuseVoltage { get; set; }
        //public double PowerVoltage { get; set; }

        //0x0B9:BCU遥信数据上报4
        public double ClusterCurrent { get; set; }

        //0x0BA:BCU遥信数据上报5
        public double ClusterMaxCellVolt { get; set; }
        public ushort ClusterMaxCellVoltPack { get; set; }
        public ushort ClusterMaxCellVoltNum { get; set; }
        public double ClusterMinCellVolt { get; set; }
        public ushort ClusterMinCellVoltPack { get; set; }
        public ushort ClusterMinCellVoltNum { get; set; }

        //0x0BB:BCU遥信数据上报6
        public double ClusterMaxCellTemp { get; set; }
        public ushort ClusterMaxCellTempPack { get; set; }
        public ushort ClusterMaxCellTempNum { get; set; }
        public double ClusterMinCellTemp { get; set; }
        public ushort ClusterMinCellTempPack { get; set; }
        public ushort ClusterMinCellTempNum { get; set; }

        //0x0BC:BCU遥测数据1
        public double BatteryChargeVoltage { get; set; }
        public double ChargeCurrentLimitation { get; set; }
        public double DischargeCurrentLimitation { get; set; }
        public double BatteryDischargeVoltage { get; set; }

        //0x0BD:BCU遥测数据2
        public string BCUSoftwareVersion { get; set; } = string.Empty;
        public string BCUHardwareVersion { get; set; } = string.Empty;
        public string CANProtocolVersion { get; set; } = string.Empty;
        //public double ClusterVoltage { get; set; }
        //public double ClusterCurrent { get; set; }
        //public double MaxPowerTerminalTemperature { get; set; }

        //0x0BE:BCU遥测数据3
        public double RemainingTotalCapacity { get; set; }
        public double BatAverageTemp { get; set; }
        public double ClusterRatePower { get; set; }
        public double Cycles { get; set; }
        //public double BatBusVolt { get; set; }

        //0x0BF:BCU遥测数据4
        public string BmsState { get; set; } = string.Empty;
        public ushort ClusterSOC { get; set; }
        public ushort ClusterSOH { get; set; }
        public ushort ClusterBatPackNum { get; set; }
        //public ushort HWVersion { get; set; }

        //0x0C0:BCU系统时间
        //public string Year { get; set; }
        //public string Month { get; set; }
        //public string Day { get; set; }
        //public string Hour { get; set; }
        //public string Minute { get; set; }
        //public string Second { get; set; }
        public string BCUSytemTime { get; set; } = string.Empty;

        //0x0F3:序列号
        public string SN { get; set; } = string.Empty;

        //0x0C7:模拟量(DSP)
        public double InductiveCurrentSampling1 { get; set; }
        public double InductiveCurrentSampling2 { get; set; }
        public double InductiveCurrentSampling3 { get; set; }

        public double InductiveCurrentSampling4 { get; set; }
        public double DischargeHighCurrentSampling { get; set; }
        public double ChargeHighCurrentSampling { get; set; }

        public double ContactorVoltage { get; set; }
        public double BatteryClusterVoltage1 { get; set; }
        public double HighBusVoltage { get; set; }

        public double HeatingFilmPowerSupplyVoltage { get; set; }
        public double BatterySamplingVoltage2 { get; set; }

        public double RadiatorTemperature1 { get; set; }
        public double RadiatorTemperature2 { get; set; }
        public double RadiatorTemperature3 { get; set; }
        public double RadiatorTemperature4 { get; set; }

        public string PCUSoftwareVersion { get; set; } = string.Empty;
        public string PCUHardwareVersion { get; set; } = string.Empty;
        public string SCIProtocolVersion { get; set; } = string.Empty;

        //public string ResetMode { get; set; }
        //public double Dry2InStatus { get; set; }
        //public string WakeSource { get; set; }

        //0x0C8:遥信数据(DSP)
        public string PCUWorkState { get; set; } = string.Empty;
        public string PCUBatteryState { get; set; } = string.Empty;

        public double DischargeLimitCurrent { get; set; }
        public double ChargeLimitCurrent { get; set; }

        public string Fault { get; set; } = string.Empty;
        public string Warning { get; set; } = string.Empty;
        public string Protection { get; set; } = string.Empty;
        public string Prompt { get; set; } = string.Empty;
        public string PCUFault { get; set; } = string.Empty;

        //0x681:电芯电压
        public string[] Voltage_Array { get; set; } = new string[192];
        #region 只读电压字段
        public string Voltage_BMU1_1 => Voltage_Array[0];
        public string Voltage_BMU1_2 => Voltage_Array[1];
        public string Voltage_BMU1_3 => Voltage_Array[2];
        public string Voltage_BMU1_4 => Voltage_Array[3];
        public string Voltage_BMU1_5 => Voltage_Array[4];
        public string Voltage_BMU1_6 => Voltage_Array[5];
        public string Voltage_BMU1_7 => Voltage_Array[6];
        public string Voltage_BMU1_8 => Voltage_Array[7];
        public string Voltage_BMU1_9 => Voltage_Array[8];
        public string Voltage_BMU1_10 => Voltage_Array[9];
        public string Voltage_BMU1_11 => Voltage_Array[10];
        public string Voltage_BMU1_12 => Voltage_Array[11];
        public string Voltage_BMU1_13 => Voltage_Array[12];
        public string Voltage_BMU1_14 => Voltage_Array[13];
        public string Voltage_BMU1_15 => Voltage_Array[14];
        public string Voltage_BMU1_16 => Voltage_Array[15];

        public string Voltage_BMU2_1 => Voltage_Array[(2 - 1) * 16];
        public string Voltage_BMU2_2 => Voltage_Array[(2 - 1) * 16 + 1];
        public string Voltage_BMU2_3 => Voltage_Array[(2 - 1) * 16 + 2];
        public string Voltage_BMU2_4 => Voltage_Array[(2 - 1) * 16 + 3];
        public string Voltage_BMU2_5 => Voltage_Array[(2 - 1) * 16 + 4];
        public string Voltage_BMU2_6 => Voltage_Array[(2 - 1) * 16 + 5];
        public string Voltage_BMU2_7 => Voltage_Array[(2 - 1) * 16 + 6];
        public string Voltage_BMU2_8 => Voltage_Array[(2 - 1) * 16 + 7];
        public string Voltage_BMU2_9 => Voltage_Array[(2 - 1) * 16 + 8];
        public string Voltage_BMU2_10 => Voltage_Array[(2 - 1) * 16 + 9];
        public string Voltage_BMU2_11 => Voltage_Array[(2 - 1) * 16 + 10];
        public string Voltage_BMU2_12 => Voltage_Array[(2 - 1) * 16 + 11];
        public string Voltage_BMU2_13 => Voltage_Array[(2 - 1) * 16 + 12];
        public string Voltage_BMU2_14 => Voltage_Array[(2 - 1) * 16 + 13];
        public string Voltage_BMU2_15 => Voltage_Array[(2 - 1) * 16 + 14];
        public string Voltage_BMU2_16 => Voltage_Array[(2 - 1) * 16 + 15];

        public string Voltage_BMU3_1 => Voltage_Array[(3 - 1) * 16];
        public string Voltage_BMU3_2 => Voltage_Array[(3 - 1) * 16 + 1];
        public string Voltage_BMU3_3 => Voltage_Array[(3 - 1) * 16 + 2];
        public string Voltage_BMU3_4 => Voltage_Array[(3 - 1) * 16 + 3];
        public string Voltage_BMU3_5 => Voltage_Array[(3 - 1) * 16 + 4];
        public string Voltage_BMU3_6 => Voltage_Array[(3 - 1) * 16 + 5];
        public string Voltage_BMU3_7 => Voltage_Array[(3 - 1) * 16 + 6];
        public string Voltage_BMU3_8 => Voltage_Array[(3 - 1) * 16 + 7];
        public string Voltage_BMU3_9 => Voltage_Array[(3 - 1) * 16 + 8];
        public string Voltage_BMU3_10 => Voltage_Array[(3 - 1) * 16 + 9];
        public string Voltage_BMU3_11 => Voltage_Array[(3 - 1) * 16 + 10];
        public string Voltage_BMU3_12 => Voltage_Array[(3 - 1) * 16 + 11];
        public string Voltage_BMU3_13 => Voltage_Array[(3 - 1) * 16 + 12];
        public string Voltage_BMU3_14 => Voltage_Array[(3 - 1) * 16 + 13];
        public string Voltage_BMU3_15 => Voltage_Array[(3 - 1) * 16 + 14];
        public string Voltage_BMU3_16 => Voltage_Array[(3 - 1) * 16 + 15];

        public string Voltage_BMU4_1 => Voltage_Array[(4 - 1) * 16];
        public string Voltage_BMU4_2 => Voltage_Array[(4 - 1) * 16 + 1];
        public string Voltage_BMU4_3 => Voltage_Array[(4 - 1) * 16 + 2];
        public string Voltage_BMU4_4 => Voltage_Array[(4 - 1) * 16 + 3];
        public string Voltage_BMU4_5 => Voltage_Array[(4 - 1) * 16 + 4];
        public string Voltage_BMU4_6 => Voltage_Array[(4 - 1) * 16 + 5];
        public string Voltage_BMU4_7 => Voltage_Array[(4 - 1) * 16 + 6];
        public string Voltage_BMU4_8 => Voltage_Array[(4 - 1) * 16 + 7];
        public string Voltage_BMU4_9 => Voltage_Array[(4 - 1) * 16 + 8];
        public string Voltage_BMU4_10 => Voltage_Array[(4 - 1) * 16 + 9];
        public string Voltage_BMU4_11 => Voltage_Array[(4 - 1) * 16 + 10];
        public string Voltage_BMU4_12 => Voltage_Array[(4 - 1) * 16 + 11];
        public string Voltage_BMU4_13 => Voltage_Array[(4 - 1) * 16 + 12];
        public string Voltage_BMU4_14 => Voltage_Array[(4 - 1) * 16 + 13];
        public string Voltage_BMU4_15 => Voltage_Array[(4 - 1) * 16 + 14];
        public string Voltage_BMU4_16 => Voltage_Array[(4 - 1) * 16 + 15];

        public string Voltage_BMU5_1 => Voltage_Array[(5 - 1) * 16];
        public string Voltage_BMU5_2 => Voltage_Array[(5 - 1) * 16 + 1];
        public string Voltage_BMU5_3 => Voltage_Array[(5 - 1) * 16 + 2];
        public string Voltage_BMU5_4 => Voltage_Array[(5 - 1) * 16 + 3];
        public string Voltage_BMU5_5 => Voltage_Array[(5 - 1) * 16 + 4];
        public string Voltage_BMU5_6 => Voltage_Array[(5 - 1) * 16 + 5];
        public string Voltage_BMU5_7 => Voltage_Array[(5 - 1) * 16 + 6];
        public string Voltage_BMU5_8 => Voltage_Array[(5 - 1) * 16 + 7];
        public string Voltage_BMU5_9 => Voltage_Array[(5 - 1) * 16 + 8];
        public string Voltage_BMU5_10 => Voltage_Array[(5 - 1) * 16 + 9];
        public string Voltage_BMU5_11 => Voltage_Array[(5 - 1) * 16 + 10];
        public string Voltage_BMU5_12 => Voltage_Array[(5 - 1) * 16 + 11];
        public string Voltage_BMU5_13 => Voltage_Array[(5 - 1) * 16 + 12];
        public string Voltage_BMU5_14 => Voltage_Array[(5 - 1) * 16 + 13];
        public string Voltage_BMU5_15 => Voltage_Array[(5 - 1) * 16 + 14];
        public string Voltage_BMU5_16 => Voltage_Array[(5 - 1) * 16 + 15];

        public string Voltage_BMU6_1 => Voltage_Array[(6 - 1) * 16];
        public string Voltage_BMU6_2 => Voltage_Array[(6 - 1) * 16 + 1];
        public string Voltage_BMU6_3 => Voltage_Array[(6 - 1) * 16 + 2];
        public string Voltage_BMU6_4 => Voltage_Array[(6 - 1) * 16 + 3];
        public string Voltage_BMU6_5 => Voltage_Array[(6 - 1) * 16 + 4];
        public string Voltage_BMU6_6 => Voltage_Array[(6 - 1) * 16 + 5];
        public string Voltage_BMU6_7 => Voltage_Array[(6 - 1) * 16 + 6];
        public string Voltage_BMU6_8 => Voltage_Array[(6 - 1) * 16 + 7];
        public string Voltage_BMU6_9 => Voltage_Array[(6 - 1) * 16 + 8];
        public string Voltage_BMU6_10 => Voltage_Array[(6 - 1) * 16 + 9];
        public string Voltage_BMU6_11 => Voltage_Array[(6 - 1) * 16 + 10];
        public string Voltage_BMU6_12 => Voltage_Array[(6 - 1) * 16 + 11];
        public string Voltage_BMU6_13 => Voltage_Array[(6 - 1) * 16 + 12];
        public string Voltage_BMU6_14 => Voltage_Array[(6 - 1) * 16 + 13];
        public string Voltage_BMU6_15 => Voltage_Array[(6 - 1) * 16 + 14];
        public string Voltage_BMU6_16 => Voltage_Array[(6 - 1) * 16 + 15];

        public string Voltage_BMU7_1 => Voltage_Array[(7 - 1) * 16];
        public string Voltage_BMU7_2 => Voltage_Array[(7 - 1) * 16 + 1];
        public string Voltage_BMU7_3 => Voltage_Array[(7 - 1) * 16 + 2];
        public string Voltage_BMU7_4 => Voltage_Array[(7 - 1) * 16 + 3];
        public string Voltage_BMU7_5 => Voltage_Array[(7 - 1) * 16 + 4];
        public string Voltage_BMU7_6 => Voltage_Array[(7 - 1) * 16 + 5];
        public string Voltage_BMU7_7 => Voltage_Array[(7 - 1) * 16 + 6];
        public string Voltage_BMU7_8 => Voltage_Array[(7 - 1) * 16 + 7];
        public string Voltage_BMU7_9 => Voltage_Array[(7 - 1) * 16 + 8];
        public string Voltage_BMU7_10 => Voltage_Array[(7 - 1) * 16 + 9];
        public string Voltage_BMU7_11 => Voltage_Array[(7 - 1) * 16 + 10];
        public string Voltage_BMU7_12 => Voltage_Array[(7 - 1) * 16 + 11];
        public string Voltage_BMU7_13 => Voltage_Array[(7 - 1) * 16 + 12];
        public string Voltage_BMU7_14 => Voltage_Array[(7 - 1) * 16 + 13];
        public string Voltage_BMU7_15 => Voltage_Array[(7 - 1) * 16 + 14];
        public string Voltage_BMU7_16 => Voltage_Array[(7 - 1) * 16 + 15];

        public string Voltage_BMU8_1 => Voltage_Array[(8 - 1) * 16];
        public string Voltage_BMU8_2 => Voltage_Array[(8 - 1) * 16 + 1];
        public string Voltage_BMU8_3 => Voltage_Array[(8 - 1) * 16 + 2];
        public string Voltage_BMU8_4 => Voltage_Array[(8 - 1) * 16 + 3];
        public string Voltage_BMU8_5 => Voltage_Array[(8 - 1) * 16 + 4];
        public string Voltage_BMU8_6 => Voltage_Array[(8 - 1) * 16 + 5];
        public string Voltage_BMU8_7 => Voltage_Array[(8 - 1) * 16 + 6];
        public string Voltage_BMU8_8 => Voltage_Array[(8 - 1) * 16 + 7];
        public string Voltage_BMU8_9 => Voltage_Array[(8 - 1) * 16 + 8];
        public string Voltage_BMU8_10 => Voltage_Array[(8 - 1) * 16 + 9];
        public string Voltage_BMU8_11 => Voltage_Array[(8 - 1) * 16 + 10];
        public string Voltage_BMU8_12 => Voltage_Array[(8 - 1) * 16 + 11];
        public string Voltage_BMU8_13 => Voltage_Array[(8 - 1) * 16 + 12];
        public string Voltage_BMU8_14 => Voltage_Array[(8 - 1) * 16 + 13];
        public string Voltage_BMU8_15 => Voltage_Array[(8 - 1) * 16 + 14];
        public string Voltage_BMU8_16 => Voltage_Array[(8 - 1) * 16 + 15];

        public string Voltage_BMU9_1 => Voltage_Array[(9 - 1) * 16];
        public string Voltage_BMU9_2 => Voltage_Array[(9 - 1) * 16 + 1];
        public string Voltage_BMU9_3 => Voltage_Array[(9 - 1) * 16 + 2];
        public string Voltage_BMU9_4 => Voltage_Array[(9 - 1) * 16 + 3];
        public string Voltage_BMU9_5 => Voltage_Array[(9 - 1) * 16 + 4];
        public string Voltage_BMU9_6 => Voltage_Array[(9 - 1) * 16 + 5];
        public string Voltage_BMU9_7 => Voltage_Array[(9 - 1) * 16 + 6];
        public string Voltage_BMU9_8 => Voltage_Array[(9 - 1) * 16 + 7];
        public string Voltage_BMU9_9 => Voltage_Array[(9 - 1) * 16 + 8];
        public string Voltage_BMU9_10 => Voltage_Array[(9 - 1) * 16 + 9];
        public string Voltage_BMU9_11 => Voltage_Array[(9 - 1) * 16 + 10];
        public string Voltage_BMU9_12 => Voltage_Array[(9 - 1) * 16 + 11];
        public string Voltage_BMU9_13 => Voltage_Array[(9 - 1) * 16 + 12];
        public string Voltage_BMU9_14 => Voltage_Array[(9 - 1) * 16 + 13];
        public string Voltage_BMU9_15 => Voltage_Array[(9 - 1) * 16 + 14];
        public string Voltage_BMU9_16 => Voltage_Array[(9 - 1) * 16 + 15];

        public string Voltage_BMU10_1 => Voltage_Array[(10 - 1) * 16];
        public string Voltage_BMU10_2 => Voltage_Array[(10 - 1) * 16 + 1];
        public string Voltage_BMU10_3 => Voltage_Array[(10 - 1) * 16 + 2];
        public string Voltage_BMU10_4 => Voltage_Array[(10 - 1) * 16 + 3];
        public string Voltage_BMU10_5 => Voltage_Array[(10 - 1) * 16 + 4];
        public string Voltage_BMU10_6 => Voltage_Array[(10 - 1) * 16 + 5];
        public string Voltage_BMU10_7 => Voltage_Array[(10 - 1) * 16 + 6];
        public string Voltage_BMU10_8 => Voltage_Array[(10 - 1) * 16 + 7];
        public string Voltage_BMU10_9 => Voltage_Array[(10 - 1) * 16 + 8];
        public string Voltage_BMU10_10 => Voltage_Array[(10 - 1) * 16 + 9];
        public string Voltage_BMU10_11 => Voltage_Array[(10 - 1) * 16 + 10];
        public string Voltage_BMU10_12 => Voltage_Array[(10 - 1) * 16 + 11];
        public string Voltage_BMU10_13 => Voltage_Array[(10 - 1) * 16 + 12];
        public string Voltage_BMU10_14 => Voltage_Array[(10 - 1) * 16 + 13];
        public string Voltage_BMU10_15 => Voltage_Array[(10 - 1) * 16 + 14];
        public string Voltage_BMU10_16 => Voltage_Array[(10 - 1) * 16 + 15];

        public string Voltage_BMU11_1 => Voltage_Array[(11 - 1) * 16];
        public string Voltage_BMU11_2 => Voltage_Array[(11 - 1) * 16 + 1];
        public string Voltage_BMU11_3 => Voltage_Array[(11 - 1) * 16 + 2];
        public string Voltage_BMU11_4 => Voltage_Array[(11 - 1) * 16 + 3];
        public string Voltage_BMU11_5 => Voltage_Array[(11 - 1) * 16 + 4];
        public string Voltage_BMU11_6 => Voltage_Array[(11 - 1) * 16 + 5];
        public string Voltage_BMU11_7 => Voltage_Array[(11 - 1) * 16 + 6];
        public string Voltage_BMU11_8 => Voltage_Array[(11 - 1) * 16 + 7];
        public string Voltage_BMU11_9 => Voltage_Array[(11 - 1) * 16 + 8];
        public string Voltage_BMU11_10 => Voltage_Array[(11 - 1) * 16 + 9];
        public string Voltage_BMU11_11 => Voltage_Array[(11 - 1) * 16 + 10];
        public string Voltage_BMU11_12 => Voltage_Array[(11 - 1) * 16 + 11];
        public string Voltage_BMU11_13 => Voltage_Array[(11 - 1) * 16 + 12];
        public string Voltage_BMU11_14 => Voltage_Array[(11 - 1) * 16 + 13];
        public string Voltage_BMU11_15 => Voltage_Array[(11 - 1) * 16 + 14];
        public string Voltage_BMU11_16 => Voltage_Array[(11 - 1) * 16 + 15];

        public string Voltage_BMU12_1 => Voltage_Array[(12 - 1) * 16];
        public string Voltage_BMU12_2 => Voltage_Array[(12 - 1) * 16 + 1];
        public string Voltage_BMU12_3 => Voltage_Array[(12 - 1) * 16 + 2];
        public string Voltage_BMU12_4 => Voltage_Array[(12 - 1) * 16 + 3];
        public string Voltage_BMU12_5 => Voltage_Array[(12 - 1) * 16 + 4];
        public string Voltage_BMU12_6 => Voltage_Array[(12 - 1) * 16 + 5];
        public string Voltage_BMU12_7 => Voltage_Array[(12 - 1) * 16 + 6];
        public string Voltage_BMU12_8 => Voltage_Array[(12 - 1) * 16 + 7];
        public string Voltage_BMU12_9 => Voltage_Array[(12 - 1) * 16 + 8];
        public string Voltage_BMU12_10 => Voltage_Array[(12 - 1) * 16 + 9];
        public string Voltage_BMU12_11 => Voltage_Array[(12 - 1) * 16 + 10];
        public string Voltage_BMU12_12 => Voltage_Array[(12 - 1) * 16 + 11];
        public string Voltage_BMU12_13 => Voltage_Array[(12 - 1) * 16 + 12];
        public string Voltage_BMU12_14 => Voltage_Array[(12 - 1) * 16 + 13];
        public string Voltage_BMU12_15 => Voltage_Array[(12 - 1) * 16 + 14];
        public string Voltage_BMU12_16 => Voltage_Array[(12 - 1) * 16 + 15];

        #endregion

        //0x682：电芯温度
        public string[] Tempeartrue_Array { get; set; } = new string[120];
        #region 只读温度字段
        public string Tempeartrue_BMU1_1 => Tempeartrue_Array[0];
        public string Tempeartrue_BMU1_2 => Tempeartrue_Array[1];
        public string Tempeartrue_BMU1_3 => Tempeartrue_Array[2];
        public string Tempeartrue_BMU1_4 => Tempeartrue_Array[3];
        public string Tempeartrue_BMU1_5 => Tempeartrue_Array[4];
        public string Tempeartrue_BMU1_6 => Tempeartrue_Array[5];
        public string Tempeartrue_BMU1_7 => Tempeartrue_Array[6];
        public string Tempeartrue_BMU1_8 => Tempeartrue_Array[7];

        public string Tempeartrue_BMU2_1 => Tempeartrue_Array[(2 - 1) * 8];
        public string Tempeartrue_BMU2_2 => Tempeartrue_Array[(2 - 1) * 8 + 1];
        public string Tempeartrue_BMU2_3 => Tempeartrue_Array[(2 - 1) * 8 + 2];
        public string Tempeartrue_BMU2_4 => Tempeartrue_Array[(2 - 1) * 8 + 3];
        public string Tempeartrue_BMU2_5 => Tempeartrue_Array[(2 - 1) * 8 + 4];
        public string Tempeartrue_BMU2_6 => Tempeartrue_Array[(2 - 1) * 8 + 5];
        public string Tempeartrue_BMU2_7 => Tempeartrue_Array[(2 - 1) * 8 + 6];
        public string Tempeartrue_BMU2_8 => Tempeartrue_Array[(2 - 1) * 8 + 7];

        public string Tempeartrue_BMU3_1 => Tempeartrue_Array[(3 - 1) * 8];
        public string Tempeartrue_BMU3_2 => Tempeartrue_Array[(3 - 1) * 8 + 1];
        public string Tempeartrue_BMU3_3 => Tempeartrue_Array[(3 - 1) * 8 + 2];
        public string Tempeartrue_BMU3_4 => Tempeartrue_Array[(3 - 1) * 8 + 3];
        public string Tempeartrue_BMU3_5 => Tempeartrue_Array[(3 - 1) * 8 + 4];
        public string Tempeartrue_BMU3_6 => Tempeartrue_Array[(3 - 1) * 8 + 5];
        public string Tempeartrue_BMU3_7 => Tempeartrue_Array[(3 - 1) * 8 + 6];
        public string Tempeartrue_BMU3_8 => Tempeartrue_Array[(3 - 1) * 8 + 7];

        public string Tempeartrue_BMU4_1 => Tempeartrue_Array[(4 - 1) * 8];
        public string Tempeartrue_BMU4_2 => Tempeartrue_Array[(4 - 1) * 8 + 1];
        public string Tempeartrue_BMU4_3 => Tempeartrue_Array[(4 - 1) * 8 + 2];
        public string Tempeartrue_BMU4_4 => Tempeartrue_Array[(4 - 1) * 8 + 3];
        public string Tempeartrue_BMU4_5 => Tempeartrue_Array[(4 - 1) * 8 + 4];
        public string Tempeartrue_BMU4_6 => Tempeartrue_Array[(4 - 1) * 8 + 5];
        public string Tempeartrue_BMU4_7 => Tempeartrue_Array[(4 - 1) * 8 + 6];
        public string Tempeartrue_BMU4_8 => Tempeartrue_Array[(4 - 1) * 8 + 7];

        public string Tempeartrue_BMU5_1 => Tempeartrue_Array[(5 - 1) * 8];
        public string Tempeartrue_BMU5_2 => Tempeartrue_Array[(5 - 1) * 8 + 1];
        public string Tempeartrue_BMU5_3 => Tempeartrue_Array[(5 - 1) * 8 + 2];
        public string Tempeartrue_BMU5_4 => Tempeartrue_Array[(5 - 1) * 8 + 3];
        public string Tempeartrue_BMU5_5 => Tempeartrue_Array[(5 - 1) * 8 + 4];
        public string Tempeartrue_BMU5_6 => Tempeartrue_Array[(5 - 1) * 8 + 5];
        public string Tempeartrue_BMU5_7 => Tempeartrue_Array[(5 - 1) * 8 + 6];
        public string Tempeartrue_BMU5_8 => Tempeartrue_Array[(5 - 1) * 8 + 7];

        public string Tempeartrue_BMU6_1 => Tempeartrue_Array[(6 - 1) * 8];
        public string Tempeartrue_BMU6_2 => Tempeartrue_Array[(6 - 1) * 8 + 1];
        public string Tempeartrue_BMU6_3 => Tempeartrue_Array[(6 - 1) * 8 + 2];
        public string Tempeartrue_BMU6_4 => Tempeartrue_Array[(6 - 1) * 8 + 3];
        public string Tempeartrue_BMU6_5 => Tempeartrue_Array[(6 - 1) * 8 + 4];
        public string Tempeartrue_BMU6_6 => Tempeartrue_Array[(6 - 1) * 8 + 5];
        public string Tempeartrue_BMU6_7 => Tempeartrue_Array[(6 - 1) * 8 + 6];
        public string Tempeartrue_BMU6_8 => Tempeartrue_Array[(6 - 1) * 8 + 7];

        public string Tempeartrue_BMU7_1 => Tempeartrue_Array[(7 - 1) * 8];
        public string Tempeartrue_BMU7_2 => Tempeartrue_Array[(7 - 1) * 8 + 1];
        public string Tempeartrue_BMU7_3 => Tempeartrue_Array[(7 - 1) * 8 + 2];
        public string Tempeartrue_BMU7_4 => Tempeartrue_Array[(7 - 1) * 8 + 3];
        public string Tempeartrue_BMU7_5 => Tempeartrue_Array[(7 - 1) * 8 + 4];
        public string Tempeartrue_BMU7_6 => Tempeartrue_Array[(7 - 1) * 8 + 5];
        public string Tempeartrue_BMU7_7 => Tempeartrue_Array[(7 - 1) * 8 + 6];
        public string Tempeartrue_BMU7_8 => Tempeartrue_Array[(7 - 1) * 8 + 7];

        public string Tempeartrue_BMU8_1 => Tempeartrue_Array[(8 - 1) * 8];
        public string Tempeartrue_BMU8_2 => Tempeartrue_Array[(8 - 1) * 8 + 1];
        public string Tempeartrue_BMU8_3 => Tempeartrue_Array[(8 - 1) * 8 + 2];
        public string Tempeartrue_BMU8_4 => Tempeartrue_Array[(8 - 1) * 8 + 3];
        public string Tempeartrue_BMU8_5 => Tempeartrue_Array[(8 - 1) * 8 + 4];
        public string Tempeartrue_BMU8_6 => Tempeartrue_Array[(8 - 1) * 8 + 5];
        public string Tempeartrue_BMU8_7 => Tempeartrue_Array[(8 - 1) * 8 + 6];
        public string Tempeartrue_BMU8_8 => Tempeartrue_Array[(8 - 1) * 8 + 7];

        public string Tempeartrue_BMU9_1 => Tempeartrue_Array[(9 - 1) * 8];
        public string Tempeartrue_BMU9_2 => Tempeartrue_Array[(9 - 1) * 8 + 1];
        public string Tempeartrue_BMU9_3 => Tempeartrue_Array[(9 - 1) * 8 + 2];
        public string Tempeartrue_BMU9_4 => Tempeartrue_Array[(9 - 1) * 8 + 3];
        public string Tempeartrue_BMU9_5 => Tempeartrue_Array[(9 - 1) * 8 + 4];
        public string Tempeartrue_BMU9_6 => Tempeartrue_Array[(9 - 1) * 8 + 5];
        public string Tempeartrue_BMU9_7 => Tempeartrue_Array[(9 - 1) * 8 + 6];
        public string Tempeartrue_BMU9_8 => Tempeartrue_Array[(9 - 1) * 8 + 7];

        public string Tempeartrue_BMU10_1 => Tempeartrue_Array[(10 - 1) * 8];
        public string Tempeartrue_BMU10_2 => Tempeartrue_Array[(10 - 1) * 8 + 1];
        public string Tempeartrue_BMU10_3 => Tempeartrue_Array[(10 - 1) * 8 + 2];
        public string Tempeartrue_BMU10_4 => Tempeartrue_Array[(10 - 1) * 8 + 3];
        public string Tempeartrue_BMU10_5 => Tempeartrue_Array[(10 - 1) * 8 + 4];
        public string Tempeartrue_BMU10_6 => Tempeartrue_Array[(10 - 1) * 8 + 5];
        public string Tempeartrue_BMU10_7 => Tempeartrue_Array[(10 - 1) * 8 + 6];
        public string Tempeartrue_BMU10_8 => Tempeartrue_Array[(10 - 1) * 8 + 7];

        public string Tempeartrue_BMU11_1 => Tempeartrue_Array[(11 - 1) * 8];
        public string Tempeartrue_BMU11_2 => Tempeartrue_Array[(11 - 1) * 8 + 1];
        public string Tempeartrue_BMU11_3 => Tempeartrue_Array[(11 - 1) * 8 + 2];
        public string Tempeartrue_BMU11_4 => Tempeartrue_Array[(11 - 1) * 8 + 3];
        public string Tempeartrue_BMU11_5 => Tempeartrue_Array[(11 - 1) * 8 + 4];
        public string Tempeartrue_BMU11_6 => Tempeartrue_Array[(11 - 1) * 8 + 5];
        public string Tempeartrue_BMU11_7 => Tempeartrue_Array[(11 - 1) * 8 + 6];
        public string Tempeartrue_BMU11_8 => Tempeartrue_Array[(11 - 1) * 8 + 7];

        public string Tempeartrue_BMU12_1 => Tempeartrue_Array[(12 - 1) * 8];
        public string Tempeartrue_BMU12_2 => Tempeartrue_Array[(12 - 1) * 8 + 1];
        public string Tempeartrue_BMU12_3 => Tempeartrue_Array[(12 - 1) * 8 + 2];
        public string Tempeartrue_BMU12_4 => Tempeartrue_Array[(12 - 1) * 8 + 3];
        public string Tempeartrue_BMU12_5 => Tempeartrue_Array[(12 - 1) * 8 + 4];
        public string Tempeartrue_BMU12_6 => Tempeartrue_Array[(12 - 1) * 8 + 5];
        public string Tempeartrue_BMU12_7 => Tempeartrue_Array[(12 - 1) * 8 + 6];
        public string Tempeartrue_BMU12_8 => Tempeartrue_Array[(12 - 1) * 8 + 7];

        #endregion

        //0x683：电芯SOC
        public string[] SOC_Array { get; set; } = new string[192];
        #region 只读SOC字段
        public string SOC_BMU1_1 => SOC_Array[0];
        public string SOC_BMU1_2 => SOC_Array[1];
        public string SOC_BMU1_3 => SOC_Array[2];
        public string SOC_BMU1_4 => SOC_Array[3];
        public string SOC_BMU1_5 => SOC_Array[4];
        public string SOC_BMU1_6 => SOC_Array[5];
        public string SOC_BMU1_7 => SOC_Array[6];
        public string SOC_BMU1_8 => SOC_Array[7];
        public string SOC_BMU1_9 => SOC_Array[8];
        public string SOC_BMU1_10 => SOC_Array[9];
        public string SOC_BMU1_11 => SOC_Array[10];
        public string SOC_BMU1_12 => SOC_Array[11];
        public string SOC_BMU1_13 => SOC_Array[12];
        public string SOC_BMU1_14 => SOC_Array[13];
        public string SOC_BMU1_15 => SOC_Array[14];
        public string SOC_BMU1_16 => SOC_Array[15];

        public string SOC_BMU2_1 => SOC_Array[(2 - 1) * 16];
        public string SOC_BMU2_2 => SOC_Array[(2 - 1) * 16 + 1];
        public string SOC_BMU2_3 => SOC_Array[(2 - 1) * 16 + 2];
        public string SOC_BMU2_4 => SOC_Array[(2 - 1) * 16 + 3];
        public string SOC_BMU2_5 => SOC_Array[(2 - 1) * 16 + 4];
        public string SOC_BMU2_6 => SOC_Array[(2 - 1) * 16 + 5];
        public string SOC_BMU2_7 => SOC_Array[(2 - 1) * 16 + 6];
        public string SOC_BMU2_8 => SOC_Array[(2 - 1) * 16 + 7];
        public string SOC_BMU2_9 => SOC_Array[(2 - 1) * 16 + 8];
        public string SOC_BMU2_10 => SOC_Array[(2 - 1) * 16 + 9];
        public string SOC_BMU2_11 => SOC_Array[(2 - 1) * 16 + 10];
        public string SOC_BMU2_12 => SOC_Array[(2 - 1) * 16 + 11];
        public string SOC_BMU2_13 => SOC_Array[(2 - 1) * 16 + 12];
        public string SOC_BMU2_14 => SOC_Array[(2 - 1) * 16 + 13];
        public string SOC_BMU2_15 => SOC_Array[(2 - 1) * 16 + 14];
        public string SOC_BMU2_16 => SOC_Array[(2 - 1) * 16 + 15];

        public string SOC_BMU3_1 => SOC_Array[(3 - 1) * 16];
        public string SOC_BMU3_2 => SOC_Array[(3 - 1) * 16 + 1];
        public string SOC_BMU3_3 => SOC_Array[(3 - 1) * 16 + 2];
        public string SOC_BMU3_4 => SOC_Array[(3 - 1) * 16 + 3];
        public string SOC_BMU3_5 => SOC_Array[(3 - 1) * 16 + 4];
        public string SOC_BMU3_6 => SOC_Array[(3 - 1) * 16 + 5];
        public string SOC_BMU3_7 => SOC_Array[(3 - 1) * 16 + 6];
        public string SOC_BMU3_8 => SOC_Array[(3 - 1) * 16 + 7];
        public string SOC_BMU3_9 => SOC_Array[(3 - 1) * 16 + 8];
        public string SOC_BMU3_10 => SOC_Array[(3 - 1) * 16 + 9];
        public string SOC_BMU3_11 => SOC_Array[(3 - 1) * 16 + 10];
        public string SOC_BMU3_12 => SOC_Array[(3 - 1) * 16 + 11];
        public string SOC_BMU3_13 => SOC_Array[(3 - 1) * 16 + 12];
        public string SOC_BMU3_14 => SOC_Array[(3 - 1) * 16 + 13];
        public string SOC_BMU3_15 => SOC_Array[(3 - 1) * 16 + 14];
        public string SOC_BMU3_16 => SOC_Array[(3 - 1) * 16 + 15];

        public string SOC_BMU4_1 => SOC_Array[(4 - 1) * 16];
        public string SOC_BMU4_2 => SOC_Array[(4 - 1) * 16 + 1];
        public string SOC_BMU4_3 => SOC_Array[(4 - 1) * 16 + 2];
        public string SOC_BMU4_4 => SOC_Array[(4 - 1) * 16 + 3];
        public string SOC_BMU4_5 => SOC_Array[(4 - 1) * 16 + 4];
        public string SOC_BMU4_6 => SOC_Array[(4 - 1) * 16 + 5];
        public string SOC_BMU4_7 => SOC_Array[(4 - 1) * 16 + 6];
        public string SOC_BMU4_8 => SOC_Array[(4 - 1) * 16 + 7];
        public string SOC_BMU4_9 => SOC_Array[(4 - 1) * 16 + 8];
        public string SOC_BMU4_10 => SOC_Array[(4 - 1) * 16 + 9];
        public string SOC_BMU4_11 => SOC_Array[(4 - 1) * 16 + 10];
        public string SOC_BMU4_12 => SOC_Array[(4 - 1) * 16 + 11];
        public string SOC_BMU4_13 => SOC_Array[(4 - 1) * 16 + 12];
        public string SOC_BMU4_14 => SOC_Array[(4 - 1) * 16 + 13];
        public string SOC_BMU4_15 => SOC_Array[(4 - 1) * 16 + 14];
        public string SOC_BMU4_16 => SOC_Array[(4 - 1) * 16 + 15];

        public string SOC_BMU5_1 => SOC_Array[(5 - 1) * 16];
        public string SOC_BMU5_2 => SOC_Array[(5 - 1) * 16 + 1];
        public string SOC_BMU5_3 => SOC_Array[(5 - 1) * 16 + 2];
        public string SOC_BMU5_4 => SOC_Array[(5 - 1) * 16 + 3];
        public string SOC_BMU5_5 => SOC_Array[(5 - 1) * 16 + 4];
        public string SOC_BMU5_6 => SOC_Array[(5 - 1) * 16 + 5];
        public string SOC_BMU5_7 => SOC_Array[(5 - 1) * 16 + 6];
        public string SOC_BMU5_8 => SOC_Array[(5 - 1) * 16 + 7];
        public string SOC_BMU5_9 => SOC_Array[(5 - 1) * 16 + 8];
        public string SOC_BMU5_10 => SOC_Array[(5 - 1) * 16 + 9];
        public string SOC_BMU5_11 => SOC_Array[(5 - 1) * 16 + 10];
        public string SOC_BMU5_12 => SOC_Array[(5 - 1) * 16 + 11];
        public string SOC_BMU5_13 => SOC_Array[(5 - 1) * 16 + 12];
        public string SOC_BMU5_14 => SOC_Array[(5 - 1) * 16 + 13];
        public string SOC_BMU5_15 => SOC_Array[(5 - 1) * 16 + 14];
        public string SOC_BMU5_16 => SOC_Array[(5 - 1) * 16 + 15];

        public string SOC_BMU6_1 => SOC_Array[(6 - 1) * 16];
        public string SOC_BMU6_2 => SOC_Array[(6 - 1) * 16 + 1];
        public string SOC_BMU6_3 => SOC_Array[(6 - 1) * 16 + 2];
        public string SOC_BMU6_4 => SOC_Array[(6 - 1) * 16 + 3];
        public string SOC_BMU6_5 => SOC_Array[(6 - 1) * 16 + 4];
        public string SOC_BMU6_6 => SOC_Array[(6 - 1) * 16 + 5];
        public string SOC_BMU6_7 => SOC_Array[(6 - 1) * 16 + 6];
        public string SOC_BMU6_8 => SOC_Array[(6 - 1) * 16 + 7];
        public string SOC_BMU6_9 => SOC_Array[(6 - 1) * 16 + 8];
        public string SOC_BMU6_10 => SOC_Array[(6 - 1) * 16 + 9];
        public string SOC_BMU6_11 => SOC_Array[(6 - 1) * 16 + 10];
        public string SOC_BMU6_12 => SOC_Array[(6 - 1) * 16 + 11];
        public string SOC_BMU6_13 => SOC_Array[(6 - 1) * 16 + 12];
        public string SOC_BMU6_14 => SOC_Array[(6 - 1) * 16 + 13];
        public string SOC_BMU6_15 => SOC_Array[(6 - 1) * 16 + 14];
        public string SOC_BMU6_16 => SOC_Array[(6 - 1) * 16 + 15];

        public string SOC_BMU7_1 => SOC_Array[(7 - 1) * 16];
        public string SOC_BMU7_2 => SOC_Array[(7 - 1) * 16 + 1];
        public string SOC_BMU7_3 => SOC_Array[(7 - 1) * 16 + 2];
        public string SOC_BMU7_4 => SOC_Array[(7 - 1) * 16 + 3];
        public string SOC_BMU7_5 => SOC_Array[(7 - 1) * 16 + 4];
        public string SOC_BMU7_6 => SOC_Array[(7 - 1) * 16 + 5];
        public string SOC_BMU7_7 => SOC_Array[(7 - 1) * 16 + 6];
        public string SOC_BMU7_8 => SOC_Array[(7 - 1) * 16 + 7];
        public string SOC_BMU7_9 => SOC_Array[(7 - 1) * 16 + 8];
        public string SOC_BMU7_10 => SOC_Array[(7 - 1) * 16 + 9];
        public string SOC_BMU7_11 => SOC_Array[(7 - 1) * 16 + 10];
        public string SOC_BMU7_12 => SOC_Array[(7 - 1) * 16 + 11];
        public string SOC_BMU7_13 => SOC_Array[(7 - 1) * 16 + 12];
        public string SOC_BMU7_14 => SOC_Array[(7 - 1) * 16 + 13];
        public string SOC_BMU7_15 => SOC_Array[(7 - 1) * 16 + 14];
        public string SOC_BMU7_16 => SOC_Array[(7 - 1) * 16 + 15];

        public string SOC_BMU8_1 => SOC_Array[(8 - 1) * 16];
        public string SOC_BMU8_2 => SOC_Array[(8 - 1) * 16 + 1];
        public string SOC_BMU8_3 => SOC_Array[(8 - 1) * 16 + 2];
        public string SOC_BMU8_4 => SOC_Array[(8 - 1) * 16 + 3];
        public string SOC_BMU8_5 => SOC_Array[(8 - 1) * 16 + 4];
        public string SOC_BMU8_6 => SOC_Array[(8 - 1) * 16 + 5];
        public string SOC_BMU8_7 => SOC_Array[(8 - 1) * 16 + 6];
        public string SOC_BMU8_8 => SOC_Array[(8 - 1) * 16 + 7];
        public string SOC_BMU8_9 => SOC_Array[(8 - 1) * 16 + 8];
        public string SOC_BMU8_10 => SOC_Array[(8 - 1) * 16 + 9];
        public string SOC_BMU8_11 => SOC_Array[(8 - 1) * 16 + 10];
        public string SOC_BMU8_12 => SOC_Array[(8 - 1) * 16 + 11];
        public string SOC_BMU8_13 => SOC_Array[(8 - 1) * 16 + 12];
        public string SOC_BMU8_14 => SOC_Array[(8 - 1) * 16 + 13];
        public string SOC_BMU8_15 => SOC_Array[(8 - 1) * 16 + 14];
        public string SOC_BMU8_16 => SOC_Array[(8 - 1) * 16 + 15];

        public string SOC_BMU9_1 => SOC_Array[(9 - 1) * 16];
        public string SOC_BMU9_2 => SOC_Array[(9 - 1) * 16 + 1];
        public string SOC_BMU9_3 => SOC_Array[(9 - 1) * 16 + 2];
        public string SOC_BMU9_4 => SOC_Array[(9 - 1) * 16 + 3];
        public string SOC_BMU9_5 => SOC_Array[(9 - 1) * 16 + 4];
        public string SOC_BMU9_6 => SOC_Array[(9 - 1) * 16 + 5];
        public string SOC_BMU9_7 => SOC_Array[(9 - 1) * 16 + 6];
        public string SOC_BMU9_8 => SOC_Array[(9 - 1) * 16 + 7];
        public string SOC_BMU9_9 => SOC_Array[(9 - 1) * 16 + 8];
        public string SOC_BMU9_10 => SOC_Array[(9 - 1) * 16 + 9];
        public string SOC_BMU9_11 => SOC_Array[(9 - 1) * 16 + 10];
        public string SOC_BMU9_12 => SOC_Array[(9 - 1) * 16 + 11];
        public string SOC_BMU9_13 => SOC_Array[(9 - 1) * 16 + 12];
        public string SOC_BMU9_14 => SOC_Array[(9 - 1) * 16 + 13];
        public string SOC_BMU9_15 => SOC_Array[(9 - 1) * 16 + 14];
        public string SOC_BMU9_16 => SOC_Array[(9 - 1) * 16 + 15];

        public string SOC_BMU10_1 => SOC_Array[(10 - 1) * 16];
        public string SOC_BMU10_2 => SOC_Array[(10 - 1) * 16 + 1];
        public string SOC_BMU10_3 => SOC_Array[(10 - 1) * 16 + 2];
        public string SOC_BMU10_4 => SOC_Array[(10 - 1) * 16 + 3];
        public string SOC_BMU10_5 => SOC_Array[(10 - 1) * 16 + 4];
        public string SOC_BMU10_6 => SOC_Array[(10 - 1) * 16 + 5];
        public string SOC_BMU10_7 => SOC_Array[(10 - 1) * 16 + 6];
        public string SOC_BMU10_8 => SOC_Array[(10 - 1) * 16 + 7];
        public string SOC_BMU10_9 => SOC_Array[(10 - 1) * 16 + 8];
        public string SOC_BMU10_10 => SOC_Array[(10 - 1) * 16 + 9];
        public string SOC_BMU10_11 => SOC_Array[(10 - 1) * 16 + 10];
        public string SOC_BMU10_12 => SOC_Array[(10 - 1) * 16 + 11];
        public string SOC_BMU10_13 => SOC_Array[(10 - 1) * 16 + 12];
        public string SOC_BMU10_14 => SOC_Array[(10 - 1) * 16 + 13];
        public string SOC_BMU10_15 => SOC_Array[(10 - 1) * 16 + 14];
        public string SOC_BMU10_16 => SOC_Array[(10 - 1) * 16 + 15];

        public string SOC_BMU11_1 => SOC_Array[(11 - 1) * 16];
        public string SOC_BMU11_2 => SOC_Array[(11 - 1) * 16 + 1];
        public string SOC_BMU11_3 => SOC_Array[(11 - 1) * 16 + 2];
        public string SOC_BMU11_4 => SOC_Array[(11 - 1) * 16 + 3];
        public string SOC_BMU11_5 => SOC_Array[(11 - 1) * 16 + 4];
        public string SOC_BMU11_6 => SOC_Array[(11 - 1) * 16 + 5];
        public string SOC_BMU11_7 => SOC_Array[(11 - 1) * 16 + 6];
        public string SOC_BMU11_8 => SOC_Array[(11 - 1) * 16 + 7];
        public string SOC_BMU11_9 => SOC_Array[(11 - 1) * 16 + 8];
        public string SOC_BMU11_10 => SOC_Array[(11 - 1) * 16 + 9];
        public string SOC_BMU11_11 => SOC_Array[(11 - 1) * 16 + 10];
        public string SOC_BMU11_12 => SOC_Array[(11 - 1) * 16 + 11];
        public string SOC_BMU11_13 => SOC_Array[(11 - 1) * 16 + 12];
        public string SOC_BMU11_14 => SOC_Array[(11 - 1) * 16 + 13];
        public string SOC_BMU11_15 => SOC_Array[(11 - 1) * 16 + 14];
        public string SOC_BMU11_16 => SOC_Array[(11 - 1) * 16 + 15];

        public string SOC_BMU12_1 => SOC_Array[(12 - 1) * 16];
        public string SOC_BMU12_2 => SOC_Array[(12 - 1) * 16 + 1];
        public string SOC_BMU12_3 => SOC_Array[(12 - 1) * 16 + 2];
        public string SOC_BMU12_4 => SOC_Array[(12 - 1) * 16 + 3];
        public string SOC_BMU12_5 => SOC_Array[(12 - 1) * 16 + 4];
        public string SOC_BMU12_6 => SOC_Array[(12 - 1) * 16 + 5];
        public string SOC_BMU12_7 => SOC_Array[(12 - 1) * 16 + 6];
        public string SOC_BMU12_8 => SOC_Array[(12 - 1) * 16 + 7];
        public string SOC_BMU12_9 => SOC_Array[(12 - 1) * 16 + 8];
        public string SOC_BMU12_10 => SOC_Array[(12 - 1) * 16 + 9];
        public string SOC_BMU12_11 => SOC_Array[(12 - 1) * 16 + 10];
        public string SOC_BMU12_12 => SOC_Array[(12 - 1) * 16 + 11];
        public string SOC_BMU12_13 => SOC_Array[(12 - 1) * 16 + 12];
        public string SOC_BMU12_14 => SOC_Array[(12 - 1) * 16 + 13];
        public string SOC_BMU12_15 => SOC_Array[(12 - 1) * 16 + 14];
        public string SOC_BMU12_16 => SOC_Array[(12 - 1) * 16 + 15];

        #endregion

        //0x684：电芯SOH
        public string[] SOH_Array { get; set; } = new string[192];
        #region 只读SOH字段
        public string SOH_BMU1_1 => SOH_Array[0];
        public string SOH_BMU1_2 => SOH_Array[1];
        public string SOH_BMU1_3 => SOH_Array[2];
        public string SOH_BMU1_4 => SOH_Array[3];
        public string SOH_BMU1_5 => SOH_Array[4];
        public string SOH_BMU1_6 => SOH_Array[5];
        public string SOH_BMU1_7 => SOH_Array[6];
        public string SOH_BMU1_8 => SOH_Array[7];
        public string SOH_BMU1_9 => SOH_Array[8];
        public string SOH_BMU1_10 => SOH_Array[9];
        public string SOH_BMU1_11 => SOH_Array[10];
        public string SOH_BMU1_12 => SOH_Array[11];
        public string SOH_BMU1_13 => SOH_Array[12];
        public string SOH_BMU1_14 => SOH_Array[13];
        public string SOH_BMU1_15 => SOH_Array[14];
        public string SOH_BMU1_16 => SOH_Array[15];

        public string SOH_BMU2_1 => SOH_Array[(2 - 1) * 16];
        public string SOH_BMU2_2 => SOH_Array[(2 - 1) * 16 + 1];
        public string SOH_BMU2_3 => SOH_Array[(2 - 1) * 16 + 2];
        public string SOH_BMU2_4 => SOH_Array[(2 - 1) * 16 + 3];
        public string SOH_BMU2_5 => SOH_Array[(2 - 1) * 16 + 4];
        public string SOH_BMU2_6 => SOH_Array[(2 - 1) * 16 + 5];
        public string SOH_BMU2_7 => SOH_Array[(2 - 1) * 16 + 6];
        public string SOH_BMU2_8 => SOH_Array[(2 - 1) * 16 + 7];
        public string SOH_BMU2_9 => SOH_Array[(2 - 1) * 16 + 8];
        public string SOH_BMU2_10 => SOH_Array[(2 - 1) * 16 + 9];
        public string SOH_BMU2_11 => SOH_Array[(2 - 1) * 16 + 10];
        public string SOH_BMU2_12 => SOH_Array[(2 - 1) * 16 + 11];
        public string SOH_BMU2_13 => SOH_Array[(2 - 1) * 16 + 12];
        public string SOH_BMU2_14 => SOH_Array[(2 - 1) * 16 + 13];
        public string SOH_BMU2_15 => SOH_Array[(2 - 1) * 16 + 14];
        public string SOH_BMU2_16 => SOH_Array[(2 - 1) * 16 + 15];

        public string SOH_BMU3_1 => SOH_Array[(3 - 1) * 16];
        public string SOH_BMU3_2 => SOH_Array[(3 - 1) * 16 + 1];
        public string SOH_BMU3_3 => SOH_Array[(3 - 1) * 16 + 2];
        public string SOH_BMU3_4 => SOH_Array[(3 - 1) * 16 + 3];
        public string SOH_BMU3_5 => SOH_Array[(3 - 1) * 16 + 4];
        public string SOH_BMU3_6 => SOH_Array[(3 - 1) * 16 + 5];
        public string SOH_BMU3_7 => SOH_Array[(3 - 1) * 16 + 6];
        public string SOH_BMU3_8 => SOH_Array[(3 - 1) * 16 + 7];
        public string SOH_BMU3_9 => SOH_Array[(3 - 1) * 16 + 8];
        public string SOH_BMU3_10 => SOH_Array[(3 - 1) * 16 + 9];
        public string SOH_BMU3_11 => SOH_Array[(3 - 1) * 16 + 10];
        public string SOH_BMU3_12 => SOH_Array[(3 - 1) * 16 + 11];
        public string SOH_BMU3_13 => SOH_Array[(3 - 1) * 16 + 12];
        public string SOH_BMU3_14 => SOH_Array[(3 - 1) * 16 + 13];
        public string SOH_BMU3_15 => SOH_Array[(3 - 1) * 16 + 14];
        public string SOH_BMU3_16 => SOH_Array[(3 - 1) * 16 + 15];

        public string SOH_BMU4_1 => SOH_Array[(4 - 1) * 16];
        public string SOH_BMU4_2 => SOH_Array[(4 - 1) * 16 + 1];
        public string SOH_BMU4_3 => SOH_Array[(4 - 1) * 16 + 2];
        public string SOH_BMU4_4 => SOH_Array[(4 - 1) * 16 + 3];
        public string SOH_BMU4_5 => SOH_Array[(4 - 1) * 16 + 4];
        public string SOH_BMU4_6 => SOH_Array[(4 - 1) * 16 + 5];
        public string SOH_BMU4_7 => SOH_Array[(4 - 1) * 16 + 6];
        public string SOH_BMU4_8 => SOH_Array[(4 - 1) * 16 + 7];
        public string SOH_BMU4_9 => SOH_Array[(4 - 1) * 16 + 8];
        public string SOH_BMU4_10 => SOH_Array[(4 - 1) * 16 + 9];
        public string SOH_BMU4_11 => SOH_Array[(4 - 1) * 16 + 10];
        public string SOH_BMU4_12 => SOH_Array[(4 - 1) * 16 + 11];
        public string SOH_BMU4_13 => SOH_Array[(4 - 1) * 16 + 12];
        public string SOH_BMU4_14 => SOH_Array[(4 - 1) * 16 + 13];
        public string SOH_BMU4_15 => SOH_Array[(4 - 1) * 16 + 14];
        public string SOH_BMU4_16 => SOH_Array[(4 - 1) * 16 + 15];

        public string SOH_BMU5_1 => SOH_Array[(5 - 1) * 16];
        public string SOH_BMU5_2 => SOH_Array[(5 - 1) * 16 + 1];
        public string SOH_BMU5_3 => SOH_Array[(5 - 1) * 16 + 2];
        public string SOH_BMU5_4 => SOH_Array[(5 - 1) * 16 + 3];
        public string SOH_BMU5_5 => SOH_Array[(5 - 1) * 16 + 4];
        public string SOH_BMU5_6 => SOH_Array[(5 - 1) * 16 + 5];
        public string SOH_BMU5_7 => SOH_Array[(5 - 1) * 16 + 6];
        public string SOH_BMU5_8 => SOH_Array[(5 - 1) * 16 + 7];
        public string SOH_BMU5_9 => SOH_Array[(5 - 1) * 16 + 8];
        public string SOH_BMU5_10 => SOH_Array[(5 - 1) * 16 + 9];
        public string SOH_BMU5_11 => SOH_Array[(5 - 1) * 16 + 10];
        public string SOH_BMU5_12 => SOH_Array[(5 - 1) * 16 + 11];
        public string SOH_BMU5_13 => SOH_Array[(5 - 1) * 16 + 12];
        public string SOH_BMU5_14 => SOH_Array[(5 - 1) * 16 + 13];
        public string SOH_BMU5_15 => SOH_Array[(5 - 1) * 16 + 14];
        public string SOH_BMU5_16 => SOH_Array[(5 - 1) * 16 + 15];

        public string SOH_BMU6_1 => SOH_Array[(6 - 1) * 16];
        public string SOH_BMU6_2 => SOH_Array[(6 - 1) * 16 + 1];
        public string SOH_BMU6_3 => SOH_Array[(6 - 1) * 16 + 2];
        public string SOH_BMU6_4 => SOH_Array[(6 - 1) * 16 + 3];
        public string SOH_BMU6_5 => SOH_Array[(6 - 1) * 16 + 4];
        public string SOH_BMU6_6 => SOH_Array[(6 - 1) * 16 + 5];
        public string SOH_BMU6_7 => SOH_Array[(6 - 1) * 16 + 6];
        public string SOH_BMU6_8 => SOH_Array[(6 - 1) * 16 + 7];
        public string SOH_BMU6_9 => SOH_Array[(6 - 1) * 16 + 8];
        public string SOH_BMU6_10 => SOH_Array[(6 - 1) * 16 + 9];
        public string SOH_BMU6_11 => SOH_Array[(6 - 1) * 16 + 10];
        public string SOH_BMU6_12 => SOH_Array[(6 - 1) * 16 + 11];
        public string SOH_BMU6_13 => SOH_Array[(6 - 1) * 16 + 12];
        public string SOH_BMU6_14 => SOH_Array[(6 - 1) * 16 + 13];
        public string SOH_BMU6_15 => SOH_Array[(6 - 1) * 16 + 14];
        public string SOH_BMU6_16 => SOH_Array[(6 - 1) * 16 + 15];

        public string SOH_BMU7_1 => SOH_Array[(7 - 1) * 16];
        public string SOH_BMU7_2 => SOH_Array[(7 - 1) * 16 + 1];
        public string SOH_BMU7_3 => SOH_Array[(7 - 1) * 16 + 2];
        public string SOH_BMU7_4 => SOH_Array[(7 - 1) * 16 + 3];
        public string SOH_BMU7_5 => SOH_Array[(7 - 1) * 16 + 4];
        public string SOH_BMU7_6 => SOH_Array[(7 - 1) * 16 + 5];
        public string SOH_BMU7_7 => SOH_Array[(7 - 1) * 16 + 6];
        public string SOH_BMU7_8 => SOH_Array[(7 - 1) * 16 + 7];
        public string SOH_BMU7_9 => SOH_Array[(7 - 1) * 16 + 8];
        public string SOH_BMU7_10 => SOH_Array[(7 - 1) * 16 + 9];
        public string SOH_BMU7_11 => SOH_Array[(7 - 1) * 16 + 10];
        public string SOH_BMU7_12 => SOH_Array[(7 - 1) * 16 + 11];
        public string SOH_BMU7_13 => SOH_Array[(7 - 1) * 16 + 12];
        public string SOH_BMU7_14 => SOH_Array[(7 - 1) * 16 + 13];
        public string SOH_BMU7_15 => SOH_Array[(7 - 1) * 16 + 14];
        public string SOH_BMU7_16 => SOH_Array[(7 - 1) * 16 + 15];

        public string SOH_BMU8_1 => SOH_Array[(8 - 1) * 16];
        public string SOH_BMU8_2 => SOH_Array[(8 - 1) * 16 + 1];
        public string SOH_BMU8_3 => SOH_Array[(8 - 1) * 16 + 2];
        public string SOH_BMU8_4 => SOH_Array[(8 - 1) * 16 + 3];
        public string SOH_BMU8_5 => SOH_Array[(8 - 1) * 16 + 4];
        public string SOH_BMU8_6 => SOH_Array[(8 - 1) * 16 + 5];
        public string SOH_BMU8_7 => SOH_Array[(8 - 1) * 16 + 6];
        public string SOH_BMU8_8 => SOH_Array[(8 - 1) * 16 + 7];
        public string SOH_BMU8_9 => SOH_Array[(8 - 1) * 16 + 8];
        public string SOH_BMU8_10 => SOH_Array[(8 - 1) * 16 + 9];
        public string SOH_BMU8_11 => SOH_Array[(8 - 1) * 16 + 10];
        public string SOH_BMU8_12 => SOH_Array[(8 - 1) * 16 + 11];
        public string SOH_BMU8_13 => SOH_Array[(8 - 1) * 16 + 12];
        public string SOH_BMU8_14 => SOH_Array[(8 - 1) * 16 + 13];
        public string SOH_BMU8_15 => SOH_Array[(8 - 1) * 16 + 14];
        public string SOH_BMU8_16 => SOH_Array[(8 - 1) * 16 + 15];

        public string SOH_BMU9_1 => SOH_Array[(9 - 1) * 16];
        public string SOH_BMU9_2 => SOH_Array[(9 - 1) * 16 + 1];
        public string SOH_BMU9_3 => SOH_Array[(9 - 1) * 16 + 2];
        public string SOH_BMU9_4 => SOH_Array[(9 - 1) * 16 + 3];
        public string SOH_BMU9_5 => SOH_Array[(9 - 1) * 16 + 4];
        public string SOH_BMU9_6 => SOH_Array[(9 - 1) * 16 + 5];
        public string SOH_BMU9_7 => SOH_Array[(9 - 1) * 16 + 6];
        public string SOH_BMU9_8 => SOH_Array[(9 - 1) * 16 + 7];
        public string SOH_BMU9_9 => SOH_Array[(9 - 1) * 16 + 8];
        public string SOH_BMU9_10 => SOH_Array[(9 - 1) * 16 + 9];
        public string SOH_BMU9_11 => SOH_Array[(9 - 1) * 16 + 10];
        public string SOH_BMU9_12 => SOH_Array[(9 - 1) * 16 + 11];
        public string SOH_BMU9_13 => SOH_Array[(9 - 1) * 16 + 12];
        public string SOH_BMU9_14 => SOH_Array[(9 - 1) * 16 + 13];
        public string SOH_BMU9_15 => SOH_Array[(9 - 1) * 16 + 14];
        public string SOH_BMU9_16 => SOH_Array[(9 - 1) * 16 + 15];

        public string SOH_BMU10_1 => SOH_Array[(10 - 1) * 16];
        public string SOH_BMU10_2 => SOH_Array[(10 - 1) * 16 + 1];
        public string SOH_BMU10_3 => SOH_Array[(10 - 1) * 16 + 2];
        public string SOH_BMU10_4 => SOH_Array[(10 - 1) * 16 + 3];
        public string SOH_BMU10_5 => SOH_Array[(10 - 1) * 16 + 4];
        public string SOH_BMU10_6 => SOH_Array[(10 - 1) * 16 + 5];
        public string SOH_BMU10_7 => SOH_Array[(10 - 1) * 16 + 6];
        public string SOH_BMU10_8 => SOH_Array[(10 - 1) * 16 + 7];
        public string SOH_BMU10_9 => SOH_Array[(10 - 1) * 16 + 8];
        public string SOH_BMU10_10 => SOH_Array[(10 - 1) * 16 + 9];
        public string SOH_BMU10_11 => SOH_Array[(10 - 1) * 16 + 10];
        public string SOH_BMU10_12 => SOH_Array[(10 - 1) * 16 + 11];
        public string SOH_BMU10_13 => SOH_Array[(10 - 1) * 16 + 12];
        public string SOH_BMU10_14 => SOH_Array[(10 - 1) * 16 + 13];
        public string SOH_BMU10_15 => SOH_Array[(10 - 1) * 16 + 14];
        public string SOH_BMU10_16 => SOH_Array[(10 - 1) * 16 + 15];

        public string SOH_BMU11_1 => SOH_Array[(11 - 1) * 16];
        public string SOH_BMU11_2 => SOH_Array[(11 - 1) * 16 + 1];
        public string SOH_BMU11_3 => SOH_Array[(11 - 1) * 16 + 2];
        public string SOH_BMU11_4 => SOH_Array[(11 - 1) * 16 + 3];
        public string SOH_BMU11_5 => SOH_Array[(11 - 1) * 16 + 4];
        public string SOH_BMU11_6 => SOH_Array[(11 - 1) * 16 + 5];
        public string SOH_BMU11_7 => SOH_Array[(11 - 1) * 16 + 6];
        public string SOH_BMU11_8 => SOH_Array[(11 - 1) * 16 + 7];
        public string SOH_BMU11_9 => SOH_Array[(11 - 1) * 16 + 8];
        public string SOH_BMU11_10 => SOH_Array[(11 - 1) * 16 + 9];
        public string SOH_BMU11_11 => SOH_Array[(11 - 1) * 16 + 10];
        public string SOH_BMU11_12 => SOH_Array[(11 - 1) * 16 + 11];
        public string SOH_BMU11_13 => SOH_Array[(11 - 1) * 16 + 12];
        public string SOH_BMU11_14 => SOH_Array[(11 - 1) * 16 + 13];
        public string SOH_BMU11_15 => SOH_Array[(11 - 1) * 16 + 14];
        public string SOH_BMU11_16 => SOH_Array[(11 - 1) * 16 + 15];

        public string SOH_BMU12_1 => SOH_Array[(12 - 1) * 16];
        public string SOH_BMU12_2 => SOH_Array[(12 - 1) * 16 + 1];
        public string SOH_BMU12_3 => SOH_Array[(12 - 1) * 16 + 2];
        public string SOH_BMU12_4 => SOH_Array[(12 - 1) * 16 + 3];
        public string SOH_BMU12_5 => SOH_Array[(12 - 1) * 16 + 4];
        public string SOH_BMU12_6 => SOH_Array[(12 - 1) * 16 + 5];
        public string SOH_BMU12_7 => SOH_Array[(12 - 1) * 16 + 6];
        public string SOH_BMU12_8 => SOH_Array[(12 - 1) * 16 + 7];
        public string SOH_BMU12_9 => SOH_Array[(12 - 1) * 16 + 8];
        public string SOH_BMU12_10 => SOH_Array[(12 - 1) * 16 + 9];
        public string SOH_BMU12_11 => SOH_Array[(12 - 1) * 16 + 10];
        public string SOH_BMU12_12 => SOH_Array[(12 - 1) * 16 + 11];
        public string SOH_BMU12_13 => SOH_Array[(12 - 1) * 16 + 12];
        public string SOH_BMU12_14 => SOH_Array[(12 - 1) * 16 + 13];
        public string SOH_BMU12_15 => SOH_Array[(12 - 1) * 16 + 14];
        public string SOH_BMU12_16 => SOH_Array[(12 - 1) * 16 + 15];

        #endregion

        public string GetHeader() => string.Join(",", CsvFields.Select(f => PropertyHeaderMap[f])) + "," + string.Join(",", customizeFields.Select(t => CustomizePropertyHeaderMap[t]));

        public string GetValue()
        {
            var values = new List<object>();
            foreach (var field in CsvFields)
            {
                var prop = GetType().GetProperty(field);
                values.Add(prop?.GetValue(this)?.ToString() ?? string.Empty);
            }


            foreach (var field in customizeFields)
            {
                var prop = GetType().GetProperty(field);
                values.Add(prop?.GetValue(this)?.ToString() ?? string.Empty);
            }

            return string.Join(",", values);
        }
    }
}