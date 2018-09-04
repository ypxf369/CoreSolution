using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Dto.Base;

namespace CoreSolution.Dto
{
    public class UserRoleDto : EntityBaseFullDto<int>
    {
        public int UserId { get; set; }
        public UserDto User { get; set; }
        public int RoleId { get; set; }
        public RoleDto Role { get; set; }
    }
}
