using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreSolution.Domain.Entities;
using CoreSolution.Dto;
using CoreSolution.EntityFrameworkCore.Repositories;
using CoreSolution.IService;
using Microsoft.EntityFrameworkCore;

namespace CoreSolution.Service
{
    public sealed class MenuItemService : EfCoreRepositoryBase<MenuItem, MenuItemDto, int>, IMenuItemService
    {
        public override IQueryable<MenuItem> GetAllIncluding()
        {
            return GetAll()
                .Include(i => i.CreatorUser)
                .Include(i => i.DeleterUser)
                .Include(i => i.MenuItems)
                .Include(i => i.Menu);
        }

        public override async Task<MenuItemDto> InsertAsync(MenuItemDto entityDto)
        {
            if (entityDto != null)
            {
                bool r = await CheckMenuItemNameDupAsync(entityDto.Name);
                if (!r)
                {
                    await _dbContext.MenuItems.AddAsync(Mapper.Map<MenuItem>(entityDto));
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"已存在name={entityDto.Name}的菜单");
                }
            }
            return entityDto;
        }

        public Task<bool> CheckMenuItemNameDupAsync(string menuItemName)
        {
            return AnyAsync(i => i.Name == menuItemName);
        }
    }
}
