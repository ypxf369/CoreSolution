using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CoreSolution.RabbitMQ.MessageSerializer
{
    /// <summary>
    /// 消息序列化
    /// </summary>
    public class MessageSerializer : IMessageSerializer
    {
        /// <summary>
        /// 反序列化消息。
        /// </summary>
        /// <typeparam name="T">消息的类型</typeparam>
        /// <param name="bytes">bytes</param>
        /// <returns></returns>
        public T Deserializer<T>(byte[] bytes) where T : class, new()
        {
            //using (var ms = new MemoryStream(bytes))
            //{
            //    var formatter = new BinaryFormatter();
            //    return formatter.Deserialize(ms) as T;
            //}
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
        }

        /// <summary>
        /// 序列化成bytes
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="message">消息实例</param>
        /// <returns></returns>
        public byte[] SerializerBytes<T>(T message) where T : class, new()
        {
            //using (var ms = new MemoryStream())
            //{
            //    var formatter = new BinaryFormatter();
            //    formatter.Serialize(ms, message);
            //    return ms.GetBuffer();
            //}
            string objStr = JsonConvert.SerializeObject(message);
            return Encoding.UTF8.GetBytes(objStr);
        }

        /// <summary>
        /// 将消息序列化为字符串
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="message">消息实例</param>
        /// <returns></returns>
        public string SerializerString<T>(T message) where T : class, new()
        {
            return JsonConvert.SerializeObject(message);
        }
    }
}
