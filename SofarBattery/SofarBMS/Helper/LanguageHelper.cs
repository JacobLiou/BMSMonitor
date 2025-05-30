using System.Data;
using System.Diagnostics;

namespace SofarBMS.Helper
{
    /// <summary>
    /// 语言翻译类
    /// </summary>
    public class LanguageHelper
    {
        
        /// <summary>
        /// 当前语言：1中文 2英文 3意大利语
        /// </summary>
        private static int languageIndex = 1;
        public static int LanaguageIndex
        {
            get { return languageIndex; }
            set { languageIndex = value; }
        }

        private static DataTable languageTable = SQLiteHelper.GetDataSet("select * from NationalLanguage").Tables[0];
        private static DataTable LanguageTable
        {
            get
            {
                return languageTable;
            }
        }

        /// <summary>
        /// 获取翻译名称
        /// </summary>
        /// <param name="key">传递的TitleName</param>
        /// <param name="index">1:中文，2:英文</param>
        /// <returns></returns>
        public static string GetLanguage(string key)
        {
            string result = String.Empty;

            for (int i = 0; i < LanguageTable.Rows.Count; i++)
            {
                if (key == LanguageTable.Rows[i]["Title"].ToString())
                {
                    if (LanaguageIndex == 1)
                    {
                        result = LanguageTable.Rows[i]["Chinese"].ToString();
                    }
                    else if (LanaguageIndex == 2)
                    {
                        result = LanguageTable.Rows[i]["English"].ToString();
                    }
                    else if (LanaguageIndex == 3)
                    {
                        result = LanguageTable.Rows[i]["Italiano"].ToString();
                    }

                    string unit = "";//LanguageTable.Rows[i][4].ToString();

                    if (unit != "")
                    {
                        result += $"({unit})";
                    }

                    break;
                }
            }
            return result;
        }

