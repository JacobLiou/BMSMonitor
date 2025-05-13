using System.Collections.Generic;

namespace SofarBMS.Model
{
    public class RealtimeData_CBS5000S_BCU
    {
        // 列顺序定义（按实际业务顺序排列）
        private static readonly string[] CsvFields =
        {
            nameof(CreateDate),
            nameof(PackID),
            nameof(Fault),
            nameof(Warning),
            nameof(Protection),
            nameof(Fault2),
            nameof(Warning2),
            nameof(Protection2),
            nameof(Di_Status_Get),
            nameof(Balance_Chg_Status),
            nameof(Balance_Dchg_Status),
            nameof(Relay_Status),
            nameof(Other_Dev_Status),
            nameof(Power_Terminal_Temperature1),
            nameof(Power_Terminal_Temperature2),
            nameof(Power_Terminal_Temperature3),
            nameof(Power_Terminal_Temperature4),
            nameof(Ambient_Temperature),
            nameof(Battery_Sampling_Voltage1),
            nameof(Battery_Sampling_Voltage2),
            nameof(Bus_Sampling_Voltage),
            nameof(Battery_Summing_Voltage),
            nameof(HeatingFilm_Voltage),
            nameof(HeatingFilm_MosVoltage),
            nameof(Insulation_Resistance),
            nameof(Auxiliary_Power_Supply_Voltage),
            nameof(Cluster_Current),
            nameof(Cluster_Max_Cell_Volt),
            nameof(Cluster_Max_Cell_Volt_Pack),
            nameof(Cluster_Max_Cell_VoltNum),
            nameof(Cluster_Min_Cell_Volt),
            nameof(Cluster_Min_Cell_Volt_Pack),
            nameof(Cluster_Min_Cell_Volt_Num),
            nameof(Cluster_Max_Cell_Temp),
            nameof(Cluster_Max_Cell_Temp_Pack),
            nameof(Cluster_Max_Cell_Temp_Num),
            nameof(Cluster_Min_Cell_Temp),
            nameof(Cluster_Min_Cell_Temp_Pack),
            nameof(Cluster_Min_Cell_Temp_Num),
            nameof(Battery_Charge_Voltage),
            nameof(Charge_Current_Limitation),
            nameof(Discharge_Current_Limitation),
            nameof(Battery_Discharge_Voltage),
            nameof(BCUSoftwareVersion),
            nameof(BCUHardwareVersion),
            nameof(CANProtocolVersion),
            nameof(Remaining_Total_Capacity),
            nameof(Bat_Average_Temp),
            nameof(Cluster_Rate_Power),
            nameof(Cycles),
            nameof(Bms_State),
            nameof(Cluster_SOC),
            nameof(Cluster_SOH),
            nameof(Cluster_BatPack_Num),
            nameof(BCU_SytemTime),
            nameof(SN),
            nameof(Inductive_Current_Sampling1),
            nameof(Inductive_Current_Sampling2),
            nameof(Inductive_Current_Sampling3),
            nameof(Inductive_Current_Sampling4),
            nameof(Discharge_Current_Limitation),
            nameof(Charge_Current_Limitation),
            nameof(Contactor_Voltage),
            nameof(Battery_Cluster_Voltage1),
            nameof(High_Bus_Voltage),
            nameof(HeatingFilm_Power_Supply_Voltage),
            nameof(Radiator_temperature1),
            nameof(Radiator_temperature2),
            nameof(Radiator_temperature3),
            nameof(Radiator_temperature4),
            nameof(PCUSoftwareVersion),
            nameof(PCUHardwareVersion),
            nameof(SCIProtocolVersion),
            nameof(PCU_WorkState),
            nameof(PCU_BatteryState),
            nameof(Discharge_Limit_Current_Value),
            nameof(Charge_Limit_Current_Value)
    };

