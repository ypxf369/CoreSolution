using System;
using System.Linq;
using Autofac;
using AutoMapper;
using CoreSolution.AutoMapper;
using CoreSolution.AutoMapper.Extensions;
using CoreSolution.AutoMapper.Startup;
using CoreSolution.Domain.Entities;
using CoreSolution.Dto;
using CoreSolution.EntityFrameworkCore;
using CoreSolution.IService;
using CoreSolution.Service;

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
            //AutoMapperStartup.Register();
            var builder = new ContainerBuilder();
            builder.RegisterType<UserService>().As<IUserService>();
            using (var container = builder.Build())
            {
                var userService = container.Resolve<IUserService>();

                var userDto = new UserDto
                {
                    UserName = "yepeng",
                    RealName = "叶鹏",
                    Email = "ypxf369@163.com",
                    PhoneNum = "18621972473"
                };

                //Todo:bug,在CoreSolution.AutoMapper中没有加载到配置
                Mapper.Initialize(i=>i.AddProfile(typeof(CoreProfile)));
                var user = Mapper.Map<User>(userDto);//userDto.MapTo<User>();
                int id=userService.InsertAndGetId(userDto);
            }


            System.Console.WriteLine("Hello World!");
        }



    }

    public class CoreProfile : Profile
    {
        public CoreProfile()
        {
            //TODO:用反射优化此代码
            CreateMap<User, UserDto>()
                .ForMember(i => i.Roles, i => i.MapFrom(m => m.UserRoles.Select(s => s.Role)));
            CreateMap<UserDto, User>()
                .ForMember(i => i.IsEmailConfirmed, i => i.Ignore())
                .ForMember(i => i.IsPhoneNumConfirmed, i => i.Ignore())
                .ForMember(i => i.CreatorUser, i => i.Ignore())
                .ForMember(i => i.DeleterUser, i => i.Ignore())
                .ForMember(i => i.IsDeleted, i => i.Ignore())
                .ForMember(i => i.UserRoles, i => i.Ignore());
            //.ForMember(i => i.UserRoles, i => i.MapFrom(m => m.Roles.Select(s => s.Users)));
        }
    }
}
