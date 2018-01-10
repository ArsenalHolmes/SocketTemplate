using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testServer
{
    class Program
    {
        static void Main(string[] args)
        {
            server s = new server();
            while (true)
            {
                string str = Console.ReadLine();
                s.getbase().SendMsg(str);
            }
        }
    }
}
