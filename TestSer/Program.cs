using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.IO;

namespace TestSer
{
    class Program
    {
        static void Main(string[] args)
        {
            //byte[] msg = SerializeTool.GetArrByObj("q");
            //Console.WriteLine(msg.Length);

            byte[] msg2 = Encoding.UTF8.GetBytes("q");
            Console.WriteLine(msg2[0]);

            object obj = "q";

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            ms.Position = 0;
            bf.Serialize(ms, obj);
            byte[] s = ms.ToArray();
            foreach (var item in s)
            {
                Console.WriteLine(item);
            }
            //Console.WriteLine(s.Length);
        }
    }
}
