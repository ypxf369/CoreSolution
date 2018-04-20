using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Dto.Base;

namespace CoreSolution.Dto
{
    public class MenuItemDto : EntityBaseFullDto
    {
        public string CustomData { get; set; }
        public string Icon { get; set; }
        public string ClassName { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int OrderIn { get; set; }
        public string RequiredPermissionName { get; set; }
        public bool RequiresAuthentication { get; set; }
        public int? CreatorUserId { get; set; }
        public string CreatorUserName { get; set; }
        public int? DeleterUserId { get; set; }
        public string DeleterUserName { get; set; }
        public int? MenuId { get; set; }
        public string MenuName { get; set; }
        public IList<MenuItemDto> MenuItems { get; set; }
    }
}
