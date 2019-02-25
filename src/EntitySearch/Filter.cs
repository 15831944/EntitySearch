using EntitySearch.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EntitySearch
{
    //[ModelBinder(BinderType = typeof(FilterBinder))]
    public class Filter<TEntity> : IFilter<TEntity>
        where TEntity : class
    {
        public Dictionary<string, object> FilterProperties { get; set; }
        public string Query { get; set; }
        public bool QueryStrict { get; set; }
        public bool QueryPhrase { get; set; }
        public List<string> QueryProperty { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string OrderBy { get; set; }
        public Order Order { get; set; }
        public Filter()
        {
            FilterProperties = new Dictionary<string, object>();
            QueryProperty = new List<string>();
        }
    }
}
