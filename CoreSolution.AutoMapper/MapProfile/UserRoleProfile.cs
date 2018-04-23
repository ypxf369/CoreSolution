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
            CreateMap<UserRole, UserRoleDto>();

            CreateMap<UserRoleDto, UserRole>()
                .ForMember(i => i.IsDeleted, i => i.Ignore())
                .ForMember(i => i.CreationTime, i => i.Ignore());
        }
    }
}
