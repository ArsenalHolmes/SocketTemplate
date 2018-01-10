

using ClientTemplate;

public class ToolClass
{
    public static T GetObjByArr<T>(byte[] arr) where T : class
    {
        return SerializeTool.GetObjByArr<T>(arr);
    }
}