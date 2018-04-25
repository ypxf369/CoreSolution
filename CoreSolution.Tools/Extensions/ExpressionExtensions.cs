using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CoreSolution.Tools.Extensions
{
    /// <summary>
    /// Expression扩展方法
    /// </summary>
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> True<T>() { return i => true; }
        public static Expression<Func<T, bool>> False<T>() { return i => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            var invokedExpr = Expression.Invoke(right, left.Parameters);
            return Expression.Lambda<Func<T, bool>>
                (Expression.Or(left.Body, invokedExpr), left.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> rigth)
        {
            var invokedExpr = Expression.Invoke(rigth, left.Parameters);
            return Expression.Lambda<Func<T, bool>>
                (Expression.And(left.Body, invokedExpr), left.Parameters);
        }
    }
}
