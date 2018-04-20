using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Dto.Base;

namespace CoreSolution.Dto
{
    public class RoleDto : EntityBaseFullDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CreatorUserId { get; set; }
        public string CreatorUserName { get; set; }
        public int? DeleterUserId { get; set; }
        public string DeleterUserName { get; set; }
        public IDictionary<int, string> Users { get; set; }
        public IList<PermissionDto> Permissions { get; set; }
    }
}
