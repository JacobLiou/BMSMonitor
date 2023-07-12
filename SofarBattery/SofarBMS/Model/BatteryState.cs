using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofarBMS.Model
{
    public enum BatteryState
    {
        Charging,//充电状态 = 0,
        Discharge//放电状态 = 1
    }

    public enum WorkState
    {
        Upgrade = 0,
        Check = 1,
        Normal = 2,
        ActiveBattery = 3,
        Fault = 4,
        PermanentFault = 5
    }
}
