using System;
using System.Collections.Generic;

namespace ServerTemplate
{
    public abstract class BaseUnDataPack
    {
        List<byte> msgList = new List<byte>();

        /// <summary>
        /// 是否由主线程调用解析消息的方法
        /// 例如：U3D，必须由主线程调用，设置成true
        /// </summary>
        public abstract bool IsMainThread { get; }

        public virtual void ReceiveDataArr(byte[] msgArr)
        {
            msgList.AddRange(msgArr);
            if (!IsMainThread) Update();
        }

        public virtual void Update()
        {
            byte[] newArr = EncodingTool.DecodePacket(ref msgList);
            if (newArr != null) ProcessData(newArr);
        }

        public abstract void ProcessData(byte[] msgArr);

    }
}
