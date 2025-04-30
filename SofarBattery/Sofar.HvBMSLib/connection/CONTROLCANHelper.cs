using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Sofar.ProtocolLib
{
    /////////////////////////////////////////////////////
    //1.ZLGCAN系列接口卡信息的数据类型。
    [StructLayout(LayoutKind.Sequential)]
    public struct VCI_BOARD_INFO
    {
        public UInt16 hw_Version;
        public UInt16 fw_Version;
        public UInt16 dr_Version;
        public UInt16 in_Version;
        public UInt16 irq_Num;
        public byte can_Num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] str_Serial_Num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public byte[] str_hw_Type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Reserved;
    }


    /////////////////////////////////////////////////////
    //2.定义CAN信息帧的数据类型。
    [StructLayout(LayoutKind.Sequential)]
    public struct VCI_CAN_OBJ
    {
        public uint ID;
        public uint TimeStamp;
        public byte TimeFlag;
        public byte SendType;
        public byte RemoteFlag;//是否是远程帧
        public byte ExternFlag;//是否是扩展帧
        public byte DataLen;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Data;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Reserved;
    }

    /////////////////////////////////////////////////////
    //3.定义CAN控制器状态的数据类型。
    [StructLayout(LayoutKind.Sequential)]
    public struct VCI_CAN_STATUS
    {
        public byte ErrInterrupt;
        public byte regMode;
        public byte regStatus;
        public byte regALCapture;
        public byte regECCapture;
        public byte regEWLimit;
        public byte regRECounter;
        public byte regTECounter;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Reserved;
    }

    /////////////////////////////////////////////////////
    //4.定义错误信息的数据类型。
    [StructLayout(LayoutKind.Sequential)]
    public struct VCI_ERR_INFO
    {
        public UInt32 ErrCode;
        public byte Passive_ErrData1;
        public byte Passive_ErrData2;
        public byte Passive_ErrData3;
        public byte ArLost_ErrData;
    }

    /////////////////////////////////////////////////////
    //5.定义初始化CAN的数据类型
    [StructLayout(LayoutKind.Sequential)]
    public struct VCI_INIT_CONFIG
    {
        public UInt32 AccCode;
        public UInt32 AccMask;
        public UInt32 Reserved;
        public byte Filter;
        public byte Timing0;
        public byte Timing1;
        public byte Mode;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CHGDESIPANDPORT
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] szpwd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] szdesip;
        public Int32 desport;

        public void Init()
        {
            szpwd = new byte[10];
            szdesip = new byte[20];
        }
    }

    public struct CONTROLCANHelper
    {
      
        //DeviceType—设备类型号 DeviceInd—设备索引号 Reserved—保留参数，通常为0。 CANInd—第几路CAN。即对应卡的CAN通道号，CAN0为0，CAN1为1，以此类推。

        [DllImport("controlcan.dll", EntryPoint = "VCI_OpenDevice")]
        public static extern CONTROLCANSTATUS VCI_OpenDevice(
            UInt32 DeviceType, 
            UInt32 DeviceInd,
            UInt32 Reserved);

        [DllImport("controlcan.dll", EntryPoint = "VCI_CloseDevice")]
        public static extern CONTROLCANSTATUS VCI_CloseDevice(
            UInt32 DeviceType,
            UInt32 DeviceInd);

        [DllImport("controlcan.dll", EntryPoint = "VCI_InitCAN")]
        public static extern CONTROLCANSTATUS VCI_InitCAN(
            UInt32 DeviceType, 
            UInt32 DeviceInd, 
            UInt32 CANInd,
            ref VCI_INIT_CONFIG pInitConfig);

        [DllImport("controlcan.dll", EntryPoint = "VCI_ReadBoardInfo")]
        public static extern CONTROLCANSTATUS VCI_ReadBoardInfo(
            UInt32 DeviceType, 
            UInt32 DeviceInd, 
            ref VCI_BOARD_INFO pInfo);

        [DllImport("controlcan.dll", EntryPoint = "VCI_ReadErrInfo")]
        public static extern CONTROLCANSTATUS VCI_ReadErrInfo(
            UInt32 DeviceType, 
            UInt32 DeviceInd,
            UInt32 CANInd, 
            ref VCI_ERR_INFO pErrInfo);

        [DllImport("controlcan.dll", EntryPoint = "VCI_ReadCANStatus")]
        public static extern CONTROLCANSTATUS VCI_ReadCANStatus(
            UInt32 DeviceType, 
            UInt32 DeviceInd, 
            UInt32 CANInd,
            ref VCI_CAN_STATUS pCANStatus);

        [DllImport("controlcan.dll", EntryPoint = "VCI_GetReference")]
        public static extern CONTROLCANSTATUS VCI_GetReference(
            UInt32 DeviceType, 
            UInt32 DeviceInd, 
            UInt32 CANInd,
            UInt32 RefType, 
            ref byte pData);

        [DllImport("controlcan.dll", EntryPoint = "VCI_SetReference")]
        /// <summary>
        /// 设置特殊设备的相应参数，主要处理不同设备的特定操作。
        /// </summary>
        /// <param name="DeviceType">设备类型</param>
        /// <param name="DeviceInd">设备索引号</param>
        /// <param name="CANInd">第几路 CAN</param>
        /// <param name="RefType">参数类型</param>
        /// <param name="pData">用来存储参数有关数据缓冲区地址首指针</param>
        public static extern CONTROLCANSTATUS VCI_SetReference(
            UInt32 DeviceType, 
            UInt32 DeviceInd,
            UInt32 CANInd,
            UInt32 RefType,
            IntPtr pData);

        [DllImport("controlcan.dll", EntryPoint = "VCI_GetReceiveNum")]
        public static extern UInt32 VCI_GetReceiveNum(
            UInt32 DeviceType, 
            UInt32 DeviceInd,
            UInt32 CANInd);

        [DllImport("controlcan.dll", EntryPoint = "VCI_ClearBuffer")]
        public static extern CONTROLCANSTATUS VCI_ClearBuffer(
            UInt32 DeviceType, 
            UInt32 DeviceInd, 
            UInt32 CANInd);

        [DllImport("controlcan.dll", EntryPoint = "VCI_StartCAN")]
        public static extern CONTROLCANSTATUS VCI_StartCAN(
            UInt32 DeviceType,
            UInt32 DeviceInd, 
            UInt32 CANInd);

        [DllImport("controlcan.dll", EntryPoint = "VCI_ResetCAN")]
        public static extern CONTROLCANSTATUS VCI_ResetCAN(
            UInt32 DeviceType, 
            UInt32 DeviceInd, 
            UInt32 CANInd);

        [DllImport("controlcan.dll", EntryPoint = "VCI_Transmit")]
        public static extern CONTROLCANSTATUS VCI_Transmit(
            UInt32 DeviceType, 
            UInt32 DeviceInd,
            UInt32 CANInd, VCI_CAN_OBJ[] pSend,
            UInt32 Len);


        [DllImport("controlcan.dll", CharSet = CharSet.Ansi, EntryPoint = "VCI_Receive")]
        public static extern UInt32 VCI_Receive(UInt32 DeviceType,
            UInt32 DeviceInd,
            UInt32 CANInd,
            IntPtr pReceive,
            UInt32 Len,
            Int32 WaitTime);

        [DllImport("controlcan.dll", EntryPoint = "VCI_Receive")]
        public static extern CONTROLCANSTATUS VCI_Receive(
            UInt32 DeviceType,
            UInt32 DeviceInd,
            UInt32 CANInd,
            ref VCI_CAN_OBJ pReceive,
            UInt32 Len,
            Int32 WaitTime);

    };


  

    // 函数调用返回状态值
    [Flags]
    public enum CONTROLCANSTATUS : uint
    {
        STATUS_OK = 1,
        STATUS_ERR = 0,
    };

    public enum REF
    {
        REFERENCE_BAUD = 1,
        REFERENCE_SET_TRANSMIT_TIMEOUT = 2,
        REFERENCE_ADD_FILTER = 3,
        REFERENCE_SET_FILTER = 4,
    };
    public enum ErrorType
    {
        //CAN错误码
        ERR_CAN_OVERFLOW = 0x0001,  //CAN控制器内部FIFO溢出
        ERR_CAN_ERRALARM = 0x0002,  //CAN控制器错误报警
        ERR_CAN_PASSIVE = 0x0004,   //CAN控制器消极错误
        ERR_CAN_LOSE = 0x0008,  //CAN控制器仲裁丢失
        ERR_CAN_BUSERR = 0x0010,    //CAN控制器总线错误

        //通用错误码
        ERR_DEVICEOPENED = 0x0100,  //设备已经打开
        ERR_DEVICEOPEN = 0x0200,    //打开设备错误
        ERR_DEVICENOTOPEN = 0x0400, //设备没有打开
        ERR_BUFFEROVERFLOW = 0x0800,    //缓冲区溢出
        ERR_DEVICENOTEXIST = 0x1000,    //此设备不存在
        ERR_LOADKERNELDLL = 0x2000, //装载动态库失败
        ERR_CMDFAILED = 0x4000, //执行命令失败错误码
        ERR_BUFFERCREATE = 0x8000   //内存不足
    };
}