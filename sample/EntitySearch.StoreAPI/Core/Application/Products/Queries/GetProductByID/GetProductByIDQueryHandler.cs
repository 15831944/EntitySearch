using EntitySearch.StoreAPI.Core.Domain.Entities;
using EntitySearch.StoreAPI.Core.Infrastructures.Data.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EntitySearch.StoreAPI.Core.Application.Products.Queries.GetProductByID
{
    public class GetProductByIDQueryHandler : IRequestHandler<GetProductByIDQuery, GetProductByIDQueryResponse>
    {

        private StoreContext Context { get; set; }
        public GetProductByIDQueryHandler(StoreContext context)
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
