﻿using Sofar.ConnectionLibs.CAN;
using SofarBMS.Helper;
using System.Diagnostics;
using System.Text;

namespace SofarBMS.UI
{
    public partial class UpgradeControl : UserControl
    {
        public UpgradeControl()
        {
            InitializeComponent();
        }

        #region 字段
        // 取消令牌源
        public static CancellationTokenSource cts = null;

        // ECAN助手实例
        private EcanHelper ecanHelper = EcanHelper.Instance;

        private const int TX_INTERVAL_TIME = 100;
        private const int TX_INTERVAL_TIME_Data = 3;
        private byte[] fileData;
        private int file_size;
        private int groupIndex;//定义组坐标
        private int chipRole; //芯片角色
        private int flag;//当前标识：步骤1 FB /步骤2 FC + FD /步骤4 FE /步骤5 FF
        private Crc16 _crc = new Crc16(Crc16Model.CcittKermit);

        private readonly object _devListLock = new object();
        private HashSet<uint> DevList = new HashSet<uint>();
        private List<int> ErrorList = new List<int>();
        private List<byte[]> ResultList = new List<byte[]>();
        private Dictionary<uint, int> DevState = new Dictionary<uint, int>();
        private static CancellationTokenSource _cts = null;

        #endregion

        #region 属性
        private bool state = false;//固件升级当前状态
        public bool State
        {
            get { return state; }
            set
            {
                state = value;
                this.Invoke(new Action(() =>
                {
                    if (state)
                    {
                        btnUpgrade_04.Text = LanguageHelper.GetLanguage("Stop_Upgrade");
                    }
                    else
                    {
                        btnUpgrade_04.Text = LanguageHelper.GetLanguage("Start_Upgrade");
                    }
                }));
            }
        }
        #endregion

        #region 事件

