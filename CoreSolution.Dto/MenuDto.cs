using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Dto.Base;

namespace CoreSolution.Dto
{
    public class MenuDto : EntityBaseFullDto
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string CustomData { get; set; }
        public string Icon { get; set; }
        public string ClassName { get; set; }
        public int OrderIn { get; set; }
        public int? CreatorUserId { get; set; }
        public string CreatorUserName { get; set; }
        public int? DeleterUserId { get; set; }
        public string DeleterUserName { get; set; }
        public IList<MenuItemDto> MenuItems { get; set; }
    }
}
