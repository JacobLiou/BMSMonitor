using Sofar.ProtocolLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Sofar.BMSLib
{
    public class BaseCanHelper
    {
        public BaseCanHelper() { }
       
        ///*创建一个更新收发数据显示的线程*/
       // public readonly static object _locker = new object();
        //public  static Queue<CAN_OBJ> _task = new Queue<CAN_OBJ>();
        //public ConcurrentDictionary<int, Byte[]> Devices = new ConcurrentDictionary<int, Byte[]>();

        public virtual bool IsConnection { get; set; }
        public virtual string CommunicationType { get; set; }

        public virtual void Connect()
        {
           
        }
        public virtual  bool Send(Byte[] data, byte[] canid)
        {
            return false;
        }

        public virtual void Receive()
        {
           
        }

        public virtual string ReadError()
        {
           return "";
        }

    }
}

