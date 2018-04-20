using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Dto.Base;

namespace CoreSolution.Dto
{
    public class PermissionDto : EntityBaseFullDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CreatorUserId { get; set; }
        public string CreatorUserName { get; set; }
        public int? DeleterUserId { get; set; }
        public string DeleterUserName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
