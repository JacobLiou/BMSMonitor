using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofar.BMSLib
{
    public class Protocols
    {
        //XX是代表BMS的编号，XX在前面说明BMS是接收方，XX在最后的字节说明BMS是发送方 LU：定义index为XX的坐标位置
        private int _index;
        private int _id;
        public int Index { get => _index; set => _index = value; }
        public int Id { get => _id; set => _id = value; }

        public Protocols(int index, int id)
        {
            this.Index = index;
            this.Id = id;
        }

        public static List<Protocols> protocols = new List<Protocols>() {
             new Protocols(3,0x1020FFFF),new Protocols(3,0x1020E0FF)
            ,new Protocols(3,0x07F0E0FF),new Protocols(3,0x07F1E0FF),new Protocols(3,0x07F2E0FF),new Protocols(3,0x07F3E0FF),new Protocols(3,0x07F4E0FF)

            ,new Protocols(3,0x1003E0FF),new Protocols(3,0x1004E0FF),new Protocols(3,0x1005E0FF),new Protocols(3,0x1006E0FF),new Protocols(3,0x1007E0FF),new Protocols(3,0x1008E0FF),new Protocols(3,0x1009E0FF),new Protocols(3,0x100AE0FF),new Protocols(3,0x100BE0FF),new Protocols(3,0x100CE0FF),new Protocols(3,0x100DE0FF),new Protocols(3,0x100EE0FF),new Protocols(3,0x100FE0FF)
            ,new Protocols(3,0x1040E0FF),new Protocols(3,0x1041E0FF),new Protocols(3,0x1042E0FF),new Protocols(3,0x1043E0FF),new Protocols(3,0x1044E0FF),new Protocols(3,0x104AE0FF),new Protocols(3,0x104EE0FF),new Protocols(3,0x104E0FFF),new Protocols(3,0x1045E0FF),new Protocols(3,0x1046E0FF),new Protocols(3,0x1047E0FF),new Protocols(3,0x1048E0FF),new Protocols(3,0x1049E0FF),new Protocols(3,0x104BE0FF),new Protocols(3,0x104CE0FF)

            ,new Protocols(3,0x1003FFFF),new Protocols(3,0x1004FFFF),new Protocols(3,0x1005FFFF),new Protocols(3,0x1006FFFF),new Protocols(3,0x1007FFFF),new Protocols(3,0x1008FFFF),new Protocols(3,0x1009FFFF),new Protocols(3,0x100AFFFF),new Protocols(3,0x100BFFFF),new Protocols(3,0x100CFFFF),new Protocols(3,0x100DFFFF),new Protocols(3,0x100EFFFF),new Protocols(3,0x100FFFFF)
            ,new Protocols(3,0x1040FFFF),new Protocols(3,0x1041FFFF),new Protocols(3,0x1042FFFF),new Protocols(3,0x1043FFFF),new Protocols(3,0x1044FFFF),new Protocols(3,0x104AFFFF),new Protocols(3,0x104EFFFF),new Protocols(3,0x104FFFFF),new Protocols(3,0x1045FFFF),new Protocols(3,0x1046FFFF),new Protocols(3,0x1047FFFF),new Protocols(3,0x1048FFFF),new Protocols(3,0x1049FFFF),new Protocols(3,0x104BFFFF),new Protocols(3,0x104CFFFF)

            ,new Protocols(3,0x106AE0FF),new Protocols(3,0x106BE0FF),new Protocols(3,0x106CE0FF),new Protocols(3,0x106DE0FF)
            ,new Protocols(3,0x106AFFFF),new Protocols(3,0x106BFFFF),new Protocols(3,0x106CFFFF),new Protocols(3,0x106DFFFF)
            ,new Protocols(2,0x102FFFE0)
            ,new Protocols(3,0x1030FFFF),new Protocols(3,0x1031FFFF),new Protocols(3,0x1033FFFF),new Protocols(3,0x1034FFFF),new Protocols(3,0x1035FFFF),new Protocols(3,0x1036FFFF),new Protocols(3,0x1037FFFF),new Protocols(3,0x1038FFFF),new Protocols(3,0x1039FFFF),new Protocols(3,0x103AFFFF),new Protocols(3,0x103BFFFF),new Protocols(3,0x103CFFFF),new Protocols(3,0x103DFFFF),new Protocols(3,0x103EFFFF),new Protocols(3,0x103FFFFF),new Protocols(3,0x1050FFFF),new Protocols(3,0x1051FFFF),new Protocols(3,0x102DFFFF)
            ,new Protocols(3,0x1030E0FF),new Protocols(3,0x1031E0FF),new Protocols(3,0x1033E0FF),new Protocols(3,0x1034E0FF),new Protocols(3,0x1035E0FF),new Protocols(3,0x1036E0FF),new Protocols(3,0x1037E0FF),new Protocols(3,0x1038E0FF),new Protocols(3,0x1039E0FF),new Protocols(3,0x103AE0FF),new Protocols(3,0x103BE0FF),new Protocols(3,0x103CE0FF),new Protocols(3,0x103DE0FF),new Protocols(3,0x103EE0FF),new Protocols(3,0x103FE0FF),new Protocols(3,0x1050E0FF),new Protocols(3,0x1051E0FF),new Protocols(3,0x102DE0FF)

             //BCU 电芯电压                 电芯温度                      电芯均衡状态                  电芯均衡温度                  电芯SOC                       电芯SOH
            ,new Protocols(3,0x1070E0FF),new Protocols(3,0x1071E0FF),new Protocols(3,0x1072E0FF),new Protocols(3,0x1073E0FF),new Protocols(3,0x10A0E0FF)   ,new Protocols(3,0x10A1E0FF)
            ,new Protocols(3,0x107081FF),new Protocols(3,0x107181FF),new Protocols(3,0x1072FFFF),new Protocols(3,0x1073FFFF),new Protocols(3,0x10A0FFFF)   ,new Protocols(3,0x10A1FFFF)


            //,new Protocols(3,0x1080FFFF)
            ,new Protocols(3,0x1080E0FF),new Protocols(3,0x1081E0FF),new Protocols(3,0x1082E0FF),new Protocols(3,0x1083E0FF),new Protocols(3,0x1084E0FF),new Protocols(3,0x1085E0FF)


            ,new Protocols(3,0x1010E0FF),new Protocols(3,0x1011E0FF),new Protocols(3,0x1012E0FF),new Protocols(3,0x1013E0FF),new Protocols(3,0x1014E0FF),new Protocols(3,0x1015E0FF),new Protocols(3,0x1016E0FF),new Protocols(3,0x1017E0FF),new Protocols(3,0x1018E0FF),new Protocols(3,0x1019E0FF),new Protocols(3,0x101AE0FF),new Protocols(3,0x101BE0FF),new Protocols(3,0x101CE0FF)
            ,new Protocols(3,0x1021E0FF),new Protocols(3,0x1022E0FF),new Protocols(3,0x1023E0FF),new Protocols(3,0x1024E0FF),new Protocols(3,0x1025E0FF),new Protocols(3,0x1026E0FF),new Protocols(3,0x1027E0FF),new Protocols(3,0x1028E0FF),new Protocols(3,0x1029E0FF),new Protocols(3,0x102AE0FF),new Protocols(3,0x102EE0FF),new Protocols(3,0x102FE0FF),new Protocols(3,0x101EE0FF)

            ,new Protocols(3,0x1010FFFF),new Protocols(3,0x1011FFFF),new Protocols(3,0x1012FFFF),new Protocols(3,0x1013FFFF),new Protocols(3,0x1014FFFF),new Protocols(3,0x1015FFFF),new Protocols(3,0x1016FFFF),new Protocols(3,0x1017FFFF),new Protocols(3,0x1018FFFF),new Protocols(3,0x1019FFFF),new Protocols(3,0x101AFFFF),new Protocols(3,0x101BFFFF),new Protocols(3,0x101CFFFF)
            ,new Protocols(3,0x1021FFFF),new Protocols(3,0x1022FFFF),new Protocols(3,0x1023FFFF),new Protocols(3,0x1024FFFF),new Protocols(3,0x1025FFFF),new Protocols(3,0x1026FFFF),new Protocols(3,0x1027FFFF),new Protocols(3,0x1028FFFF),new Protocols(3,0x1029FFFF),new Protocols(3,0x102AFFFF),new Protocols(3,0x102EFFFF),new Protocols(3,0x102FFFFF),new Protocols(3,0x101EFFFF)

            //BCU升级-增加地址E8~F1
            ,new Protocols(3,0x07FAF4FF),new Protocols(3,0x07FBF4FF),new Protocols(3,0x07FCF4FF),new Protocols(3,0x07FDF4FF),new Protocols(3,0x07FEF4FF),new Protocols(3,0x07FFF4FF)
            ,new Protocols(3,0x07FAF4E8),new Protocols(3,0x07FBF4E8),new Protocols(3,0x07FCF4E8),new Protocols(3,0x07FDF4E8),new Protocols(3,0x07FEF4E8),new Protocols(3,0x07FFF4E8)
            ,new Protocols(3,0x07FAF4E9),new Protocols(3,0x07FBF4E9),new Protocols(3,0x07FCF4E9),new Protocols(3,0x07FDF4E9),new Protocols(3,0x07FEF4E9),new Protocols(3,0x07FFF4E9)
            ,new Protocols(3,0x07FAF4EA),new Protocols(3,0x07FBF4EA),new Protocols(3,0x07FCF4EA),new Protocols(3,0x07FDF4EA),new Protocols(3,0x07FEF4EA),new Protocols(3,0x07FFF4EA)
            ,new Protocols(3,0x07FAF4EB),new Protocols(3,0x07FBF4EB),new Protocols(3,0x07FCF4EB),new Protocols(3,0x07FDF4EB),new Protocols(3,0x07FEF4EB),new Protocols(3,0x07FFF4EB)
            ,new Protocols(3,0x07FAF4EC),new Protocols(3,0x07FBF4EC),new Protocols(3,0x07FCF4EC),new Protocols(3,0x07FDF4EC),new Protocols(3,0x07FEF4EC),new Protocols(3,0x07FFF4EC)
            ,new Protocols(3,0x07FAF4ED),new Protocols(3,0x07FBF4ED),new Protocols(3,0x07FCF4ED),new Protocols(3,0x07FDF4ED),new Protocols(3,0x07FEF4ED),new Protocols(3,0x07FFF4ED)
            ,new Protocols(3,0x07FAF4EE),new Protocols(3,0x07FBF4EE),new Protocols(3,0x07FCF4EE),new Protocols(3,0x07FDF4EE),new Protocols(3,0x07FEF4EE),new Protocols(3,0x07FFF4EE)
            ,new Protocols(3,0x07FAF4EF),new Protocols(3,0x07FBF4EF),new Protocols(3,0x07FCF4EF),new Protocols(3,0x07FDF4EF),new Protocols(3,0x07FEF4EF),new Protocols(3,0x07FFF4EF)
            ,new Protocols(3,0x07FAF4F0),new Protocols(3,0x07FBF4F0),new Protocols(3,0x07FCF4F0),new Protocols(3,0x07FDF4F0),new Protocols(3,0x07FEF4F0),new Protocols(3,0x07FFF4F0)
            ,new Protocols(3,0x07FAF4F1),new Protocols(3,0x07FBF4F1),new Protocols(3,0x07FCF4F1),new Protocols(3,0x07FDF4F1),new Protocols(3,0x07FEF4F1),new Protocols(3,0x07FFF4F1)

           

            //BMU升级
            ,new Protocols(3,0x07FAE0FF),new Protocols(3,0x07FBE0FF),new Protocols(3,0x07FCE0FF),new Protocols(3,0x07FDE0FF),new Protocols(3,0x07FEE0FF),new Protocols(3,0x07FFE0FF)
            ,new Protocols(3,0x07FB41FF),new Protocols(3,0x07FC41FF),new Protocols(3,0x07FD41FF),new Protocols(3,0x07FE41FF),new Protocols(3,0x07FF41FF),new Protocols(3,0x07FF5FFF)
            ,new Protocols(3,0x0B605FFF),new Protocols(3,0x0B615FFF),new Protocols(3,0x0B625FFF),new Protocols(3,0x0B635FFF),new Protocols(3,0x0B665FFF),new Protocols(3,0x0B70E0FF),new Protocols(3,0x0B71E0FF),new Protocols(3,0x0B72E0FF),new Protocols(3,0x0B73E0FF),new Protocols(3,0x0B74E0FF),new Protocols(3,0x0B75E0FF),new Protocols(3,0x0B76E0FF),new Protocols(3,0x0B77E0FF),new Protocols(3,0x0B78E0FF),new Protocols(3,0x0B6A5FFF)
            ,new Protocols(3,0x0B70E0FF),new Protocols(3,0x0B71E0FF),new Protocols(3,0x0B72E0FF),new Protocols(3,0x0B73E0FF),new Protocols(3,0x0B74E0FF),new Protocols(3,0x0B76E0FF),new Protocols(3,0x0B77E0FF),new Protocols(3,0x0B78E0FF),new Protocols(3,0x0B6A5FFF)
            ,new Protocols(3,0x1403FFFF),new Protocols(3,0x1400E0FF)
            ,new Protocols(3,0x1060FFFF),new Protocols(3,0x1060FF1F),new Protocols(3,0x1061FFFF),new Protocols(3,0x1061FF1F)
            ,new Protocols(3,0x10B6E0FF),new Protocols(3,0x10B7E0FF),new Protocols(3,0x10B8E0FF),new Protocols(3,0x10B9E0FF),new Protocols(3,0x10BAE0FF),new Protocols(3,0x10BBE0FF),new Protocols(3,0x10BCE0FF),new Protocols(3,0x10BDE0FF),new Protocols(3,0x10BEE0FF),new Protocols(3,0x10BFE0FF),new Protocols(3,0x10C0E0FF),new Protocols(3,0x10C1E0FF),new Protocols(3,0x10C2E0FF),new Protocols(3,0x10C3E0FF),new Protocols(3,0x10C4E0FF),new Protocols(3,0x10C5E0FF),new Protocols(3,0x10C6E0FF)
            ,new Protocols(3,0x10E0E0FF),new Protocols(3,0x10E1E0FF),new Protocols(3,0x10E2E0FF),new Protocols(3,0x10E3E0FF),new Protocols(3,0x10E4E0FF),new Protocols(3,0x10E5E0FF),new Protocols(3,0x10EFE0FF),new Protocols(3,0x10F0E0FF),new Protocols(3,0x10F1E0FF),new Protocols(3,0x10F2E0FF),new Protocols(3,0x10F3E0FF),new Protocols(3,0x10F4E0FF),new Protocols(3,0x10F5E0FF),new Protocols(3,0x10F6E0FF),new Protocols(3,0x10F7E0FF),new Protocols(3,0x10F8E0FF),new Protocols(3,0x10F9E0FF),new Protocols(3,0x10FAE0FF)
            
            //EVBCM
            ,new Protocols(3,0x1800F4FF),new Protocols(3,0x1803F4FF),new Protocols(3,0x1812F4FF),new Protocols(3,0x1816F4FF),new Protocols(3,0x181AF4FF),new Protocols(3,0x1840F4FF),new Protocols(3,0x1841F4FF),new Protocols(3,0x1804F4FF),new Protocols(3,0x1805F4FF),new Protocols(3,0x1806F4FF),new Protocols(3,0x1807F4FF),new Protocols(3,0x1808F4FF),new Protocols(3,0x1809F4FF),new Protocols(3,0x180AF4FF),new Protocols(3,0x180BF4FF),new Protocols(3,0x180CF4FF),new Protocols(3,0x180DF4FF),new Protocols(3,0x180EF4FF),new Protocols(3,0x180FF4FF),new Protocols(3,0x181CF4FF),new Protocols(3,0x182BF4FF),new Protocols(3,0x182CF4FF)
            ,new Protocols(3,0x181EF4FF),new Protocols(3,0x1831F4FF),new Protocols(3,0x1832F4FF),new Protocols(3,0x1832F4FF),new Protocols(3,0x18B2F4FF),new Protocols(3,0x1833F4FF),new Protocols(3,0x1834F4FF),new Protocols(3,0x1835F4FF),new Protocols(3,0x1836F4FF),new Protocols(3,0x1837F4FF),new Protocols(3,0x1838F4FF),new Protocols(3,0x1839F4FF),new Protocols(3,0x183AF4FF),new Protocols(3,0x183BF4FF),new Protocols(3,0x183CF4FF),new Protocols(3,0x183DF4FF)
            ,new Protocols(3,0x1814F4FF),
        };
    }
}
