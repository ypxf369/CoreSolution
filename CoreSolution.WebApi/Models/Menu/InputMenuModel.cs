using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreSolution.WebApi.Models.Menu
{
    /// <summary>
    /// 菜单参数model
    /// </summary>
    public class InputMenuModel
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
        /// 菜单项
        /// </summary>
        public IList<InputMenuItemModel> MenuItems { get; set; }
    }
}
