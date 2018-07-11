using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;


/// <summary>
/// 
/// </summary>
namespace  YWSNet
{

    /// <summary>
    ///  Socket  通信的基类脚本  
    /// </summary>
    public class ByteBuffer 
    {
        MemoryStream stream = null;
        BinaryWriter writer = null;
        BinaryReader reader = null;

        /// <summary>
        /// 构造函数 -1 
        /// </summary>
        public ByteBuffer()
        {
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
        }

        public ByteBuffer(byte[] data)
        {
            if (data != null)
            {
                stream = new MemoryStream(data);
                reader = new BinaryReader(stream);
            }
            else
            {
                stream = new MemoryStream();
                writer = new BinaryWriter(stream);
            }
        }

        /// <summary>
        ///  关闭构造函数
        /// </summary>
        public void Close()
        {
            if (writer != null) writer.Close();
            if (reader != null) reader.Close();

            stream.Close();
            writer = null;
            reader = null;
            stream = null;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        public void WriteByte(byte v)
        {
            writer.Write(v);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>

        public void WriteInit(int v)
        {
            writer.Write(v);
        }

        /// <summary>
        /// 编写短类型
        /// </summary>
        public void WriteShort(ushort v)
        {

        }

        /// <summary>
        /// 编写长类型
        /// </summary>
        public void WriteLong(long v)
        {
            writer.Write((long)v);
        }

        /// <summary>
        ///   
        /// </summary>
        /// <param name="v"></param>
        public void WriteFloat(float v)
        {
            byte[] temp = BitConverter.GetBytes(v);
            Array.Reverse(temp);
            writer.Write(BitConverter.ToSingle(temp, 0));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        public void WriteDouble(double v)
        {
            byte[] temp = BitConverter.GetBytes(v);
            Array.Reverse(temp);
            writer.Write(BitConverter.ToDouble(temp, 0));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        public void WriteString(string v)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(v);
            writer.Write((ushort)bytes.Length);
            writer.Write(bytes);
        }

        /// <summary>
        /// 
        /// </summary>
        public void WriteBytes(byte[]  v)
        {
            writer.Write((int) v.Length);
            writer.Write(v); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  byte  ReadByte()
        {
            return reader.ReadByte();
        }
     
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         public  int ReadInt()
        {
            return (int)reader.ReadInt32();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         public ushort   ReadShort()
        {
            return (ushort)reader.ReadInt16();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  long ReadLong()
        {
            return (long)reader.ReadInt64();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  float ReadFloat()
        {
            byte[] temp = BitConverter.GetBytes(reader.ReadSingle());
            Array.Reverse(temp);
            return BitConverter.ToSingle(temp,0);
         }

        /// <summary>
        /// 获取double 类型的变量
        /// </summary>
        /// <returns></returns>
        public double  ReadDouble()
        {
            byte[] temp = BitConverter.GetBytes(reader.ReadBoolean());
            Array.Reverse(temp);
            return BitConverter.ToDouble(temp, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            ushort len = ReadShort();
            byte[] buffer = new byte[len];
            buffer = reader.ReadBytes(len);
            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte []   ReadBytes()
        {
            int len = ReadInt();
            return reader.ReadBytes(len); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  byte [] ToBytes()
        {
            writer.Flush();
            return stream.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Flush()
        {
            writer.Flush();
        }
    }

}
