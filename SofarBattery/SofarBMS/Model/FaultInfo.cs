using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofarBMS.Model
{
    public class FaultInfo
    {
        private string _content;
        private int _byte;
        private int _bit;
        private int _value;
        private int _state;
        private int _type;
        public FaultInfo(string content,int canbyte,int canbit, int value, int state,int type)
        {
            this._content = content;
            this._value = value;
            this._state = state;
            this.Byte = canbyte;
            this.Bit = canbit;
            this._type = type;
        }
        public string Content { get => _content; set => _content = value; }
        public int Type { get => _type; set => _type = value; }
        public int Value { get => _value; set => _value = value; }
        public int State { get => _state; set => _state = value; }
        public int Byte { get => _byte; set => _byte = value; }
        public int Bit { get => _bit; set => _bit = value; }


        public static List<FaultInfo> FaultInfos = new List<FaultInfo>() {
                    new FaultInfo("单体过压保护,CELL_U_H1",0,0,0,0,2),
                    new FaultInfo("单体过压告警,CELL_U_H2",0,1,0,0,1),
                    new FaultInfo("单体欠压保护,CELL_U_L1",0,2,0,0,2),
                    new FaultInfo("单体欠压告警,CELL_U_L2",0,3,0,0,1),
                    new FaultInfo("总压过压保护,TOTAL_U_H1",0,4,0,0,2),
                    new FaultInfo("总压过压告警,TOTAL_U_H2",0,5,0,0,1),
                    new FaultInfo("总压欠压保护,TOTAL_U_L1",0,6,0,0,2),
                    new FaultInfo("总压欠压告警,TOTAL_U_L2",0,7,0,0,1),
                    new FaultInfo("充电温度过高保护,CHG_T_H1",1,0,0,0,2),
                    new FaultInfo("充电温度过高告警,CHG_T_H2",1,1,0,0,1),
                    new FaultInfo("充电温度过低保护,CHG_T_L1",1,2,0,0,2),
                    new FaultInfo("充电温度过低告警,CHG_T_L2",1,3,0,0,1),
                    new FaultInfo("放电温度过高保护,DSG_T_H1",1,4,0,0,2),
                    new FaultInfo("放电温度过高告警,DSG_T_H2",1,5,0,0,1),
                    new FaultInfo("放电温度过低保护,DSG_T_L1",1,6,0,0,2),
                    new FaultInfo("放电温度过低告警,DSG_T_L2",1,7,0,0,1),
                    new FaultInfo("充电过流保护,CHG_I_H1",2,0,0,0,2),
                    new FaultInfo("充电过流告警,CHG_I_H2",2,1,0,0,1),
                    new FaultInfo("放电过流保护,DSG_I_H1",2,2,0,0,2),
                    new FaultInfo("放电过流告警,DSG_I_H2",2,3,0,0,1),
                    new FaultInfo("环境温度过高保护,EN_T_H1",2,4,0,0,2),
                    new FaultInfo("环境温度过高告警,EN_T_H2",2,5,0,0,1),
                    new FaultInfo("环境温度过低保护,EN_T_L1",2,6,0,0,2),
                    new FaultInfo("环境温度过低告警,EN_T_L2",2,7,0,0,1),
                    new FaultInfo("MOS温度过高保护,MOS_T_H1",3,0,0,0,2),
                    new FaultInfo("MOS温度过高告警,MOS_T_H2",3,1,0,0,1),
                    new FaultInfo("SOC过低保护,SOC_L1",3,2,0,0,2),
                    new FaultInfo("SOC过低告警,SOC_L2",3,3,0,0,1),
                    new FaultInfo("总压采样异常,TOTAL_VOLT_DIFF_OVER",3,4,0,0,3),
                    new FaultInfo("总压过大硬件保护,TOTAL_U_H0",3,5,0,0,2),
                    new FaultInfo("充电过流硬件保护,CHG_I_H0",3,6,0,0,2),
                    new FaultInfo("放电过流硬件保护,DSG_I_H0",3,7,0,0,2),
                    new FaultInfo("电池满充,CHG_FULL",4,0,0,0,1),
                    new FaultInfo("短路保护,SHORT_CIRCUIT",4,1,0,0,2),
                    new FaultInfo("EEPROM异常,EEPROM_ERROR",4,2,0,0,3),
                    new FaultInfo("电芯失效,CELL_INVALID",4,3,0,0,3),
                    new FaultInfo("NTC异常,NTC_INVALID",4,4,0,0,3),
                    new FaultInfo("充电MOS异常,CHG_MOS_INVALID",4,5,0,0,3),
                    new FaultInfo("放电MOS异常,DSG_MOS_INVALID",4,6,0,0,3),
                    new FaultInfo("采集异常,SAMP_INVALID",4,7,0,0,3),
                    new FaultInfo("限流异常,LIMIT_INVALID",5,0,0,0,3),
                    new FaultInfo("充电器反接,CHG_REVERSED",5,1,0,0,3),
                    new FaultInfo("CAN通信异常,CAN_COM_FAIL",5,2,0,0,2),
                    new FaultInfo("CAN_ID冲突保护,CANID_CONFLICT1",5,3,0,0,2),
                    new FaultInfo("电池放空,DISCHG_EMPTY",5,4,0,0,1),
                    new FaultInfo("PCU永久故障,PCU_INVALID",5,5,0,0,3),
                    new FaultInfo("预充失败,PRE_CHG_ERR",5,6,0,0,3),
                    new FaultInfo("软件异常,SOFT_ERROR",5,7,0,0,3),
                    new FaultInfo("充电电流大环零点不良,CHG_CURR_BIG_RING_BADNESS",6,0,0,0,3),
                    new FaultInfo("充电电流小环零点不良,CHG_CURR_LITTLE_RING_BADNESS",6,1,0,0,3),
                    new FaultInfo("零点电流异常,CURR_OFFSET_BADNESS",6,2,0,0,3),
                    new FaultInfo("主回路保险丝熔断,MAIN_CIRCUIT_BLOWN_FUSE",6,3,0,0,3),
                    new FaultInfo("锁存器异常,LATCH_ERR",6,4,0,0,3),
                    new FaultInfo("12V电压异常,VOLT_12V_ERR",6,5,0,0,3),
                    new FaultInfo("电池过压严重故障,CELL_VOLT_OVER_MAJOR_FAULT",6,6,0,0,3),
                    new FaultInfo("电池欠压严重故障,CELL_VOLT_LOW_MAJOR_FAULT",6,7,0,0,3),
                    new FaultInfo("放电电流大环零点不良,DSG_CURR_BIG_RING_BADNESS",7,0,0,0,3),
                    new FaultInfo("放电电流小环零点不良,DSG_CURR_LITTLE_RING_BADNESS",7,1,0,0,3),
                    new FaultInfo("电芯温度过大,CELL_TEMP_DIFF_OVER",7,2,0,0,2),
                    new FaultInfo("绝缘检测,INSUALATION_FAULT",7,3,0,0,2)
        };
        
        public static List<FaultInfo> FaultInfos2 = new List<FaultInfo>()
        {
                    new FaultInfo("主动均衡失效,ACTIVE_BALANCE_ERR",0,0,0,0,1),
                    new FaultInfo("主动均衡参考电流均衡电流差值异常,ACT_BAL_CHG_CURR_ABNORMAL_ERR",0,1,0,0,1),
                    new FaultInfo("主动均衡原边放电慢过流保护,ACT_BAL_DIS_OVER_CURR_PROTECT",0,2,0,0,1),
                    new FaultInfo("主动均衡原边充电慢过流保护,ACT_BAL_CHG_OVER_CURR_PROTECT",0,3,0,0,1),
                    new FaultInfo("主动均衡硬件过流保护,ACT_BAL_HARD_OVER_CURR_PROTECT",0,4,0,0,1),
                    new FaultInfo("主动均衡过流锁死,ACT_BAL_OVER_CURR_LOCK",0,5,0,0,1),
                    new FaultInfo("主动均衡原边慢过压,ACT_BAL_PRI_OVER_VOLT_PROTECT",0,6,0,0,1),
                    new FaultInfo("主动均衡副边慢过压,ACT_BAL_SEC_OVER_VOLT_PROTECT",0,7,0,0,1),
                    new FaultInfo("主动均衡副边慢欠压,ACT_BAL_SEC_UNDER_VOLT_PROTECT",1,0,0,0,1),
                    new FaultInfo("主动均衡原边快过压,ACT_BAL_PRI_FAST_OVER_VOLT_PROTECT",1,1,0,0,1),
                    new FaultInfo("主动均衡副边快过压,ACT_BAL_SEC_FAST_OVER_VOLT_PROTECT",1,2,0,0,1),
                    new FaultInfo("主动均衡原边放电快过流保护,ACT_BAL_DIS_FAST_OVER_CURR_PROTECT",1,3,0,0,1),
                    new FaultInfo("主动均衡原边充电快过流保护,ACT_BAL_CHG_FAST_OVER_CURR_PROTECT",1,4,0,0,1),
                    new FaultInfo("主动均衡硬件过压保护,ACT_BAL_HARD_VOLT_H",1,5,0,0,1),
                    new FaultInfo("主动均衡功率过大,ACT_BAL_POWER_OVER_PROTECT",1,6,0,0,3),
                    new FaultInfo("主动均衡电流零点不良,BAL_CURR_RING_BADNESS",1,7,0,0,3),
                    new FaultInfo("保留,res",2,0,0,0,1),
                    new FaultInfo("功率端子温度采样线异常,PWR_TEMP_WIRE_ABNORMAL_FAULT",2,1,0,0,3),
                    new FaultInfo("被动均衡温度过高告警,BAT_BALANCE_TEMP_OVER_ALARM",2,2,0,0,1),
                    new FaultInfo("电芯电压严重不均衡故障,BAT_CELL_UNBALANCE_SERIOUS",2,3,0,0,3),
                    new FaultInfo("电芯过温故障,BAT_CELL_TEMP_OVER_ERR",2,4,0,0,3),
                    new FaultInfo("过流失能,BAT_CURR_DISABLE",2,5,0,0,3),
                    new FaultInfo("电池过压失能,BAT_VOLT_HIGH_DISABLE",2,6,0,0,3),
                    new FaultInfo("电池欠压失能,BAT_VOLT_LOW_DISABLE",2,7,0,0,3),
                    new FaultInfo("电池高低温失能,BAT_CELL_TEMP_OVER_OR_LOW_DISABLE",3,0,0,0,3),
                    new FaultInfo("温升过大,BAT_TEMP_RISE_DIFF_OVER",3,1,0,0,3),
                    new FaultInfo("保留,res",3,2,0,0,3),
                    new FaultInfo("放电电流过高保护2,BAT_DISCHG_CURR_OVER_PROTECT2",3,3,0,0,2),
                    new FaultInfo("充电温度过高提示,BAT_CHG_TEMP_OVER_TIPS",3,4,0,0,1),
                    new FaultInfo("充电温度过低提示,BAT_CHG_TEMP_LOW_TIPS",3,5,0,0,1),
                    new FaultInfo("放电温度过高提示,BAT_DCHG_TEMP_OVER_TIPS",3,6,0,0,1),
                    new FaultInfo("放电温度过低提示,BAT_DCHG_TEMP_LOW_TIPS",3,7,0,0,1),
                    new FaultInfo("flash异常,FLASH_SAVE_INVALID",4,0,0,0,3),
                    new FaultInfo("功率端子过温失能,POWER_TEMP_DISABLE_H",4,1,0,0,3),
                    new FaultInfo("mos过温失效,MOS_TEMP_DISABLE_H",4,2,0,0,1),
                    new FaultInfo("电芯严重过压锁死故障,BAT_CELL_VOLT_HIGH_SERIOUS_LOCK",4,3,0,0,3),
                    new FaultInfo("电芯电压采样线异常,CELL_VOLT_WIRE_ABNORMAL_FAULT",4,4,0,0,3),
                    new FaultInfo("电芯温度采样线异常,CELL_TEMP_WIRE_ABNORMAL_FAULT",4,5,0,0,3),
                    new FaultInfo("均衡电阻温度采样线异常,BAL_TEMP_WIRE_ABNORMAL_FAULT",4,6,0,0,3),
                    new FaultInfo("功率端子温度采样线异常,PWR_TEMP_WIRE_ABNORMAL_FAULT",4,7,0,0,3),
                    new FaultInfo("功率端子过温保护,POWER_TEMP_PROT",5,0,0,0,2),
                    new FaultInfo("加热异常,FLT_HEAT_ERROR",5,1,0,0,1),
                    new FaultInfo("加热继电器粘连,FLT_HEAT_RELAY_ADHESION",5,2,0,0,1),
                    new FaultInfo("加热继电器断路,FLT_HEAT_RELAY_OPEN",5,3,0,0,1),
                    new FaultInfo("保留,res",5,4,0,0,3),
                    new FaultInfo("保留,res",5,5,0,0,3),
                    new FaultInfo("保留,res",5,6,0,0,3),
                    new FaultInfo("保留,res",5,7,0,0,3),
                    new FaultInfo("充电严重过流故障锁定,FLT_CHG_CUR_OVER_SERIOUS_LOCK",6,0,0,0,3),
                    new FaultInfo("放电严重过流故障锁定,FLT_DCHG_CUR_OVER_SERIOUS_LOCK",6,1,0,0,3),
                    new FaultInfo("加热回路失控故障,FLT_HEAT_LOSE_CONTROL",6,2,0,0,3),
        };

        public static List<FaultInfo> FaultInfos3 = new List<FaultInfo>()
        {
                    new FaultInfo("电池簇电压过低保护(BCU),Clu_Low_Voltage_Protec",0,0,0,0,2),
                    new FaultInfo("电池簇电压过高保护(BCU),Clu_High_Voltage_Protect",0,1,0,0,2),
                    new FaultInfo("电池簇充电过流保护(BCU),Clu_Chg_Over_Currr_Prot",0,2,0,0,2),
                    new FaultInfo("电池簇放电过流保护(BCU),Clu_Dsg_Over_Currr_Prot",0,3,0,0,2),
                    new FaultInfo("保留,res",0,4,0,0,2),
                    new FaultInfo("电池簇放电过流保护2(BCU),Clu_Dsg_Over_Currr_Prot2",0,5,0,0,2),
                    new FaultInfo("保留,res",0,6,0,0,2),
                    new FaultInfo("BCU与BMU间CAN通讯故障(BCU),BOARD_BCU_BMU_CAN_COMM_ERR",0,7,0,0,2),
                    new FaultInfo("功率端子过温保护(BCU),BOARD_POWER_TERMINAL_TEMP_OVER_PROTECT",1,0,0,0,2),
                    new FaultInfo("保留,res",1,1,0,0,2),
                    new FaultInfo("保留,res",1,2,0,0,2),
                    new FaultInfo("保留,res",1,3,0,0,2),
                    new FaultInfo("保留,res",1,4,0,0,2),
                    new FaultInfo("保留,res",1,5,0,0,2),
                    new FaultInfo("保留,res",1,6,0,0,2),
                    new FaultInfo("保留,res",1,7,0,0,2),
                    new FaultInfo("过流失能,Clu Over_Curr_Disable",2,0,0,0,3),
                    new FaultInfo("充电严重过流锁死(BCU),Chg_Over_Curr_Lock",2,1,0,0,3),
                    new FaultInfo("放电严重过流锁死(BCU),Dsg_Over_Curr_Lock",2,2,0,0,3),
                    new FaultInfo("电芯严重过压锁死(BCU),Cell_Over_Volt_Lock",2,3,0,0,3),
                    new FaultInfo("电流大环零点不良(BCU),BOARD_CURR_BIG_RING_BADNESS",2,4,0,0,3),
                    new FaultInfo("电流小环零点不良(BCU),BOARD_CURR_LITTLE_RING_BADNESS",2,5,0,0,3),
                    new FaultInfo("NTC开路(BCU),BOARD_NTC_OPEN_CIRCUIT",2,6,0,0,3),
                    new FaultInfo("NTC短路(BCU),BOARD_NTC_SHORT_CIRCUIT",2,7,0,0,3),
                    new FaultInfo("簇电压压差过大(BCU),BOARD_TOTAL_VOLT_DIFF_OVER",3,0,0,0,3),
                    new FaultInfo("辅源异常(BCU),BOARD_VOLT_9V_ERR",3,1,0,0,3),
                    new FaultInfo("flash存储错误(BCU),BOARD_FLASH_ERR",3,2,0,0,3),
                    new FaultInfo("功率端子过温失能(BCU),BOARD_POWER_TERMINAL_TEMP_OVER_LOCK",3,3,0,0,3),
                    new FaultInfo("主回路保险丝熔断(BCU),BOARD_MAIN_CIRCUIT_BLOWN_FUSE",3,4,0,0,3),
                    new FaultInfo("正继电器粘连(BCU),BOARD_POSITIVE_RELAY_ADHESION",3,5,0,0,3),
                    new FaultInfo("负继电器粘连(BCU),BOARD_NEGATIVE_RELAY_ADHESION",3,6,0,0,3),
                    new FaultInfo("绝缘异常(BCU),BOARD_INSUALATION_FAULT",3,7,0,0,3),
                    new FaultInfo("电池簇电压过低告警(BCU),Clu_Low_Voltage_Alm",4,0,0,0,1),
                    new FaultInfo("电池簇电压过高告警(BCU),Clu_High_Voltage_Alm",4,1,0,0,1),
                    new FaultInfo("电池簇充电过流告警(BCU),Clu_Chg_Over_Currr_Alm",4,2,0,0,1),
                    new FaultInfo("电池簇放电过流告警(BCU),Clu_Dsg_Over_Currr_Alm",4,3,0,0,1),
                    new FaultInfo("SOC过低告警(BCU),Soc_low_Alm",4,4,0,0,1),
                    new FaultInfo("保留,res",4,5,0,0,1),
                    new FaultInfo("保留,res",4,6,0,0,1),
                    new FaultInfo("保留,res",4,7,0,0,1),
                    new FaultInfo("BCU与BMU间内网CAN地址冲突(BCU),BOARD_BCU_BMU_CANID_CONFLICT",5,0,0,0,1),
                    new FaultInfo("BCU与BCU间外网CAN地址冲突(BCU),BOARD_BCU_BCU_CANID_CONFLICT",5,1,0,0,1),
                    new FaultInfo("外can编址失败(BCU),BOARD_EXT_CAN_ADDR_FAULT",5,2,0,0,1),
                    new FaultInfo("BMU异常关机告警(BCU),BOARD_BMU_ABNORMAL_SLEEP_ALARM",5,3,0,0,1),
                    new FaultInfo("BCU与DCDC间CAN通讯故障(BCU),BOARD_BCU_DCDC_CAN_COMM_ERR",5,4,0,0,1),
                    new FaultInfo("单板过温告警,BOARD_BCU_TEMP_OVER_ALM",5,5,0,0,1),
                    new FaultInfo("单板低温告警,BOARD_BCU_TEMP_UNDER_ALM",5,6,0,0,1),
                    new FaultInfo("pcs与bcu通讯故障,BOARD_BCU_PCS_CAN_COMM_ERR",5,7,0,0,1),
                    new FaultInfo("内can编址失败(BCU),BOARD_INNER_CAN_ADDR_FAULT",6,0,0,0,3),
                    new FaultInfo("电池包数目异常(BCU),BOARD_PACK_NUM_FAULT",6,1,0,0,3),
                    new FaultInfo("片外adc采样异常(BCU),BOARD_EXT_ADC_FAULT",6,2,0,0,3),
                    new FaultInfo("内can反复编址异常(BCU),BOARD_INNER_CAN_ADDR_REPEAT_FAULT",6,3,0,0,3),
                    new FaultInfo("预充异常(BCU),BOARD_PRE_CHARGE_FAULT",6,4,0,0,3),
                    new FaultInfo("加热膜回路故障,BOARD_HEAT_CIR_ERR",6,5,0,0,3),
                    new FaultInfo("保留,res",6,6,0,0,3),
                    new FaultInfo("保留,res",6,7,0,0,3),
                    new FaultInfo("加热继电器粘连(BCU),BOARD_HEAT_RELAY_ADHESION",7,0,0,0,1),
                    new FaultInfo("加热功率异常(BCU),BOARD_HEAT_POWER_ERR",7,1,0,0,1),
                    new FaultInfo("加热长时间无功率(BCU),BOARD_HEAT_POWER_NULL",7,2,0,0,1),
                    new FaultInfo("加热单次超时(BCU),BOARD_HEAT_OVER_TIME",7,3,0,0,1),
                    new FaultInfo("加热膜阻值异常(BCU),BOARD_HEAT_RES_ERR",7,4,0,0,1),
                    new FaultInfo("加热MOS异常(BCU),BOARD_HEAT_MOS_ERR",7,5,0,0,1),
                    new FaultInfo("加热保险丝异常(BCU),BOARD_HEAT_FUSE_ERR",7,6,0,0,1),
                    new FaultInfo("保留,res",7,7,0,0,1)
        };
    }
}
