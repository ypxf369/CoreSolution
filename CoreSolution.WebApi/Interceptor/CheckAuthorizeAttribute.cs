using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoreSolution.Tools.Extensions;
using CoreSolution.Tools.WebResult;
using CoreSolution.WebApi.Manager;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreSolution.WebApi.Interceptor
{
    public class CheckAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string _permissionName;
        private readonly string _roleName;
        public CheckAuthorizeAttribute(string roleName, string permissionName = null)
        {
            _permissionName = permissionName;
            _roleName = roleName;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string token = context.HttpContext.Request.Headers["token"];
            if (string.IsNullOrWhiteSpace(token) || await LoginManager.GetUserIdAsync(token) == null)
            {
                context.Result = AjaxHelper.JsonResult(HttpStatusCode.Unauthorized, "未登录");
                return;
            }
            //检查当前登录用户是否拥有该权限
            var userId = (await LoginManager.GetUserIdAsync(token)).GetValueOrDefault();
            var roles = await LoginManager.GetCurrentUserRolesAsync(userId);
            if (!_roleName.IsNullOrWhiteSpace() && !roles.Contains(_roleName))
            {
                context.Result = AjaxHelper.JsonResult(HttpStatusCode.Unauthorized, "未授权");
                return;
            }
            else if(!roles.Contains("Admin"))
            {
                var permissions = await LoginManager.GetCurrentUserPermissionsAsync(userId);
                if (!permissions.Contains(_permissionName))
                {
                    context.Result = AjaxHelper.JsonResult(HttpStatusCode.Unauthorized, "未授权");
                    return;
                }
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
