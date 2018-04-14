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
    public interface IUserService : IEfCoreRepository<User,UserDto>, IServiceSupport
    {
        Task<bool> CheckUserNameDupAsync(string userName);
        Task<bool> CheckPhoneDupAsync(string phoneNum);
        Task<bool> CheckEmailDupAsync(string email);
        Task<LoginResults> CheckUserPasswordAsync(string userNameOrEmailOrPhone, string password);
        Task<UserDto> GetUserByUserNameOrEmailOrPhoneAsync(string userNameOrEmailOrPhone);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserByPhoneNumAsync(string phoneNum);
        Task<UserDto> GetUserByUserNameAsync(string userName);
    }
}
