using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;
using System.Configuration;
using System.Runtime.InteropServices;
using SofarBMS.Helper;
using SofarBMS.Model;
using SofarBMS.UI;
using System.Text;
using NPOI.SS.Formula.Functions;

namespace SofarBMS
{
    public partial class FrmMain : Form
    {
        public static string WIN_ID;
        public static byte BMS_ID = 0x01;
        public static byte PCU_ID = 0x21;
        public static byte BDU_ID = 0xA1;
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
        public static string MessageBoxTextStr = @"keyWriteSuccess,写入成功,WriteSuccess
keyWriteFail,写入失败,WriteFail
keyReadSuccess,读取成功,ReadSuccess
keyReadFail,读取失败,ReadFail
keyOpenPrompt,请先打开CAN口!,Please open the CAN port first!";

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            string s = Guid.NewGuid().ToString();

            //程序确保Log文件的存在性
            if (!Directory.Exists("Log"))
            {
                Directory.CreateDirectory("Log");
            }

            //本地SQLite数据库连接字符串（后期需要去除冗余，要调整为MySQL数据库）Pooling=true;FailIfMissing=false;
            SQLiteHelper.ConStr = "Data Source=DB//RealtimeDataBase;Version=3;";

            //使用多线程轮询获取CAN错误码（后期需要去除冗余，改为异常后自动复位）
            Task.Run(async () =>
            {
                while (true)
                {
                    string error = EcanHelper.ReadError().Replace("当前错误码：", "");
                    if (error != "00")
                    {
                        btnResetCAN_Click(sender, e);
                        error = EcanHelper.ReadError();
                    }
                    else
                    {
                        error = EcanHelper.ReadError().Replace("当前错误码：", "当前状态码：");
                    }

                    this.BeginInvoke(new Action(() => { toolStripStatusLabel1.Text = error; }));

                    await Task.Delay(1000 * 10);
                }
            });

            //使用多线程实时获取接收数据
            EcanHelper.Receive();

            //加载菜单栏和初始化界面
            InitMenuStrip();
            lblSp_01.Text = LanguageHelper.GetLanguage("Sp_01");
            lblSp_02.Text = LanguageHelper.GetLanguage("Sp_02");
            btnConnectionCAN.Text = LanguageHelper.GetLanguage("ConnectionCAN");
            btnResetCAN.Text = LanguageHelper.GetLanguage("ResetCAN");

            this.panel1.Controls.Add(new RTAControl());
            WIN_ID = "RTAControl";

            cbbID.SelectedIndex = 0;
            cbbIDP.SelectedIndex = 0;
            cbbBaud.SelectedIndex = 5;

            //this.Text = LanguageHelper.GetLanguage("Title");
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }


