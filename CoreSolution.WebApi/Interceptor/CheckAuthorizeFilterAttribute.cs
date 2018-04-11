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
    public class CheckAuthorizeFilterAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string token = context.HttpContext.Request.Query["token"];
            long? userId = await LoginManager.GetUserIdAsync(token);
            if (userId == null)
            {
                context.Result = AjaxHelper.JsonResult(HttpStatusCode.Unauthorized, "未登录");

            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}
