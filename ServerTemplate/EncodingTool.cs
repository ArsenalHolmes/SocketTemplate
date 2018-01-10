using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTemplate
{
    class EncodingTool
    {
        public static byte[] EncodePacket(byte[] arr)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(arr.Length);
                    bw.Write(arr);
                    return ms.ToArray();
                }
            }
        }

        public static byte[] DecodePacket(ref List<byte> list)
        {
            if (list.Count < 4) return null;
            using (MemoryStream ms = new MemoryStream(list.ToArray()))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    int len = br.ReadInt32();

                    int dataRemainLength = (int)(ms.Length - ms.Position);
                    //数据长度不够包头约定的长度 不能构成一个完整的消息
                    if (len > dataRemainLength) return null;

                    //throw new Exception("数据长度不够包头约定的长度 不能构成一个完整的消息");

                    byte[] data = br.ReadBytes(len);
                    //更新一下数据缓存
                    list.Clear();
                    list.AddRange(br.ReadBytes(dataRemainLength));

                    return data;
                }
            }
        }
    }
}
