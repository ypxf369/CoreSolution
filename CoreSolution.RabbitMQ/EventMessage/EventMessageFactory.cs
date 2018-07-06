using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.RabbitMQ.MessageSerializer;

namespace CoreSolution.RabbitMQ.EventMessage
{
    /// <summary>
    /// 创建EventMessage实例。
    /// </summary>
    public class EventMessageFactory
    {
        public static EventMessage CreageEventMessageInstance<T>(T originObject, string eventMessageMarkcode) where T : class, new()
        {
            var result = new EventMessage
            {
                CreateDateTime = DateTime.Now,
                EventMessageMarkcode = eventMessageMarkcode
            };

            var bytes = MessageSerializerFactory.CreateMessageSerializerInstance().SerializerBytes<T>(originObject);
            result.EventMessageBytes = bytes;
            return result;
        }

        /// <summary>
        /// 表示消息到达客户端发起的事件。
        /// </summary>
        /// <param name="result">EventMessageResult 事件消息对象</param>
        public delegate void ActionEvent(EventMessageResult result);
    }
}
