using EntitySearch.Interfaces;
using EntitySearch.StoreAPI.Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntitySearch.StoreAPI.Core.Application.Products.Queries.GetProductsByFilter
{
    public class GetProductsByFilterQuery : Filter<Product>, IRequest<GetProductsByFilterQueryResponse>
    {
        public GetProductsByFilterQuery() : base()
        {
            PageNumber = 0;
            PageSize = 10;
            OrderBy = "ProductID";
            Order = Order.ASCENDING;
            QueryStrict = false;
            QueryPhrase = false;
        }
    }
}
