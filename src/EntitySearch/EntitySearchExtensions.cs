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
            var containsExp = GenerateContainsExpression<TSource>(queryTokens);
            //var lambdaArgs = GenerateSearchLambdaExpression(GenerateContainsExpression());

            return source.Where(containsExp);
        }
        private static Expression<Func<TSource, bool>> GenerateContainsExpression<TSource>(List<string> tokens)
        {
            List<Expression> expressions = new List<Expression>();

            var xExp = Expression.Parameter(typeof(TSource), "x");
            
            foreach(var property in typeof(TSource).GetProperties())
            {
                var propertyExp = Expression.MakeMemberAccess(xExp, property);
                
                var method = property.PropertyType.GetMethods().SingleOrDefault(x => x.Name == "ToString" && x.GetParameters().Length == 0);
                var toStringExp = Expression.Call(propertyExp, method);

                method = typeof(string).GetMethods().SingleOrDefault(x => x.Name == "ToLower" && x.GetParameters().Length == 0);

                var toLowerExp = Expression.Call(toStringExp, method);

                method = typeof(string).GetMethods().SingleOrDefault(x => x.Name == "Contains" && x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == typeof(string));

                foreach(var token in tokens)
                {
                    var tokenExp = Expression.Constant(token, typeof(string));
                    var containsExp = Expression.Call(toLowerExp, method, tokenExp);
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

            return Expression.Lambda<Func<TSource, bool>>(orExp, xExp);
        }

        private static Expression<Func<TSource, bool>> GenerateContainsExpression2<TSource>(IQueryable<string> tokens)
        {
            var xExp = Expression.Parameter(typeof(TSource), "x");
            var yExp = Expression.Parameter(typeof(string), "y");

            var tokenExp = Expression.Constant(tokens, typeof(IQueryable<string>));

            var property = typeof(TSource).GetProperties()[1];

            var propertyExp = Expression.MakeMemberAccess(xExp, property);
            var method = property.PropertyType.GetMethods().SingleOrDefault(x => x.Name == "ToString" && x.GetParameters().Length == 0);
            var toStringExp = Expression.Call(propertyExp, method);

            method = typeof(string).GetMethods().SingleOrDefault(x => x.Name == "ToLower" && x.GetParameters().Length == 0);

            var toLowerExp = Expression.Call(toStringExp, method);

            method = typeof(string).GetMethods().SingleOrDefault(x => x.Name == "Contains" && x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == typeof(string));

            var containsExp = Expression.Call(toLowerExp, method, yExp);

            var yLambda = Expression.Lambda<Func<string, bool>>(containsExp, yExp);
            var methodAny = typeof(Queryable).GetMethods().SingleOrDefault(
                x => x.Name == "Any"
                && x.IsGenericMethodDefinition
                && x.GetGenericArguments().Length == 1
                && x.GetParameters().Length == 2);

            var anyExp = Expression.Call(tokenExp, methodAny, yLambda);

            return Expression.Lambda<Func<TSource, bool>>(containsExp, yExp);
        }
        private static Expression<Func<TSource,bool>> GenerateContainsExpression1<TSource>(List<string> tokens)
        {
            var xExp = Expression.Parameter(typeof(TSource), "x");
            var yExp = Expression.Parameter(typeof(string), "y");

            var constExp = Expression.Constant("madden", typeof(string));
            //var tokenExp = Expression.Constant(tokens, typeof(List<string>));

            var property = typeof(TSource).GetProperties()[1];

            var propertyExp = Expression.MakeMemberAccess(xExp, property);
            var method = property.PropertyType.GetMethods().SingleOrDefault(x => x.Name == "ToString" && x.GetParameters().Length == 0);
            var toStringExp = Expression.Call(propertyExp, method);

            method = typeof(string).GetMethods().SingleOrDefault(x => x.Name == "ToLower" && x.GetParameters().Length == 0);

            var toLowerExp = Expression.Call(toStringExp, method);

            method = typeof(string).GetMethods().SingleOrDefault(x => x.Name == "Contains" && x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == typeof(string));

            var containsExp = Expression.Call(toLowerExp, method, constExp);

            var yLambda = Expression.Lambda<Func<TSource, bool>>(containsExp, xExp);
            //var methodAny = typeof(Queryable).GetMethods().SingleOrDefault(
            //    x => x.Name == "Any"
            //    && x.IsGenericMethodDefinition
            //    && x.GetGenericArguments().Length == 1
            //    && x.GetParameters().Length == 2);
            
            //var anyExp = Expression.Call(tokenExp, methodAny, yLambda);

            //Expression<Func<TSource, bool>> args = (x) => tokens.Any(yLambda);

            return yLambda;
        }

        private static Expression GenerateSearchLambdaExpression<TSource>(Expression body, params ParameterExpression[] parameters)
        {
            var delegateType = typeof(Func<,>).MakeGenericType(typeof(TSource), typeof(bool));
            return Expression.Lambda(delegateType, body, parameters);
        }

        private static List<string> BreakQuery(string query)
        {
            return query.Split(" ").ToList();
        }

        public static IQueryable<TSource> Search_Old1<TSource>(this IQueryable<TSource> source, string query) where TSource : class
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
            var xExp = Expression.Parameter(type, "x");
            var yExp = Expression.Parameter(typeof(string), "y");

            var queryTokensExp = Expression.Parameter(typeof(IQueryable<string>), "queryTokens");

            var exps = new List<Expression>();

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                var propertyExp = Expression.MakeMemberAccess(xExp, property);
                type = property.PropertyType;
                var method = type.GetMethods().SingleOrDefault(x => x.Name == "ToString" && x.GetParameters().Length == 0);
                var toStringExp = Expression.Call(propertyExp, method);

                method = typeof(string).GetMethods().SingleOrDefault(x => x.Name == "Contains" && x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == typeof(string));
                                        
                var containsExp = Expression.Call(toStringExp, method, yExp);

                exps.Add(containsExp);
            }
            Expression orExp = Expression.Empty();
            if (exps.Count == 1)
                orExp = exps[0];
            else
            {
                orExp = Expression.Or(exps[0], exps[1]);

                for (int i = 2; i < exps.Count; i++)
                {
                    orExp = Expression.Or(orExp, exps[i]);
                }
            }
            var yLambda = Expression.Lambda<Func<string, bool>>(orExp, yExp);

            var methodAny = typeof(Queryable).GetMethods().SingleOrDefault(
                x => x.Name == "Any"
                && x.IsGenericMethodDefinition
                && x.GetGenericArguments().Length == 1
                && x.GetParameters().Length == 2);

            
            Type delegateType = typeof(Func<>).MakeGenericType(typeof(string));
            var lambda = Expression.Lambda(delegateType, orExp, xExp, yExp);

            methodAny.MakeGenericMethod(typeof(string))
                .Invoke(null, new object[] { queryTokens.AsQueryable(), lambda });

            methodAny.MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { yLambda });

            return null;
        }
    }
}
