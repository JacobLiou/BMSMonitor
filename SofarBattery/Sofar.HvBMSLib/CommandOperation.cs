using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofar.BMSLib
{
    public class CommandOperation
    {
        BaseCanHelper baseCanHelper = null;
        public bool IsConnection
        {
            get
            {
                return baseCanHelper.IsConnection;
            }
        }
        public string CommunicationType
        {
            get
            {
                return baseCanHelper.CommunicationType;
            }
        }
        public CommandOperation(BMSConfigurationModel config)
        {
            switch (config.CAN_DevType)
            {

                case "USBCAN-I/I+":
                case "USBCAN-II/II+":
                    baseCanHelper = EcanHelper.Instance();
                    break;
                case "USBCAN-E-U":
                case "USBCAN-2E-U":
                    baseCanHelper = ControlcanHelper.Instance();
                    break;
                default:
                    throw new ArgumentException($"不支持的CAN设备类型: {config.CAN_DevType}");
            }
        }
        public virtual bool Send(Byte[] data, byte[] canid)
        {
            return baseCanHelper.Send(data, canid);
        }
        public void ReadBCUParam(int SelectedAddress_BCU)
        {

            /*后台软件查询配置表内容，后台软件发送给命令码 0x1F 消息（命令码）给 EVBCM，EVBCM 收到消息后依次发送 0x00~0x1a 消息给后台软件*/

            //查询发送的命令码数组
            //0x04-0x0F、0x1C、0x2B、0x2C 共15个命令码
            byte[] commandCodes = new byte[] { 0x04, 0x05, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x1C, 0x2B, 0x2C };
            byte Address_BCU = Convert.ToByte(SelectedAddress_BCU);
            //遍历命令码数组，依次下发
            foreach (byte code in commandCodes)
            {
                byte[] bytes = new byte[8] { code, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                baseCanHelper.Send(bytes, new byte[] { 0xF4, Address_BCU, 0x1F, 0x18 });
            }

            //充电电流报警值（命令码 0x06）
            //充电类型
            byte[] subCommandCodes = new byte[] { 0x01, 0x02, 0x03 };
            //遍历子命令码数组，依次下发
            foreach (byte Code in subCommandCodes)
            {
                byte[] Bytes = new byte[8] { 0x06, Code, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                baseCanHelper.Send(Bytes, new byte[] { 0xF4, Address_BCU, 0x1F, 0x18 });
            }

        }
    }
}
