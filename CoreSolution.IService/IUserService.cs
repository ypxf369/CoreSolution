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
        Task<LoginResults> CheckEmailAndPasswordAsync(string email, string password);
        Task<LoginResults> CheckPhoneNumAndPasswordAsync(string phoneNum, string password);
        Task<LoginResults> CheckUserNameAndPasswordAsync(string userName, string password);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserByPhoneNumAsync(string phoneNum);
        Task<UserDto> GetUserByUserNameAsync(string userName);
        Task<bool> CheckPhoneDupAsync(string phoneNum);
    }
}
