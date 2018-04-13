using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using CoreSolution.Domain.Entities;
using CoreSolution.Dto;

namespace CoreSolution.AutoMapper.MapProfile
{
    public class RoleProfile : Profile, IProfile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>()
                .ForMember(i => i.CreatorUserName,
                    i => i.MapFrom(m => m.CreatorUser != null ? m.CreatorUser.UserName : null))
                .ForMember(i => i.DeleterUserName,
                    i => i.MapFrom(m => m.DeleterUser != null ? m.DeleterUser.UserName : null))
                .ForMember(i => i.Users,
                    i => i.MapFrom(m => m.UserRoles.Select(s => s.User).ToDictionary(d => d.Id, d => d.UserName)))
                .ForMember(i => i.Permissions, i => i.MapFrom(m => m.Permissions.ToDictionary(d => d.Id, d => d.Name)));

            CreateMap<RoleDto, Role>()
                .ForMember(i => i.CreatorUser, i => i.Ignore())
                .ForMember(i => i.DeleterUser, i => i.Ignore())
                .ForMember(i => i.IsDeleted, i => i.Ignore())
                .ForMember(i => i.CreationTime, i => i.Ignore())
                .ForMember(i => i.UserRoles,
                    i => i.MapFrom(m => m.Users.Select(s => new UserRole { UserId = s.Key, RoleId = m.Id })))
                .ForMember(i => i.Permissions,
                    i => i.MapFrom(m => m.Permissions.Select(s => new Permission { Id = s.Key, Name = s.Value })));
        }
    }
}
