using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace PowerKit.UI.Models
{
    public class RealtimeData_BMS1500V_BCU
    {
        public abstract class IBatteryData
        {
            public string DataName { get; set; }
            public string IndexName { get; set; }
            public string SectionNumber { get; set; }
            public string CellNumber { get; set; }
            public string Voltage { get; set; }
            public string SOC { get; set; }
            public string SOH { get; set; }
            public string PoleTemperature { get; set; }
            public string Temperature { get; set; }
            public string SupplyVoltage { get; set; }
            public string ModuleTotalVoltage { get; set; }
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
        }


        public class batteryVoltageData : IBatteryData { }

        public class batterySocData : IBatteryData { }

        public class batteryTemperatureData : IBatteryData { }

        public class batterySohData : IBatteryData { }

        public class poleTemperatureData : IBatteryData { }

        public class supplyVoltageData : IBatteryData { }

        public class moduleTotalVoltageData : IBatteryData { }


        
        /// <summary>
        /// 报警信息表格数据
        /// </summary>
        public class AlarmMessageData : ObservableObject
        {

            private string _alarmNumber;
            private string _isEnd;
            private string _alarmStartTime;
            private string _alarmStopTime;
            private string _alarmLevel;
            private string _alarmMessage;
            private string _id;
            private string _batterySectionNumber;

            /// <summary>
            /// 报警序号
            /// </summary>
            public string AlarmNumber
            {
                get { return _alarmNumber; }
                set
                {
                    if (_alarmNumber != value)
                    {
                        _alarmNumber = value;
                        OnPropertyChanged(nameof(AlarmNumber));
                    }
                }
            }

            /// <summary>
            /// 是否结束
            /// </summary>
            public string isEnd
            {
                get { return _isEnd; }
                set
                {
                    if (_isEnd != value)
                    {
                        _isEnd = value;
                        OnPropertyChanged(nameof(isEnd));
                    }
                }
            }

            /// <summary>
            /// 报警开始时间
            /// </summary>
            public string AlarmStartTime
            {
                get { return _alarmStartTime; }
                set
                {
                    if (_alarmStartTime != value)
                    {
                        _alarmStartTime = value;
                        OnPropertyChanged(nameof(AlarmStartTime));
                    }
                }
            }

            /// <summary>
            /// 报警结束时间
            /// </summary>
            public string AlarmStopTime
            {
                get { return _alarmStopTime; }
                set
                {
                    if (_alarmStopTime != value)
                    {
                        _alarmStopTime = value;
                        OnPropertyChanged(nameof(AlarmStopTime));
                    }
                }
            }

            /// <summary>
            /// 报警等级
            /// </summary>
            public string AlarmLevel
            {
                get { return _alarmLevel; }
                set
                {
                    if (_alarmLevel != value)
                    {
                        _alarmLevel = value;
                        OnPropertyChanged(nameof(AlarmLevel));
                    }
                }
            }

            /// <summary>
            /// 报警信息描述
            /// </summary>
            public string AlarmMessage
            {
                get { return _alarmMessage; }
                set
                {
                    if (_alarmMessage != value)
                    {
                        _alarmMessage = value;
                        OnPropertyChanged(nameof(AlarmMessage));
                    }
                }
            }

            /// <summary>
            /// 设备ID
            /// </summary>
            public string ID
            {
                get { return _id; }
                set
                {
                    if (_id != value)
                    {
                        _id = value;
                        OnPropertyChanged(nameof(ID));
                    }
                }
            }

            /// <summary>
            /// 电池节号
            /// </summary>
            public string BatterySectionNumber
            {
                get { return _batterySectionNumber; }
                set
                {
                    if (_batterySectionNumber != value)
                    {
                        _batterySectionNumber = value;
                        OnPropertyChanged(nameof(BatterySectionNumber));
                    }
                }
            }

        }

        public String BCU_ID { get; set; }
        public String CreateDate { get; set; }
        /// <summary>
        /// 轻微报警信息
        /// </summary>
        public String MinorAlarm { get; set; }
        /// <summary>
        /// 一般报警信息
        /// </summary>
        public String GeneralAlarm { get; set; }
        /// <summary>
        /// 严重报警信息
        /// </summary>
        public String SevereAlarm { get; set; }
        /// <summary>
        /// 设备硬件故障报警信息
        /// </summary>
        public String EquipmentHardwareFailureAlarm { get; set; }



        /// <summary>
        /// 模块温度 分辨率 1℃/bit
        /// </summary>
        public string ModuleTemperature { get; set; }

        /// <summary>
        /// 组端电压 分辨率为 0.1V/bit
        /// </summary>
        public string GroupTerminalVoltage { get; set; }

        /// <summary>
        /// 组端SOC 分辨率 1%/bit
        /// </summary>
        public string GroupEndSOC { get; set; }

        /// <summary>
        /// 供电电压 分辨率 0.1V/bit
        /// </summary>
        public string SupplyVoltage { get; set; }

        /// <summary>
        /// 预充电压 分辨率 0.1V/bit
        /// </summary>
        public string PreChargeVoltage { get; set; }

        /// <summary>
        /// 组端温度 1 
        /// </summary>
        public string GroupEndTemperature_1 { get; set; }

        /// <summary>
        /// 组端温度 2
        /// </summary>
        public string GroupEndTemperature_2 { get; set; }

        /// <summary>
        /// 组端温度 3
        /// </summary>
        public string GroupEndTemperature_3 { get; set; }

        /// <summary>
        /// 组端温度 4
        /// </summary>
        public string GroupEndTemperature_4 { get; set; }

        /// <summary>
        /// 组端电流1 分辨率 0.1A/bit 偏移量：-1600
        /// </summary>
        public string GroupEndCurrent_1 { get; set; }

        /// <summary>
        /// 组端电流2 分辨率 0.1A/bit 偏移量：-1600
        /// </summary>
        public string GroupEndCurrent_2 { get; set; }

        /// <summary>
        /// 组端电流3 分辨率 0.1A/bit 偏移量：-1600
        /// </summary>
        public string GroupEndCurrent_3 { get; set; }

        /// <summary>
        /// 绝缘电阻R+ 
        /// </summary>
        public string InsulationResistance_R_Positive { get; set; }

        /// <summary>
        /// 绝缘电阻R-
        /// </summary>
        public string InsulationResistance_R_Negative { get; set; }


        /// <summary>
        /// 电压最大值（前一）    
        /// </summary>
        public string BeforeTheMaximumValue_Voltage { get; set; }

        /// <summary>
        /// 电压最大值（前二）    
        /// </summary>
        public string TopTwoMaximumValue_Voltage { get; set; }

        /// <summary>
        /// 电压最大值（前三）    
        /// </summary>  
        public string TopThreeMaximumValue_Voltage { get; set; }

        /// <summary>
        /// 电压最小值（前一）     
        /// </summary>  
        public string BeforeTheMinimumValue_Voltage { get; set; }


        /// <summary>
        /// 电压最小值（前二）     
        /// </summary>  
        public string TopTwoMinimumValue_Voltage { get; set; }

        /// <summary>
        /// 电压最小值（前三）     
        /// </summary>  
        public string TopThreeMinimumValue_Voltage { get; set; }

        /// <summary>
        /// 电压平均值     
        /// </summary>  
        public string AverageValue_Voltage { get; set; }

        /// <summary>
        /// 电压极差值     
        /// </summary>  
        public string RangeValue_Voltage { get; set; }

        /// <summary>
        /// 温度最大值（前一）    
        /// </summary>
        public string BeforeTheMaximumValue_Temperature { get; set; }

        /// <summary>
        /// 温度最大值（前二）    
        /// </summary>
        public string TopTwoMaximumValue_Temperature { get; set; }

        /// <summary>
        /// 温度最大值（前三）    
        /// </summary>
        public string TopThreeMaximumValue_Temperature { get; set; }

        /// <summary>
        /// 温度最小值（前一）     
        /// </summary>  
        public string BeforeTheMinimumValue_Temperature { get; set; }

        /// <summary>
        /// 温度最小值（前二）     
        /// </summary>  
        public string TopTwoMinimumValue_Temperature { get; set; }

        /// <summary>
        /// 温度最小值（前三）     
        /// </summary>  
        public string TopThreeMinimumValue_Temperature { get; set; }

        /// <summary>
        /// 温度平均值     
        /// </summary>  
        public string AverageValue_Temperature { get; set; }

        /// <summary>
        /// 温度极差值     
        /// </summary>  
        public string RangeValue_Temperature { get; set; }

        /// <summary>
        /// SOC最大值（前一）     
        /// </summary>  
        public string BeforeTheMaximumValue_SOC { get; set; }

        /// <summary>
        /// SOC最大值（前二）     
        /// </summary>  
        public string TopTwoMaximumValue_SOC { get; set; }

        /// <summary>
        /// SOC最大值（前三）     
        /// </summary>  
        public string TopThreeMaximumValue_SOC { get; set; }

        /// <summary>
        /// SOC最小值（前一）     
        /// </summary>  
        public string BeforeTheMinimumValue_SOC { get; set; }

        /// <summary>
        /// SOC最小值（前二）     
        /// </summary>  
        public string TopTwoMinimumValue_SOC { get; set; }

        /// <summary>
        /// SOC最小值（前三）     
        /// </summary>  
        public string TopThreeMinimumValue_SOC { get; set; }

        /// <summary>
        /// SOC平均值     
        /// </summary>  
        public string AverageValue_SOC { get; set; }

        /// <summary>
        /// SOC极差值     
        /// </summary>  
        public string RangeValue_SOC { get; set; }

        /// <summary>
        /// SOH最大值（前一）     
        /// </summary>  
        public string BeforeTheMaximumValue_SOH { get; set; }

        /// <summary>
        /// SOH最大值（前二）     
        /// </summary>  
        public string TopTwoMaximumValue_SOH { get; set; }

        /// <summary>
        /// SOH最大值（前三）     
        /// </summary>  
        public string TopThreeMaximumValue_SOH { get; set; }

        /// <summary>
        /// SOH最小值（前一）     
        /// </summary>  
        public string BeforeTheMinimumValue_SOH { get; set; }

        /// <summary>
        /// SOH最小值（前二）     
        /// </summary>  
        public string TopTwoMinimumValue_SOH { get; set; }

        /// <summary>
        /// SOH最小值（前三）     
        /// </summary>  
        public string TopThreeMinimumValue_SOH { get; set; }

        /// <summary>
        /// SOH平均值     
        /// </summary>  
        public string AverageValue_SOH { get; set; }

        /// <summary>
        /// SOH极差值     
        /// </summary>  
        public string RangeValue_SOH { get; set; }

        /// <summary>
        /// 电压最大值节号       
        /// </summary>  
        public string VoltageMaxNum { get; set; }

        /// <summary>
        /// 电压最小值节号       
        /// </summary>  
        public string VoltageMinNum { get; set; }

        /// <summary>
        /// 温度最大值节号
        /// </summary>  
        public string TemperatureMaxNum { get; set; }

        /// <summary>
        /// 温度最小值节号
        /// </summary>  
        public string TemperatureMinNum { get; set; }

        /// <summary>
        /// SOC最大值节号       
        /// </summary>  
        public string SOCMaxNum { get; set; }

        /// <summary>
        /// SOC最小值节号       
        /// </summary>  
        public string SOCMinNum { get; set; }

        /// <summary>
        /// SOH最大值节号       
        /// </summary>  
        public string SOHMaxNum { get; set; }

        /// <summary>
        /// SOH最小值节号       
        /// </summary>  
        public string SOHMinNum { get; set; }

        /// <summary>
        /// 模块总数 N ( 0 ~ 60 )个
        /// </summary>
        public string ModuleNumber { get; set; }

        /// <summary>
        /// 电池节数 N ( 0 ~ 400 )节
        /// </summary>      
        public string BatteryCellsNumber { get; set; }

        /// <summary>
        /// 温度总数 ( 0 ~ 400 )节
        /// </summary>
        public string TemperatureNumber { get; set; }

        /// <summary>
        /// 从控采集极柱个数
        /// </summary>
        public string PoleNumber { get; set; }

        public String GetHeader()
        {
            return @"记录时间,BCU地址,轻微报警信息,一般报警信息,严重报警信息,设备硬件故障信息," +
                   @"组端电压(V),组端电流1(A),组端电流2(A),组端电流3(A),预充电压(V),绝缘电阻R+(KΩ)," +
                   @"绝缘电阻R-(KΩ),模块温度(℃),供电电压(V),组端SOC(%),组端温度1(℃),组端温度2(℃)," +
                   @"组端温度3(℃),组端温度4(℃),电压最大值前一(V),电压最大值前二(V),电压最大值前三(V)," +
                   @"电压最小值前一(V),电压最小值前二(V),电压最小值前三(V),电压平均值(V),电压极差值(V)," +
                   @"电压最大值节号,电压最小值节号,温度最大值前一(℃),温度最大值前二(℃),温度最大值前三(℃)," +
                   @"温度最小值前一(℃),温度最小值前二(℃),温度最小值前三(℃),温度平均值(℃),温度极差值(℃)," +
                   @"温度最大值节号,温度最小值节号,SOC最大值前一(%),SOC最大值前二(%),SOC最大值前三(%)," +
                   @"SOC最小值前一(%),SOC最小值前二(%),SOC最小值前三(%),SOC平均值(%),SOC极差值(%), SOC最大值节号," +
                   @"SOC最小值节号,SOH最大值前一(%),SOH最大值前二(%),SOH最大值前三(%),SOH最小值前一(%),SOH最小值前二(%)," +
                   @"SOH最小值前三(%),SOH平均值(%),SOH极差值(%),SOH最大值节号,SOH最小值节号,模块总数,电池节数,温度总数,极柱个数";
        }

        public string GetValue()
        {
            return $"{this.CreateDate},{this.BCU_ID},{this.MinorAlarm},{this.GeneralAlarm},{this.SevereAlarm},{this.EquipmentHardwareFailureAlarm},{this.GroupTerminalVoltage},{this.GroupEndCurrent_1},{this.GroupEndCurrent_2}," +
                   $"{this.GroupEndCurrent_3},{this.PreChargeVoltage},{this.InsulationResistance_R_Positive},{this.InsulationResistance_R_Negative}," +
                   $"{this.ModuleTemperature},{this.SupplyVoltage},{this.GroupEndSOC},{this.GroupEndTemperature_1},{this.GroupEndTemperature_2},{this.GroupEndTemperature_3}," +
                   $"{this.GroupEndTemperature_4},{this.BeforeTheMaximumValue_Voltage},{this.TopTwoMaximumValue_Voltage},{this.TopThreeMaximumValue_Voltage}," +
                   $"{this.BeforeTheMinimumValue_Voltage},{this.TopTwoMinimumValue_Voltage},{this.TopThreeMinimumValue_Voltage},{this.AverageValue_Voltage},{this.RangeValue_Voltage}," +
                   $"{this.VoltageMaxNum},{this.VoltageMinNum},{this.BeforeTheMaximumValue_Temperature},{this.TopTwoMaximumValue_Temperature},{this.TopThreeMaximumValue_Temperature}," +
                   $"{this.BeforeTheMinimumValue_Temperature},{this.TopTwoMinimumValue_Temperature},{this.TopThreeMinimumValue_Temperature},{this.AverageValue_Temperature},{this.RangeValue_Temperature}," +
                   $"{this.TemperatureMaxNum},{this.TemperatureMinNum},{this.BeforeTheMaximumValue_SOC},{this.TopTwoMaximumValue_SOC},{this.TopThreeMaximumValue_SOC},{this.BeforeTheMinimumValue_SOC}," +
                   $"{this.TopTwoMinimumValue_SOC},{this.TopThreeMinimumValue_SOC},{this.AverageValue_SOC},{this.RangeValue_SOC},{this.SOCMaxNum},{this.SOCMinNum},{this.BeforeTheMaximumValue_SOH}," +
                   $"{this.TopTwoMaximumValue_SOH},{this.TopThreeMaximumValue_SOH},{this.BeforeTheMinimumValue_SOH},{this.TopTwoMinimumValue_SOH},{this.TopThreeMinimumValue_SOH},{this.AverageValue_SOH}," +
                   $"{this.RangeValue_SOH},{this.SOHMaxNum},{this.SOHMinNum},{this.ModuleNumber},{this.BatteryCellsNumber},{this.TemperatureNumber},{this.PoleNumber}";
        }
    }
}
