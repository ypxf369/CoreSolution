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
                .ForMember(i => i.Password, i => i.Ignore());

            CreateMap<UserDto, User>()
                .ForMember(i => i.IsEmailConfirmed, i => i.Ignore())
                .ForMember(i => i.IsPhoneNumConfirmed, i => i.Ignore())
                .ForMember(i => i.IsDeleted, i => i.Ignore())
                .ForMember(i => i.CreationTime, i => i.Ignore());
        }
    }
}
