using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ServerTemplate;

namespace testServer
{
    class server : ServerTemplate.BaseServer
    {
        public override bool isHeart => true;
        public override BaseClient ClientConnect(Socket s)
        {
            return new Client(s);
        }

        public override void PrintMessage(string error)
        {
            Console.WriteLine(error);
        }

        public BaseClient getbase()
        {
            return clientList[0];
        }
    }
}
