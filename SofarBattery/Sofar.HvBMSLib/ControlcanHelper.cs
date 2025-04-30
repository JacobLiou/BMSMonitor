using PowerKit.Domain.Enums;
using Sofar.HvBMSLib;
using Sofar.ProtocolLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Sofar.BMSLib
{
    public class ControlcanHelper : BaseCanHelper
    {
        private static ControlcanHelper instance = null;
        private static readonly object obj = new object();
        private string _baudRate;
        private UInt32 _devType;
        private UInt32 _devIndex;
        private UInt32 _channel;

        public override bool IsConnection { get; set; }
        public override string CommunicationType { get; set; } = "Controlcan";
        private int Can_error_count = 0;

        public delegate void ReceiveInfo(VCI_CAN_OBJ _obj);
        public event ReceiveInfo ReceivecEventHandler;
        public bool StartListen { get; set; }

        public const int REC_MSG_BUF_MAX = 0x2710;
        public VCI_CAN_OBJ[] gRecMsgBuf = new VCI_CAN_OBJ[REC_MSG_BUF_MAX];
        public uint gRecMsgBufHead = 0;
        public uint gRecMsgBufTail = 0;

        public const int SEND_MSG_BUF_MAX = 0x2710;
        public VCI_CAN_OBJ[] gSendMsgBuf;
        public uint gSendMsgBufHead;
        public uint gSendMsgBufTail;

        /*创建一个更新收发数据显示的线程*/
        public readonly static object _locker = new object();
        public new static Queue<VCI_CAN_OBJ> _task = new Queue<VCI_CAN_OBJ>();
        public new ConcurrentDictionary<int, Byte[]> Devices = new ConcurrentDictionary<int, Byte[]>();
        public static EventWaitHandle _wh = new AutoResetEvent(false);

        // 私有构造函数
        private ControlcanHelper(BMSConfigurationModel config)
        {
            if (!string.IsNullOrEmpty(config.CAN_DevType))
            {
                _baudRate = config.CAN_BaudRate;
                _devType = (uint)GetDeviceType(config.CAN_DevType);
                _devIndex = Convert.ToUInt32(config.CAN_DevIndex);
                _channel = Convert.ToUInt32(config.CAN_Channel);
            }

        }

        public static ControlcanHelper Instance()
        {

            if (instance == null)
            {
                lock (obj)
                {
                    if (instance == null)
                    {
                        instance = new ControlcanHelper(BMSConfig.ConfigManager);
                    }
                }
            }
            return instance;

        }

        // 获取设备类型
        private PCIDeviceType GetDeviceType(string deviceType)
        {
            return deviceType switch
            {
                "PCI-5121" => PCIDeviceType.VCI_PCI5121,
                "PCI-9810I" => PCIDeviceType.VCI_PCI9810,
                "USBCAN-I/I+" => PCIDeviceType.VCI_USBCAN1,
                "USBCAN-II/II+" => PCIDeviceType.VCI_USBCAN2,
                "PCI-9820" => PCIDeviceType.VCI_PCI9820,
                "CAN232" => PCIDeviceType.VCI_CAN232,
                "PCI-5110" => PCIDeviceType.VCI_PCI5110,
                "CANmini" => PCIDeviceType.VCI_CANLITE,
                "ISA-9620" => PCIDeviceType.VCI_ISA9620,
                "ISA-5420" => PCIDeviceType.VCI_ISA5420,
                "PC104-CAN" => PCIDeviceType.VCI_PC104CAN,
                "CANET系列的UDP工作方式" => PCIDeviceType.VCI_CANETUDP,
                "PCI-9840I" => PCIDeviceType.VCI_PCI9840,
                "PC104-CAN2I" => PCIDeviceType.VCI_PC104CAN2,
                "PCI-9820I" => PCIDeviceType.VCI_PCI9820I,
                "CANET系列的TCP工作方式" => PCIDeviceType.VCI_CANETTCP,
                "PEC-9920" => PCIDeviceType.VCI_PEC9920,
                "PCI-5010-U" => PCIDeviceType.VCI_PCI5010U,
                "USBCAN-E-U" => PCIDeviceType.VCI_USBCAN_E_U,
                "USBCAN-2E-U" => PCIDeviceType.VCI_USBCAN_2E_U,
                "PCI-5020-U" => PCIDeviceType.VCI_PCI5020U,
                "EG20T-CAN" => PCIDeviceType.VCI_EG20T_CAN,
                "PCIe-9221" => PCIDeviceType.VCI_PCIE9221,
                "CANWiFi-200T 的 TCP 工作方式" => PCIDeviceType.VCI_CANWIFI_TCP,
                "CANWiFi-200T 的 UDP 工作方式" => PCIDeviceType.VCI_CANWIFI_UDP,
                "PCIe-9120I" => PCIDeviceType.VCI_PCIe_9120I,
                "PCIe-9110I" => PCIDeviceType.VCI_PCIe_9110I,
                "PCIe-9140I" => PCIDeviceType.VCI_PCIe_9140I,
                "USBCAN-4E-U" => PCIDeviceType.VCI_USBCAN_4E_U,
                "CANDTU" => PCIDeviceType.VCI_CANDTU,
                "USBCAN-8E-U" => PCIDeviceType.VCI_USBCAN_8E_U,
                "CANDTU-NET" => PCIDeviceType.VCI_CANDTU_NET,
                _ => throw new ArgumentException($"不支持的设备类型: {deviceType}")
            };
        }




        public override void Connect()
        {
            if (!IsConnection)
            {
                if (CONTROLCANHelper.VCI_OpenDevice(_devType, _devIndex, _channel) != CONTROLCANSTATUS.STATUS_OK)
                {
                    //MessageBoxHelper.Warning($"打开CAN设备失败!", "警告", null, ButtonType.OK);
                    throw new Exception("打开CAN设备失败!");
                    return;
                }

                VCI_INIT_CONFIG VCIINITCONFIG = new VCI_INIT_CONFIG();
                VCIINITCONFIG.AccCode = 0;//验收码
                VCIINITCONFIG.AccMask = 0xffffffff;//屏蔽码推荐设置为 0xFFFFFFFF，即全部接收
                VCIINITCONFIG.Filter = 0;//1 表示单滤波，0 表示双滤波
                VCIINITCONFIG.Mode = 0;//0 表示正常模式（相当于正常节点），1 表示只听模式（只接收，不影响总线）

                //当设备类型为 PCI-5010-U、PCI-5020-U、USBCAN-E-U、USBCAN-2E-U 时波特率设置
                if (_devType == 19 || _devType == 20 || _devType == 21 || _devType == 22)
                {
                    //常用标准波特率设置
                    int baudRate = _baudRate switch
                    {
                        "5Kbps" => 0x1C01C1,
                        "10Kbps" => 0x1C00E0,
                        "20Kbps" => 0x1600B3,
                        "50Kbps" => 0x1C002C,
                        "100Kbps" => 0x160023,
                        "125Kbps" => 0x1C0011,
                        "250Kbps" => 0x1C0008,
                        "500Kbps" => 0x060007,
                        "800Kbps" => 0x060004,
                        "1000Kbps" => 0x060003,
                        _ => throw new ArgumentException($"未定义的波特率: {_baudRate}"),
                    };

                    // 指针分配和写入逻辑
                    IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(baudRate));
                    Marshal.WriteInt32(ptr, baudRate);
                    CONTROLCANHelper.VCI_SetReference(_devType, _devIndex, _channel, 0, ptr);
                }

                // 常用型号波特率设置
                else
                {
                    switch (_baudRate)
                    {
                        case "5Kbps":
                            VCIINITCONFIG.Timing0 = 0xBF;
                            VCIINITCONFIG.Timing1 = 0xFF;
                            break;
                        case "10Kbps":
                            VCIINITCONFIG.Timing0 = 0x31;
                            VCIINITCONFIG.Timing1 = 0x1C;
                            break;
                        case "20Kbps":
                            VCIINITCONFIG.Timing0 = 0x18;
                            VCIINITCONFIG.Timing1 = 0x1C;
                            break;
                        case "40Kbps":
                            VCIINITCONFIG.Timing0 = 0x87;
                            VCIINITCONFIG.Timing1 = 0xFF;
                            break;
                        case "50Kbps":
                            VCIINITCONFIG.Timing0 = 0x09;
                            VCIINITCONFIG.Timing1 = 0x1C;
                            break;
                        case "80Kbps":
                            VCIINITCONFIG.Timing0 = 0x83;
                            VCIINITCONFIG.Timing1 = 0xFF;
                            break;
                        case "100Kbps":
                            VCIINITCONFIG.Timing0 = 0x04;
                            VCIINITCONFIG.Timing1 = 0x1C;
                            break;
                        case "125Kbps":
                            VCIINITCONFIG.Timing0 = 0x03;
                            VCIINITCONFIG.Timing1 = 0x1C;
                            break;
                        case "200Kbps":
                            VCIINITCONFIG.Timing0 = 0x81;
                            VCIINITCONFIG.Timing1 = 0xFA;
                            break;
                        case "250Kbps":
                            VCIINITCONFIG.Timing0 = 0x01;
                            VCIINITCONFIG.Timing1 = 0x1C;
                            break;
                        case "400Kbps":
                            VCIINITCONFIG.Timing0 = 0x80;
                            VCIINITCONFIG.Timing1 = 0xFA;
                            break;
                        case "500Kbps":
                            VCIINITCONFIG.Timing0 = 0x00;
                            VCIINITCONFIG.Timing1 = 0x1C;
                            break;
                        case "666Kbps":
                            VCIINITCONFIG.Timing0 = 0x80;
                            VCIINITCONFIG.Timing1 = 0xB6;
                            break;
                        case "800Kbps":
                            VCIINITCONFIG.Timing0 = 0x00;
                            VCIINITCONFIG.Timing1 = 0x16;
                            break;
                        case "1000Kbps":
                            VCIINITCONFIG.Timing0 = 0x00;
                            VCIINITCONFIG.Timing1 = 0x14;
                            break;
                    }
                }

                if (CONTROLCANHelper.VCI_InitCAN(_devType, _devIndex, _channel, ref VCIINITCONFIG) != CONTROLCANSTATUS.STATUS_OK)
                {
                    //MessageBoxHelper.Warning("初始化CAN设备失败!", "警告", null, ButtonType.OK);

                    CONTROLCANHelper.VCI_CloseDevice(_devType, _devIndex);
                    throw new Exception("初始化CAN设备失败!");
                    return;
                }

                if (CONTROLCANHelper.VCI_StartCAN(_devType, _devIndex, _channel) != CONTROLCANSTATUS.STATUS_OK)
                {
                    //MessageBoxHelper.Warning("启动CAN设备失败!", "警告", null, ButtonType.OK);
                    CONTROLCANHelper.VCI_CloseDevice(_devType, _devIndex);
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
            gSendMsgBuf = new VCI_CAN_OBJ[SEND_MSG_BUF_MAX];
            gSendMsgBufHead = 0;
            gSendMsgBufTail = 0;

            CAN_OBJ co = new CAN_OBJ();
            co.SendType = 0;
            co.DataLen = 8;
            co.Data = data;
            co.ID = BitConverter.ToUInt32(canid, 0);

            gSendMsgBuf[gSendMsgBufHead].ID = co.ID;
            gSendMsgBuf[gSendMsgBufHead].Data = co.Data;
            gSendMsgBuf[gSendMsgBufHead].DataLen = co.DataLen;
            gSendMsgBuf[gSendMsgBufHead].ExternFlag = 1;//扩展帧
            gSendMsgBuf[gSendMsgBufHead].RemoteFlag = 0;//数据帧
            gSendMsgBufHead++;
            if (gSendMsgBufHead >= SEND_MSG_BUF_MAX)
            {
                gSendMsgBufHead = 0;
            }

            VCI_CAN_OBJ[] coMsg = new VCI_CAN_OBJ[2];

            if (gSendMsgBufHead != gSendMsgBufTail)
            {
                coMsg[0] = gSendMsgBuf[gSendMsgBufTail];
                coMsg[1] = gSendMsgBuf[gSendMsgBufTail];
                gSendMsgBufTail++;

                if (gSendMsgBufTail >= SEND_MSG_BUF_MAX)
                {
                    gSendMsgBufTail = 0;
                }
                LogAction?.Invoke(1, HexDataHelper.GetDebugByteString(data, "Send：0x" + co.ID.ToString("X")));
                if (CONTROLCANHelper.VCI_Transmit(_devType, _devIndex, _channel, coMsg, 1) == CONTROLCANSTATUS.STATUS_OK)
                {
                    VCI_ERR_INFO err_info = new VCI_ERR_INFO();
                    var v = CONTROLCANHelper.VCI_ReadErrInfo(_devType, _devIndex, _channel, ref err_info) == CONTROLCANSTATUS.STATUS_OK;


                    if (err_info.ErrCode == 0x00)//成功
                    {
                        Debug.WriteLine($"{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")} 发送数据   帧ID:{co.ID.ToString("X8")}");
                        return true;
                    }
                    else if (err_info.ErrCode == 0x400)
                    {
                        Debug.WriteLine($"{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}-->发送失败");
                    }
                }
            }

            return false;


        }

        //报警参数无法读取
        //public override void Receive()
        //{
        //    ReceivecEventHandler += EnqueueTask;
        //    ReceivecEventHandler += EnqueueTask_MLQ;

        //    Task.Run(() =>
        //    {
        //        while (true)
        //        {
        //            // 适当的等待时间，避免高频率轮询
        //            Thread.Sleep(10);  // 等待10毫秒
        //            uint receiveNum = CONTROLCANHelper.VCI_GetReceiveNum(_devType, _devIndex, _channel);
        //            if (receiveNum <= 0)
        //            {
        //                // 适当的等待时间，避免高频率轮询
        //                Thread.Sleep(100);  // 等待100毫秒
        //                continue;
        //            }

        //            if (receiveNum > 1)
        //                receiveNum = 1;

        //            IntPtr pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VCI_CAN_OBJ)) * (Int32)receiveNum);
        //            try
        //            {
        //                uint len = CONTROLCANHelper.VCI_Receive(_devType, _devIndex, _channel, pt, receiveNum, -1);
        //                if (len <= 0)
        //                {
        //                    VCI_ERR_INFO errInfo = new VCI_ERR_INFO();
        //                    CONTROLCANHelper.VCI_ReadErrInfo(_devType, _devIndex, _channel, ref errInfo);
        //                    continue; // 如果没有数据，继续下一次循环
        //                }

        //                for (int i = 0; i < receiveNum; i++)
        //                {
        //                    VCI_CAN_OBJ coMsg = (VCI_CAN_OBJ)Marshal.PtrToStructure((IntPtr)((UInt64)pt +
        //                        (UInt64)i * (UInt64)Marshal.SizeOf(typeof(VCI_CAN_OBJ))), typeof(VCI_CAN_OBJ));
        //                    //lock (gRecMsgBuf)
        //                    //                    //{
        //                    //                    //    gRecMsgBuf[gRecMsgBufHead].ID = coMsg.ID;
        //                    //                    //    gRecMsgBuf[gRecMsgBufHead].Data = coMsg.Data;
        //                    //                    //    gRecMsgBuf[gRecMsgBufHead].DataLen = coMsg.DataLen;
        //                    //                    //    gRecMsgBuf[gRecMsgBufHead].ExternFlag = coMsg.ExternFlag;
        //                    //                    //    gRecMsgBuf[gRecMsgBufHead].RemoteFlag = coMsg.RemoteFlag;
        //                    //                    //    gRecMsgBuf[gRecMsgBufHead].TimeStamp = coMsg.TimeStamp;
        //                    //                    //    gRecMsgBuf[gRecMsgBufHead].Reserved = coMsg.Reserved;
        //                    //                    //    gRecMsgBuf[gRecMsgBufHead].TimeFlag = coMsg.TimeFlag;
        //                    //                    //    gRecMsgBufHead += 1;
        //                    //                    //    if (gRecMsgBufHead >= 2)
        //                    //                    //    {
        //                    //                    //        gRecMsgBufHead = 0;
        //                    //                    //    }
        //                    //                    //}
        //                    lock (gRecMsgBuf)
        //                    {
        //                        gRecMsgBuf[gRecMsgBufHead] = coMsg;
        //                        gRecMsgBufHead = (gRecMsgBufHead + 1) % 2; // 使用取模运算简化代码
        //                    }

        //                    // 筛选并处理数据逻辑
        //                    Task.Run(() =>
        //                    {
        //                        foreach (Protocols item in Protocols.protocols)
        //                        {
        //                            uint revId = coMsg.ID | 0xff;
        //                            if (revId == item.Id)
        //                            {
        //                                ReceivecEventHandler(coMsg);
        //                                break;
        //                            }
        //                        }
        //                    });
        //                }
        //            }
        //            finally
        //            {
        //                Marshal.FreeHGlobal(pt);//内存释放
        //            }
        //        }
        //    });

        //}

        //报警参数无法读取
        //public override void Receive()
        //{
        //    ReceivecEventHandler += EnqueueTask;
        //    ReceivecEventHandler += EnqueueTask_MLQ;

        //    Task.Run(() =>
        //    {
        //        IntPtr pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VCI_CAN_OBJ)) * 1);
        //        try
        //        {
        //            while (true)
        //            {
        //                Thread.Sleep(10); // 避免高频率轮询

        //                uint receiveNum = CONTROLCANHelper.VCI_GetReceiveNum(_devType, _devIndex, _channel);
        //                if (receiveNum <= 0)
        //                {
        //                    Thread.Sleep(100); // 等待时间可以调整，避免无数据时频繁轮询
        //                    continue;
        //                }

        //                uint len = CONTROLCANHelper.VCI_Receive(_devType, _devIndex, _channel, pt, 1, -1);
        //                if (len <= 0)
        //                {
        //                    VCI_ERR_INFO errInfo = new VCI_ERR_INFO();
        //                    CONTROLCANHelper.VCI_ReadErrInfo(_devType, _devIndex, _channel, ref errInfo);
        //                    continue;
        //                }

        //                VCI_CAN_OBJ coMsg = (VCI_CAN_OBJ)Marshal.PtrToStructure(pt, typeof(VCI_CAN_OBJ));

        //                lock (gRecMsgBuf)
        //                {
        //                    gRecMsgBuf[gRecMsgBufHead] = coMsg;
        //                    gRecMsgBufHead = (gRecMsgBufHead + 1) % 2;
        //                }

        //                // 筛选并处理数据逻辑
        //                foreach (Protocols item in Protocols.protocols)
        //                {
        //                    uint revId = coMsg.ID | 0xff;
        //                    if (revId == item.Id)
        //                    {
        //                        Task.Run(() => ReceivecEventHandler(coMsg));
        //                        //ReceivecEventHandler(coMsg);
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        finally
        //        {
        //            Marshal.FreeHGlobal(pt); // 释放内存
        //        }
        //    });
        //}

        public override void Receive()
        {
            ReceivecEventHandler += EnqueueTask;
            ReceivecEventHandler += EnqueueTask_MLQ;

            Task.Run(() =>
            {
                while (true)
                {
                    VCI_CAN_OBJ coMsg = new VCI_CAN_OBJ();

                    if (CONTROLCANHelper.VCI_Receive(_devType, _devIndex, _channel, ref coMsg, 1, 1) == CONTROLCANSTATUS.STATUS_OK)
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

                        //// 更新缓冲区头指针，并在必要时进行环绕
                        //gRecMsgBufHead = (gRecMsgBufHead + 1) % REC_MSG_BUF_MAX;

                        if (gRecMsgBufHead >= REC_MSG_BUF_MAX)
                        {
                            gRecMsgBufHead = 0;
                        }

                        //进入队列前，先进行筛选（集合内的ID可加入至队列，否则过滤掉）
                        foreach (Protocols item in Protocols.protocols)
                        {
                            uint revId = coMsg.ID | 0xff;
                            uint devId = AnalysisID_EVBCM(coMsg.ID);
                            if (revId == item.Id)
                            {
                                ReceivecEventHandler(coMsg);
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

            VCI_ERR_INFO err_info = new VCI_ERR_INFO();

            if (CONTROLCANHelper.VCI_ReadErrInfo(_devType, _devIndex, _channel, ref err_info) == CONTROLCANSTATUS.STATUS_OK)
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
                        if (CONTROLCANHelper.VCI_ResetCAN(_devType, _devIndex, _channel) == CONTROLCANSTATUS.STATUS_OK)
                        {
                            Can_error_count = 0;
                            CONTROLCANHelper.VCI_StartCAN(_devType, _devIndex, _channel);
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

        private void EnqueueTask(VCI_CAN_OBJ CANOBJ)
        {
            lock (_locker)
            {
                //LogAction?.Invoke(1, HexDataHelper.GetDebugByteString(CANOBJ.Data, "Recv：0x" + CANOBJ.ID.ToString("X")));
                _task.Enqueue(CANOBJ);
                _wh.Set();
            }
        }

        private void EnqueueTask_MLQ(VCI_CAN_OBJ coMsg)
        {
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
        }
    }
}
