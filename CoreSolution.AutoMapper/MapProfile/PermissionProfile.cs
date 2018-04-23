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
            CreateMap<Permission, PermissionDto>();

            CreateMap<PermissionDto, Permission>()
                .ForMember(i => i.IsDeleted, i => i.Ignore())
                .ForMember(i => i.CreationTime, i => i.Ignore());
        }
    }
}
