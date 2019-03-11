using EntitySearch.StoreAPI.Core.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EntitySearch.StoreAPI.Core.Application.Products.Queries.GetProductByID
{
    public class GetProductByIDQueryHandler : IRequestHandler<GetProductByIDQuery, GetProductByIDQueryResponse>
    {

        private IStoreContext Context { get; set; }
        public GetProductByIDQueryHandler(IStoreContext context)
        {
            Context = context;
        }

        public async Task<GetProductByIDQueryResponse> Handle(GetProductByIDQuery request, CancellationToken cancellationToken)
        {
            var product = await Context.Products.SingleOrDefaultAsync(x => x.ProductID == request.ProductID);

            return new GetProductByIDQueryResponse
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Description = product.Description,
                RegistrationDate = product.RegistrationDate,
                Value = product.Value,
                Ammount = product.Ammount,
                IsPublic = product.IsPublic
            };
        }
    }
}