        private void UpgradeControl_Load(object sender, EventArgs e)
        {
            this.Invoke((Action)(() =>
            {
                foreach (Control item in this.Controls)
                {
                    GetControls(item);
                }
                cbbChip_role.SelectedIndex = 3;
                lblUpgradeRole.Text = LanguageHelper.GetLanguage("Upgrade_Role");
            }));

            cts = new CancellationTokenSource();
            ecanHelper.AnalysisDataInvoked += ServiceBase_AnalysisDataInvoked;
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

        private void AnalysisData(uint obj_ID, byte[] data)
        {
            uint id = obj_ID | 0xff;
            if (!(id == 0x07FB41FF || id == 0x07fE41FF || id == 0x07FF41FF || id == 0x07FF5FFF))
                return;

            switch (flag)
            {
                case 1:
                    if (data[0] == 0x01 && data[1] == 0x01 && id == 0x07FB41FF)
                    {
                        DevList.Add(obj_ID);
                        AddLog(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), obj_ID.ToString("X8"), BitConverter.ToString(data));
                        //LogHelper.AddLog($"[received]-{DateTime.Now.ToString("HH:mm:ss.fff"),-15}\t0x{obj_ID.ToString("X")}    {BitConverter.ToString(data)}\r\n");
                    }
                    break;
                case 4:
                    if (data[0] == 0x01 && id == 0x07fe41ff)
                    {
                        ResultList.Add(data);
                        AddLog(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), "FE", BitConverter.ToString(data));
                        //LogHelper.AddLog($"[received]-{DateTime.Now.ToString("HH:mm:ss.fff"),-15}\t0x{obj_ID.ToString("X")}    {BitConverter.ToString(data)}\r\n");
                    }
                    break;
                case 5:
                    LogHelper.AddLog($"[received]-{DateTime.Now.ToString("HH:mm:ss.fff"),-15}\t0x{obj_ID.ToString("X")}    {BitConverter.ToString(data)}\r\n");
                    if (id == 0x07ff41ff || id == 0x07ff5fff)
                    {
                        this.Invoke(new Action(() =>
                        {
                            if (data[0] == 0x01 && data[1] == 0x00)
                            {
                                if (!DevState.ContainsKey(obj_ID) && (obj_ID & 0xff) != 0x0f)
                                {
                                    DevState.Add(obj_ID, 1);
                                    AddLog(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), obj_ID.ToString("X8"), LanguageHelper.GetLanguage("Upgrade_Success"));
                                }
                            }
                            else
                            {
                                //提示升级失败
                                AddLog(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), obj_ID.ToString("X8"), LanguageHelper.GetLanguage("Upgrade_Error"));
                                lblUpgrade_05.ForeColor = System.Drawing.Color.Red;
                            }

                            if (DevList.Count <= DevState.Count)
                            {
                                int totalSussces = 0;
                                foreach (var item in DevState)
                                {
                                    if (item.Value == 1)
                                        totalSussces++;
                                }
                                lblUpgrade_05.Text = $"{LanguageHelper.GetLanguage("Upgrade_Result")}" + totalSussces;
                                lblUpgrade_05.ForeColor = System.Drawing.Color.Green;
                                progressBar1.Value = 0;

                                Init();
                            }
                        }));
                    }
                    break;
            }
        }

        private void btnUpgrade_03_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "二进制文件|*.bin";
            openFile.Title = "请选择升级文件";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                txtUpgradeFile.Text = openFile.FileName;

                string bin = openFile.SafeFileName;
                if (bin.Contains("BMS"))
                {
                    cbbChip_role.SelectedIndex = 3;
                }
                else if (bin.Contains("PCU"))
                {
                    cbbChip_role.SelectedIndex = 1;
                }
                else
                {
                    cbbChip_role.SelectedIndex = 4;
                }

                chipRole = cbbChip_role.SelectedIndex;
            }
        }

        private void btnUpgrade_04_Click(object sender, EventArgs e)
        {
            string path = txtUpgradeFile.Text.Trim();

            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    // 正确计算总数据块数
                    file_size = (Convert.ToInt32(fs.Length / 1024) + Convert.ToInt32((file_size % 1024) != 0 ? 1 : 0) - 1);
                    int x = (int)Math.Ceiling((double)fs.Length / 1024);

                    // 读取文件数据
                    BinaryReader r = new BinaryReader(fs);
                    fileData = r.ReadBytes((int)fs.Length);
                }

                // 初始化进度条
                progressBar1.Maximum = file_size - 1 <= 0 ? 0 : file_size - 1;//file_size;

                // 判断升级状态
                if (State == false)
                {
                    flag = 1;
                    State = true;

                    _cts = new CancellationTokenSource();
                    Task.Run(() =>
                    {
                        int counter = 10;
                        while (!_cts.IsCancellationRequested)
                        {
                            if (flag == 1)
                            {
                                while (counter > 0)
                                {
                                    //Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss:ffff") + " FB：" + counter);

                                    this.Invoke(new Action(() =>
                                    {
                                        startDownloadFlag1(Convert.ToByte(cbbChip_role.SelectedIndex), txtChip_code.Text, file_size, 1024);
                                    }));
                                    Thread.Sleep(1000);
                                    counter--;
                                }

                                if (DevList.Count == 0)
                                {
                                    this.Invoke(new Action(() =>
                                    {
                                        lblUpgrade_05.Text = LanguageHelper.GetLanguage("Response_Timed");
                                        lblUpgrade_05.ForeColor = System.Drawing.Color.Red;
                                    }));

                                    Init();
                                }
                                else
                                {
                                    this.Invoke(new Action(() =>
                                    {
                                        lblUpgrade_05.Text = LanguageHelper.GetLanguage("Upgrade_Start") + DevList.Count;
                                        lblUpgrade_05.ForeColor = Color.Black;
                                    }));

                                    flag = 2;
                                    counter = 0;
                                }
                            }
                            else if (flag == 2)
                            {
                                Thread.Sleep(TX_INTERVAL_TIME);

                                AddLog(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), "FC", "PACK_ID" + groupIndex);
                                startDownloadPack2(groupIndex, 1024);
                                Thread.Sleep(TX_INTERVAL_TIME);

                                int offset = groupIndex * 1024;

                                for (int i = 0; i < 1024; i += 8)
                                {
                                    startDownloadData3(offset + i);
                                    Thread.Sleep(TX_INTERVAL_TIME_Data);
                                }

                                this.BeginInvoke(new Action(() =>
                                {
                                    //progressBar1.Maximum = file_size - 1 <= 0 ? 0 : file_size - 1;
                                    progressBar1.Value = groupIndex;
                                }));


                                //当前包数据未发送完成
                                if (groupIndex != file_size - 1)
                                {
                                    groupIndex++;
                                }
                                else
                                {
                                    flag = 4;
                                }
                            }
                            else if (flag == 4)
                            {
                                //do
                                //{
                                ErrorList.Clear();
                                ResultList.Clear();

                                byte[] dataCRC = new byte[fileData.Length - 1024];

                                Buffer.BlockCopy(fileData, 0, dataCRC, 0, dataCRC.Length);

                                this.Invoke(new Action(() =>
                                {
                                    startDownloadCheck4(Convert.ToByte(cbbChip_role.SelectedIndex), txtChip_code.Text, dataCRC);
                                }));
                                Thread.Sleep(TX_INTERVAL_TIME + 2000);

                                //if (ResultList.Count == 0)
                                //    continue;
                                //if (ResultList.Count != DevList.Count)
                                //    continue;

                                int[][] arr = new int[ResultList.Count][];//不规则二位数组，行、不确定列

                                for (int i = 0; i < ResultList.Count; i++)
                                {
                                    byte[] rec = ResultList[i]; //模拟接收到的数据

                                    int[] error = Check(rec).ToArray();

                                    arr[i] = error;
                                }

                                //二维数组的集合
                                for (int ii = 0; ii < arr.Length; ++ii)
                                {
                                    foreach (int j in arr[ii])
                                    {
                                        if (!ErrorList.Contains(j))
                                        {
                                            ErrorList.Add(j);
                                        }
                                    }
                                }

                                for (int i = 0; i < ErrorList.Count; i++)
                                {
                                    //根据坐标地址，计算出第几包；在该处执行FC+FD的数据下发
                                    groupIndex = ErrorList[i] % 24 == 0 ? ErrorList[i] / 24 - 1 : ErrorList[i] / 24;
                                    //Debug.WriteLine($"第{groupIndex}组异常，Pack为{ErrorList[i]} ");
                                    //AddLog(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), "FE", $"第{groupIndex}组异常，Pack为{ErrorList[i]}");

                                    startDownloadPack2(ErrorList[i] - 1, 1024);
                                    Thread.Sleep(TX_INTERVAL_TIME);

                                    int offset = (ErrorList[i] - 1) * 1024;

                                    for (int j = 0; j < 1024; j += 8)
                                    {
                                        startDownloadData3(offset + j);
                                        Thread.Sleep(TX_INTERVAL_TIME_Data);
                                    }
                                    Thread.Sleep(TX_INTERVAL_TIME);
                                }
                                //} while (ErrorList.Count != 0);

                                if (ErrorList.Count == 0)//ResultList.Count >= DevList.Count && 
                                    flag = 5;
                            }
                            else if (flag == 5)
                            {
                                Thread.Sleep(2000);

                                if (counter <= 5)
                                {
                                    Debug.WriteLine(System.DateTime.Now.ToString("HH:mm:ss:ffff") + " FF：" + counter);

                                    this.Invoke(new Action(() =>
                                    {
                                        startDownloadState5(Convert.ToByte(cbbChip_role.SelectedIndex), txtChip_code.Text);
                                    }));
                                }
                                else
                                {
                                    StringBuilder sb = new StringBuilder();

                                    if (DevState.Count != 0)
                                    {
                                        sb.Append("，检查到完成升级设备已有：");
                                        foreach (uint item in DevState.Keys)
                                        {
                                            sb.Append((item & 0x1f) + ",");
                                        }
                                        sb.ToString().Substring(0, sb.Length - 1);
                                    }

                                    this.Invoke(new Action(() =>
                                    {
                                        lblUpgrade_05.Text = LanguageHelper.GetLanguage("Response_Timed") + sb.ToString();
                                        lblUpgrade_05.ForeColor = System.Drawing.Color.Red;
                                    }));

                                    Init();
                                }

                                counter = counter + 1;
                            }
                        }
                    }, _cts.Token);
                }
                else
                {
                    Init();
                }
            }
            else
            {
                MessageBox.Show(LanguageHelper.GetLanguage("Upgrade_FileSelect"));
            }
        }

        public void Init()
        {
            _cts.Cancel();
            _cts.Dispose();

            flag = 0;
            State = false;

            retry = 0;
            file_size = 0;
            groupIndex = 0;
            DevList.Clear();
            DevState.Clear();
        }

        private void cbbChip_role_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbChip_role.SelectedIndex == 1)
            {
                txtChip_code.Text = "T1";
            }
            else if (cbbChip_role.SelectedIndex == 3)
            {
                txtChip_code.Text = "X0";
            }
            else
            {
                txtChip_code.Text = "00";
            }
        }
        #endregion

        #region 成员方法
        /// <summary>
        /// 升级文件传输开始帧
        /// </summary>
        /// <param name="chip_role">芯片角色 1B 0:ARM 1:DSP_M 2:DSP_S 3:BMS</param>
        /// <param name="chip_code">芯片型号编码 2B 芯片编码</param>
        /// <param name="file_size">文件数据块总数 2B 文件数据块总数=文件大小/数据块的大小+1</param>
        /// <param name="data_size">数据块的大小 2B 默认1025Byte</param>
        /// <exception cref="Exception"></exception>
        public void startDownloadFlag1(byte chip_role, string chip_code, int file_size, int data_size)
        {
            int i = 0;
            byte[] data = new byte[8];
            try
            {
                data[i++] = 0x00;//发送请求帧
                data[i++] = chip_role;
                data[i++] = ASCIIEncoding.Default.GetBytes(chip_code)[1];
                data[i++] = ASCIIEncoding.Default.GetBytes(chip_code)[0];
                data[i++] = Convert.ToByte(file_size & 0xff);
                data[i++] = Convert.ToByte(file_size >> 8);
                data[i++] = Convert.ToByte(data_size & 0xff);
                data[i++] = Convert.ToByte(data_size >> 8);

                AssembleInstruction(0xFB, data);
            }
            catch (Exception ex)
            {
                throw new Exception("升级文件传输开始帧0x7FB，ERROR：" + ex.Message);
            }
        }

        /// <summary>
        /// 升级数据块开始帧
        /// </summary>
        /// <param name="serial_number">当前的数据块序号 2B 当前传输的数据块的序号(0开始)</param>
        /// <param name="data_size">数据块的大小</param>
        /// <exception cref="Exception"></exception>
        public void startDownloadPack2(int serial_number, int data_size)
        {
            int i = 0;
            byte[] data = new byte[8];
            byte[] bytes = new byte[1024];

            try
            {
                readBinFile(serial_number * 1024, ref bytes);
                byte[] hexCRC = _crc.ComputeChecksumBytes(bytes);//序号总CRC计算
                data[i++] = 0x00;//发送请求帧
                data[i++] = 0x00;//预留
                data[i++] = Convert.ToByte(serial_number & 0xff);
                data[i++] = Convert.ToByte(serial_number >> 8);
                data[i++] = Convert.ToByte(data_size & 0xff);
                data[i++] = Convert.ToByte(data_size >> 8);
                data[i++] = hexCRC[0];
                data[i++] = hexCRC[1];

                AssembleInstruction(0xFC, data);
            }
            catch (Exception ex)
            {
                throw new Exception("升级数据块开始帧0xFC，ERROR：" + ex.Message);
            }
        }

        /// <summary>
        /// 升级数据块数据帧
        /// </summary>
        /// <param name="offset">文件数据</param>
        /// <exception cref="Exception"></exception>
        public void startDownloadData3(int offset)
        {
            byte[] data = new byte[8];
            try
            {
                readBinFile(offset, ref data);

                AssembleInstruction(0xFD, data);
            }
            catch (Exception ex)
            {
                throw new Exception("升级数据块数据帧0xFD，ERROR：" + ex.Message);
            }
        }

        /// <summary>
        /// 升级文件接收结果查询帧
        /// </summary>
        /// <param name="chip_role">芯片角色 1B 0:ARM 1:DSP_M 2:DSP_S 3:BMS</param>
        /// <param name="chip_code">芯片型号编码 2B 芯片编码</param>
        /// <param name="file_buffer"></param>
        /// <exception cref="Exception"></exception>
        public void startDownloadCheck4(byte chip_role, string chip_code, byte[] file_buffer)
        {
            int i = 0;
            byte[] data = new byte[8];
            try
            {
                byte[] hexCRC = _crc.ComputeChecksumBytes(file_buffer);//序号总CRC计算
                data[i++] = 0x00;//发送请求帧
                data[i++] = chip_role;
                data[i++] = ASCIIEncoding.Default.GetBytes(chip_code)[1];
                data[i++] = ASCIIEncoding.Default.GetBytes(chip_code)[0];
                data[i++] = hexCRC[0];
                data[i++] = hexCRC[1];
                data[i++] = 0xAA;

                AssembleInstruction(0xFE, data);
            }
            catch (Exception ex)
            {
                throw new Exception("升级文件接收结果查询帧0xFE，ERROR：" + ex.Message);
            }
        }

        /// <summary>
        /// 升级完成状态查询帧
        /// </summary>
        /// <param name="chip_role">芯片角色 1B 0:ARM 1:DSP_M 2:DSP_S 3:BMS</param>
        /// <param name="chip_code">芯片型号编码 2B 芯片编码</param>
        /// <exception cref="Exception"></exception>
        public void startDownloadState5(byte chip_role, string chip_code)
        {
            int i = 0;
            byte[] data = new byte[8];
            try
            {
                data[i++] = 0x00;//发送请求帧
                data[i++] = chip_role;
                data[i++] = ASCIIEncoding.Default.GetBytes(chip_code)[1];
                data[i++] = ASCIIEncoding.Default.GetBytes(chip_code)[0];

                AssembleInstruction(0xFF, data);
            }
            catch (Exception ex)
            {
                throw new Exception("升级完成状态查询帧0xFF，ERROR：" + ex.Message);
            }
        }

        int retry = 0;
        /// <summary>
        /// 组装指令函数
        /// </summary>
        /// <param name="mark"></param>
        /// <param name="data"></param>
        private void AssembleInstruction(byte mark, byte[] data)
        {
            try
            {
                byte[] canid = new byte[] { 0x41, 0x1F, mark, 0x07 };
                if (chipRole == 1)
                {
                    canid = new byte[] { 0x41, 0x3F, mark, 0x07 };
                }
                else if (chipRole == 4)
                {
                    canid = new byte[] { 0x41, 0xBF, mark, 0x07 };
                }

                if (!_cts.IsCancellationRequested)
                {
                    bool result = ecanHelper.Send(data, canid);
                    if (!result)
                    {
                        if (retry >= 10 && State)
                        {
                            AddLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), BitConverter.ToUInt32(canid, 0).ToString("X"), "Communication error, task terminated!");
                            Init();//发送无效，自动结束升级！
                        }

                        retry++;
                    }
                    else
                    {
                        if (mark != 0xFD)
                            AddLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), BitConverter.ToUInt32(canid, 0).ToString("X"), $"PACK_ID:{groupIndex},Data:{BitConverter.ToString(data)}");
                    }
                }

                //LogHelper.AddLog($"[send]-{DateTime.Now.ToString("HH:mm:ss.fff"),-15}\t0x{BitConverter.ToUInt32(canid, 0).ToString("X")}    {groupIndex} {BitConverter.ToString(data)}\r\n");
            }
            catch (Exception ex)
            {
                Debug.Print(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "下发数据异常：" + ex.Message);
            }
        }

        /// <summary>
        /// 获取Bin文件数据
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool readBinFile(int offset, ref byte[] data)
        {
            try
            {
                var temp = fileData.Skip(offset).Take(data.Length).ToArray();
                data = temp;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 检查BIT位
        /// </summary>
        /// <param name="responsedata"></param>
        /// <returns></returns>
        public List<int> Check(byte[] responsedata)
        {
            List<int> lists = new List<int>();

            byte[] table = { 0x01, 0x02, 0x04, 0x8, 0x10, 0x20, 0x40, 0x80 };

            int cnt = 0;
            for (int i = 5; i < responsedata.Length; i++)
            {
                if (responsedata[i] != 0x00)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if ((table[j] & responsedata[i]) == table[j])
                        {
                            lists.Add(j + cnt + 1 + (responsedata[4] * 24));
                        }
                    }
                }
                cnt += 8;
            }
            return lists;
        }

        private void AddLog(string date, string id, string context)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Text = date;

            lvi.SubItems.Add(id);

            lvi.SubItems.Add(context);

            this.BeginInvoke(new Action(() =>
            {
                this.listView1.Items.Insert(0, lvi);
            }));
        }

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
            int len = control.Width * str.Length / strWidth;
            if (len > 3 && len < str.Length)
                return str.Substring(0, len - 3) + "...";
            else
                return str;
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
