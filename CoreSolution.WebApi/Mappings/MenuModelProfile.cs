using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreSolution.AutoMapper.MapProfile;
using CoreSolution.Dto;
using CoreSolution.WebApi.Models.Menu;

namespace CoreSolution.WebApi.Mappings
{
    public class MenuModelProfile:Profile,IProfile
    {
        public MenuModelProfile()
        {
            CreateMap<MenuDto, OutputMenuModel>()
                .ForMember(i => i.MenuItems, i => i.MapFrom(m => m.MenuItems));
        }
    }
}
