using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerKit.UI.Common
{
    public class FaultInfo
    {
        private string _content;
        private int _byte;
        private int _bit;
        private int _value;
        private int _state;
        private int _type;
        public FaultInfo(string content, int canbyte, int canbit, int value, int state, int type)
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

        // type：1—告警 、2—保护、3—故障、4-提示
        //0x008:BMS发送内部电池故障信息1
        public static List<FaultInfo> FaultInfos1 = new List<FaultInfo>() {
                    new FaultInfo("单体过压保护,FLT_CELL_U_H1",0,0,0,0,2),
                    new FaultInfo("单体过压告警,FLT_CELL_U_H2",0,1,0,0,1),
                    new FaultInfo("单体欠压保护,FLT_CELL_U_L1",0,2,0,0,2),
                    new FaultInfo("单体欠压告警,FLT_CELL_U_L2",0,3,0,0,1),
                    new FaultInfo("总压过压保护,FLT_TOTAL_U_H1",0,4,0,0,2),
                    new FaultInfo("总压过压告警,FLT_TOTAL_U_H2",0,5,0,0,1),
                    new FaultInfo("总压欠压保护,FLT_TOTAL_U_L1",0,6,0,0,2),
                    new FaultInfo("总压欠压告警,FLT_TOTAL_U_L2",0,7,0,0,1),
                    new FaultInfo("充电温度过高保护,FLT_CHG_T_H1",1,0,0,0,2),
                    new FaultInfo("充电温度过高告警,FLT_CHG_T_H2",1,1,0,0,1),
                    new FaultInfo("充电温度过低保护,FLT_CHG_T_L1",1,2,0,0,2),
                    new FaultInfo("充电温度过低告警,FLT_CHG_T_L2",1,3,0,0,1),
                    new FaultInfo("放电温度过高保护,FLT_DSG_T_H1",1,4,0,0,2),
                    new FaultInfo("放电温度过高告警,FLT_DSG_T_H2",1,5,0,0,1),
                    new FaultInfo("放电温度过低保护,FLT_DSG_T_L1",1,6,0,0,2),
                    new FaultInfo("放电温度过低告警,FLT_DSG_T_L2",1,7,0,0,1),
                    new FaultInfo("充电过流保护,FLT_CHG_I_H1",2,0,0,0,2),
                    new FaultInfo("充电过流告警,FLT_CHG_I_H2",2,1,0,0,1),
                    new FaultInfo("放电过流保护,FLT_DSG_I_H1",2,2,0,0,2),
                    new FaultInfo("放电过流告警,FLT_DSG_I_H2",2,3,0,0,1),
                    new FaultInfo("环境温度过高保护,FLT_EN_T_H1",2,4,0,0,2),
                    new FaultInfo("环境温度过高告警,FLT_EN_T_H2",2,5,0,0,1),
                    new FaultInfo("环境温度过低保护,FLT_EN_T_L1",2,6,0,0,2),
                    new FaultInfo("环境温度过低告警,FLT_EN_T_L2",2,7,0,0,1),
                    new FaultInfo("MOS温度过高保护,FLT_MOS_T_H1",3,0,0,0,2),
                    new FaultInfo("MOS温度过高告警,FLT_MOS_T_H2",3,1,0,0,1),
                    new FaultInfo("SOC过低保护,FLT_SOC_L1",3,2,0,0,2),
                    new FaultInfo("SOC过低告警,FLT_SOC_L2",3,3,0,0,1),
                    new FaultInfo("总压采样异常,FLT_TOTAL_VOLT_DIFF_OVER",3,4,0,0,3),
                    new FaultInfo("总压过大硬件保护,FLT_TOTAL_U_H0",3,5,0,0,2),
                    new FaultInfo("充电过流硬件保护,FLT_CHG_I_H0",3,6,0,0,2),
                    new FaultInfo("放电过流硬件保护,FLT_DSG_I_H0",3,7,0,0,2),
                    new FaultInfo("电池满充,FLT_CHG_FULL",4,0,0,0,1),
                    new FaultInfo("短路保护,FLT_SHORT_CIRCUIT",4,1,0,0,2),
                    new FaultInfo("EEPROM异常,FLT_EEPROM_ERROR",4,2,0,0,3),
                    new FaultInfo("电芯故障,FLT_CELL_VOLT_FAULT",4,3,0,0,3),
                    new FaultInfo("NTC异常(板温),FLT_NTC_INVALID",4,4,0,0,1),
                    new FaultInfo("充电MOS异常,FLT_CHG_MOS_INVALID",4,5,0,0,3),
                    new FaultInfo("放电MOS异常,FLT_DSG_MOS_INVALID",4,6,0,0,3),
                    new FaultInfo("AFE异常,FLT_SAMP_INVALID",4,7,0,0,3),
                    new FaultInfo("限流异常,FLT_LIMIT_INVALID",5,0,0,0,3),
                    new FaultInfo("充电器反接,FLT_CHG_REVERSED",5,1,0,0,3),
                    new FaultInfo("CAN通信异常,FLT_CAN_COM_FAIL",5,2,0,0,1),
                    new FaultInfo("CAN_ID冲突告警,FLT_CANID_CONFLICT1",5,3,0,0,1),
                    new FaultInfo("电池放空,FLT_DISCHG_EMPTY",5,4,0,0,1),
                    new FaultInfo("PCU永久故障,FLT_PCU_INVALID",5,5,0,0,1),
                    new FaultInfo("预充失败,FLT_PRE_CHG_ERR",5,6,0,0,3),
                    new FaultInfo("软件异常,FLT_SOFT_ERROR",5,7,0,0,3),
                    new FaultInfo("充电电流大环零点不良,CHG_CURR_BIG_RING_BADNESS",6,0,0,0,3),
                    new FaultInfo("充电电流小环零点不良,CHG_CURR_LITTLE_RING_BADNESS",6,1,0,0,3),
                    new FaultInfo("零点电流异常,CURR_OFFSET_BADNESS",6,2,0,0,3),
                    new FaultInfo("主回路保险丝熔断,MAIN_CIRCUIT_BLOWN_FUSE",6,3,0,0,3),
                    new FaultInfo("锁存器不良,LATCH_ERR",6,4,0,0,3),
                    new FaultInfo("辅源电压异常,AUX_VOLT_ERR",6,5,0,0,3),
                    new FaultInfo("电池过压严重故障,CELL_VOLT_OVER_MAJOR_FAULT",6,6,0,0,3),
                    new FaultInfo("电池欠压严重故障,CELL_VOLT_LOW_MAJOR_FAULT",6,7,0,0,3),
                    new FaultInfo("放电电流大环零点不良,DSG_CURR_BIG_RING_BADNESS",7,0,0,0,3),
                    new FaultInfo("放电电流小环零点不良,DSG_CURR_LITTLE_RING_BADNESS",7,1,0,0,3),
                    new FaultInfo("单体温差过大保护,CELL_TEMP_DIFF_OVER",7,2,0,0,2),
                    new FaultInfo("绝缘检测,INSUALATION_FAULT",7,3,0,0,3)
        };

        //0x045:BMS发送内部电池故障信息2
        public static List<FaultInfo> FaultInfos2 = new List<FaultInfo>()
        {
                    new FaultInfo("主动均衡失效,ACTIVE_BALANCE_ERR",0,0,0,0,4),
                    new FaultInfo("主动均衡参考电流均衡电流差值异常,ACT_BAL_CHG_CURR_ABNORMAL_ERR",0,1,0,0,4),
                    new FaultInfo("主动均衡原边放电慢过流保护,ACT_BAL_DIS_OVER_CURR_PROTECT",0,2,0,0,4),
                    new FaultInfo("主动均衡原边充电慢过流保护,ACT_BAL_CHG_OVER_CURR_PROTECT",0,3,0,0,4),
                    new FaultInfo("主动均衡硬件过流保护,ACT_BAL_HARD_OVER_CURR_PROTECT",0,4,0,0,4),
                    new FaultInfo("主动均衡过流锁死,ACT_BAL_OVER_CURR_LOCK",0,5,0,0,4),
                    new FaultInfo("主动均衡原边慢过压,ACT_BAL_PRI_OVER_VOLT_PROTECT",0,6,0,0,4),
                    new FaultInfo("主动均衡副边慢过压,ACT_BAL_SEC_OVER_VOLT_PROTECT",0,7,0,0,4),
                    new FaultInfo("主动均衡副边慢欠压,ACT_BAL_SEC_UNDER_VOLT_PROTECT",1,0,0,0,4),
                    new FaultInfo("主动均衡原边快过压,ACT_BAL_PRI_FAST_OVER_VOLT_PROTECT",1,1,0,0,4),
                    new FaultInfo("主动均衡副边快过压,ACT_BAL_SEC_FAST_OVER_VOLT_PROTECT",1,2,0,0,4),
                    new FaultInfo("主动均衡原边放电快过流保护,ACT_BAL_DIS_FAST_OVER_CURR_PROTECT",1,3,0,0,4),
                    new FaultInfo("主动均衡原边充电快过流保护,ACT_BAL_CHG_FAST_OVER_CURR_PROTECT",1,4,0,0,4),
                    new FaultInfo("主动均衡硬件过压保护,ACT_BAL_HARD_VOLT_H",1,5,0,0,4),
                    new FaultInfo("主动均衡功率过大,ACT_BAL_POWER_OVER_PROTECT",1,6,0,0,4),
                    new FaultInfo("主动均衡电流零点不良,BAL_CURR_RING_BADNESS",1,7,0,0,4),
                    new FaultInfo("保留,Res",2,0,0,0,4),
                    new FaultInfo("保留,Res",2,1,0,0,3),
                    new FaultInfo("被动均衡温度过高告警,BAT_BALANCE_TEMP_OVER_ALARM",2,2,0,0,1),
                    new FaultInfo("电芯压差过大保护,BAT_CELL_UNBALANCE_SERIOUS",2,3,0,0,2),
                    new FaultInfo("电芯过温故障,BAT_CELL_TEMP_OVER_ERR",2,4,0,0,3),
                    new FaultInfo("过流失能,BAT_CURR_DISABLE",2,5,0,0,3),
                    new FaultInfo("电池过压失能,BAT_VOLT_HIGH_DISABLE",2,6,0,0,3),
                    new FaultInfo("电池欠压失能,BAT_VOLT_LOW_DISABLE",2,7,0,0,3),
                    new FaultInfo("电池高低温失能,BAT_CELL_TEMP_OVER_OR_LOW_DISABLE",3,0,0,0,3),
                    new FaultInfo("温升过大,BAT_TEMP_RISE_DIFF_OVER",3,1,0,0,3),
                    new FaultInfo("保留,Res",3,2,0,0,3),
                    new FaultInfo("放电电流过高保护2,BAT_DISCHG_CURR_OVER_PROTECT2",3,3,0,0,2),
                    new FaultInfo("充电温度过高提示,BAT_CHG_TEMP_OVER_TIPS",3,4,0,0,1),
                    new FaultInfo("充电温度过低提示,BAT_CHG_TEMP_LOW_TIPS",3,5,0,0,1),
                    new FaultInfo("放电温度过高提示,BAT_DCHG_TEMP_OVER_TIPS",3,6,0,0,1),
                    new FaultInfo("放电温度过低提示,BAT_DCHG_TEMP_LOW_TIPS",3,7,0,0,1),
                    new FaultInfo("flash异常,FLASH_SAVE_INVALID",4,0,0,0,3),
                    new FaultInfo("功率端子过温失能,POWER_TEMP_DISABLE_H",4,1,0,0,3),
                    new FaultInfo("mos过温失效,MOS_TEMP_DISABLE_H",4,2,0,0,4),
                    new FaultInfo("电芯严重过压锁死故障,BAT_CELL_VOLT_HIGH_SERIOUS_LOCK",4,3,0,0,3),
                    new FaultInfo("电芯电压采样线异常,CELL_VOLT_WIRE_ABNORMAL_FAULT",4,4,0,0,3),
                    new FaultInfo("电芯温度采样异常,CELL_TEMP_WIRE_ABNORMAL_FAULT",4,5,0,0,3),
                    new FaultInfo("均衡温度采样异常,BAL_TEMP_WIRE_ABNORMAL_FAULT",4,6,0,0,1),
                    new FaultInfo("功率端子温度采样线异常,PWR_TEMP_WIRE_ABNORMAL_FAULT",4,7,0,0,3),
                    new FaultInfo("功率端子过温保护,POWER_TEMP_PROT",5,0,0,0,2),
                    new FaultInfo("加热异常,FLT_HEAT_ERROR",5,1,0,0,4),
                    new FaultInfo("加热继电器粘连,FLT_HEAT_RELAY_ADHESION",5,2,0,0,4),
                    new FaultInfo("加热继电器断路,FLT_HEAT_RELAY_OPEN",5,3,0,0,4),
                    new FaultInfo("保留,Res",5,4,0,0,3),
                    new FaultInfo("保留,Res",5,5,0,0,3),
                    new FaultInfo("保留,Res",5,6,0,0,3),
                    new FaultInfo("保留,Res",5,7,0,0,3),
                    new FaultInfo("充电严重过流故障锁定,FLT_CHG_CUR_OVER_SERIOUS_LOCK",6,0,0,0,3),
                    new FaultInfo("放电严重过流故障锁定,FLT_DCHG_CUR_OVER_SERIOUS_LOCK",6,1,0,0,3),
                    new FaultInfo("加热回路失控故障,FLT_HEAT_LOSE_CONTROL",6,2,0,0,3)
        };

        //0x04B:BMS发送内部电池故障信息3
        public static List<FaultInfo> FaultInfos3 = new List<FaultInfo>()
        {
                    new FaultInfo("环境温度过高提示,FLT_EN_T_H3",0,0,0,0,4),
                    new FaultInfo("单体过压提示,FLT_CELL_U_H3",0,1,0,0,4),
                    new FaultInfo("单体欠压提示,FLT_CELL_U_L3",0,2,0,0,4),
                    new FaultInfo("电芯压差大提示,BAT_CELL_UNBALANCE_TIPS",0,3,0,0,4),
                    new FaultInfo("单体温差过大提示,CELL_TEMP_DIFF_OVER_TIPS",0,4,0,0,4),
                    new FaultInfo("总压过压提示,FLT_TOTAL_U_H3",0,5,0,0,4),
                    new FaultInfo("总压欠压提示,FLT_TOTAL_U_L3",0,6,0,0,4),
                    new FaultInfo("SOC过低提示,TIP_SOC_L",0,7,0,0,4),
                    new FaultInfo("充电过流提示,TIP_CHG_CUR_H",1,0,0,0,4),
                    new FaultInfo("放电过流提示,TIP_DSG_CUR_H",1,1,0,0,4),
                    new FaultInfo("AFE断链,AFE_CHAIN_BREAK",1,2,0,0,4),
                    new FaultInfo("电解液检测传感器异常提示,ELECTROLYTE_MON_SEN_TIPS",1,3,0,0,4),
                    new FaultInfo("电芯温度采样线提示,CELL_TEMP_WIRE_ABNORMAL_TIPS",1,4,0,0,4),
                    new FaultInfo("保留,Res",1,5,0,0,4),
                    new FaultInfo("保留,Res",1,6,0,0,4),
                    new FaultInfo("保留,Res",1,7,0,0,4),
                    new FaultInfo("电芯压差大告警,BAT_CELL_UNBALANCE_ALM",2,0,0,0,1),
                    new FaultInfo("单体温差过大告警,CELL_TEMP_DIFF_OVER_ALM",2,1,0,0,1),
                    new FaultInfo("保留,Res",2,2,0,0,1),
                    new FaultInfo("保留,Res",2,3,0,0,1),
                    new FaultInfo("保留,Res",2,4,0,0,1),
                    new FaultInfo("保留,Res",2,5,0,0,1),
                    new FaultInfo("保留,Res",2,6,0,0,1),
                    new FaultInfo("保留,Res",2,7,0,0,1),
                    new FaultInfo("电池极限故障,FLT_CELL_VOLT_LIMIT",4,0,0,0,3),
        };

        //BCU报警信息应答消息（命令码 0x38）
        public static List<FaultInfo> FaultInfos4 = new List<FaultInfo>()
        {
            //type--类型1：无报警(data[0]=0)、严重报警(data[0]=1)、一般报警(data[0]=2)、轻微报警(data[0]=3)
            //type--类型2：设备硬件故障(data[0]=4)
 
                    new FaultInfo("供电电压欠压报警     ",1,0,0,0,1),
                    new FaultInfo("供电电压过压报警     ",1,1,0,0,1),
                    new FaultInfo("模块温度过温报警     ",1,2,0,0,1),
                    new FaultInfo("组端电压欠压报警     ",1,3,0,0,1),
                    new FaultInfo("组端电压过压报警     ",1,4,0,0,1),
                    new FaultInfo("快充电流过流报警     ",1,5,0,0,1),
                    new FaultInfo("动力插件过温报警     ",1,6,0,0,1),
                    new FaultInfo("馈电电流过流报警     ",1,7,0,0,1),
                    new FaultInfo("放电电流过流报警     ",2,0,0,0,1),
                    new FaultInfo("绝缘电阻过低报警     ",2,1,0,0,1),
                    new FaultInfo("SOC过低报警          ",2,2,0,0,1),
                    new FaultInfo("SOC过高报警          ",2,3,0,0,1),
                    new FaultInfo("单体电压过压报警     ",2,4,0,0,1),
                    new FaultInfo("单体电压欠压报警     ",2,5,0,0,1),
                    new FaultInfo("单体压差报警         ",2,6,0,0,1),
                    new FaultInfo("充电单体过温报警     ",2,7,0,0,1),
                    new FaultInfo("充电单体欠温报警     ",3,0,0,0,1),
                    new FaultInfo("放电单体过温报警     ",3,1,0,0,1),
                    new FaultInfo("放电单体欠温报警     ",3,2,0,0,1),
                    new FaultInfo("单体温差报警         ",3,3,0,0,1),
                    new FaultInfo("SOC差异过大报警      ",3,4,0,0,1),
                    new FaultInfo("温升快报警           ",3,5,0,0,1),
                    new FaultInfo("模组过压报警         ",3,6,0,0,1),
                    new FaultInfo("模组欠压报警         ",3,7,0,0,1),


                    new FaultInfo("供电电压欠压报警     ",1,0,1,0,1),
                    new FaultInfo("供电电压过压报警     ",1,1,1,0,1),
                    new FaultInfo("模块温度过温报警     ",1,2,1,0,1),
                    new FaultInfo("组端电压欠压报警     ",1,3,1,0,1),
                    new FaultInfo("组端电压过压报警     ",1,4,1,0,1),
                    new FaultInfo("快充电流过流报警     ",1,5,1,0,1),
                    new FaultInfo("动力插件过温报警     ",1,6,1,0,1),
                    new FaultInfo("馈电电流过流报警     ",1,7,1,0,1),
                    new FaultInfo("放电电流过流报警     ",2,0,1,0,1),
                    new FaultInfo("绝缘电阻过低报警     ",2,1,1,0,1),
                    new FaultInfo("SOC过低报警          ",2,2,1,0,1),
                    new FaultInfo("SOC过高报警          ",2,3,1,0,1),
                    new FaultInfo("单体电压过压报警     ",2,4,1,0,1),
                    new FaultInfo("单体电压欠压报警     ",2,5,1,0,1),
                    new FaultInfo("单体压差报警         ",2,6,1,0,1),
                    new FaultInfo("充电单体过温报警     ",2,7,1,0,1),
                    new FaultInfo("充电单体欠温报警     ",3,0,1,0,1),
                    new FaultInfo("放电单体过温报警     ",3,1,1,0,1),
                    new FaultInfo("放电单体欠温报警     ",3,2,1,0,1),
                    new FaultInfo("单体温差报警         ",3,3,1,0,1),
                    new FaultInfo("SOC差异过大报警      ",3,4,1,0,1),
                    new FaultInfo("温升快报警           ",3,5,1,0,1),
                    new FaultInfo("模组过压报警         ",3,6,1,0,1),
                    new FaultInfo("模组欠压报警         ",3,7,1,0,1),

                    new FaultInfo("供电电压欠压报警     ",1,0,2,0,1),
                    new FaultInfo("供电电压过压报警     ",1,1,2,0,1),
                    new FaultInfo("模块温度过温报警     ",1,2,2,0,1),
                    new FaultInfo("组端电压欠压报警     ",1,3,2,0,1),
                    new FaultInfo("组端电压过压报警     ",1,4,2,0,1),
                    new FaultInfo("快充电流过流报警     ",1,5,2,0,1),
                    new FaultInfo("动力插件过温报警     ",1,6,2,0,1),
                    new FaultInfo("馈电电流过流报警     ",1,7,2,0,1),
                    new FaultInfo("放电电流过流报警     ",2,0,2,0,1),
                    new FaultInfo("绝缘电阻过低报警     ",2,1,2,0,1),
                    new FaultInfo("SOC过低报警          ",2,2,2,0,1),
                    new FaultInfo("SOC过高报警          ",2,3,2,0,1),
                    new FaultInfo("单体电压过压报警     ",2,4,2,0,1),
                    new FaultInfo("单体电压欠压报警     ",2,5,2,0,1),
                    new FaultInfo("单体压差报警         ",2,6,2,0,1),
                    new FaultInfo("充电单体过温报警     ",2,7,2,0,1),
                    new FaultInfo("充电单体欠温报警     ",3,0,2,0,1),
                    new FaultInfo("放电单体过温报警     ",3,1,2,0,1),
                    new FaultInfo("放电单体欠温报警     ",3,2,2,0,1),
                    new FaultInfo("单体温差报警         ",3,3,2,0,1),
                    new FaultInfo("SOC差异过大报警      ",3,4,2,0,1),
                    new FaultInfo("温升快报警           ",3,5,2,0,1),
                    new FaultInfo("模组过压报警         ",3,6,2,0,1),
                    new FaultInfo("模组欠压报警         ",3,7,2,0,1),

                    new FaultInfo("供电电压欠压报警     ",1,0,3,0,1),
                    new FaultInfo("供电电压过压报警     ",1,1,3,0,1),
                    new FaultInfo("模块温度过温报警     ",1,2,3,0,1),
                    new FaultInfo("组端电压欠压报警     ",1,3,3,0,1),
                    new FaultInfo("组端电压过压报警     ",1,4,3,0,1),
                    new FaultInfo("快充电流过流报警     ",1,5,3,0,1),
                    new FaultInfo("动力插件过温报警     ",1,6,3,0,1),
                    new FaultInfo("馈电电流过流报警     ",1,7,3,0,1),
                    new FaultInfo("放电电流过流报警     ",2,0,3,0,1),
                    new FaultInfo("绝缘电阻过低报警     ",2,1,3,0,1),
                    new FaultInfo("SOC过低报警          ",2,2,3,0,1),
                    new FaultInfo("SOC过高报警          ",2,3,3,0,1),
                    new FaultInfo("单体电压过压报警     ",2,4,3,0,1),
                    new FaultInfo("单体电压欠压报警     ",2,5,3,0,1),
                    new FaultInfo("单体压差报警         ",2,6,3,0,1),
                    new FaultInfo("充电单体过温报警     ",2,7,3,0,1),
                    new FaultInfo("充电单体欠温报警     ",3,0,3,0,1),
                    new FaultInfo("放电单体过温报警     ",3,1,3,0,1),
                    new FaultInfo("放电单体欠温报警     ",3,2,3,0,1),
                    new FaultInfo("单体温差报警         ",3,3,3,0,1),
                    new FaultInfo("SOC差异过大报警      ",3,4,3,0,1),
                    new FaultInfo("温升快报警           ",3,5,3,0,1),
                    new FaultInfo("模组过压报警         ",3,6,3,0,1),
                    new FaultInfo("模组欠压报警         ",3,7,3,0,1),

                    new FaultInfo("DI1检测故障          ",1,0,4,0,2),
                    new FaultInfo("DI2检测故障          ",1,1,4,0,2),
                    new FaultInfo("DI3检测故障          ",1,2,4,0,2),
                    new FaultInfo("DI4检测故障          ",1,3,4,0,2),
                    new FaultInfo("DI5检测故障          ",1,4,4,0,2),
                    new FaultInfo("DI6检测故障          ",1,5,4,0,2),
                    new FaultInfo("DI7检测故障          ",1,6,4,0,2),
                    new FaultInfo("DI8检测故障          ",1,7,4,0,2),
                    new FaultInfo("内网通讯故障         ",2,0,4,0,2),
                    new FaultInfo("单体电压采集故障     ",2,1,4,0,2),
                    new FaultInfo("单体温度采集故障     ",2,2,4,0,2),
                    new FaultInfo("显控下发故障         ",2,3,4,0,2),
                    new FaultInfo("簇间压差故障         ",2,4,4,0,2),
                    new FaultInfo("跳机故障             ",2,5,4,0,2),
                    new FaultInfo("从控DO/DI检测故障    ",2,6,4,0,2),
                    new FaultInfo("电池极限故障         ",2,7,4,0,2),
                    new FaultInfo("程序和参数不一致故障 ",3,0,4,0,2),
                    new FaultInfo("PCS通讯故障          ",3,1,4,0,2),
                    new FaultInfo("PC强控调试模式       ",3,2,4,0,2),
                    new FaultInfo("CAN霍尔传感器故障    ",3,3,4,0,2),
                    new FaultInfo("CAN霍尔传感器通讯故障",3,4,4,0,2),
                    new FaultInfo("硬件自检异常         ",3,5,4,0,2),
                    new FaultInfo("极柱温度采集故障     ",3,6,4,0,2),
                    new FaultInfo("均衡故障             ",3,7,4,0,2),
                    new FaultInfo("预留                 ",4,0,4,0,2),
                    new FaultInfo("电流差异大告警(一级) ",4,1,4,0,2),
                    new FaultInfo("预留                 ",4,2,4,0,2),
                    new FaultInfo("预充超时             ",4,3,4,0,2),
                    new FaultInfo("脱扣前关闭风扇       ",4,4,4,0,2),
                    new FaultInfo("DBC 使能已开启       ",4,5,4,0,2),
                    new FaultInfo("正常下电失败         ",4,6,4,0,2),
                    new FaultInfo("接收到所有簇故障下电 ",4,7,4,0,2),
        };

        //BCU报警信息应答消息（命令码 0xE1）
        public static List<FaultInfo> FaultInfos4_new = new List<FaultInfo>()
        {
            new FaultInfo("高压采样故障,board_ext_adc_fault",1,0,0,0,4),
            new FaultInfo("内网编址异常,board_inner_can_addr_repeat_fault",1,1,0,0,4),
            new FaultInfo("afe异常故障,samp_invalid",1,2,0,0,4),
            new FaultInfo("存储异常故障,flash_err",1,3,0,0,4),
            new FaultInfo("严重过流锁死故障,Over_Curr_Lock",1,4,0,0,4),
            new FaultInfo("电芯严重过压锁死故障,BAT_CELL_VOLT_HIGH_SERIOUS_LOCK",1,5,0,0,4),
            new FaultInfo("保留,Res",1,6,0,0,4),
            new FaultInfo("保留,Res",1,7,0,0,4),
        };

        public static List<FaultInfo> FaultInfos1_new = new List<FaultInfo>()
        {
            new FaultInfo("电池簇内总压压差过大保护,bat_cluster_total_volt_diff_over_fault",1,0,0,0,1),
            new FaultInfo("过流失能,BAT_CURR_DISABLE",1,1,0,0,1),
            new FaultInfo("电池过压失能,BAT_VOLT_HIGH_DISABLE",1,2,0,0,1),
            new FaultInfo("电池欠压失能,BAT_VOLT_LOW_DISABLE",1,3,0,0,1),
            new FaultInfo("电池高低温失能,BAT_CELL_TEMP_OVER_OR_LOW_LOCK",1,4,0,0,1),
            new FaultInfo("保留,Res",1,5,0,0,1),
            new FaultInfo("保留,Res",1,6,0,0,1),
            new FaultInfo("保留,Res",1,7,0,0,1),
        };

        public static List<FaultInfo> FaultInfos2_new = new List<FaultInfo>()
        {
            new FaultInfo("地址冲突",1,0,0,0,2),
            new FaultInfo("保留,Res",1,1,0,0,2),
            new FaultInfo("保留,Res",1,2,0,0,2),
            new FaultInfo("保留,Res",1,3,0,0,2),
            new FaultInfo("保留,Res",1,4,0,0,2),
            new FaultInfo("保留,Res",1,5,0,0,2),
            new FaultInfo("保留,Res",1,6,0,0,2),
            new FaultInfo("保留,Res",1,7,0,0,2),
        };

        public static List<FaultInfo> FaultInfos3_new = new List<FaultInfo>()
        {
            new FaultInfo("afe断链,afe_chain_break",1,0,0,0,2),
            new FaultInfo("霍尔电流差异过大提示,board_hall_curr_diff_over_tip",1,1,0,0,2),
            new FaultInfo("环境温度采集失效提示,env_ntc_invalid",1,2,0,0,2),
            new FaultInfo("电解液漏液检测传感器异常提示,electrolyte_mon_sen_err_tip",1,3,0,0,2),
            new FaultInfo("保留,Res",1,4,0,0,2),
            new FaultInfo("保留,Res",1,5,0,0,2),
            new FaultInfo("保留,Res",1,6,0,0,2),
            new FaultInfo("保留,Res",1,7,0,0,2),
        };
    }
}
