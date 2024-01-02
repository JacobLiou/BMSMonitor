using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SofarBMS.Helper;

namespace SofarBMS.Model
{
    public class EcanHelper
    {
        public const int REC_MSG_BUF_MAX = 0x2710;

        public static CAN_OBJ[] gRecMsgBuf = new CAN_OBJ[REC_MSG_BUF_MAX];
        public static uint gRecMsgBufHead = 0;
        public static uint gRecMsgBufTail = 0;

        public const int SEND_MSG_BUF_MAX = 0x2710;

        public static CAN_OBJ[] gSendMsgBuf;
        public static uint gSendMsgBufHead;
        public static uint gSendMsgBufTail;


        /*创建一个更新收发数据显示的线程*/
        public readonly static object _locker = new object();
        public static Queue<CAN_OBJ> _task = new Queue<CAN_OBJ>();
        public static EventWaitHandle _wh = new AutoResetEvent(false);
        public static List<Protocols> protocols = new List<Protocols>() {
            new Protocols(3,0x1020FFFF),new Protocols(3,0x1020E0FF),
            new Protocols(3,0x1003FFFF), new Protocols(3,0x1004FFFF),new Protocols(3,0x1005FFFF),new Protocols(3,0x1006FFFF),new Protocols(3,0x1007FFFF),new Protocols(3,0x1008FFFF),new Protocols(3,0x1009FFFF),new Protocols(3,0x100AFFFF),new Protocols(3,0x100BFFFF),new Protocols(3,0x100CFFFF),new Protocols(3,0x100DFFFF),new Protocols(3,0x100EFFFF),new Protocols(3,0x100FFFFF),new Protocols(3,0x1040FFFF),new Protocols(3,0x1041FFFF),new Protocols(3,0x1042FFFF)
            ,new Protocols(3,0x104EFFFF),new Protocols(3,0x104FFFFF)
            ,new Protocols(2,0x102FFFE0)
            ,new Protocols(3,0x1030FFFF),new Protocols(3,0x1031FFFF),new Protocols(3,0x1033FFFF),new Protocols(3,0x1034FFFF),new Protocols(3,0x1035FFFF),new Protocols(3,0x1036FFFF),new Protocols(3,0x1037FFFF),new Protocols(3,0x1038FFFF),new Protocols(3,0x1039FFFF),new Protocols(3,0x103AFFFF),new Protocols(3,0x103BFFFF),new Protocols(3,0x103CFFFF),new Protocols(3,0x103DFFFF),new Protocols(3,0x103EFFFF),new Protocols(3,0x103FFFFF),new Protocols(3,0x1050FFFF),new Protocols(3,0x1051FFFF),new Protocols(3,0x102DFFFF)
            ,new Protocols(3,0x1010E0FF),new Protocols(3,0x1011E0FF),new Protocols(3,0x1012E0FF),new Protocols(3,0x1013E0FF),new Protocols(3,0x1014E0FF),new Protocols(3,0x1015E0FF),new Protocols(3,0x1016E0FF),new Protocols(3,0x1017E0FF),new Protocols(3,0x1018E0FF),new Protocols(3,0x1019E0FF),new Protocols(3,0x101AE0FF),new Protocols(3,0x1021E0FF),new Protocols(3,0x1022E0FF),new Protocols(3,0x1023E0FF),new Protocols(3,0x1024E0FF),new Protocols(3,0x1025E0FF),new Protocols(3,0x1026E0FF),new Protocols(3,0x1027E0FF),new Protocols(3,0x1028E0FF),new Protocols(3,0x1029E0FF),new Protocols(3,0x102AE0FF),new Protocols(3,0x102EE0FF),new Protocols(3,0x102FE0FF)
            ,new Protocols(3,0x07FAE0FF),new Protocols(3,0x07FBE0FF),new Protocols(3,0x07FCE0FF),new Protocols(3,0x07FDE0FF),new Protocols(3,0x07FEE0FF),new Protocols(3,0x07FFE0FF)
            ,new Protocols(3,0x07FB41FF),new Protocols(3,0x07FC41FF),new Protocols(3,0x07FD41FF),new Protocols(3,0x07FE41FF),new Protocols(3,0x07FF41FF),new Protocols(3,0x07FF5FFF)
            ,new Protocols(3,0xB605FFF),new Protocols(3,0xB615FFF),new Protocols(3,0xB625FFF),new Protocols(3,0xB635FFF),new Protocols(3,0xB665FFF),new Protocols(3,0x0B70E0FF),new Protocols(3,0x0B71E0FF),new Protocols(3,0x0B72E0FF),new Protocols(3,0x0B73E0FF),new Protocols(3,0x0B74E0FF),new Protocols(3,0x0B75E0FF),new Protocols(3,0x0B76E0FF),new Protocols(3,0x0B77E0FF),new Protocols(3,0x0B78E0FF),new Protocols(3,0x0B6A5FFF)
            ,new Protocols(3,0xB70E0FF),new Protocols(3,0xB71E0FF),new Protocols(3,0xB72E0FF),new Protocols(3,0xB73E0FF),new Protocols(3,0xB74E0FF),new Protocols(3,0xB76E0FF),new Protocols(3,0xB77E0FF),new Protocols(3,0x0B78E0FF),new Protocols(3,0x0B6A5FFF)
        };
        public static bool IsConnection { get; set; }
        private static int Can_error_count = 0;


