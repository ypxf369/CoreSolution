using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
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
    public sealed class PermissionService : EfCoreRepositoryBase<Permission, PermissionDto, int>, IPermissionService
    {
        public override IQueryable<Permission> GetAllIncluding()
        {
            return GetAll()
                .Include(i => i.CreatorUser)
                .Include(i => i.DeleterUser)
                .Include(i => i.Role);
        }

        public override async Task<PermissionDto> InsertAsync(PermissionDto entityDto)
        {
            if (entityDto != null)
            {
                bool r = await CheckPermissionNameDupAsync(entityDto.Name);
                if (!r)
                {
                    await _dbContext.Permissions.AddAsync(Mapper.Map<Permission>(entityDto));
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"已存在name={entityDto.Name}的权限");
                }
            }
            return entityDto;
        }

        public async Task<IList<PermissionDto>> GetPermissionsByUserIdAsync(int userId)
        {
            var roleIds = await _dbContext.UserRoles
                .Where(i => i.UserId == userId)
                .Select(i => i.RoleId)
                .ToListAsync();
            return await GetAll()
                .Where(i => roleIds.Contains(i.RoleId))
                .ProjectTo<PermissionDto>()
                .ToListAsync();
        }

        public Task<bool> CheckPermissionNameDupAsync(string permissionName)
        {
            return AnyAsync(i => i.Name == permissionName);
        }
    }
}
