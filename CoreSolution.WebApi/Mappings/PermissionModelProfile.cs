using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreSolution.AutoMapper.MapProfile;
using CoreSolution.Dto;
using CoreSolution.WebApi.Models.Permission;

namespace CoreSolution.WebApi.Mappings
{
    public class PermissionModelProfile : Profile, IProfile
    {
        public PermissionModelProfile()
        {
            CreateMap<PermissionDto, OutputPermissionModel>();
        }
    }
}
