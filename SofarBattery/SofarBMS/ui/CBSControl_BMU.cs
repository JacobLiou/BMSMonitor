using Sofar.BMS;
using Sofar.BMS.Common;
using Sofar.BMS.Models;
using Sofar.ConnectionLibs.CAN;
using SofarBMS.Helper;
using SofarBMS.Model;
using System.Diagnostics;
using System.Text;

namespace SofarBMS.ui
{
    public partial class CBSControl_BMU : UserControl
    {
        public int SelectedRequest7 => FrmMain.BMS_ID;

        private string[] packSN = new string[3];

        public static CancellationTokenSource cts = null;
        private EcanHelper ecanHelper = EcanHelper.Instance;
        public BMURealDataVM RealDataVM = new BMURealDataVM();
        public BmuRealtimeData model { get; set; } = new();

        private List<batteryVoltageData> batteryVoltageDataList = new();
        private List<batterySocData> batterySocDataList = new();
        private List<batterySohData> batterySohDataList = new();
        private List<batteryTemperatureData> batteryTemperatureDataList = new();
        private List<batteryEquilibriumStateData> batteryEquilibriumStateDataList = new();
        private List<batteryEquilibriumTemperatureData> batteryEquilibriumTemperatureDataList = new();

        public CBSControl_BMU()
        {
            InitializeComponent();
        }

        private void CBSControl_BMU_Load(object sender, EventArgs e)
        {
            this.Invoke(() =>
            {
                foreach (Control item in this.Controls)
                {
                    GetControls(item);
                }
            });

            int p = -1;
            cts = new CancellationTokenSource();
            Task.Run(async delegate
            {
                while (!cts.IsCancellationRequested)
                {
                    if (p == -1 || p != FrmMain.BMS_ID)
                    {
                        p = FrmMain.BMS_ID;
                        this.Invoke(() => { ClearInputControls(this); });
                    }

                    //if (model != null && initCount >= InitCountThreshold)
                    //{
                    //    //var fileName = $"BTS5K_{DateTime.Now:yyyy-MM-dd}.csv";
                    //    //var filePath = Path.Combine(logDirectory, fileName);

                    //    //lock (_fileLock)
                    //    //{
                    //    //    if (!File.Exists(filePath))
                    //    //    {
                    //    //        InitLogWriter(filePath);
                    //    //    }
                    //    //    _logWriter?.WriteLine(model.GetValue());
                    //    //}

                    //    //initCount = 0;
                    //    //model = new RealtimeData_BTS5K();
                    //}


                    if (ecanHelper.IsConnected)
                    {
                        RealDataVM.SelectedRequest7 = FrmMain.BMS_ID;// 切换ID时，需同步更新
                        RealDataVM.ReadAllParameter();
                        await Task.Delay(1000);
                    }
                }
            });

            ecanHelper.AnalysisDataInvoked += RealDataVM_AnalysisDataInvoked;
        }

        private void RealDataVM_AnalysisDataInvoked(object? sender, object e)
        {
            if (cts.IsCancellationRequested && ecanHelper.IsConnected)
            {
                ecanHelper.AnalysisDataInvoked -= RealDataVM_AnalysisDataInvoked;
                return;
            }

            var frameModel = e as CanFrameModel;
            if (frameModel != null)
            {
                this.Invoke((Delegate)(() => { AnalysisData(frameModel.CanID, frameModel.Data); }));
            }
        }

