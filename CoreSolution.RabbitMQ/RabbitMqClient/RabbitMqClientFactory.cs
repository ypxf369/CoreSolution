using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.RabbitMQ.Config;
using Microsoft.CodeAnalysis;
using RabbitMQ.Client;

namespace CoreSolution.RabbitMQ.RabbitMqClient
{
    /// <summary>
    /// RabbitMqClient创建工厂
    /// </summary>
    public class RabbitMqClientFactory
    {
        public static IRabbitMqClient CreateRabbitMqClientInstance()
        {
            var rabbitMqClientContext = new RabbitMqClientContext
            {
                ListenQueueName = MqConfigFactory.CreateConfigInstance().MqListenQueueName,
                InstanceCode = Guid.NewGuid().ToString()
            };

            RabbitMqClient.Instance = new RabbitMqClient
            {
                Context = rabbitMqClientContext
            };
            return RabbitMqClient.Instance;
        }

        /// <summary>
        /// 创建一个IConnection
        /// </summary>
        /// <returns></returns>
        internal static IConnection CreateConnection()
        {
            var mqConfig = MqConfigFactory.CreateConfigInstance();//获取MQ配置

            const ushort heartBeat = 60;
            var factory = new ConnectionFactory
            {
                HostName = mqConfig.MqHost,
                UserName = mqConfig.MqUserName,
                Password = mqConfig.MqPassword,
                RequestedHeartbeat = heartBeat,//心跳超时事件
                AutomaticRecoveryEnabled = true//自动重连
            };
            return factory.CreateConnection();//创建连接对象
        }

        /// <summary>
        /// 创建一个IModel
        /// </summary>
        /// <param name="connection">IConnection</param>
        /// <returns></returns>
        internal static IModel CreateModel(IConnection connection)
        {
            return connection.CreateModel();//创建通道
        }
    }
}
