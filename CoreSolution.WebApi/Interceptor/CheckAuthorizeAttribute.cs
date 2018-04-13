using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoreSolution.Tools.WebResult;
using CoreSolution.WebApi.Manager;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreSolution.WebApi.Interceptor
{
    public class CheckAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string _name;
        public CheckAuthorizeAttribute(string name)
        {
            _name = name;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string token = context.HttpContext.Request.Headers["token"];
            if (string.IsNullOrWhiteSpace(token) || await LoginManager.GetUserIdAsync(token) == null)
            {
                context.Result = AjaxHelper.JsonResult(HttpStatusCode.Unauthorized, "未登录");
            }
            //检查当前登录用户是否拥有该权限
            int userId = (await LoginManager.GetUserIdAsync(token)).Value;
            var permissions = await LoginManager.GetCurrentUserPermissionsAsync(userId);
            if (!permissions.Contains(_name))
            {
                context.Result = AjaxHelper.JsonResult(HttpStatusCode.Unauthorized, "未授权");
            }

            /*string token = context.HttpContext.Request.Query["token"];
            long? userId = await LoginManager.GetUserIdAsync(token);
            if (userId == null)
            {
                context.Result = AjaxHelper.JsonResult(HttpStatusCode.Unauthorized, "未登录");

            }*/
            await base.OnActionExecutionAsync(context, next);
        }
    }
}