        // 属性到中文列名的映射
        private static readonly Dictionary<string, string> PropertyHeaderMap = new()
        {
            [nameof(CreateDate)] = "记录时间",
            [nameof(PackID)] = "电池ID",
            [nameof(Fault)] = "故障信息1",
            [nameof(Warning)] = "告警信息1",
            [nameof(Protection)] = "保护信息1",
            [nameof(Fault2)] = "故障信息2",
            [nameof(Warning2)] = "告警信息2",
            [nameof(Protection2)] = "保护信息2",
            [nameof(Di_Status_Get)] = "BCU的di状态",
            [nameof(Balance_Chg_Status)] = "电池包充均衡状态",
            [nameof(Balance_Dchg_Status)] = "电池包放均衡状态",
            [nameof(Relay_Status)] = "继电器状态",
            [nameof(Other_Dev_Status)] = "器件的状态",
            [nameof(Power_Terminal_Temperature1)] = "P+端子温度",
            [nameof(Power_Terminal_Temperature2)] = "P-端子温度",
            [nameof(Power_Terminal_Temperature3)] = "B+端子温度",
            [nameof(Power_Terminal_Temperature4)] = "B-端子温度",
            [nameof(Ambient_Temperature)] = "环境温度",
            [nameof(Battery_Sampling_Voltage1)] = "电池采样电压1",
            [nameof(Battery_Sampling_Voltage2)] = "电池电压采样2",
            [nameof(Bus_Sampling_Voltage)] = "母线采样电压",
            [nameof(Battery_Summing_Voltage)] = "电池累加和电压",
            [nameof(HeatingFilm_Voltage)] = "加热膜供电电压",
            [nameof(HeatingFilm_MosVoltage)] = "加热膜MOS电压",
            [nameof(Insulation_Resistance)] = "绝缘电阻电压",
            [nameof(Auxiliary_Power_Supply_Voltage)] = "辅源电压",
            [nameof(Cluster_Current)] = "簇电流",
            [nameof(Cluster_Max_Cell_Volt)] = "簇最高单体电压",
            [nameof(Cluster_Max_Cell_Volt_Pack)] = "簇最高单体电压所在pack",
            [nameof(Cluster_Max_Cell_VoltNum)] = "簇最高单体电压所在pack的第几个位置",
            [nameof(Cluster_Min_Cell_Volt)] = "簇最低单体电压",
            [nameof(Cluster_Min_Cell_Volt_Pack)] = "簇最低单体电压所在pack",
            [nameof(Cluster_Min_Cell_Volt_Num)] = "簇最低单体电压所在pack的第几位置",
            [nameof(Cluster_Max_Cell_Temp)] = "簇最高单体温度",
            [nameof(Cluster_Max_Cell_Temp_Pack)] = "簇最高单体温度所在pack",
            [nameof(Cluster_Max_Cell_Temp_Num)] = "簇最高单体温度所在pack的第几个位置",
            [nameof(Cluster_Min_Cell_Temp)] = "簇最低单体温度",
            [nameof(Cluster_Min_Cell_Temp_Pack)] = "簇最低单体温度所在pack",
            [nameof(Cluster_Min_Cell_Temp_Num)] = "簇最低单体温度所在pack的第几个位置",
            [nameof(Battery_Charge_Voltage)] = "充电电压",
            [nameof(Charge_Current_Limitation)] = "充电电流上限",
            [nameof(Discharge_Current_Limitation)] = "放电电流上限",
            [nameof(Battery_Discharge_Voltage)] = "放电截止电压",
            [nameof(BCUSoftwareVersion)] = "BCU软件版本号",
            [nameof(BCUHardwareVersion)] = "BCU硬件版本号",
            [nameof(CANProtocolVersion)] = "CAN协议版本号",
            [nameof(Remaining_Total_Capacity)] = "总剩余容量",
            [nameof(Bat_Average_Temp)] = "电池平均温度",
            [nameof(Cluster_Rate_Power)] = "额定功率",
            [nameof(Cycles)] = "循环次数",
            [nameof(Bms_State)] = "BCU上送的BMS状态",
            [nameof(Cluster_SOC)] = "电池簇SOC",
            [nameof(Cluster_SOH)] = "电池簇SOH",
            [nameof(Cluster_BatPack_Num)] = "簇内电池包数量",
            [nameof(BCU_SytemTime)] = "BCU系统时间",
            [nameof(SN)] = "BCU序列号",
            [nameof(Inductive_Current_Sampling1)] = "电感电流采样1",
            [nameof(Inductive_Current_Sampling2)] = "电感电流采样2",
            [nameof(Inductive_Current_Sampling3)] = "电感电流采样3",
            [nameof(Inductive_Current_Sampling4)] = "电感电流采样4",
            [nameof(Discharge_Current_Limitation)] = "放电大电流采样",
            [nameof(Charge_Current_Limitation)] = "充电大电流采样",
            [nameof(Contactor_Voltage)] = "接触器电压",
            [nameof(Battery_Cluster_Voltage1)] = "电池簇电压1",
            [nameof(High_Bus_Voltage)] = "高压母线电压",
            [nameof(HeatingFilm_Power_Supply_Voltage)] = "加热膜供电电压",
            [nameof(Radiator_temperature1)] = "散热器温度1",
            [nameof(Radiator_temperature2)] = "散热器温度2",
            [nameof(Radiator_temperature3)] = "散热器温度3",
            [nameof(Radiator_temperature4)] = "散热器温度4",
            [nameof(PCUSoftwareVersion)] = "PCU软件版本号",
            [nameof(PCUHardwareVersion)] = "PCU硬件版本号",
            [nameof(SCIProtocolVersion)] = "SCI协议版本号",
            [nameof(PCU_WorkState)] = "PCU工作状态",
            [nameof(PCU_BatteryState)] = "PCU运行状态",
            [nameof(Discharge_Limit_Current_Value)] = "充电限载电流值",
            [nameof(Charge_Limit_Current_Value)] = "放电限载电流值"
        };