        #region Menu菜单栏，添加子菜单，事件函数等
        /// <summary>
        /// 菜单栏绑定数据源
        /// </summary>
        private void InitMenuStrip()
        {
            LanguageHelper.LanaguageIndex = Convert.ToInt32(ConfigurationManager.AppSettings["LanaguageIndex"]);

            //MenuStrip控件声明
            ToolStripMenuItem tsmiMenu;

            //添加“BTS 5K堆叠项目”菜单
            tsmiMenu = AddContextMenu(LanguageHelper.GetLanguage("tsmi_1"), Menu.Items, new EventHandler(MenuClicked));
            AddContextMenu(LanguageHelper.GetLanguage("tsmi_11"), tsmiMenu.DropDownItems, new EventHandler(MenuClicked));
            AddContextMenu(LanguageHelper.GetLanguage("tsmi_12"), tsmiMenu.DropDownItems, new EventHandler(MenuClicked));
            AddContextMenu(LanguageHelper.GetLanguage("tsmi_13"), tsmiMenu.DropDownItems, new EventHandler(MenuClicked));
            AddContextMenu(LanguageHelper.GetLanguage("tsmi_14"), tsmiMenu.DropDownItems, new EventHandler(MenuClicked));
            AddContextMenu(LanguageHelper.GetLanguage("tsmi_15"), tsmiMenu.DropDownItems, new EventHandler(MenuClicked));

            //添加“BMS低压储能项目”菜单
            tsmiMenu = AddContextMenu(LanguageHelper.GetLanguage("tsmi_2"), Menu.Items, null);
            AddContextMenu(LanguageHelper.GetLanguage("tsmi_21"), tsmiMenu.DropDownItems, new EventHandler(MenuClicked));
            AddContextMenu(LanguageHelper.GetLanguage("tsmi_22"), tsmiMenu.DropDownItems, new EventHandler(MenuClicked));
            AddContextMenu(LanguageHelper.GetLanguage("tsmi_23"), tsmiMenu.DropDownItems, new EventHandler(MenuClicked));
            //AddContextMenu(LanguageHelper.GetLanguage("tsmi_13"), tsmiMenu.DropDownItems, new EventHandler(MenuClicked));

            tsmiMenu = AddContextMenu(LanguageHelper.GetLanguage("tsmi_5"), Menu.Items, null);
            AddContextMenu(LanguageHelper.GetLanguage("tsmi_51"), tsmiMenu.DropDownItems, new EventHandler(MenuClicked));
            AddContextMenu(LanguageHelper.GetLanguage("tsmi_52"), tsmiMenu.DropDownItems, new EventHandler(MenuClicked));
            AddContextMenu(LanguageHelper.GetLanguage("tsmi_53"), tsmiMenu.DropDownItems, new EventHandler(MenuClicked));
            AddContextMenu(LanguageHelper.GetLanguage("tsmi_54"), tsmiMenu.DropDownItems, new EventHandler(MenuClicked));

            //添加“语言”菜单
            tsmiMenu = AddContextMenu(LanguageHelper.GetLanguage("tsmi_4"), Menu.Items, null);
            AddContextMenu(LanguageHelper.GetLanguage("tsmi_41"), tsmiMenu.DropDownItems, new EventHandler(MenuClicked));
            AddContextMenu(LanguageHelper.GetLanguage("tsmi_42"), tsmiMenu.DropDownItems, new EventHandler(MenuClicked));
        }

        /// <summary>
        /// 动态生成事件并打开窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuClicked(object sender, EventArgs e)
        {
            Dictionary<string, UserControl> MenuAndControl1 = new Dictionary<string, UserControl>()
            {
                { LanguageHelper.GetLanguage("tsmi_11"), new RTAControl() },
                { LanguageHelper.GetLanguage("tsmi_12"), new BMSSystemSetControl() },
                { LanguageHelper.GetLanguage("tsmi_13"), new ParamControl() },
                { LanguageHelper.GetLanguage("tsmi_14"), new StorageInfoControl() },
                { LanguageHelper.GetLanguage("tsmi_15"), new UpgradeControl() },
            };

            Dictionary<string, UserControl> MenuAndControl2 = new Dictionary<string, UserControl>()
            {
                { LanguageHelper.GetLanguage("tsmi_21"), new BMSControl() },
                { LanguageHelper.GetLanguage("tsmi_22"), new BMSMMultipleControl() },
                { LanguageHelper.GetLanguage("tsmi_23"), new BMSUpgradeControl() },
                { LanguageHelper.GetLanguage("tsmi_13"), new ParamControl() }
            };

            Dictionary<string, UserControl> MenuAndControl3 = new Dictionary<string, UserControl>()
            {
                { LanguageHelper.GetLanguage("tsmi_51"), new CBSControl() },
                { LanguageHelper.GetLanguage("tsmi_52"), new CBSParamControl() },
                { LanguageHelper.GetLanguage("tsmi_53"), new CBSUpgradeControl() },
                { LanguageHelper.GetLanguage("tsmi_54"), new CBSFileTransmit() }
            };

            bool isUserControl = false;

            string parentTitle = ((System.Windows.Forms.ToolStripItem)sender).OwnerItem?.ToString();

            if (parentTitle == null) return;

            string title = ((sender as ToolStripMenuItem).Text);

            if (parentTitle == "BTS 5K堆叠一体机" || parentTitle == "BTS 5K")
            {
                foreach (var item in MenuAndControl1)
                {
                    if (item.Key == title)
                    {
                        AddMenuClick(item.Value);

                        isUserControl = true;
                        break;
                    }
                }
            }
            else if (parentTitle == "BMS低压储能电池" || parentTitle == "BMS")
            {
                foreach (var item in MenuAndControl2)
                {
                    if (item.Key == title)
                    {
                        AddMenuClick(item.Value);

                        isUserControl = true;
                        break;
                    }
                }
            }
            else if (parentTitle == "CBS高压储能电池" || parentTitle == "CBS5000")
            {
                foreach (var item in MenuAndControl3)
                {
                    if (item.Key == title)
                    {
                        AddMenuClick(item.Value);

                        isUserControl = true;
                        break;
                    }
                }
            }

            if (!isUserControl)
            {
                if (title == LanguageHelper.GetLanguage("tsmi_41"))//中文
                {
                    SaveAppsetting(1.ToString());
                    Process.Start(Application.StartupPath + "\\SofarBMS.exe");
                    Process.GetCurrentProcess().Kill();
                }
                else if (title == LanguageHelper.GetLanguage("tsmi_42"))//中文
                {
                    SaveAppsetting(2.ToString());
                    Process.Start(Application.StartupPath + "\\SofarBMS.exe");
                    Process.GetCurrentProcess().Kill();
                }
            }
        }

