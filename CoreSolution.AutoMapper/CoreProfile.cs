using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using CoreSolution.Domain.Entities;
using CoreSolution.Dto;

namespace CoreSolution.AutoMapper
{
    public class CoreProfile : Profile, IProfile
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
