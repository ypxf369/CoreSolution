using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Domain.Entities.Base;

namespace CoreSolution.Domain.Entities
{
    public class Role : Entity, IFullEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CreatorUserId { get; set; }
        /// <summary>
        /// 该角色的创建者
        /// </summary>
        public virtual User CreatorUser { get; set; }
        public int? DeleterUserId { get; set; }
        /// <summary>
        /// 该角色的删除者
        /// </summary>
        public virtual User DeleterUser { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public DateTime? LastModificationTime { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
