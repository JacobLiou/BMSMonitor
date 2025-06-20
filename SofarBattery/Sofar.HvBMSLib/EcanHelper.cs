using Sofar.ProtocolLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using PowerKit.Domain.Enums;
using Sofar.HvBMSLib;

namespace Sofar.BMSLib
{
    public class EcanHelper : BaseCanHelper
    {
        private static EcanHelper instance = null;
        public static EcanHelper Instance()
        {

            if (instance == null)
            {
                lock (obj)
                {
                    if (instance == null)
                    {
                        instance = new EcanHelper(BMSConfig.ConfigManager);
                    }
                }
            }
            return instance;

        }

        private static readonly object obj = new object();

        private string _baudRate;
        private UInt32 _devType;
        private UInt32 _devIndex;
        private UInt32 _channel;
        public EcanHelper(BMSConfigurationModel config)
        {
            if (config != null && !string.IsNullOrEmpty(config.CAN_DevType))
            {
                _baudRate = config.CAN_BaudRate;
                _devType = (uint)GetDeviceType(config.CAN_DevType);
                _devIndex = Convert.ToUInt32(config.CAN_DevIndex);
                _channel = Convert.ToUInt32(config.CAN_Channel);
            }

        }
        public override bool IsConnection { get; set; }
        public override string CommunicationType { get; set; } = "Ecan";
        private int Can_error_count = 0;

        public delegate void ReceiveInfo(CAN_OBJ _obj);
        public event ReceiveInfo ReceivecEventHandler;
        public bool StartListen { get; set; }

        public const int REC_MSG_BUF_MAX = 0x2710;

        public CAN_OBJ[] gRecMsgBuf = new CAN_OBJ[REC_MSG_BUF_MAX];
        public uint gRecMsgBufHead = 0;
        public uint gRecMsgBufTail = 0;

        public const int SEND_MSG_BUF_MAX = 0x2710;

        public CAN_OBJ[] gSendMsgBuf = new CAN_OBJ[SEND_MSG_BUF_MAX];
        public uint gSendMsgBufHead = 0;
        public uint gSendMsgBufTail = 0;

        /*创建一个更新收发数据显示的线程*/
        public readonly static object _locker = new object();
        public readonly static object _lockerSend = new object();
        public static ConcurrentQueue<CAN_OBJ> _task = new ConcurrentQueue<CAN_OBJ>();
        public ConcurrentDictionary<int, Byte[]> Devices = new ConcurrentDictionary<int, Byte[]>();

        public static EventWaitHandle _wh = new AutoResetEvent(false);

        // 获取设备类型
        private PCIDeviceType GetDeviceType(string deviceType)
        {
            return deviceType switch
            {
                "USBCAN-I/I+" => PCIDeviceType.VCI_USBCAN1,
                "USBCAN-II/II+" => PCIDeviceType.VCI_USBCAN2,
                _ => throw new ArgumentException($"不支持的设备类型: {deviceType}")
            };
        }

