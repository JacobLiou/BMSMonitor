using Sofar.BMS;
using Sofar.BMS.Common;
using Sofar.ConnectionLibs.CAN;
using SofarBMS.Helper;
using System.Xml;

namespace SofarBMS.ui
{
    public partial class CBSParamControl_BMU : UserControl
    {
        EcanHelper ecanHelper = EcanHelper.Instance;
        BMUParamVM paramVM = new BMUParamVM();
        public static CancellationTokenSource cts = null;

        private Dictionary<string, string> AlarmParametersDic => new(){
                {"txt_1", "单体过充保护(mV)"},
                {"txt_2", "单体过充解除(mV)"},
                {"txt_3", "单体过充告警(mV)"},
                {"txt_4", "单体过充解除(mV)"},
                {"txt_5", "总体过充保护(V)"},
                {"txt_6", "总体过充解除(V)"},
                {"txt_7", "总体过充告警(V)"},
                {"txt_8", "总体过充解除(V)"},
                {"txt_9", "单体过放保护(mV)"},
                {"txt_10", "单体过放解除(mV)"},
                {"txt_11", "单体过放告警(mV)"},
                {"txt_12", "单体过放解除(mV)"},
                {"txt_13", "总体过放保护(V)"},
                {"txt_14", "总体过放解除(V)"},
                {"txt_15", "总体过放告警(V)"},
                {"txt_16", "总体过放解除(V)"},
                {"txt_17", "充电过流保护(A)"},
                {"txt_18", "充电过流解除(A)"},
                {"txt_19", "充电过流告警(A)"},
                {"txt_20", "充电过流解除(A)"},
                {"txt_21", "放电过流保护(A)"},
                {"txt_22", "放电过流解除(A)"},
                {"txt_23", "放电过流告警(A)"},
                {"txt_24", "放电过流解除(A)"},
                {"txt_25", "充电高温保护(℃)"},
                {"txt_26", "充电高温解除(℃)"},
                {"txt_27", "充电高温告警(℃)"},
                {"txt_28", "充电高温解除(℃)"},
                {"txt_29", "放电高温保护(℃)"},
                {"txt_30", "放电高温解除(℃)"},
                {"txt_31", "放电高温告警(℃)"},
                {"txt_32", "放电高温解除(℃)"},
                {"txt_33", "充电低温保护(℃)"},
                {"txt_34", "充电低温解除(℃)"},
                {"txt_35", "充电低温告警(℃)"},
                {"txt_36", "充电低温解除(℃)"},
                {"txt_37", "放电低温保护(℃)"},
                {"txt_38", "放电低温解除(℃)"},
                {"txt_39", "放电低温告警(℃)"},
                {"txt_40", "放电低温解除(℃)"},
                {"txt_41", "低电量保护(%)"},
                {"txt_42", "低电量解除(%)"},
                {"txt_43", "低电量告警(%)"},
                {"txt_44", "低电量解除(%)"},
                {"txt_45", "充电过流二级保护(A)"},
                {"txt_46", "充电过流二级保护解除(A)"},
                {"txt_47", "放电过流二级保护(A)"},
                {"txt_48", "放电过流二级保护解除(A)"},
                {"txt_49", "单体超限产生阈值(mV)"},
                {"txt_50", "单体超限解除阈值(mV)"},
                {"txt_51", "单体超低产生阈值(mV)"},
                {"txt_52", "单体超低解除阈值(mV)"},
                {"txt_53", "单体过充提示(mV)"},
                {"txt_54", "单体过充提示解除(mV)"},
                {"txt_55", "单体过放提示(mV)"},
                {"txt_56", "单体过放提示解除(mV)"},
                {"txt_57", "单体压差过大提示(mV)"},
                {"txt_58", "单体压差过大提示解除(mV)"},
                {"txt_59", "单体压差过大告警(mV)"},
                {"txt_60", "单体压差过大告警解除(mV)"},
                {"txt_61", "单体压差过大故障(mV)"},
                {"txt_62", "单体压差过大故障解除(mV)"},
                {"txt_63", "充电高温提示(℃)"},
                {"txt_64", "充电高温提示解除(℃)"},
                {"txt_65", "充电低温提示(℃)"},
                {"txt_66", "充电低温提示解除(℃)"},
                {"txt_67", "放电高温提示(℃)"},
                {"txt_68", "放电高温提示解除(℃)"},
                {"txt_69", "放电低温提示(℃)"},
                {"txt_70", "放电低温提示解除(℃)"},
                {"txt_71", "电芯温差过大提示(℃)"},
                {"txt_72", "电芯温差过大提示解除(℃)"},
                {"txt_73", "电芯温差过大告警(℃)"},
                {"txt_74", "电芯温差过大告警解除(℃)"},
                {"txt_75", "电芯温差过大保护(℃)"},
                {"txt_76", "电芯温差过大保护解除(℃)"},
                {"txt_77", "总体过充提示(V)"},
                {"txt_78", "总体过充提示解除(V)"},
                {"txt_79", "总体过放提示(V)"},
                {"txt_80", "总体过放提示解除(V)"}
            };
        private int index = 1;
        private bool readFlag = true;

        public CBSParamControl_BMU()
        {
            InitializeComponent();
        }

