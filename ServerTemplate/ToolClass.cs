using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketToolClass;


public class ToolClass
{
    public static T GetObjByArr<T>(byte[] arr) where T : class
    {
        return SerializeTool.GetObjByArr<T>(arr);
    }
}
