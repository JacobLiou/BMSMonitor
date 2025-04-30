using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerKit.Domain.Enums
{
    // 接口卡类型定义
    public enum PCIDeviceType
    {
        VCI_PCI5121 = 1,
        VCI_PCI9810 = 2,
        VCI_USBCAN1 = 3,
        VCI_USBCAN2 = 4,
        VCI_PCI9820 = 5,
        VCI_CAN232 = 6,
        VCI_PCI5110 = 7,
        VCI_CANLITE = 8,
        VCI_ISA9620 = 9,
        VCI_ISA5420 = 10,
        VCI_PC104CAN = 11,
        VCI_CANETUDP = 12,
        VCI_CANETE = 12,
        VCI_DNP9810 = 13,
        VCI_PCI9840 = 14,
        VCI_PC104CAN2 = 15,
        VCI_PCI9820I = 16,
        VCI_CANETTCP = 17,
        VCI_PEC9920 = 18,
        VCI_PCI5010U = 19,
        VCI_USBCAN_E_U = 20,
        VCI_USBCAN_2E_U = 21,
        VCI_PCI5020U = 22,
        VCI_EG20T_CAN = 23,
        VCI_PCIE9221 = 24,
        VCI_CANWIFI_TCP = 25,
        VCI_CANWIFI_UDP = 26,
        VCI_PCIe_9120I = 27,
        VCI_PCIe_9110I = 28,
        VCI_PCIe_9140I = 29,
        VCI_USBCAN_4E_U = 31,
        VCI_CANDTU = 32,
        VCI_USBCAN_8E_U = 34,
        VCI_CANDTU_NET = 36
    }
}