        private void CBSParamControl_BMU_Load(object? sender, EventArgs e)
        {
            this.Invoke(() =>
            {
                foreach (Control item in this.Controls)
                {
                    GetControls(item);
                }
            });

            cts = new CancellationTokenSource();
            Task.Run(async delegate
            {
                while (!cts.IsCancellationRequested)
                {
                    if (ecanHelper.IsConnected)
                    {
                        if (readFlag)
                        {
                            await Task.Delay(1000);
                            byte[] bytes = new byte[8] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                            ecanHelper.Send(bytes, new byte[] { 0xE0, (byte)(FrmMain.BMS_ID + 0x80), 0x2E, 0x10 });

                            readFlag = false;
                        }
                    }
                }
            }, cts.Token);

            ecanHelper.AnalysisDataInvoked += ParamVM_AnalysisDataInvoked;
        }

        private void ParamVM_AnalysisDataInvoked(object? sender, object e)
        {
            if (cts.IsCancellationRequested && ecanHelper.IsConnected)
            {
                ecanHelper.AnalysisDataInvoked -= ParamVM_AnalysisDataInvoked;
                return;
            }

            var frameModel = e as CanFrameModel;
            if (frameModel != null)
            {
                this.Invoke(() => { AnalysisData(frameModel.CanID, frameModel.Data); });
            }
        }

