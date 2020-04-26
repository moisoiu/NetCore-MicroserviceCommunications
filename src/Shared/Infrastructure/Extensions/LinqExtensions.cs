using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> ConditionalWhere<T>(this IQueryable<T> query, bool trueCondition, Expression<Func<T, bool>> expression)
        {
            if (trueCondition)
            {
                query = query.Where(expression);
            }

            return query;
        }
    }
}
