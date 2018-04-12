using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoreSolution.AutoMapper;
using CoreSolution.AutoMapper.Configuration;
using CoreSolution.AutoMapper.Extensions;
using CoreSolution.AutoMapper.Startup;
using CoreSolution.Domain.Entities;
using CoreSolution.Dto;
using CoreSolution.EntityFrameworkCore;
using CoreSolution.IService;
using CoreSolution.Service;
using Microsoft.EntityFrameworkCore;

namespace CoreSolution.Console.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            /*using (var dbContext = new CoreDbContext())
            {
                dbContext.Users.ToList();
            }*/
            AutoMapperStartup.Register();
            var builder = new ContainerBuilder();
            builder.RegisterType<UserService>().As<IUserService>();
            using (var container = builder.Build())
            {
                var userService = container.Resolve<IUserService>();
                var userDto = new UserDto
                {
                    Id=1,
                    UserName = "yepeng",
                    RealName = "ypxf369",
                    Email = "ypxf369@163.com",
                    PhoneNum = "18621972473",
                    Password = "123456",
                    CreatorUserId = 1,
                    DeleterUserId = 1,
                    //Roles = new List<RoleDto>() { new RoleDto(){Id = 1}}
                };
                
                
                var user1 = new User
                {
                    Id=1,
                    UserName = "aaa",
                    RealName = "aaaypxf369",
                    Email = "ypxf369@163.com",
                    IsEmailConfirmed = true,
                    PhoneNum = "18621972473",
                    IsPhoneNumConfirmed = true,
                    Password = "123456",
                };

                var userEntity = new User()
                {
                    CreationTime = DateTime.Now,
                    UserName = "yepeng",
                    RealName = "ypxf369",
                    Email = "ypxf369@163.com",
                    IsEmailConfirmed = true,
                    PhoneNum = "18621972473",
                    IsPhoneNumConfirmed = true,
                    Password = "123456",
                    IsLocked = false,
                    //CreatorUser = user1,
                    //CreatorUserId = 1,
                    //DeleterUser = user1,
                    //DeleterUserId = 1
                };

                var roleEntity = new Role()
                {
                    Id = 1,
                    Name = "Admin",
                    Description = "我是管理员",
                    CreatorUser = user1,
                    CreatorUserId = 1,
                    DeleterUser = user1,
                    DeleterUserId = 1,

                };

                var userRoleEntity = new UserRole
                {
                    Id = 1,
                    UserId =user1.Id,
                    User = user1,
                    RoleId = roleEntity.Id,
                    Role = roleEntity
                };
                user1.UserRoles=new List<UserRole>(){userRoleEntity};
                userEntity.UserRoles=new List<UserRole>(){userRoleEntity};
                var user = Mapper.Map<User>(userDto);//userDto.MapTo<User>();
                var uDto = Mapper.Map<UserDto>(userEntity);

                using (var dbContext = new CoreDbContext())
                {
                    //var d = dbContext.Users.ProjectTo<UserDto>().First();
                    var d = dbContext.Users.Include(i=>i.CreatorUser).Include(i=>i.DeleterUser).Include(i=>i.UserRoles).ToList();
                }

                //int id=userService.InsertAndGetId(userDto);
                var u = userService.Get(2);
                //var usere = Mapper.Map<User>(u);
                u.UserName = "ypxf369_2";
                int id=userService.InsertAndGetId(u);
            }


            System.Console.WriteLine("Hello World!");
        }



    }
}
