using System;
using System.Runtime.InteropServices;

namespace ZLGCAN
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ZCAN
    {
        public uint acc_code;
        public uint acc_mask;
        public uint reserved;
        public byte filter;
        public byte timing0;
        public byte timing1;
        public byte mode;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct CANFD
    {
        public uint acc_code;
        public uint acc_mask;
        public uint abit_timing;
        public uint dbit_timing;
        public uint brp;
        public byte filter;
        public byte mode;
        public UInt16 pad;
        public uint reserved;

    };



    [StructLayout(LayoutKind.Sequential)]
    public struct ZCAN_DEVICE_INFO
    {
        public ushort hw_Version;
        public ushort fw_Version;
        public ushort dr_Version;
        public ushort in_Version;
        public ushort irq_Num;
        public byte can_Num;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] str_Serial_Num;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public byte[] str_hw_Type;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] reserved;
    } ;





    [StructLayout(LayoutKind.Sequential)]
    public struct can_frame
    {
        public uint can_id;  /* 32 bit MAKE_CAN_ID + EFF/RTR/ERR flags */
        public byte can_dlc; /* frame payload length in byte (0 .. CAN_MAX_DLEN) */
        public byte __pad;   /* padding */
        public byte __res0;  /* reserved / padding */
        public byte __res1;  /* reserved / padding */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] data/* __attribute__((aligned(8)))*/;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct canfd_frame
    {
        public uint can_id;  /* 32 bit MAKE_CAN_ID + EFF/RTR/ERR flags */
        public byte len;     /* frame payload length in byte */
        public byte flags;   /* additional flags for CAN FD,i.e error code */
        public byte __res0;  /* reserved / padding */
        public byte __res1;  /* reserved / padding */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] data/* __attribute__((aligned(8)))*/;
    };

    [StructLayout(LayoutKind.Explicit)]
    public struct ZCAN_CHANNEL_INIT_CONFIG
    {
        [FieldOffset(0)]
        public uint can_type; //type:TYPE_CAN TYPE_CANFD

        [FieldOffset(4)]
        public ZCAN can;

        [FieldOffset(4)]
        public CANFD canfd;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ZCAN_Transmit_Data
    {
        public can_frame frame;
        public uint transmit_type;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ZCAN_Receive_Data
    {
        public can_frame frame;
        public UInt64 timestamp;//us
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ZCAN_TransmitFD_Data
    {
        public canfd_frame frame;
        public uint transmit_type;
    };




    [StructLayout(LayoutKind.Sequential)]
    public struct ZCAN_AUTO_TRANSMIT_OBJ     //CAN定时发送帧结构体
    {
        public ushort enable;           //0-禁用，1-使能  
        public ushort index;            //定时报文索引  
        public uint interval;                  //定时周期
        public ZCAN_Transmit_Data  obj;
    };


    [StructLayout(LayoutKind.Sequential)]
    public struct ZCANFD_AUTO_TRANSMIT_OBJ    //CANFD定时发送帧结构体
    {
        public ushort enable;           //0-禁用，1-使能  
        public ushort index;            //定时报文索引  
        public uint interval;                  //定时周期
        public ZCAN_TransmitFD_Data obj;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ZCANLINEventData
    {
        public UInt64 timeStamp;
        public byte type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public byte[] reserved;

    }

    public struct DeviceInfo
    {
        public uint device_type;  //设备类型
        public uint channel_count;//设备的通道个数
        public DeviceInfo(uint type, uint count)
        {
            device_type = type;
            channel_count = count;
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ZCAN_ReceiveFD_Data
    {
        public canfd_frame frame;
        public UInt64 timestamp;//us
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ZCAN_CHANNEL_ERROR_INFO
    {
        public uint error_code;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] passive_ErrData;
        public byte arLost_ErrData;
    } ;

    //for zlg cloud
    [StructLayout(LayoutKind.Sequential)]
    public struct ZCLOUD_CHNINFO
    {
        public byte enable;
        public byte type;
        public byte isUpload;
        public byte isDownload;
    };



    //for zlg cloud
    [StructLayout(LayoutKind.Sequential)]
    public struct ZCLOUD_DEVINFO
    {
        public int devIndex;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] id;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] owner;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] model;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public char[] fwVer;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public char[] hwVer;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] serial;
        public int status;             // 0:online, 1:offline
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte bGpsUploads;
        public byte channelCnt;   // each channel enable can upload
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public ZCLOUD_CHNINFO[] channels;

    };

    //[StructLayout(LayoutKind.Sequential)]
    //public struct ZCLOUD_DEV_GROUP_INFO
    //{
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    //    public char[] groupName;
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    //    public char[] desc;
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    //    public char[] groupId;
    //    //public ZCLOUD_DEVINFO *pDevices;
    //    public IntPtr pDevices;
    //    public uint devSize;
    //};

    [StructLayout(LayoutKind.Sequential)]
    public struct ZCLOUD_USER_DATA
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] username;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] mobile;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public char[] dllVer;

        public uint devCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public ZCLOUD_DEVINFO[] devices;

    };


    //LIN_init_config
    [StructLayout(LayoutKind.Sequential)]
    public struct ZCAN_LIN_INIT_CONFIG
    {
        public byte linMode;                                                //0-slave,1-master
        public byte chkSumMode;                                             //1-经典校验，2-增强校验 3-自动(对应eZLINChkSumMode的模式)
        public UInt16 reserved;  
        public uint libBaud;                                                //波特率，取值1000~20000

    };

    //ZCAN_LIN_PUBLISH_CFG，注册从站响应报文
    [StructLayout(LayoutKind.Sequential)]
    public struct ZCAN_LIN_PUBLISH_CFG
    {
        public byte ID;                                                     //受保护的ID（ID取值范围为0-63）
        public byte datelen;                                                //范围1~8
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]                //数据段内容
        public byte[] data;
        public byte chkSumMode;                                              //校验方式：0-默认，启动时配置  1-经典校验  2-增强校验(对应eZLINChkSumMode的模式)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] reserved;
    };

    /////////////////////LIN收发报文结构体////////////////
        //LIN收发的ID
    [StructLayout(LayoutKind.Sequential,Pack=1)]
    public struct PID
    {
        public byte rawVal;

    }

        //LIN收发的数据段部分
    [StructLayout(LayoutKind.Sequential,Pack=1)]
    public struct RxData
    {
       public UInt64 timeStamp;
       public byte datalen;
       public byte dir;
       public byte  chkSum;
       [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
       public byte[]  reserved;
       [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
       public byte[]  data;

    }



    //LIN收发的报文部分结构体
    [StructLayout(LayoutKind.Sequential,Pack=1)]
    public struct ZCANLINData
    {
       public PID  pid;
       public RxData rxData;
       [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
       public byte[]  reserved;

    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Reserved
    {

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 46)]
        public byte[] reserved;

    }


    //ZCAN_LIN_MSG，发送/接收LIN的结构体
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ZCAN_LIN_MSG
    {
        
        public byte chnl;                                                     //数据通道


        public byte dataType;                                                //0-LIN，1-ErrLIN

   
       
        public ZCANLINData zcanLINData;                                     //数据段内容，共40字节

        //FieldOffset(2)]
       // public ZCAN_AUTO_TRANSMIT_OBJ ZCAN_OBJ;

  
    };

    //////////////////合并发送/接收的结构体////////////////////////////////////

    public class flag
    {
        public const uint CANFD_FLAG = 0x1;              //1代表CANFD报文，0代表CAN报文
        public const uint TXDELAY_FLAG = 0x4;            //发送有效，队列发送标志位
        public const uint TRANSMIT_TYPE_FLAG = 0x0;      //正常发送
        public const uint TXECHOREQUEST_FLAG = 0x100;    //发送回显请求，发送有效
        public const uint TXECHOED_FLAG = 0x200;         //发送回显标志，接收有效
    
    }




    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ZCANCANFDData
    {
        public UInt64 timeStamp;
        public UInt32 flag;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] extraData;                                    //未使用  
        public canfd_frame frame;                                   //实际报文结构体
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ZCANDataObj
    {
        public byte dataType;                                       //1-can/canfd数据。
        public byte chnl;                                           //数据通道 
        public UInt16 flag;                                         //未使用
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] extraData;                                    //未使用  
        public ZCANCANFDData zcanCANFDData;                         //报文结构体
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] reserve;                                      //用于字节对齐 
    }




    public class Define
    {
        public const int TYPE_CAN = 0;
        public const int TYPE_CANFD = 1;
        public const int ZCAN_USBCAN1 = 3;
        public const int ZCAN_USBCAN2 = 4;
        public const int ZCAN_PCI9820I = 16;
        public const int ZCAN_CANETUDP = 12;
        public const int ZCAN_CANETTCP = 17;
        public const int ZCAN_CANWIFI_TCP = 25;
        public const int ZCAN_USBCAN_E_U = 20;
        public const int ZCAN_USBCAN_2E_U = 21;
        public const int ZCAN_USBCAN_4E_U = 31;
        public const int ZCAN_PCIECANFD_100U = 38;
        public const int ZCAN_PCIECANFD_200U = 39;
        public const int ZCAN_PCIECANFD_200U_EX = 62;
        public const int ZCAN_PCIECANFD_400U = 61;
        public const int ZCAN_USBCANFD_200U = 41;
        public const int ZCAN_USBCANFD_400U = 76;
        public const int ZCAN_USBCANFD_100U = 42;
        public const int ZCAN_USBCANFD_MINI = 43;
        public const int ZCAN_USBCANFD_800U = 59;
        public const int ZCAN_CLOUD = 46;
        public const int ZCAN_CANFDNET_200U_TCP = 48;
        public const int ZCAN_CANFDNET_200U_UDP = 49;
        public const int ZCAN_CANFDNET_400U_TCP = 52;
        public const int ZCAN_CANFDNET_400U_UDP = 53;
        public const int ZCAN_CANFDNET_800U_TCP = 57;
        public const int ZCAN_CANFDNET_800U_UDP = 58;
        public const int STATUS_ERR = 0;
        public const int STATUS_OK = 1;
    };

    public class Method
    {
        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr ZCAN_OpenDevice(uint device_type, uint device_index, uint reserved);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_CloseDevice(IntPtr device_handle);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        // pInitConfig -> ZCAN_CHANNEL_INIT_CONFIG
        public static extern IntPtr ZCAN_InitCAN(IntPtr device_handle, uint can_index, IntPtr pInitConfig);



        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        // pErrInfo -> ZCAN_CHANNEL_ERROR_INFO
        public static extern uint ZCAN_GetDeviceInf(IntPtr device_handle, IntPtr pDeInfo);           //////////////////////add

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_SetValue(IntPtr device_handle,string path  , byte[] value);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_SetValue(IntPtr device_handle, string path, IntPtr value);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr ZCAN_GetValue(IntPtr device_handle, string path);


        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_StartCAN(IntPtr channel_handle);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_ResetCAN(IntPtr channel_handle);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_ClearBuffer(IntPtr channel_handle);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        // pTransmit -> ZCAN_Transmit_Data
        public static extern uint ZCAN_Transmit(IntPtr channel_handle, IntPtr pTransmit, uint len);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        // pTransmit -> ZCAN_TransmitFD_Data
        public static extern uint ZCAN_TransmitFD(IntPtr channel_handle, IntPtr pTransmit, uint len);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        // pTransmit -> ZCAN_TransmitFD_Data
        public static extern uint ZCAN_TransmitData(IntPtr device_handle, IntPtr pTransmit, uint len);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_GetReceiveNum(IntPtr channel_handle, byte type);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_Receive(IntPtr channel_handle, IntPtr data, uint len, int wait_time = -1);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_ReceiveFD(IntPtr channel_handle, IntPtr data, uint len, int wait_time = -1);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_ReceiveData(IntPtr device_handle, IntPtr data, uint len, int wait_time = -1);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        // pErrInfo -> ZCAN_CHANNEL_ERROR_INFO
        public static extern uint ZCAN_ReadChannelErrInfo(IntPtr channel_handle, IntPtr pErrInfo);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetIProperty(IntPtr device_handle);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool ZCLOUD_IsConnected();

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void ZCLOUD_SetServerInfo(string httpAddr, ushort httpPort,
            string mqttAddr, ushort mqttPort);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCLOUD_ConnectServer(string username, string password);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCLOUD_DisconnectServer();

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr ZCLOUD_GetUserData(int updata);

        ///////////LIN相关接口函数////////

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr ZCAN_InitLIN(IntPtr device_handle, uint lin_index, IntPtr pLINinitConfig);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_StartLIN(IntPtr channel_handle);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_ResetLIN(IntPtr channel_handle);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_TransmitLIN(IntPtr channel_handle, IntPtr pTransmit, uint len);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_ReceiveLIN(IntPtr channel_handle, IntPtr data, uint len, int wait_time);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_SetLINPublish(IntPtr channel_handle, IntPtr data, uint nPublishCount);  //注册响应报文
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SetValueFunc(string path, byte[] value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetValueFunc(string path);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetPropertysFunc(string path, string value);

    public struct IProperty
    {
        public SetValueFunc SetValue;
        public GetValueFunc GetValue;
        public GetPropertysFunc GetPropertys;
    };
}