        public void ClearInputControls(Control control)
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
        #region 帧数据解析
        /// <summary>
        /// 一级数据
        /// </summary>
        /// <param name="canID"></param>
        /// <param name="data"></param>
        private void AnalysisData(uint canID, byte[] data)
        {
            if ((canID & 0xff) != FrmMain.BMS_ID)
                return;

            string[] strs;
            string[] controls;
            byte[] canid = BitConverter.GetBytes(canID);

            try
            {
                switch (canID | 0xff)
                {
                    case 0x1027E0FF:
                        string strSn = AnalysisSN(data);
                        if (!string.IsNullOrEmpty(strSn)) txtSN.Text = strSn;

                        model.SerialNumber = strSn;
                        break;
                    case 0x1004FFFF:
                    case 0x1004E0FF:
                        strs = new string[4] { "0.1", "0.1", "0.01", "0.1" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                        }
                        controls = new string[4] { "txtBatteryVolt", "txtLoadVolt", "txtBatteryCurrent", "txtSOC" };
                        for (int i = 0; i < controls.Length; i++)
                        {
                            (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                        }

                        model.BatteryVoltage = Convert.ToDouble(strs[0]);
                        model.LoadVoltage = Convert.ToDouble(strs[1]);
                        model.BatteryCurrent = Convert.ToDouble(strs[2]);
                        model.SOC = Convert.ToDouble(strs[3]);
                        break;
                    case 0x1005FFFF:
                    case 0x1005E0FF:
                        strs = new string[5];
                        txtBatMaxCellVolt.Text = strs[0] = BytesToIntger(data[1], data[0]);
                        txtBatMaxCellVoltNum.Text = strs[1] = BytesToIntger(0x00, data[2]);
                        txtBatMinCellVolt.Text = strs[2] = BytesToIntger(data[4], data[3]);
                        txtBatMinCellVoltNum.Text = strs[3] = BytesToIntger(0x00, data[5]);
                        txtBatDiffCellVolt.Text = (Convert.ToInt32(strs[0]) - Convert.ToInt32(strs[2])).ToString();

                        model.MaxVoltageCellId = ushort.Parse(strs[1]);
                        model.MaxVoltage = ushort.Parse(strs[0]);
                        model.MinVoltageCellId = ushort.Parse(strs[3]);
                        model.MinVoltage = ushort.Parse(strs[2]);
                        model.VoltageDifference = ushort.Parse((Convert.ToInt32(strs[0]) - Convert.ToInt32(strs[2])).ToString());
                        break;
                    case 0x1006FFFF:
                    case 0x1006E0FF:
                        strs = new string[4] { "0.1", "1", "0.1", "1" };
                        txtBatMaxCellTemp.Text = strs[0] = BytesToIntger(data[1], data[0], 0.1);
                        txtBatMaxCellTempNum.Text = strs[1] = BytesToIntger(0x00, data[2]);
                        txtBatMinCellTemp.Text = strs[2] = BytesToIntger(data[4], data[3], 0.1);
                        txtBatMinCellTempNum.Text = strs[3] = BytesToIntger(0x00, data[5]);

                        model.MaxTemperatureCellId = ushort.Parse(strs[1]);
                        model.MaxTemperature = Convert.ToDouble(strs[0]);
                        model.MinTemperatureCellId = ushort.Parse(strs[3]);
                        model.MinTemperature = Convert.ToDouble(strs[2]);
                        break;
                    case 0x1007FFFF:
                    case 0x1007E0FF:
                        string totalDischargedCapacity = txtTotalChgCap.Text = (Convert.ToDouble(((data[3] << 24) + (data[2] << 16) + (data[1] << 8) + (data[0] & 0xff)) * 0.001)).ToString();
                        string totalChargedCapacity = txtTotalDsgCap.Text = (Convert.ToDouble(((data[7] << 24) + (data[6] << 16) + (data[5] << 8) + (data[4] & 0xff)) * 0.001)).ToString();

                        model.TotalDischargedCapacity = Convert.ToDouble(totalDischargedCapacity);
                        model.TotalChargedCapacity = Convert.ToDouble(totalChargedCapacity);
                        break;
                    case 0x1008FFFF:
                    case 0x1008E0FF:
                        //报警信息                     
                        AnalysisLog(data, 1);
                        break;
                    case 0x1009FFFF:
                    case 0x1009E0FF:
                        //BMS电池单体电压1-4
                        strs = new string[4];
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2]);
                        }
                        controls = new string[4] { "txtCellvoltage1", "txtCellvoltage2", "txtCellvoltage3", "txtCellvoltage4" };
                        for (int i = 0; i < controls.Length; i++)
                        {
                            (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                        }

                        model.CellVoltage1 = Convert.ToUInt16(strs[0]);
                        model.CellVoltage2 = Convert.ToUInt16(strs[1]);
                        model.CellVoltage3 = Convert.ToUInt16(strs[2]);
                        model.CellVoltage4 = Convert.ToUInt16(strs[3]);
                        break;
                    case 0x100AFFFF:
                    case 0x100AE0FF:
                        //BMS电池单体电压5-8
                        strs = new string[4];
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2]);
                        }
                        controls = new string[4] { "txtCellvoltage5", "txtCellvoltage6", "txtCellvoltage7", "txtCellvoltage8" };
                        for (int i = 0; i < controls.Length; i++)
                        {
                            (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                        }

                        model.CellVoltage5 = Convert.ToUInt16(strs[0]);
                        model.CellVoltage6 = Convert.ToUInt16(strs[1]);
                        model.CellVoltage7 = Convert.ToUInt16(strs[2]);
                        model.CellVoltage8 = Convert.ToUInt16(strs[3]);
                        break;
                    case 0x100BFFFF:
                    case 0x100BE0FF:
                        //BMS电池单体电压9-12
                        strs = new string[4];
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2]);
                        }
                        controls = new string[4] { "txtCellvoltage9", "txtCellvoltage10", "txtCellvoltage11", "txtCellvoltage12" };
                        for (int i = 0; i < controls.Length; i++)
                        {
                            (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                        }

                        model.CellVoltage9 = Convert.ToUInt16(strs[0]);
                        model.CellVoltage10 = Convert.ToUInt16(strs[1]);
                        model.CellVoltage11 = Convert.ToUInt16(strs[2]);
                        model.CellVoltage12 = Convert.ToUInt16(strs[3]);
                        break;
                    case 0x100CFFFF:
                    case 0x100CE0FF:
                        //BMS电池单体电压13-16
                        strs = new string[4];
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2]);
                        }
                        controls = new string[4] { "txtCellvoltage13", "txtCellvoltage14", "txtCellvoltage15", "txtCellvoltage16" };
                        for (int i = 0; i < controls.Length; i++)
                        {
                            (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                        }

                        model.CellVoltage13 = Convert.ToUInt16(strs[0]);
                        model.CellVoltage14 = Convert.ToUInt16(strs[1]);
                        model.CellVoltage15 = Convert.ToUInt16(strs[2]);
                        model.CellVoltage16 = Convert.ToUInt16(strs[3]);
                        break;
                    case 0x100DFFFF:
                    case 0x100DE0FF:
                        //BMS电池温度1-4
                        strs = new string[4] { "0.1", "0.1", "0.1", "0.1" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                        }
                        controls = new string[4] { "txtCelltemperature1", "txtCelltemperature2", "txtCelltemperature3", "txtCelltemperature4" };
                        for (int i = 0; i < controls.Length; i++)
                        {
                            (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                        }

                        model.CellTemperature1 = Convert.ToUInt16(strs[0]);
                        model.CellTemperature2 = Convert.ToUInt16(strs[1]);
                        model.CellTemperature3 = Convert.ToUInt16(strs[2]);
                        model.CellTemperature4 = Convert.ToUInt16(strs[3]);
                        break;
                    case 0x100EFFFF:
                    case 0x100EE0FF:
                        strs = new string[3] { "0.1", "0.1", "0.1" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                        }
                        controls = new string[3] { "txtMosTemperature", "txtEnvTemperature", "txtSOH" };
                        for (int i = 0; i < controls.Length; i++)
                        {
                            (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                        }

                        model.MosTemperature = Convert.ToDouble(strs[0]);
                        model.AmbientTemperature = Convert.ToDouble(strs[1]);
                        model.SOH = Convert.ToDouble(strs[2]);
                        break;
                    case 0x100FFFFF:
                    case 0x100FE0FF:
                        strs = new string[3] { "0.1", "0.1", "1" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                        }
                        controls = new string[3] { "txtRemainingCapacity", "txtFullCapacity", "txtCycleTime" };
                        for (int i = 0; i < controls.Length; i++)
                        {
                            (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                        }

                        model.RemainingCapacity = Convert.ToDouble(strs[0]);
                        model.FullCapacity = Convert.ToDouble(strs[1]);
                        model.CycleTime = ushort.Parse(strs[2]);
                        break;
                    case 0x1040FFFF:
                    case 0x1040E0FF:
                        string CumulativeDischargeCapacity = txtCumulativeDischargeCapacity.Text = ((data[3] << 24) + (data[2] << 16) + (data[1] << 8) + (data[0] & 0xff)).ToString();
                        string CumulativeChargeCapacity = txtCumulativeChargeCapacity.Text = (((data[7] << 24) + (data[6] << 16) + (data[5] << 8) + (data[4] & 0xff))).ToString();

                        model.CumulativeDischargeCapacity = Convert.ToDouble(CumulativeDischargeCapacity);
                        model.CumulativeChargeCapacity = Convert.ToDouble(CumulativeChargeCapacity);
                        break;
                    case 0x1042FFFF:
                    case 0x1042E0FF:
                        //BMS均衡温度5-8
                        strs = new string[4] { "0.1", "0.1", "0.1", "0.1" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                        }
                        controls = new string[4] { "txtCelltemperature5", "txtCelltemperature6", "txtCelltemperature7", "txtCelltemperature8" };
                        for (int i = 0; i < controls.Length; i++)
                        {
                            (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                        }

                        model.CellTemperature5 = Convert.ToUInt16(strs[0]);
                        model.CellTemperature6 = Convert.ToUInt16(strs[1]);
                        model.CellTemperature7 = Convert.ToUInt16(strs[2]);
                        model.CellTemperature8 = Convert.ToUInt16(strs[3]);
                        break;
                    case 0x104AFFFF:
                    case 0x104AE0FF:
                        strs = new string[4] { "0.1", "0.1", "0.1", "0.1" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                        }
                        controls = new string[4] { "txtPowerTemperture1", "txtPowerTemperture2", "txtDcdcTemperature1", "txtDcdcTemperature2" };
                        for (int i = 0; i < controls.Length; i++)
                        {
                            (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                        }

                        //P+端子温度，P-端子温度,主动均衡温度1,主动均衡温度2
                        model.PositiveTerminalTemperature = Convert.ToDouble(strs[0]);
                        model.NegativeTerminalTemperature = Convert.ToDouble(strs[1]);
                        model.ActiveBalanceTemperature1 = Convert.ToDouble(strs[2]);
                        model.ActiveBalanceTemperature2 = Convert.ToDouble(strs[3]);
                        break;
                    case 0x104BFFFF:
                    case 0x104BE0FF:
                        AnalysisLog(data, 3);
                        break;
                    case 0x1045FFFF:
                    case 0x1045E0FF:
                        AnalysisLog(data, 2);
                        break;
                    case 0x1046FFFF:
                    case 0x1046E0FF:
                        //加热请求
                        string HeatRequest = "0:无请求无禁止 1:请求加热 2:禁止加热";
                        switch (data[1] & 0x03)
                        {
                            case 0: HeatRequest = "NONE"; break;
                            case 1: HeatRequest = "请求加热"; break;
                            case 2: HeatRequest = "禁止加热"; break;
                            default:
                                break;
                        }

                        pbChargeEnable.BackColor = MyCustomConverter.GetBit(data[0], 0) == 1 ? Color.Red : Color.Green;
                        pbDischargeEnable.BackColor = MyCustomConverter.GetBit(data[0], 1) == 1 ? Color.Red : Color.Green;
                        pbBmuCutOffRequest.BackColor = MyCustomConverter.GetBit(data[0], 2) == 1 ? Color.Red : Color.Green;
                        pbBmuPowOffRequest.BackColor = MyCustomConverter.GetBit(data[0], 3) == 1 ? Color.Red : Color.Green;
                        pbForceChrgRequest.BackColor = MyCustomConverter.GetBit(data[0], 4) == 1 ? Color.Red : Color.Green;
                        pbChargeStatus.BackColor = MyCustomConverter.GetBit(data[2], 0) == 1 ? Color.Red : Color.Green;
                        pbDischargeStatus.BackColor = MyCustomConverter.GetBit(data[2], 1) == 1 ? Color.Red : Color.Green;
                        pbDiIO.BackColor = MyCustomConverter.GetBit(data[6], 0) == 1 ? Color.Red : Color.Green;
                        pbChargeIO.BackColor = MyCustomConverter.GetBit(data[6], 1) == 1 ? Color.Red : Color.Green;
                        break;
                    case 0x1047FFFF:
                    case 0x1047E0FF:
                        string SyncFallSoc = Convert.ToInt32(data[0].ToString("X2"), 16).ToString();
                        string BmsStatus = Enum.Parse(typeof(BMSState), (Convert.ToInt32(data[1].ToString("X2"), 16) & 0x0f).ToString()).ToString();

                        string balanceStatus = "";
                        switch ((Convert.ToInt32(data[2].ToString("X2"), 16) & 0x0f))
                        {
                            case 0: balanceStatus = "禁止"; break;
                            case 1: balanceStatus = "放电"; break;
                            case 2: balanceStatus = "充电"; break;
                            default:
                                break;
                        }

                        txtSyncFallSoc.Text = SyncFallSoc;
                        txtBMSStatus.Text = BmsStatus;
                        txtActiveBalanceStatus.Text = balanceStatus;
                        txtChargeCurrentLimitation.Text = BytesToIntger(data[5], data[4], 0.01);
                        txtDischargeCurrentLimitation.Text = BytesToIntger(data[7], data[6], 0.01);
                        break;
                    case 0x1048FFFF:
                    case 0x1048E0FF:
                        strs = new string[4] { "0.1", "0.1", "1", "0.1" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                        }
                        controls = new string[4] { "txtBalanceBusVoltage", "txtBalanceCurrent", "txtActiveBalanceMaxCellVolt", "txtBatAverageTemp" };
                        for (int i = 0; i < controls.Length; i++)
                        {
                            (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                        }

                        model.BalanceBusVoltage = Convert.ToDouble(strs[0]);
                        model.BalanceCurrent = Convert.ToDouble(strs[1]);
                        model.ActiveBalanceMaxCellVoltage = Convert.ToDouble(strs[2]);
                        model.AverageCellTemperature = Convert.ToDouble(strs[3]);
                        break;
                    case 0x1049FFFF:
                    case 0x1049E0FF:
                        strs = new string[3] { "0.01", "1", "1" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                        }
                        controls = new string[3] { "txtActiveBalanceCellSoc", "txtActiveBalanceAccCap", "txtActiveBalanceRemainCap" };
                        for (int i = 0; i < controls.Length; i++)
                        {
                            (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                        }

                        model.ActiveBalanceCellSoc = Convert.ToDouble(strs[0]);
                        model.ActiveBalanceAccumulatedCapacity = Convert.ToDouble(strs[1]);
                        model.ActiveBalanceRemainingCapacity = Convert.ToDouble(strs[2]);
                        break;
                    case 0x106AFFFF:
                    case 0x106AE0FF:
                        string[] softwareVersion = new string[3];
                        for (int i = 0; i < 3; i++)
                        {
                            softwareVersion[i] = data[i + 1].ToString().PadLeft(2, '0');
                        }
                        string saftwareVersion = Encoding.ASCII.GetString(new byte[] { data[0] }) + string.Join("", softwareVersion);
                        txtBmuSaftwareVersion.Text = model.BMUSaftwareVersion = saftwareVersion;
                        txtBmuHardwareVersion.Text = model.BMUHardwareVersion = $"{(short)data[4]}";
                        txtBmuCanVersion.Text = model.BMUCanVersion = Encoding.ASCII.GetString(new byte[] { data[5], data[6] });
                        break;
                    case 0x106BFFFF:
                    case 0x106BE0FF:
                        strs = new string[3] { "0.1", "1", "1" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                        }
                        controls = new string[3] { "txtBatNominalCapacity", "txtRegisterName", "txtBatType" };
                        for (int i = 0; i < controls.Length; i++)
                        {
                            (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                        }

                        model.NominalCapacity = Convert.ToDouble(strs[0]);
                        model.RegisterName = strs[1];
                        model.BatteryType = strs[2];
                        break;
                    case 0x106CFFFF:
                    case 0x106CE0FF:
                        //厂家信息
                        StringBuilder manufacturerName = new StringBuilder();
                        manufacturerName.Append(Encoding.ASCII.GetString(data));
                        txtManufacturerName.Text = model.ManufacturerName = manufacturerName.ToString();
                        break;
                    case 0x106DFFFF:
                    case 0x106DE0FF:
                        strs = new string[3] { "0.001", "0.001", "0.001" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                        }
                        controls = new string[3] { "txtAuxVolt", "txtChgCurOffsetVolt", "txtDsgCurOffsetVolt" };
                        for (int i = 0; i < controls.Length; i++)
                        {
                            (this.Controls.Find(controls[i], true)[0] as TextBox).Text = strs[i];
                        }
                        txtResetMode.Text = model.ResetMode = Enum.Parse(typeof(Reset_Mode), (Convert.ToInt32(data[6].ToString("X2"), 16) & 0x0f).ToString()).ToString();

                        model.AuxiliaryVoltage = Convert.ToDouble(strs[0]);
                        model.ChargeCurrentOffset = Convert.ToDouble(strs[1]);
                        model.DischargeCurrentOffset = Convert.ToDouble(strs[2]);
                        break;
                    case 0x107081FF:
                    case 0x1070E0FF:
                        // Byte 1:帧序号M  1-10    一帧3个电池电压数据，10帧共30个电池电压数据
                        int frameNumber = data[0];
                        // Byte 2: 包序号N 8包 1包10帧  8包共240个电池电压数据
                        int sequenceNumber = data[1];
                        //调试时只有一个模块 共48个电池电压数据
                        if (sequenceNumber == 0x01)
                        {
                            if (frameNumber >= 0x01 && frameNumber <= 0x6)
                            {
                                int startBatteryIndex = GetBatteryStartIndex(sequenceNumber, frameNumber);
                                ProcessBatteryData(startBatteryIndex, data);
                            }

                            if (batteryVoltageDataList.Count >= 16)
                            {
                                for (int i = 1; i <= 16; i++)
                                {
                                    TextBox c = this.Controls.Find("txtCellvoltage" + i, true)[0] as TextBox;
                                    c.Text = batteryVoltageDataList[i - 1].Voltage.ToString();
                                }
                            }
                        }

                        int GetBatteryStartIndex(int sequenceNumber, int frameNumber)
                        {
                            // 每个包有48个电池电压数据，每帧3个电池电压数据                        
                            return (sequenceNumber - 1) * 48 + (frameNumber - 1) * 3;
                        }

                        void ProcessBatteryData(int startBatteryIndex, byte[] data)
                        {
                            var batteryVoltages = new string[3 + startBatteryIndex];

                            for (int k = 0; k < 3; k++)
                            {
                                int byteIndex = 2 + k * 2;
                                double voltageRaw = (data[byteIndex + 1] << 8) | data[byteIndex]; // [3:2] 2:低位，3:高位                                                    
                                batteryVoltages[startBatteryIndex + k] = (voltageRaw * 0.001).ToString("F3");// 电压数据 0.001V/bit 保留3位小数
                            }

                            for (int i = startBatteryIndex; i < batteryVoltages.Length; i++)
                            {
                                string sectionNumber = (i + 1).ToString() + "#";

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
                                        SectionNumber = (i + 1).ToString() + "#",
                                        Voltage = batteryVoltages[i]
                                    });
                                }
                            }
                        }

                        break;

                    //0x071：BMS电池温度+(M - 1)* 30+ (N - 1)*3 ~ 17+(M - 1)* 30 + (N)*3 
                    case 0x107181FF:
                    case 0x1071E0FF:
                        // Byte 1:帧序号 1~10 10帧 1帧3个电池温度数据
                        frameNumber = data[0];
                        // Byte 2: 包序号 1~N（电压按 30节电池划分的包数） 
                        sequenceNumber = data[1];

                        //实际调试30个电池温度数据
                        if (sequenceNumber >= 0x01)
                        {
                            if (frameNumber >= 0x01 && frameNumber <= 0x3)
                            {
                                int startBatteryIndex = GetbatteryTemperaturesStartIndex(sequenceNumber, frameNumber);
                                ProcessBatteryTemperature(startBatteryIndex, data);
                            }
                        }

                        if (batteryTemperatureDataList.Count >= 8)
                        {
                            for (int i = 1; i <= 8; i++)
                            {
                                TextBox c = this.Controls.Find("txtCelltemperature" + i, true)[0] as TextBox;
                                c.Text = batteryTemperatureDataList[i - 1].Temperature.ToString();
                            }
                        }

                        int GetbatteryTemperaturesStartIndex(int sequenceNumber, int frameNumber)
                        {
                            // 每个包有30个电池温度，每帧 3个电池温度数据                       
                            return (sequenceNumber - 1) * 30 + (frameNumber - 1) * 3;
                        }

                        void ProcessBatteryTemperature(int startBatteryIndex, byte[] data)
                        {
                            var batteryTemperatures = new string[3 + startBatteryIndex];
                            for (int k = 0; k < 3; k++)
                            {
                                int byteIndex = 2 + k * 2;
                                Int16 temperatureRaw = (short)((data[byteIndex + 1] << 8) | data[byteIndex]); // [3:2] 2:低位，3:高位
                                                                                                              // 温度数据 0.1/bit 
                                batteryTemperatures[startBatteryIndex + k] = (temperatureRaw * 0.1).ToString("F1");// 保留1位小数
                            }

                            for (int i = startBatteryIndex; i < batteryTemperatures.Length; i++)
                            {
                                // 如果索引为28或29，直接跳过当前循环
                                if (i == 28 || i == 29) continue;
                                string sectionNumber = (i + 1).ToString() + "#";

                                // 查找是否已存在该编号的记录
                                var existingData = batteryTemperatureDataList.FirstOrDefault(data => data.SectionNumber == sectionNumber);
                                if (existingData != null)
                                {
                                    // 更新已有记录的均衡状态值
                                    existingData.Temperature = batteryTemperatures[i];
                                }
                                else
                                {
                                    // 新增记录
                                    batteryTemperatureDataList.Add(new batteryTemperatureData
                                    {
                                        SectionNumber = (i + 1).ToString() + "#",
                                        Temperature = batteryTemperatures[i]
                                    });
                                }
                            }
                        }
                        break;
                    //0x072：BMS电池均衡状态 (电芯id>16)
                    case 0x107281FF:
                    case 0x1072E0FF:
                        // Byte 1:包序号 1~N  1包56个电池均衡状态数据
                        frameNumber = data[0];

                        int startBatteryIndex1 = GetbatteryEquilibriumStateDataStartIndex(frameNumber);
                        ProcessBatteryEquilibriumStateData(startBatteryIndex1, data);

                        //实际调试一包只有48个电池均衡状态数据
                        // 获取电池均衡状态数据的起始索引
                        int GetbatteryEquilibriumStateDataStartIndex(int frameNumber)
                        {
                            return (frameNumber - 1) * 48;
                        }

                        // 处理电池均衡状态数据
                        void ProcessBatteryEquilibriumStateData(int startBatteryIndex1, byte[] data)
                        {
                            var batteryEquilibriumStates = new string[48]; // 创建一个数组来存储当前 frame 的状态

                            for (int j = 0; j < 6; j++)
                            {
                                for (int k = 0; k < 8; k++)
                                {
                                    batteryEquilibriumStates[startBatteryIndex1 + k] = MyCustomConverter.GetBit(data[j + 1], (short)k).ToString();
                                }

                                for (int i = startBatteryIndex1; i < batteryEquilibriumStates.Length; i++)
                                {
                                    string sectionNumber = (i + 1).ToString() + "#";

                                    // 查找是否已存在该编号的记录
                                    var existingData = batteryEquilibriumStateDataList.FirstOrDefault(data => data.SectionNumber == sectionNumber);
                                    if (existingData != null)
                                    {
                                        // 更新已有记录的均衡状态值
                                        existingData.BatteryEquilibriumState = batteryEquilibriumStates[i];
                                    }
                                    else
                                    {
                                        // 新增记录
                                        batteryEquilibriumStateDataList.Add(new batteryEquilibriumStateData
                                        {
                                            SectionNumber = (i + 1).ToString() + "#",
                                            BatteryEquilibriumState = batteryEquilibriumStates[i]
                                        });
                                    }
                                }
                                startBatteryIndex1 += 8;
                            }
                        }


                        break;

                    //0x073：BMS均衡温度 N1=1+(N-1)*30+3*(M-1)
                    case 0x107381FF:
                    case 0x1073E0FF:
                        string balanceTemperature1 = txtBalance_temperature1.Text = BytesToIntger(data[3], data[2], 0.1);
                        string balanceTemperature2 = txtBalance_temperature2.Text = BytesToIntger(data[5], data[4], 0.1);

                        model.BalanceTemperature1 = Convert.ToDouble(balanceTemperature1);
                        model.BalanceTemperature2 = Convert.ToDouble(balanceTemperature2);
                        break;
                    // 0x0A0：BMS单电芯的SOC
                    case 0x10A0FFFF:
                    case 0x10A0E0FF:
                        // Byte 1:单电芯数据包组号(0~((PACK电芯个数/7) - 1))  224/7-1=31
                        frameNumber = data[0];

                        //调试数据 实际7包共48个电池SOC数据,最后一帧是48 % 7 = 6
                        if (frameNumber >= 0x00 && frameNumber <= 0x02)
                        {
                            int startBatteryIndex = GetbatterySOCStartIndex(frameNumber);
                            ProcessBatterySOC(startBatteryIndex, data);
                        }

                        //最后一帧
                        if (frameNumber == 0x02)
                        {
                            int startBatteryIndex = (frameNumber + 1) * (48 % 7);
                            ProcessBatterySOC(startBatteryIndex, data);

                            for (int i = 1; i <= 16; i++)
                            {
                                TextBox c = this.Controls.Find("txtSOC" + i, true)[0] as TextBox;
                                c.Text = batterySocDataList[i - 1].SOC.ToString();
                            }
                        }

                        int GetbatterySOCStartIndex(int frameNumber)
                        {
                            // 每个包有7个电池SOC数据                     
                            return frameNumber * 7;
                        }
                        var batterySocs = new string[48];
                        void ProcessBatterySOC(int startBatteryIndex, byte[] data)
                        {
                            if (frameNumber == 0x06)
                            {
                                batterySocs = new string[6 + startBatteryIndex];
                                for (int k = 0; k < 6; k++)
                                {
                                    int byteIndex = k + 1;
                                    double SocRaw = data[byteIndex];
                                    // SOC数据 1/bit 范围：0-255
                                    batterySocs[startBatteryIndex + k] = SocRaw.ToString();
                                }
                            }
                            else
                            {
                                batterySocs = new string[7 + startBatteryIndex];
                                for (int k = 0; k < 7; k++)
                                {
                                    int byteIndex = k + 1;
                                    double SocRaw = data[byteIndex];
                                    // SOC数据 1/bit 范围：0-255
                                    batterySocs[startBatteryIndex + k] = SocRaw.ToString();
                                }
                            }

                            for (int i = startBatteryIndex; i < batterySocs.Length; i++)
                            {
                                string sectionNumber = (i + 1).ToString() + "#";

                                // 查找是否已存在该编号的记录
                                var existingData = batterySocDataList.FirstOrDefault(data => data.SectionNumber == sectionNumber);
                                if (existingData != null)
                                {
                                    // 更新已有记录的SOC值
                                    existingData.SOC = batterySocs[i];
                                }
                                else
                                {
                                    // 新增记录
                                    batterySocDataList.Add(new batterySocData
                                    {
                                        SectionNumber = (i + 1).ToString() + "#",
                                        SOC = batterySocs[i]
                                    });
                                }
                            }
                        }

                        break;

                    // 0x0A0：BMS单电芯的SOH
                    case 0x10A1FFFF:
                    case 0x10A1E0FF:
                        // Byte 1:单电芯数据包组号(0~((PACK电芯个数/7) - 1))  224/7-1=31
                        frameNumber = data[0];

                        //调试数据 实际7包共48个电池SOH数据,最后一帧是48 % 7 = 6
                        if (frameNumber >= 0x00 && frameNumber <= 0x06)
                        {
                            int startBatteryIndex = GetbatterySOHStartIndex(frameNumber);
                            ProcessBatterySOH(startBatteryIndex, data);
                        }

                        if (batterySohDataList.Count >= 16)
                        {
                            int startBatteryIndex = (frameNumber + 1) * (48 % 7);
                            ProcessBatterySOH(startBatteryIndex, data);

                            for (int i = 1; i <= 16; i++)
                            {
                                TextBox c = this.Controls.Find("txtSOH" + i, true)[0] as TextBox;
                                c.Text = batterySohDataList[i - 1].SOH.ToString();
                            }
                        }

                        int GetbatterySOHStartIndex(int frameNumber)
                        {
                            // 每个包有7个电池SOH数据                     
                            return frameNumber * 7;
                        }

                        void ProcessBatterySOH(int startBatteryIndex, byte[] data)
                        {
                            var batterySohs = new string[48];
                            if (frameNumber == 0x06)
                            {
                                batterySohs = new string[6 + startBatteryIndex];
                                for (int k = 0; k < 6; k++)
                                {
                                    int byteIndex = k + 1;
                                    double SohRaw = data[byteIndex];
                                    // SOH数据 1/bit 范围：0-255
                                    batterySohs[startBatteryIndex + k] = SohRaw.ToString();
                                }
                            }
                            else
                            {
                                batterySohs = new string[7 + startBatteryIndex];
                                for (int k = 0; k < 7; k++)
                                {
                                    int byteIndex = k + 1;
                                    double SohRaw = data[byteIndex];
                                    // SOH数据 1/bit 范围：0-255
                                    batterySohs[startBatteryIndex + k] = SohRaw.ToString();
                                }
                            }

                            for (int i = startBatteryIndex; i < batterySohs.Length; i++)
                            {
                                string sectionNumber = (i + 1).ToString() + "#";

                                // 查找是否已存在该编号的记录
                                var existingData = batterySohDataList.FirstOrDefault(data => data.SectionNumber == sectionNumber);
                                if (existingData != null)
                                {
                                    // 更新已有记录的SOH值
                                    existingData.SOH = batterySohs[i];
                                }
                                else
                                {
                                    // 新增记录
                                    batterySohDataList.Add(new batterySohData
                                    {
                                        SectionNumber = (i + 1).ToString() + "#",
                                        SOH = batterySohs[i]
                                    });
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("帧数据解析错误 " + ex.Message);
            }
        }

        /// <summary>
        /// 条形码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string AnalysisSN(byte[] data)
        {
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
                //return strSN.Substring(0, strSN.Length - 1);
                return strSN;//序列号为21位，无需去除最后一个字符
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 故障数据帧
        /// </summary>
        /// <param name="data"></param>
        /// <param name="faultNum"></param>
        private void AnalysisLog(byte[] data, int faultNum)
        {
            string[] msg = new string[2];

            for (int i = 0; i < data.Length; i++)
            {
                for (short j = 0; j < 8; j++)
                {
                    if (MyCustomConverter.GetBit(data[i], j) == 1)
                    {
                        // 报警状态为激活
                        getLog(out msg, i, j, faultNum);
                        string type = "";
                        switch (msg[1])
                        {
                            case "1":
                                if (richTextBox3.Text.Contains(msg[0]))
                                    continue;

                                richTextBox3.AppendText(msg[0] + "\r");
                                type = "告警";
                                break;
                            case "2":
                                if (richTextBox2.Text.Contains(msg[0]))
                                    continue;

                                richTextBox2.AppendText(msg[0] + "\r");
                                type = "保护";
                                break;
                            case "3":
                                if (richTextBox1.Text.Contains(msg[0]))
                                    continue;

                                richTextBox1.AppendText(msg[0] + "\r");
                                type = "故障";
                                break;
                            case "4":
                                if (richTextBox4.Text.Contains(msg[0]))
                                    continue;

                                richTextBox4.AppendText(msg[0] + "\r");
                                type = "提示";
                                break;
                        }

                        var query = FrmMain.AlarmList.FirstOrDefault(t => t.Id == FrmMain.BMS_ID && t.Content == "BMU:" + msg[0]);
                        if (query == null)
                        {
                            FrmMain.AlarmList.Add(new AlarmInfo()
                            {
                                DataTime = DateTime.Now.ToString("yy-MM-dd HH:mm:ss"),
                                Id = FrmMain.BMS_ID,
                                Type = type,
                                State = 0,
                                Content = $"BMU:{msg[0]}"
                            });
                        }
                    }
                    else
                    {
                        // 解除报警状态
                        getLog(out msg, i, j, faultNum);//state=1 可解除
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
                            case "4":
                                type = "提示";
                                break;
                        }

                        // 查找激活报警记录以确认解除
                        var query = FrmMain.AlarmList.FirstOrDefault(t => t.Id == FrmMain.BMS_ID && t.Type == type && t.Content == "BMU:" + msg[0] && t.State == 0);
                        if (type == "故障" && msg[0].StartsWith("功率端子温度采样"))
                        {

                        }

                        if (query != null)
                        {
                            query.State = 1;
                            FrmMain.AlarmList.Add(new AlarmInfo()
                            {
                                DataTime = DateTime.Now.ToString("yy-MM-dd HH:mm:ss"),
                                State = 1,
                                Id = FrmMain.BMS_ID,
                                Type = type,
                                Content = $"[解除]BMU:{msg[0]}"
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
        private string[] getLog(out string[] msg, int row, int column, int faultNum)
        {
            msg = new string[2];
            List<FaultInfo> FaultInfos = new List<FaultInfo>();
            switch (faultNum)
            {
                case 1:
                    FaultInfos = FaultInfo.FaultInfos;
                    break;
                case 2:
                    FaultInfos = FaultInfo.FaultInfos2;
                    break;
                case 3:
                    FaultInfos = FaultInfo.FaultInfos3;
                    break;
            }
            /*FaultInfo faultInfo = FaultInfos.FirstOrDefault(f => f.Byte == row && f.Bit == column && f.State == state);
            if (faultInfo != null)
            {
                msg[0] = $"{faultInfo.Content.Split(',')[0]}({faultInfo.Content.Split(',')[1]})";//中文(英文或者代码)
                                                                                                 //msg[0] = faultInfo.Content.Trim();  
                msg[1] = faultInfo.Type.ToString();
                faultInfo.State = state == 1 ? 0 : 1; // 更新状态
            }*/
            FaultInfo faultInfo = FaultInfos.FirstOrDefault(f => f.Byte == row && f.Bit == column);
            if (faultInfo != null)
            {
                msg[0] = $"{faultInfo.Content.Split(',')[0]}({faultInfo.Content.Split(',')[1]})";//中文(英文或者代码) //msg[0] = faultInfo.Content.Trim();  
                msg[1] = faultInfo.Type.ToString();
            }

            return msg;
        }

        #endregion

        #region 数据转换方法《Method》
        private bool ConvertIntToByteArray(Int32 m, ref byte[] arry)
        {
            if (arry == null) return false;
            if (arry.Length < 4) return false;

            arry[0] = (byte)(m & 0xFF);
            arry[1] = (byte)((m & 0xFF00) >> 8);
            arry[2] = (byte)((m & 0xFF0000) >> 16);
            arry[3] = (byte)((m >> 24) & 0xFF);

            return true;
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
                value = (long.Parse(value) / 10.0f).ToString("F1");
            }
            else if (unit == 0.01)
            {
                value = (long.Parse(value) / 100.0f).ToString("F2");
            }
            else if (unit == 0.001)
            {
                value = (long.Parse(value) / 1000.0f).ToString("F3");
            }
            else if (unit == 0.0001)
            {
                value = (long.Parse(value) / 10000).ToString("F4");
            }

            return value;
        }

        /*******************************************************************************
        * Function Name  : Crc8_8210_nBytesCalculate
        * Description    : CRC校验,多项式为0x2F
        *******************************************************************************/
        public static uint Crc8_8210_nBytesCalculate(byte[] pBuff, uint bLen, uint bCrsMask)
        {
            uint i;
            for (int k = 0; k < bLen; k++)
            {
                for (i = 0x80; i > 0; i >>= 1)
                {
                    if (0 != (bCrsMask & 0x80))
                    {
                        bCrsMask <<= 1;
                        bCrsMask ^= 0x2F;
                    }
                    else
                    {
                        bCrsMask <<= 1;
                    }
                    if (0 != (pBuff[k] & i))
                    {
                        bCrsMask ^= 0x2F;
                    }
                }
            }
            return bCrsMask;
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
