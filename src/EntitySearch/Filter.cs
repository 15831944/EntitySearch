using EntitySearch.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EntitySearch
{
    [ModelBinder(BinderType = typeof(FilterBinder))]
    public class Filter<TEntity> : IFilter<TEntity>
        where TEntity : class
    {
        public Dictionary<string, object> FilterProperties { get; set; }
        private List<PropertyInfo> NonSearchableProperties { get; set; }
        public string Query { get; set; }
        public bool QueryStrict { get; set; }
        public bool QueryPhrase { get; set; }
        public List<string> QueryProperties { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string OrderBy { get; set; }
        public Order Order { get; set; }
        public Filter()
        {
            FilterProperties = new Dictionary<string, object>();
            QueryProperties = new List<string>();
            PageSize = 10;
            PageNumber = 0;
            Order = Order.ASCENDING;
        }
        public IFilter<TEntity> SetRestrictProperty<TKey>(Expression<Func<TEntity, TKey>> keySelector)
        {
            if (NonSearchableProperties == null)
                NonSearchableProperties = new List<PropertyInfo>();

            if (keySelector.Body is MemberExpression && (keySelector.Body as MemberExpression).Member is PropertyInfo)
                NonSearchableProperties.Add(((keySelector.Body as MemberExpression).Member as PropertyInfo));

            return this;
        }

        public List<PropertyInfo> GetSearchableProperties(IList<PropertyInfo> typeProperties)
        {
            return typeProperties.Where(propertyInfo =>
                !NonSearchableProperties.Any(nonSearchableProperty => nonSearchableProperty.Name == propertyInfo.Name)
            ).ToList();
        }
    }
}
