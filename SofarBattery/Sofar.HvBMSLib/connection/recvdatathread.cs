using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using ZLGCAN;



namespace ZLGCANDemo
{
    //接收数据线程类
    class recvdatathread
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void RecvCANDataEventHandler(ZCAN_Receive_Data[] data, uint len);//CAN数据接收事件委托

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void RecvFDDataEventHandler(ZCAN_ReceiveFD_Data[] data, uint len);//CANFD数据接收事件委托

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void RecvDataEventHandler(ZCANDataObj[] data, uint len);//CANFD数据接收事件委托

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void RecvLINDataEventHandler(ZCAN_LIN_MSG[] data, uint len);//CANFD数据接收事件委托


        const int TYPE_CAN = 0;
        const int TYPE_CANFD = 1;

        bool m_bStart;
        IntPtr lin_channel_handle_;
        IntPtr channel_handle_;
        IntPtr device_handle_;
        byte merge_ =0;//初始不开启合并
        Thread recv_thread_;
        static object locker = new object();
        public  RecvCANDataEventHandler OnRecvCANDataEvent;
        public  RecvFDDataEventHandler OnRecvFDDataEvent;
        public  RecvDataEventHandler OnRecvDataEvent;
        public  RecvLINDataEventHandler OnRecvLINDataEvent;
        public recvdatathread()
        {
        }

        public event RecvCANDataEventHandler RecvCANData
        {
            add { OnRecvCANDataEvent += new RecvCANDataEventHandler(value); }
            remove { OnRecvCANDataEvent -= new RecvCANDataEventHandler(value); }
        }

        public event RecvFDDataEventHandler RecvFDData
        {
            add { OnRecvFDDataEvent += new RecvFDDataEventHandler(value); }
            remove { OnRecvFDDataEvent -= new RecvFDDataEventHandler(value); }
        }

        public event RecvDataEventHandler RecvData
        {
            add { OnRecvDataEvent += new RecvDataEventHandler(value); }
            remove { OnRecvDataEvent -= new RecvDataEventHandler(value); }
        }


        public event RecvLINDataEventHandler RecvLINData
        {
            add { OnRecvLINDataEvent += new RecvLINDataEventHandler(value); }
            remove { OnRecvLINDataEvent -= new RecvLINDataEventHandler(value); }
        }

        public void setStart(bool start)
        {
            m_bStart = start;
            if (start)
            {
                recv_thread_ = new Thread(RecvDataFunc);
                recv_thread_.IsBackground = true;
                recv_thread_.Start();
            }
            else
            {
                recv_thread_.Join();
                recv_thread_ = null;
            }
        }

        public void setChannelHandle(IntPtr channel_handle)
        {
            lock(locker)
            {
                channel_handle_ = channel_handle;
            }
        }

        public void setDeviceHandle(IntPtr device_handle)
        {
            lock (locker)
            {
                device_handle_ = device_handle;
            }
        }


        public void setMerge_flag(Byte merge)
        {
            lock (locker)
            {
                merge_ = merge;
            }
        }


        public void setLINChannelHandle(IntPtr lin_channel_handle)
        {
            lock (locker)
            {
                lin_channel_handle_ = lin_channel_handle;
            }
        }


        //数据接收函数
        protected void RecvDataFunc()
        {
            ZCAN_Receive_Data[] can_data = new ZCAN_Receive_Data[10000];
            ZCAN_ReceiveFD_Data[] canfd_data = new ZCAN_ReceiveFD_Data[10000];
            ZCANDataObj[] data_obj = new ZCANDataObj[10000];
            ZCAN_LIN_MSG[] lin_data = new ZCAN_LIN_MSG[10000];
            uint len=0;
            while (m_bStart)
            {
                lock (locker)
                {
                    if (merge_!=1)
                    { //分开接收

                    len = Method.ZCAN_GetReceiveNum(channel_handle_, TYPE_CAN);

                    if (len > 0)
                    {
                        int size = Marshal.SizeOf(typeof(ZCAN_Receive_Data));
                        IntPtr ptr = Marshal.AllocHGlobal((int)100 * size);
                        len = Method.ZCAN_Receive(channel_handle_, ptr, 100, 50);
                        for (int i = 0; i < len; ++i)
                        {
                            can_data[i] = (ZCAN_Receive_Data)Marshal.PtrToStructure(
                                (IntPtr)((Int64)ptr+i*size), typeof(ZCAN_Receive_Data));
                        }
                        OnRecvCANDataEvent(can_data, len);
                        Marshal.FreeHGlobal(ptr);
                    }

                    len = Method.ZCAN_GetReceiveNum(channel_handle_, TYPE_CANFD);
                    if (len > 0)
                    {
                        int size = Marshal.SizeOf(typeof(ZCAN_ReceiveFD_Data));
                        IntPtr ptr = Marshal.AllocHGlobal((int)100 * size);
                        len = Method.ZCAN_ReceiveFD(channel_handle_, ptr, 100, 50);
                        for (int i = 0; i < len; ++i)
                        {
                            canfd_data[i] = (ZCAN_ReceiveFD_Data)Marshal.PtrToStructure(
                                (IntPtr)((Int64)ptr+i*size), typeof(ZCAN_ReceiveFD_Data));
                        }
                        OnRecvFDDataEvent(canfd_data, len);
                        Marshal.FreeHGlobal(ptr);
                    }

                      {
                        int size = Marshal.SizeOf(typeof(ZCAN_LIN_MSG));
                        IntPtr ptr = Marshal.AllocHGlobal(50 * size);
                        len = Method.ZCAN_ReceiveLIN(lin_channel_handle_, ptr, 50, 0);
                        if (len > 0)
                        {
                            for (int i = 0; i < len; ++i)
                            {
                                lin_data[i] = (ZCAN_LIN_MSG)Marshal.PtrToStructure(
                                    (IntPtr)((Int64)ptr + i * size), typeof(ZCAN_LIN_MSG));
                            }
                            OnRecvLINDataEvent(lin_data, len);
                           
                        }
                        Marshal.FreeHGlobal(ptr); 
                      }
                    }
                    else
                    { //合并接收
                        len = Method.ZCAN_GetReceiveNum(channel_handle_, 2); //合并接收类型type为2
                        if (len > 0)
                        {
                            int size = Marshal.SizeOf(typeof(ZCANDataObj));
                            IntPtr ptr = Marshal.AllocHGlobal((int)100 * size);
                            len = Method.ZCAN_ReceiveData(device_handle_, ptr, 100, 50);         //传设备的句柄
                            for (int i = 0; i < len; ++i)
                            {
                                data_obj[i] = (ZCANDataObj)Marshal.PtrToStructure(
                                    (IntPtr)((Int64)ptr + i * size), typeof(ZCANDataObj));
                            }
                            OnRecvDataEvent(data_obj, len);
                            Marshal.FreeHGlobal(ptr);    
                         }
                        
                     }
            }


                Thread.Sleep(1);
            }
        }
    }
}
