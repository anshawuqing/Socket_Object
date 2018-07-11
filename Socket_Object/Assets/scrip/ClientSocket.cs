using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace YWSNet
{

    /// <summary>
    /// Socket 客户端 通信 代码 
    /// </summary>
    public class ClientSocket
    {
        private static byte[] result = new byte[1024];
        private static Socket clientSocket;

        // 是否已经连接的标识
        public bool IsConnected = false;

        /// <summary>
        /// 构造函数初始化 客户端
        /// </summary>
        public ClientSocket()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
        }
       
        /// <summary>
        /// 连接指定IP 和端口的服务器 
        /// </summary>
        /// <param name="ip">ip 地址</param>
        /// <param name="port"> 端口号</param>
        public  void ConnectServer(string ip, int port)
        {
            IPAddress mIp = IPAddress.Parse(ip);
            IPEndPoint ip_end_point = new IPEndPoint(mIp,port);

            try
            {
                clientSocket.Connect(ip_end_point);
                IsConnected = true;
                Debug.Log("连接成功");
            }
            catch (System.Exception)
            {
                IsConnected = false;
                Debug.Log("连接失败");
                return;
            }

            int receiveLength = clientSocket.Receive(result);
            ByteBuffer buffer = new ByteBuffer(result);
            int len = buffer.ReadShort();
            string data = buffer.ReadString();
            Debug.Log("服务器返回 数据"+data);
        }

        /// <summary>
        /// 发送数据给 服务器
        /// </summary>
        /// <param name="data"></param>
         public  void SendMessage(string data)
        {
            if (IsConnected == false)
                return;
            try
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteString(data);
                clientSocket.Send(WriteMessage(buffer.ToBytes())); 

            }
            catch (System.Exception)
            {
                IsConnected = false;
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }

        }

        /// <summary>
        /// 数据转换，网络发送需要2 部分数据 ，一部分是长度，另一部分 是 主体数据
        /// </summary>
        /// <param name="message"> </param>
        /// <returns></returns>
       private   static  byte [] WriteMessage( byte [] message)
        {
            MemoryStream ms = null;
            using ( ms  = new MemoryStream())
            {
                ms.Position = 0;
                BinaryWriter writer = new BinaryWriter(ms);
                ushort msglen = (ushort)message.Length;
                writer.Write(msglen);
                writer.Write(message);
                writer.Flush();
                return ms.ToArray();
            }
        }
    }
}
