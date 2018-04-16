using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoreSolution.Domain.Enum;
using CoreSolution.IService;
using CoreSolution.Tools.WebResult;
using CoreSolution.WebApi.Manager;
using CoreSolution.WebApi.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreSolution.WebApi.Controllers
{
    /// <summary>
    /// 用户操作控制器
    /// </summary>
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private static string LoginErrorTimes_Prefix = "LoginErrorTimes.User.";
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;


        public UserController(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }

        /// <summary>
        /// 登录。200 成功，返回token，400 userNameOrEmailOrPhoneNum、password参数不能为空，403 登录错误次数太多，账户已经锁定15分钟，404 用户名不存在，406 用户名或密码错误，502 未知的result
        /// </summary>
        /// <param name="inputLoginModel">用户登录参数model</param>
        /// <returns></returns>
        [Route("login")]
        [HttpPost]
        public async Task<JsonResult> Login([FromBody] InputLoginModel inputLoginModel)
        {
            return await DoLoginAsync(inputLoginModel.UserNameOrEmailOrPhone, inputLoginModel.Password);
        }

        private async Task<JsonResult> DoLoginAsync(string userNameOrEmailOrPhone, string password)
        {
            if (string.IsNullOrWhiteSpace(userNameOrEmailOrPhone))
            {
                return AjaxHelper.JsonResult(HttpStatusCode.BadRequest, "用户名不能为空");//400
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                return AjaxHelper.JsonResult(HttpStatusCode.BadRequest, "password参数不能为空");
            }

            //登录错误次数过多要锁定账户
            LoginResults result = await _userService.CheckUserPasswordAsync(userNameOrEmailOrPhone, password);
            if (result == LoginResults.Success)
            {
                var user = await _userService.GetUserByUserNameOrEmailOrPhoneAsync(userNameOrEmailOrPhone);
                string token = Guid.NewGuid().ToString();
                await LoginManager.LoginAsync(token, user.Id);
                //获取当前用户的权限
                var permissions = await _permissionService.GetPermissionsByUserIdAsync(user.Id);
                await LoginManager.SaveCurrentUserPermissionsAsync(permissions.Select(i => i.RoleName).ToArray(), user.Id);
                LoginManager.ResetErrorLogin(LoginErrorTimes_Prefix, userNameOrEmailOrPhone);
                return AjaxHelper.JsonResult(HttpStatusCode.OK, "成功", token);//200
            }
            if (result == LoginResults.NotExist)
            {
                return AjaxHelper.JsonResult(HttpStatusCode.NotFound, "用户不存在");//404
            }
            if (result == LoginResults.PassWordError)
            {
                await LoginManager.IncreaseErrorLoginAsync(LoginErrorTimes_Prefix, userNameOrEmailOrPhone);
                return AjaxHelper.JsonResult(HttpStatusCode.NotAcceptable, "用户名或密码错误");//406
            }
            return AjaxHelper.JsonResult(HttpStatusCode.BadGateway, "未知的result:" + result);//502
        }

    }
}