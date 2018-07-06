﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CoreSolution.RabbitMQ.Config
{
    /// <summary>
    /// 消息队列相关配置
    /// </summary>
    public class MqConfig
    {
        /// <summary>
        /// 消息队列的地址
        /// </summary>
        public string MqHost { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string MqUserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string MqPassword { get; set; }

        /// <summary>
        /// 客户端默认监听的队列名称
        /// </summary>
        public string MqListenQueueName { get; set; }
    }
}
