using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoreSolution.Domain.Entities;
using CoreSolution.Domain.Enum;
using CoreSolution.Dto;
using CoreSolution.IService.Convention;
using CoreSolution.Repository;

namespace CoreSolution.IService
{
    public interface IUserService : IRepository<User,UserDto>, IServiceSupport
    {
        Task<bool> CheckUserNameDupAsync(string userName);
        Task<bool> CheckPhoneDupAsync(string phoneNum);
        Task<bool> CheckEmailDupAsync(string email);
        Task<LoginResults> CheckUserPasswordAsync(string userNameOrEmailOrPhone, string password);
        Task<UserDto> GetUserByUserNameOrEmailOrPhone(string userNameOrEmailOrPhone);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserByPhoneNumAsync(string phoneNum);
        Task<UserDto> GetUserByUserNameAsync(string userName);
    }
}
