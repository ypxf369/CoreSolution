﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreSolution.Domain.Entities;
using CoreSolution.Domain.Enum;
using CoreSolution.Dto;
using CoreSolution.EntityFrameworkCore.Repositories;
using CoreSolution.IService;
using CoreSolution.Tools.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CoreSolution.Service
{
    public sealed class UserService : EfCoreRepositoryBase<User, UserDto, int>, IUserService
    {
        public override IQueryable<User> GetAllIncluding()
        {
            return GetAll()
                .Include(i => i.CreatorUser)
                .Include(i => i.DeleterUser)
                .Include(i => i.UserRoles);
        }

        public override async Task<UserDto> InsertAsync(UserDto entityDto)
        {
            if (entityDto != null)
            {
                //检查用户名是否存在
                bool r = await CheckUserNameDupAsync(entityDto.UserName);
                if (!r)
                {
                    await _dbContext.Users.AddAsync(Mapper.Map<User>(entityDto));
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"已存在name={entityDto.UserName}的用户");
                }
            }
            return entityDto;
        }

        public Task<bool> CheckUserNameDupAsync(string userName)
        {
            return AnyAsync(i => i.UserName == userName);
        }

        public Task<bool> CheckPhoneDupAsync(string phoneNum)
        {
            return AnyAsync(i => i.PhoneNum == phoneNum);
        }

        public Task<bool> CheckEmailDupAsync(string email)
        {
            return AnyAsync(i => i.Email == email);
        }

        public async Task<LoginResults> CheckUserPasswordAsync(string userNameOrEmailOrPhone, string password)
        {
            var userDto = await GetUserByUserNameOrEmailOrPhoneAsync(userNameOrEmailOrPhone);
            if (userDto != null)
            {
                if (userDto.Password.Equals((password.ToMd5() + userDto.Salt).ToMd5(), StringComparison.OrdinalIgnoreCase))
                {
                    return LoginResults.Success;
                }
                else
                {
                    return LoginResults.PassWordError;
                }
            }
            else
            {
                return LoginResults.NotExist;
            }
        }

        public async Task<UserDto> GetUserByUserNameOrEmailOrPhoneAsync(string userNameOrEmailOrPhone)
        {
            UserDto userDto;
            if (userNameOrEmailOrPhone.IsNumeric() && userNameOrEmailOrPhone.Length == 11)
            {
                userDto = await SingleOrDefaultAsync(i => i.PhoneNum == userNameOrEmailOrPhone && i.IsPhoneNumConfirmed);
            }
            else
            {
                userDto = await SingleOrDefaultAsync(i =>
                    i.UserName == userNameOrEmailOrPhone || i.Email == userNameOrEmailOrPhone && i.IsEmailConfirmed);
            }
            return userDto;
        }
    }
}
