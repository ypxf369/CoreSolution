using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Domain.Entities.Base;

namespace CoreSolution.Domain.Entities
{
    public class User : Entity, IFullEntity
    {
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string PhoneNum { get; set; }
        public bool IsPhoneNumConfirmed { get; set; }
        public string Password { get; set; }
        public int? CreatorUserId { get; set; }
        /// <summary>
        /// 该用户的创建者
        /// </summary>
        public virtual User CreatorUser { get; set; }
        public int? DeleterUserId { get; set; }
        /// <summary>
        /// 该用户的删除者
        /// </summary>
        public virtual User DeleterUser { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLocked { get; set; } = false;
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public DateTime? LastModificationTime { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
