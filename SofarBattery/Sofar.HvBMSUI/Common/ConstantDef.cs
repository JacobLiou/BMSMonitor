using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofar.BMSUI
{
    public static class ConstantDef
    {

        /// <summary>
        /// 主控参数模块数量 （单簇PACK数量）
        /// </summary>
        public static int BCU_ModuleNumber = -1;

        /// <summary>
        /// 页面显示的最大Cell格子数（单个PACK）
        /// </summary>
        public static int PageMaxBatteryCellNumber = 64;

        /// <summary>
        /// 每个PACK里面的电芯数量，PowerMagic 1.0 48个，  PowerMagic 2.0 64个
        /// </summary>
        public static int BatteryCellNumber = 64;

        /// <summary>
        /// 每个PACK里面的温度数量，PowerMagic 1.0 28个，  PowerMagic 2.0 36个
        /// </summary>
        public static int BatteryTemperatureNumber = 36;

    }
}
