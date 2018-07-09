using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.RabbitMQ.MessageSerializer;

namespace CoreSolution.RabbitMQ.EventMessage
{
    /// <summary>
    /// 表示一个事件消息。
    /// </summary>
    public sealed class EventMessage
    {
        /// <summary>
        /// 消息的标记码
        /// </summary>
        public string EventMessageMarkcode { get; set; }

        /// <summary>
        /// 消息序列化字节流。
        /// </summary>
        public byte[] EventMessageBytes { get; set; }

        /// <summary>
        /// 消息创建时间。
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        internal static EventMessageResult BuildEventMessageResult(byte[] bytes)
        {
            var eventMessage =
                MessageSerializerFactory.CreateMessageSerializerInstance().Deserializer<EventMessage>(bytes);
            var result = new EventMessageResult
            {
                EventMessageBytes = eventMessage,
                MessageBytes = eventMessage.EventMessageBytes
            };
            return result;
        }
    }
}