        private static List<string> customizeFields = new List<string>();
        private static Dictionary<string, string> CustomizePropertyHeaderMap = new Dictionary<string, string>();//readonly

        public RealtimeData_CBS5000S_BCU()
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

        public string PackID { get; set; }
        public string CreateDate { get; } // 只读，构造时初始化

        // 0x0B6:BCU遥信数据上报1：
        public double Di_Status_Get { get; set; }
        public double Balance_Chg_Status { get; set; }
        public double Balance_Dchg_Status { get; set; }
        public double Relay_Status { get; set; }
        public double Other_Dev_Status { get; set; }

        //0x0B7:BCU遥信数据上报2
        public double Power_Terminal_Temperature1 { get; set; }
        public double Power_Terminal_Temperature2 { get; set; }
        public double Power_Terminal_Temperature3 { get; set; }
        public double Power_Terminal_Temperature4 { get; set; }
        public double Ambient_Temperature { get; set; }

        //0x0B8:BCU遥信数据上报3
        public double Battery_Sampling_Voltage1 { get; set; }
        public double Bus_Sampling_Voltage { get; set; }
        public double Battery_Summing_Voltage { get; set; }
        public double HeatingFilm_Voltage { get; set; }
        public double HeatingFilm_MosVoltage { get; set; }
        public double Insulation_Resistance { get; set; }
        public double Auxiliary_Power_Supply_Voltage { get; set; }
        //public double Fuse_Voltage { get; set; }
        //public double Power_Voltage { get; set; }

        //0x0B9:BCU遥信数据上报4
        public double Cluster_Current { get; set; }

        //0x0BA:BCU遥信数据上报5
        public double Cluster_Max_Cell_Volt { get; set; }
        public ushort Cluster_Max_Cell_Volt_Pack { get; set; }
        public ushort Cluster_Max_Cell_VoltNum { get; set; }
        public double Cluster_Min_Cell_Volt { get; set; }
        public ushort Cluster_Min_Cell_Volt_Pack { get; set; }
        public ushort Cluster_Min_Cell_Volt_Num { get; set; }

        //0x0BB:BCU遥信数据上报6
        public double Cluster_Max_Cell_Temp { get; set; }
        public ushort Cluster_Max_Cell_Temp_Pack { get; set; }
        public ushort Cluster_Max_Cell_Temp_Num { get; set; }
        public double Cluster_Min_Cell_Temp { get; set; }
        public ushort Cluster_Min_Cell_Temp_Pack { get; set; }
        public ushort Cluster_Min_Cell_Temp_Num { get; set; }

