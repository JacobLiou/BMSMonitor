using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofar.BMSLib
{

    public class BMSConfig
    {

        public static BMSConfigurationModel ConfigManager = null;

       


    }
    public class BMSConfigurationModel
    {
        /// <summary>
      /// CAN 波特率
      /// </summary>
        public string CAN_BaudRate { get; set; }

        /// <summary>
        /// CAN卡类型
        /// </summary>
        public string CAN_DevType { get; set; }

        /// <summary>
        /// CAN卡号
        /// </summary>
        public string CAN_DevIndex { get; set; }

        /// <summary>
        /// CAN卡通道
        /// </summary>
        public string CAN_Channel { get; set; }
    }
}
