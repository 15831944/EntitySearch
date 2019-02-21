using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EntitySearch
{
    public static class EntitySearchExtensions
    {
        public static IQueryable<TSource> Search<TSource>(this IQueryable<TSource> source, string query)
            where TSource : class
        {
            if (string.IsNullOrWhiteSpace(query))
                return source;

            var queryTokens = BreakQuery(query);
            var criteriaExp = GenerateSearchCriteriaExpression<TSource>(queryTokens);

            return source.Where(criteriaExp);
        }
        private static List<string> BreakQuery(string query)
        {
            return query.ToLower().Split(" ").ToList();
        }
        private static MethodInfo GetMethodFromType(Type type, string methodName, int parameters, int genericArguments, List<Type> parameterTypes = null)
        {
            return type.GetMethods()
                .SingleOrDefault(method =>
                    method.Name == methodName
                    && method.GetParameters().Count() == parameters
                    && method.GetGenericArguments().Count() == genericArguments
                    && (parameterTypes == null || parameterTypes.All(x=>method.GetParameters().Select(parameter=>parameter.ParameterType).Contains(x)))
                );
        }

        private static Expression<Func<TSource, bool>> GenerateSearchCriteriaExpression<TSource>(List<string> tokens)
        {
            List<Expression> expressions = new List<Expression>();

            var xExp = Expression.Parameter(typeof(TSource), "x");
            
            foreach(var propertyInfo in typeof(TSource).GetProperties())
            {
                Expression memberExp = Expression.MakeMemberAccess(xExp, propertyInfo);

                if(memberExp.Type !=typeof(string))
                    memberExp = Expression.Call(memberExp, GetMethodFromType(memberExp.Type, "ToString", 0, 0));

                memberExp = Expression.Call(memberExp, GetMethodFromType(memberExp.Type, "ToLower", 0, 0));
                
                foreach(var token in tokens)
                {
                    var tokenExp = Expression.Constant(token, typeof(string));
                    var containsExp = Expression.Call(memberExp, GetMethodFromType(memberExp.Type,"Contains",1,0, new List<Type> { typeof(string) }), tokenExp);
                    expressions.Add(containsExp);
                }
            }

            Expression orExp = Expression.Empty();
            if (expressions.Count == 1)
                orExp = expressions[0];
            else
            {
                orExp = Expression.Or(expressions[0], expressions[1]);

                for (int i = 2; i < expressions.Count; i++)
                {
                    orExp = Expression.Or(orExp, expressions[i]);
                }
            }

            return Expression.Lambda<Func<TSource, bool>>(orExp.Reduce(), xExp);
        }
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string order, string orderBy)
        {
            if (order.ToLower() == "ASCENDING".ToLower())
            {
                return source.OrderBy(orderBy);
            }
            else
            {
                return source.OrderByDescending(orderBy);
            }
        }
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException("propertyName");

            Type type = typeof(T);
            LambdaExpression lambda = GenereteLambdaExpression<T>(ref type, propertyName);

            object result = typeof(Queryable).GetMethods().Single(
                method => method.Name == "OrderBy"
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 2
                        && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { source, lambda });
            return (IQueryable<T>)result;
        }
        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException("propertyName");

            Type type = typeof(T);
            LambdaExpression lambda = GenereteLambdaExpression<T>(ref type, propertyName);

            object result = typeof(Queryable).GetMethods().Single(
                method => method.Name == "OrderByDescending"
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 2
                        && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { source, lambda });
            return (IQueryable<T>)result;
        }
        public static LambdaExpression GenereteLambdaExpression<T>(ref Type type, string propertyName)
        {
            ParameterExpression arg = Expression.Parameter(type, "x");
            List<string> listProperties = propertyName.Split('.').ToList();

            Expression expr = arg;
            foreach (string item in listProperties)
            {
                PropertyInfo np = type.GetProperty(item);
                expr = Expression.MakeMemberAccess(expr, np);
                type = np.PropertyType;
            }

            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            return Expression.Lambda(delegateType, expr, arg);
        }
    }
}
