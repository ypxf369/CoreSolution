using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using CoreSolution.Domain.Entities;
using CoreSolution.Dto;

namespace CoreSolution.AutoMapper.MapProfile
{
    public class MenuProfile : Profile, IProfile
    {
        public MenuProfile()
        {
            CreateMap<Menu, MenuDto>()
                .ForMember(i => i.CreatorUserName,
                    i => i.MapFrom(m => m.CreatorUser != null ? m.CreatorUser.UserName : null))
                .ForMember(i => i.DeleterUserName,
                    i => i.MapFrom(m => m.DeleterUser != null ? m.DeleterUser.UserName : null));
                //.ForMember(i => i.MenuItems, i => i.MapFrom(m => m.MenuItems.ToDictionary(d => d.Id, d => d.Name)));

            CreateMap<MenuDto, Menu>()
                .ForMember(i => i.IsDeleted, i => i.Ignore())
                .ForMember(i => i.CreationTime, i => i.Ignore());
            // .ForMember(i => i.MenuItems, i => i.MapFrom(m => m.MenuItems.Select(s => new MenuItem { MenuId = m.Id, Id = s.Key, Name = s.Value })));
        }
    }
}
