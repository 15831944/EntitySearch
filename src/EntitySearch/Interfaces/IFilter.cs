using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EntitySearch.Interfaces
{
    public enum Order
    {
        ASCENDING,
        DESCENEDING
    }
    public interface IFilter
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

    }
    public interface IFilter<TSource> : IFilter
        where TSource : class
    {
    }
}