        public override void Connect()
        {
            if (!IsConnection)
            {

                if (ECANHelper.OpenDevice(_devType, _devIndex, _channel) != ECANStatus.STATUS_OK)
                {
                    //MessageBoxHelper.Warning($"打开CAN设备失败!", "警告", null, ButtonType.OK);
                    throw new Exception("打开CAN设备失败!");
                    return;
                }

                INIT_CONFIG INITCONFIG = new INIT_CONFIG();
                INITCONFIG.AccCode = 0;
                INITCONFIG.AccMask = 0xffffffff;
                INITCONFIG.Filter = 0;

                switch (_baudRate)
                {
                    case "5Kbps":
                        INITCONFIG.Timing0 = 0xBF;
                        INITCONFIG.Timing1 = 0xFF;
                        break;
                    case "10Kbps":
                        INITCONFIG.Timing0 = 0x31;
                        INITCONFIG.Timing1 = 0x1C;
                        break;
                    case "20Kbps":
                        INITCONFIG.Timing0 = 0x18;
                        INITCONFIG.Timing1 = 0x1C;
                        break;
                    case "40Kbps":
                        INITCONFIG.Timing0 = 0x87;
                        INITCONFIG.Timing1 = 0xFF;
                        break;
                    case "50Kbps":
                        INITCONFIG.Timing0 = 0x09;
                        INITCONFIG.Timing1 = 0x1C;
                        break;
                    case "80Kbps":
                        INITCONFIG.Timing0 = 0x83;
                        INITCONFIG.Timing1 = 0xFF;
                        break;
                    case "100Kbps":
                        INITCONFIG.Timing0 = 0x04;
                        INITCONFIG.Timing1 = 0x1C;
                        break;
                    case "125Kbps":
                        INITCONFIG.Timing0 = 0x03;
                        INITCONFIG.Timing1 = 0x1C;
                        break;
                    case "200Kbps":
                        INITCONFIG.Timing0 = 0x81;
                        INITCONFIG.Timing1 = 0xFA;
                        break;
                    case "250Kbps":
                        INITCONFIG.Timing0 = 0x01;
                        INITCONFIG.Timing1 = 0x1C;
                        break;
                    case "400Kbps":
                        INITCONFIG.Timing0 = 0x80;
                        INITCONFIG.Timing1 = 0xB6;
                        break;
                    case "500Kbps":
                        INITCONFIG.Timing0 = 0x00;
                        INITCONFIG.Timing1 = 0x1C;
                        break;
                    case "666Kbps":
                        INITCONFIG.Timing0 = 0x80;
                        INITCONFIG.Timing1 = 0xB6;
                        break;
                    case "800Kbps":
                        INITCONFIG.Timing0 = 0x00;
                        INITCONFIG.Timing1 = 0x16;
                        break;
                    case "1000Kbps":
                        INITCONFIG.Timing0 = 0x00;
                        INITCONFIG.Timing1 = 0x14;
                        break;
                }

                INITCONFIG.Mode = 0;


                if (ECANHelper.InitCAN(_devType, _devIndex, _channel, ref INITCONFIG) != ECANStatus.STATUS_OK)
                {
                    //MessageBoxHelper.Warning("打开CAN设备失败!", "警告", null, ButtonType.OK);
                    ECANHelper.CloseDevice(_devType, _devIndex);
                    throw new Exception("初始化CAN设备失败!");
                    return;
                }

                if (ECANHelper.StartCAN(_devType, _devIndex, _channel) != ECANStatus.STATUS_OK)
                {
                    //MessageBoxHelper.Warning("打开CAN设备失败!", "警告", null, ButtonType.OK);
                    ECANHelper.CloseDevice(_devType, _devIndex);
                    throw new Exception("启动CAN设备失败!");
                    return;
                }
                IsConnection = true;

                //使用多线程实时获取接收数据
                Receive();
            }
        }
        public override bool Send(Byte[] data, byte[] canid)
        {

            lock (_lockerSend)
            {
                CAN_OBJ co = new CAN_OBJ();
                co.SendType = 0;
                co.DataLen = 8;
                co.Data = data;
                co.ID = BitConverter.ToUInt32(canid, 0);

                gSendMsgBuf[gSendMsgBufHead].ID = co.ID;
                gSendMsgBuf[gSendMsgBufHead].Data = co.Data;
                gSendMsgBuf[gSendMsgBufHead].DataLen = co.DataLen;
                gSendMsgBuf[gSendMsgBufHead].ExternFlag = 1;
                gSendMsgBuf[gSendMsgBufHead].RemoteFlag = 0;
                gSendMsgBufHead++;
                if (gSendMsgBufHead >= SEND_MSG_BUF_MAX)
                {
                    gSendMsgBufHead = 0;
                }

                CAN_OBJ[] coMsg = new CAN_OBJ[1];

                if (gSendMsgBufHead != gSendMsgBufTail)
                {
                    coMsg[0] = gSendMsgBuf[gSendMsgBufTail];
                    gSendMsgBufTail++;

                    if (gSendMsgBufTail >= SEND_MSG_BUF_MAX)
                    {
                        gSendMsgBufTail = 0;
                    }
                    LogAction?.Invoke(1, HexDataHelper.GetDebugByteString(data, "Send：0x" + co.ID.ToString("X")));
                    if (ECANHelper.Transmit(_devType, _devIndex, _channel, coMsg, 1) == ECANStatus.STATUS_OK)
                    {
                        CAN_ERR_INFO err_info = new CAN_ERR_INFO();
                        var v = ECANHelper.ReadErrInfo(_devType, _devIndex, _channel, out err_info) == ECANStatus.STATUS_OK;
                        if (err_info.ErrCode == 0x00)//成功
                        {
                            //Log.Info($"{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")} CAN通信协议—发送数据:{BitConverter.ToString(data).Replace("-", " ")}  帧ID:{co.ID.ToString("X8")}");
                            return true;
                        }
                        else if (err_info.ErrCode == 0x400)
                        {
                            //Log.Info($"{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}-->CAN通信协议—发送失败");
                        }
                    }
                }
            }
            return false;
        }

