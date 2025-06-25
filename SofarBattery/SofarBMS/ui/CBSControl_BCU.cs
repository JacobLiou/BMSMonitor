using NPOI.POIFS.Crypt.Dsig;
using Sofar.BMS.Common;
using Sofar.BMS.Models;
using Sofar.ConnectionLibs.CAN;
using SofarBMS.Helper;
using SofarBMS.Model;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

namespace SofarBMS.UI
{
    public partial class CBSControl_BCU : UserControl
    {
        public CBSControl_BCU()
        {
            InitializeComponent();
        }

        public static CancellationTokenSource cts = null;
        private EcanHelper ecanHelper = EcanHelper.Instance;
        private string[] packSN = new string[3];
        private BcuRealtimeData model = new();

        //多簇数据存储
        private int bindingType;
        private List<batterySocData> batterySocDataList = new();
        private List<batterySohData> batterySohDataList = new();
        private List<batteryVoltageData> batteryVoltageDataList = new();
        private List<batteryTemperatureData> batteryTemperatureDataList = new();

        private string filePath
        {
            get
            {
                string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");//, DateTime.Now.ToString("yyyy-MM")
                return Path.Combine(directory, $"BCU_{DateTime.Now:yyyy-MM-dd}.csv");
            }
        }
        private readonly object _fileLock = new object();
        private (string Date, bool HeaderWritten) _fileState = ("", false);
        private const int MAX_RETRIES = 3;
        private const int RETRY_DELAY_MS = 300;

        /// <summary>
        /// 通用电池数据
        /// </summary>
        private ObservableCollection<IBatteryData> _batteryDataList = new();
        public ObservableCollection<IBatteryData> BatteryDataList
        {
            get => _batteryDataList;
            set
            {
                if (_batteryDataList != null)
                    _batteryDataList.CollectionChanged -= BatteryDataCollectionChanged;

                _batteryDataList = value;

                if (_batteryDataList != null)
                    _batteryDataList.CollectionChanged += BatteryDataCollectionChanged;

                this.Invoke((MethodInvoker)delegate
                {
                    dataGridView1.DataSource = _batteryDataList?.ToList(); // 强制刷新
                });
            }
        }

