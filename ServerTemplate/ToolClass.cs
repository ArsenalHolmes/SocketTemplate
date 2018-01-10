using ServerTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ToolClass
{
    public static T GetObjByArr<T>(byte[] arr) where T : class
    {
        return SerializeTool.GetObjByArr<T>(arr);
    }
}
