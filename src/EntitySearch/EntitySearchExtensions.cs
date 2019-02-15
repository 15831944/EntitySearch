using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EntitySearch
{
    public static class EntitySearchExtensions
    {
        public static IQueryable<TSource> Search<TSource>(this IQueryable<TSource> source, string query) where TSource : class
        {
            if (string.IsNullOrWhiteSpace(query))
                return source;

            var properties = typeof(TSource).GetProperties();

            var queryTokens = query.Split(" ").ToList();

            var queryTokensExpression = queryTokens.AsQueryable();
            Type type = typeof(TSource);
            var expression = GenereteContainsLambdaExpression<TSource>(ref type, queryTokens);

            source = source.Where(x => properties.Any(y => queryTokens.Any(q => y.GetValue(x).ToString().ToLower().Contains(q.ToLower()))));

            return source;
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

        
        public static LambdaExpression GenereteContainsLambdaExpression<T>(ref Type type, List<string> queryTokens)
        {
            ParameterExpression parameterExp = Expression.Parameter(type, "x");

            var queryTokensExp = Expression.Constant(queryTokens.AsQueryable(), typeof(IQueryable<string>));

            Expression exp = parameterExp;

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                var propertyExp = Expression.Property(exp, property);
                exp = Expression.MakeMemberAccess(exp, property);
                type = property.PropertyType;
                MethodInfo method = type.GetMethod("Contains");
                
                if(method == null)
                {
                    method = type.GetMethods().SingleOrDefault(x => x.Name == "ToString" && x.GetParameters().Length == 0);
                    var toStringExp = Expression.Call(propertyExp, method);

                    var yExp = Expression.Parameter(typeof(string),"y");

                    method = typeof(string).GetMethods().SingleOrDefault(x => x.Name == "Contains" && x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == typeof(string));
                                        
                    var containsExp = Expression.Call(toStringExp, method, yExp);

                    method = typeof(Queryable).GetMethods().SingleOrDefault(
                            x => x.Name == "Any"
                            && x.IsGenericMethodDefinition
                            && x.GetGenericArguments().Length == 1
                            && x.GetParameters().Length == 2);

                    var yLambda = Expression.Lambda<Func<string,bool>>(containsExp, yExp);
                    var xLambda = Expression.Lambda<Func<T, bool>>(yLambda, parameterExp);
                    var anyExp = Expression.Call(queryTokensExp, method, yLambda);
                }
                else
                {
                    string token = "text";
                    var constantExp = Expression.Constant(token);
                    var containsExp = Expression.Call(propertyExp, method, constantExp);
                }
                //Expression.Call(propertyExp, )
            }

            return Expression.Lambda<Func<T, bool>>(exp, parameterExp);
        }
    }
}
