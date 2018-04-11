using System;
using System.Collections.Generic;
using System.Text;

namespace CoreSolution.Domain.Entities.Base
{
    /// <summary>
    /// 创建时间、修改时间、删除时间、软删除标识
    /// </summary>
    public interface IFullEntity : ICreationTime, IModificationTime, IDeletionTime
    {
    }
}
