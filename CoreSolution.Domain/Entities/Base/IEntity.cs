using System;
using System.Collections.Generic;
using System.Text;

namespace CoreSolution.Domain.Entities.Base
{
    /// <summary>
    /// 定义主键类型为Int的基类接口Entity
    /// </summary>
    public interface IEntity : IEntity<int>
    {
    }

    /// <summary>
    /// 定义基类泛型接口Entity，系统中所有Entity必须实现这个接口
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
        bool IsDeleted { get; set; }
    }
}
