using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;

namespace CoreSolution.Tools.Extensions
{
    public static class ExceptionExtensions
    {
        public static void ReThrow(this Exception exception)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }
}
