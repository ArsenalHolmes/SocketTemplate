using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTemplate
{
    /// <summary>
    /// 消息解析器
    /// </summary>
    public abstract class BaseUnDataPack
    {
        ///心跳包的字节数组长度为25。可以屏蔽掉。

        /// <summary>
        /// 存储消息字节数组列表
        /// </summary>
        List<byte> msgList = new List<byte>();

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="msgArr"></param>
        public virtual void ReceiveDataArr(byte[] msgArr)
        {
            msgList.AddRange(msgArr);//把字节数组填充到列表中
            byte[] newArr = EncodingTool.DecodePacket(ref msgList);//如果长度够一条消息。把那条消息的完整字节返回
            if (newArr != null) ProcessData(newArr);//如果返回字节数组。调用解析函数
        }

        /// <summary>
        /// 消息解析函数
        /// </summary>
        /// <param name="msgArr"></param>
        public abstract void ProcessData(byte[] msgArr);
    }
}
