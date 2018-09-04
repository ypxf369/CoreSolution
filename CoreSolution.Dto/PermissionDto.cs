using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Dto.Base;

namespace CoreSolution.Dto
{
    public class PermissionDto : EntityBaseFullDto<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CreatorUserId { get; set; }
        public UserDto CreatorUser { get; set; }
        public int? DeleterUserId { get; set; }
        public UserDto DeleterUser { get; set; }
        public int RoleId { get; set; }
        public RoleDto Role { get; set; }
    }
}
