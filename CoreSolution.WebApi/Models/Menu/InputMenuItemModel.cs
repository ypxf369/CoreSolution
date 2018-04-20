using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreSolution.WebApi.Models.Menu
{
    /// <summary>
    /// 菜单项参数model
    /// </summary>
    public class InputMenuItemModel
    {
        /// <summary>
        /// 菜单显示名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public string CustomData { get; set; }
        /// <summary>
        /// Icon
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// ClassName
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderIn { get; set; }
        /// <summary>
        /// 是否启用菜单权限验证
        /// </summary>
        public bool RequiresAuthentication { get; set; }
        /// <summary>
        /// 该菜单项对应权限名称（只有具有该权限的用户才会显示该菜单）
        /// </summary>
        public string RequiredPermissionName { get; set; }
        /// <summary>
        /// 子菜单项
        /// </summary>
        public IList<InputMenuItemModel> MenuItems { get; set; }

    }
}
