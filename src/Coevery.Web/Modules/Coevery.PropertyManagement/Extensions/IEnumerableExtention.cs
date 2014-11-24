using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;
using Microsoft.CSharp.RuntimeBinder;

namespace Coevery.PropertyManagement.Extensions {
    public static class EnumerableExtention {

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string sortBy, string sortOrder) {

            if (String.IsNullOrEmpty(sortBy)) {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(typeof (T), String.Empty);

            MemberExpression property = Expression.Property(parameter, sortBy);
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = sortOrder == "asc" ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression = Expression.Call(typeof (Enumerable), methodName, new[] {typeof (T), property.Type},
                                                              Expression.Constant(source), lambda);

            return (IEnumerable<T>) Expression.Lambda(methodCallExpression).Compile().DynamicInvoke();
        }
    }
}