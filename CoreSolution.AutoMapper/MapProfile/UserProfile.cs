using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using CoreSolution.Domain.Entities;
using CoreSolution.Dto;

namespace CoreSolution.AutoMapper.MapProfile
{
    public class UserProfile : Profile, IProfile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(i => i.CreatorUserName,
                    i => i.MapFrom(m => m.CreatorUser != null ? m.CreatorUser.UserName : null))
                .ForMember(i => i.DeleterUserName,
                    i => i.MapFrom(m => m.DeleterUser != null ? m.DeleterUser.UserName : null))
                .ForMember(i => i.Roles,
                    i => i.MapFrom(m => m.UserRoles.Select(s => s.Role).ToDictionary(d => d.Id, d => d.Name)));

            CreateMap<UserDto, User>()
                .ForMember(i => i.IsEmailConfirmed, i => i.Ignore())
                .ForMember(i => i.IsPhoneNumConfirmed, i => i.Ignore())
                .ForMember(i => i.IsDeleted, i => i.Ignore())
                .ForMember(i => i.CreationTime, i => i.Ignore())
                .ForMember(i => i.UserRoles, i => i.MapFrom(m => m.Roles.Select(s => new UserRole { UserId = m.Id, RoleId = s.Key })));
        }
    }
}
