using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
using System.Text;
using System.Threading.Tasks;
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
        public override void Delete(PermissionDto entityDto)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override Task DeleteAsync(PermissionDto entityDto)
        {
            throw new NotImplementedException();
        }

        public override Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public override Task DeleteAsync(Expression<Func<Permission, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<Permission> GetAll()
        {
            throw new NotImplementedException();
        }

        public override IQueryable<Permission> GetAllIncluding()
        {
            throw new NotImplementedException();
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

        public override PermissionDto Insert(PermissionDto entityDto)
        {
            throw new NotImplementedException();
        }

        public override Task<PermissionDto> InsertAsync(PermissionDto entityDto)
        {
            throw new NotImplementedException();
        }

        public override PermissionDto Update(PermissionDto entityDto)
        {
            throw new NotImplementedException();
        }

        public override Task<PermissionDto> UpdateAsync(PermissionDto entityDto)
        {
            throw new NotImplementedException();
        }
    }
}
