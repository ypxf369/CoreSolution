using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Domain.Entities.Base;

namespace CoreSolution.Domain.Entities
{
    public class Role : EntityBaseFull
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CreatorUserId { get; set; }
        public virtual User CreatorUser { get; set; }
        public int? DeleterUserId { get; set; }
        public virtual User DeleterUser { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
