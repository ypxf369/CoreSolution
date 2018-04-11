using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Dto.Base;

namespace CoreSolution.Dto
{
    public class RoleDto : EntityDto, IFullEntityDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CreatorUserId { get; set; }
        public string CreatorUserName { get; set; }
        public int? DeleterUserId { get; set; }
        public string DeleterUserName { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public DateTime? LastModificationTime { get; set; }
        public DateTime? DeletionTime { get; set; }
        public IList<UserDto> Users { get; set; }
        public IList<PermissionDto> Permissions { get; set; }
    }
}
