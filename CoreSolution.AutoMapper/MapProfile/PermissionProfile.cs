using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CoreSolution.Domain.Entities;
using CoreSolution.Dto;

namespace CoreSolution.AutoMapper.MapProfile
{
    public class PermissionProfile : Profile, IProfile
    {
        public PermissionProfile()
        {
            CreateMap<Permission, PermissionDto>()
                .ForMember(i => i.CreatorUserName,
                    i => i.MapFrom(m => m.CreatorUser != null ? m.CreatorUser.UserName : null))
                .ForMember(i => i.DeleterUserName,
                    i => i.MapFrom(m => m.DeleterUser != null ? m.DeleterUser.UserName : null))
                .ForMember(i => i.RoleId, i => i.MapFrom(m => m.Role != null ? m.Role.Id : 0))
                .ForMember(i => i.RoleName, i => i.MapFrom(m => m.Role != null ? m.Role.Name : null));

            CreateMap<PermissionDto, Permission>()
                .ForMember(i => i.IsDeleted, i => i.Ignore())
                .ForMember(i => i.CreationTime, i => i.Ignore());
        }
    }
}
