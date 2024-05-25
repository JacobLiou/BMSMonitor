using SofarBMS.Helper;
using SofarBMS.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SofarBMS.UI
{
    public partial class CBSFileTransmit : UserControl
    {
        public static CancellationTokenSource cts = new CancellationTokenSource();

        //变量定义｛｝
        private readonly string FiveMinHeadStr = "时间,电池采集电压(mV),电池累计电压(mV),SOC显示值(%),SOH显示值(%),SOC计算值,SOH计算值,电池电流(mA),最高单体电压(mV),最低单体电压(mV),最高单体电压序号,最低单体电压序号,最高单体温度(℃),最低单体温度(℃),最高单体温度序号,最低单体温度序号,BMU编号,系统状态,充放电使能,切断请求,关机请求,充电电流上限(A),放电电流上限(A),保护1,保护2,告警1,告警2,故障1,故障2,主动均衡状态,均衡母线电压(mV),均衡母线电流(mA),辅助供电电压(mV),满充容量(Ah),循环次数,累计放电安时(Ah),累计充电安时(Ah),累计放电瓦时(Wh),累计充电瓦时(Wh),环境温度(℃),DCDC温度1(℃),DCDC温度2(℃),均衡温度1(℃),均衡温度2(℃),1-16串均衡状态,单体电压1(mV),单体电压2(mV),单体电压3(mV),单体电压4(mV),单体电压5(mV),单体电压6(mV),单体电压7(mV),单体电压8(mV),单体电压9(mV),单体电压10(mV),单体电压11(mV),单体电压12(mV),单体电压13(mV),单体电压14(mV),单体电压15(mV),单体电压16(mV),单体温度1(℃),单体温度2(℃),单体温度3(℃),单体温度4(℃),单体温度5(℃),单体温度6(℃),单体温度7(℃),单体温度8(℃),单体温度9(℃),单体温度10(℃),单体温度11(℃),单体温度12(℃),单体温度13(℃),单体温度14(℃),单体温度15(℃),单体温度16(℃),RSV1,RSV2,RSV3,RSV4,RSV5,RSV6,RSV7,RSV8,RSV9,RSV10\r\n";
        private readonly string FaultRecordStr = "电流(A),最大电压(mV),最小电压(mV),最大温度(℃),最小温度(℃)\r\n";

        bool state = false;
        StepRemark stepFlag;

        int slaveAddress = -1;
        int fileNumber = -1;
        int readType = -1;

        int fileOffset = 200;
        int dataLength = 200;

        int fileSize = 0;

        public CBSFileTransmit()
        {
            InitializeComponent();

            this.Load += CBSFileTransmit_Load;
        }

        private void CBSFileTransmit_Load(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    lock (EcanHelper._locker)
                    {
                        while (EcanHelper._task.Count > 0)
                        {
                            //出队
                            CAN_OBJ ch = (CAN_OBJ)EcanHelper._task.Dequeue();

                            //解析
                            this.Invoke(new Action(() => { AnalysisData(ch.ID, ch.Data); }));
                        }
                    }
                }
            });

            //初始化值
            this.txtSlaveAddress.Text = FrmMain.BMS_ID.ToString();
            this.cbbFileNumber.SelectedIndex = 3;
            this.ckReadAll.Checked = true;
        }

        private void btnFileTransmit_Click(object sender, EventArgs e)
        {
            fileNumber = cbbFileNumber.SelectedIndex;
            readType = ckReadAll.Checked ? 0 : 1;

            Task.Run(() => { StartTransmit(); }, cts.Token);
        }

        private void AnalysisData(uint id, byte[] data)
        {
            id = id | 0xff;

            switch (stepFlag)
            {
                case StepRemark.None:
                    break;
                case StepRemark.读文件指令帧:
                    if (id == 0x7F0E0FF && data[3] == 0x0)
                    {
                        //U32:BitConverter.ToUInt32(new byte[] { data[4], data[5], data[6] },0);
                        fileSize = Convert.ToInt32(data[3].ToString("X2") + data[4].ToString("X2") + data[5].ToString("X2"), 16);
                    }
                    break;
                case StepRemark.读文件数据内容帧:
                    //文件数据
                    break;
                case StepRemark.查询完成状态帧:
                    if (id == 0x7F4E0FF && data[3] == 0x0)
                    {
                        Debug.WriteLine("success：查询完成状态帧！");
                    }
                    break;
                default:
                    break;
            }
        }

        private void To5MinFeatures(byte[] dataBuffer)
        {
            if (dataBuffer.Length != 200)
                return;

            //代号,数据类型,数据长度,精度,单位
            string protocolText = @"时间,U8,6,1,1,
电池采样电压,U16,1,1,mV
电池累计电压,U16,1,1,mV
SOC显示值,U8,1,1,%
SOH显示值,U8,1,1,%
SOC计算值,U32,1,0.001,%
SOH计算值,U32,1,0.001,%
电池电流,I16,1,1,mA
最高单体电压,U16,1,1,mV
最低单体电压,U16,1,1,mV
最高单体电压序号,U8,1,,
最低单体电压序号,U8,1,,
最高单体温度,U16,1,0.1,℃
最低单体温度,U16,1,0.1,℃
最高单体温度序号,U8,1,,
最低单体温度序号,U8,1,,
BMU编号,U8,1,,
系统状态,U8,1,,
充放电使能,U16,1,,
切断请求,U8,1,,
关机请求,U8,1,,
充电电流上限,U16,1,1,A
放电电流上限,U16,1,1,A
保护1,U16,1,,
保护2,U16,1,,
告警1,U16,1,,
告警2,U16,1,,
故障1,U16,1,,
故障2,U16,1,,
主动均衡状态,U16,1,,
均衡母线电压,U16,1,,mV
均衡母线电流,I16,1,,mA
辅助供电电压,U16,1,,mV
满充容量,U16,1,,Ah
循环次数,U16,1,,
累计放电安时,U32,1,1,Ah
累计充电安时,U32,1,1,Ah
累计放电瓦时,U32,1,1,Wh
累计充电瓦时,U32,1,1,Wh
环境温度,I16,1,0.1,℃
dcdc温度1,I16,1,0.1,℃
dcdc温度2,I16,1,0.1,℃
均衡温度1,I16,1,0.1,℃
均衡温度2,I16,1,0.1,℃
1-16串均衡状态,U16,1,1,
单体电压1,U16,1,1,mV
单体电压2,U16,1,1,mV
单体电压3,U16,1,1,mV
单体电压4,U16,1,1,mV
单体电压5,U16,1,1,mV
单体电压6,U16,1,1,mV
单体电压7,U16,1,1,mV
单体电压8,U16,1,1,mV
单体电压9,U16,1,1,mV
单体电压10,U16,1,1,mV
单体电压11,U16,1,1,mV
单体电压12,U16,1,1,mV
单体电压13,U16,1,1,mV
单体电压14,U16,1,1,mV
单体电压15,U16,1,1,mV
单体电压16,U16,1,1,mV
单体温度1,I16,1,0.1,℃
单体温度2,I16,1,0.1,℃
单体温度3,I16,1,0.1,℃
单体温度4,I16,1,0.1,℃
单体温度5,I16,1,0.1,℃
单体温度6,I16,1,0.1,℃
单体温度7,I16,1,0.1,℃
单体温度8,I16,1,0.1,℃
单体温度9,I16,1,0.1,℃
单体温度10,I16,1,0.1,℃
单体温度11,I16,1,0.1,℃
单体温度12,I16,1,0.1,℃
单体温度13,I16,1,0.1,℃
单体温度14,I16,1,0.1,℃
单体温度15,I16,1,0.1,℃
单体温度16,I16,1,0.1,℃
RSV1,U32,1,,
RSV2,U32,1,,
RSV3,U32,1,,
RSV4,U32,1,,
RSV5,U32,1,,
RSV6,U32,1,,
RSV7,U32,1,,
RSV8,U32,1,,
RSV9,U32,1,,
RSV10,U32,1,,
";
        }

        private void ToFaultRecord(byte[] dataBuffer)
        {
            //代号,数据类型,数据长度,精度,单位
            string protocolText = @"电流,I16,1,1,A
最大电压,U16,1,1,V
最小电压,U16,1,1,V
最大温度,I8,1,1,℃
最小温度,I8,1,1,℃";


        }

        private void StartTransmit()
        {
            int recount = 0;
            if (!state)
            {
                stepFlag = StepRemark.读文件指令帧;

                switch (stepFlag)
                {
                    case StepRemark.None:
                        break;
                    case StepRemark.读文件指令帧:
                        if (ReadFileCommand())
                        {
                            recount = 0;
                            stepFlag = StepRemark.读文件数据内容帧;
                        }
                        else
                        {
                            if (recount == 5)
                            {
                                recount = 0;
                                cts.Cancel(); //终止执行。
                                break;
                            }

                            recount++;
                        }
                        break;
                    case StepRemark.读文件数据内容帧:
                        if (ReadFileDataContent())
                        {
                            recount = 0;
                            stepFlag = StepRemark.查询完成状态帧;
                        }
                        else
                        {
                            if (recount == 5)
                            {
                                recount = 0;
                                cts.Cancel(); //终止执行。
                                break;
                            }

                            recount++;
                        }

                        break;
                    case StepRemark.查询完成状态帧:
                        if (QueryCompletionStatus())
                        {
                            Debug.WriteLine("查询完成状态帧：success!");
                            cts.Cancel();
                        }

                        break;
                    default:
                        break;
                }

                state = true;
            }
            else
            {

                state = false;
            }
        }

        private bool ReadFileCommand()
        {
            byte[] canid = { 0xE0, Convert.ToByte(slaveAddress), 0xF0, 0x07 };

            byte[] data = new byte[8];
            data[0] = 0x0;
            data[1] = (byte)slaveAddress;
            data[2] = (byte)fileNumber;
            data[3] = (byte)readType;

            EcanHelper.Send(data, canid);
            AddLog(System.DateTime.Now.ToString("HH:mm:ss:fff"), "F0", byteToHexString(data));

            //==发送等待应答结果
            bool isOk = false;
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (true)
            {
                //if (false)//接收区域传参...
                {
                    isOk = true;
                    break;
                }

                if (stopwatch.ElapsedMilliseconds > 1000)
                {
                    stopwatch.Stop();
                    break;
                }
            }

            return isOk;
        }

        private bool ReadFileDataContent()
        {
            byte[] canid = { 0xE0, Convert.ToByte(slaveAddress), 0xF1, 0x07 };

            byte[] data = new byte[8];
            data[0] = 0x0;
            data[1] = (byte)slaveAddress;
            data[2] = (byte)fileNumber;
            data[3] = (byte)(fileOffset & 0xff);
            data[4] = (byte)(fileOffset >> 8);
            data[5] = (byte)(fileOffset >> 16);
            data[6] = (byte)(dataLength & 0xff);
            data[7] = (byte)(dataLength >> 8);

            EcanHelper.Send(data, canid);
            AddLog(System.DateTime.Now.ToString("HH:mm:ss:fff"), "F1", byteToHexString(data));

            //==发送等待应答结果
            bool isOk = false;

            return isOk;
        }

        private bool QueryCompletionStatus()
        {
            byte[] canid = { 0xE0, Convert.ToByte(slaveAddress), 0xF4, 0x07 };

            byte[] data = new byte[8];
            data[0] = 0x0;
            data[1] = (byte)slaveAddress;
            data[2] = (byte)fileNumber;
            data[3] = (byte)0x01;

            EcanHelper.Send(data, canid);
            AddLog(System.DateTime.Now.ToString("HH:mm:ss:fff"), "F4", byteToHexString(data));

            //==发送等待应答结果
            bool isOk = false;

            return isOk;
        }

        private void AddLog(string date, string id, string context)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Text = date;
            lvi.SubItems.Add(id);
            lvi.SubItems.Add(context);
            this.Invoke(new Action(() =>
            {
                this.lvPrintBlock.Items.Insert(0, lvi);
            }));
        }

        private string byteToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            
            foreach (byte b in data)
            {
                sb.Append(b.ToString("X")+" ");
            }
            
             return sb.ToString();
        }

        private void txtSlaveAddress_TextChanged(object sender, EventArgs e)
        {
            slaveAddress = Convert.ToInt32(txtSlaveAddress.Text);
        }

        private void ckReadAll_CheckedChanged(object sender, EventArgs e)
        {
            if (ckReadAll.Checked)
            {
                txtStartLocal.Enabled = false;
                txtReadCount.Enabled = false;
            }
            else
            {
                txtStartLocal.Enabled = true;
                txtReadCount.Enabled = true;
            }
        }
    }



    enum StepRemark
    {
        None,
        读文件指令帧,
        读文件数据内容帧,
        查询完成状态帧
    }
}
