using System;
using System.Collections.Generic;
using System.Text;

namespace CoreSolution.Domain.Entities.Base
{
    public class EntityBaseFull : Entity<int>, IFullEntity
    {
        public override int Id { get; set; }
        public override bool IsDeleted { get; set; } = false;
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public DateTime? LastModificationTime { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
