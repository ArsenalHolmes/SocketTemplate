using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ServerTemplate
{
    public abstract class BaseClient
    {
        public ISocketEvent SocketEvent;
        public BaseUnDataPack DataPack;
        
        public virtual void InitInterFace() { }
        public abstract void InitDataPack();
        public abstract void PrintMessage(string error);

        bool isKill = false;

        Socket client;
        byte[] msgArr = new byte[1024];

        public BaseClient(Socket client)
        {
            InitInterFace();
            InitDataPack();
            this.client = client;
            client.BeginReceive(msgArr, 0, 1024, SocketFlags.None, ReceiveAsyn, client);
        }



        private void ReceiveAsyn(IAsyncResult ar)
        {
            if (isKill) return;
            try
            {
                int count = client.EndReceive(ar);
                if (count == 0) { ClientClose(); return; }
                if (SocketEvent != null) SocketEvent.ReceiveEvent(count);
                byte[] newMsg=new byte[count];
                Buffer.BlockCopy(msgArr, 0, newMsg, 0, count);
                DataPack.ReceiveDataArr(newMsg);
            }
            catch (Exception e)
            {
                PrintMessage(e.Message+e.TargetSite+e.StackTrace);
                ClientClose();
            }
            finally
            {
                if (!isKill) client.BeginReceive(msgArr, 0, 1024, SocketFlags.None, ReceiveAsyn, client);
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
    }
}
