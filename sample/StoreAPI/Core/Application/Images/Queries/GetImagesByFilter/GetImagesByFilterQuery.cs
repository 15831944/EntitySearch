using EntitySearch;
using MediatR;
using StoreAPI.Core.Domain.Entities;

namespace StoreAPI.Core.Application.Images.Queries.GetImagesByFilter
{
    public class GetImagesByFilterQuery : Filter<Image>, IRequest<GetImagesByFilterQueryResponse>
    {
        public GetImagesByFilterQuery()
        {
            SetRestrictProperty(x => x.Product);
        }
    }
}
