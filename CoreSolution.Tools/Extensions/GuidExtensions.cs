using System;
using System.Collections.Generic;
using System.Text;

namespace CoreSolution.Tools.Extensions
{
    public static class GuidExtensions
    {
        public static bool IsNullOrEmpty(this Guid guid)
        {
            return guid == new Guid();
        }
    }
}
