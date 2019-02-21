using System;
using System.Collections.Generic;
using System.Text;

namespace EntitySearch.Interfaces
{
    public enum Order
    {
        ASCENDING,
        DESCENEDING
    }
    public interface IFilter<TSource> where TSource : class
    {
        string Query { get; set; }
        int PageSize { get; set; }
        int PageNumber { get; set; }
        string OrderBy { get; set; }
        Order Order { get; set; }
    }
}