        //private static string LanaguageStr = "off,断开,OFF\r\non,闭合,ON\r\nprompt,提示，Prompt\r\nmessage_prompt,是否清空存储的历史记录,Whether to empty the stored history\r\nCell1,发生时间,OccurredDate\r\nCell2,序列号,SerialNumber\r\nCell3,故障byte1,FaultByte1\r\nCell4,故障byte2,FaultByte2\r\nCell5,故障byte3,FaultByte3\r\nCell6,故障byte4,FaultByte4\r\nCell7,故障byte5,FaultByte5\r\nCell8,故障byte6,FaultByte6\r\nCell9,故障byte7,FaultByte7\r\nCell10,故障byte8,FaultByte8\r\nCell11,电池状态,BatteryStatus\r\nCell12,充电MOS,ChargingMOS\r\nCell13,放电MOS,DischargeMOS\r\nCell14,预充MOS,Pre-chargedMOS\r\nCell15,充电急停,ChargingEmergencyStop\r\nCell16,加热MOS,HeatedMOS\r\nCell17,电池电压(V),BatteryVoltage(V)\r\nCell18,负载电压(V),LoadVoltage(V)\r\nCell19,电池电流(A),BatteryCurrent(A)\r\nCell20,电池剩余容量(SOC),SOC\r\nCell21,电池健康程度(SOH),SOH\r\nCell22,充电电流上限(A),ChargeCurrentLimit(A)\r\nCell23,放电电流上限(A),DischargeCurrentLimit(A)\r\nCell24,累计充电容量(Ah,CumulativeChargingCapacity(Ah))\r\nCell25,累计放电容量(Ah,Cumulativedischargecapacity(Ah))\r\nCell26,最高单体电压(mV),Maximumcellvoltage(mV)\r\nCell27,最高单体电压编号,Maximumcellvoltagenumber\r\nCell28,最低单体电压(mV),Minimumcellvoltage(mV)\r\nCell29,最低单体电压编号,Minimumcellvoltagenumber\r\nCell30,电压1(mV),Voltage1(mV)\r\nCell31,电压2(mV),Voltage2(mV)\r\nCell32,电压3(mV),Voltage3(mV)\r\nCell33,电压4(mV),Voltage4(mV)\r\nCell34,电压5(mV),Voltage5(mV)\r\nCell35,电压6(mV),Voltage6(mV)\r\nCell36,电压7(mV),Voltage7(mV)\r\nCell37,电压8(mV),Voltage8(mV)\r\nCell38,电压9(mV),Voltage9(mV)\r\nCell39,电压10(mV),Voltage10(mV)\r\nCell40,电压11(mV),Voltage11(mV)\r\nCell41,电压12(mV),Voltage12(mV)\r\nCell42,电压13(mV),Voltage13(mV)\r\nCell43,电压14(mV),Voltage14(mV)\r\nCell44,电压15(mV),Voltage15(mV)\r\nCell45,电压16(mV),Voltage16(mV)\r\nCell46,环境温度(℃),AmbientTemperature(°C)\r\nCell47,Mos温度(℃),MosTemperature(°C)\r\nCell48,最高单体温度(℃),Maximumcelltemperature(°C)\r\nCell49,最高单体温度编号,Maximumcelltemperaturenumber\r\nCell50,最低单体温度(℃),Minimumcelltemperature(°C)\r\nCell51,最低单体温度编号,Minimumcelltemperaturenumber\r\nCell52,温度1(℃),Temperature1(°C)\r\nCell53,温度2(℃),Temperature2(°C)\r\nCell54,温度3(℃),Temperature3(°C)\r\nCell55,温度4(℃),Temperature4(°C)\r\nCell56,温度5(℃),Temperature5(°C)\r\nCell57,温度6(℃),Temperature6(°C)\r\nCell58,温度7(℃),Temperature7(°C)\r\nCell59,温度8(℃),Temperature8(°C)\r\nCell60,剩余容量(Ah),RemainingCapacity(Ah)\r\nCell61,满充容量(Ah),FullChargeCapacity(Ah)\r\nCell62,均衡状态,EquilibriumState\r\nCell63,均衡温度1(℃),EquilibriumTemperature1(°C)\r\nCell64,均衡温度2(℃),EquilibriumTemperature2(°C)";
        private static string LanaguageStr = @"off,断开,OFF,Staccare‌
on,闭合,ON,Chiudere‌
prompt,提示,Prompt,Tempestiva
message_prompt,是否清空存储的历史记录,Whether to empty the stored history,Se svuotare la cronologia memorizzata？
Cell1,发生时间,OccurredDate
Cell2,序列号,SerialNumber
Cell3,故障byte1,FaultByte1,GuastoByte1
Cell4,故障byte2,FaultByte2,GuastoByte2
Cell5,故障byte3,FaultByte3,GuastoByte3
Cell6,故障byte4,FaultByte4,GuastoByte4
Cell7,故障byte5,FaultByte5,GuastoByte5
Cell8,故障byte6,FaultByte6,GuastoByte6
Cell9,故障byte7,FaultByte7,GuastoByte7
Cell10,故障byte8,FaultByte8,GuastoByte8
Cell11,电池状态,BatteryStatus,Stato della batteria
Cell12,充电MOS,ChargingMOS,MOS di carica
Cell13,放电MOS,DischargeMOS,MOS di scarica
Cell14,预充MOS,Pre-chargedMOS,MOS di precarica
Cell15,充电急停,ChargingEmergencyStop,Carica Arresto di emergenza
Cell16,加热MOS,HeatedMOS,Riscaldamento MOS
Cell17,电池电压(V),BatteryVoltage(V),Tensione della batteria (V)
Cell18,负载电压(V),LoadVoltage(V),Tensione di carico (V)
Cell19,电池电流(A),BatteryCurrent(A),Corrente della batteria (A)
Cell20,电池剩余容量(SOC),SOC,SOC
Cell21,电池健康程度(SOH),SOH,SOH
Cell22,充电电流上限(A),ChargeCurrentLimit(A),Limite di corrente di carica (A)
Cell23,放电电流上限(A),DischargeCurrentLimit(A),Limite di corrente di scarica (A)
Cell24,累计充电容量(Ah),CumulativeChargingCapacity(Ah),Capacità di carica cumulativa (Ah)
Cell25,累计放电容量(Ah),Cumulativedischargecapacity(Ah),Capacità di scarica cumulativa (Ah)
Cell26,最高单体电压(mV),Maximumcellvoltage(mV),Tensione massima individuale (mV)
Cell27,最高单体电压编号,Maximumcellvoltagenumber,Tensione massima individuale n.
Cell28,最低单体电压(mV),Minimumcellvoltage(mV),Tensione singola più bassa (mV)
Cell29,最低单体电压编号,Minimumcellvoltagenumber,Tensione singola più bassa n.
Cell30,电压1(mV),Voltage1(mV),Tensione1(mV)
Cell31,电压2(mV),Voltage2(mV),Tensione2(mV)
Cell32,电压3(mV),Voltage3(mV),Tensione3(mV)
Cell33,电压4(mV),Voltage4(mV),Tensione4(mV)
Cell34,电压5(mV),Voltage5(mV),Tensione5(mV)
Cell35,电压6(mV),Voltage6(mV),Tensione6(mV)
Cell36,电压7(mV),Voltage7(mV),Tensione7(mV)
Cell37,电压8(mV),Voltage8(mV),Tensione8(mV)
Cell38,电压9(mV),Voltage9(mV),Tensione9(mV)
Cell39,电压10(mV),Voltage10(mV),Tensione10(mV)
Cell40,电压11(mV),Voltage11(mV),Tensione11(mV)
Cell41,电压12(mV),Voltage12(mV),Tensione12(mV)
Cell42,电压13(mV),Voltage13(mV),Tensione13(mV)
Cell43,电压14(mV),Voltage14(mV),Tensione14(mV)
Cell44,电压15(mV),Voltage15(mV),Tensione15(mV)
Cell45,电压16(mV),Voltage16(mV),Tensione16(mV)
Cell46,环境温度(℃),AmbientTemperature(℃),Temperatura ambiente (℃)
Cell47,Mos温度(℃),MosTemperature(℃),Temperatura Mos (℃)
Cell48,最高单体温度(℃),Maximumcelltemperature(℃),Temperatura massima singola (℃)
Cell49,最高单体温度编号,Maximumcelltemperaturenumber,Temperatura massima singola n.
Cell50,最低单体温度(℃),Minimumcelltemperature(℃),Temperatura singola più bassa (℃)
Cell51,最低单体温度编号,Minimumcelltemperaturenumber,Temperatura singola più bassa n.
Cell52,温度1(℃),Temperature1(℃),Temperatura1(℃)
Cell53,温度2(℃),Temperature2(℃),Temperatura2(℃)
Cell54,温度3(℃),Temperature3(℃),Temperatura3(℃)
Cell55,温度4(℃),Temperature4(℃),Temperatura4(℃)
Cell56,温度5(℃),Temperature5(℃),Temperatura5(℃)
Cell57,温度6(℃),Temperature6(℃),Temperatura6(℃)
Cell58,温度7(℃),Temperature7(℃),Temperatura7(℃)
Cell59,温度8(℃),Temperature8(℃),Temperatura8(℃)
Cell60,剩余容量(Ah),RemainingCapacity(Ah),Capacità residua (Ah)
Cell61,满充容量(Ah),FullChargeCapacity(Ah),Capacità completa (Ah)
Cell62,均衡状态,EquilibriumState,Stato di equilibrio
Cell63,均衡温度1(℃),EquilibriumTemperature1(℃),Temperatura di equilibrio1(℃)
Cell64,均衡温度2(℃),EquilibriumTemperature2(℃),Temperatura di equilibrio2(℃)";

        public static string GetString(string key)
        {
            if (key == null || key.Length == 0)
            {
                return "";
            }
            string[] csvarr = LanaguageStr.Split("\n".ToCharArray());
            string[] array = csvarr;
            foreach (string s in array)
            {
                string[] sarr = s.Split(",".ToCharArray());
                if (sarr.Length >= 4 && sarr[0].Equals(key))
                {
                    return sarr[languageIndex].TrimEnd("\r".ToCharArray()).Trim();
                }
            }
            return key;
        }
    }
}
