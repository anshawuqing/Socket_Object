using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;


/// <summary>
/// 
/// </summary>
namespace  YWSNet
{

/// <summary>
///  
/// </summary>
public class ByteBuffer : MonoBehaviour
    {
        MemoryStream stream = null;
        BinaryWriter writer = null;
        BinaryReader reader = null;
        
        /// <summary>
        /// 构造函数 -1 
        /// </summary>
         public ByteBuffer  ()
        {
            stream = new MemoryStream();
            writer = new  BinaryWriter (stream);
        }
            
         public ByteBuffer  ( byte []  data )
        {
            if (data  != null)
            {
                stream = new MemoryStream();
                writer = new BinaryWriter  (stream);                             
            }
        }

        /// <summary>
        ///  关闭构造函数
        /// </summary>
        public  void  Close()
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
         public  void WriteByte( byte v )
        {
            writer.Write(v);
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
                 
        public   void WriteInit( int v)
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
       public  void  WriteLong( long v)
        {
            writer.Write((long)v);
        }
      
        /// <summary>
        ///   
        /// </summary>
        /// <param name="v"></param>
        public  void WriteFloat(float v)
        {
            byte[] temp = BitConverter.GetBytes(v);
            Array.Reverse(temp);
            writer.Write(BitConverter.ToSingle(temp, 0));
        }
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        public void  WriteDouble(double v)
        {
            byte[] temp = BitConverter.GetBytes(v);
            Array.Reverse(temp);
            writer.Write(BitConverter.ToDouble(temp, 0));
        } 
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        public   void WriteString(string v)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(v);
            writer.Write( (ushort)  bytes.Length);
            writer.Write(bytes);
        }

        
    }

}
