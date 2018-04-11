using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreSolution.WebApi.Manager;
using log4net;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreSolution.WebApi.Interceptor
{
    public class AuditingFilter : ActionFilterAttribute//IAsyncActionFilter
    {
        private static readonly ILog Logger = LogManager.GetLogger(Startup.Logger.Name, typeof(AuditingFilter));
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //通过Filter记录访问日志：ip为***、用户id为***（如果有）的用户在****执行了***操作，参数是***
            string ip = context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                        context.HttpContext.Connection.RemoteIpAddress.ToString();
            long? userId = null;
            string para = string.Empty;
            foreach (var item in context.ActionArguments)
            {
                if (item.Key == "token")
                {
                    userId = (await LoginManager.GetUserIdAsync(item.Value.ToString())).Value;
                }
                para += item.Key + ":" + item.Value + "/";
            }
            string controllerName = context.ActionDescriptor.RouteValues["controller"];
            string actionName = context.ActionDescriptor.RouteValues["action"];
            await Task.Run(() => Logger.Info($"ip为 {ip} 的用户(userId={userId})在 {DateTime.Now} 执行了 {controllerName}/{actionName} 操作，参数是 {para}"));
            await base.OnActionExecutionAsync(context, next);
        }
    }
}
