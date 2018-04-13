using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoreSolution.Domain.Entities;
using CoreSolution.Dto;
using CoreSolution.IService.Convention;
using CoreSolution.Repository;

namespace CoreSolution.IService
{
    public interface IPermissionService : IRepository<Permission, PermissionDto>, IServiceSupport
    {
        Task<IList<PermissionDto>> GetPermissionsByUserIdAsync(int userId);
    }
}
