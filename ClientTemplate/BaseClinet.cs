using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.InteropServices;

namespace ClientTemplate
{
    
    public abstract class BaseClinet
    {
        public virtual string ip { get { return "127.0.0.1"; } }//IP
        public virtual int port { get { return 54321; } }//端口
        
        public virtual bool isHeart { get { return true; } }//是否心跳检测
        public virtual int HeartTime { get { return 5000; } }//心跳线程沉睡时间

        byte[] msgArr = new byte[1024];
        bool isKill = false;//连接是否断开

        public abstract BaseUnDataPack dataPack { get; }
        public BaseUnDataPack DataPack { get; private set; }
        public abstract ISocketEvent socketEvent { get; }
        public ISocketEvent SocketEvent { get; private set; }

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="error"></param>
        public abstract void PrintMessage(string error);

        protected Thread t;//心跳线程
        public Socket client;

        public BaseClinet()
        {
            try
            {
                DataPack = dataPack;//给消息解析器赋值
                SocketEvent = socketEvent;//给Socket事件赋值
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//初始化
                client.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), ConnectAsyn, client);//开始异步连接
            }
            catch (Exception e)
            {
                PrintMessage(e.Message+e.TargetSite+e.StackTrace);
            }
        }

        /// <summary>
        /// 开始心跳检测线程
        /// </summary>
        void BeginHeadCheckThread()
        {
            if (isHeart)
            {
                t = new Thread(HeartThread);
                t.IsBackground = true;
                t.Start();
            }
        }

        /// <summary>
        /// 连接异步回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void ConnectAsyn(IAsyncResult ar)
        {
            try
            {
                client.EndConnect(ar);
                if (SocketEvent != null) SocketEvent.ConnectEvent();//调用连接成功事件
                client.BeginReceive(msgArr, 0, 1024, SocketFlags.None, ReceiveAsyn, client);//开始异步消息接收
                BeginHeadCheckThread();//开启心跳线程
            }
            catch (Exception e)
            {
                PrintMessage(e.Message + e.TargetSite + e.StackTrace);
            }
        }

        /// <summary>
        /// 接收消息异步回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveAsyn(IAsyncResult ar)
        {
            if (isKill) return;
            try
            {
                int count = client.EndReceive(ar);//得到消息长度
                if (count == 0) { ClientClose(); return; }//长度为0.断开连接
                if (SocketEvent != null) SocketEvent.ReceiveEvent(count);//出发接收消息函数
                byte[] newMsg = new byte[count];
                Buffer.BlockCopy(msgArr, 0, newMsg, 0, count);//讲接收到的消息。根据长度复制到新的数组中
                DataPack.ReceiveDataArr(newMsg);//新数组传入到消息解析器中
            }
            catch (Exception e)
            {
                PrintMessage(e.Message + e.TargetSite + e.StackTrace);
                ClientClose();//断开连接
            }
            finally
            {
                //只要不断开连接。就会再次调用异步连接函数
                if (!isKill) client.BeginReceive(msgArr, 0, 1024, SocketFlags.None, ReceiveAsyn, client);
            }
        }

        /// <summary>
        /// 发送消息，参数为Obj
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SendMsg(object obj)
        {
            //序列化
            if (isKill) return false;
            byte[] arr =SerializeTool.GetArrByObj(obj);//把Obj序列号成字节数组
            return SendMsg(arr);
        }

        /// <summary>
        /// 发送消息。参数为字节数组    不包含长度的字节数组  返回值为消息发送成功或失败
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SendMsg(byte[] msg)
        {
            if (isKill) return false;
            try
            {
                byte[] newArr = EncodingTool.EncodePacket(msg);//给数组填充前4位，内容为数组长度
                client.BeginSend(newArr, 0, newArr.Length, SocketFlags.None, SendAsyn, client);//异步发送消息
                return true;
            }
            catch (Exception e)
            {
                PrintMessage(e.Message+e.TargetSite+e.StackTrace);
                ClientClose();
                return false;
            }

        }

        private void SendAsyn(IAsyncResult ar)
        {
            if (isKill) return;
            try
            {
                int count = client.EndSend(ar);
                if (SocketEvent != null) SocketEvent.SendEvent(count);//调用发送消息完成后的事件
            }
            catch (Exception e)
            {
                PrintMessage(e.Message+e.TargetSite+e.StackTrace);
                ClientClose();
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void ClientClose()
        {
            if (isKill) return;
            try
            {
                isKill = true;
                if (SocketEvent != null) SocketEvent.ClientCloseEvent();//断开连接事件
                client.Close();//断开连接
            }
            catch (Exception e)
            {
                PrintMessage(e.Message+e.TargetSite+e.StackTrace);
            }
        }

        /// <summary>
        /// 心跳检测线程  线程沉睡N秒。发送一个H字符
        /// </summary>
        private void HeartThread()
        {
            while (true)
            {
                Thread.Sleep(HeartTime);
                try
                {
                    SendMsg("h");
                }
                catch (Exception e)
                {
                    PrintMessage(e.Message+e.TargetSite+e.StackTrace);
                    ClientClose();
                }
            }
        }

    }
}
