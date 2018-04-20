using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoreSolution.Domain.Entities;
using CoreSolution.Dto;
using CoreSolution.EntityFrameworkCore.Repositories;
using CoreSolution.IService;
using CoreSolution.Tools.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CoreSolution.Service
{
    public sealed class MenuService : EfCoreRepositoryBase<Menu, MenuDto, int>, IMenuService
    {
        public override IQueryable<Menu> GetAllIncluding()
        {
            return GetAll()
                .Include(i => i.CreatorUser)
                .Include(i => i.DeleterUser)
                .Include(i => i.MenuItems);
        }

        public override async Task<MenuDto> InsertAsync(MenuDto entityDto)
        {
            if (entityDto != null)
            {
                bool r = await CheckMenuNameDupAsync(entityDto.Name);
                if (!r)
                {
                    await _dbContext.Menus.AddAsync(Mapper.Map<Menu>(entityDto));
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"已存在name={entityDto.Name}的菜单");
                }
            }
            return entityDto;
        }

        public Task<bool> CheckMenuNameDupAsync(string menuName)
        {
            return AnyAsync(i => i.Name == menuName);
        }

        public async Task<IList<MenuDto>> GetUserMenuList(int userId)
        {
            var roleIds = await _dbContext.UserRoles
                .Include(i => i.Role)
                .Include(i => i.Role.Permissions)
                .Where(i => i.UserId == userId)
                .Select(i => i.RoleId)
                .ToListAsync();
            var userPermissionList = await _dbContext.Permissions
                .Where(i => roleIds.Contains(i.RoleId))
                .Select(i => i.Name).ToListAsync();
            var menus = await GetAllIncluding().ProjectTo<MenuDto>().ToListAsync();
            foreach (var menu in menus)//第一层
            {
                if (!menu.MenuItems.IsNullOrEmpty())
                {
                    var menuItemsTwo = new List<MenuItemDto>();
                    menuItemsTwo.AddRange(menu.MenuItems);
                    foreach (var menuItem in menu.MenuItems)//第二层
                    {
                        if (menuItem.RequiresAuthentication &&
                            !userPermissionList.Contains(menuItem.RequiredPermissionName))
                        {
                            menuItemsTwo.Remove(menuItem);
                        }
                        else
                        {
                            var menuItems = await _dbContext.MenuItems.Where(i => i.MenuItemId == menuItem.Id).ProjectTo<MenuItemDto>().ToListAsync();
                            menuItem.MenuItems = menuItems;
                            if (!menuItem.MenuItems.IsNullOrEmpty())
                            {
                                var menuItemsThree = new List<MenuItemDto>();
                                menuItemsThree.AddRange(menuItem.MenuItems);
                                foreach (var mi in menuItem.MenuItems)//第三层
                                {
                                    if (mi.RequiresAuthentication &&
                                        !userPermissionList.Contains(mi.RequiredPermissionName))
                                    {
                                        menuItemsThree.Remove(mi);
                                    }
                                }
                                menuItem.MenuItems = menuItemsThree;
                            }
                        }
                    }
                    menu.MenuItems = menuItemsTwo;
                }
            }
            return menus;
        }
    }
}