        private void BatteryDataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                dataGridView1.DataSource = BatteryDataList?.ToList(); // 重新绑定触发刷新
            });
        }

        private void RTAControl_Load(object sender, EventArgs e)
        {
            // UI初始化
            this.Invoke(() =>
            {
                foreach (Control item in this.Controls)
                {
                    GetControls(item);
                }
                this.dataGridView1.AutoGenerateColumns = false;
            });

            // 初始化计时器（优化后）
            //InitializeTimers();

            // 初始化取消令牌
            cts = new CancellationTokenSource();
            int p = -1;
            // 优化后的CAN通信任务
            Task.Run(async () =>
            {
                const int MAX_RETRY = 3;
                while (!cts.IsCancellationRequested)
                {
                    if (p == -1
                    || p != FrmMain.BCU_ID)
                    {
                        p = FrmMain.BCU_ID;
                        ClearInputControls(this);
                    }

                    if (!ecanHelper.IsConnected)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    try
                    {
                        // 使用Burst模式发送命令（减少await次数）
                        var sendTasks = new List<Task>();

                        // 一键操作标定参数
                        sendTasks.Add(SendCommandWithRetry(
                            new byte[8] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                            new byte[] { 0xE0, FrmMain.BCU_ID, 0xF9, 0x10 },
                            MAX_RETRY
                        ));

                        // 上位机监控读取
                        sendTasks.Add(SendCommandWithRetry(
                            new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                            new byte[] { 0xE0, FrmMain.BCU_ID, 0xF7, 0x10 },
                            MAX_RETRY
                        ));

                        // 多簇实时数据（并行发送）
                        var clusterTasks = Enumerable.Range(1, 12)
                            .Select(bIndex => SendCommandWithRetry(
                                new byte[] { (byte)bIndex, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                                new byte[] { 0xE0, FrmMain.BCU_ID, 0x05, 0x16 },
                                MAX_RETRY
                            ));

                        sendTasks.AddRange(clusterTasks);

                        // 等待所有命令完成（带超时）
                        await Task.WhenAll(sendTasks).WaitAsync(TimeSpan.FromSeconds(30), cts.Token);

                        // 统一等待时间（替代多次Delay）
                        Thread.Sleep(1000);
                    }
                    catch (OperationCanceledException)
                    {
                        // 正常取消
                        break;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"CAN通信异常: {ex.Message}");
                        Thread.Sleep(5000);  // 错误恢复间隔
                    }
                    finally
                    {
                        if (model != null)
                        {
                            this.Invoke((Action)(() =>
                            {
                                model.Fault = richTextBox1.Text.Replace("\n", "，").Replace("\r", "，");
                                model.Protection = richTextBox2.Text.Replace("\n", "，").Replace("\r", "，");
                                model.Warning = richTextBox3.Text.Replace("\n", "，").Replace("\r", "，");
                                model.Prompt = richTextBox4.Text.Replace("\n", "，").Replace("\r", "，");
                            }));

                            try
                            {
                                for (int group = 0; group < 12; group++)
                                {
                                    // 计算当前组在A中的起始位置（每组18个元素）
                                    int startA = group * 18;
                                    // 计算当前组在B中的起始位置（每组16个元素）
                                    int startB = group * 16;

                                    // 复制当前组的16个有效数据
                                    for (int i = 0; i < 16; i++)
                                    {
                                        model.Voltage_Array[startB + i] = batteryVoltageDataList[startA + i].Voltage;
                                        model.SOC_Array[startB + i] = batterySocDataList[startA + i].SOC;
                                        model.SOH_Array[startB + i] = batterySohDataList[startA + i].SOH;
                                    }

                                    // 计算当前组在A中的起始位置（每组18个元素）
                                    startA = group * 12;
                                    // 计算当前组在B中的起始位置（每组16个元素）
                                    startB = group * 8;

                                    // 复制当前组的8个有效数据
                                    for (int i = 0; i < 8; i++)
                                    {
                                        model.Tempeartrue_Array[startB + i] = batteryTemperatureDataList[startA + i].Temperature;
                                    }
                                }
                            }
                            catch (Exception)
                            {

                            }

                            lock (_fileLock)
                            {
                                if (!File.Exists(filePath))
                                {
                                    InitLogWriter(filePath);
                                }
                                _logWriter?.WriteLine(model.GetValue());
                            }
                        }
                    }
                }
            }, cts.Token);

            // 事件订阅
            ecanHelper.AnalysisDataInvoked += ServiceBase_AnalysisDataInvoked;
        }

        // 新增辅助方法：带重试机制的命令发送
        private async Task SendCommandWithRetry(byte[] data, byte[] address, int maxRetry)
        {
            int retryCount = 0;
            while (retryCount++ < maxRetry)
            {
                try
                {
                    ecanHelper.Send(data, address);
                    Thread.Sleep(100);
                    return;
                }
                catch (Exception ex)
                {
                    if (retryCount >= maxRetry) throw;
                    Debug.WriteLine($"命令发送失败，第{retryCount}次重试: {ex.Message}");
                    await Task.Delay(100 * retryCount);
                }
            }
        }

        // 使用 StreamWriter 保持文件流打开（注意线程安全）
        private static StreamWriter _logWriter;

        // 初始化日志写入器
        void InitLogWriter(string filePath)
        {
            lock (_fileLock)
            {
                _logWriter?.Dispose();
                _logWriter = new StreamWriter(filePath, append: true) { AutoFlush = true };
                if (new FileInfo(filePath).Length == 0)
                {
                    _logWriter.WriteLine(model.GetHeader());
                }
            }
        }

        // 初始化计时器（与数据存储优化结合）
        private void InitializeTimers()
        {
            // 数据采集定时器（保持5秒间隔）
            System.Timers.Timer timer = new System.Timers.Timer(5000);
            timer.Elapsed += async (s, e) =>
            {
                Debug.WriteLine($"{System.DateTime.Now.ToString("HH:mm:ss:ffff")}数据采集正在进行！！！");
                try
                {
                    if (!ecanHelper.IsConnected || model == null)
                        return;

                    //this.Invoke((Action)(() =>
                    //{
                    //    model.Fault = richTextBox1.Text.Replace("\n", "，").Replace("\r", "，");
                    //    model.Protection = richTextBox2.Text.Replace("\n", "，").Replace("\r", "，");
                    //    model.Warning = richTextBox3.Text.Replace("\n", "，").Replace("\r", "，");
                    //    model.Prompt = richTextBox4.Text.Replace("\n", "，").Replace("\r", "，");
                    //}));

                    for (int group = 0; group < 12; group++)
                    {
                        // 计算当前组在A中的起始位置（每组18个元素）
                        int startA = group * 18;
                        // 计算当前组在B中的起始位置（每组16个元素）
                        int startB = group * 16;

                        // 复制当前组的16个有效数据
                        for (int i = 0; i < 16; i++)
                        {
                            model.Voltage_Array[startB + i] = batteryVoltageDataList[startA + i].Voltage;
                            model.SOC_Array[startB + i] = batterySocDataList[startA + i].SOC;
                            model.SOH_Array[startB + i] = batterySohDataList[startA + i].SOH;
                        }

                        // 计算当前组在A中的起始位置（每组18个元素）
                        startA = group * 12;
                        // 计算当前组在B中的起始位置（每组16个元素）
                        startB = group * 8;

                        // 复制当前组的8个有效数据
                        for (int i = 0; i < 8; i++)
                        {
                            model.Tempeartrue_Array[startB + i] = batteryTemperatureDataList[startA + i].Temperature;
                        }
                    }

                    // 获取当前文件路径（确保路径一致性）
                    string filePath = this.filePath;
                    string data = model.GetValue();

                    // 在UI线程安全执行
                    if (this.InvokeRequired)
                    {
                        this.Invoke((Action)(() => AppendDataToCsv(filePath, data)));
                    }
                    else
                    {
                        AppendDataToCsv(filePath, data);
                    }
                }
                catch (Exception ex)
                {
                    //Debug.WriteLine($"数据采集失败: {ex.Message}");
                    //this.BeginInvoke((Action)(() =>
                    //    MessageBox.Show($"CSV写入错误: {ex.Message}", "数据记录错误",
                    //                  MessageBoxButtons.OK, MessageBoxIcon.Error)));
                }
            };
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void ServiceBase_AnalysisDataInvoked(object? sender, object e)
        {
            if (cts.IsCancellationRequested && ecanHelper.IsConnected)
            {
                ecanHelper.AnalysisDataInvoked -= ServiceBase_AnalysisDataInvoked;
                return;
            }

            var frameModel = e as CanFrameModel;
            if (frameModel != null)
            {
                this.Invoke(() => { AnalysisData(frameModel.CanID, frameModel.Data); });
            }
        }

        #region 解析数据
        public void AnalysisData(uint canID, byte[] data)
        {
            if ((canID & 0xff) != FrmMain.BCU_ID)
                return;

            if (model == null)
            {
                model = new BcuRealtimeData();
                model.PackID = FrmMain.BCU_ID.ToString("X2");
            }

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
                                   {0, "pbCCDetectionStatus" },
                                   {1, "pbRelay1Status" },
                                   {2, "pbRelay2Status" },
                                   {3, "pbButtonWakeupStatus" },
                                   {4, "pbInverterWakeupStatus" },
                                   {5, "pbDCRelayControlSignal" },
                                };

                                UpdateControlStatus(diTypeDic, Convert.ToUInt16(BytesToIntger(0x00, data[1])));

                                model.CCDetectionStatus = MyCustomConverter.GetBit(data[1], 0);
                                model.Relay1Status = MyCustomConverter.GetBit(data[1], 1);
                                model.Relay2Status = MyCustomConverter.GetBit(data[1], 2);
                                model.ButtonWakeupStatus = MyCustomConverter.GetBit(data[1], 3);
                                model.InverterWakeupStatus = MyCustomConverter.GetBit(data[1], 4);
                                model.DCRelayControlSignal = MyCustomConverter.GetBit(data[1], 5);
                                break;
                            case 0x02:
                                //继电器类型保留
                                break;
                            case 0x03:
                                var chgAndDischargeStateDic = new Dictionary<short, string>
                                {
                                   {0, "pbChagreStatus"},
                                   {1, "pbDischargeStatus"},
                                   {2, "pbForceChargingEnable"},
                                   {3, "pbFullCharge"},
                                   {4, "pbEmpty"}
                                };

                                UpdateControlStatus(chgAndDischargeStateDic, Convert.ToUInt16(BytesToIntger(0x00, data[1])));
                                model.ChagreStatus = MyCustomConverter.GetBit(data[1], 0);
                                model.DischargeStatus = MyCustomConverter.GetBit(data[1], 1);
                                model.ForceChargeStatus = MyCustomConverter.GetBit(data[1], 2);
                                model.FullyCharged = MyCustomConverter.GetBit(data[1], 3);
                                model.FullyDischarged = MyCustomConverter.GetBit(data[1], 4);
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
                                 };
                                var chargBalancedStateDic2 = new Dictionary<short, string>
                                 {
                                     {0, "pbBatteryPack_9_BalancedState" },
                                     {1, "pbBatteryPack_10_BalancedState" },
                                     {2, "pbBatteryPack_11_BalancedState" },
                                     {3, "pbBatteryPack_12_BalancedState" },
                                     {4, "pbBatteryPack_13_BalancedState" },
                                     {5, "pbBatteryPack_14_BalancedState" },
                                     {6, "pbBatteryPack_15_BalancedState" },
                                     {7, "pbBatteryPack_16_BalancedState" },
                                 };

                                foreach (var kvp in chargBalancedStateDic)
                                {
                                    short bitIndex = kvp.Key;
                                    string controlName = kvp.Value;
                                    var control = this.Controls.Find(controlName, true).FirstOrDefault() as PictureBox;
                                    if (control != null)
                                    {
                                        control.BackColor = MyCustomConverter.GetBit((byte)data[1], bitIndex) == 0 ? Color.Red : Color.Green;
                                    }
                                }

                                foreach (var kvp in chargBalancedStateDic2)
                                {
                                    short bitIndex = kvp.Key;
                                    string controlName = kvp.Value;
                                    var control = this.Controls.Find(controlName, true).FirstOrDefault() as PictureBox;
                                    if (control != null)
                                    {
                                        control.BackColor = MyCustomConverter.GetBit((byte)data[2], bitIndex) == 0 ? Color.Red : Color.Green;
                                    }
                                }
                                //UpdateControlStatus(chargBalancedStateDic, Convert.ToUInt16(BytesToIntger(0x00, data[1])));
                                //UpdateControlStatus(chargBalancedStateDic2, Convert.ToUInt16(BytesToIntger(0x00, data[2])));
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
                                 };
                                var dischargBalancedStateDic2 = new Dictionary<short, string>
                                 {
                                    {0, "pbBatteryPackBalancedState_9" },
                                    {1, "pbBatteryPackBalancedState_10" },
                                    {2, "pbBatteryPackBalancedState_11" },
                                    {3, "pbBatteryPackBalancedState_12" },
                                    {4, "pbBatteryPackBalancedState_13" },
                                    {5, "pbBatteryPackBalancedState_14" },
                                    {6, "pbBatteryPackBalancedState_15" },
                                    {7, "pbBatteryPackBalancedState_16" },
                                 };

                                foreach (var kvp in dischargBalancedStateDic)
                                {
                                    short bitIndex = kvp.Key;
                                    string controlName = kvp.Value;
                                    var control = this.Controls.Find(controlName, true).FirstOrDefault() as PictureBox;
                                    if (control != null)
                                    {
                                        control.BackColor = MyCustomConverter.GetBit((byte)data[1], bitIndex) == 0 ? Color.Red : Color.Green;
                                    }
                                }

                                foreach (var kvp in dischargBalancedStateDic2)
                                {
                                    short bitIndex = kvp.Key;
                                    string controlName = kvp.Value;
                                    var control = this.Controls.Find(controlName, true).FirstOrDefault() as PictureBox;
                                    if (control != null)
                                    {
                                        control.BackColor = MyCustomConverter.GetBit((byte)data[2], bitIndex) == 0 ? Color.Red : Color.Green;
                                    }
                                }
                                //UpdateControlStatus(dischargBalancedStateDic, Convert.ToUInt16(BytesToIntger(0x00, data[1])));
                                //UpdateControlStatus(dischargBalancedStateDic2, Convert.ToUInt16(BytesToIntger(0x00, data[2])));
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
                                    int byteValue = data[i + 1];
                                    strs[i] = ((byteValue > 127) ? (int)byteValue - 256 : byteValue).ToString();
                                }

                                controls = new string[5] { "txtPower_Terminal_Temperature1", "txtPower_Terminal_Temperature2", "txtPower_Terminal_Temperature3", "txtPower_Terminal_Temperature4", "txtPower_Terminal_Temperature5" };
                                for (int i = 0; i < strs.Length; i++)
                                {
                                    (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                                }

                                model.PowerTerminalTemperature1 = Convert.ToDouble(strs[0]);
                                model.PowerTerminalTemperature2 = Convert.ToDouble(strs[1]);
                                model.PowerTerminalTemperature3 = Convert.ToDouble(strs[2]);
                                model.PowerTerminalTemperature4 = Convert.ToDouble(strs[3]);
                                model.AmbientTemperature = Convert.ToDouble(strs[4]);
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

                                model.BatterySamplingVoltage1 = Convert.ToDouble(strs[0]);
                                model.BusSamplingVoltage = Convert.ToDouble(strs[1]);
                                model.BatterySummingVoltage = Convert.ToDouble(strs[2]);
                                break;
                            case 0x02:
                                strs = new string[2];
                                strs[0] = BytesToIntger(data[2], data[1], 0.1);
                                strs[1] = BytesToIntger(data[4], data[3], 0.1);

                                //加热膜供电电压，加热膜MOS电压（0.1V）
                                txtHeating_film_voltage.Text = strs[0];
                                txtHeating_film_MOSvoltage.Text = strs[1];

                                model.HeatingFilmVoltage = Convert.ToDouble(strs[0]);
                                model.HeatingFilmMosVoltage = Convert.ToDouble(strs[1]);
                                break;
                            case 0x03:
                                strs = new string[1];
                                strs[0] = BytesToIntger(data[2], data[1], 0.1);

                                //绝缘电阻电压（0.1V）
                                txtInsulation_resistance_voltage.Text = strs[0];
                                model.InsulationResistance = Convert.ToDouble(strs[0]);
                                break;
                            case 0x04:
                                strs = new string[1];
                                strs[0] = BytesToIntger(data[2], data[1], 1);

                                //辅源电压（1mV）
                                txtAuxiliary_source_voltage.Text = strs[0];
                                model.AuxiliaryPowerSupplyVoltage = Convert.ToDouble(strs[0]);
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

                                model.ClusterCurrent = Convert.ToDouble(strs[0]);
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

                        model.ClusterMaxCellVolt = Convert.ToDouble(strs[0]);
                        model.ClusterMaxCellVoltPack = Convert.ToUInt16(strs[1]);
                        model.ClusterMaxCellVoltNum = Convert.ToUInt16(strs[2]);
                        model.ClusterMinCellVolt = Convert.ToDouble(strs[3]);
                        model.ClusterMinCellVoltPack = Convert.ToUInt16(strs[4]);
                        model.ClusterMinCellVoltNum = Convert.ToUInt16(strs[5]);
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

                        model.ClusterMaxCellTemp = Convert.ToDouble(strs[0]);
                        model.ClusterMaxCellTempPack = Convert.ToUInt16(strs[1]);
                        model.ClusterMaxCellTempNum = Convert.ToUInt16(strs[2]);
                        model.ClusterMinCellTemp = Convert.ToDouble(strs[3]);
                        model.ClusterMinCellTempPack = Convert.ToUInt16(strs[4]);
                        model.ClusterMinCellTempNum = Convert.ToUInt16(strs[5]);
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

                        model.BatteryChargeVoltage = Convert.ToDouble(strs[0]);
                        model.ChargeCurrentLimitation = Convert.ToDouble(strs[1]);
                        model.DischargeCurrentLimitation = Convert.ToDouble(strs[2]);
                        model.BatteryDischargeVoltage = Convert.ToDouble(strs[3]);
                        break;
                    //0x0BD:BCU遥测数据2->>BCU版本号信息
                    case 0x10BDE0FF:
                        //BCU软件版本
                        string[] bcu_soft = new string[3];
                        for (int i = 0; i < 3; i++)
                        {
                            bcu_soft[i] = data[i + 1].ToString().PadLeft(2, '0');
                        }
                        txtSoftware_Version_BCU.Text = model.BCUSoftwareVersion = Encoding.ASCII.GetString(new byte[] { data[0] }) + string.Join("", bcu_soft);
                        //BCU硬件版本
                        string[] bsm_HW = new string[2];
                        for (int i = 0; i < 1; i++)
                        {
                            bsm_HW[i] = data[i + 4].ToString().PadLeft(2, '0');
                        }
                        txtHardware_Version_BCU.Text = model.BCUHardwareVersion = string.Join("", bsm_HW);
                        //CAN协议版本号
                        txtCANprotocolVersion.Text = model.CANProtocolVersion = BytesToIntger(data[6], data[5]);
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

                        model.RemainingTotalCapacity = Convert.ToDouble(strs[0]);
                        model.BatAverageTemp = Convert.ToDouble(strs[1]);
                        model.ClusterRatePower = Convert.ToDouble(strs[2]);
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

                                txtBms_State.Text = model.BmsState = Enum.Parse(typeof(BMSState), (Convert.ToInt32(data[1].ToString("X2"), 16) & 0x0f).ToString()).ToString();

                                model.ClusterSOC = Convert.ToUInt16(strs[1]);
                                model.ClusterSOH = Convert.ToUInt16(strs[2]);
                                model.ClusterBatPackNum = Convert.ToUInt16(strs[3]);
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
                        txtBCU_System_Time.Text = model.BCUSytemTime = date.ToString();
                        break;
                    //0x0C1:BCU遥测数据上报3---其他数据
                    case 0x10C1E0FF:
                        switch (data[0])
                        {
                            case 0x01:
                                //绝缘阻抗（1KΩ）
                                textBox2.Text = BytesToIntger(data[2], data[1]);

                                //绝缘检测状态，系统接入状态，编址状态
                                string status = "";
                                switch (data[3])
                                {
                                    case 0x00: status = "初始状态"; break;
                                    case 0x01: status = "检测中"; break;
                                    case 0x02: status = "异常"; break;
                                    case 0x03: status = "正常"; break;
                                    default:
                                        break;
                                }
                                textBox3.Text = status;

                                ckRealtimeData_BCU_3.Checked = data[4] == 1 ? true : false;

                                ckRealtimeData_BCU_6.Checked = (data[5] & 0x01) == 0x01 ? true : false;
                                ckRealtimeData_BCU_7.Checked = (data[5] & 0x02) == 0x02 ? true : false;
                                ckRealtimeData_BCU_8.Checked = (data[5] & 0x04) == 0x04 ? true : false;

                                ckRealtimeData_BCU_4.Checked = data[6] == 1 ? true : false;
                                break;
                            case 0x02:
                                textBox1.Text = $"0x{data[2].ToString("X2")}{data[1].ToString("X2")}";
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
                                analysisLog(new byte[] { data[1], data[2], data[3], data[4] }, 1);
                                break;
                            case 0x02:
                                analysisLog(new byte[] { data[1], data[2] }, 2);
                                break;
                            case 0x03:
                                analysisLog(new byte[] { data[1], data[2] }, 3);
                                break;
                            case 0x04:
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
                            txtSN.Text = model.SN = strSn;
                        break;
                    //综合所有BMU故障信息
                    case 0x10C5E0FF:
                        AnalysisInsideFaultInfo(data, 0);//0x008
                        break;
                    case 0x10C6E0FF:
                        AnalysisInsideFaultInfo(data, 1);//0x045
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

                                //model.Inductive_Current_Sampling1 = Convert.ToDouble(strs[0]);
                                //model.Inductive_Current_Sampling2 = Convert.ToDouble(strs[1]);
                                //model.Inductive_Current_Sampling3 = Convert.ToDouble(strs[2]);
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

                                model.InductiveCurrentSampling4 = Convert.ToDouble(strs[0]);
                                model.DischargeHighCurrentSampling = Convert.ToDouble(strs[1]);
                                model.ChargeHighCurrentSampling = Convert.ToDouble(strs[2]);
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

                                model.ContactorVoltage = Convert.ToDouble(strs[0]);
                                model.BatteryClusterVoltage1 = Convert.ToDouble(strs[1]);
                                model.HighBusVoltage = Convert.ToDouble(strs[2]);
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

                                model.HeatingFilmPowerSupplyVoltage = Convert.ToDouble(strs[0]);
                                model.BatterySamplingVoltage2 = Convert.ToDouble(strs[1]);
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

                                model.RadiatorTemperature1 = Convert.ToDouble(strs[0]);
                                model.RadiatorTemperature2 = Convert.ToDouble(strs[1]);
                                model.RadiatorTemperature3 = Convert.ToDouble(strs[2]);
                                model.RadiatorTemperature4 = Convert.ToDouble(strs[3]);
                                break;
                            case 0x06:
                                //软件版本号，硬件版本号，SCI协议版本号
                                string[] softwareVersion = new string[3];
                                for (int i = 0; i < 3; i++)
                                {
                                    softwareVersion[i] = data[i + 2].ToString().PadLeft(2, '0');
                                }
                                txtSoftware_Version_DCDC.Text = model.PCUSoftwareVersion = Encoding.ASCII.GetString(new byte[] { data[1] }) + string.Join("", softwareVersion);

                                //BCU硬件版本
                                string[] dcdc_HW = new string[2];
                                for (int i = 0; i < 1; i++)
                                {
                                    dcdc_HW[i] = data[i + 5].ToString().PadLeft(2, '0');
                                }
                                txtHardware_Version_DCDC.Text = model.PCUHardwareVersion = string.Join("", dcdc_HW);
                                //CAN协议版本号
                                txtSCIprotocolVersion.Text = model.SCIProtocolVersion = BytesToIntger(data[7], data[6]);
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
                                    txtWorkState.Text = model.PCUWorkState = Enum.Parse(typeof(WorkState_DCDC), strs[0]).ToString();
                                }

                                if (Enum.TryParse(typeof(BatteryState_DCDC), strs[1], out dcdcState))
                                {
                                    txtDCDC_State.Text = model.PCUBatteryState = Enum.Parse(typeof(BatteryState_DCDC), strs[1]).ToString();
                                }
                                break;
                            case 0x02:
                                analysisLog(new byte[] { data[1], data[2], data[3], data[4], data[5], data[6], data[7] }, 1, "DCDC");
                                break;
                            case 0x03:
                                //故障数据8,协议未补充
                                break;
                            case 0x04:
                                //遥信数据：充电限载电流值，放电限载电流值
                                double val1 = (short)((data[2] << 8) | data[1]);
                                double val2 = (short)((data[4] << 8) | data[3]);
                                txtChargeLimitCurrentValue.Text = (val1 * 0.01).ToString("F2");
                                txtDischargeLimitCurrentValue.Text = (val2 * 0.01).ToString("F2");

                                model.DischargeLimitCurrent = val1;
                                model.ChargeLimitCurrent = val2;
                                break;
                            default:
                                break;
                        }
                        break;
                    case 0x1681E0FF:
                    case 0x1681FFFF:
                        // Byte 1:帧序号（n） 一帧3个电池数据 6帧共18个电池数据
                        int frameNumber = data[0];
                        // Byte 2: 包序号（N） 
                        int sequenceNumber = data[1];

                        if (frameNumber >= 0x01 && frameNumber <= 0x0C)
                        {
                            #region 临时代码
                            int startIndex = (sequenceNumber - 1) * 3 + (frameNumber - 1) * 16;

                            if (sequenceNumber >= 0x01 && sequenceNumber < 0x06)
                            {
                                for (int k = 0; k < 3; k++)
                                {
                                    int byteIndex = 2 + k * 2;
                                    double voltageRaw = (data[byteIndex + 1] << 8) | data[byteIndex];
                                    model.Voltage_Array[startIndex + k] = (voltageRaw * 0.001).ToString("F3");// 保留3位小数
                                }
                            }
                            else if (sequenceNumber == 0x06)
                            {
                                int byteIndex = 2;
                                double voltageRaw = (data[byteIndex + 1] << 8) | data[byteIndex];
                                model.Voltage_Array[startIndex] = (voltageRaw * 0.001).ToString("F3");// 保留3位小数

                                if (bindingType == 81)
                                {
                                    this.BeginInvoke(() =>
                                    {
                                        BatteryDataList = new ObservableCollection<IBatteryData>(
                                            this.UpdateDataGrid<batteryVoltageData>(batteryVoltageDataList.OrderBy(t => Convert.ToInt32(t.SectionNumber)).ToList<batteryVoltageData>(),
                                            new[] { "序号", "电压" },
                                            data => data.SectionNumber,
                                            data => data.Voltage,
                                            16,
                                            18));
                                    });
                                }
                            }
                            #endregion

                            if (sequenceNumber >= 0x01 && sequenceNumber <= 0x06)
                            {
                                // 每个包有18个电池电压数据，每帧3个电池电压数据
                                int startBatteryIndex = (sequenceNumber - 1) * 3 + (frameNumber - 1) * 18;

                                // 表格数据
                                var batteryVoltages = new string[3 + startBatteryIndex];
                                for (int k = 0; k < 3; k++)
                                {
                                    int byteIndex = 2 + k * 2;
                                    double voltageRaw = (data[byteIndex + 1] << 8) | data[byteIndex];
                                    batteryVoltages[startBatteryIndex + k] = (voltageRaw * 0.001).ToString("F3");// 保留3位小数
                                }

                                // 修改或新增电压数据
                                for (int i = startBatteryIndex; i < batteryVoltages.Length; i++)
                                {
                                    string sectionNumber = (i + 1).ToString();

                                    // 查找是否已存在该编号的记录
                                    var existingData = batteryVoltageDataList.FirstOrDefault(data => data.SectionNumber == sectionNumber);
                                    if (existingData != null)
                                    {
                                        // 更新已有记录的电压值
                                        existingData.Voltage = batteryVoltages[i];
                                    }
                                    else
                                    {
                                        // 新增记录
                                        batteryVoltageDataList.Add(new batteryVoltageData
                                        {
                                            SectionNumber = sectionNumber,
                                            Voltage = batteryVoltages[i]
                                        });
                                    }
                                }
                            }
                        }
                        break;
                    case 0x1682E0FF:
                    case 0x1682FFFF:
                        // Byte 1:帧序号 1~20（n） 20帧 1帧6个电池温度数据
                        frameNumber = data[0];
                        // Byte 2: 包序号 1~4（N）（电压按 120 节电池划分的包数） 4*20*6=480
                        sequenceNumber = data[1];
                        // 实际调试224个电池温度数据
                        if (frameNumber >= 0x01 && frameNumber <= 0x0C)
                        {
                            if (sequenceNumber >= 0x01 && sequenceNumber <= 0x02)
                            {
                                #region 临时代码
                                int startIndex = (sequenceNumber - 1) * 6 + (frameNumber - 1) * 8;

                                if (sequenceNumber >= 0x01 && sequenceNumber < 0x02)
                                {
                                    for (int k = 0; k < 6; k++)
                                    {
                                        int byteIndex = k + 2;
                                        double temperatureRaw = HexConverter.HexToSByte(data[byteIndex].ToString("X2"));
                                        model.Tempeartrue_Array[startIndex + k] = (temperatureRaw).ToString();
                                    }
                                }
                                else if (sequenceNumber == 0x02)
                                {
                                    for (int k = 0; k < 6 - 4; k++)
                                    {
                                        int byteIndex = k + 2;
                                        double temperatureRaw = HexConverter.HexToSByte(data[byteIndex].ToString("X2"));
                                        model.Tempeartrue_Array[startIndex + k] = (temperatureRaw).ToString();
                                    }

                                    if (bindingType == 82)
                                    {
                                        this.BeginInvoke(() =>
                                        {
                                            BatteryDataList = new ObservableCollection<IBatteryData>(
                                                this.UpdateDataGrid<batterySocData>(batterySocDataList.OrderBy(t => Convert.ToInt32(t.SectionNumber)).ToList<batterySocData>(),
                                                new[] { "序号", "SOC" },
                                                data => data.SectionNumber,
                                                data => data.SOC,
                                                16,
                                                18));
                                        });
                                    }
                                }
                                #endregion

                                int startBatteryIndex = (sequenceNumber - 1) * 6 + (frameNumber - 1) * 12;

                                var batteryTemperatures = new string[6 + startBatteryIndex];
                                for (int k = 0; k < 6; k++)
                                {
                                    int byteIndex = k + 2;
                                    double temperatureRaw = HexConverter.HexToSByte(data[byteIndex].ToString("X2"));
                                    batteryTemperatures[startBatteryIndex + k] = (temperatureRaw).ToString();
                                }

                                // 修改或新增温度数据
                                for (int i = startBatteryIndex; i < batteryTemperatures.Length; i++)
                                {
                                    if (i <= Convert.ToInt32(224) - 1)
                                    {
                                        string sectionNumber = (i + 1).ToString();

                                        // 查找是否已存在该编号的记录
                                        var existingData = batteryTemperatureDataList.FirstOrDefault(data => data.SectionNumber == sectionNumber);
                                        if (existingData != null)
                                        {
                                            // 更新已有记录的温度值
                                            existingData.Temperature = batteryTemperatures[i];
                                        }
                                        else
                                        {
                                            // 新增记录
                                            batteryTemperatureDataList.Add(new batteryTemperatureData
                                            {
                                                SectionNumber = sectionNumber,
                                                Temperature = batteryTemperatures[i]
                                            });
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case 0x1683E0FF:
                    case 0x1683FFFF:
                        // Byte 1:帧序号 1~20（n）20帧 1帧6个电池SOC数据
                        frameNumber = data[0];
                        // Byte 2: 包序号 1~4（N）  共4包480个电池SOC数据
                        sequenceNumber = data[1];

                        // 实际调试384个电池SOC数据
                        if (frameNumber >= 0x01 && frameNumber <= 0x0C)
                        {
                            if (sequenceNumber >= 0x01 && sequenceNumber <= 0x03)
                            {
                                #region 临时代码
                                int startIndex = (sequenceNumber - 1) * 6 + (frameNumber - 1) * 16;

                                if (sequenceNumber >= 0x01 && sequenceNumber < 0x03)
                                {
                                    for (int k = 0; k < 6; k++)
                                    {
                                        int byteIndex = k + 2;
                                        double SocRaw = HexConverter.HexToByte(data[byteIndex].ToString("X2"));
                                        model.SOC_Array[startIndex + k] = SocRaw.ToString();
                                    }
                                }
                                else if (sequenceNumber == 0x03)
                                {
                                    for (int k = 0; k < 6 - 2; k++)
                                    {
                                        int byteIndex = k + 2;
                                        double SocRaw = HexConverter.HexToByte(data[byteIndex].ToString("X2"));
                                        model.SOC_Array[startIndex + k] = SocRaw.ToString();
                                    }

                                    if (bindingType == 83)
                                    {
                                        this.BeginInvoke(() =>
                                        {
                                            BatteryDataList = new ObservableCollection<IBatteryData>(
                                                this.UpdateDataGrid<batterySohData>(batterySohDataList.OrderBy(t => Convert.ToInt32(t.SectionNumber)).ToList<batterySohData>(),
                                                new[] { "序号", "SOH" },
                                                data => data.SectionNumber,
                                                data => data.SOH,
                                                16,
                                                18));
                                        });
                                    }
                                }
                                #endregion

                                int startBatteryIndex = (sequenceNumber - 1) * 6 + (frameNumber - 1) * 18;

                                var batterySocs = new string[6 + startBatteryIndex];
                                for (int k = 0; k < 6; k++)
                                {
                                    int byteIndex = k + 2;
                                    double SocRaw = HexConverter.HexToByte(data[byteIndex].ToString("X2"));
                                    batterySocs[startBatteryIndex + k] = SocRaw.ToString();
                                }

                                // 修改或新增SOC数据
                                for (int i = startBatteryIndex; i < batterySocs.Length; i++)
                                {
                                    string sectionNumber = (i + 1).ToString();

                                    // 查找是否已存在该编号的记录
                                    var existingData = batterySocDataList.FirstOrDefault(data => data.SectionNumber == sectionNumber);
                                    if (existingData != null)
                                    {
                                        // 更新已有记录的电压值
                                        existingData.SOC = batterySocs[i];
                                    }
                                    else
                                    {
                                        // 新增记录
                                        batterySocDataList.Add(new batterySocData
                                        {
                                            SectionNumber = sectionNumber,
                                            SOC = batterySocs[i]
                                        });
                                    }
                                }
                            }
                        }
                        break;
                    case 0x1684E0FF:
                    case 0x1684FFFF:
                        // Byte 1:帧序号 1~20（n）
                        frameNumber = data[0];
                        // Byte 2: 包序号 1~4（N）（同请求包序号）                   
                        sequenceNumber = data[1];
                        if (frameNumber >= 0x01 && frameNumber <= 0x0C)
                        {
                            if (sequenceNumber >= 0x01 && sequenceNumber <= 0x03)
                            {
                                #region 临时代码
                                int startIndex = (sequenceNumber - 1) * 6 + (frameNumber - 1) * 16;

                                if (sequenceNumber >= 0x01 && sequenceNumber < 0x03)
                                {
                                    for (int k = 0; k < 6; k++)
                                    {
                                        int byteIndex = k + 2;
                                        double SocRaw = HexConverter.HexToByte(data[byteIndex].ToString("X2"));
                                        model.SOH_Array[startIndex + k] = SocRaw.ToString();
                                    }
                                }
                                else if (sequenceNumber == 0x03)
                                {
                                    for (int k = 0; k < 6 - 2; k++)
                                    {
                                        int byteIndex = k + 2;
                                        double SocRaw = HexConverter.HexToByte(data[byteIndex].ToString("X2"));
                                        model.SOH_Array[startIndex + k] = SocRaw.ToString();
                                    }
                                    if (bindingType == 84)
                                    {
                                        this.BeginInvoke(() =>
                                        {
                                            BatteryDataList = new ObservableCollection<IBatteryData>(
                                                this.UpdateDataGrid<batteryTemperatureData>(batteryTemperatureDataList.OrderBy(t => Convert.ToInt32(t.SectionNumber)).ToList<batteryTemperatureData>(),
                                                new[] { "序号", "温度" },
                                                data => data.SectionNumber,
                                                data => data.Temperature,
                                                8,
                                                12));
                                        });
                                    }
                                }
                                #endregion

                                int startBatteryIndex = (sequenceNumber - 1) * 6 + (frameNumber - 1) * 18;

                                var batterySohs = new string[6 + startBatteryIndex];
                                for (int k = 0; k < 6; k++)
                                {
                                    int byteIndex = k + 2;
                                    double SohRaw = HexConverter.HexToByte(data[byteIndex].ToString("X2"));
                                    batterySohs[startBatteryIndex + k] = SohRaw.ToString();
                                }

                                // 修改或新增SOH数据
                                for (int i = startBatteryIndex; i < batterySohs.Length; i++)
                                {
                                    string sectionNumber = (i + 1).ToString();

                                    // 查找是否已存在该编号的记录
                                    var existingData = batterySohDataList.FirstOrDefault(data => data.SectionNumber == sectionNumber);
                                    if (existingData != null)
                                    {
                                        // 更新已有记录的电压值
                                        existingData.SOH = batterySohs[i];
                                    }
                                    else
                                    {
                                        // 新增记录
                                        batterySohDataList.Add(new batterySohData
                                        {
                                            SectionNumber = sectionNumber,
                                            SOH = batterySohs[i]
                                        });
                                    }
                                }
                            }
                        }
                        break;
                    default: break;
                }
            }
            catch (Exception)
            {

            }
        }

        public void UpdateControlStatus(Dictionary<short, string> statusByteList, ushort value, int startIndex = 0)
        {
            foreach (var kvp in statusByteList)
            {
                short bitIndex = kvp.Key;
                string controlName = kvp.Value;
                var control = this.Controls.Find(controlName, true).FirstOrDefault() as PictureBox;
                if (control != null)
                {
                    control.BackColor = MyCustomConverter.GetBit((byte)value, bitIndex) == 0 ? Color.Red : Color.Green;
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

        #endregion

        // 切换单簇多PACK信息
        private void rbBatterySwitch_Click(object sender, EventArgs e)
        {

            switch (((ButtonBase)sender).Text)
            {
                case "电池电压":
                case "BatteryVoltage":
                    bindingType = 81;
                    this.BeginInvoke(() =>
                    {
                        BatteryDataList = new ObservableCollection<IBatteryData>(
                            this.UpdateDataGrid<batteryVoltageData>(batteryVoltageDataList.OrderBy(t => Convert.ToInt32(t.SectionNumber)).ToList<batteryVoltageData>(),
                            new[] { "序号", "电压" },
                            data => data.SectionNumber,
                            data => data.Voltage,
                            16,
                            18));
                    });
                    break;

                case "电池SOC":
                case "BatterySOC":
                    bindingType = 82;
                    this.BeginInvoke(() =>
                    {
                        BatteryDataList = new ObservableCollection<IBatteryData>(
                            this.UpdateDataGrid<batterySocData>(batterySocDataList.OrderBy(t => Convert.ToInt32(t.SectionNumber)).ToList<batterySocData>(),
                            new[] { "序号", "SOC" },
                            data => data.SectionNumber,
                            data => data.SOC,
                            16,
                            18));
                    });
                    break;
                case "电池SOH":
                case "BatterySOH":
                    bindingType = 83;
                    this.BeginInvoke(() =>
                    {
                        BatteryDataList = new ObservableCollection<IBatteryData>(
                            this.UpdateDataGrid<batterySohData>(batterySohDataList.OrderBy(t => Convert.ToInt32(t.SectionNumber)).ToList<batterySohData>(),
                            new[] { "序号", "SOH" },
                            data => data.SectionNumber,
                            data => data.SOH,
                            16,
                            18));
                    });
                    break;
                case "电池温度":
                case "BatteryTemperature":
                    bindingType = 84;
                    this.BeginInvoke(() =>
                    {
                        BatteryDataList = new ObservableCollection<IBatteryData>(
                            this.UpdateDataGrid<batteryTemperatureData>(batteryTemperatureDataList.OrderBy(t => Convert.ToInt32(t.SectionNumber)).ToList<batteryTemperatureData>(),
                            new[] { "序号", "温度" },
                            data => data.SectionNumber,
                            data => data.Temperature,
                            8,
                            12));
                    });
                    break;
                default:
                    break;
            }
        }

        private List<T> UpdateDataGrid<T>(List<T> dataList, string[] headers, Func<T, string> getPrimaryValue, Func<T, string> getSecondaryValue, int validityPerGroup, int maxSectionsPerGroup) where T : IBatteryData, new()
        {
            var tempDataList = new List<T>(dataList);
            dataList.Clear();

            if (tempDataList.Count >= 1)
            {
                // 列数和行数
                int totalColumns = 16, totalRows = 12;

                string[] dataName = new string[totalRows];
                for (int i = 0; i < totalRows; i++)
                {
                    int bmmIndex = (i / 1) + 1;
                    dataName[i] = $"BMU{bmmIndex}";
                }

                string[,] values = new string[totalColumns, totalRows];
                for (int currentIndex = 0; currentIndex < tempDataList.Count; currentIndex += maxSectionsPerGroup)
                {
                    // 新的数组
                    List<T> list = tempDataList.Skip(currentIndex).Take(validityPerGroup).ToList();
                    int startIndex = 0;
                    for (int col = 0; col < list.Count; col++)
                    {
                        var data = list[col];

                        startIndex = Convert.ToInt32(data.SectionNumber.Replace("#", "").ToString());
                        int row = currentIndex == 0 ? 0 : startIndex / maxSectionsPerGroup;

                        values[col, row] = getSecondaryValue(data);
                    }
                }

                for (int i = 0; i < totalRows; i++)
                {
                    var rowData = new T
                    {
                        DataName = dataName[i],
                    };

                    // 使用循环赋值，动态处理列
                    for (int col = 0; col < totalColumns; col++)
                    {
                        typeof(T).GetProperty($"Value{col + 1}")?.SetValue(rowData, values[col, i]);
                    }

                    dataList.Add(rowData);
                }
            }

            return dataList;
        }

        private void AppendDataToCsv(string filePath, string data)
        {
            int retryCount = 0;
            while (retryCount < MAX_RETRIES)
            {
                try
                {
                    lock (_fileLock)
                    {
                        // 确保目录存在（首次尝试时创建）
                        string directory = Path.GetDirectoryName(filePath);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        // 检查日期变更
                        string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                        if (currentDate != _fileState.Date)
                        {
                            _fileState = (currentDate, false);
                        }

                        // 使用更安全的文件访问模式
                        using (var stream = new FileStream(
                            filePath,
                            FileMode.OpenOrCreate,
                            FileAccess.Write,
                            FileShare.ReadWrite)) // 允许其他进程读取
                        {
                            bool isNewFile = stream.Length == 0;
                            bool writeHeader = isNewFile || !_fileState.HeaderWritten;

                            using (var sw = new StreamWriter(stream, Encoding.UTF8))
                            {
                                // 新文件处理
                                if (writeHeader)
                                {
                                    if (isNewFile)
                                    {
                                        stream.Position = 0;
                                    }
                                    else // 现有文件但未写入表头
                                    {
                                        // 移动到文件末尾追加表头
                                        stream.Position = stream.Length;
                                        if (stream.Length > 0)
                                        {
                                            sw.WriteLine(); // 确保新行
                                        }
                                    }

                                    sw.WriteLine(model.GetHeader());
                                    _fileState.HeaderWritten = true;
                                }
                                else
                                {
                                    stream.Position = stream.Length;
                                }

                                sw.WriteLine(data);
                            }
                        }
                        return; // 成功写入后退出
                    }
                }
                catch (IOException ex) when (IsFileLocked(ex))
                {
                    retryCount++;
                    if (retryCount >= MAX_RETRIES)
                    {
                        // 最终失败处理
                        this.BeginInvoke((Action)(() =>
                            MessageBox.Show($"无法写入文件：{filePath}\n文件被其他程序锁定。",
                                          "文件访问错误",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning)));
                        return;
                    }

                    // 指数退避重试
                    int delay = RETRY_DELAY_MS * (int)Math.Pow(2, retryCount);
                    Thread.Sleep(delay);
                }
                catch (Exception ex)
                {
                    this.BeginInvoke((Action)(() =>
                        MessageBox.Show($"写入错误: {ex.Message}", "数据记录错误")));
                    return;
                }
            }
        }

        // 检测文件锁定异常
        private bool IsFileLocked(IOException ex)
        {
            int errorCode = Marshal.GetHRForException(ex) & 0xFFFF;
            return errorCode == 32 || errorCode == 33; // 32=共享冲突, 33=进程锁定
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

        public void ClearInputControls(Control control)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)(() =>
                {
                    foreach (Control ctrl in control.Controls)
                    {
                        // 清空当前控件（如果是文本框或富文本框）
                        if (ctrl is TextBox)
                            ((TextBox)ctrl).Clear();
                        else if (ctrl is RichTextBox)
                            ((RichTextBox)ctrl).Clear();

                        // 递归处理容器控件
                        if (ctrl.HasChildren)
                            ClearInputControls(ctrl);
                    }
                }));
            }
            else
            {
                foreach (Control ctrl in control.Controls)
                {
                    // 清空当前控件（如果是文本框或富文本框）
                    if (ctrl is TextBox)
                        ((TextBox)ctrl).Clear();
                    else if (ctrl is RichTextBox)
                        ((RichTextBox)ctrl).Clear();

                    // 递归处理容器控件
                    if (ctrl.HasChildren)
                        ClearInputControls(ctrl);
                }
            }

        }

        #region BMS发送内部电池故障信息-模块处理
        /// <summary>
        /// 解析日志内容
        /// </summary>
        /// <param name="_bytes"></param>
        /// <returns></returns>
        private void AnalysisInsideFaultInfo(byte[] data, int faultNum)
        {
            string[] msg = new string[2];

            for (int i = 0; i < data.Length; i++)
            {
                for (short j = 0; j < 8; j++)
                {
                    if (MyCustomConverter.GetBit(data[i], j) == 1)
                    {
                        getLog_Info12(out msg, i, j, faultNum);
                        string type = "";
                        switch (msg[1])
                        {
                            case "1":
                                if (msg[0].StartsWith("预留") || richTextBox1.Text.Contains(msg[0]))
                                    continue;

                                richTextBox1.AppendText(msg[0] + "\r");
                                type = "故障";
                                break;
                            case "2":
                                if (msg[0].StartsWith("预留") || richTextBox2.Text.Contains(msg[0]))
                                    continue;

                                richTextBox2.AppendText(msg[0] + "\r");
                                type = "保护";
                                break;
                            case "3":
                                if (msg[0].StartsWith("预留") || richTextBox3.Text.Contains(msg[0]))
                                    continue;

                                richTextBox3.AppendText(msg[0] + "\r");
                                type = "告警";
                                break;
                            case "4":
                                if (msg[0].StartsWith("预留") || richTextBox4.Text.Contains(msg[0]))
                                    continue;

                                richTextBox4.AppendText(msg[0] + "\r");
                                type = "提示";
                                break;
                        }

                        var query = FrmMain.AlarmList.FirstOrDefault(t => t.Id == FrmMain.BMS_ID && t.Content == msg[0]);
                        if (query == null)
                        {
                            FrmMain.AlarmList.Add(new AlarmInfo()
                            {
                                DataTime = DateTime.Now.ToString("yy-MM-dd HH:mm:ss"),
                                Id = FrmMain.BMS_ID,
                                Type = type,
                                Content = $"{msg[0]}"
                            });
                        }
                    }
                    else
                    {
                        getLog_Info12(out msg, i, j, faultNum);
                        string type = "";
                        switch (msg[1])
                        {
                            case "1":
                                type = "故障";
                                break;
                            case "2":
                                type = "保护";
                                break;
                            case "3":
                                type = "告警";
                                break;
                            case "4":
                                type = "提示";
                                break;
                        }

                        var query = FrmMain.AlarmList.FirstOrDefault(t => t.Id == FrmMain.BMS_ID && t.Type == type && t.Content == msg[0] && t.State == 0);
                        if (query != null)
                        {
                            query.State = 1;
                            FrmMain.AlarmList.Add(new AlarmInfo()
                            {
                                DataTime = DateTime.Now.ToString("yy-MM-dd HH:mm:ss"),
                                State = 1,
                                Id = FrmMain.BMS_ID,
                                Type = type,
                                Content = $"[解除]{msg[0]}"
                            });

                            if (type == "故障")
                                richTextBox1.Text = richTextBox1.Text.Replace($"{msg[0]}\n", "");
                            else if (type == "保护")
                                richTextBox2.Text = richTextBox2.Text.Replace($"{msg[0]}\n", "");
                            else if (type == "告警")
                                richTextBox3.Text = richTextBox3.Text.Replace($"{msg[0]}\n", "");
                            else if (type == "提示")
                                richTextBox4.Text = richTextBox4.Text.Replace($"{msg[0]}\n", "");

                        }
                    }
                }
            }
        }
        public static string[] getLog_Info12(out string[] msg, int row, int column, int faultNum = 0)
        {
            msg = new string[2];
            List<FaultInfo> faultInfos = FaultInfo.FaultInfos;
            switch (faultNum)
            {
                case 0:
                    faultInfos = FaultInfo.FaultInfos;// 0x08:BMS发送内部电池故障信息1     BCU 0x0C5
                    break;
                case 1:
                    faultInfos = FaultInfo.FaultInfos2;//0x45:BMS发送内部电池故障信息2     BCU 0x0C6
                    break;
                case 2:
                    faultInfos = FaultInfo.FaultInfos3;//0x0C3:BCU故障上报1
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

        private void analysisLog(byte[] data, int faultNum, string devType = "CBS5000")
        {
            string[] msg = new string[2];

            for (int i = 0; i < data.Length; i++)
            {
                for (short j = 0; j < 8; j++)
                {
                    if (MyCustomConverter.GetBit(data[i], j) == 1)
                    {
                        getLog(out msg, i + 1, j, faultNum, devType);
                        string type = "";
                        switch (faultNum)
                        {
                            case 1:
                                if (devType == "CBS5000")
                                {
                                    if (msg[0].StartsWith("预留") || richTextBox1.Text.Contains(msg[0]))
                                        continue;

                                    richTextBox1.AppendText(msg[0] + "\r");
                                }
                                else
                                {
                                    if (msg[0].StartsWith("预留") || richTextBox5.Text.Contains(msg[0]))
                                        continue;

                                    richTextBox5.AppendText(msg[0] + "\r");
                                }
                                type = "故障";
                                break;
                            case 2:
                                if (msg[0].StartsWith("预留") || richTextBox2.Text.Contains(msg[0]))
                                    continue;

                                richTextBox2.AppendText(msg[0] + "\r");
                                type = "保护";
                                break;
                            case 3:
                                if (msg[0].StartsWith("预留") || richTextBox3.Text.Contains(msg[0]))
                                    continue;

                                richTextBox3.AppendText(msg[0] + "\r");
                                type = "告警";
                                break;
                            case 4:
                                if (msg[0].StartsWith("预留") || richTextBox4.Text.Contains(msg[0]))
                                    continue;

                                richTextBox4.AppendText(msg[0] + "\r");
                                type = "提示";
                                break;
                            default:
                                break;
                        }

                        var query = FrmMain.AlarmList.FirstOrDefault(t => t.Id == FrmMain.BMS_ID && t.Content == msg[0]);
                        if (query == null)
                        {
                            FrmMain.AlarmList.Add(new AlarmInfo()
                            {
                                DataTime = DateTime.Now.ToString("yy-MM-dd HH:mm:ss"),
                                Id = FrmMain.BMS_ID,
                                Type = type,
                                Content = msg[0]
                            });
                        }
                    }
                    else
                    {
                        getLog(out msg, i + 1, j, faultNum, devType);
                        string type = "";
                        switch (msg[1])
                        {
                            case "1":
                                type = "故障";
                                break;
                            case "2":
                                type = "保护";
                                break;
                            case "3":
                                type = "告警";
                                break;
                            case "4":
                                type = "提示";
                                break;
                        }

                        var query = FrmMain.AlarmList.FirstOrDefault(t => t.Id == FrmMain.BMS_ID && t.Type == type && t.Content == msg[0] && t.State == 0);
                        if (query != null)
                        {
                            query.State = 1;
                            FrmMain.AlarmList.Add(new AlarmInfo()
                            {
                                DataTime = DateTime.Now.ToString("yy-MM-dd HH:mm:ss"),
                                State = 1,
                                Id = FrmMain.BMS_ID,
                                Type = type,
                                Content = $"[解除]{msg[0]}"
                            });
                        }

                        if (type == "故障")
                        {
                            if (devType == "CBS5000")
                                richTextBox1.Text = richTextBox1.Text.Replace($"{msg[0]}\n", "");
                            else
                                richTextBox5.Text = richTextBox5.Text.Replace($"{msg[0]}\n", "");
                        }
                        else if (type == "保护")
                        {
                            richTextBox2.Text = richTextBox2.Text.Replace($"{msg[0]}\n", "");
                        }
                        else if (type == "告警")
                        {
                            richTextBox3.Text = richTextBox3.Text.Replace($"{msg[0]}\n", "");
                        }
                        else if (type == "提示")
                        {
                            richTextBox4.Text = richTextBox4.Text.Replace($"{msg[0]}\n", "");
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
                    faultInfos = FaultInfo.FaultNotify;
                    if (devType != "CBS5000")
                    {
                        faultInfos = FaultInfo.FaultNotify_DCDC;
                    }
                    break;
                case 2:
                    faultInfos = FaultInfo.ProtectNotify;
                    break;
                case 3:
                    faultInfos = FaultInfo.WarningNotify;
                    break;
                case 4:
                    faultInfos = FaultInfo.PromptNotify;
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
        #endregion

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

        public static void LTooltip(Control control, string value)
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

    }
}