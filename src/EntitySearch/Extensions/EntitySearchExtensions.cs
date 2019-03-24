using EntitySearch.Interfaces;
using EntitySearch.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EntitySearch.Extensions
{
    public static class EntitySearchExtensions
    {
        public static IQueryable<TSource> Filter<TSource>(this IQueryable<TSource> source, IEntitySearch<TSource> filter)
            where TSource : class
        {
            if (filter.FilterProperties == null || filter.FilterProperties.Count == 0)
            {
                return source;
            }

            var criteriaExp = GenerateFilterCriteriaExpression(filter);

            return source.Where(criteriaExp);
        }
        public static IQueryable<TSource> Search<TSource>(this IQueryable<TSource> source, IEntitySearch<TSource> filter)
            where TSource : class
        {
            if (string.IsNullOrWhiteSpace(filter.Query))
                return source;

            var queryTokens = TokenHelper.GetTokens(filter.Query, filter.QueryPhrase);

            filter.Query = string.Join("+", queryTokens.ToArray());

            if (queryTokens.Count == 0)
                return source;

            var criteriaExp = GenerateSearchCriteriaExpression(queryTokens, filter);

            return source.Where(criteriaExp);
        }
        public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, IEntitySearch<TSource> filter)
            where TSource : class
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (string.IsNullOrWhiteSpace(filter.OrderBy))
            {
                var someProperty = typeof(TSource).GetProperties().FirstOrDefault();
                if (someProperty==null)
                {
                    throw new ArgumentNullException(nameof(filter.OrderBy));
                }
                filter.OrderBy = someProperty.Name;
            }

            Type type = typeof(TSource);
            Expression lambda = ExpressionHelper.GenereteLambdaExpression<TSource>(ref type, filter.OrderBy);
            MethodInfo orderMethod;

            if (filter.Order == Order.ASCENDING)
            {
                orderMethod = ReflectionHelper.GetMethodFromType(typeof(Queryable), "OrderBy", 2, 2);
            }
            else
            {
                orderMethod = ReflectionHelper.GetMethodFromType(typeof(Queryable), "OrderByDescending", 2, 2);
            }

            return (IQueryable<TSource>)orderMethod.MakeGenericMethod(typeof(TSource), type).Invoke(null, new object[] { source, lambda });
        }
        public static IQueryable<TSource> Scope<TSource>(this IQueryable<TSource> source, IEntitySearch<TSource> filter)
            where TSource : class
        {
            return source.Skip(filter.PageNumber * filter.PageSize).Take(filter.PageSize);
        }
        public static IQueryable<TSource> Count<TSource>(this IQueryable<TSource> source, ref int count)
        {
            count = source.Count();
            return source;
        }
        public static IQueryable<TSource> CountLong<TSource>(this IQueryable<TSource> source, ref long count)
        {
            count = source.Count();
            return source;
        }
        private static Expression<Func<TSource, bool>> GenerateFilterCriteriaExpression<TSource>(IEntitySearch<TSource> filter) where TSource : class
        {
            List<Expression> expressions = new List<Expression>();

            var xExp = Expression.Parameter(typeof(TSource), "x");

            foreach (var filterProperty in filter.FilterProperties)
            {
                var propertyParts = filterProperty.Key.Split("_");
                var property = typeof(TSource).GetProperty(propertyParts[0]);

                Expression memberExp = Expression.MakeMemberAccess(xExp, property);

                if (propertyParts.Count() == 1)
                {
                    expressions.Add(ExpressionHelper.GenerateFilterComparationExpression(memberExp, filterProperty, property));
                }
                else
                {
                    expressions.Add(ExpressionHelper.GenerateFilterComparationExpression(memberExp, filterProperty, property, propertyParts[1]));
                }
            }

            Expression orExp = ExpressionHelper.GenerateAndExpressions(expressions);

            return Expression.Lambda<Func<TSource, bool>>(orExp.Reduce(), xExp);
        }
        private static Expression<Func<TSource, bool>> GenerateSearchCriteriaExpression<TSource>(IList<string> tokens, IEntitySearch<TSource> filter)
            where TSource : class
        {
            List<Expression> orExpressions = new List<Expression>();

            var xExp = Expression.Parameter(typeof(TSource), "x");

            foreach (var propertyInfo in ReflectionHelper.GetPropertiesFromType(filter.GetSearchableProperties(typeof(TSource).GetProperties()), filter.QueryProperties))
            {
                Expression memberExp = Expression.MakeMemberAccess(xExp, propertyInfo);
                Expression memberHasValue = null;
                if (memberExp.Type != typeof(string))
                {
                    if(Nullable.GetUnderlyingType(memberExp.Type) != null)
                    {
                        memberHasValue = Expression.MakeMemberAccess(memberExp, memberExp.Type.GetProperty("HasValue"));
                        memberHasValue = filter.QueryStrict ? memberHasValue : Expression.Not(memberHasValue);
                    }
                    else if (memberExp.Type.IsClass)
                    {
                        memberHasValue = Expression.Equal(memberExp, Expression.Constant(null));
                        memberHasValue = !filter.QueryStrict ? memberHasValue : Expression.Not(memberHasValue);
                    }
                    memberExp = Expression.Call(memberExp, ReflectionHelper.GetMethodFromType(memberExp.Type, "ToString", 0, 0));
                }
                else
                {
                    memberHasValue = Expression.Equal(memberExp, Expression.Constant(null));
                    memberHasValue = !filter.QueryStrict ? memberHasValue : Expression.Not(memberHasValue);
                }

                memberExp = Expression.Call(memberExp, ReflectionHelper.GetMethodFromType(memberExp.Type, "ToLower", 0, 0));
                List<Expression> andExpressions = new List<Expression>();
                
                foreach (var token in tokens)
                {
                    andExpressions.Add(ExpressionHelper.GenerateStringContainsExpression(memberExp, Expression.Constant(token, typeof(string))));
                }
                orExpressions.Add(filter.QueryStrict ? ExpressionHelper.GenerateAndExpressions(andExpressions) : ExpressionHelper.GenerateOrExpression(andExpressions));
            }

            Expression orExp = ExpressionHelper.GenerateOrExpression(orExpressions);

            return Expression.Lambda<Func<TSource, bool>>(orExp.Reduce(), xExp);
        }
    }
}
