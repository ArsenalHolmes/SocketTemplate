using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientTemplate;

namespace testClient
{
    class client : BaseClinet
    {
        public override bool isHeart => false;
        public override void InitDataPack()
        {
            DataPack = new datapacks();
        }
        public override void InitInterface()
        {
            SocketEvent = new newInter();
        }

        public override void PrintMessage(string error)
        {
            Console.WriteLine(error);
        }
    }
}
