using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofarBMS.Helper
{
    /// <summary>
    /// 语言翻译类
    /// </summary>
    public class LanguageHelper
    {
        
        /// <summary>
        /// 当前语言：1中文 2英文
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
                if (key == LanguageTable.Rows[i][1].ToString())
                {
                    if (LanaguageIndex == 1)
                    {
                        result = LanguageTable.Rows[i][2].ToString();
                    }
                    else if (LanaguageIndex == 2)
                    {
                        result = LanguageTable.Rows[i][3].ToString();
                    }

                    string unit = LanguageTable.Rows[i][4].ToString();

                    if (unit != "")
                    {
                        result += $"({unit})";
                    }

                    break;
                }
            }
            return result;
        }

        private static string LanaguageStr = "off,断开,OFF\r\non,闭合,ON\r\nprompt,提示，Prompt\r\nmessage_prompt,是否清空存储的历史记录,Whether to empty the stored history\r\nCell1,发生时间,OccurredDate\r\nCell2,序列号,SerialNumber\r\nCell3,故障byte1,FaultByte1\r\nCell4,故障byte2,FaultByte2\r\nCell5,故障byte3,FaultByte3\r\nCell6,故障byte4,FaultByte4\r\nCell7,故障byte5,FaultByte5\r\nCell8,故障byte6,FaultByte6\r\nCell9,故障byte7,FaultByte7\r\nCell10,故障byte8,FaultByte8\r\nCell11,电池状态,BatteryStatus\r\nCell12,充电MOS,ChargingMOS\r\nCell13,放电MOS,DischargeMOS\r\nCell14,预充MOS,Pre-chargedMOS\r\nCell15,充电急停,ChargingEmergencyStop\r\nCell16,加热MOS,HeatedMOS\r\nCell17,电池电压(V),BatteryVoltage(V)\r\nCell18,负载电压(V),LoadVoltage(V)\r\nCell19,电池电流(A),BatteryCurrent(A)\r\nCell20,电池剩余容量(SOC),SOC\r\nCell21,电池健康程度(SOH),SOH\r\nCell22,充电电流上限(A),ChargeCurrentLimit(A)\r\nCell23,放电电流上限(A),DischargeCurrentLimit(A)\r\nCell24,累计充电容量(Ah,CumulativeChargingCapacity(Ah))\r\nCell25,累计放电容量(Ah,Cumulativedischargecapacity(Ah))\r\nCell26,最高单体电压(mV),Maximumcellvoltage(mV)\r\nCell27,最高单体电压编号,Maximumcellvoltagenumber\r\nCell28,最低单体电压(mV),Minimumcellvoltage(mV)\r\nCell29,最低单体电压编号,Minimumcellvoltagenumber\r\nCell30,电压1(mV),Voltage1(mV)\r\nCell31,电压2(mV),Voltage2(mV)\r\nCell32,电压3(mV),Voltage3(mV)\r\nCell33,电压4(mV),Voltage4(mV)\r\nCell34,电压5(mV),Voltage5(mV)\r\nCell35,电压6(mV),Voltage6(mV)\r\nCell36,电压7(mV),Voltage7(mV)\r\nCell37,电压8(mV),Voltage8(mV)\r\nCell38,电压9(mV),Voltage9(mV)\r\nCell39,电压10(mV),Voltage10(mV)\r\nCell40,电压11(mV),Voltage11(mV)\r\nCell41,电压12(mV),Voltage12(mV)\r\nCell42,电压13(mV),Voltage13(mV)\r\nCell43,电压14(mV),Voltage14(mV)\r\nCell44,电压15(mV),Voltage15(mV)\r\nCell45,电压16(mV),Voltage16(mV)\r\nCell46,环境温度(℃),AmbientTemperature(°C)\r\nCell47,Mos温度(℃),MosTemperature(°C)\r\nCell48,最高单体温度(℃),Maximumcelltemperature(°C)\r\nCell49,最高单体温度编号,Maximumcelltemperaturenumber\r\nCell50,最低单体温度(℃),Minimumcelltemperature(°C)\r\nCell51,最低单体温度编号,Minimumcelltemperaturenumber\r\nCell52,温度1(℃),Temperature1(°C)\r\nCell53,温度2(℃),Temperature2(°C)\r\nCell54,温度3(℃),Temperature3(°C)\r\nCell55,温度4(℃),Temperature4(°C)\r\nCell56,温度5(℃),Temperature5(°C)\r\nCell57,温度6(℃),Temperature6(°C)\r\nCell58,温度7(℃),Temperature7(°C)\r\nCell59,温度8(℃),Temperature8(°C)\r\nCell60,剩余容量(Ah),RemainingCapacity(Ah)\r\nCell61,满充容量(Ah),FullChargeCapacity(Ah)\r\nCell62,均衡状态,EquilibriumState\r\nCell63,均衡温度1(℃),EquilibriumTemperature1(°C)\r\nCell64,均衡温度2(℃),EquilibriumTemperature2(°C)";

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
                if (sarr.Length >= 3 && sarr[0].Equals(key))
                {
                    return sarr[languageIndex].TrimEnd("\r".ToCharArray()).Trim();
                }
            }
            return key;
        }
    }
}
