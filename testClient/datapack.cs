using ClientTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testClient
{
    class datapacks : BaseUnDataPack
    {
        public override void ProcessData(byte[] msgArr)
        {
            string str = ToolClass.GetObjByArr<string>(msgArr);
            Console.WriteLine(str);
        }
    }
}
