using System;
using System.Collections.Generic;
using System.Text;

namespace CoreSolution.Dto.Base
{
    public class EntityBaseFullDto<TPrimaryKey> : EntityDto<TPrimaryKey>, IFullEntityDto
    {
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public DateTime? LastModificationTime { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
