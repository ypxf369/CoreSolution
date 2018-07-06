using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoreSolution.RabbitMQ.EventMessage;
using CoreSolution.RabbitMQ.MessageSerializer;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CoreSolution.RabbitMQ.RabbitMqClient
{
    /// <summary>
    /// 表示RabbitMq客户端组件。
    /// </summary>
    public class RabbitMqClient : IRabbitMqClient
    {
        #region Static fields

        /// <summary>
        /// 客户端实例私有字段。
        /// </summary>
        private static IRabbitMqClient _instanceClient;

        public static IRabbitMqClient Instance
        {
            get
            {
                if (_instanceClient == null)
                {
                    RabbitMqClientFactory.CreateRabbitMqClientInstance();
                }
                return _instanceClient;
            }

            internal set { _instanceClient = value; }
        }
        #endregion

        #region Instance fields

        /// <summary>
        /// RabbitMqClient数据上下文。
        /// </summary>
        public RabbitMqClientContext Context { get; set; }

        /// <summary>
        /// 事件激活委托实例
        /// </summary>
        private EventMessageFactory.ActionEvent _actionMessage;

        public event EventMessageFactory.ActionEvent ActionEventMessage
        {
            add
            {
                if (_actionMessage == null)
                {
                    _actionMessage += value;
                }
            }
            remove
            {
                if (_actionMessage != null)
                {
                    _actionMessage -= value;
                }
            }
        }

        #endregion

        #region send method

        /// <summary>
        /// 出发一个事件且将事件打包成消息发送到远程队列
        /// </summary>
        /// <param name="eventMessage">发送的消息实例。</param>
        /// <param name="queue">队列名称</param>
        /// <param name="exChange">RabbitMq的Exchange名称</param>
        public void TriggerEventMessage(EventMessage.EventMessage eventMessage, string queue, string exChange)
        {
            Context.SendConnection = RabbitMqClientFactory.CreateConnection();//获取连接
            using (Context.SendConnection)
            {
                Context.SendChannel = RabbitMqClientFactory.CreateModel(Context.SendConnection);//获取通道

                const byte deliveryMode = 2;
                using (Context.SendChannel)
                {
                    var messageSerializer = MessageSerializerFactory.CreateMessageSerializerInstance();//发序列化消息
                    var properties = Context.SendChannel.CreateBasicProperties();
                    properties.DeliveryMode = deliveryMode;//表示持久化消息
                    //声明队列
                    Context.SendChannel.QueueDeclare(queue, deliveryMode == 2, false, false, null);
                    Context.SendChannel.BasicPublish(exChange, queue, properties, messageSerializer.SerializerBytes(eventMessage));
                }
            }
        }

        #endregion

        #region receive method

        public void OnListening(string queue)
        {
            Task.Run(() => ListenInit(queue));
        }

        /// <summary>
        /// 侦听初始化
        /// </summary>
        private void ListenInit(string queue)
        {
            try
            {
                Context.ListenConnection = RabbitMqClientFactory.CreateConnection();//获取连接

                Context.ListenConnection.ConnectionShutdown += (o, e) =>
                {
                    //ESLog.Info("connection shutdown:" + e.ReplyText);
                    //todo:记录日志
                };

                Context.ListenChannel = RabbitMqClientFactory.CreateModel(Context.ListenConnection);//获取通道

                var consumer = new EventingBasicConsumer(Context.ListenChannel);//创建事件驱动的消费者类型
                consumer.Received += ConsumerReveived;

                Context.ListenChannel.BasicQos(0, 1, false);//一次只获取一个消息进行消费
                Context.ListenChannel.BasicConsume(queue ?? Context.ListenQueueName, false, consumer);
            }
            catch (Exception e)
            {
                //todo:记录日志
                throw;
            }
        }

        private void ConsumerReveived(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                var result = EventMessage.EventMessage.BuildEventMessageResult(e.Body);//获取消息返回对象
                _actionMessage?.Invoke(result);//触发外部侦听事件

                if (!result.IsOperationOk)
                {
                    //未能消费此消息，重新放入队列头
                    Context.ListenChannel.BasicReject(e.DeliveryTag, true);
                }
                else if (!Context.ListenChannel.IsClosed)
                {
                    Context.ListenChannel.BasicAck(e.DeliveryTag, false);
                }
            }
            catch (Exception exception)
            {
                //ESLog.Error(exception.Message,exception);
                //todo:记录日志
                throw;
            }
        }

        #endregion

        #region IDispose

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            if (Context.SendConnection == null) return;

            if (Context.SendConnection.IsOpen)
                Context.SendConnection.Close();

            Context.SendConnection.Dispose();

            if (Context.SendChannel == null) return;
            if (Context.SendChannel.IsOpen)
                Context.SendChannel.Close();

            Context.SendChannel.Dispose();
        }

        #endregion
    }
}
