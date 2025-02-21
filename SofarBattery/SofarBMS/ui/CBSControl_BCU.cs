using NPOI.SS.Formula.Functions;
using Sofar.ConnectionLibs.CAN.Driver.ECAN;
using SofarBMS.Helper;
using SofarBMS.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace SofarBMS.UI
{
    public partial class CBSControl_BCU : UserControl
    {
        public CBSControl_BCU()
        {
            InitializeComponent();

            cts = new CancellationTokenSource();
        }

        string[] packSN = new string[3];

        RealtimeData_CBS5000S_BCU model = null;

        public static CancellationTokenSource cts = null;
        private EcanHelper ecanHelper = EcanHelper.Instance;

        private void RTAControl_Load(object sender, EventArgs e)
        {
            //多语言翻译
            foreach (Control item in this.Controls)
            {
                GetControls(item);
            }
            Task.Run(async delegate
            {
                while (!cts.IsCancellationRequested)
                {
                    if (ecanHelper.IsConnection)
                    {
                        //if (model != null)
                        //{
                        //    var filePath = $"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}//Log//CBS5000_BCU{DateTime.Now.ToString("yyyy-MM-dd")}.csv";
                        //    if (!File.Exists(filePath))
                        //    {
                        //        File.AppendAllText(filePath, model.GetHeader() + "\r\n");
                        //    }
                        //    File.AppendAllText(filePath, model.GetValue() + "\r\n");
                        //    model = null;
                        //}

                        while (EcanHelper._task.Count > 0 && !cts.IsCancellationRequested)
                        {
                            CAN_OBJ ch = (CAN_OBJ)EcanHelper._task.Dequeue();

                            this.Invoke(new Action(() => { AnalysisData(ch.ID, ch.Data); }));
                        }

                        //一键操作标定参数
                        ecanHelper.Send(new byte[8] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }
                                       , new byte[] { 0xE0, FrmMain.BCU_ID, 0xF9, 0x10 });

                        await Task.Delay(500);

                        //上位机监控读取
                        ecanHelper.Send(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }
                                       , new byte[] { 0xE0, FrmMain.BCU_ID, 0xF7, 0x10 });

                        //定时一秒存储一次数据
                        await Task.Delay(500);
                    }
                }
            }, cts.Token);
        }


        public void AnalysisData(uint canID, byte[] data)
        {
            if ((canID & 0xff) != FrmMain.BCU_ID)
                return;

            if (model == null)
                model = new RealtimeData_CBS5000S_BCU();

            string[] strs;
            string[] controls;

            try
            {
                switch (canID | 0xff)
                {
                    //0x0B6:BCU遥信数据上报1--状态
                    case 0x10B6E0FF:
                        switch (data[0])
                        {
                            case 0x01:
                                var diTypeDic = new Dictionary<short, string>()
                                {
                                   {0, "pbExternalCANAddressingInputIOStatus" },
                                   {1, "pbContactorPositiveSwitchDetectionHighLevelClosure" },
                                   {2, "pbContactorNegativeSwitchDetectionHighLevelClosure" },
                                   {3, "pbDryContactInput1Channel" },
                                   {4, "pbADC1115_1Feedback1" },
                                   {5, "pbADC1115_1Feedback2" },
                                   {6, "pbChargingWakeUp" }
                                };

                                UpdateControlStatus(diTypeDic, Convert.ToUInt16(BytesToIntger(0x00, data[1])));
                                break;
                            case 0x02:
                                //继电器类型保留
                                break;
                            case 0x03:
                                var chgAndDischargeStateDic = new Dictionary<short, string>
                                {
                                   {0, "pbFanStatus"},
                                   {1, "pbChagreStatus"},
                                   {2, "pbDischargeStatus"},
                                   {3, "pbForceChargingEnable"},
                                   {4, "pbFullCharge"},
                                   {5, "pbEmpty"}
                                };

                                UpdateControlStatus(chgAndDischargeStateDic, Convert.ToUInt16(BytesToIntger(0x00, data[1])));
                                break;
                            case 0x04:
                                var chargBalancedStateDic = new Dictionary<short, string>
                                 {
                                     {0, "pbBatteryPack_1_BalancedState" },
                                     {1, "pbBatteryPack_2_BalancedState" },
                                     {2, "pbBatteryPack_3_BalancedState" },
                                     {3, "pbBatteryPack_4_BalancedState" },
                                     {4, "pbBatteryPack_5_BalancedState" },
                                     {5, "pbBatteryPack_6_BalancedState" },
                                     {6, "pbBatteryPack_7_BalancedState" },
                                     {7, "pbBatteryPack_8_BalancedState" },
                                     {8, "pbBatteryPack_9_BalancedState" },
                                     {9, "pbBatteryPack_10_BalancedState" },
                                     {10, "pbBatteryPack_11_BalancedState" },
                                     {11, "pbBatteryPack_12_BalancedState" },
                                     {12, "pbBatteryPack_13_BalancedState" },
                                     {13, "pbBatteryPack_14_BalancedState" },
                                     {14, "pbBatteryPack_15_BalancedState" },
                                     {15, "pbBatteryPack_16_BalancedState" },
                                 };

                                UpdateControlStatus(chargBalancedStateDic, Convert.ToUInt16(BytesToIntger(0x00, data[1])));
                                break;
                            case 0x05:
                                var dischargBalancedStateDic = new Dictionary<short, string>
                                 {
                                    {0, "pbBatteryPackBalancedState_1" },
                                    {1, "pbBatteryPackBalancedState_2" },
                                    {2, "pbBatteryPackBalancedState_3" },
                                    {3, "pbBatteryPackBalancedState_4" },
                                    {4, "pbBatteryPackBalancedState_5" },
                                    {5, "pbBatteryPackBalancedState_6" },
                                    {6, "pbBatteryPackBalancedState_7" },
                                    {7, "pbBatteryPackBalancedState_8" },
                                    {8, "pbBatteryPackBalancedState_9" },
                                    {9, "pbBatteryPackBalancedState_10" },
                                    {10, "pbBatteryPackBalancedState_11" },
                                    {11, "pbBatteryPackBalancedState_12" },
                                    {12, "pbBatteryPackBalancedState_13" },
                                    {13, "pbBatteryPackBalancedState_14" },
                                    {14, "pbBatteryPackBalancedState_15" },
                                    {15, "pbBatteryPackBalancedState_16" },
                                 };

                                UpdateControlStatus(dischargBalancedStateDic, Convert.ToUInt16(BytesToIntger(0x00, data[1])));
                                break;
                            default:
                                break;
                        }
                        break;
                    //0x0B7:BCU遥测数据上报1--温度采样数据
                    case 0x10B7E0FF:
                        switch (data[0])
                        {
                            case 0x01:
                                strs = new string[5] { "1", "1", "1", "1", "1" };
                                for (int i = 0; i < strs.Length; i++)
                                {
                                    strs[i] = BytesToIntger(0x00, data[i + 1]);
                                }

                                controls = new string[5] { "txtPower_Terminal_Temperature1", "txtPower_Terminal_Temperature2", "txtPower_Terminal_Temperature3", "txtPower_Terminal_Temperature4", "txtPower_Terminal_Temperature5" };
                                for (int i = 0; i < strs.Length; i++)
                                {
                                    (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                                }
                                break;
                            case 0xF0:
                                //模拟量数据1~7
                                break;
                            default:
                                break;
                        }
                        break;
                    //0x0B8:BCU遥测数据上报2--电压采样
                    case 0x10B8E0FF:
                        switch (data[0])
                        {
                            case 0x01:
                                strs = new string[3];
                                strs[0] = BytesToIntger(data[2], data[1], 0.1);
                                strs[1] = BytesToIntger(data[4], data[3], 0.1);
                                strs[2] = BytesToIntger(data[6], data[5], 0.1);

                                //电池采样电压1，母线采样电压，电池累加和电压（0.1V）
                                txtBattery_sampling_voltage1.Text = strs[0];
                                txtBus_sampling_voltage.Text = strs[1];
                                txtBattery_cumulative_sum_voltage.Text = strs[2];
                                break;
                            case 0x02:
                                strs = new string[2];
                                strs[0] = BytesToIntger(data[2], data[1], 0.1);
                                strs[1] = BytesToIntger(data[4], data[3], 0.1);

                                //加热膜供电电压，加热膜MOS电压（0.1V）
                                txtHeating_film_voltage.Text = strs[0];
                                txtHeating_film_MOSvoltage.Text = strs[1];

                                break;
                            case 0x03:
                                strs = new string[1];
                                strs[0] = BytesToIntger(data[2], data[1], 0.1);

                                //绝缘电阻电压（0.1V）
                                txtInsulation_resistance_voltage.Text = strs[0];
                                break;
                            case 0x04:
                                strs = new string[1];
                                strs[0] = BytesToIntger(data[2], data[1], 1);

                                //辅源电压（1mV）
                                txtAuxiliary_source_voltage.Text = strs[0];
                                break;
                            default:
                                break;
                        }
                        break;
                    //0x0B9:BCU遥测数据上报3---电流数据
                    case 0x10B9E0FF:
                        switch (data[0])
                        {
                            case 0x01:
                                strs = new string[1];
                                strs[0] = BytesToIntger(data[2], data[1], 0.1);

                                //簇电流（0.1A）
                                txtLoad_Voltage.Text = strs[0];
                                break;
                            default:
                                break;
                        }
                        break;
                    //0x0BA:BCU遥测数据上报4--簇最高最低单体电压信息
                    case 0x10BAE0FF:
                        strs = new string[6];
                        strs[0] = BytesToIntger(data[1], data[0], 0.001);
                        strs[1] = BytesToIntger(0x00, data[2]);
                        strs[2] = BytesToIntger(0x00, data[3]);
                        strs[3] = BytesToIntger(data[5], data[4], 0.001);
                        strs[4] = BytesToIntger(0x00, data[6]);
                        strs[5] = BytesToIntger(0x00, data[7]);

                        controls = new string[6] { "txtBat_Max_Cell_Volt", "txtBat_Max_Cell_VoltPack", "txtBat_Max_Cell_VoltNum", "txtBat_Min_Cell_Volt", "txtBat_Min_Cell_Volt_Pack", "txtBat_Min_Cell_Volt_Num" };
                        for (int i = 0; i < controls.Length; i++)
                        {
                            (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                        }

                        model.Bat_Max_Cell_Volt = Convert.ToUInt16(strs[0]);
                        model.Bat_Max_Cell_VoltPack = Convert.ToUInt16(strs[1]);
                        model.Bat_Max_Cell_VoltNum = Convert.ToUInt16(strs[2]);
                        model.Bat_Min_Cell_Volt = Convert.ToUInt16(strs[3]);
                        model.Bat_Min_Cell_Volt_Pack = Convert.ToUInt16(strs[4]);
                        model.Bat_Min_Cell_Volt_Num = Convert.ToUInt16(strs[5]);
                        break;
                    //0x0BB:BCU遥测数据上报5--簇最高最低单体温度信息
                    case 0x10BBE0FF:
                        strs = new string[6];
                        strs[0] = BytesToIntger(data[1], data[0], 0.1);
                        strs[1] = BytesToIntger(0x00, data[2]);
                        strs[2] = BytesToIntger(0x00, data[3]);
                        strs[3] = BytesToIntger(data[5], data[4], 0.1);
                        strs[4] = BytesToIntger(0x00, data[6]);
                        strs[5] = BytesToIntger(0x00, data[7]);

                        controls = new string[6] { "txtBat_Max_Cell_Temp", "txtBat_Max_Cell_Temp_Pack", "txtBat_Max_Cell_Temp_Num", "txtBat_Min_Cell_Temp", "txtBat_Min_Cell_Temp_Pack", "txtBat_Min_Cell_Temp_Num" };
                        for (int i = 0; i < controls.Length; i++)
                        {
                            (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                        }

                        model.Bat_Max_Cell_Temp = Convert.ToUInt16(strs[0]);
                        model.Bat_Max_Cell_Temp_Pack = Convert.ToUInt16(strs[1]);
                        model.Bat_Max_Cell_Temp_Num = Convert.ToUInt16(strs[2]);
                        model.Bat_Min_Cell_Temp = Convert.ToUInt16(strs[3]);
                        model.Bat_Min_Cell_Temp_Pack = Convert.ToUInt16(strs[4]);
                        model.Bat_Min_Cell_Temp_Num = Convert.ToUInt16(strs[5]);
                        break;
                    //0x0BC:BCU遥信数据2--簇充放电限流限压值
                    case 0x10BCE0FF:
                        strs = new string[4] { "0.1", "0.1", "0.1", "0.1" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                        }

                        controls = new string[4] { "txtBattery_Charge_Voltage", "txtCharge_Current_Limitation", "txtDischarge_Current_Limitation", "txtBattery_Discharge_Voltage" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                        }

                        model.Battery_Charge_Voltage = Convert.ToDouble(strs[0]);
                        model.Charge_Current_Limitation = Convert.ToDouble(strs[1]);
                        model.Discharge_Current_Limitation = Convert.ToDouble(strs[2]);
                        model.Battery_Discharge_Voltage = Convert.ToDouble(strs[3]);
                        break;
                    //0x0BD:BCU遥测数据2->>BCU版本号信息
                    case 0x10BDE0FF:
                        //BCU软件版本
                        string[] bcu_soft = new string[3];
                        for (int i = 0; i < 3; i++)
                        {
                            bcu_soft[i] = data[i + 1].ToString().PadLeft(2, '0');
                        }
                        txtSoftware_Version_BCU.Text = Encoding.ASCII.GetString(new byte[] { data[0] }) + string.Join("", bcu_soft);
                        //BCU硬件版本
                        string[] bsm_HW = new string[2];
                        for (int i = 0; i < 1; i++)
                        {
                            bsm_HW[i] = data[i + 4].ToString().PadLeft(2, '0');
                        }
                        txtHardware_Version_BCU.Text = string.Join("", bsm_HW);
                        //CAN协议版本号
                        txtCANprotocolVersion.Text = BytesToIntger(data[6], data[5]);

                        model.BCUSaftwareVersion = txtSoftware_Version_BCU.Text;
                        model.BCUHardwareVersion = txtHardware_Version_BCU.Text;
                        break;
                    //0x0BE:BCU遥信数据4--簇容量信息
                    case 0x10BEE0FF:
                        strs = new string[4] { "0.1", "0.1", "1", "1" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                        }

                        controls = new string[4] { "txtRemaining_Total_Capacity", "txtBat_Temp", "txtCluster_Rate_Power", "txtCycles" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                        }

                        model.Remaining_Total_Capacity = Convert.ToDouble(strs[0]);
                        model.Bat_Temp = Convert.ToDouble(strs[1]);
                        model.Cluster_Rate_Power = Convert.ToDouble(strs[2]);
                        model.Cycles = Convert.ToDouble(strs[3]);
                        break;
                    //0x0BF:BCU遥信数据5--簇基本信息
                    case 0x10BFE0FF:
                        switch (data[0])
                        {
                            case 0x01:
                                strs = new string[4];
                                strs[0] = BytesToIntger(0x00, data[1]);
                                strs[1] = BytesToIntger(0x00, data[2]);
                                strs[2] = BytesToIntger(0x00, data[3]);
                                strs[3] = BytesToIntger(0x00, data[4]);

                                //BCU状态，电池簇SOC，电池簇SOH，簇内电池包数量（U8,1）
                                controls = new string[4] { "txtBms_State", "txtCluster_SOC", "txtCluster_SOH", "txtPack_Num" };
                                for (int i = 0; i < controls.Length; i++)
                                {
                                    (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                                }

                                txtBms_State.Text = Enum.Parse(typeof(BMSState), (Convert.ToInt32(data[1].ToString("X2"), 16) & 0x0f).ToString()).ToString();
                                model.Bms_State = txtBms_State.Text;
                                model.Cluster_SOC = Convert.ToUInt16(strs[1]);
                                model.Cluster_SOH = Convert.ToUInt16(strs[2]);
                                model.Pack_Num = Convert.ToUInt16(strs[3]);
                                break;
                            default:
                                break;
                        }
                        break;
                    //0x0C0:BCU系统时间
                    case 0x10C0E0FF:
                        int[] numbers_bit = BytesToBit(data);
                        StringBuilder date = new StringBuilder();
                        date.Append(numbers_bit[0] + 2000);
                        date.Append("-");
                        date.Append(numbers_bit[1]);
                        date.Append("-");
                        date.Append(numbers_bit[2]);
                        date.Append(" ");
                        date.Append(numbers_bit[3]);
                        date.Append(":");
                        date.Append(numbers_bit[4]);
                        date.Append(":");
                        date.Append(numbers_bit[5]);
                        txtBCU_System_Time.Text = date.ToString();
                        break;
                    //0x0C1:BCU遥测数据上报3---其他数据
                    case 0x10C1E0FF:
                        switch (data[0])
                        {
                            case 0x01:
                                strs = new string[1];
                                strs[0] = BytesToIntger(data[2], data[1]);

                                //绝缘阻抗（1KΩ）
                                break;
                            default:
                                break;
                        }
                        break;
                    //0x0C2:测试结果/其他参数
                    case 0x10C2E0FF:
                        break;
                    //0x0C3:故障上报，0x0C4~0x0C5故障帧预留
                    case 0x10C3E0FF:
                        switch (data[0])
                        {
                            case 0x01:
                                richTextBox1.Clear();
                                analysisLog(new byte[] { data[1], data[2], data[3] }, 1);
                                break;
                            case 0x02:
                                richTextBox2.Clear();
                                analysisLog(new byte[] { data[1], data[2] }, 2);
                                break;
                            case 0x03:
                                richTextBox3.Clear();
                                analysisLog(new byte[] { data[1], data[2] }, 3);
                                break;
                            case 0x04:
                                richTextBox4.Clear();
                                analysisLog(new byte[] { data[1], data[2] }, 4);
                                break;
                            default:
                                break;
                        }
                        break;
                    //序列号
                    case 0x10F3E0FF:
                        string strSn = GetPackSN(data);

                        if (!string.IsNullOrEmpty(strSn))
                            txtSN.Text = strSn;
                        break;
                    //0x0C7:模拟量(DSP)
                    case 0x10C7E0FF:
                        switch (data[0])
                        {
                            case 0x01:
                                strs = new string[3];
                                strs[0] = BytesToIntger(data[2], data[1], 0.01);
                                strs[1] = BytesToIntger(data[4], data[3], 0.01);
                                strs[2] = BytesToIntger(data[6], data[5], 0.01);

                                //电感电流采样1~3（I16，0.1A）
                                controls = new string[3] { "txtInductive_current_sampling1", "txtInductive_current_sampling2", "txtInductive_current_sampling3" };
                                for (int i = 0; i < controls.Length; i++)
                                {
                                    (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                                }
                                break;
                            case 0x02:
                                strs = new string[3];
                                strs[0] = BytesToIntger(data[2], data[1], 0.01);
                                strs[1] = BytesToIntger(data[4], data[3], 0.01);
                                strs[2] = BytesToIntger(data[6], data[5], 0.01);

                                //电感电流采样4，充电/放电大电流采样（I16，0.1A）
                                controls = new string[3] { "txtInductive_current_sampling4", "txtDischarge_high_current_sampling", "txtCharge_high_current_sampling" };
                                for (int i = 0; i < controls.Length; i++)
                                {
                                    (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                                }
                                break;
                            case 0x03:
                                strs = new string[3];
                                strs[0] = BytesToIntger(data[2], data[1], 0.1);
                                strs[1] = BytesToIntger(data[4], data[3], 0.1);
                                strs[2] = BytesToIntger(data[6], data[5], 0.1);

                                //接触器电压，电池簇电压1，高压母线电压（U16，0.1V）
                                controls = new string[3] { "txtContactor_voltage", "txtBattery_cluster_voltage1", "txtHigh_voltage_bus_voltage" };
                                for (int i = 0; i < controls.Length; i++)
                                {
                                    (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                                }
                                break;
                            case 0x04:
                                strs = new string[3];
                                strs[0] = BytesToIntger(data[2], data[1], 0.1);
                                strs[1] = BytesToIntger(data[4], data[3], 0.1);

                                //加热膜供电电压，电池电压采样2（U16，0.1V）
                                controls = new string[2] { "txtHeating_film_power_supply_voltage", "txtBattery_sampling_voltage2" };
                                for (int i = 0; i < controls.Length; i++)
                                {
                                    (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                                }
                                break;
                            case 0x05:
                                strs = new string[4];
                                strs[0] = (sbyte)(data[1] & 0xFF) + "";
                                strs[1] = (sbyte)(data[2] & 0xFF) + "";
                                strs[2] = (sbyte)(data[3] & 0xFF) + "";
                                strs[3] = (sbyte)(data[4] & 0xFF) + "";

                                //温度1~4（单位℃，无效值为0x80）
                                controls = new string[4] { "txtRadiator_temperature1", "txtRadiator_temperature2", "txtRadiator_temperature3", "txtRadiator_temperature4" };
                                for (int i = 0; i < strs.Length; i++)
                                {
                                    (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                                }
                                break;
                            case 0x06:

                                //软件版本号，硬件版本号，SCI协议版本号
                                string[] softwareVersion = new string[3];
                                for (int i = 0; i < 3; i++)
                                {
                                    softwareVersion[i] = data[i + 2].ToString().PadLeft(2, '0');
                                }
                                txtSoftware_Version_DCDC.Text = Encoding.ASCII.GetString(new byte[] { data[1] }) + string.Join("", softwareVersion);

                                //BCU硬件版本
                                string[] dcdc_HW = new string[2];
                                for (int i = 0; i < 1; i++)
                                {
                                    dcdc_HW[i] = data[i + 5].ToString().PadLeft(2, '0');
                                }
                                txtHardware_Version_DCDC.Text = string.Join("", dcdc_HW);
                                //CAN协议版本号
                                txtSCIprotocolVersion.Text = BytesToIntger(data[7], data[6]);
                                break;
                            default:
                                break;
                        }
                        break;
                    //0x0C8:遥信数据(DSP)
                    case 0x10C8E0FF:
                        switch (data[0])
                        {
                            case 0x01:
                                strs = new string[2];
                                strs[0] = BytesToIntger(0x00, data[1]);
                                strs[1] = BytesToIntger(0x00, data[2]);

                                object? workState = "", dcdcState = "";

                                if (Enum.TryParse(typeof(WorkState_DCDC), strs[0], out workState))
                                {
                                    txtWorkState.Text = Enum.Parse(typeof(WorkState_DCDC), strs[0]).ToString();
                                }

                                if (Enum.TryParse(typeof(BatteryState_DCDC), strs[1], out dcdcState))
                                {
                                    txtDCDC_State.Text = Enum.Parse(typeof(BatteryState_DCDC), strs[1]).ToString();
                                }
                                break;
                            case 0x02:
                                richTextBox5.Clear();
                                analysisLog(new byte[] { data[1], data[2], data[3] }, 1, "DCDC");
                                break;
                            case 0x03: break;
                            default:
                                break;
                        }
                        break;
                    default: break;
                }

                model.PackID = FrmMain.BCU_ID.ToString("X2");
            }
            catch (Exception)
            {

            }
        }

        public void UpdateControlStatus(List<Dictionary<short, string>> statusByteList, ushort value)
        {
            foreach (var statusByte in statusByteList)
            {
                foreach (var kvp in statusByte)
                {
                    short bitIndex = kvp.Key;
                    string controlName = kvp.Value;
                    var control = this.Controls.Find(controlName, true).FirstOrDefault() as PictureBox;
                    if (control != null)
                    {
                        control.BackColor = GetBit((byte)value, bitIndex) == 0 ? Color.Red : Color.Green;
                    }
                }
            }
        }

        public void UpdateControlStatus(Dictionary<short, string> statusByteList, ushort value)
        {
            foreach (var kvp in statusByteList)
            {
                short bitIndex = kvp.Key;
                string controlName = kvp.Value;
                var control = this.Controls.Find(controlName, true).FirstOrDefault() as PictureBox;
                if (control != null)
                {
                    control.BackColor = GetBit((byte)value, bitIndex) == 0 ? Color.Red : Color.Green;
                }
            }
        }

        private string GetPackSN(byte[] data)
        {
            //条形码解析
            switch (data[0])
            {
                case 0:
                    packSN[0] = Encoding.Default.GetString(data).Substring(1);
                    break;
                case 1:
                    packSN[1] = Encoding.Default.GetString(data).Substring(1);
                    break;
                case 2:
                    packSN[2] = Encoding.Default.GetString(data).Substring(1);
                    break;
            }

            //判断sn是否接收完成
            if (packSN[0] != null && packSN[1] != null && packSN[2] != null)
            {
                string strSN = String.Join("", packSN);
                return strSN.Substring(0, strSN.Length - 1);
            }
            else
            {
                return string.Empty;
            }
        }

        public List<int> GetEventList(byte[] responsedata)
        {
            List<int> lists = new List<int>();

            byte[] table = { 0x01, 0x02, 0x04, 0x8, 0x10, 0x20, 0x40, 0x80 };

            int cnt = 0;
            for (int i = 0; i < responsedata.Length; i++)
            {
                if (responsedata[i] != 0x00)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if ((table[j] & responsedata[i]) == table[j])
                        {
                            lists.Add(j + cnt + 1);
                        }
                    }
                }
                cnt += 8;
            }
            return lists;
        }


        private int[] BytesToBit(byte[] data)
        {
            int[] numbers = new int[8];
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = Convert.ToInt32(data[i].ToString("X2"), 16);
            }
            return numbers;
        }

        private string BytesToIntger(byte high, byte low = 0x00, double unit = 1)
        {
            string value = Convert.ToInt16(high.ToString("X2") + low.ToString("X2"), 16).ToString();
            if (unit == 10)
            {
                value = (long.Parse(value) * 10).ToString();
            }
            else if (unit == 0.1)
            {
                value = (long.Parse(value) / 10.0f).ToString();
            }
            else if (unit == 0.01)
            {
                value = (long.Parse(value) / 100.0f).ToString();
            }
            else if (unit == 0.001)
            {
                value = (long.Parse(value) / 1000.0f).ToString();
            }
            else if (unit == 0.0001)
            {
                value = (long.Parse(value) / 10000).ToString();
            }

            return value;
        }

        #region 翻译所用得函数
        private void GetControls(Control c)
        {
            if (c is GroupBox || c is TabControl)
            {
                c.Text = LanguageHelper.GetLanguage(c.Name.Remove(0, 2));

                foreach (Control item in c.Controls)
                {
                    this.GetControls(item);
                }
            }
            else
            {
                string name = c.Name;

                if (c is CheckBox)
                {
                    c.Text = LanguageHelper.GetLanguage(name.Remove(0, 2));

                    LTooltip(c as CheckBox, c.Text);
                }
                else if (c is RadioButton)
                {
                    c.Text = LanguageHelper.GetLanguage(name.Remove(0, 2));

                    LTooltip(c as RadioButton, c.Text);
                }
                else if (c is Label)
                {
                    c.Text = LanguageHelper.GetLanguage(name.Remove(0, 3));

                    LTooltip(c as Label, c.Text);
                }
                else if (c is Button)
                {
                    if (name.Contains("Set"))
                    {
                        c.Text = LanguageHelper.GetLanguage("Settings");
                    }
                    else if (name.Contains("_Close"))
                    {
                        c.Text = LanguageHelper.GetLanguage("Systemset_43");
                    }
                    else if (name.Contains("_Open"))
                    {
                        c.Text = LanguageHelper.GetLanguage("Systemset_44");
                    }
                    else if (name.Contains("_Lifted"))
                    {
                        c.Text = LanguageHelper.GetLanguage("Systemset_45");
                    }
                    else
                    {
                        c.Text = LanguageHelper.GetLanguage(name.Remove(0, 3));

                    }
                }
                else if (c is TabPage | c is Panel)
                {
                    foreach (Control item in c.Controls)
                    {
                        this.GetControls(item);
                    }
                }
            }
        }

        public static void LTooltip(System.Windows.Forms.Control control, string value)
        {
            if (value.Length == 0) return;
            control.Text = Abbreviation(control, value);
            var tip = new ToolTip();
            tip.IsBalloon = false;
            tip.ShowAlways = true;
            tip.SetToolTip(control, value);
        }

        public static string Abbreviation(Control control, string str)
        {
            if (str == null)
            {
                return null;
            }
            int strWidth = FontWidth(control.Font, control, str);

            //获取label最长可以显示多少字符
            int len = control.Width * str.Length / strWidth;

            if (len > 20 && len < str.Length)

            {
                return str.Substring(0, 20) + "...";
            }
            else
            {
                return str;
            }
        }

        private static int FontWidth(Font font, Control control, string str)
        {
            using (Graphics g = control.CreateGraphics())
            {
                SizeF siF = g.MeasureString(str, font);
                return (int)siF.Width;
            }
        }
        #endregion

        #region BMS发送内部电池故障信息-模块处理
        /// <summary>
        /// 解析日志内容
        /// </summary>
        /// <param name="_bytes"></param>
        /// <returns></returns>
        private void analysisLog1(byte[] data, int faultNum)
        {
            string[] msg = new string[2];

            for (int i = 0; i < data.Length; i++)
            {
                for (short j = 0; j < 8; j++)
                {
                    if (GetBit(data[i], j) == 1)
                    {
                        getLog(out msg, i, j, faultNum);
                        string type = "";
                        //if (faultNum == 2)
                        //{
                        //    switch (msg[1])
                        //    {
                        //        case "1":
                        //            richTextBox6.AppendText(msg[0] + "\r");
                        //            model.Warning2 = richTextBox6.Text.Replace("\n", "，").Replace("\r", "，");
                        //            type = "告警";
                        //            break;
                        //        case "2":
                        //            richTextBox5.AppendText(msg[0] + "\r");
                        //            model.Protection2 = richTextBox5.Text.Replace("\n", "，").Replace("\r", "，");
                        //            type = "保护";
                        //            break;
                        //        case "3":
                        //            richTextBox4.AppendText(msg[0] + "\r");
                        //            model.Fault2 = richTextBox4.Text.Replace("\n", "，").Replace("\r", "，");
                        //            type = "故障";
                        //            break;
                        //    }
                        //}
                        //else
                        {
                            switch (msg[1])
                            {
                                case "1":
                                    richTextBox4.AppendText(msg[0] + "\r");
                                    model.Warning = richTextBox4.Text.Replace("\n", "，").Replace("\r", "，");
                                    type = "告警";
                                    break;
                                case "2":
                                    richTextBox2.AppendText(msg[0] + "\r");
                                    model.Protection = richTextBox2.Text.Replace("\n", "，").Replace("\r", "，");
                                    type = "保护";
                                    break;
                                case "3":
                                    richTextBox1.AppendText(msg[0] + "\r");
                                    model.Fault = richTextBox1.Text.Replace("\n", "，").Replace("\r", "，");
                                    type = "故障";
                                    break;
                            }
                        }

                        var query = FrmMain.AlarmList.FirstOrDefault(t => t.Id == FrmMain.BMS_ID && t.Content == "BCU:" + msg[0]);
                        if (query == null)
                        {
                            FrmMain.AlarmList.Add(new AlarmInfo()
                            {
                                DataTime = DateTime.Now.ToString("yy-MM-dd HH:mm:ss"),
                                Id = FrmMain.BMS_ID,
                                Type = type,
                                Content = $"BCU:{msg[0]}"
                            });
                        }
                    }
                    else
                    {
                        getLog(out msg, i, j, faultNum);
                        string type = "";
                        switch (msg[1])
                        {
                            case "1":
                                type = "告警";
                                break;
                            case "2":
                                type = "保护";
                                break;
                            case "3":
                                type = "故障";
                                break;
                        }

                        var query = FrmMain.AlarmList.FirstOrDefault(t => t.Id == FrmMain.BMS_ID && t.Type == type && t.Content == "BMU:" + msg[0] && t.State == 0);
                        if (query != null)
                        {
                            query.State = 1;
                            FrmMain.AlarmList.Add(new AlarmInfo()
                            {
                                DataTime = DateTime.Now.ToString("yy-MM-dd HH:mm:ss"),
                                State = 1,
                                Id = FrmMain.BMS_ID,
                                Type = type,
                                Content = $"[解除]BCU:{msg[0]}"
                            });
                        }
                    }
                }
            }
        }

        private void analysisLog(byte[] data, int faultNum, string devType = "CBS5000")
        {
            string[] msg = new string[2];

            for (int i = 0; i < data.Length; i++)
            {
                for (short j = 0; j < 8; j++)
                {
                    if (GetBit(data[i], j) == 1)
                    {
                        getLog(out msg, i + 1, j, faultNum, devType);
                        string type = "";
                        switch (faultNum)
                        {
                            case 1:
                                if (devType == "CBS5000")
                                {
                                    richTextBox1.AppendText(msg[0] + "\r");
                                }
                                else
                                {

                                    richTextBox5.AppendText(msg[0] + "\r");
                                }

                                //model.Warning = richTextBox1.Text.Replace("\n", "，").Replace("\r", "，");
                                type = "故障";
                                break;
                            case 2:
                                richTextBox2.AppendText(msg[0] + "\r");
                                //model.Protection = richTextBox2.Text.Replace("\n", "，").Replace("\r", "，");
                                type = "保护";
                                break;
                            case 3:
                                richTextBox3.AppendText(msg[0] + "\r");
                                //model.Fault = richTextBox3.Text.Replace("\n", "，").Replace("\r", "，");
                                type = "告警";
                                break;
                            case 4:
                                richTextBox4.AppendText(msg[0] + "\r");
                                //model.Fault = richTextBox4.Text.Replace("\n", "，").Replace("\r", "，");
                                type = "提示";
                                break;
                            default:
                                break;
                        }

                        var query = FrmMain.AlarmList.FirstOrDefault(t => t.Id == FrmMain.BMS_ID && t.Content == "BCU:" + msg[0]);
                        if (query == null)
                        {
                            FrmMain.AlarmList.Add(new AlarmInfo()
                            {
                                DataTime = DateTime.Now.ToString("yy-MM-dd HH:mm:ss"),
                                Id = FrmMain.BMS_ID,
                                Type = type,
                                Content = $"BCU:{msg[0]}"
                            });
                        }
                    }
                    else
                    {
                        getLog(out msg, i + 1, j, faultNum);
                        string type = "";
                        switch (msg[1])
                        {
                            case "1":
                                type = "告警";
                                break;
                            case "2":
                                type = "保护";
                                break;
                            case "3":
                                type = "故障";
                                break;
                        }

                        var query = FrmMain.AlarmList.FirstOrDefault(t => t.Id == FrmMain.BMS_ID && t.Type == type && t.Content == "BMU:" + msg[0] && t.State == 0);
                        if (query != null)
                        {
                            query.State = 1;
                            FrmMain.AlarmList.Add(new AlarmInfo()
                            {
                                DataTime = DateTime.Now.ToString("yy-MM-dd HH:mm:ss"),
                                State = 1,
                                Id = FrmMain.BMS_ID,
                                Type = type,
                                Content = $"[解除]BCU:{msg[0]}"
                            });
                        }
                    }
                }
            }
        }
        private string[] getLog(out string[] msg, int row, int column, int faultNum = -1, string devType = "CBS5000")
        {
            msg = new string[2];
            List<FaultInfo> faultInfos = new List<FaultInfo>();
            switch (faultNum)
            {
                case 1:
                    faultInfos = FaultNotify;
                    if (devType != "CBS5000")
                    {
                        faultInfos = FaultNotify_DCDC;
                    }
                    break;
                case 2:
                    faultInfos = ProtectNotify;
                    break;
                case 3:
                    faultInfos = WarningNotify;
                    break;
                case 4:
                    faultInfos = PromptNotify;
                    break;
                default: break;
            }

            for (int i = 0; i < faultInfos.Count; i++)
            {
                if (faultInfos[i].Byte == row && faultInfos[i].Bit == column)
                {
                    int index = LanguageHelper.LanaguageIndex;
                    msg[0] = faultInfos[i].Content.Split(',')[index - 1];
                    msg[1] = faultInfos[i].Type.ToString();
                    break;
                }
            }
            return msg;
        }

        /// <summary>
        /// 获取字节中的指定Bit的值
        /// </summary>
        /// <param name="b">字节</param>
        /// <param name="index">Bit的索引值(0-7)</param>
        /// <returns></returns>
        public static int GetBit(byte b, short index)
        {
            byte _byte = 0x01;
            switch (index)
            {
                case 0: { _byte = 0x01; } break;
                case 1: { _byte = 0x02; } break;
                case 2: { _byte = 0x04; } break;
                case 3: { _byte = 0x08; } break;
                case 4: { _byte = 0x10; } break;
                case 5: { _byte = 0x20; } break;
                case 6: { _byte = 0x40; } break;
                case 7: { _byte = 0x80; } break;
                default: { return 0; }
            }
            int x = (b & _byte) == _byte ? 1 : 0;

            return (b & _byte) == _byte ? 1 : 0;
        }

        public static string[] getLog1(out string[] msg, int row, int column, int faultNum = 0)
        {
            msg = new string[2];
            List<FaultInfo> faultInfos = FrmMain.FaultInfos;
            switch (faultNum)
            {
                case 0:
                    faultInfos = FrmMain.FaultInfos;// 0x08:BMS发送内部电池故障信息1     BCU 0x0C5
                    break;
                case 1:
                    faultInfos = FrmMain.FaultInfos2;//0x45:BMS发送内部电池故障信息2     BCU 0x0C6
                    break;
                case 2:
                    faultInfos = FrmMain.FaultInfos3;//0x0C3:BCU故障上报1
                    break;
            }

            for (int i = 0; i < faultInfos.Count; i++)
            {
                if (faultInfos[i].Byte == row && faultInfos[i].Bit == column)
                {
                    int index = LanguageHelper.LanaguageIndex;
                    msg[0] = faultInfos[i].Content.Split(',')[index - 1];
                    msg[1] = faultInfos[i].Type.ToString();
                    break;
                }
            }
            return msg;
        }
        #endregion

        public List<FaultInfo> FaultNotify = new List<FaultInfo>() {
                    new FaultInfo("过流失能,Clu Over_Curr_Disable",1,0,0,0,1),
                    new FaultInfo("充电严重过流锁死(BCU),Chg_Over_Curr_Lock",1,1,0,0,1),
                    new FaultInfo("放电严重过流锁死(BCU),Dsg_Over_Curr_Lock",1,2,0,0,1),
                    new FaultInfo("电芯严重过压锁死(BCU),Cell_Over_Volt_Lock",1,3,0,0,1),
                    new FaultInfo("电流大环零点不良(BCU),BOARD_CURR_BIG_RING_BADNESS",1,4,0,0,1),
                    new FaultInfo("电流小环零点不良(BCU),BOARD_CURR_LITTLE_RING_BADNESS",1,5,0,0,1),
                    new FaultInfo("NTC开路(BCU),BOARD_NTC_OPEN_CIRCUIT",1,6,0,0,1),
                    new FaultInfo("NTC短路(BCU),BOARD_NTC_SHORT_CIRCUIT",1,7,0,0,1),
                    new FaultInfo("簇电压压差过大(BCU),BOARD_TOTAL_VOLT_DIFF_OVER",2,0,0,0,1),
                    new FaultInfo("辅源异常(BCU),BOARD_VOLT_9V_ERR",2,1,0,0,1),
                    new FaultInfo("flash存储错误(BCU),BOARD_FLASH_ERR",2,2,0,0,1),
                    new FaultInfo("功率端子过温失能(BCU),BOARD_POWER_TERMINAL_TEMP_OVER_LOCK",2,3,0,0,1),
                    new FaultInfo("主回路保险丝熔断(BCU),BOARD_MAIN_CIRCUIT_BLOWN_FUSE",2,4,0,0,1),
                    new FaultInfo("正继电器粘连(BCU),BOARD_POSITIVE_RELAY_ADHESION",2,5,0,0,1),
                    new FaultInfo("负继电器粘连(BCU),BOARD_NEGATIVE_RELAY_ADHESION",2,6,0,0,1),
                    new FaultInfo("绝缘异常(BCU),BOARD_INSUALATION_FAULT",2,7,0,0,1),
                    new FaultInfo("内can编址失败(BCU),BOARD_INNER_CAN_ADDR_FAULT",3,0,0,0,1),
                    new FaultInfo("电池包数目异常(BCU),BOARD_PACK_NUM_FAULT",3,1,0,0,1),
                    new FaultInfo("采样异常(BCU),BOARD_SAMPLE_FAULT",3,2,0,0,1),
                    new FaultInfo("保留,bit res3",3,3,0,0,1),
                    new FaultInfo("预充异常(BCU),BOARD_PRE_CHARGE_FAULT",3,4,0,0,1),
                    new FaultInfo("加热膜回路故障,BOARD_HEAT_CIR_ERR",3,5,0,0,1),
                    new FaultInfo("CC检测故障,Connect Check",3,6,0,0,1),
                    new FaultInfo("保留,bit res7",3,7,0,0,1)
        };
        public List<FaultInfo> ProtectNotify = new List<FaultInfo>()
        {
                    new FaultInfo("电池簇电压过低保护(BCU),Clu_Low_Voltage_Protec",1,0,0,0,2),
                    new FaultInfo("电池簇电压过高保护(BCU),Clu_High_Voltage_Protect",1,1,0,0,2),
                    new FaultInfo("电池簇充电过流保护(BCU),Clu_Chg_Over_Currr_Prot",1,2,0,0,2),
                    new FaultInfo("电池簇放电过流保护(BCU),Clu_Dsg_Over_Currr_Prot",1,3,0,0,2),
                    new FaultInfo("保留,res",1,4,0,0,2),
                    new FaultInfo("电池簇放电过流保护2(BCU),Clu_Dsg_Over_Currr_Prot2",1,5,0,0,2),
                    new FaultInfo("保留,res",1,6,0,0,2),
                    new FaultInfo("BCU与BMU间CAN通讯故障(BCU),BOARD_BCU_BMU_CAN_COMM_ERR",1,7,0,0,2),
                    new FaultInfo("功率端子过温保护(BCU),BOARD_POWER_TERMINAL_TEMP_OVER_PROTECT",2,0,0,0,2),
                    new FaultInfo("保留,res",2,1,0,0,2),
                    new FaultInfo("保留,res",2,2,0,0,2),
                    new FaultInfo("保留,res",2,3,0,0,2),
                    new FaultInfo("保留,res",2,4,0,0,2),
                    new FaultInfo("保留,res",2,5,0,0,2),
                    new FaultInfo("保留,res",2,6,0,0,2),
                    new FaultInfo("保留,res",2,7,0,0,2)
        };
        public List<FaultInfo> WarningNotify = new List<FaultInfo>()
        {
                    new FaultInfo("电池簇电压过低告警(BCU),Clu Low Voltage Alm",1,0,0,0,3),
                    new FaultInfo("电池簇电压过高告警(BCU),Clu High Voltage Alm",1,1,0,0,3),
                    new FaultInfo("电池簇充电过流告警(BCU),Clu_Chg_Over_Currr Alm",1,2,0,0,3),
                    new FaultInfo("电池簇放电过流告警(BCU),Clu_Dsg_Over_Currr Alm",1,3,0,0,3),
                    new FaultInfo("SOC过低告警(BCU),Soc_low_Alm",1,4,0,0,3),
                    new FaultInfo("保留,Res bit5",1,5,0,0,3),
                    new FaultInfo("保留,Res bit6",1,6,0,0,3),
                    new FaultInfo("保留,Res bit7",1,7,0,0,3),
                    new FaultInfo("保留,Res bit0",2,0,0,0,3),
                    new FaultInfo("BCU与BCU间外网CAN地址冲突(BCU),BOARD_BCU_BCU_CANID_CONFLICT",2,1,0,0,3),
                    new FaultInfo("外can编址失败(BCU),BOARD_EXT_CAN_ADDR_FAULT",2,2,0,0,3),
                    new FaultInfo("保留,Res bit3",2,3,0,0,3),
                    new FaultInfo("BCU与DCDC间CAN通讯故障(BCU),BOARD_BCU_DCDC_CAN_COMM_ERR",2,4,0,0,3),
                    new FaultInfo("保留,Res bit5",2,5,0,0,3),
                    new FaultInfo("保留,Res bit6",2,6,0,0,3),
                    new FaultInfo("pcs与bcu通讯故障,BOARD_BCU_PCS_CAN_COMM_ERR",2,7,0,0,3)
        };
        public List<FaultInfo> PromptNotify = new List<FaultInfo>()
        {
                    new FaultInfo("加热继电器粘连(BCU),BOARD_HEAT_RELAY_ADHESION",1,0,0,0,4),
                    new FaultInfo("加热功率异常(BCU),BOARD_HEAT_POWER_ERR",1,1,0,0,4),
                    new FaultInfo("加热长时间无功率(BCU),BOARD_HEAT_POWER_NULL",1,2,0,0,4),
                    new FaultInfo("加热单次超时(BCU),BOARD_HEAT_OVER_TIME",1,3,0,0,4),
                    new FaultInfo("加热膜阻值异常(BCU),BOARD_HEAT_RES_ERR",1,4,0,0,4),
                    new FaultInfo("加热MOS异常,BOARD_HEAT_MOS_ERR",1,5,0,0,4),
                    new FaultInfo("加热保险丝异常,BOARD_HEAT_FUSE_ERR",1,6,0,0,4),
                    new FaultInfo("保留,Res bit7",1,7,0,0,4),
                    new FaultInfo("单板过温告警,BOARD_BCU_TEMP_OVER_ALM",2,0,0,0,4),
                    new FaultInfo("单板低温告警,BOARD_BCU_TEMP_UNDER_ALM",2,1,0,0,4),
                    new FaultInfo("BMU异常关机告警(BCU),BOARD_BMU_ABNORMAL_SLEEP_ALARM",2,2,0,0,4),
                    new FaultInfo("保留,Res bit3",2,3,0,0,4),
                    new FaultInfo("保留,Res bit4",2,4,0,0,4),
                    new FaultInfo("保留,Res bit5",2,5,0,0,4),
                    new FaultInfo("保留,Res bit6",2,6,0,0,4),
                    new FaultInfo("保留,Res bit7",2,7,0,0,4)
        };



        public List<FaultInfo> FaultNotify_DCDC = new List<FaultInfo>() {
                    new FaultInfo("电感电流瞬时过流保护,BYTE1>>bit0",1,0,0,0,1),
                    new FaultInfo("电感电流CBC过流保护,BYTE1>>bit1",1,1,0,0,1),
                    new FaultInfo("放电电流瞬时过流保护,BYTE1>>bit2",1,2,0,0,1),
                    new FaultInfo("放电电流CBC过流保护,BYTE1>>bit3",1,3,0,0,1),
                    new FaultInfo("充电电流瞬时过流保护,BYTE1>>bit4",1,4,0,0,1),
                    new FaultInfo("充电电流CBC过流保护,BYTE1>>bit5",1,5,0,0,1),
                    new FaultInfo("电池电压瞬时过压保护,BYTE1>>bit6",1,6,0,0,1),
                    new FaultInfo("电池电压瞬时欠压保护,BYTE1>>bit7",1,7,0,0,1),
                    new FaultInfo("电池电压硬件过压保护,BYTE2>>bit0",2,0,0,0,1),
                    new FaultInfo("母线电压瞬时过压保护,BYTE2>>bit1",2,1,0,0,1),
                    new FaultInfo("母线电压瞬时欠压保护,BYTE2>>bit2",2,2,0,0,1),
                    new FaultInfo("母线短路保护,BYTE2>>bit3",2,3,0,0,1),
                    new FaultInfo("BCU-COM过压过流故障,BYTE2>>bit4",2,4,0,0,1),
                    new FaultInfo("预留,BYTE2>>bit5",2,5,0,0,1),
                    new FaultInfo("预留,BYTE2>>bit6",2,6,0,0,1),
                    new FaultInfo("预留,BYTE2>>bit7",2,7,0,0,1),
                    new FaultInfo("接触器短路故障,BYTE3>>bit0",3,0,0,0,1),
                    new FaultInfo("接触器断路故障,BYTE3>>bit1",3,1,0,0,1),
                    new FaultInfo("电流采样错误,BYTE3>>bit2",3,2,0,0,1),
                    new FaultInfo("电感电流平均值过流保护,BYTE3>>bit3",3,3,0,0,1),
                    new FaultInfo("放电电流平均值过流保护,BYTE3>>bit4",3,4,0,0,1),
                    new FaultInfo("充电电流平均值过流保护,BYTE3>>bit5",3,5,0,0,1),
                    new FaultInfo("电池电压平均值过压保护,BYTE3>>bit6",3,6,0,0,1),
                    new FaultInfo("电池电压平均值欠压保护,BYTE3>>bit7",3,7,0,0,1),
                    new FaultInfo("母线电压平均值过压保护,BYTE4>>bit0",4,0,0,0,1),
                    new FaultInfo("母线电压平均值欠压保护,BYTE4>>bit1",4,1,0,0,1),
                    new FaultInfo("Can通信故障,BYTE4>>bit2",4,2,0,0,1),
                    new FaultInfo("SCI通信故障,BYTE4>>bit3",4,3,0,0,1),
                    new FaultInfo("环境温度过温保护,BYTE4>>bit4",4,4,0,0,1),
                    new FaultInfo("散热器温度过温保护,BYTE4>>bit5",4,5,0,0,1),
                    new FaultInfo("硬件版本错误保护,BYTE4>>bit6",4,6,0,0,1),
                    new FaultInfo("充电电流监测保护,BYTE4>>bit7",4,7,0,0,1),
                    new FaultInfo("电感电流平均值过载保护,BYTE5>>bit0",5,0,0,0,1),
                    new FaultInfo("放电电流平均值过载保护,BYTE5>>bit1",5,1,0,0,1),
                    new FaultInfo("母线功率端子P+过温保护,BYTE5>>bit2",5,2,0,0,1),
                    new FaultInfo("母线功率端子P-过温保护,BYTE5>>bit3",5,3,0,0,1),
                    new FaultInfo("堆叠连接器端子过温保护,BYTE5>>bit4",5,4,0,0,1),
                    new FaultInfo("预留,BYTE5>>bit5",5,5,0,0,1),
                    new FaultInfo("预留,BYTE5>>bit6",5,6,0,0,1),
                    new FaultInfo("预留,BYTE5>>bit7",5,7,0,0,1),
                    new FaultInfo("预留,BYTE6>>bit0",6,0,0,0,1),
                    new FaultInfo("预留,BYTE6>>bit1",6,1,0,0,1),
                    new FaultInfo("预留,BYTE6>>bit2",6,2,0,0,1),
                    new FaultInfo("预留,BYTE6>>bit3",6,3,0,0,1),
                    new FaultInfo("预留,BYTE6>>bit4",6,4,0,0,1),
                    new FaultInfo("预留,BYTE6>>bit5",6,5,0,0,1),
                    new FaultInfo("预留,BYTE6>>bit6",6,6,0,0,1),
                    new FaultInfo("预留,BYTE6>>bit7",6,7,0,0,1),
                    new FaultInfo("母线短路永久故障,BYTE7>>bit0",7,0,0,0,1),
                    new FaultInfo("母线反接永久故障,BYTE7>>bit1",7,1,0,0,1),
                    new FaultInfo("预留,BYTE6>>bit2",6,2,0,0,1),
                    new FaultInfo("预留,BYTE6>>bit3",6,3,0,0,1),
                    new FaultInfo("预留,BYTE6>>bit4",6,4,0,0,1),
                    new FaultInfo("预留,BYTE6>>bit5",6,5,0,0,1),
                    new FaultInfo("预留,BYTE6>>bit6",6,6,0,0,1),
                    new FaultInfo("预留,BYTE6>>bit7",6,7,0,0,1),

        };
    }
}