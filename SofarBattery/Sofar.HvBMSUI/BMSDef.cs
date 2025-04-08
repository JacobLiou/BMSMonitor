using Sofar.BMSLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sofar.BMSUI
{
    public class BMSDef
    {
        public new static Application Current => Application.Current;


        public static void SetBMSConfig()
        {
            BMSConfig.ConfigManager = new BMSConfigurationModel();
            BMSConfig.ConfigManager.CAN_DevType = System.Configuration.ConfigurationManager.AppSettings.Get("CAN_DevType");
            BMSConfig.ConfigManager.CAN_BaudRate = System.Configuration.ConfigurationManager.AppSettings.Get("CAN_BaudRate");
            BMSConfig.ConfigManager.CAN_DevIndex = System.Configuration.ConfigurationManager.AppSettings.Get("CAN_DevIndex");
            BMSConfig.ConfigManager.CAN_Channel = System.Configuration.ConfigurationManager.AppSettings.Get("CAN_Channel");

            switch (BMSConfig.ConfigManager.CAN_DevType)
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
                    throw new ArgumentException($"不支持的CAN设备类型: {BMSConfig.ConfigManager.CAN_DevType}");
            }


        }
        public static void SetBMSConfig(string CAN_DevType, string CAN_BaudRate, string CAN_DevIndex, string CAN_Channel)
        {
            BMSConfig.ConfigManager = new BMSConfigurationModel();
            BMSConfig.ConfigManager.CAN_DevType = CAN_DevType;
            BMSConfig.ConfigManager.CAN_BaudRate = CAN_BaudRate;
            BMSConfig.ConfigManager.CAN_DevIndex = CAN_DevIndex;
            BMSConfig.ConfigManager.CAN_Channel = CAN_Channel;

            switch (BMSConfig.ConfigManager.CAN_DevType)
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
                    throw new ArgumentException($"不支持的CAN设备类型: {BMSConfig.ConfigManager.CAN_DevType}");
            }


        }
        static BaseCanHelper baseCanHelper = null;
        public static bool GetConnectionStatus()
        {


            return baseCanHelper.IsConnection;
        }

        public static bool Connect()
        {
            if (baseCanHelper != null)
            {
                baseCanHelper.Connect();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 主窗体的依赖注入对象
        /// </summary>
        public static IServiceProvider MainAppServices { get; set; }
    }
}