        //0x0BC:BCU遥测数据1
        public double Battery_Charge_Voltage { get; set; }
        public double Charge_Current_Limitation { get; set; }
        public double Discharge_Current_Limitation { get; set; }
        public double Battery_Discharge_Voltage { get; set; }

        //0x0BD:BCU遥测数据2
        public string BCUSoftwareVersion { get; set; }
        public string BCUHardwareVersion { get; set; }
        public string CANProtocolVersion { get; set; }
        //public double Cluster_Voltage { get; set; }
        //public double Cluster_Current { get; set; }
        //public double Max_Power_Terminal_Temperature { get; set; }

        //0x0BE:BCU遥测数据3
        public double Remaining_Total_Capacity { get; set; }
        public double Bat_Average_Temp { get; set; }
        public double Cluster_Rate_Power { get; set; }
        public double Cycles { get; set; }
        //public double Bat_Bus_Volt { get; set; }


        //0x0BF:BCU遥测数据4
        public string Bms_State { get; set; }
        public ushort Cluster_SOC { get; set; }
        public ushort Cluster_SOH { get; set; }
        public ushort Cluster_BatPack_Num { get; set; }
        //public ushort HW_Version { get; set; }

        //0x0C0:BCU系统时间
        //public string Year { get; set; }
        //public string Month { get; set; }
        //public string Day { get; set; }
        //public string Hour { get; set; }
        //public string Minute { get; set; }
        //public string Second { get; set; }
        public string BCU_SytemTime { get; set; }

        //0x0C1:模拟量与测试结果1(一般用于ate测试)
        //public double Max_Ring_Charge_Zero_Volt { get; set; }
        //public double Min_Ring_Charge_Zero_Volt { get; set; }
        //public double Max_Ring_Discharge_Zero_Volt { get; set; }
        //public double Min_Ring_Discharge_Zero_Volt { get; set; }

        //0x0C2:模拟量与测试结果2
        //public double RT1_Temperature { get; set; } // 修正拼写错误
        //public string Eeprom_Test_Result { get; set; }
        //public string Test_Result_485 { get; set; }
        //public string CAN1_Test_Result { get; set; }
        //public string CAN2_Test_Result { get; set; }
        //public string CAN3_Test_Result { get; set; }

        //0x0F3:序列号
        public string SN { get; set; }

        //0x0C7:模拟量(DSP)
        public double Inductive_Current_Sampling1 { get; set; }
        public double Inductive_Current_Sampling2 { get; set; }
        public double Inductive_Current_Sampling3 { get; set; }

        public double Inductive_Current_Sampling4 { get; set; }
        public double Discharge_High_Current_Sampling { get; set; }
        public double Charge_High_Current_Sampling { get; set; }

        public double Contactor_Voltage { get; set; }
        public double Battery_Cluster_Voltage1 { get; set; }
        public double High_Bus_Voltage { get; set; }

        public double HeatingFilm_Power_Supply_Voltage { get; set; }
        public double Battery_Sampling_Voltage2 { get; set; }

        public double Radiator_temperature1 { get; set; }
        public double Radiator_temperature2 { get; set; }
        public double Radiator_temperature3 { get; set; }
        public double Radiator_temperature4 { get; set; }

        public string PCUSoftwareVersion { get; set; }
        public string PCUHardwareVersion { get; set; }
        public string SCIProtocolVersion { get; set; }

        //public string Reset_Mode { get; set; }
        //public double Dry2_In_Status { get; set; }
        //public string Wake_Source { get; set; }

        //0x0C8:遥信数据(DSP)
        public string PCU_WorkState { get; set; }
        public string PCU_BatteryState { get; set; }

        public double Discharge_Limit_Current_Value { get; set; }
        public double Charge_Limit_Current_Value { get; set; }


        public string Fault { get; set; }
        public string Warning { get; set; }
        public string Protection { get; set; }
        public string Fault2 { get; set; }
        public string Warning2 { get; set; }
        public string Protection2 { get; set; }

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