using ClientTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testClient
{
    class newInter : ISocketEvent
    {
        public void ClientCloseEvent()
        {
            Console.WriteLine("断开");
        }

        public void ConnectEvent()
        {
            Console.WriteLine("连接");
        }

        public void ReceiveEvent(int count)
        {
            Console.WriteLine("受到"+count);
        }

        public void SendEvent(int count)
        {
            Console.WriteLine("发送");
        }
    }
}