        public void AnalysisData(uint canID, byte[] data)
        {
            byte[] canid = BitConverter.GetBytes(canID);
            if (canid[0] != FrmMain.BMS_ID || !(canid[0] == FrmMain.BMS_ID && canid[1] == 0xE0 && canid[3] == 0x10)) return;

            int[] numbers = MyCustomConverter.BytesToUint16(data);
            int[] numbers_bit = MyCustomConverter.BytesToBit(data);

            switch (canid[2])
            {
                case 0x10://单体过充                      
                    txt_1.Text = numbers[0].ToString();
                    txt_2.Text = numbers[1].ToString();
                    txt_3.Text = numbers[2].ToString();
                    txt_4.Text = numbers[3].ToString();
                    break;
                case 0x11://总体过充                                                                                      
                    txt_5.Text = (numbers[0] * 0.1).ToString("F1");
                    txt_6.Text = (numbers[1] * 0.1).ToString("F1");
                    txt_7.Text = (numbers[2] * 0.1).ToString("F1");
                    txt_8.Text = (numbers[3] * 0.1).ToString("F1");
                    break;
                case 0x12://单体过放                                                                                  
                    txt_9.Text = numbers[0].ToString();
                    txt_10.Text = numbers[1].ToString();
                    txt_11.Text = numbers[2].ToString();
                    txt_12.Text = numbers[3].ToString();
                    break;
                case 0x13://总体过放                                                                                    
                    txt_13.Text = (numbers[0] * 0.1).ToString("0.0");
                    txt_14.Text = (numbers[1] * 0.1).ToString("0.0");
                    txt_15.Text = (numbers[2] * 0.1).ToString("0.0");
                    txt_16.Text = (numbers[3] * 0.1).ToString("0.0");
                    break;
                //case 0x14://充电过流                                                                                
                //    txt_17.Text = (numbers[0] * 0.01).ToString("0.00");
                //    txt_18.Text = (numbers[1] * 0.01).ToString("0.00");
                //    txt_19.Text = (numbers[2] * 0.01).ToString("0.00");
                //    txt_20.Text = (numbers[3] * 0.01).ToString("0.00");
                //    break;
                //case 0x15://放电过流                                                                             
                //    txt_21.Text = (numbers[0] * 0.01).ToString("0.00");
                //    txt_22.Text = (numbers[1] * 0.01).ToString("0.00");
                //    txt_23.Text = (numbers[2] * 0.01).ToString("0.00");
                //    txt_24.Text = (numbers[3] * 0.01).ToString("0.00");
                //    break;
                case 0x16://充电、放电高温                                                                        
                    txt_25.Text = (numbers_bit[0] - 40).ToString();
                    txt_26.Text = (numbers_bit[1] - 40).ToString();
                    txt_27.Text = (numbers_bit[2] - 40).ToString();
                    txt_28.Text = (numbers_bit[3] - 40).ToString();
                    txt_29.Text = "0";//(numbers_bit[4] - 40).ToString();
                    txt_30.Text = "0";//(numbers_bit[5] - 40).ToString();
                    txt_31.Text = (numbers_bit[6] - 40).ToString();
                    txt_32.Text = (numbers_bit[7] - 40).ToString();
                    break;
                case 0x17://充电、放电低温                                                                        
                    txt_33.Text = "0";//(numbers_bit[0] - 40).ToString();
                    txt_34.Text = "0";//(numbers_bit[1] - 40).ToString();
                    txt_35.Text = (numbers_bit[2] - 40).ToString();
                    txt_36.Text = (numbers_bit[3] - 40).ToString();
                    txt_37.Text = "0";//(numbers_bit[4] - 40).ToString();
                    txt_38.Text = "0";//(numbers_bit[5] - 40).ToString();
                    txt_39.Text = (numbers_bit[6] - 40).ToString();
                    txt_40.Text = (numbers_bit[7] - 40).ToString();
                    break;
                case 0x19://低电量
                    txt_41.Text = numbers[0].ToString();
                    txt_42.Text = numbers[1].ToString();
                    txt_43.Text = numbers[2].ToString();
                    txt_44.Text = numbers[3].ToString();
                    break;
                //case 0x1B://充放电过流                                                                             
                //    txt_45.Text = (numbers[0] * 0.01).ToString("0.00");
                //    txt_46.Text = (numbers[1] * 0.01).ToString("0.00");
                //    txt_47.Text = (numbers[2] * 0.01).ToString("0.00");
                //    txt_48.Text = (numbers[3] * 0.01).ToString("0.00");
                //    break;
                case 0x1C://单体超限、超低
                    txt_49.Text = numbers[0].ToString();
                    txt_50.Text = numbers[1].ToString();
                    txt_51.Text = numbers[2].ToString();
                    txt_52.Text = numbers[3].ToString();
                    break;
                /*//单体过充、过放提示
                case 0x80:
                    txt_53.Text = numbers[0].ToString();
                    txt_54.Text = numbers[1].ToString();
                    txt_55.Text = numbers[2].ToString();
                    txt_56.Text = numbers[3].ToString();
                    break;
                //单体压差过大提示、告警
                case 0x81:
                    txt_57.Text = numbers[0].ToString();
                    txt_58.Text = numbers[1].ToString();
                    txt_59.Text = numbers[2].ToString();
                    txt_60.Text = numbers[3].ToString();
                    break;
                //单体压差过大故障
                case 0x82:
                    txt_61.Text = numbers[0].ToString();
                    txt_62.Text = numbers[1].ToString();
                    break;*/
                //充放电高低温提示
                case 0x83:
                    txt_63.Text = (numbers_bit[0] - 40).ToString();
                    txt_64.Text = (numbers_bit[1] - 40).ToString();
                    txt_65.Text = (numbers_bit[2] - 40).ToString();
                    txt_66.Text = (numbers_bit[3] - 40).ToString();
                    txt_67.Text = (numbers_bit[4] - 40).ToString();
                    txt_68.Text = (numbers_bit[5] - 40).ToString();
                    txt_69.Text = (numbers_bit[6] - 40).ToString();
                    txt_70.Text = (numbers_bit[7] - 40).ToString();
                    break;
                    /* //电芯温差过大
                     case 0x84:
                         txt_71.Text = numbers_bit[0].ToString();
                         txt_72.Text = numbers_bit[1].ToString();
                         txt_73.Text = numbers_bit[2].ToString();
                         txt_74.Text = numbers_bit[3].ToString();
                         txt_75.Text = numbers_bit[4].ToString();
                         txt_76.Text = numbers_bit[5].ToString();
                         break;
                     //总体过充、过放提示
                     case 0x85:
                         txt_77.Text = (numbers[0] * 0.1).ToString();
                         txt_78.Text = (numbers[1] * 0.1).ToString();
                         txt_79.Text = (numbers[2] * 0.1).ToString();
                         txt_80.Text = (numbers[3] * 0.1).ToString();
                         break;*/
            }
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnParameter_70_Click(object sender, EventArgs e)
        {
            paramVM.SelectedAddress_BMU = (byte)(FrmMain.BMS_ID + 0x00);
            if (!paramVM.Read())
            {
                //读取失败提示
                MessageBox.Show("读取失败");
            }
        }

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnParameter_71_Click(object sender, EventArgs e)
        {
            List<bool> checkList = new();
            Dictionary<int, string[]> paramList = new();

            for (int i = 1; i <= 17; i++)
            {
                Control c = this.Controls.Find("ckb_" + i, true)[0];
                if (((CheckBox)c).Checked)
                {
                    checkList.Add(true);
                    switch (c.Name)
                    {
                        case "ckb_1":
                            paramList.Add(0x10, new string[] { txt_1.Text.Trim(), txt_2.Text.Trim(), txt_3.Text.Trim(), txt_4.Text.Trim() });
                            break;
                        case "ckb_2":
                            paramList.Add(0x11, new string[] { txt_5.Text.Trim(), txt_6.Text.Trim(), txt_7.Text.Trim(), txt_8.Text.Trim() });
                            break;
                        case "ckb_3":
                            paramList.Add(0x12, new string[] { txt_9.Text.Trim(), txt_10.Text.Trim(), txt_11.Text.Trim(), txt_12.Text.Trim() });
                            break;
                        case "ckb_4":
                            paramList.Add(0x13, new string[] { txt_13.Text.Trim(), txt_14.Text.Trim(), txt_15.Text.Trim(), txt_16.Text.Trim() });
                            break;
                        //case "ckb_5":
                        //    paramList.Add(0x14, new string[] { txt_17.Text.Trim(), txt_18.Text.Trim(), txt_19.Text.Trim(), txt_20.Text.Trim() });
                        //    break;
                        //case "ckb_6":
                        //    paramList.Add(0x15, new string[] { txt_21.Text.Trim(), txt_22.Text.Trim(), txt_23.Text.Trim(), txt_24.Text.Trim() });
                        //    break;
                        case "ckb_7":
                            paramList.Add(0x16, new string[] { txt_25.Text.Trim(), txt_26.Text.Trim(), txt_27.Text.Trim(), txt_28.Text.Trim(), txt_29.Text.Trim(), txt_30.Text.Trim(), txt_31.Text.Trim(), txt_32.Text.Trim() });
                            break;
                        case "ckb_8":
                            paramList.Add(0x17, new string[] { txt_33.Text.Trim(), txt_34.Text.Trim(), txt_35.Text.Trim(), txt_36.Text.Trim(), txt_37.Text.Trim(), txt_38.Text.Trim(), txt_39.Text.Trim(), txt_40.Text.Trim() });
                            break;
                        case "ckb_9"://ckb_11
                            paramList.Add(0x19, new string[] { txt_41.Text.Trim(), txt_42.Text.Trim(), txt_43.Text.Trim(), txt_44.Text.Trim() });
                            break;
                        //case "ckb_10"://ckb_9
                        //    paramList.Add(0x1B, new string[] { txt_45.Text.Trim(), txt_46.Text.Trim(), txt_47.Text.Trim(), txt_48.Text.Trim() });
                        //    break;
                        case "ckb_11"://ckb_10
                            paramList.Add(0x1C, new string[] { txt_49.Text.Trim(), txt_50.Text.Trim(), txt_51.Text.Trim(), txt_52.Text.Trim() });
                            break;
                        //case "ckb_12":
                        //    paramList.Add(0x80, new string[] { txt_53.Text.Trim(), txt_54.Text.Trim(), txt_55.Text.Trim(), txt_56.Text.Trim() });
                        //    break;
                        //case "ckb_13":
                        //    paramList.Add(0x81, new string[] { txt_57.Text.Trim(), txt_58.Text.Trim(), txt_59.Text.Trim(), txt_60.Text.Trim() });
                        //    break;
                        //case "ckb_14":
                        //    paramList.Add(0x82, new string[] { txt_61.Text.Trim(), txt_62.Text.Trim(), txt_63.Text.Trim(), txt_64.Text.Trim() });
                        //    break;
                        case "ckb_15":
                            paramList.Add(0x83, new string[] { txt_63.Text.Trim(), txt_64.Text.Trim(), txt_65.Text.Trim(), txt_66.Text.Trim(), txt_67.Text.Trim(), txt_68.Text.Trim(), txt_69.Text.Trim(), txt_70.Text.Trim() });
                            break;
                            //case "ckb_16":
                            //    paramList.Add(0x84, new string[] { txt_73.Text.Trim(), txt_74.Text.Trim(), txt_75.Text.Trim(), txt_76.Text.Trim() });
                            //    break;
                            //case "ckb_17":
                            //    paramList.Add(0x85, new string[] { txt_77.Text.Trim(), txt_78.Text.Trim(), txt_79.Text.Trim(), txt_80.Text.Trim() });
                            //    break;

                    }
                }
                else
                {
                    checkList.Add(false);
                }
            }

            if (checkList.Any(t => t))
            {
                //paramVM.SelectedAddress_BMU = (byte)(FrmMain.BMS_ID + 0x00);
                if (paramVM.Write(checkList.ToArray(), paramList))
                {
                    MessageBox.Show(FrmMain.GetString("keyWriteSuccess"));
                }
                else
                {
                    MessageBox.Show(FrmMain.GetString("keyWriteFail"));
                }
            }
        }

        /// <summary>
        /// 恢复默认值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnParameter_72_Click(object sender, EventArgs e)
        {
            paramVM.Init();
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnParameter_73_Click(object sender, EventArgs e)
        {
            try
            {
                var fileDialog = new OpenFileDialog
                {
                    Multiselect = false,
                    Title = "请选择文件",
                    Filter = "XML文件|*.xml"
                };

                if (!fileDialog.ShowDialog().Equals(DialogResult.OK))
                    return;

                string filePath = fileDialog.FileName;
                XmlDocument document = new XmlDocument();
                document.Load(filePath);

                XmlNodeList nodeList = document.DocumentElement.SelectNodes("//param");
                if (nodeList == null || nodeList.Count == 0)
                {
                    MessageBox.Show("未找到参数节点。", "导入错误");
                    return;
                }

                AlarmParameter parameter = new();

                // 创建字典以映射参数名称到相应的属性设置
                var propertyMap = new Dictionary<string, Action<string>>()
                {
                    { "txt_1",   value => parameter.AlarmParameter_1_1   = value },
                    { "txt_2",   value => parameter.AlarmParameter_1_2   = value },
                    { "txt_3",   value => parameter.AlarmParameter_1_3   = value },
                    { "txt_4",   value => parameter.AlarmParameter_1_4   = value },
                    { "txt_5",   value => parameter.AlarmParameter_2_1   = value },
                    { "txt_6",   value => parameter.AlarmParameter_2_2   = value },
                    { "txt_7",   value => parameter.AlarmParameter_2_3   = value },
                    { "txt_8",   value => parameter.AlarmParameter_2_4   = value },
                    { "txt_9",   value => parameter.AlarmParameter_3_1   = value },
                    { "txt_10",   value => parameter.AlarmParameter_3_2   = value },
                    { "txt_11",   value => parameter.AlarmParameter_3_3   = value },
                    { "txt_12",   value => parameter.AlarmParameter_3_4   = value },
                    { "txt_13",   value => parameter.AlarmParameter_4_1   = value },
                    { "txt_14",   value => parameter.AlarmParameter_4_2   = value },
                    { "txt_15",   value => parameter.AlarmParameter_4_3   = value },
                    { "txt_16",   value => parameter.AlarmParameter_4_4   = value },
                    { "txt_17",   value => parameter.AlarmParameter_5_1   = value },
                    { "txt_18",   value => parameter.AlarmParameter_5_2   = value },
                    { "txt_19",   value => parameter.AlarmParameter_5_3   = value },
                    { "txt_20",   value => parameter.AlarmParameter_5_4   = value },
                    { "txt_21",   value => parameter.AlarmParameter_6_1   = value },
                    { "txt_22",   value => parameter.AlarmParameter_6_2   = value },
                    { "txt_23",   value => parameter.AlarmParameter_6_3   = value },
                    { "txt_24",   value => parameter.AlarmParameter_6_4   = value },
                    { "txt_25",   value => parameter.AlarmParameter_7_1   = value },
                    { "txt_26",   value => parameter.AlarmParameter_7_2   = value },
                    { "txt_27",   value => parameter.AlarmParameter_7_3   = value },
                    { "txt_28",   value => parameter.AlarmParameter_7_4   = value },
                    { "txt_29",   value => parameter.AlarmParameter_7_5   = value },
                    { "txt_30",   value => parameter.AlarmParameter_7_6   = value },
                    { "txt_31",   value => parameter.AlarmParameter_7_7   = value },
                    { "txt_32",   value => parameter.AlarmParameter_7_8   = value },
                    { "txt_33",   value => parameter.AlarmParameter_8_1   = value },
                    { "txt_34",   value => parameter.AlarmParameter_8_2   = value },
                    { "txt_35",   value => parameter.AlarmParameter_8_3   = value },
                    { "txt_36",   value => parameter.AlarmParameter_8_4   = value },
                    { "txt_37",   value => parameter.AlarmParameter_8_5   = value },
                    { "txt_38",   value => parameter.AlarmParameter_8_6   = value },
                    { "txt_39",   value => parameter.AlarmParameter_8_7   = value },
                    { "txt_40",   value => parameter.AlarmParameter_8_8   = value },
                    { "txt_41",   value => parameter.AlarmParameter_9_1   = value },
                    { "txt_42",   value => parameter.AlarmParameter_9_2   = value },
                    { "txt_43",   value => parameter.AlarmParameter_9_3   = value },
                    { "txt_44",   value => parameter.AlarmParameter_9_4   = value },
                    { "txt_45",  value => parameter.AlarmParameter_10_1  = value },
                    { "txt_46",  value => parameter.AlarmParameter_10_2  = value },
                    { "txt_47",  value => parameter.AlarmParameter_10_3  = value },
                    { "txt_48",  value => parameter.AlarmParameter_10_4  = value },
                    { "txt_49",  value => parameter.AlarmParameter_11_1  = value },
                    { "txt_50",  value => parameter.AlarmParameter_11_2  = value },
                    { "txt_51",  value => parameter.AlarmParameter_11_3  = value },
                    { "txt_52",  value => parameter.AlarmParameter_11_4  = value },
                    { "txt_53",  value => parameter.AlarmParameter_12_1  = value },
                    { "txt_54",  value => parameter.AlarmParameter_12_2  = value },
                    { "txt_55",  value => parameter.AlarmParameter_12_3  = value },
                    { "txt_56",  value => parameter.AlarmParameter_12_4  = value },
                    { "txt_57",  value => parameter.AlarmParameter_13_1  = value },
                    { "txt_58",  value => parameter.AlarmParameter_13_2  = value },
                    { "txt_59",  value => parameter.AlarmParameter_13_3  = value },
                    { "txt_60",  value => parameter.AlarmParameter_13_4  = value },
                    { "txt_61",  value => parameter.AlarmParameter_14_1  = value },
                    { "txt_62",  value => parameter.AlarmParameter_14_2  = value },
                    { "txt_63",  value => parameter.AlarmParameter_15_1  = value },
                    { "txt_64",  value => parameter.AlarmParameter_15_2  = value },
                    { "txt_65",  value => parameter.AlarmParameter_15_3  = value },
                    { "txt_66",  value => parameter.AlarmParameter_15_4  = value },
                    { "txt_67",  value => parameter.AlarmParameter_15_5  = value },
                    { "txt_68",  value => parameter.AlarmParameter_15_6  = value },
                    { "txt_69",  value => parameter.AlarmParameter_15_7  = value },
                    { "txt_70",  value => parameter.AlarmParameter_15_8  = value },
                    { "txt_71",  value => parameter.AlarmParameter_16_1  = value },
                    { "txt_72",  value => parameter.AlarmParameter_16_2  = value },
                    { "txt_73",  value => parameter.AlarmParameter_16_3  = value },
                    { "txt_74",  value => parameter.AlarmParameter_16_4  = value },
                    { "txt_75",  value => parameter.AlarmParameter_16_5  = value },
                    { "txt_76",  value => parameter.AlarmParameter_16_6  = value },
                    { "txt_77",  value => parameter.AlarmParameter_17_1  = value },
                    { "txt_78",  value => parameter.AlarmParameter_17_2  = value },
                    { "txt_79",  value => parameter.AlarmParameter_17_3  = value },
                    { "txt_80",  value => parameter.AlarmParameter_17_4  = value },
                };

                // 遍历每个参数节点并更新对应的值
                foreach (XmlNode xmlNode in nodeList)
                {
                    XmlElement element = (XmlElement)xmlNode;
                    string controlName = element.GetAttribute("id");
                    string paramValue = element.InnerText;

                    // 更新参数
                    //if (propertyMap.TryGetValue(controlName, out Action<string> setParameter))
                    //{
                    //    setParameter(paramValue);
                    //}
                    (this.Controls.Find(controlName, true)[0] as TextBox).Text = paramValue;
                }
                MessageBox.Show("导入成功！", "提示");
            }
            catch (XmlException xmlEx)
            {
                MessageBox.Show($"XML格式错误: {xmlEx.Message}", "错误");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入过程中发生错误: {ex.Message}", "错误");
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnParameter_74_Click(object sender, EventArgs e)
        {
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Title = "保存文件",
                    Filter = "XML文件|*.xml"
                };

                if (!saveFileDialog.ShowDialog().Equals(DialogResult.OK))
                    return;

                string filePath = saveFileDialog.FileName;
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine("<documentParams>");

                    index = 1;
                    // 导出参数
                    ExportParameters(sw, 1, 6, 4);   // 导出1-6参数组，    每组4个
                    ExportParameters(sw, 7, 8, 8);   // 导出第7-8组参数，  每组8个
                    ExportParameters(sw, 9, 11, 4);  // 导出第9-11组参数,  每组4个
                    ExportParameters(sw, 12, 13, 4); // 导出第12-13组参数，每组4个
                    ExportParameters(sw, 14, 14, 2); // 导出第14组参数，   2个
                    ExportParameters(sw, 15, 15, 8); // 导出第15组参数，   8个
                    ExportParameters(sw, 16, 16, 6); // 导出第16组参数，   6个
                    ExportParameters(sw, 17, 17, 4); // 导出第17组参数，   4个

                    sw.WriteLine("</documentParams>");
                }
                MessageBox.Show("导出成功！", "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出过程中发生错误: {ex.Message}", "导出错误");
            }
        }
        private void ExportParameters(StreamWriter sw, int startGroup, int endGroup, int parameterCount)
        {
            for (int i = startGroup; i <= endGroup; i++)
            {
                for (int k = 1; k <= parameterCount; k++)
                {
                    string controlName = $"txt_{index++}";
                    string val = (this.Controls.Find(controlName, true)[0] as TextBox).Text.Trim();

                    sw.WriteLine($" <param id=\"{controlName}\" name=\"{AlarmParametersDic[controlName]}\">{val}</param>");
                }
            }
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
    }

    public class AlarmParameter
    {
        private string _alarmParameter_1_1 = "";
        private string _alarmParameter_1_2 = "";
        private string _alarmParameter_1_3 = "";
        private string _alarmParameter_1_4 = "";
        private string _alarmParameter_2_1 = "";
        private string _alarmParameter_2_2 = "";
        private string _alarmParameter_2_3 = "";
        private string _alarmParameter_2_4 = "";
        private string _alarmParameter_3_1 = "";
        private string _alarmParameter_3_2 = "";
        private string _alarmParameter_3_3 = "";
        private string _alarmParameter_3_4 = "";
        private string _alarmParameter_4_1 = "";
        private string _alarmParameter_4_2 = "";
        private string _alarmParameter_4_3 = "";
        private string _alarmParameter_4_4 = "";
        private string _alarmParameter_5_1 = "";
        private string _alarmParameter_5_2 = "";
        private string _alarmParameter_5_3 = "";
        private string _alarmParameter_5_4 = "";
        private string _alarmParameter_6_1 = "";
        private string _alarmParameter_6_2 = "";
        private string _alarmParameter_6_3 = "";
        private string _alarmParameter_6_4 = "";
        private string _alarmParameter_7_1 = "";
        private string _alarmParameter_7_2 = "";
        private string _alarmParameter_7_3 = "";
        private string _alarmParameter_7_4 = "";
        private string _alarmParameter_7_5 = "";
        private string _alarmParameter_7_6 = "";
        private string _alarmParameter_7_7 = "";
        private string _alarmParameter_7_8 = "";
        private string _alarmParameter_8_1 = "";
        private string _alarmParameter_8_2 = "";
        private string _alarmParameter_8_3 = "";
        private string _alarmParameter_8_4 = "";
        private string _alarmParameter_8_5 = "";
        private string _alarmParameter_8_6 = "";
        private string _alarmParameter_8_7 = "";
        private string _alarmParameter_8_8 = "";
        private string _alarmParameter_9_1 = "";
        private string _alarmParameter_9_2 = "";
        private string _alarmParameter_9_3 = "";
        private string _alarmParameter_9_4 = "";
        private string _alarmParameter_10_1 = "";
        private string _alarmParameter_10_2 = "";
        private string _alarmParameter_10_3 = "";
        private string _alarmParameter_10_4 = "";
        private string _alarmParameter_11_1 = "";
        private string _alarmParameter_11_2 = "";
        private string _alarmParameter_11_3 = "";
        private string _alarmParameter_11_4 = "";
        private string _alarmParameter_12_1 = "";
        private string _alarmParameter_12_2 = "";
        private string _alarmParameter_12_3 = "";
        private string _alarmParameter_12_4 = "";
        private string _alarmParameter_13_1 = "";
        private string _alarmParameter_13_2 = "";
        private string _alarmParameter_13_3 = "";
        private string _alarmParameter_13_4 = "";
        private string _alarmParameter_14_1 = "";
        private string _alarmParameter_14_2 = "";
        private string _alarmParameter_15_1 = "";
        private string _alarmParameter_15_2 = "";
        private string _alarmParameter_15_3 = "";
        private string _alarmParameter_15_4 = "";
        private string _alarmParameter_15_5 = "";
        private string _alarmParameter_15_6 = "";
        private string _alarmParameter_15_7 = "";
        private string _alarmParameter_15_8 = "";
        private string _alarmParameter_16_1 = "";
        private string _alarmParameter_16_2 = "";
        private string _alarmParameter_16_3 = "";
        private string _alarmParameter_16_4 = "";
        private string _alarmParameter_16_5 = "";
        private string _alarmParameter_16_6 = "";
        private string _alarmParameter_17_1 = "";
        private string _alarmParameter_17_2 = "";
        private string _alarmParameter_17_3 = "";
        private string _alarmParameter_17_4 = "";

        public string AlarmParameter_1_1 { get => _alarmParameter_1_1; set => _alarmParameter_1_1 = value; }
        public string AlarmParameter_1_2 { get => _alarmParameter_1_2; set => _alarmParameter_1_2 = value; }
        public string AlarmParameter_1_3 { get => _alarmParameter_1_3; set => _alarmParameter_1_3 = value; }
        public string AlarmParameter_1_4 { get => _alarmParameter_1_4; set => _alarmParameter_1_4 = value; }
        public string AlarmParameter_2_1 { get => _alarmParameter_2_1; set => _alarmParameter_2_1 = value; }
        public string AlarmParameter_2_2 { get => _alarmParameter_2_2; set => _alarmParameter_2_2 = value; }
        public string AlarmParameter_2_3 { get => _alarmParameter_2_3; set => _alarmParameter_2_3 = value; }
        public string AlarmParameter_2_4 { get => _alarmParameter_2_4; set => _alarmParameter_2_4 = value; }
        public string AlarmParameter_3_1 { get => _alarmParameter_3_1; set => _alarmParameter_3_1 = value; }
        public string AlarmParameter_3_2 { get => _alarmParameter_3_2; set => _alarmParameter_3_2 = value; }
        public string AlarmParameter_3_3 { get => _alarmParameter_3_3; set => _alarmParameter_3_3 = value; }
        public string AlarmParameter_3_4 { get => _alarmParameter_3_4; set => _alarmParameter_3_4 = value; }
        public string AlarmParameter_4_1 { get => _alarmParameter_4_1; set => _alarmParameter_4_1 = value; }
        public string AlarmParameter_4_2 { get => _alarmParameter_4_2; set => _alarmParameter_4_2 = value; }
        public string AlarmParameter_4_3 { get => _alarmParameter_4_3; set => _alarmParameter_4_3 = value; }
        public string AlarmParameter_4_4 { get => _alarmParameter_4_4; set => _alarmParameter_4_4 = value; }
        public string AlarmParameter_5_1 { get => _alarmParameter_5_1; set => _alarmParameter_5_1 = value; }
        public string AlarmParameter_5_2 { get => _alarmParameter_5_2; set => _alarmParameter_5_2 = value; }
        public string AlarmParameter_5_3 { get => _alarmParameter_5_3; set => _alarmParameter_5_3 = value; }
        public string AlarmParameter_5_4 { get => _alarmParameter_5_4; set => _alarmParameter_5_4 = value; }
        public string AlarmParameter_6_1 { get => _alarmParameter_6_1; set => _alarmParameter_6_1 = value; }
        public string AlarmParameter_6_2 { get => _alarmParameter_6_2; set => _alarmParameter_6_2 = value; }
        public string AlarmParameter_6_3 { get => _alarmParameter_6_3; set => _alarmParameter_6_3 = value; }
        public string AlarmParameter_6_4 { get => _alarmParameter_6_4; set => _alarmParameter_6_4 = value; }
        public string AlarmParameter_7_1 { get => _alarmParameter_7_1; set => _alarmParameter_7_1 = value; }
        public string AlarmParameter_7_2 { get => _alarmParameter_7_2; set => _alarmParameter_7_2 = value; }
        public string AlarmParameter_7_3 { get => _alarmParameter_7_3; set => _alarmParameter_7_3 = value; }
        public string AlarmParameter_7_4 { get => _alarmParameter_7_4; set => _alarmParameter_7_4 = value; }
        public string AlarmParameter_7_5 { get => _alarmParameter_7_5; set => _alarmParameter_7_5 = value; }
        public string AlarmParameter_7_6 { get => _alarmParameter_7_6; set => _alarmParameter_7_6 = value; }
        public string AlarmParameter_7_7 { get => _alarmParameter_7_7; set => _alarmParameter_7_7 = value; }
        public string AlarmParameter_7_8 { get => _alarmParameter_7_8; set => _alarmParameter_7_8 = value; }
        public string AlarmParameter_8_1 { get => _alarmParameter_8_1; set => _alarmParameter_8_1 = value; }
        public string AlarmParameter_8_2 { get => _alarmParameter_8_2; set => _alarmParameter_8_2 = value; }
        public string AlarmParameter_8_3 { get => _alarmParameter_8_3; set => _alarmParameter_8_3 = value; }
        public string AlarmParameter_8_4 { get => _alarmParameter_8_4; set => _alarmParameter_8_4 = value; }
        public string AlarmParameter_8_5 { get => _alarmParameter_8_5; set => _alarmParameter_8_5 = value; }
        public string AlarmParameter_8_6 { get => _alarmParameter_8_6; set => _alarmParameter_8_6 = value; }
        public string AlarmParameter_8_7 { get => _alarmParameter_8_7; set => _alarmParameter_8_7 = value; }
        public string AlarmParameter_8_8 { get => _alarmParameter_8_8; set => _alarmParameter_8_8 = value; }
        public string AlarmParameter_9_1 { get => _alarmParameter_9_1; set => _alarmParameter_9_1 = value; }
        public string AlarmParameter_9_2 { get => _alarmParameter_9_2; set => _alarmParameter_9_2 = value; }
        public string AlarmParameter_9_3 { get => _alarmParameter_9_3; set => _alarmParameter_9_3 = value; }
        public string AlarmParameter_9_4 { get => _alarmParameter_9_4; set => _alarmParameter_9_4 = value; }
        public string AlarmParameter_10_1 { get => _alarmParameter_10_1; set => _alarmParameter_10_1 = value; }
        public string AlarmParameter_10_2 { get => _alarmParameter_10_2; set => _alarmParameter_10_2 = value; }
        public string AlarmParameter_10_3 { get => _alarmParameter_10_3; set => _alarmParameter_10_3 = value; }
        public string AlarmParameter_10_4 { get => _alarmParameter_10_4; set => _alarmParameter_10_4 = value; }
        public string AlarmParameter_11_1 { get => _alarmParameter_11_1; set => _alarmParameter_11_1 = value; }
        public string AlarmParameter_11_2 { get => _alarmParameter_11_2; set => _alarmParameter_11_2 = value; }
        public string AlarmParameter_11_3 { get => _alarmParameter_11_3; set => _alarmParameter_11_3 = value; }
        public string AlarmParameter_11_4 { get => _alarmParameter_11_4; set => _alarmParameter_11_4 = value; }
        public string AlarmParameter_12_1 { get => _alarmParameter_12_1; set => _alarmParameter_12_1 = value; }
        public string AlarmParameter_12_2 { get => _alarmParameter_12_2; set => _alarmParameter_12_2 = value; }
        public string AlarmParameter_12_3 { get => _alarmParameter_12_3; set => _alarmParameter_12_3 = value; }
        public string AlarmParameter_12_4 { get => _alarmParameter_12_4; set => _alarmParameter_12_4 = value; }
        public string AlarmParameter_13_1 { get => _alarmParameter_13_1; set => _alarmParameter_13_1 = value; }
        public string AlarmParameter_13_2 { get => _alarmParameter_13_2; set => _alarmParameter_13_2 = value; }
        public string AlarmParameter_13_3 { get => _alarmParameter_13_3; set => _alarmParameter_13_3 = value; }
        public string AlarmParameter_13_4 { get => _alarmParameter_13_4; set => _alarmParameter_13_4 = value; }
        public string AlarmParameter_14_1 { get => _alarmParameter_14_1; set => _alarmParameter_14_1 = value; }
        public string AlarmParameter_14_2 { get => _alarmParameter_14_2; set => _alarmParameter_14_2 = value; }
        public string AlarmParameter_15_1 { get => _alarmParameter_15_1; set => _alarmParameter_15_1 = value; }
        public string AlarmParameter_15_2 { get => _alarmParameter_15_2; set => _alarmParameter_15_2 = value; }
        public string AlarmParameter_15_3 { get => _alarmParameter_15_3; set => _alarmParameter_15_3 = value; }
        public string AlarmParameter_15_4 { get => _alarmParameter_15_4; set => _alarmParameter_15_4 = value; }
        public string AlarmParameter_15_5 { get => _alarmParameter_15_5; set => _alarmParameter_15_5 = value; }
        public string AlarmParameter_15_6 { get => _alarmParameter_15_6; set => _alarmParameter_15_6 = value; }
        public string AlarmParameter_15_7 { get => _alarmParameter_15_7; set => _alarmParameter_15_7 = value; }
        public string AlarmParameter_15_8 { get => _alarmParameter_15_8; set => _alarmParameter_15_8 = value; }
        public string AlarmParameter_16_1 { get => _alarmParameter_16_1; set => _alarmParameter_16_1 = value; }
        public string AlarmParameter_16_2 { get => _alarmParameter_16_2; set => _alarmParameter_16_2 = value; }
        public string AlarmParameter_16_3 { get => _alarmParameter_16_3; set => _alarmParameter_16_3 = value; }
        public string AlarmParameter_16_4 { get => _alarmParameter_16_4; set => _alarmParameter_16_4 = value; }
        public string AlarmParameter_16_5 { get => _alarmParameter_16_5; set => _alarmParameter_16_5 = value; }
        public string AlarmParameter_16_6 { get => _alarmParameter_16_6; set => _alarmParameter_16_6 = value; }
        public string AlarmParameter_17_1 { get => _alarmParameter_17_1; set => _alarmParameter_17_1 = value; }
        public string AlarmParameter_17_2 { get => _alarmParameter_17_2; set => _alarmParameter_17_2 = value; }
        public string AlarmParameter_17_3 { get => _alarmParameter_17_3; set => _alarmParameter_17_3 = value; }
        public string AlarmParameter_17_4 { get => _alarmParameter_17_4; set => _alarmParameter_17_4 = value; }
    }
}
