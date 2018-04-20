using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoreSolution.Domain.Entities;
using CoreSolution.Dto;
using CoreSolution.EntityFrameworkCore.Repositories;
using CoreSolution.IService.Convention;

namespace CoreSolution.IService
{
    public interface IRoleService : IEfCoreRepository<Role, RoleDto>, IServiceSupport
    {
        Task<bool> CheckRoleNameDupAsync(string roleName);
        Task<IList<RoleDto>> GetRolesByUserIdAsync(int userId);
    }
}
