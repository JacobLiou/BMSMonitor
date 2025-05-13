using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Sofar.HvBMSLib
{
    public static class HexDataHelper
    {
        public static string ByteToHexString(byte[] data)
        {
            if (data == null) return "";
            StringBuilder sb = new StringBuilder();//清除字符串构造器的内容  
            foreach (byte b in data)
            {
                sb.Append(b.ToString("X2") + " ");//一个字节一个字节的处理，
            }
            return sb.ToString();
        }


        /// <summary>
        /// 字符串转byte数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] HexStringToByte(string str)
        {
            var arr = str.Split(' ');
            byte[] result = new byte[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                int n = Convert.ToInt32(arr[i], 16);
                result[i] = (byte)n;
            }
            return result;
        }
        public static byte[] ShortToByte(short value)
        {
            byte[] data = new byte[2];
            int k = 0;
            data[k++] = (byte)(value >> 8);
            data[k++] = (byte)(value & 0xff);
            return data;

        }
        public static byte[] ShortToByte(ushort value)
        {
            byte[] data = new byte[2];
            int k = 0;
            data[k++] = (byte)(value >> 8);
            data[k++] = (byte)(value & 0xff);
            return data;

        }
        public static byte[] IntToByte(int value, bool IsLittleEndian)
        {
            byte[] data = new byte[4];
            int k = 0;
            if (IsLittleEndian)
            {
                data[k++] = (byte)(value >> 24);
                data[k++] = (byte)(value >> 16);
                data[k++] = (byte)(value >> 8);
                data[k++] = (byte)(value & 0xff);

            }
            else//big-Endian
            {
                data[k++] = (byte)(value >> 8);
                data[k++] = (byte)(value & 0xff);
                data[k++] = (byte)(value >> 24);
                data[k++] = (byte)(value >> 16);
            }

            return data;

        }
        public static byte[] IntToByte(uint value, bool IsLittleEndian)
        {
            byte[] data = new byte[4];
            int k = 0;
            if (IsLittleEndian)
            {
                data[k++] = (byte)(value >> 24);
                data[k++] = (byte)(value >> 16);
                data[k++] = (byte)(value >> 8);
                data[k++] = (byte)(value & 0xff);
            }
            else //big-Endian
            {
                data[k++] = (byte)(value >> 8);
                data[k++] = (byte)(value & 0xff);
                data[k++] = (byte)(value >> 24);
                data[k++] = (byte)(value >> 16);
            }
            return data;

        }
        public static int ByteToShort(byte[] buffer)
        {


            var byte1 = buffer[0] << 8;
            var byte2 = buffer[1] & 0xff;
            int u32 = Convert.ToInt32(byte1 + byte2);
            return u32;

        }

        public static int ByteToInt(byte[] buffer, bool IsLittleEndian)
        {

            int byte1, byte2, byte3, byte4;
            if (IsLittleEndian)
            {

                byte1 = buffer[0] << 24;
                byte2 = buffer[1] << 16;
                byte3 = buffer[2] << 8;
                byte4 = buffer[3] & 0xff;
                int u32 = Convert.ToInt32(byte1 + byte2 + byte3 + byte4);
                return u32;
            }
            else
            {
                byte1 = buffer[0] << 8;
                byte2 = buffer[1] & 0xff;
                byte3 = buffer[2] << 24;
                byte4 = buffer[3] << 16;
                int u32 = Convert.ToInt32(byte1 + byte2 + byte3 + byte4);
                return u32;
            }

        }
        public static uint ByteToUInt(byte[] buffer, bool IsLittleEndian)
        {

            uint byte1, byte2, byte3, byte4;
            if (IsLittleEndian)
            {

                byte1 = (uint)buffer[0] << 24;
                byte2 = (uint)buffer[1] << 16;
                byte3 = (uint)buffer[2] << 8;
                byte4 = (uint)buffer[3] & 0xff;
                uint u32 = Convert.ToUInt32(byte1 + byte2 + byte3 + byte4);
                return u32;
            }
            else
            {
                byte1 = (uint)buffer[0] << 8;
                byte2 = (uint)buffer[1] & 0xff;
                byte3 = (uint)buffer[2] << 24;
                byte4 = (uint)buffer[3] << 16;
                uint u32 = Convert.ToUInt32(byte1 + byte2 + byte3 + byte4);
                return u32;
            }

        }

        public static byte[] LongToByte(long value, bool IsLittleEndian)
        {


            byte[] data = new byte[8];
            int k = 0;

            if (IsLittleEndian)
            {
                data[k++] = (byte)(value >> 56);
                data[k++] = (byte)(value >> 48);
                data[k++] = (byte)(value >> 40);
                data[k++] = (byte)(value >> 32);
                data[k++] = (byte)(value >> 24);
                data[k++] = (byte)(value >> 16);
                data[k++] = (byte)(value >> 8);
                data[k++] = (byte)(value & 0xff);
            }
            else//big-Endian
            {
                data[k++] = (byte)(value & 0xff);
                data[k++] = (byte)(value >> 8);
                data[k++] = (byte)(value >> 16);
                data[k++] = (byte)(value >> 24);
                data[k++] = (byte)(value >> 32);
                data[k++] = (byte)(value >> 40);
                data[k++] = (byte)(value >> 48);
                data[k++] = (byte)(value >> 56);
            }
            return data;
        }

        public static string GetDebugByteString(byte[] data, string title)
        {
            if (data == null) return "";
            StringBuilder sb = new StringBuilder();//清除字符串构造器的内容  
            sb.AppendFormat("{0} {1} ", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), title);

            foreach (byte b in data)
            {
                sb.Append(b.ToString("X2") + " ");//一个字节一个字节的处理，
            }
            return sb.ToString();
        }



    }
}
