using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Domain.Entities.Base;

namespace CoreSolution.Domain.Entities
{
    public class MenuItem : EntityBaseFull
    {
        /// <summary>
        /// 自定义数据
        /// </summary>
        public string CustomData { get; set; }

        public string Icon { get; set; }
        public string ClassName { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderIn { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string RequiredPermissionName { get; set; }

        /// <summary>
        /// 权限验证如果通过验证显示此菜单否则不可见
        /// </summary>
        public bool RequiresAuthentication { get; set; }
        public int? CreatorUserId { get; set; }
        public virtual User CreatorUser { get; set; }
        public int? DeleterUserId { get; set; }
        public virtual User DeleterUser { get; set; }
        public int? MenuId { get; set; }
        public virtual Menu Menu { get; set; }
        public int? MenuItemId { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
