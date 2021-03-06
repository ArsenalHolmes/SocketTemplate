﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ClientTemplate
{
    class SerializeTool
    {
        /// <summary>
        /// 把Obj序列化成字节数组
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] GetArrByObj(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            ms.Position = 0;
            bf.Serialize(ms, obj);
            byte[] s = ms.ToArray();
            return s;
        }

        /// <summary>
        /// 把字节数组反序列化成泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static T GetObjByArr<T>(byte[] arr) where T : class
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Binder = new UBinder();
            MemoryStream stream = new MemoryStream(arr);
            stream.Position = 0;
            object obj = formatter.Deserialize(stream);
            T t = (T)obj;
            return t;
        }
    }
    public class UBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Assembly ass = Assembly.GetExecutingAssembly();//GetEntryAssembly();
            return ass.GetType(typeName);
        }
    }
}
