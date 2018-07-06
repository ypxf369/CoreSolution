using System;
using System.Collections.Generic;
using System.Text;

namespace CoreSolution.RabbitMQ.MessageSerializer
{
    /// <summary>
    /// 消息序列化接口。
    /// </summary>
    public interface IMessageSerializer
    {
        /// <summary>
        /// 序列化消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="message">消息实例</param>
        /// <returns></returns>
        byte[] SerializerBytes<T>(T message) where T : class, new();

        /// <summary>
        /// 序列化消息为字符串。
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="message">消息实例</param>
        /// <returns></returns>
        string SerializerString<T>(T message) where T : class, new();

        /// <summary>
        /// 反序列化消息。
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="bytes">bytes</param>
        /// <returns></returns>
        T Deserializer<T>(byte[] bytes) where T : class, new();
    }
}
