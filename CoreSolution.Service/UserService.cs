using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CoreSolution.AutoMapper.Extensions;
using CoreSolution.Domain.Entities;
using CoreSolution.Domain.Enum;
using CoreSolution.Dto;
using CoreSolution.IService;
using CoreSolution.Repository;
using Microsoft.EntityFrameworkCore;

namespace CoreSolution.Service
{
    public sealed class UserService : RepositoryBase<User, UserDto, int>, IUserService
    {
        public UserService()
        {
            CoreDbContext = DbContextFactory.DbContext;
        }
        public override void Delete(UserDto entityDto)
        {
            if (entityDto != null)
            {
                var entry = CoreDbContext.Entry(entityDto.MapTo<User>());
                entry.State = EntityState.Deleted;
                CoreDbContext.SaveChanges();
            }
        }

        public override async Task DeleteAsync(UserDto entityDto)
        {
            if (entityDto != null)
            {
                var entry = CoreDbContext.Entry(entityDto.MapTo<User>());
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

        public override UserDto Insert(UserDto entityDto)
        {
            if (entityDto != null)
            {
                CoreDbContext.Users.Add(entityDto.MapTo<User>());
                CoreDbContext.SaveChanges();
            }
            return entityDto;
        }

        public override async Task<UserDto> InsertAsync(UserDto entityDto)
        {
            if (entityDto != null)
            {

                CoreDbContext.Users.Add(entityDto.MapTo<User>());
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
                    entity = entityDto.MapTo<User>();
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
                    entity = entityDto.MapTo<User>();
                }
                await CoreDbContext.SaveChangesAsync();
            }
            return entityDto;
        }

        Task<LoginResults> IUserService.CheckEmailAndPasswordAsync(string email, string password)
        {
            throw new NotImplementedException();
        }

        Task<bool> IUserService.CheckPhoneDupAsync(string phoneNum)
        {
            throw new NotImplementedException();
        }

        Task<LoginResults> IUserService.CheckPhoneNumAndPasswordAsync(string phoneNum, string password)
        {
            throw new NotImplementedException();
        }

        Task<LoginResults> IUserService.CheckUserNameAndPasswordAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }

        Task<UserDto> IUserService.GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        Task<UserDto> IUserService.GetUserByPhoneNumAsync(string phoneNum)
        {
            throw new NotImplementedException();
        }

        Task<UserDto> IUserService.GetUserByUserNameAsync(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
