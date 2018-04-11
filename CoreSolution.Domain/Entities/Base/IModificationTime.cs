using System;
using System.Collections.Generic;
using System.Text;

namespace CoreSolution.Domain.Entities.Base
{
    /// <summary>
    /// 定义上次修改时间标识
    /// </summary>
    public interface IModificationTime
    {
        DateTime? LastModificationTime { get; set; }
    }
}
