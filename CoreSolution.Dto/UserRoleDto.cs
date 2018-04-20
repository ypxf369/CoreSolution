using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Dto.Base;

namespace CoreSolution.Dto
{
    public class UserRoleDto : EntityBaseFullDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
