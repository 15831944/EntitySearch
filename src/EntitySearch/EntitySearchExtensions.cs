using System;
using System.Linq;

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

            source = source.Where(x => properties.Any(y => queryTokens.Any(q => y.GetValue(x).ToString().ToLower().Contains(q.ToLower()))));

            return source;
        }
    }
}
