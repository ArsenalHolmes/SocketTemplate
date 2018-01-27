using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testServer
{
    class datapack : ServerTemplate.BaseUnDataPack
    {
        public override void ProcessData(byte[] msgArr)
        {
            if (msgArr.Length>25)
            {
                Dictionary<string, string> dic = ToolClass.GetObjByArr<Dictionary<string, string>>(msgArr);
                foreach (var item in dic)
                {
                    Console.WriteLine(item.Key+"    "+item.Value);
                }
            }
            else
            {
                Console.WriteLine(ToolClass.GetObjByArr<string>(msgArr));
            }
        }
    }
}
