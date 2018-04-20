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
using Microsoft.EntityFrameworkCore;

namespace CoreSolution.Service
{
    public sealed class RoleService : EfCoreRepositoryBase<Role, RoleDto, int>, IRoleService
    {
        public override IQueryable<Role> GetAllIncluding()
        {
            return GetAll()
                .Include(i => i.CreatorUser)
                .Include(i => i.DeleterUser)
                .Include(i => i.UserRoles);
        }

        public override async Task<RoleDto> InsertAsync(RoleDto entityDto)
        {
            if (entityDto != null)
            {
                bool r = Any(i => i.Name == entityDto.Name);
                if (!r)
                {
                    await _dbContext.Roles.AddAsync(Mapper.Map<Role>(entityDto));
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"已存在name={entityDto.Name}的角色");
                }
            }
            return entityDto;
        }

        public Task<bool> CheckRoleNameDupAsync(string roleName)
        {
            return AnyAsync(i => i.Name == roleName);
        }

        public async Task<IList<RoleDto>> GetRolesByUserIdAsync(int userId)
        {
            var roleIds = await _dbContext.UserRoles
                .Where(i => i.UserId == userId)
                .Select(i => i.RoleId)
                .ToListAsync();
            return await GetAll()
                .Where(i => roleIds.Contains(i.Id))
                .ProjectTo<RoleDto>()
                .ToListAsync();
        }
    }
}
