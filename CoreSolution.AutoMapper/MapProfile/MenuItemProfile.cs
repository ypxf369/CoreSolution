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
            CreateMap<MenuItem, MenuItemDto>()
                .ForMember(i => i.CreatorUserName,
                    i => i.MapFrom(m => m.CreatorUser != null ? m.CreatorUser.UserName : null))
                .ForMember(i => i.DeleterUserName,
                    i => i.MapFrom(m => m.DeleterUser != null ? m.DeleterUser.UserName : null))
                .ForMember(i => i.MenuName, i => i.MapFrom(m => m.Menu != null ? m.Menu.Name : null));
                //.ForMember(i => i.MenuItems, i => i.MapFrom(m => m.MenuItems.Select(s => new { s.CustomData, s.Icon, s.ClassName, s.Name, s.Url, s.OrderIn, s.RequiredPermissionName, s.RequiresAuthentication }).ToList()));

            CreateMap<MenuItemDto, MenuItem>()
                .ForMember(i => i.IsDeleted, i => i.Ignore())
                .ForMember(i => i.CreationTime, i => i.Ignore());
            //.ForMember(i => i.MenuItems, i => i.MapFrom(m => m.MenuItems.Select(s => new MenuItem { MenuId = m.Id, Id = s., Name = s.Value })));
        }
    }
}
