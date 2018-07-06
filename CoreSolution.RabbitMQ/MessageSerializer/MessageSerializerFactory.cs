using System;
using System.Collections.Generic;
using System.Text;

namespace CoreSolution.RabbitMQ.MessageSerializer
{
    /// <summary>
    /// IMessageSerializer 创建工厂
    /// </summary>
    public class MessageSerializerFactory
    {
        public static IMessageSerializer CreateMessageSerializerInstance()
        {
            return new MessageSerializer();
        }
    }
}
