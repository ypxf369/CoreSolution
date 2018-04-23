using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreSolution.Domain.Enum;

namespace CoreSolution.WebApi.Models.User
{
    /// <summary>
    /// 用户注册参数model
    /// </summary>
    public class InputRegisterModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 真实姓名（可选）
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 注册类型（1分配，2用户注册）
        /// </summary>
        public RegisterType RegisterType { get; set; }
        /// <summary>
        /// 角色Id数组
        /// </summary>
        public int[] Roles { get; set; }
    }
}
