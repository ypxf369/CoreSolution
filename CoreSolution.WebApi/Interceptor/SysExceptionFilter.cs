using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreSolution.WebApi.Interceptor
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class SysExceptionFilter : IAsyncExceptionFilter
    {
        private static readonly ILog Logger = LogManager.GetLogger(Startup.Logger.Name, typeof(SysExceptionFilter));
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var error = context.Exception;
            var message = error.Message;
            var innerException = error.InnerException?.Message;
            var httpContext = context.HttpContext;
            var url = httpContext?.Request.GetUri().ToString();//错误发生地址

            await Task.Run(() => Logger.Error("Url:" + url + ";Message:" + message + "\r\n InnerException:" + innerException, error));
        }
    }
}
