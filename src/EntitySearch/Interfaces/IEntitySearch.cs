using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EntitySearch.Interfaces
{
    public enum Order
    {
        ASCENDING,
        DESCENEDING
    }
    public interface IEntitySearch
    {
        Dictionary<string, object> FilterProperties { get; set; }
        string Query { get; set; }
        bool QueryStrict { get; set; }
        bool QueryPhrase { get; set; }
        List<string> QueryProperties { get; set; }
        int PageSize { get; set; }
        int PageNumber { get; set; }
        string OrderBy { get; set; }
        Order Order { get; set; }
        List<PropertyInfo> GetSearchableProperties(IList<PropertyInfo> typeProperties);
    }
    public interface IEntitySearch<TSource> : IEntitySearch
        where TSource : class
    {
        IEntitySearch<TSource> SetRestrictProperty<TKey>(Expression<Func<TSource, TKey>> keySelector);
    }
}