        private void SaveAppsetting(string val)
        {
            string file = System.Windows.Forms.Application.ExecutablePath;
            Configuration config = ConfigurationManager.OpenExeConfiguration(file);
            config.AppSettings.Settings["LanaguageIndex"].Value = val;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void AddMenuClick(UserControl bc)
        {
            EcanHelper._task.Clear();
            panel1.Controls.Clear();
            bc.Dock = DockStyle.Fill;
            panel1.Controls.Add(bc);
            WIN_ID = bc.Name;

            if (bc.Name != "RTAControl")
            {
                RTAControl.cts?.Cancel();
            }
            if (bc.Name != "ParamControl")
            {
                ParamControl.cts?.Cancel();
            }
            if (bc.Name != "StorageInfoControl")
            {
                StorageInfoControl.cts?.Cancel();
            }
            if (bc.Name != "UpgradeControl")
            {
                UpgradeControl.cts?.Cancel();
            }
            if (bc.Name != "BMSControl")
            {
                BMSControl.cts?.Cancel();
            }
            if (bc.Name != "BMSMMultipleControl")
            {
                BMSMMultipleControl.cts?.Cancel();
            }
            if (bc.Name != "BMSUpgradeControl")
            {
                BMSUpgradeControl.cts?.Cancel();
            }
            if (bc.Name != "BMSSystemSetControl")
            {
                BMSSystemSetControl.cts?.Cancel();
            }
            if (bc.Name != "CBSControl")
            {
                CBSControl.cts?.Cancel();
            }
            if (bc.Name != "CBSParamControl")
            {
                CBSParamControl.cts?.Cancel();
            }
            if (bc.Name != "CBSUpgradeControl")
            {
                CBSUpgradeControl.cts?.Cancel();
            }
        }

        /// <summary>
        /// 添加子菜单
        /// </summary>
        /// <param name="text">要显示的文字，如果为 - 则显示为分割线</param>
        /// <param name="cms">要添加到的子菜单集合</param>
        /// <param name="callback">点击时触发的事件</param>
        /// <returns>生成的子菜单，如果为分隔条则返回null</returns>
        private ToolStripMenuItem AddContextMenu(string text, ToolStripItemCollection cms, EventHandler callback)
        {
            try
            {
                if (text == "-")
                {
                    ToolStripSeparator tsp = new ToolStripSeparator();
                    cms.Add(tsp);
                    return null;
                }
                else if (!string.IsNullOrEmpty(text))
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem(text);
                    tsmi.Tag = text + "TAG";
                    if (callback != null) tsmi.Click += callback;
                    cms.Add(tsmi);

                    return tsmi;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }
        #endregion

        #region 连接/断开/复位CAN
        private void btnConnectionCAN_Click(object sender, EventArgs e)
        {
            if (!EcanHelper.IsConnection)
            {
                if (ECANHelper.OpenDevice(1, 0, 0) != ECANStatus.STATUS_OK)
                {
                    MessageBox.Show(LanguageHelper.GetLanguage("OpenCAN_Error"));
                    return;
                }

                INIT_CONFIG INITCONFIG = new INIT_CONFIG();
                INITCONFIG.AccCode = 0;
                INITCONFIG.AccMask = 0xffffffff;
                INITCONFIG.Filter = 0;
                switch (cbbBaud.Text)
                {
                    case "125Kbps":
                        INITCONFIG.Timing0 = 0x03;
                        INITCONFIG.Timing1 = 0x1C;
                        break;
                    case "250Kbps":
                        INITCONFIG.Timing0 = 0x01;
                        INITCONFIG.Timing1 = 0x1C;
                        break;
                    case "500Kbps":
                        INITCONFIG.Timing0 = 0x00;
                        INITCONFIG.Timing1 = 0x1C;
                        break;
                    case "1000Kbps":
                        INITCONFIG.Timing0 = 0x00;
                        INITCONFIG.Timing1 = 0x14;
                        break;
                }

                INITCONFIG.Mode = 0;


                if (ECANHelper.InitCAN(1, 0, 0, ref INITCONFIG) != ECANStatus.STATUS_OK)
                {
                    MessageBox.Show(LanguageHelper.GetLanguage("InitCAN_Error"));
                    ECANHelper.CloseDevice(1, 0);
                    return;
                }

                if (ECANHelper.StartCAN(1, 0, 0) != ECANStatus.STATUS_OK)
                {
                    MessageBox.Show(LanguageHelper.GetLanguage("StartCAN_Error"));
                    ECANHelper.CloseDevice(1, 0);
                    return;
                }

                btnConnectionCAN.Text = LanguageHelper.GetLanguage("CanState_DisConn");
                EcanHelper.IsConnection = true;
            }
            else
            {
                ECANHelper.CloseDevice(0, 0);
                btnConnectionCAN.Text = LanguageHelper.GetLanguage("CanState_Conn");
                EcanHelper.IsConnection = false;

                //ExportData(System.AppDomain.CurrentDomain.BaseDirectory + "\\Log\\" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ff") + ".xlsx");
                //SQLiteHelper.Update("delete from RealtimeData");
            }
        }

        private void btnResetCAN_Click(object sender, EventArgs e)
        {
            if (!EcanHelper.IsConnection)
            {
                //MessageBox.Show(LanguageHelper.GetLanguage("Message_NotConn"));
                return;
            }
            //复位CAN
            if (ECANHelper.ResetCAN(1, 0, 0) != ECANStatus.STATUS_OK)
            {
                //MessageBox.Show(LanguageHelper.GetLanguage("Message_ResetError"));
                return;
            }
            //启动CAN
            if (ECANHelper.StartCAN(1, 0, 0) != ECANStatus.STATUS_OK)
            {
                //MessageBox.Show(LanguageHelper.GetLanguage("Message_StartError"));
                return;
            }
            //MessageBox.Show(LanguageHelper.GetLanguage("Message_ResetSuccess"));
        }
        #endregion

        #region 设备ID
        private void cbbID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbID.SelectedIndex != -1)
            {
                int num = cbbIDP.SelectedIndex == 1 ? 0x10 : 0x0;
                FrmMain.BMS_ID = Convert.ToByte(cbbID.SelectedIndex + 1 + num);
                FrmMain.PCU_ID = Convert.ToByte(FrmMain.BMS_ID + 0x20);
                FrmMain.BDU_ID = Convert.ToByte(FrmMain.BMS_ID + 0xA0);
            }
        }


