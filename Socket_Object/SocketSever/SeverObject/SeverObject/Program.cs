using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using YWSNet;
using System.IO;

namespace SeverObject
{
    class Program
    {
      

         private static byte[] result = new byte[1024];
        private const int port = 8088;
        private static string IpStr = "192.168.1.240";
        private static Socket severSocket;


        static void Main(string[] args)
        {
            IPAddress Ip = IPAddress.Parse(IpStr);
            IPEndPoint ip_end_point = new IPEndPoint(Ip, port);
            // 设置 相关的属性
            severSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // 绑定 ip 和 端口
            severSocket.Bind(ip_end_point);
            // 设置最长的 连接 请求队列长度
            severSocket.Listen(10);
            Console.WriteLine("启动监听{0}成功", severSocket.LocalEndPoint.ToString());
            //在新线程 中监听客户端的 链接
            Thread thred = new Thread(ClientConnectListen);
            thred.Start();
            Console.ReadLine();
        }

        /// <summary>
        /// 客户端连接请求监听
        /// </summary>
        private static void ClientConnectListen()
        {
            while (true)
            {
                // 为新的 客户端连接创建一个socket 对象
                Socket clientSocket = severSocket.Accept();
                Console.WriteLine("客户端成功连接" + clientSocket.RemoteEndPoint.ToString());
                // 向连接的客户端发送连接 成功的 数据
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteString("Connected Server");
                clientSocket.Send(WriteMessage(buffer.ToBytes()));
                //每个 客户端连接 创建一个线程来接受该客户端 发送来的数据
                Thread thread = new Thread(RecieveMessage);
                thread.Start(clientSocket); 
            }
        }



        /// <summary>
        /// 数据转换，网络发送需要两部分数据，一是数据长度，二是主体数据
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static byte[] WriteMessage(byte[] message)
        {
            MemoryStream ms = null;
            using (ms = new MemoryStream())
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

        /// <summary>
        ///  接收指定 客户端 发送的 Socket数据
        /// </summary>
        /// <param name="clientSocket"></param>
        private static void RecieveMessage(object clientSocket)
        {
            Socket mClientSocket = (Socket)clientSocket;
            while (true)
            {
                try
                {
                    int receiveNumber = mClientSocket.Receive(result);  // j接受 传输的结果
                    Console.WriteLine("接收客户端数据{0}, 长度为{1}", mClientSocket.RemoteEndPoint.ToString(), receiveNumber);
                    ByteBuffer buff = new ByteBuffer(result);
                    //数据长度
                    int len = buff.ReadShort();
                    //数据内容
                   
                    string data = buff.ReadString();
                    Console.WriteLine("数据内容：{0}", data);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message+"客户端接受数据异常");
                    mClientSocket.Shutdown(SocketShutdown.Both);
                    mClientSocket.Close();
                    break;
                }
            }
        }
    }
    
}
