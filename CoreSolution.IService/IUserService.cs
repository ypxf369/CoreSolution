using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoreSolution.Domain.Entities;
using CoreSolution.Domain.Enum;
using CoreSolution.Dto;
using CoreSolution.EntityFrameworkCore.Repositories;
using CoreSolution.IService.Convention;

namespace CoreSolution.IService
{
    public interface IUserService : IEfCoreRepository<User, UserDto>, IServiceSupport
    {
        /// <summary>
        /// 检查用户名是否存在，true存在，false不存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        Task<bool> CheckUserNameDupAsync(string userName);
        Task<bool> CheckPhoneDupAsync(string phoneNum);
        Task<bool> CheckEmailDupAsync(string email);
        Task<LoginResults> CheckUserPasswordAsync(string userNameOrEmailOrPhone, string password);
        Task<UserDto> GetUserByUserNameOrEmailOrPhoneAsync(string userNameOrEmailOrPhone);
    }
}
