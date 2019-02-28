using EntitySearch.StoreAPI.Core.Domain.Entities;
using MediatR;

namespace EntitySearch.StoreAPI.Core.Application.Products.Queries.GetProductsByFilter
{
    public class GetProductsByFilterQuery : Filter<Product>, IRequest<GetProductsByFilterQueryResponse>
    {
        public GetProductsByFilterQuery() : base()
        {
            OrderBy = "ProductID";
            this.SetRestrictProperty(x => x.Description)
                .SetRestrictProperty(x => x.IsPublic);
        }
    }
}
