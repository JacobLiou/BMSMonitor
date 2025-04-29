using PowerKit.Domain.Enums;
using Sofar.BMSLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZLGCAN;
using ZLGCANDemo;

namespace Sofar.HvBMSLib
{
    public class ZlgcanHelper : BaseCanHelper
    {
        private static ZlgcanHelper instance = null;
        public static ZlgcanHelper Instance()
        {

            if (instance == null)
            {
                lock (obj)
                {
                    if (instance == null)
                    {
                        instance = new ZlgcanHelper(BMSConfig.ConfigManager);
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
        public ZlgcanHelper(BMSConfigurationModel config)
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
        public override string CommunicationType { get; set; } = "Zlgcan";

        private int GetDeviceType(string deviceType)
        {
            return deviceType switch
            {
                "USBCAN-2E-U" => Define.ZCAN_USBCAN_2E_U,
                _ => throw new ArgumentException($"不支持的设备类型: {deviceType}")
            };
        }

        const int NULL = 0;
        IntPtr device_handle_;
        IntPtr channel_handle_;
        IntPtr lin_channel_handle;
        IProperty property_;
        recvdatathread recv_data_thread_;
        List<string> list_box_data_ = new List<string>();
        static object lock_obj = new object();

        bool m_bOpen = false;
        bool m_bStart = false;
        bool m_bCloud = false;
        bool netDevice =false;
        bool usbCanfd = false;
        bool pcieCanfd = false;

        const int CAN_MAX_DLEN = 8;

        
        public override void Connect()
        {
            if (!IsConnection)
            {
                device_handle_ = Method.ZCAN_OpenDevice(_devType, _devIndex, _channel);
                if (NULL == (int)device_handle_)
                {
                    throw new Exception("打开CAN设备失败!");
                    return;
                }

                ZCAN_CHANNEL_INIT_CONFIG config_ = new ZCAN_CHANNEL_INIT_CONFIG();
                if (!m_bCloud && !netDevice)
                {
                    config_.canfd.mode = 0;
                    if (usbCanfd)
                    {
                        config_.can_type = Define.TYPE_CANFD;
                        config_.canfd.mode = 0;
                    }
                    else if (pcieCanfd)
                    {
                        config_.can_type = Define.TYPE_CANFD;
                        config_.canfd.filter = 0;
                        config_.canfd.acc_code = 0;
                        config_.canfd.acc_mask = 0xFFFFFFFF;
                        config_.canfd.mode = 0;
                    }
                    else
                    {
                        config_.can_type = Define.TYPE_CAN;
                        config_.can.filter = 0;
                        config_.can.acc_code = 0;
                        config_.can.acc_mask = 0xFFFFFFFF;
                        config_.can.mode = 0;
                    }
                }
                IntPtr pConfig = Marshal.AllocHGlobal(Marshal.SizeOf(config_));
                Marshal.StructureToPtr(config_, pConfig, true);
                channel_handle_ = Method.ZCAN_InitCAN(device_handle_, _devIndex, pConfig);
                if (NULL == (int)channel_handle_)
                {
                    throw new Exception("初始化CAN设备失败!");
                    return;
                }
                if (Method.ZCAN_StartCAN(channel_handle_) != Define.STATUS_OK)
                {
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
            uint id = BitConverter.ToUInt32(canid, 0);
            string Sdata = BitConverter.ToString(data, 0);
            int frame_type_index = 1; //0 标准帧  1 扩展帧
            int protocol_index = 0; //0 can   1 canfd
            int send_type_index = 0;// 0 正常发送
            int canfd_exp_index = 0;//0 fd加速否
            uint result; //发送的帧数

            ZCAN_Transmit_Data can_data = new ZCAN_Transmit_Data();
            can_data.frame.can_id = MakeCanId(id, frame_type_index, 0, 0);
            can_data.frame.data = new byte[8];
            can_data.frame.can_dlc = (byte)SplitData(Sdata, ref can_data.frame.data, CAN_MAX_DLEN);
            can_data.transmit_type = (uint)send_type_index;
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(can_data));
            Marshal.StructureToPtr(can_data, ptr, true);
            LogAction?.Invoke(1, HexDataHelper.GetDebugByteString(data, "Send：0x" + id.ToString("X")));
            result = Method.ZCAN_Transmit(channel_handle_, ptr, 1);
            Marshal.FreeHGlobal(ptr);
            
            if (result != 1)
            {
                return false;
                AddErr();
            }

            return true;

        }

        public uint MakeCanId(uint id, int eff, int rtr, int err)//1:extend frame 0:standard frame
        {
            uint ueff = (uint)(!!(Convert.ToBoolean(eff)) ? 1 : 0);
            uint urtr = (uint)(!!(Convert.ToBoolean(rtr)) ? 1 : 0);
            uint uerr = (uint)(!!(Convert.ToBoolean(err)) ? 1 : 0);
            return id | ueff << 31 | urtr << 30 | uerr << 29;
        }
        private int SplitData(string data, ref byte[] transData, int maxLen)
        {
            string[] dataArray = data.Split(' ');
            for (int i = 0; (i < maxLen) && (i < dataArray.Length); i++)
            {
                transData[i] = Convert.ToByte(dataArray[i].Substring(0, 2), 16);
            }

            return dataArray.Length;
        }
        private void AddErr()
        {
            ZCAN_CHANNEL_ERROR_INFO pErrInfo = new ZCAN_CHANNEL_ERROR_INFO();
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(pErrInfo));
            //Marshal.StructureToPtr(pErrInfo, ptr, true);
            if (Method.ZCAN_ReadChannelErrInfo(channel_handle_, ptr) != Define.STATUS_OK)
            {
                throw new Exception("获取错误信息失败!");
            }
            pErrInfo = (ZCAN_CHANNEL_ERROR_INFO)Marshal.PtrToStructure(ptr, typeof(ZCAN_CHANNEL_ERROR_INFO));
            Marshal.FreeHGlobal(ptr);

            string errorInfo = String.Format("错误码：{0:D1}", pErrInfo.error_code);

        }
        
        public override void Receive()
        {
            
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
       
    }
}