        private void cbbIDP_SelectedIndexChanged(object sender, EventArgs e)
        {
            int num = cbbIDP.SelectedIndex == 1 ? 0x10 : 0x0;
            FrmMain.BMS_ID = Convert.ToByte(cbbID.SelectedIndex + 1 + num);
            FrmMain.PCU_ID = Convert.ToByte(FrmMain.BMS_ID + 0x20);
            FrmMain.BDU_ID = Convert.ToByte(FrmMain.BMS_ID + 0xA0);
        }
        #endregion

        #region 旧版本-导入Log
        private void ExportData(string path)
        {
            DataSet ds = SQLiteHelper.GetDataSet("select * from RealtimeData");
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count == 0)
                return;

            //List集合导出为Excel
            //创建工作簿对象
            IWorkbook workbook = new HSSFWorkbook();
            //创建工作表
            ISheet sheet = workbook.CreateSheet("onesheet");
            IRow row0 = sheet.CreateRow(0);
            string[] cells = new string[] { "运行时间", "电池状态", "故障信息", "告警信息", "保护信息", "充电电流上限", "放电电流上限", "充电MOS", "放电MOS", "预充MOS", "充电急停", "加热MOS", "电池电压", "负载电压", "电池电流", "电池剩余容量", "最高单体电压", "最高单体电压编号", "最低单体电压", "最低单体电压编号", "单体电压差值", "最高单体温度", "最高单体温度编号", "最低单体温度", "最低单体温度编号", "累计充电容量", "累计放电容量", "电芯电压1", "电芯电压2", "电芯电压3", "电芯电压4", "电芯电压5", "电芯电压6", "电芯电压7", "电芯电压8", "电芯电压9", "电芯电压10", "电芯电压11", "电芯电压12", "电芯电压13", "电芯电压14", "电芯电压15", "电芯电压16", "电芯温度1", "电芯温度2", "电芯温度3", "电芯温度4", "Mos温度", "环境温度", "电池健康程度" };
            for (int i = 0; i < cells.Length; i++)
            {
                row0.CreateCell(i).SetCellValue(cells[i]);
            }