        public override void Receive()
        {
            ReceivecEventHandler += EnqueueTask;
            ReceivecEventHandler += EnqueueTask_MLQ;

            Task.Run(() =>
            {
                while (true)
                {
                    CAN_OBJ coMsg = new CAN_OBJ();

                    if (ECANHelper.Receive(_devType, _devIndex, _channel, out coMsg, 1, 1) == ECANStatus.STATUS_OK)
                    {
                        gRecMsgBuf[gRecMsgBufHead].ID = coMsg.ID;
                        gRecMsgBuf[gRecMsgBufHead].Data = coMsg.Data;
                        gRecMsgBuf[gRecMsgBufHead].DataLen = coMsg.DataLen;
                        gRecMsgBuf[gRecMsgBufHead].ExternFlag = coMsg.ExternFlag;
                        gRecMsgBuf[gRecMsgBufHead].RemoteFlag = coMsg.RemoteFlag;
                        gRecMsgBuf[gRecMsgBufHead].TimeStamp = coMsg.TimeStamp;
                        gRecMsgBuf[gRecMsgBufHead].Reserved = coMsg.Reserved;
                        gRecMsgBuf[gRecMsgBufHead].TimeFlag = coMsg.TimeFlag;
                        gRecMsgBufHead += 1;
                        if (gRecMsgBufHead >= REC_MSG_BUF_MAX)
                        {
                            gRecMsgBufHead = 0;
                        }

                        //进入队列前，先进行筛选（集合内的ID可加入至队列，否则过滤掉）
                        foreach (Protocols item in Protocols.protocols)
                        {
                            //uint index = 0x00;
                            //switch (item.Index)
                            //{
                            //    case 0: index = 0xff000000; break;
                            //    case 1: index = 0xff0000; break;
                            //    case 2: index = 0xff00; break;
                            //    case 3: index = 0xff; break;
                            //}

                            uint revId = coMsg.ID | 0xff;
                            uint devId = AnalysisID_EVBCM(coMsg.ID);
                            if (revId == item.Id)
                            {
                                //EnqueueTask(coMsg);
                                ReceivecEventHandler(coMsg);
                                //Log.Info($"{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")} CAN通信协议—接收数据:{coMsg}  帧ID:{revId.ToString("X8")}");
                                break;
                            }
                        }
                    }
                }
            });
        }

        /// <summary>
        /// EVBCM协议数据单元（PDU）,遵循 J1939 协议: P 是优先级，R 是保留位，DP 是数据页，PF 是 PDU 格式，PS 是特定 PDU，SA 是源地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public uint AnalysisID_EVBCM(uint id)
        {
            // SA源地址（bit0~bit7）
            uint sa = (id & 0xFF);

            // PS目标地址（bit8~bit15）
            uint ps = ((id >> 8) & 0xFF);

            // PF功能码(bit16~bit23)
            uint pf = ((id >> 16) & 0xFF);

            // DP数据页(bit24)
            uint dp = ((id >> 24) & 0x1);

            // R预留(bit25)
            uint r = ((id >> 25) & 0x1);

            // PR优先级(bit26~bit28)
            uint pr = ((id >> 26) & 0x7);

            return sa;
        }
        public override string ReadError()
        {
            string error = string.Empty;

            CAN_ERR_INFO err_info = new CAN_ERR_INFO();

            if (ECANHelper.ReadErrInfo(_devType, _devIndex, _channel, out err_info) == ECANStatus.STATUS_OK)
            {
                error = "当前错误码：" + err_info.ErrCode.ToString("X2");

                if (err_info.ErrCode >= 0x01 && err_info.ErrCode < 0x100)
                {
                    if (Can_error_count < 10)
                    {
                        Can_error_count++;
                    }
                    else
                    {
                        if (ECANHelper.ResetCAN(_devType, _devIndex, _channel) == ECANStatus.STATUS_OK)
                        {
                            Can_error_count = 0;
                            ECANHelper.StartCAN(_devType, _devIndex, _channel);
                            Debug.WriteLine($"当前错误码：{0}，执行了复位CAN操作");
                        }
                    }
                }
                else { Can_error_count = 0; }
            }
            else
            {
                error = "Read_Error Fault";
            }

            return error;
        }

        private void EnqueueTask(CAN_OBJ CANOBJ)
        {
            lock (_locker)
            {
                //测试打印接收报文
                //Debug.WriteLine($"{System.DateTime.Now.ToString("hh:mm:ss:fff")} 入队数据   帧ID:{CANOBJ.ID.ToString("X8")}");
                //LogAction?.Invoke(1, HexDataHelper.GetDebugByteString(CANOBJ.Data, "Recv：0x" + CANOBJ.ID.ToString("X")));
                _task.Enqueue(CANOBJ);
                //_wh.Set();
            }
        }

        private void EnqueueTask_MLQ(CAN_OBJ coMsg)
        {
            //未启动监听，终止入队操作
            if (!StartListen)
                return;

            int devId = (int)coMsg.ID;
            if (!Devices.TryAdd(devId, coMsg.Data))
            {
                Devices[devId] = coMsg.Data;
            }

            string ss = "";
            for (int i = 0; i < coMsg.Data.Length; i++)
            {
                ss += " " + coMsg.Data[i].ToString("X2");
            }

            //Debug.WriteLine($"{System.DateTime.Now.ToString("hh:mm:ss:fff")} Dev:{devId} CAN_ID:{coMsg.ID.ToString("X8")},Data：{ss.ToString()}");

        }
    }
}

