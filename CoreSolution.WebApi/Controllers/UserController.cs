using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using CoreSolution.Domain.Enum;
using CoreSolution.Dto;
using CoreSolution.IService;
using CoreSolution.Tools.Extensions;
using CoreSolution.Tools.WebResult;
using CoreSolution.WebApi.Manager;
using CoreSolution.WebApi.Models;
using CoreSolution.WebApi.Models.User;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CoreSolution.WebApi.Controllers
{
    /// <summary>
    /// 用户操作控制器
    /// </summary>
    [EnableCors("AllowAllOrigin")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private static string LoginErrorTimes_Prefix = "LoginErrorTimes.User.";
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly IRoleService _roleService;


        public UserController(IUserService userService, IRoleService roleService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
            _roleService = roleService;
        }
    
        /// <summary>
        /// 用户注册。200注册成功，400用户名、密码不能为空，302用户名已存在。
        /// </summary>
        /// <param name="inputRegisterModel">用户注册参数model</param>
        /// <returns></returns>
        [Route("register")]
        [HttpPost]
        public async Task<JsonResult> Register([FromBody]InputRegisterModel inputRegisterModel)
        {
            if (inputRegisterModel.UserName.IsNullOrWhiteSpace())
            {
                return AjaxHelper.JsonResult(HttpStatusCode.BadRequest, "用户名不能为空");
            }
            //检查用户名是否重复
            bool isExist = await _userService.CheckUserNameDupAsync(inputRegisterModel.UserName);
            if (isExist)
            {
                return AjaxHelper.JsonResult(HttpStatusCode.Found, "用户名已存在");
            }
            if (inputRegisterModel.Password.IsNullOrWhiteSpace())
            {
                return AjaxHelper.JsonResult(HttpStatusCode.BadRequest, "密码不能为空");
            }
            string salt = new Random().Next(100000, 999999).ToString();
            var userDto = new UserDto
            {
                UserName = inputRegisterModel.UserName,
                RealName = inputRegisterModel.RealName,
                Password = (inputRegisterModel.Password.ToMd5() + salt).ToMd5(),
                Salt = salt
            };
            if (inputRegisterModel.RegisterType == RegisterType.Assign)
            {
                userDto.UserRoles = inputRegisterModel.Roles.Select(i => new UserRoleDto { User = userDto, RoleId = i }).ToList();
                string token = HttpContext.Request.Headers["token"];
                var userId = (await LoginManager.GetUserIdAsync(token)).GetValueOrDefault();
                userDto.CreatorUserId = userId;
            }

            var id = await _userService.InsertAndGetIdAsync(userDto);
            return AjaxHelper.JsonResult(HttpStatusCode.OK, "注册成功", id);
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
                //获取当前用户的角色
                var roles = await _roleService.GetRolesByUserIdAsync(user.Id);
                await LoginManager.SaveCurrentUserRolesAsync(roles.Select(i => i.Name).ToArray(), user.Id);
                //获取当前用户的权限
                var permissions = await _permissionService.GetPermissionsByUserIdAsync(user.Id);
                await LoginManager.SaveCurrentUserPermissionsAsync(permissions.Select(i => i.Name).ToArray(), user.Id);
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

        /// <summary>
        /// 删除用户。200删除成功，404该用户不存在
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        //[CheckAuthorize("User.Delete")]
        public async Task<JsonResult> Delete([FromBody]int userId)
        {
            var user = await _userService.GetAsync(userId);
            if (user == null)
            {
                return AjaxHelper.JsonResult(HttpStatusCode.NotFound, "该用户不存在");
            }
            string token = HttpContext.Request.Headers["token"];
            var currentUserId = (await LoginManager.GetUserIdAsync(token)).GetValueOrDefault();
            user.DeleterUserId = currentUserId;
            user.DeletionTime = DateTime.Now;
            await _userService.DeleteAsync(user);
            return AjaxHelper.JsonResult(HttpStatusCode.OK, "删除成功");
        }

        /// <summary>
        /// 根据Id获取用户。200获取成功
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [Route("getUser")]
        [HttpGet]
        public async Task<JsonResult> GetUserById(int userId)
        {
            var result = await _userService.GetAsync(userId);
            return AjaxHelper.JsonResult(HttpStatusCode.OK, "成功", Mapper.Map<OutputUserModel>(result));
        }

        /// <summary>
        /// 根据参数获取用户。200获取成功，400参数不能为空。
        /// </summary>
        /// <param name="userNameOrEmailOrPhone">用户名或邮箱或手机号</param>
        /// <returns></returns>
        [Route("getUserByParams")]
        [HttpGet]
        public async Task<JsonResult> GetUserByUserNameOrEmailOrPhone(string userNameOrEmailOrPhone)
        {
            if (userNameOrEmailOrPhone.IsNullOrWhiteSpace())
            {
                return AjaxHelper.JsonResult(HttpStatusCode.BadRequest, "参数不能为空");
            }
            var result = await _userService.GetUserByUserNameOrEmailOrPhoneAsync(userNameOrEmailOrPhone);
            return AjaxHelper.JsonResult(HttpStatusCode.OK, "成功", Mapper.Map<OutputUserModel>(result));
        }

        /// <summary>
        /// 分页获取用户列表。200获取成功
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <returns></returns>
        [Route("getUsersPaged")]
        [HttpGet]
        public async Task<JsonResult> GetUsersPaged(int pageIndex = 1, int pageSize = 10)
        {
            var result = await _userService.GetPagedAsync(i => true, i => i.CreationTime, pageIndex, pageSize);
            return AjaxHelper.JsonResult(HttpStatusCode.OK, "成功", new ListModel<OutputUserModel> { Total = result.Item1, List = Mapper.Map<IList<OutputUserModel>>(result.Item2) });
        }

        /// <summary>
        /// 检查用户名是否重复。200成功，400用户名不能为空
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>true存在，false不存在</returns>
        [Route("checkUserNameDup")]
        [HttpPost]
        public async Task<JsonResult> CheckUserNameDup([FromBody]string userName)
        {
            if (userName.IsNullOrWhiteSpace())
            {
                return AjaxHelper.JsonResult(HttpStatusCode.BadRequest, "用户名不能为空");
            }
            var result = await _userService.CheckUserNameDupAsync(userName);
            return AjaxHelper.JsonResult(HttpStatusCode.OK, "成功", result);
        }

        /// <summary>
        /// 检查邮箱是否重复。200成功，400邮箱不能为空
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <returns>true存在，false不存在</returns>
        [Route("checkEmailDup")]
        [HttpPost]
        public async Task<JsonResult> CheckEmailDup([FromBody]string email)
        {
            if (email.IsNullOrWhiteSpace())
            {
                return AjaxHelper.JsonResult(HttpStatusCode.BadRequest, "邮箱不能为空");
            }
            var result = await _userService.CheckEmailDupAsync(email);
            return AjaxHelper.JsonResult(HttpStatusCode.OK, "成功", result);
        }

        /// <summary>
        /// 检查手机号是否重复。200成功，400邮箱不能为空
        /// </summary>
        /// <param name="phoneNum">手机号</param>
        /// <returns>true存在，false不存在</returns>
        [Route("checkPhoneDup")]
        [HttpPost]
        public async Task<JsonResult> CheckPhoneDup([FromBody]string phoneNum)
        {
            if (phoneNum.IsNullOrWhiteSpace())
            {
                return AjaxHelper.JsonResult(HttpStatusCode.BadRequest, "手机号不能为空");
            }
            var result = await _userService.CheckPhoneDupAsync(phoneNum);
            return AjaxHelper.JsonResult(HttpStatusCode.OK, "成功", result);
        }

        /// <summary>
        /// 更新用户信息。400 用户Id不能为空,用户名不能为空，200成功
        /// </summary>
        /// <param name="inputUserModel">用户参数model</param>
        /// <returns></returns>
        [Route("updateUserInfo")]
        [HttpPost]
        public async Task<JsonResult> UpdateUserInfo([FromBody] InputUserModel inputUserModel)
        {
            if (inputUserModel.UserId <= 0)
            {
                return AjaxHelper.JsonResult(HttpStatusCode.BadRequest, "用户Id不能为空");
            }
            if (inputUserModel.UserName.IsNullOrWhiteSpace())
            {
                return AjaxHelper.JsonResult(HttpStatusCode.BadRequest, "用户名不能为空");
            }
            var userDto = new UserDto
            {
                Id = inputUserModel.UserId,
                UserName = inputUserModel.UserName,
                RealName = inputUserModel.RealName
            };
            if (!inputUserModel.Roles.IsNullOrEmpty())
            {
                var user = await _userService.GetAsync(inputUserModel.UserId);
                var userRoleIds = user.UserRoles.Select(i => i.RoleId).ToList();
                var userRoleDtos = new List<UserRoleDto>();
                foreach (var roleId in inputUserModel.Roles)
                {
                    if (!userRoleIds.Contains(roleId))
                    {
                        userRoleDtos.Add(new UserRoleDto { UserId = inputUserModel.UserId, RoleId = roleId });
                    }
                }
                userDto.UserRoles = userRoleDtos;
            }
            await _userService.UpdateAsync(userDto);
            return AjaxHelper.JsonResult(HttpStatusCode.OK, "成功");
        }
    }
}