using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreSolution.Domain.Entities;
using CoreSolution.Domain.Enum;
using CoreSolution.Dto;
using CoreSolution.EntityFrameworkCore;
using CoreSolution.EntityFrameworkCore.Repositories;
using CoreSolution.IService;
using CoreSolution.Tools.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CoreSolution.Service
{
    public sealed class UserService : EfCoreRepositoryBase<User, UserDto, int>, IUserService
    {
        public UserService()
        {
            CoreDbContext = DbContextFactory.DbContext;
        }
        public override void Delete(UserDto entityDto)
        {
            if (entityDto != null)
            {
                var entry = CoreDbContext.Entry(Mapper.Map<User>(entityDto));
                entry.State = EntityState.Deleted;
                CoreDbContext.SaveChanges();
            }
        }

        public override async Task DeleteAsync(UserDto entityDto)
        {
            if (entityDto != null)
            {
                var entry = CoreDbContext.Entry(Mapper.Map<User>(entityDto));
                entry.State = EntityState.Deleted;
                await CoreDbContext.SaveChangesAsync();
            }
        }

        public override void Delete(int id)
        {
            if (id > 0)
            {
                var user = Single(i => i.Id == id);
                var entry = CoreDbContext.Entry(user);
                entry.State = EntityState.Deleted;
                CoreDbContext.SaveChanges();
            }
        }

        public override async Task DeleteAsync(int id)
        {
            if (id > 0)
            {
                var user = await SingleAsync(i => i.Id == id);
                var entry = CoreDbContext.Entry(user);
                entry.State = EntityState.Deleted;
                await CoreDbContext.SaveChangesAsync();
            }
        }

        public override async Task DeleteAsync(Expression<Func<User, bool>> predicate)
        {
            var users = GetAll().Where(predicate);
            if (await users.AnyAsync())
            {
                await users.ForEachAsync(i =>
               {
                   var entry = CoreDbContext.Entry(i);
                   entry.State = EntityState.Deleted;
               });
                await CoreDbContext.SaveChangesAsync();
            }
        }

        public override IQueryable<User> GetAll()
        {
            return CoreDbContext.Users;
        }

        public override IQueryable<User> GetAllIncluding()
        {
            return GetAll()
                .Include(i => i.CreatorUser)
                .Include(i => i.DeleterUser)
                .Include(i => i.UserRoles);
        }

        public override UserDto Insert(UserDto entityDto)
        {
            if (entityDto != null)
            {
                CoreDbContext.Users.Add(Mapper.Map<User>(entityDto));
                CoreDbContext.SaveChanges();
            }
            return entityDto;
        }

        public override async Task<UserDto> InsertAsync(UserDto entityDto)
        {
            if (entityDto != null)
            {

                CoreDbContext.Users.Add(Mapper.Map<User>(entityDto));
                await CoreDbContext.SaveChangesAsync();
            }
            return entityDto;
        }

        public override UserDto Update(UserDto entityDto)
        {
            if (entityDto != null && entityDto.Id > 0)
            {
                var entity = GetAll().SingleOrDefault(i => i.Id == entityDto.Id);
                if (entity != null)
                {
                    entity = Mapper.Map<User>(entityDto);
                    CoreDbContext.SaveChanges();
                }
            }
            return entityDto;
        }

        public override async Task<UserDto> UpdateAsync(UserDto entityDto)
        {
            if (entityDto != null && entityDto.Id > 0)
            {
                var entity = await GetAll().SingleOrDefaultAsync(i => i.Id == entityDto.Id);
                if (entity != null)
                {
                    entity = Mapper.Map<User>(entityDto);
                }
                await CoreDbContext.SaveChangesAsync();
            }
            return entityDto;
        }

        public Task<bool> CheckUserNameDupAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckPhoneDupAsync(string phoneNum)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckEmailDupAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginResults> CheckUserPasswordAsync(string userNameOrEmailOrPhone, string password)
        {
            var userDto = await GetUserByUserNameOrEmailOrPhoneAsync(userNameOrEmailOrPhone);
            if (userDto != null)
            {
                if (userDto.Password == password.ToMd5())
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
            if (userNameOrEmailOrPhone.IsNumeric())
            {
                userDto = await SingleOrDefaultAsync(i => i.PhoneNum == userNameOrEmailOrPhone);
            }
            else
            {
                userDto = await SingleOrDefaultAsync(i =>
                    i.UserName == userNameOrEmailOrPhone || i.IsEmailConfirmed && i.Email == userNameOrEmailOrPhone);
            }
            return userDto;
        }

        public Task<UserDto> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetUserByPhoneNumAsync(string phoneNum)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetUserByUserNameAsync(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
