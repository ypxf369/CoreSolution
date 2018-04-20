using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreSolution.WebApi.Models.Permission;

namespace CoreSolution.WebApi.Models.Role
{
    /// <summary>
    /// 角色参数model
    /// </summary>
    public class InputRoleModel
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 该角色所对应的权限列表
        /// </summary>
        public IList<InputPermissionModel> Permissions { get; set; }
    }
}
