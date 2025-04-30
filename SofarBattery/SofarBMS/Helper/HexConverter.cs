using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofarBMS.Helper
{
    public class HexConverter
    {
        /// <summary>
        /// 将十六进制字符串转换为有符号8位整数(sbyte/i8)
        /// </summary>
        /// <param name="hex">十六进制字符串（1-2个字符）</param>
        public static sbyte HexToSByte(string hex)
        {
            // 清理输入并验证格式
            string cleanHex = CleanHexString(hex);
            ValidateHexLength(cleanHex, 2);

            // 转换为字节再转为sbyte（自动处理二进制补码）
            byte byteValue = HexToByte(cleanHex);
            return (sbyte)byteValue;
        }

        /// <summary>
        /// 将十六进制字符串转换为无符号8位整数(byte/u8)
        /// </summary>
        /// <param name="hex">十六进制字符串（1-2个字符）</param>
        public static byte HexToByte(string hex)
        {
            // 清理输入并验证格式
            string cleanHex = CleanHexString(hex);
            ValidateHexLength(cleanHex, 2);

            // 使用内置方法转换
            return byte.Parse(cleanHex, NumberStyles.HexNumber);
        }

        private static string CleanHexString(string input)
        {
            // 去除空格和前缀
            return input?.Trim()
                .Replace("0x", "")
                .Replace("0X", "")
                .Replace(" ", "")
                .ToUpper();
        }

        private static void ValidateHexLength(string hex, int maxLength)
        {
            if (string.IsNullOrEmpty(hex))
                throw new ArgumentException("输入不能为空");

            if (hex.Length > maxLength)
                throw new ArgumentException($"十六进制值过长（最大支持{maxLength}位）");

            foreach (char c in hex)
            {
                if (!Uri.IsHexDigit(c))
                    throw new FormatException($"包含非法字符: {c}");
            }
        }
    }
}
