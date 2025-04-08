using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerKit.UI.Models
{
    public class RealtimeData_BMS1500V_BMU
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
            public string Temperature { get; set; }
            public string BatteryEquilibriumState { get; set; }
            public string BatteryEquilibriumTemperature { get; set; }


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

        public class batteryEquilibriumStateData : IBatteryData { }

        public class batteryEquilibriumTemperatureData : IBatteryData { }


        public String PackID { get; set; }
        public String CreateDate { get; set; }
        //状态信息区
        public Double BatteryVolt { get; set; }                     //电池电压
        public Double LoadVolt { get; set; }                        //负载电压
        public Double BatteryCurrent { get; set; }                  //电池电流
        public Double SOC { get; set; }                             //电池剩余容量
        public Double SOH { get; set; }                             //电池健康程度
        public String RemainingCapacity { get; set; }               //剩余容量
        public String FullCapacity { get; set; }                    //满充容量
        public String BatteryStatus { get; set; }                   //电池状态
        public String BmsStatus { get; set; }                       //+BMS状态
        public String SyncFallSoc { get; set; }                     //主动均衡SOC+
        public String ActiveBalanceStatus { get; set; }              //主动均衡状态+
        public Double ChargeCurrentLimitation { get; set; }         //充电电流上限
        public Double DischargeCurrentLimitation { get; set; }      //放电电流上限
        public String CumulativeDischargeCapacity { get; set; }     //累计放电量
        public String CumulativeChargeCapacity { get; set; }        //累计充电量+
        public Double TotalChgCap { get; set; }                     //累计充电容量
        public Double TotalDsgCap { get; set; }                     //累计放电容量

        public UInt16 LOAD_VOLT_N { get; set; }                     //P-对B-电压

        //电池数据
        public UInt16 CycleTime { get; set; }                       //循环次数
        public UInt16 BatMaxCellVolt { get; set; }                  //最高单体电压
        public ushort BatMaxCellVoltNum { get; set; }               //最高单体电压编号
        public UInt16 BatMinCellVolt { get; set; }                  //最低单体电压
        public ushort BatMinCellVoltNum { get; set; }               //最低单体电压编号
        public UInt16 BatDiffCellVolt { get; set; }                 //单体电压压差
        public UInt32 CellVoltage1 { get; set; }                    //电压1
        public UInt32 CellVoltage2 { get; set; }                    //电压2
        public UInt32 CellVoltage3 { get; set; }                    //电压3
        public UInt32 CellVoltage4 { get; set; }                    //电压4
        public UInt32 CellVoltage5 { get; set; }                    //电压5
        public UInt32 CellVoltage6 { get; set; }                    //电压6
        public UInt32 CellVoltage7 { get; set; }                    //电压7
        public UInt32 CellVoltage8 { get; set; }                    //电压8
        public UInt32 CellVoltage9 { get; set; }                    //电压9
        public UInt32 CellVoltage10 { get; set; }                   //电压10
        public UInt32 CellVoltage11 { get; set; }                   //电压11
        public UInt32 CellVoltage12 { get; set; }                   //电压12
        public UInt32 CellVoltage13 { get; set; }                   //电压13
        public UInt32 CellVoltage14 { get; set; }                   //电压14
        public UInt32 CellVoltage15 { get; set; }                   //电压15
        public UInt32 CellVoltage16 { get; set; }                   //电压16
        public Double EnvTemperature { get; set; }                  //环境温度
        public Double MosTemperature { get; set; }                  //Mos温度
        public String BalanceTemperature1 { get; set; }             //均衡温度1
        public String BalanceTemperature2 { get; set; }             //均衡温度2
        public Double DcdcTemperature1 { get; set; }                //主动均衡温度1+
        public Double DcdcTemperature2 { get; set; }                //主动均衡温度2+
        public Double BatMaxCellTemp { get; set; }                  //最高单体温度
        public ushort BatMaxCellTempNum { get; set; }               //最高单体温度编号
        public Double BatMinCellTemp { get; set; }                  //最低单体温度
        public ushort BatMinCellTempNum { get; set; }               //最低单体温度编号
        public Double CellTemperature1 { get; set; }                //温度1
        public Double CellTemperature2 { get; set; }                //温度2
        public Double CellTemperature3 { get; set; }                //温度3
        public Double CellTemperature4 { get; set; }                //温度4
        public Double CellTemperature5 { get; set; }                //温度5
        public Double CellTemperature6 { get; set; }                //温度6
        public Double CellTemperature7 { get; set; }                //温度7
        public Double CellTemperature8 { get; set; }                //温度8
        public Double PowerTemperture1 { get; set; }                //功率正端子温度
        public Double PowerTemperture2 { get; set; }                //功率负端子温度
        public Double HeatCur { get; set; }                         //加热膜电流
        public Double HeatRelayVol { get; set; }                    //加热膜电压

        //public String Fault { get; set; }                           //故障状态
        //public String Warning { get; set; }                         //告警状态
        //public String Protection { get; set; }                      //保护状态
        //public String Fault2 { get; set; }                          //故障状态
        //public String Warning2 { get; set; }                        //告警状态
        //public String Protection2 { get; set; }                      //保护状态
        public String EquaState { get; set; }                       //均衡状态
        public String HeatRequest { get; set; }                     //加热请求
        public ushort ChargeEnable { get; set; }                    //允许充电
        public ushort DischargeEnable { get; set; }                 //允许放电
        public ushort BmuCutOffRequest { get; set; }                //切断继电器
        public ushort BmuPowOffRequest { get; set; }                //BMU关机
        public ushort ForceChrgRequest { get; set; }                //请求强充
        public ushort ChagreStatus { get; set; }                    //充满
        public ushort DischargeStatus { get; set; }                 //放空
        public ushort DiIO { get; set; }                            //编址输入电平
        public ushort ChargeIO { get; set; }                        //补电输入电平

        //新增帧信息
        public Double BalanceBusVoltage { get; set; }               //均衡母线电压
        public Double BalanceCurrent { get; set; }                  //均衡电流
        public Double ActiveBalanceMaxCellVolt { get; set; }        //主动均衡最大单体电压
        public Double BatAverageTemp { get; set; }                  //电芯平均温度
        public Double ActiveBalanceCellSoc { get; set; }            //主动均衡SOC
        public String ActiveBalanceAccCap { get; set; }             //主动均衡累计容量
        public String ActiveBalanceRemainCap { get; set; }          //主动均衡剩余容量
        public String BMUSaftwareVersion { get; set; }              //BMU软件版本
        public String BMUCanVersion { get; set; }                   //BMU-CAN版本
        public String BatNominalCapacity { get; set; }              //标定容量
        public String RegisterName { get; set; }                    //供应商厂家名称
        public String BatType { get; set; }                         //锂电池类型
        public String ManufacturerName { get; set; }                //厂家名称
        public String ResetMode { get; set; }                       //复位方式
        public String AuxVolt { get; set; }                         //辅源电压
        public String ChgCurOffsetVolt { get; set; }                //充电电流偏压
        public String DsgCurOffsetVolt { get; set; }                //放电电流偏压
        public String BMSSoftwareVersion { get; set; }              //BMS软件版本
        public String BMSHardwareVersion { get; set; }              //BMS硬件版本


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

        }

        /// <summary>
        /// 故障信息
        /// </summary>
        public String Fault { get; set; }
        /// <summary>
        /// 告警信息
        /// </summary>
        public String Warning { get; set; }
        /// <summary>
        /// 保护信息
        /// </summary>
        public String Protection { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public String Prompt { get; set; }
        public String GetHeader()
        {
            return @"记录时间,电池ID,故障信息,告警信息,保护信息,提示信息,电池状态,BMS状态,允许充电,允许放电,切断继电器,BMU关机,请求强充,充满,放空,编址输入电平,补电输入电平,加热请求,电池电压,负载电压," +
                   @"电池电流,电池剩余容量(SOC),电池健康程度(SOH),剩余容量,满充容量,充电电流上限,放电电流上限,累计放电量,累计充电量,累计放电容量,累计充电容量,最高单体电压编号,最高单体电压,最低单体电压编号," +
                   @"最低单体电压,单体电压差,环境温度,Mos温度,功率正端子温度,功率负端子温度,主动均衡温度1,主动均衡温度2,最高单体温度编号,最高单体温度,最低单体温度编号,最低单体温度,均衡母线电压," +
                   @"均衡电流,主动均衡最大单体电压,电芯平均温度,主动均衡查表SOC,主动均衡累计容量,主动均衡剩余容量,辅源电压,充电电流偏压,放电电流偏压,复位方式,放空同步下降SOC,主动均衡状态,标定容量,供应商名称,锂电池类型";
        }

        public string GetValue()
        {
            return $"{this.CreateDate},{this.PackID},{this.Fault},{this.Warning},{this.Protection},{this.Prompt},{this.BatteryStatus},{this.BmsStatus},{this.ChargeEnable},{this.DischargeEnable},{this.BmuCutOffRequest},{this.BmuPowOffRequest},{this.ForceChrgRequest},{this.ChagreStatus},{this.DischargeStatus},{this.DiIO},{this.ChargeIO},{this.HeatRequest},{this.BatteryVolt},{this.LoadVolt}," +
                   $"{this.BatteryCurrent},{this.SOC}, {this.SOH},{this.RemainingCapacity},{this.FullCapacity},{this.ChargeCurrentLimitation},{this.DischargeCurrentLimitation},{this.CumulativeDischargeCapacity},{this.CumulativeChargeCapacity},{this.TotalDsgCap},{this.TotalChgCap},{this.BatMaxCellVoltNum},{this.BatMaxCellVolt},{this.BatMinCellVoltNum}," +
                   $"{this.BatMinCellVolt},{this.BatDiffCellVolt},{this.EnvTemperature},{this.MosTemperature},{this.PowerTemperture1},{this.PowerTemperture2},{this.DcdcTemperature1},{this.DcdcTemperature2},{this.BatMaxCellTempNum},{this.BatMaxCellTemp},{this.BatMinCellTempNum},{this.BatMinCellTemp},{this.BalanceBusVoltage}," +
                   $"{this.BalanceCurrent},{this.ActiveBalanceMaxCellVolt},{this.BatAverageTemp},{this.ActiveBalanceCellSoc},{this.ActiveBalanceAccCap},{this.ActiveBalanceRemainCap},{this.AuxVolt},{this.ChgCurOffsetVolt},{this.DsgCurOffsetVolt},{this.ResetMode},{this.SyncFallSoc},{this.ActiveBalanceStatus},{this.BatNominalCapacity},{this.RegisterName},{this.BatType}";
        }
    }
}
