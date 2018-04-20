using System;
using System.Collections.Generic;
using System.Text;

namespace CoreSolution.Dto.Base
{
    /// <summary>
    /// 主键为Int的基类EntityDto
    /// </summary>
    public class EntityDto : EntityDto<int>, IEntityDto
    {
        public EntityDto()
        {

        }

        public EntityDto(int id)
            : base(id)
        {
        }
    }

    /// <summary>
    /// 泛型主键的基类EntityDto
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public class EntityDto<TPrimaryKey> : IEntityDto<TPrimaryKey>
    {
        public virtual TPrimaryKey Id { get; set; }

        public EntityDto()
        {

        }

        public EntityDto(TPrimaryKey id)
        {
            Id = id;
        }
    }
}
