using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using SocketToolClass;
using System.Threading;
using System.Runtime.InteropServices;

namespace ClientTemplate
{
    
    public abstract class BaseClinet
    {
        public virtual string ip { get { return "127.0.0.1"; } }
        public virtual int port { get { return 54321; } }
        public virtual bool isHeart { get { return true; } }
        public virtual int HeartTime { get { return 5; } }

        byte[] msgArr = new byte[1024];
        bool isKill = false;

        public BaseUnDataPack DataPack;

        public ISocketEvent SocketEvent;
        public virtual void InitInterface() { }
        public abstract void InitDataPack();

        public abstract void PrintMessage(string error);

        public Socket client;
        public BaseClinet()
        {
            try
            {
                InitDataPack();
                InitInterface();
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                client.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), ConnectAsyn, client);
                if (isHeart)
                {
                    Thread t = new Thread(HeartThread);
                    t.Start();
                }
            }
            catch (Exception e)
            {
                PrintMessage(e.Message+e.TargetSite+e.StackTrace);
            }
        }

        private void ConnectAsyn(IAsyncResult ar)
        {
            try
            {
                client.EndConnect(ar);
                if (SocketEvent != null) SocketEvent.ConnectEvent();
                client.BeginReceive(msgArr, 0, 1024, SocketFlags.None, ReceiveAsyn, client);
            }
            catch (Exception e)
            {
                PrintMessage(e.Message + e.TargetSite + e.StackTrace);
            }
        }

        private void ReceiveAsyn(IAsyncResult ar)
        {
            if (isKill) return;
            try
            {
                int count = client.EndReceive(ar);
                if (count == 0) { ClientClose(); return; }
                if (SocketEvent != null) SocketEvent.ReceiveEvent(count);
                byte[] newMsg = new byte[count];
                Buffer.BlockCopy(msgArr, 0, newMsg, 0, count);
                DataPack.ReceiveDataArr(newMsg);
            }
            catch (Exception e)
            {
                PrintMessage(e.Message + e.TargetSite + e.StackTrace);
                ClientClose();
            }
            finally
            {
                if (!isKill) client.BeginReceive(msgArr, 0, 1024, SocketFlags.None, ReceiveAsyn, client); ;
                
            }
        }

        public bool SendMsg(object obj)
        {
            //序列化
            if (isKill) return false;
            byte[] arr =SerializeTool.GetArrByObj(obj);
            return SendMsg(arr);
        }

        public bool SendMsg(byte[] msg)
        {
            if (isKill) return false;
            //根据数组加长度
            try
            {
                byte[] newArr = EncodingTool.EncodePacket(msg);
                client.BeginSend(newArr, 0, newArr.Length, SocketFlags.None, SendAsyn, client);
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
                if (SocketEvent != null) SocketEvent.SendEvent(count);
            }
            catch (Exception e)
            {
                PrintMessage(e.Message+e.TargetSite+e.StackTrace);
                ClientClose();
            }
        }

        private void ClientClose()
        {
            if (isKill) return;
            try
            {
                isKill = true;
                if (SocketEvent != null) SocketEvent.ClientCloseEvent();//断开连接
                client.Close();
            }
            catch (Exception e)
            {
                PrintMessage(e.Message+e.TargetSite+e.StackTrace);
            }
        }

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
