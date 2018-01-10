using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testClient
{
    class Program
    {
        static void Main(string[] args)
        {
            client c = new client();
            while (true)
            {
                string str = Console.ReadLine();
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("q", "1");
                dic.Add("w", "1");
                dic.Add("e", "1");
                dic.Add("r", "1");
                dic.Add("t", "1");
                c.SendMsg(dic);
            }
        }
    }
}