            for (int r = 1; r < dt.Rows.Count; r++)
            {
                //创建行row
                IRow row = sheet.CreateRow(r);
                int c = 0;
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["CreateDate"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["BatteryStatus"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Fault"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Warning"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Protection"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Charge_current_limitation"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Discharge_current_limitation"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["ChargeMosEnable"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["DischargeMosEnable"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["PrechgMosEnable"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["StopChgEnable"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["HeatEnable"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Battery_Volt"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Load_Volt"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Battery_current"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["SOC"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["BatMaxCellVolt"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["BatMaxCellVoltNum"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["BatMinCellVolt"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["BatMinCellVoltNum"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["BatDiffCellVolt"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["BatMaxCellTemp"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["BatMaxCellTempNum"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["BatMinCellTemp"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["BatMinCellTempNum"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["TotalChgCap"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["TotalDsgCap"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Cell_voltage1"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Cell_voltage2"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Cell_voltage3"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Cell_voltage4"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Cell_voltage5"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Cell_voltage6"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Cell_voltage7"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Cell_voltage8"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Cell_voltage9"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Cell_voltage10"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Cell_voltage11"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Cell_voltage12"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Cell_voltage13"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Cell_voltage14"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Cell_voltage15"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Cell_voltage16"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Cell_temperature1"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Cell_temperature2"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Cell_temperature3"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Cell_temperature4"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["MOS_temperature"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["Env_Temperature"].ToString());
                row.CreateCell(c++).SetCellValue(dt.Rows[r]["SOH"].ToString());
            }

            //创建流对象并设置存储Excel文件的路径
            using (FileStream url = File.OpenWrite(path))//OpenWrite  @"C:\Users\admin\Desktop\1.xls"
            {
                //导出Excel文件
                workbook.Write(url);
            };
        }
        #endregion

        #region 清除出厂设置
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string IpClassName, string IpWindowName);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowText(IntPtr hWnd, string text);
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        const int WM_CLOSE = 0x10;
        const int BM_CLICK = 0xF5;
        int FunCord;
        IntPtr hwnd;
        int t;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (FunCord == 1)
            {
                hwnd = FindWindow(null, "系统将于" + t.ToString() + "秒后关机");
                t = t - 1;
                SetWindowText(hwnd, "系统将于" + t.ToString() + "秒后关机");
                if (t == 0)
                {
                    timer1.Enabled = false;
                    SendMessage(hwnd, WM_CLOSE, 0, 0);
                }
            }
            else if (FunCord == 2)
            {
                hwnd = FindWindow(null, "成功提示");
                IntPtr a = FindWindowEx(hwnd, (IntPtr)null, null, "对话框将于" + t.ToString() + "秒后关闭");
                t = t - 1;
                SetWindowText(a, "对话框将于" + t.ToString() + "秒后关闭");
                if (t == 0)
                {
                    timer1.Enabled = false;
                    SendMessage(hwnd, WM_CLOSE, 0, 0);
                }
            }
            else if (FunCord == 3)
            {
                hwnd = FindWindow(null, "系统将于" + t.ToString() + "秒后关机");
                t = t - 1;
                SetWindowText(hwnd, "系统将于" + t.ToString() + "秒后关机");
                if (t == 0)
                {
                    IntPtr OKHwnd = FindWindowEx(hwnd, IntPtr.Zero, null, "确定");
                    SendMessage(OKHwnd, BM_CLICK, 0, 0);
                    timer1.Enabled = false;
                }
            }
            else if (FunCord == 4)
            {
                hwnd = FindWindow(null, "系统将于" + t.ToString() + "秒后关机");
                t = t - 1;
                SetWindowText(hwnd, "系统将于" + t.ToString() + "秒后关机");
                if (t == 0)
                {
                    IntPtr OKHwnd = FindWindowEx(hwnd, IntPtr.Zero, null, "取消");
                    SendMessage(OKHwnd, BM_CLICK, 0, 0);
                    timer1.Enabled = false;
                }
            }
        }

        private void btnClearInit_Click(object sender, EventArgs e)
        {
            timer1.Start();

            try
            {
                bool result = true;
                //1.清空存储信息 2.清除充/放电容量
                result &= EcanHelper.Send(new byte[] { 0xAA, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }
                                       , new byte[] { 0xE0, FrmMain.BMS_ID, 0x2D, 0x10 });
                result &= EcanHelper.Send(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }
                                       , new byte[] { 0xE0, FrmMain.BMS_ID, 0x24, 0x10 });
                if (result)
                {
                    FunCord = 2;
                    t = 5;
                    timer1.Enabled = true;
                    MessageBox.Show("对话框将于" + t + "秒后关闭", "成功提示");
                    timer1.Enabled = false;
                }
                else
                {
                    MessageBox.Show("执行失败！");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("执行失败！");
            }

            timer1.Stop();
        }
        #endregion

        /// <summary>
        /// 获取MessageBox语言资源
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="languageIndex">1|2 1中文 2英文</param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            if (key == null || key.Length == 0)
            {
                return "";
            }
            string[] csvarr = MessageBoxTextStr.Split("\n".ToCharArray());
            foreach (string s in csvarr)
            {
                string[] sarr = s.Split(",".ToCharArray());
                if (sarr.Length < 3)
                {
                    continue;
                }
                if (sarr[0].Equals(key))
                {
                    return sarr[LanguageHelper.LanaguageIndex].TrimEnd("\r".ToCharArray()).Trim();
                }
            }

            return key;
        }
    }
}
