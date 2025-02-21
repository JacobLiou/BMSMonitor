using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofarBMS.Model
{
    public class FirmwareModel
    {
        public FirmwareModel()
        {

        }
        public FirmwareModel(string firmwareName, string firmwareType, int startAddress, string firmwareVersion, int length)
        {
            this.FirmwareName = firmwareName;
            this.FirmwareType = firmwareType;
            this.StartAddress = startAddress;
            this.FirmwareVersion = firmwareVersion;
            this.Length = length;
        }

        public string FirmwareName { get; set; }
        public string FirmwareType { get; set; }
        public int StartAddress { get; set; }
        public string FirmwareVersion { get; set; }
        public int Length { get; set; }
        public bool CheckFlg { get; set; }
    }

    public class FirmwareModel_BMS1500V 
    {
        /// <summary>
        /// 工程名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public uint FirmwareSize { get; set; }

        /// <summary>
        /// 文件创建时间
        /// </summary>
        public string dateTimeString { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FirmwareFileType { get; set; }

        /// <summary>
        /// 文件类型代号
        /// </summary>
        public byte FirmwareFileTypeCode { get; set; }

        /// <summary>
        /// 芯片角色
        /// </summary>
        public string FirmwareChipRole { get; set; }

        /// <summary>
        /// 芯片角色代号
        /// </summary>
        public byte FirmwareChipRoleCode { get; set; }

        /// <summary>
        /// 芯片代号
        /// </summary>
        public string ChipModel { get; set; }

        /// <summary>
        /// 芯片型号
        /// </summary>
        public string ChipMark { get; set; }

        /// <summary>
        ///软件版本号
        /// </summary>
        public string SoftwareVersion { get; set; }

        /// <summary>
        /// 硬件版本号
        /// </summary>
        public string HardwareVersion { get; set; }



    }
}
