using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreSolution.WebApi.Models
{
    /// <summary>
    /// 分页数据model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListModel<T>
    {
        private int PageIndex { get; set; }

        private int PageSize { get; set; }

        public int Total { get; set; }

        public IList<T> List { get; set; }
    }
}
