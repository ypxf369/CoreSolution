using System;
using System.Collections.Generic;
using System.Text;

namespace CoreSolution.Dto.Base
{
    public class EntityBaseFullDto : EntityDto<int>, IFullEntityDto
    {
        public override int Id { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public DateTime? LastModificationTime { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
