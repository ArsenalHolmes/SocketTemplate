using System;
using System.Collections.Generic;

namespace ServerTemplate
{
    public abstract class BaseUnDataPack
    {
        List<byte> msgList = new List<byte>();
        public virtual void ReceiveDataArr(byte[] msgArr)
        {
            msgList.AddRange(msgArr);
            byte[] newArr = EncodingTool.DecodePacket(ref msgList);
            if (newArr != null) ProcessData(newArr);
        }

        public abstract void ProcessData(byte[] msgArr);

    }
}
