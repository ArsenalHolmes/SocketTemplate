using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerTemplate
{
    public abstract class BaseServer
    {
        public virtual string ip { get { return "127.0.0.1"; } }
        public virtual int port { get { return 54321; } }
        public virtual int maxCount { get { return 5; } }

        public virtual bool isHeart { get { return true; } }
        public virtual int HeartTime { get { return 5000; } }

        public abstract void PrintMessage(string error);

        Socket server;
        public BaseServer()
        {
            try
            {
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
                server.Listen(maxCount);

                server.BeginAccept(AcceptAsyn, server);

                if (isHeart)
                {
                    Thread t = new Thread(HeartThread);
                    t.IsBackground = true;
                    t.Start();
                }
            }
            catch (Exception e)
            {
                PrintMessage(e.Message+e.TargetSite+e.StackTrace);
            }

        }

        public List<BaseClient> clientList = new List<BaseClient>();

        private void AcceptAsyn(IAsyncResult ar)
        {
            try
            {
                Socket client = server.EndAccept(ar);
                BaseClient bc = ClientConnect(client);
                clientList.Add(bc);
            }
            catch (Exception e)
            {
                PrintMessage(e.Message+e.TargetSite+e.StackTrace);
            }
            finally
            {
                server.BeginAccept(AcceptAsyn, server);
            }
        }

        public abstract BaseClient ClientConnect(Socket s);

        private void HeartThread()
        {
            while (true)
            {
                Thread.Sleep(HeartTime);
                Console.WriteLine("发送心跳"+HeartTime);
                List<BaseClient> temp = new List<BaseClient>();
                foreach (var item in clientList)
                {
                    try
                    {
                        if (!item.SendMsg("h"))
                        {
                            temp.Add(item);
                        }
                    }
                    catch (Exception e)
                    {
                        temp.Add(item);
                        PrintMessage(e.Message+e.TargetSite+e.StackTrace);
                    }
                }
                foreach (var item in temp)
                {
                    clientList.Remove(item);
                }
            }
        }
    }
}
