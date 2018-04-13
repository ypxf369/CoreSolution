using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CoreSolution.Domain.Entities;
using CoreSolution.Dto;

namespace CoreSolution.AutoMapper.MapProfile
{
    public class UserRoleProfile : Profile, IProfile
    {
        public UserRoleProfile()
        {
            CreateMap<UserRole, UserRoleDto>()
                .ForMember(i => i.UserName, i => i.MapFrom(m => m.User != null ? m.User.UserName : null))
                .ForMember(i => i.RoleName, i => i.MapFrom(m => m.Role != null ? m.Role.Name : null));

            CreateMap<UserRoleDto, UserRole>()
                .ForMember(i => i.IsDeleted, i => i.Ignore())
                .ForMember(i => i.CreationTime, i => i.Ignore());
        }
    }
}
