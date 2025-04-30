using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofar.BMSUI.Models
{
    public class FirmwareModel : ObservableObject
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string FirmwareName { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public byte FirmwareFileType { get; set; }

        /// <summary>
        /// 芯片角色
        /// </summary>
        public byte FirmwareChipRole { get; set; }

        /// <summary>
        /// 起始偏移地址
        /// </summary>
        public long FirmwareStartAddress { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string FirmwareVersion { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public long FirmwareLength { get; set; }

        /// <summary>
        /// 升级进度
        /// </summary>
        public long ProgressData { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsFinish { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdataTime { get; set; }
    }
}
