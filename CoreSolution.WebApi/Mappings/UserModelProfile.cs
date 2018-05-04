using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreSolution.AutoMapper.MapProfile;
using CoreSolution.Dto;
using CoreSolution.WebApi.Models.User;

namespace CoreSolution.WebApi.Mappings
{
    public class UserModelProfile : Profile, IProfile
    {
        public UserModelProfile()
        {
            CreateMap<UserDto, OutputUserModel>()
                .ForMember(i => i.RoleName, i => i.MapFrom(m => m.UserRoles.Select(s => s.Role.Name).ToArray()));
        }
    }
}
