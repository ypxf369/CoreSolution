using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreSolution.Dto;
using CoreSolution.IService;
using CoreSolution.Tools.Extensions;
using CoreSolution.WebApi.Manager;
using log4net;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace CoreSolution.WebApi.Interceptor
{
    public class AuditingFilter : ActionFilterAttribute
    {
        private static readonly ILog Logger = LogManager.GetLogger(Startup.Logger.Name, typeof(AuditingFilter));
        private readonly IAuditLogService _auditLogService;
        public AuditingFilter(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string ip = context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? context.HttpContext.Connection.RemoteIpAddress.ToString();
            int? userId = null;
            //string para = string.Empty;
            string token = context.HttpContext.Request.Headers["token"];
            if (!token.IsNullOrWhiteSpace())
            {
                userId = (await LoginManager.GetUserIdAsync(token)).GetValueOrDefault();
            }
            //foreach (var item in context.ActionArguments)
            //{
            //    /*if (item.Key == "token")
            //    {
            //        userId = (await LoginManager.GetUserIdAsync(item.Value.ToString())).Value;
            //    }*/
            //    para += item.Key + ":" + item.Value + "/";
            //}
            //string controllerName = context.ActionDescriptor.RouteValues["controller"];
            string actionName = context.ActionDescriptor.RouteValues["action"];
            //await Task.Run(() => Logger.Info($"ip为 {ip} 的用户(userId={userId})在 {DateTime.Now} 执行了 {controllerName}/{actionName} 操作，参数是 {para}"));

            var auditLogDto = new AuditLogDto
            {
                UserId = userId,
                ServiceName = context.Controller.ToString(),
                MethodName = actionName,
                Parameters = ConvertArgumentsToJson(context.ActionArguments),
                ExecutionTime = DateTime.Now,
                ClientIpAddress = ip,
                BrowserInfo = context.HttpContext?.Request?.Headers?["User-Agent"],
            };
            await _auditLogService.InsertAsync(auditLogDto);

            await base.OnActionExecutionAsync(context, next);
        }

        private string ConvertArgumentsToJson(IDictionary<string, object> arguments)
        {
            try
            {
                if (arguments.IsNullOrEmpty())
                {
                    return "{}";
                }

                var dictionary = new Dictionary<string, object>();

                foreach (var argument in arguments)
                {
                    dictionary[argument.Key] = argument.Value;
                }
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                };
                return JsonConvert.SerializeObject(dictionary, settings);
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString(), ex);
                return "{}";
            }
        }
    }
}
