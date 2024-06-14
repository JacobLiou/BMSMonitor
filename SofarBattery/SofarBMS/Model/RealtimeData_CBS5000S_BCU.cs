using Microsoft.Office.Interop.Excel;
using MySqlX.XDevAPI.Common;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NPOI.POIFS.Crypt.CryptoFunctions;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SofarBMS.Model
{
    internal class RealtimeData_CBS5000S_BCU
    {
        public String PackID { get; set; }
        public String CreateDate { get { return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"); } }

        //0x0B6:BCU遥信数据上报1
        public Double Di_Status_Get { get; set; }                //BCU的di状态
        public Double Balance_Chg_Status { get; set; }           //电池包充均衡状态
        public Double Balance_Dchg_Status { get; set; }          //电池包放均衡状态
        public Double Relay_Status { get; set; }                 //继电器状态
        public Double Other_Dev_Staus { get; set; }              //器件的状态

        //0x0B7:BCU遥信数据上报2
        public Double Power_Terminal_Temperature1 { get; set; }//功率端子温度1
        public Double Power_Terminal_Temperature2 { get; set; }//功率端子温度1
        public Double Power_Terminal_Temperature3 { get; set; }//功率端子温度1
        public Double Power_Terminal_Temperature4 { get; set; }//功率端子温度1

        //0x0B8:BCU遥信数据上报3
        public Double Insulation_Resistance { get; set; }//高压绝缘阻抗
        public Double Auxiliary_Power_Supply_Voltage { get; set; }//辅助电源电压
        public Double Fuse_Voltage { get; set; }//保险丝后电压
        public Double Power_Voltage { get; set; }//Power_voltage

        //0x0B9:BCU遥信数据上报4
        public Double Load_Voltage { get; set; }//负载侧电压

        //0x0BA:BCU遥信数据上报5
        public UInt16 Bat_Max_Cell_Volt { get; set; }//簇最高单体电压
        public ushort Bat_Max_Cell_VoltPack { get; set; }//簇最高单体电压所在pack
        public ushort Bat_Max_Cell_VoltNum { get; set; }//簇最高单体电压所在pack的第几个位置
        public UInt16 Bat_Min_Cell_Volt { get; set; } //簇最低单体电压
        public ushort Bat_Min_Cell_Volt_Pack { get; set; }//簇最低单体电压所在pack
        public ushort Bat_Min_Cell_Volt_Num { get; set; }//簇最低单体电压所在pack的第几个位置     

        //0x0BB:BCU遥信数据上报6
        public ushort Bat_Max_Cell_Temp { get; set; }//簇最高单体温度
        public ushort Bat_Max_Cell_Temp_Pack { get; set; }//簇最高单体温度所在pack
        public ushort Bat_Max_Cell_Temp_Num { get; set; }//簇最高单体温度所在pack的第几个位置
        public ushort Bat_Min_Cell_Temp { get; set; }//簇最低单体温度
        public ushort Bat_Min_Cell_Temp_Pack { get; set; }//簇最低单体温度所在pack
        public ushort Bat_Min_Cell_Temp_Num { get; set; }//簇最低单体温度所在pack的第几个位置

        //0x0BC:BCU遥测数据1
        public Double Battery_Charge_Voltage { get; set; }//电池簇的充电电压
        public Double Charge_Current_Limitation { get; set; }//电池簇充电电流上限
        public Double Discharge_Current_Limitation { get; set; }//电池簇放电电流上限
        public Double Battery_Discharge_Voltage { get; set; }//电池簇的放电截止电压

        //0x0BD:BCU遥测数据2
        public Double Cluster_Voltage { get; set; }//电池簇电压
        public Double Cluster_Current { get; set; }//电池簇电流
        public Double Max_Power_Terminal_Temperature { get; set; }//最高功率端子温度
        public Double Cycles { get; set; }//电池充放电循环次数

        //0x0BE:BCU遥测数据3
        public Double Remaining_Total_Capacity { get; set; }//实时剩余总容量
        public Double Bat_Temp { get; set; }//电池平均温度
        public Double Cluster_Rate_Power { get; set; }//额定功率
        public Double Bat_Bus_Volt { get; set; }//电池母线电压

        //0x0BF:BCU遥测数据4
        public string Bms_State { get; set; }//bcu上送的bms状态
        public ushort Cluster_SOC { get; set; }//电池簇SOC
        public ushort Cluster_SOH { get; set; }//电池簇SOH
        public ushort Pack_Num { get; set; }//簇内电池包数量
        public ushort HW_Version { get; set; }//硬件版本号

        //0x0C0:BCU系统时间
        public String Year { get; set; }//年
        public String Month { get; set; }//月
        public String Day { get; set; }//日
        public String Hour { get; set; }//时
        public String Minute { get; set; }//分
        public String Second { get; set; }//秒

        //0x0C1:模拟量与测试结果1(一般用于ate测试)
        public Double Max_Ring_Charge_Zero_Volt { get; set; }//充电大电流偏压
        public Double In_Ring_Charge_Zero_Volt { get; set; }//充电小电流偏压
        public Double Ax_Ring_Discharge_Zero_Volt { get; set; }//放电大电流偏压
        public Double In_Ring_Discharge_Zero_Volt { get; set; }//放电小电流偏压

        //0x0C2:模拟量与测试结果2(一般用于ate测试)
        public Double RT1_Tempture { get; set; }//RT1环境温度
        public string Eeprom_Test_Result { get; set; }//eeprom测试结果
        public string Test_Result_485 { get; set; }//485测试结果
        public string CAN1_Test_Result { get; set; }//CAN1测试结果
        public string CAN2_Test_Result { get; set; }//CAN2测试结果
        public string CAN3_Test_Result { get; set; }//CAN3测试结果
        public string Reset_Mode { get; set; }//复位方式
        public Double Dry2_In_Status { get; set; }//干接点2输入
        public string Wake_Source { get; set; }//唤醒原因

        //public Double BatteryVolt { get; set; }                     //电池电压
        //public Double LoadVolt { get; set; }                        //负载电压
        //public Double BatteryCurrent { get; set; }                  //电池电流
        //public Double SOC { get; set; }                             //电池剩余容量
        //public Double SOH { get; set; }                             //电池健康程度
        //public String RemainingCapacity { get; set; }               //剩余容量
        //public String FullCapacity { get; set; }                    //满充容量
        //public String BatteryStatus { get; set; }                   //电池状态
        //public String BmsStatus { get; set; }                       //+BMS状态
        //public String SyncFallSoc { get; set; }                     //主动均衡SOC+
        //public String ActiveBalanceStatus { get; set; }              //主动均衡状态+
        //public Double ChargeCurrentLimitation { get; set; }         //充电电流上限
        //public Double DischargeCurrentLimitation { get; set; }      //放电电流上限
        //public String CumulativeDischargeCapacity { get; set; }     //累计放电量
        //public String CumulativeChargeCapacity { get; set; }        //累计充电量+
        //public Double TotalChgCap { get; set; }                     //累计充电容量
        //public Double TotalDsgCap { get; set; }                     //累计放电容量

        //public UInt16 LOAD_VOLT_N { get; set; }                       //P-对B-电压

        ////电池数据
        //public UInt16 CycleTIme { get; set; }                       //循环次数
        //public UInt16 BatMaxCellVolt { get; set; }                  //最高单体电压
        //public ushort BatMaxCellVoltNum { get; set; }               //最高单体电压编号
        //public UInt16 BatMinCellVolt { get; set; }                  //最低单体电压
        //public ushort BatMinCellVoltNum { get; set; }               //最低单体电压编号
        //public UInt16 BatDiffCellVolt { get; set; }                 //单体电压压差
        //public UInt32 CellVoltage1 { get; set; }                    //电压1
        //public UInt32 CellVoltage2 { get; set; }                    //电压2
        //public UInt32 CellVoltage3 { get; set; }                    //电压3
        //public UInt32 CellVoltage4 { get; set; }                    //电压4
        //public UInt32 CellVoltage5 { get; set; }                    //电压5
        //public UInt32 CellVoltage6 { get; set; }                    //电压6
        //public UInt32 CellVoltage7 { get; set; }                    //电压7
        //public UInt32 CellVoltage8 { get; set; }                    //电压8
        //public UInt32 CellVoltage9 { get; set; }                    //电压9
        //public UInt32 CellVoltage10 { get; set; }                   //电压10
        //public UInt32 CellVoltage11 { get; set; }                   //电压11
        //public UInt32 CellVoltage12 { get; set; }                   //电压12
        //public UInt32 CellVoltage13 { get; set; }                   //电压13
        //public UInt32 CellVoltage14 { get; set; }                   //电压14
        //public UInt32 CellVoltage15 { get; set; }                   //电压15
        //public UInt32 CellVoltage16 { get; set; }                   //电压16
        //public Double EnvTemperature { get; set; }                  //环境温度
        //public Double MosTemperature { get; set; }                  //Mos温度
        //public String BalanceTemperature1 { get; set; }             //均衡温度1
        //public String BalanceTemperature2 { get; set; }             //均衡温度2
        //public String DcdcTemperature1 { get; set; }                //主动均衡温度1+
        //public String DcdcTemperature2 { get; set; }                //主动均衡温度2+
        //public Double BatMaxCellTemp { get; set; }                  //最高单体温度
        //public ushort BatMaxCellTempNum { get; set; }               //最高单体温度编号
        //public Double BatMinCellTemp { get; set; }                  //最低单体温度
        //public ushort BatMinCellTempNum { get; set; }               //最低单体温度编号
        //public Double CellTemperature1 { get; set; }                //温度1
        //public Double CellTemperature2 { get; set; }                //温度2
        //public Double CellTemperature3 { get; set; }                //温度3
        //public Double CellTemperature4 { get; set; }                //温度4
        //public Double CellTemperature5 { get; set; }                //温度5
        //public Double CellTemperature6 { get; set; }                //温度6
        //public Double CellTemperature7 { get; set; }                //温度7
        //public Double CellTemperature8 { get; set; }                //温度8
        //public Double PowerTemperture1 { get; set; }                //功率温度1
        //public Double PowerTemperture2 { get; set; }                //功率温度2
        public String Fault { get; set; }                           //故障状态
        public String Warning { get; set; }                         //告警状态
        public String Protection { get; set; }                      //保护状态
        public String Fault2 { get; set; }                           //故障状态
        public String Warning2 { get; set; }                         //告警状态
        public String Protection2 { get; set; }                      //保护状态

        ////public ushort ChargeMosEnable { get; set; }                 //充电MOS
        ////public ushort DischargeMosEnable { get; set; }              //放电MOS
        ////public ushort PrechgMosEnable { get; set; }                 //预充MOS
        ////public ushort StopChgEnable { get; set; }                   //充电急停
        ////public ushort HeatEnable { get; set; }                      //加热MOS
        //public String EquaState { get; set; }                       //均衡状态
        //public String HeatRequest { get; set; }                     //加热请求
        //public ushort ChargeEnable { get; set; }                    //允许充电
        //public ushort DischargeEnable { get; set; }                 //允许放电
        //public ushort BmuCutOffRequest { get; set; }                //切断继电器
        //public ushort BmuPowOffRequest { get; set; }                //BMU关机
        //public ushort ForceChrgRequest { get; set; }                //请求强充
        //public ushort ChagreStatus { get; set; }                    //充满
        //public ushort DischargeStatus { get; set; }                 //放空
        //public ushort DiIO { get; set; }                            //编址输入电平
        //public ushort ChargeIO { get; set; }                        //补电输入电平

        ////新增帧信息
        //public Double BalanceBusVoltage { get; set; }               //均衡母线电压
        //public Double BalanceCurrent { get; set; }                  //均衡电流
        //public Double ActiveBalanceMaxCellVolt { get; set; }        //主动均衡最大单体电压
        //public Double BatAverageTemp { get; set; }                  //电芯平均温度
        //public Double ActiveBalanceCellSoc { get; set; }            //主动均衡SOC
        //public String ActiveBalanceAccCap { get; set; }             //主动均衡累计容量
        //public String ActiveBalanceRemainCap { get; set; }          //主动均衡剩余容量
        //public String BMUSaftwareVersion { get; set; }              //BMU软件版本
        //public String BMUCanVersion { get; set; }                   //BMU-CAN版本
        //public String BatNominalCapacity { get; set; }              //标定容量
        //public String RegisterName { get; set; }                    //供应商厂家名称
        //public String BatType { get; set; }                         //锂电池类型
        //public String ManufacturerName { get; set; }                //厂家名称
        //public String ResetMode { get; set; }                       //复位方式
        //public String AuxVolt { get; set; }                         //辅源电压
        //public String ChgCurOffsetVolt { get; set; }                //充电电流偏压
        //public String DsgCurOffsetVolt { get; set; }                //放电电流偏压
        //public String BMSSoftwareVersion { get; set; }              //BMS软件版本
        //public String BMSHardwareVersion { get; set; }              //BMS硬件版本

        public String GetHeader()
        {
            return @"记录时间,电池ID,故障信息1,告警信息1,保护信息1,故障信息2,告警信息2,保护信息2,电池状态,BMS状态,允许充电,允许放电,切断继电器,BMU关机,请求强充,充满,放空,编址输入电平,补电输入电平,加热请求,电池电压,负载电压,电池电流,电池剩余容量(SOC),电池健康程度(SOH),剩余容量,满充容量,充电电流上限,放电电流上限,累计放电量,累计充电容量,累计放电容量,累计充电容量,最高单体电压编号,最高单体电压,最低单体电压编号,最低单体电压,单体电压差,电压1,电压2,电压3,电压4,电压5,电压6,电压7,电压8,电压9,电压10,电压11,电压12,电压13,电压14,电压15,电压16,环境温度,Mos温度,被动均衡温度1,被动均衡温度2,主动均衡温度1,主动均衡温度2,最高单体温度编号,最高单体温度,最低单体温度编号,最低单体温度,温度1,温度2,温度3,温度4,温度5,温度6,温度7,温度8,均衡状态,均衡母线电压,均衡电流,主动均衡最大单体电压,电芯平均温度,主动均衡查表SOC,主动均衡累计容量,主动均衡剩余容量,辅源电压,充电电流偏压,放电电流偏压,复位方式,放空同步下降SOC,主动均衡状态,标定容量,供应商名称,锂电池类型";
        }

        public string GetValue()
        {
            return $"{this.CreateDate},{this.PackID},{this.Fault},{this.Warning},{this.Protection},{this.Fault2},{this.Warning2},{this.Protection2}";
            //return $"{this.CreateDate},{this.PackID},{this.Fault},{this.Warning},{this.Protection},{this.Fault2},{this.Warning2},{this.Protection2}," +
            //    $"{this.BatteryStatus},{this.BmsStatus},{this.ChargeEnable},{this.DischargeEnable},{this.BmuCutOffRequest},{this.BmuPowOffRequest}," +
            //    $"{this.ForceChrgRequest},{this.ChagreStatus},{this.DischargeStatus},{this.DiIO},{this.ChargeIO},{this.HeatRequest},{this.BatteryVolt}," +
            //    $"{this.LoadVolt},{this.BatteryCurrent},{this.SOC}, {this.SOH},{this.RemainingCapacity},{this.FullCapacity},{this.ChargeCurrentLimitation}," +
            //    $"{this.DischargeCurrentLimitation},{this.CumulativeDischargeCapacity},{this.CumulativeChargeCapacity},{this.TotalChgCap},{this.TotalDsgCap}," +
            //    $"{this.BatMaxCellVoltNum},{this.BatMaxCellVolt},{this.BatMinCellVoltNum},{this.BatMinCellVolt},{this.BatDiffCellVolt},{this.CellVoltage1}," +
            //    $"{this.CellVoltage2},{this.CellVoltage3},{this.CellVoltage4},{this.CellVoltage5},{this.CellVoltage6},{this.CellVoltage7},{this.CellVoltage8}," +
            //    $"{this.CellVoltage9},{this.CellVoltage10},{this.CellVoltage11},{this.CellVoltage12},{this.CellVoltage13},{this.CellVoltage14},{this.CellVoltage15}," +
            //    $"{this.CellVoltage16},{this.EnvTemperature},{this.MosTemperature},{this.BalanceTemperature1},{this.BalanceTemperature2},{this.DcdcTemperature1}," +
            //    $"{this.DcdcTemperature2},{this.BatMaxCellTempNum},{this.BatMaxCellTemp},{this.BatMinCellTempNum},{this.BatMinCellTemp},{this.CellTemperature1}, " +
            //    $"{this.CellTemperature2}, {this.CellTemperature3}, {this.CellTemperature4},{this.CellTemperature5}, {this.CellTemperature6}, {this.CellTemperature7}, " +
            //    $"{this.CellTemperature8}, {this.EquaState},{this.BalanceBusVoltage},{this.BalanceCurrent},{this.ActiveBalanceMaxCellVolt},{this.BatAverageTemp}," +
            //    $"{this.ActiveBalanceCellSoc},{this.ActiveBalanceAccCap},{this.ActiveBalanceRemainCap},{this.AuxVolt},{this.ChgCurOffsetVolt},{this.DsgCurOffsetVolt}," +
            //    $"{this.ResetMode},{this.SyncFallSoc},{this.ActiveBalanceStatus},{this.BatNominalCapacity},{this.RegisterName},{this.BatType}";
        }
    }
}