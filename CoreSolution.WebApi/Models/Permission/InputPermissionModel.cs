using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreSolution.WebApi.Models.Permission
{
    /// <summary>
    /// 权限参数model
    /// </summary>
    public class InputPermissionModel
    {
        /// <summary>
        /// 权限Id，新增是不用传，修改时必传
        /// </summary>
        public int? PermissionId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 权限描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 角色Id，该权限绑定到RoleId角色下面
        /// </summary>
        public int RoleId { get; set; }
    }
}
