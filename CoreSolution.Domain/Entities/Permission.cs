using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Domain.Entities.Base;

namespace CoreSolution.Domain.Entities
{
    public class Permission : EntityBaseFull<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CreatorUserId { get; set; }
        public virtual User CreatorUser { get; set; }
        public int? DeleterUserId { get; set; }
        public virtual User DeleterUser { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