        public static bool Send(Byte[] data, byte[] canid)
        {
            gSendMsgBuf = new CAN_OBJ[SEND_MSG_BUF_MAX];
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
            gSendMsgBuf[gSendMsgBufHead].ExternFlag = 1;
            gSendMsgBuf[gSendMsgBufHead].RemoteFlag = 0;
            gSendMsgBufHead++;
            if (gSendMsgBufHead >= SEND_MSG_BUF_MAX)
            {
                gSendMsgBufHead = 0;
            }

            CAN_OBJ[] coMsg = new CAN_OBJ[2];

            if (gSendMsgBufHead != gSendMsgBufTail)
            {
                coMsg[0] = gSendMsgBuf[gSendMsgBufTail];
                coMsg[1] = gSendMsgBuf[gSendMsgBufTail];
                gSendMsgBufTail++;

                if (gSendMsgBufTail >= SEND_MSG_BUF_MAX)
                {
                    gSendMsgBufTail = 0;
                }

                if (ECANHelper.Transmit(1, 0, 0, coMsg, 1) == ECANStatus.STATUS_OK)
                {
                    Console.WriteLine($"发送数据   帧ID:{co.ID.ToString("X8")},帧数据：{co.Data[0].ToString("X2")} {co.Data[1].ToString("X2")} {co.Data[2].ToString("X2")} {co.Data[3].ToString("X2")} {co.Data[4].ToString("X2")} {co.Data[5].ToString("X2")} {co.Data[6].ToString("X2")} {co.Data[7].ToString("X2")}");
                    return true;
                }
            }

            return false;
        }

        public static void Receive()
        {
            while (true)
            {
                CAN_OBJ coMsg = new CAN_OBJ();

                if (ECANHelper.Receive(1, 0, 0, out coMsg, 1, 1) == ECANStatus.STATUS_OK)
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
                    foreach (Protocols item in protocols)
                    {
                        uint index = 0x00;
                        switch (item.Index)
                        {
                            case 0: index = 0xff000000; break;
                            case 1: index = 0xff0000; break;
                            case 2: index = 0xff00; break;
                            case 3: index = 0xff; break;
                        }

                        uint revId = coMsg.ID | index;

                        if (revId == item.Id)
                        {
                            string ss = "";
                            for (int i = 0; i < coMsg.Data.Length; i++)
                            {
                                ss += " " + coMsg.Data[i].ToString("X2");
                            }

                            Console.WriteLine($"CAN_ID:{coMsg.ID.ToString("X8")},Data：{ss.ToString()}");

                            EnqueueTask(coMsg);
                            break;
                        }
                    }
                }
            }
        }

        public static string ReadError()
        {
            string error = string.Empty;

            CAN_ERR_INFO err_info = new CAN_ERR_INFO();

            if (ECANHelper.ReadErrInfo(1, 0, 0, out err_info) == ECANStatus.STATUS_OK)
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
                        if (ECANHelper.ResetCAN(1, 0, 0) == ECANStatus.STATUS_OK)
                        {
                            Can_error_count = 0;
                            ECANHelper.StartCAN(1, 0, 0);
                            Console.WriteLine($"当前错误码：{0}，执行了复位CAN操作");
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

        private static void EnqueueTask(CAN_OBJ CANOBJ)
        {
            lock (_locker)
            {
                _task.Enqueue(CANOBJ);
                _wh.Set();
            }
        }
    }
}
