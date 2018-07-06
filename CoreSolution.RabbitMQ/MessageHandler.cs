using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.RabbitMQ.EventMessage;
using CoreSolution.RabbitMQ.MessageSerializer;
using CoreSolution.Tools.Extensions;
using RabbitMQ.Client;

namespace CoreSolution.RabbitMQ
{
    public class MessageHandler
    {
        private static Action<byte[]> _action;
        /// <summary>
        /// 生产者
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="queue">队列名称</param>
        /// <param name="messageObj">消息对象</param>
        /// <param name="markcode">消息的标记码</param>
        /// <param name="exchange">RabbitMq的Exchange名称</param>
        public static void Producer<T>(string queue, T messageObj, string markcode = null, string exchange = "") where T : class, new()
        {
            var sendMessage = EventMessageFactory.CreageEventMessageInstance(messageObj, markcode);
            RabbitMqClient.RabbitMqClient.Instance.TriggerEventMessage(sendMessage, queue, exchange);
        }

        /// <summary>
        /// 消费者
        /// <param name="action">消费执行动作，byte[]为原始消息bytes</param>
        /// <param name="queue">队列名称(null为默认队列名)</param>
        /// </summary>
        public static void Consumer(Action<byte[]> action, string queue = null)
        {
            _action = action;
            Listening(queue);
        }

        private static void Listening(string queue)
        {
            RabbitMqClient.RabbitMqClient.Instance.ActionEventMessage += MqClientActionEventMessage;
            RabbitMqClient.RabbitMqClient.Instance.OnListening(queue);
        }

        private static void MqClientActionEventMessage(EventMessageResult result)
        {
            try
            {
                var messageBytes = result.MessageBytes;
                _action(messageBytes);
                result.IsOperationOk = true;
            }
            catch (Exception e)
            {
                result.IsOperationOk = false;
                //ESLog.Error(e.Message,e);
                //todo:记录日志
                throw;
            }
        }

        public static T Deserializer<T>(byte[] bytes) where T : class, new()
        {
            return MessageSerializerFactory.CreateMessageSerializerInstance().Deserializer<T>(bytes);
        }
    }
}
