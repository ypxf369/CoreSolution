using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Tools.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace CoreSolution.RabbitMQ.Config
{
    /// <summary>
    /// <see cref="CoreSolution.RabbitMQ.Config.MqConfig"/>创建工厂。
    /// </summary>
    public class MqConfigFactory
    {
        /// <summary>
        /// 创建一个MqConfig实例
        /// </summary>
        /// <returns></returns>
        internal static MqConfig CreateConfigInstance()
        {
            return GetConfigFromSetting();
        }

        private static MqConfig GetConfigFromSetting()
        {
            var cfg = new ConfigurationBuilder().Add(new JsonConfigurationSource { Path = "rabbitmqSettings.json", ReloadOnChange = true }).Build();
            var connection = cfg.GetSection("Connection");
            var config = new MqConfig();

            string mqHost = connection.GetSection("Host").Value;
            if (mqHost.IsNullOrWhiteSpace())
            {
                throw new Exception("RabbitMQ地址配置错误");
            }
            config.MqHost = mqHost;

            string mqUserName = connection.GetSection("UserName").Value;
            if (mqUserName.IsNullOrWhiteSpace())
            {
                throw new Exception("RabbitMQ用户名不能为空");
            }
            config.MqUserName = mqUserName;

            string mqPassword = connection.GetSection("Password").Value;
            if (mqPassword.IsNullOrWhiteSpace())
            {
                throw new Exception("RabbitMQ密码不能为空");
            }
            config.MqPassword = mqPassword;

            string mqListenQueueName = cfg.GetSection("DefaultListenQueue:Name").Value;
            if (mqListenQueueName.IsNullOrWhiteSpace())
            {
                throw new Exception("RabbitMQClient 默认侦听的MQ队列名称不能为空");
            }
            config.MqListenQueueName = mqListenQueueName;

            return config;
        }
    }
}
