using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.RabbitMQ.EventMessage;

namespace CoreSolution.RabbitMQ.RabbitMqClient
{
    /// <summary>
    /// RabbitMq client 接口
    /// </summary>
    public interface IRabbitMqClient : IDisposable
    {
        /// <summary>
        /// RabbitMqClient 数据上下文
        /// </summary>
        RabbitMqClientContext Context { get; set; }

        /// <summary>
        /// 消息被本地事件激活。通过绑定该事件来获取消息队列推送过来的消息。只能绑定一个事件处理程序。
        /// </summary>
        event EventMessageFactory.ActionEvent ActionEventMessage;

        /// <summary>
        /// 触发一个事件，向队列推送一个事件消息。
        /// </summary>
        /// <param name="eventMessage">消息类型实例</param>
        /// <param name="queue">队列名称</param>
        /// <param name="exchange">exchange名称</param>
        void TriggerEventMessage(EventMessage.EventMessage eventMessage, string queue, string exchange = "");

        /// <summary>
        /// 开始消息队列的默认监听。
        /// <param name="queue">队列名称</param>
        /// </summary>
        void OnListening(string queue = null);
    }
}
