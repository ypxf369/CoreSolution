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
using CoreSolution.WebApi.Models.Menu;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreSolution.WebApi.Controllers
{
    /// <summary>
    /// 菜单操作控制器
    /// </summary>
    [Produces("application/json")]
    [Route("api/Menu")]
    [CheckAuthorize("Admin")]
    public class MenuController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly IMenuItemService _menuItemService;


        public MenuController(IMenuService menuService, IMenuItemService menuItemService)
        {
            _menuService = menuService;
            _menuItemService = menuItemService;
        }

        /// <summary>
        /// 新增菜单（最大支持三级菜单）。200成功，400菜单名不能为空
        /// </summary>
        /// <param name="inputMenuModel">菜单参数model</param>
        /// <returns></returns>
        [Route("addNew")]
        [HttpPost]
        public async Task<JsonResult> AddNew([FromBody] InputMenuModel inputMenuModel)
        {
            if (inputMenuModel.Name.IsNullOrWhiteSpace())
            {
                return AjaxHelper.JsonResult(HttpStatusCode.BadRequest, "菜单名不能为空");
            }
            string token = HttpContext.Request.Headers["token"];
            var userId = (await LoginManager.GetUserIdAsync(token)).GetValueOrDefault();
            var menuDto = new MenuDto //主菜单-第一层
            {
                Name = inputMenuModel.Name,
                Url = inputMenuModel.Url,
                CustomData = inputMenuModel.CustomData,
                Icon = inputMenuModel.Icon,
                ClassName = inputMenuModel.ClassName,
                OrderIn = inputMenuModel.OrderIn,
                CreatorUserId = userId
            };
            var menuItemListTwo = new List<MenuItemDto>();
            if (!inputMenuModel.MenuItems.IsNullOrEmpty())
            {
                foreach (var menuItemTwo in inputMenuModel.MenuItems.Select(i => ToMenuItemDto(userId, i))) //第二层
                {
                    if (!menuItemTwo.MenuItems.IsNullOrEmpty())
                    {
                        menuItemTwo.MenuItems = menuItemTwo.MenuItems.Select(i => new MenuItemDto //第三层
                        {
                            Name = i.Name,
                            Url = i.Url,
                            CustomData = i.CustomData,
                            Icon = i.Icon,
                            ClassName = i.ClassName,
                            OrderIn = i.OrderIn,
                            RequiredPermissionName = i.RequiredPermissionName,
                            RequiresAuthentication = i.RequiresAuthentication,
                            CreatorUserId = userId
                        }).ToList();
                        menuItemListTwo.Add(menuItemTwo);
                    }
                }

                #region 冗余代码

                //    var menuDto = new MenuDto
                //{
                //    Name = inputMenuModel.Name,
                //    Url = inputMenuModel.Url,
                //    CustomData = inputMenuModel.CustomData,
                //    Icon = inputMenuModel.Icon,
                //    ClassName = inputMenuModel.ClassName,
                //    OrderIn = inputMenuModel.OrderIn,
                //    MenuItems = inputMenuModel.MenuItems.Select(i => new MenuItemDto
                //    {
                //        Name = i.Name,
                //        Url = i.Url,
                //        CustomData = i.CustomData,
                //        Icon = i.Icon,
                //        ClassName = i.ClassName,
                //        OrderIn = i.OrderIn,
                //        RequiredPermissionName = i.RequiredPermissionName,
                //        RequiresAuthentication = i.RequiresAuthentication,
                //        MenuItems = inputMenuModel.MenuItems.Select(s=>s.MenuItems.Select(sss=>))
                //    }).ToList(),
                //    //CreatorUserId = userId
                //}; 

                #endregion
            }
            menuDto.MenuItems = menuItemListTwo;
            var id = await _menuService.InsertAndGetIdAsync(menuDto);
            return AjaxHelper.JsonResult(HttpStatusCode.OK, "成功", id);
        }

        private MenuItemDto ToMenuItemDto(int userId, InputMenuItemModel model)
        {
            if (model == null)
            {
                return new MenuItemDto();
            }
            return new MenuItemDto
            {
                Name = model.Name,
                Url = model.Url,
                CustomData = model.CustomData,
                Icon = model.Icon,
                ClassName = model.ClassName,
                OrderIn = model.OrderIn,
                RequiredPermissionName = model.RequiredPermissionName,
                RequiresAuthentication = model.RequiresAuthentication,
                CreatorUserId = userId,
                MenuItems = model.MenuItems.Select(i => ToMenuItemDto(userId, i)).ToList()
            };
        }

        /// <summary>
        /// 新增菜单项。200成功，400菜单项名不能为空
        /// </summary>
        /// <param name="inputMenuItemModel">菜单项参数model</param>
        /// <returns></returns>
        [Route("addNewItem")]
        [HttpPost]
        public async Task<JsonResult> AddNewItem([FromBody] InputMenuItemModel inputMenuItemModel)
        {
            if (inputMenuItemModel.Name.IsNullOrWhiteSpace())
            {
                return AjaxHelper.JsonResult(HttpStatusCode.BadRequest, "菜单项名不能为空");
            }
            string token = HttpContext.Request.Headers["token"];
            var userId = (await LoginManager.GetUserIdAsync(token)).GetValueOrDefault();
            var menuItemDto = new MenuItemDto
            {
                Name = inputMenuItemModel.Name,
                Url = inputMenuItemModel.Url,
                CustomData = inputMenuItemModel.CustomData,
                Icon = inputMenuItemModel.Icon,
                ClassName = inputMenuItemModel.ClassName,
                OrderIn = inputMenuItemModel.OrderIn,
                RequiredPermissionName = inputMenuItemModel.RequiredPermissionName,
                RequiresAuthentication = inputMenuItemModel.RequiresAuthentication,
                MenuItems = inputMenuItemModel.MenuItems.Select(i => new MenuItemDto
                {
                    Name = i.Name,
                    Url = i.Url,
                    CustomData = i.CustomData,
                    Icon = i.Icon,
                    ClassName = i.ClassName,
                    OrderIn = i.OrderIn,
                    RequiredPermissionName = i.RequiredPermissionName,
                    RequiresAuthentication = i.RequiresAuthentication
                }).ToList(),
                CreatorUserId = userId
            };
            var id = await _menuItemService.InsertAndGetIdAsync(menuItemDto);
            return AjaxHelper.JsonResult(HttpStatusCode.OK, "成功", id);
        }

        /// <summary>
        /// 删除菜单。200删除成功，该菜单不存在
        /// </summary>
        /// <param name="menuId">菜单Id</param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        public async Task<JsonResult> Delete([FromBody]int menuId)
        {
            var menu = await _menuService.GetAsync(menuId);
            if (menu == null)
            {
                return AjaxHelper.JsonResult(HttpStatusCode.NotFound, "该菜单不存在");
            }
            string token = HttpContext.Request.Headers["token"];
            var currentUserId = (await LoginManager.GetUserIdAsync(token)).GetValueOrDefault();
            menu.DeleterUserId = currentUserId;
            menu.DeletionTime = DateTime.Now;
            await _menuService.DeleteAsync(menu);
            return AjaxHelper.JsonResult(HttpStatusCode.OK, "删除成功");
        }

        /// <summary>
        /// 删除菜单项。200删除成功，404该菜单项不存在
        /// </summary>
        /// <param name="menuItemId">菜单项Id</param>
        /// <returns></returns>
        [Route("deleteItem")]
        [HttpPost]
        public async Task<JsonResult> DeleteItem([FromBody]int menuItemId)
        {
            var menuItem = await _menuItemService.GetAsync(menuItemId);
            if (menuItem == null)
            {
                return AjaxHelper.JsonResult(HttpStatusCode.NotFound, "该菜单项不存在");
            }
            string token = HttpContext.Request.Headers["token"];
            var currentUserId = (await LoginManager.GetUserIdAsync(token)).GetValueOrDefault();
            menuItem.DeleterUserId = currentUserId;
            menuItem.DeletionTime = DateTime.Now;
            await _menuItemService.DeleteAsync(menuItem);
            return AjaxHelper.JsonResult(HttpStatusCode.OK, "删除成功");
        }

        /// <summary>
        /// 检查菜单名是否重复。200成功，400菜单名不能为空
        /// </summary>
        /// <param name="menuName">菜单名</param>
        /// <returns>true存在，false不存在</returns>
        [Route("checkMenuNameDup")]
        [HttpPost]
        public async Task<JsonResult> CheckMenuNameDup([FromBody]string menuName)
        {
            if (menuName.IsNullOrWhiteSpace())
            {
                return AjaxHelper.JsonResult(HttpStatusCode.BadRequest, "菜单名不能为空");
            }
            var result = await _menuService.CheckMenuNameDupAsync(menuName);
            return AjaxHelper.JsonResult(HttpStatusCode.OK, "成功", result);
        }

        /// <summary>
        /// 检查菜单项名是否重复。200成功，400菜单项名不能为空
        /// </summary>
        /// <param name="menuItemName">菜单项名</param>
        /// <returns>true存在，false不存在</returns>
        [Route("checkMenuItemNameDup")]
        [HttpPost]
        public async Task<JsonResult> CheckMenuItemNameDup([FromBody]string menuItemName)
        {
            if (menuItemName.IsNullOrWhiteSpace())
            {
                return AjaxHelper.JsonResult(HttpStatusCode.BadRequest, "菜单项名不能为空");
            }
            var result = await _menuItemService.CheckMenuItemNameDupAsync(menuItemName);
            return AjaxHelper.JsonResult(HttpStatusCode.OK, "成功", result);
        }

        /// <summary>
        /// 获取菜单列表。200获取成功
        /// </summary>
        /// <returns></returns>
        [Route("getMenuList")]
        [HttpGet]
        public async Task<JsonResult> GetMenuList()
        {
            string token = HttpContext.Request.Headers["token"];
            var userId = (await LoginManager.GetUserIdAsync(token)).GetValueOrDefault();
            var result = await _menuService.GetUserMenuList(userId);
            return AjaxHelper.JsonResult(HttpStatusCode.OK, "成功", new ListModel<OutputMenuModel> { Total = result.Count, List = Mapper.Map<IList<OutputMenuModel>>(result) });
        }
    }
}