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

    public enum WorkState_PCU
    {
        Upgrade = 0,
        Check = 1,
        Normal = 2,
        ActiveBattery = 3,
        Fault = 4,
        PermanentFault = 5
    }

    public enum ResetMode
    {
        低功率复位 = 0,
        电压低复位 = 1,
        软件复位 = 2,
        IWDG复位 = 3,
        WWDG复位 = 4,
        Pin引脚复位 = 5
    }

    public enum BMSState
    {
        开机自检 = 0,
        运行 = 1,
        永久性故障 = 2,
        升级 = 3,
        关机 = 4
    }

    public enum WakeSource
    {
        无唤醒源 = 0,
        按键唤醒 = 1,
        充电唤醒 = 2,
        can唤醒 = 3,
        电池包IN唤醒 = 4,
        充电激活换唤醒 = 5,
    }

    public enum WorkState_DCDC
    {
        等待唤醒 = 0,
        开机自检 = 1,
        运行 = 2,
        永久性故障 = 3,
        升级 = 4,
    }

    public enum BatteryState_DCDC
    {
        待机 = 0x0,
        充电 = 0x1,
        放电 = 0x2
    }
}
