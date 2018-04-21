using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using CoreSolution.Dto;
using CoreSolution.IService;
using CoreSolution.Tools.Extensions;
using CoreSolution.Tools.WebResult;
using CoreSolution.WebApi.Interceptor;
using CoreSolution.WebApi.Manager;
using CoreSolution.WebApi.Models;
using CoreSolution.WebApi.Models.Role;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreSolution.WebApi.Controllers
{
    /// <summary>
    /// 角色操作控制器
    /// </summary>
    [Produces("application/json")]
    [Route("api/Role")]
    [CheckAuthorize("Admin")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }


        /// <summary>
        /// 新增角色。200成功，400角色名不能为空、角色必须具有至少一种权限
        /// </summary>
        /// <param name="inputRoleModel">角色参数model</param>
        /// <returns></returns>
        [Route("addNew")]
        [HttpPost]
        public async Task<JsonResult> AddNew([FromBody] InputRoleModel inputRoleModel)
        {
            if (inputRoleModel.Name.IsNullOrWhiteSpace())
            {
                return AjaxHelper.JsonResult(HttpStatusCode.BadRequest, "角色名不能为空");
            }
            if (inputRoleModel.Permissions.Count <= 0)
            {
                return AjaxHelper.JsonResult(HttpStatusCode.BadRequest, "角色必须具有至少一种权限");
            }
            string token = HttpContext.Request.Headers["token"];
            var userId = (await LoginManager.GetUserIdAsync(token)).GetValueOrDefault();
            var roleDto = new RoleDto
            {
                Name = inputRoleModel.Name,
                Description = inputRoleModel.Description,
                CreatorUserId = userId,
                Permissions = inputRoleModel.Permissions.Select(i => new PermissionDto
                {
                    Name = i.Name,
                    Description = i.Description,
                    CreatorUserId = userId
                }).ToList()
            };
            var id = await _roleService.InsertAndGetIdAsync(roleDto);

            return AjaxHelper.JsonResult(HttpStatusCode.OK, "成功", id);
        }

        /// <summary>
        /// 删除角色。200删除成功，404该角色不存在
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        public async Task<JsonResult> Delete([FromBody]int roleId)
        {
            var role = await _roleService.GetAsync(roleId);
            if (role == null)
            {
                return AjaxHelper.JsonResult(HttpStatusCode.NotFound, "该角色不存在");
            }
            string token = HttpContext.Request.Headers["token"];
            var currentUserId = (await LoginManager.GetUserIdAsync(token)).GetValueOrDefault();
            role.DeleterUserId = currentUserId;
            role.DeletionTime = DateTime.Now;
            await _roleService.DeleteAsync(role);
            return AjaxHelper.JsonResult(HttpStatusCode.OK, "删除成功");
        }

        /// <summary>
        /// 检查角色名是否重复。200成功，400角色名不能为空
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <returns>true存在，false不存在</returns>
        [Route("checkRoleNameDup")]
        [HttpPost]
        public async Task<JsonResult> CheckUserNameDup([FromBody]string roleName)
        {
            if (roleName.IsNullOrWhiteSpace())
            {
                return AjaxHelper.JsonResult(HttpStatusCode.BadRequest, "角色名不能为空");
            }
            var result = await _roleService.CheckRoleNameDupAsync(roleName);
            return AjaxHelper.JsonResult(HttpStatusCode.OK, "成功", result);
        }

        /// <summary>
        /// 根据Id获取角色。200获取成功
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        [Route("getRole")]
        [HttpGet]
        public async Task<JsonResult> GetRoleById(int roleId)
        {
            var result = await _roleService.GetAsync(roleId);
            return AjaxHelper.JsonResult(HttpStatusCode.OK, "成功", Mapper.Map<OutputRoleModel>(result));
        }

        /// <summary>
        /// 获取角色列表。200获取成功
        /// </summary>
        /// <returns></returns>
        [Route("getRoleList")]
        [HttpGet]
        public async Task<JsonResult> GetRoleList()
        {
            var result = await _roleService.GetAllListAsync();
            return AjaxHelper.JsonResult(HttpStatusCode.OK, "成功", new ListModel<OutputRoleModel> { Total = result.Count, List = Mapper.Map<IList<OutputRoleModel>>(result) });
        }
    }
}