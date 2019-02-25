using EntitySearch.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EntitySearch
{
    public abstract class Filter<TEntity> : IFilter<TEntity>
        where TEntity : class
    {
        public Dictionary<PropertyInfo, object> SuppliedProperties { get; set; }
        public string Query { get; set; }
        public bool QueryStrict { get; set; }
        public bool QueryPhrase { get; set; }
        public string QueryProperty { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string OrderBy { get; set; }
        public Order Order { get; set; }
    }
}
