using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace testServer
{
    class Client : ServerTemplate.BaseClient
    {

        public Client(Socket s):base(s)
        {

        }
        public override void InitDataPack()
        {
            DataPack = new datapack();
        }

        public override void PrintMessage(string error)
        {
            Console.WriteLine(error);
        }
    }
}
