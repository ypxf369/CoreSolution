using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using CoreSolution.Domain.Entities;
using CoreSolution.Dto;

namespace CoreSolution.AutoMapper.MapProfile
{
    public class MenuItemProfile : Profile, IProfile
    {
        public MenuItemProfile()
        {
            CreateMap<MenuItem, MenuItemDto>();

            CreateMap<MenuItemDto, MenuItem>()
                .ForMember(i => i.IsDeleted, i => i.Ignore())
                .ForMember(i => i.CreationTime, i => i.Ignore());
        }
    }
}
