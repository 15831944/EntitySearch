using EntitySearch.StoreAPI.Core.Domain.Entities;
using EntitySearch.StoreAPI.Core.Infrastructures.Data.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EntitySearch.StoreAPI.Core.Application.Products.Queries.GetProductsByFilter
{
    public class GetProductsByFilterQueryHandler : IRequestHandler<GetProductsByFilterQuery, GetProductsByFilterQueryResponse>
    {
        private StoreContext Context { get; set; }
        public GetProductsByFilterQueryHandler(StoreContext context)
        {
            Context = context;
        }

        public async Task<GetProductsByFilterQueryResponse> Handle(GetProductsByFilterQuery request, CancellationToken cancellationToken)
        {
            var query = Context.Products.AsQueryable();
            query = query.Search(request.Query);
            
            var resultCount = await query.CountAsync(cancellationToken);
            //var results = await query.ToListAsync(cancellationToken);

            //Expression<Func<Product, bool>> expression1 = (x) => x.Name.ToLower().Contains("madden");

            //var results = await query.Where(expression1).ToListAsync(cancellationToken);
            //var results = await query.Where(x=>x.Name.ToLower().Contains("madden")).ToListAsync(cancellationToken);
            var results = await query.ToListAsync(cancellationToken);

            return new GetProductsByFilterQueryResponse
            {
                Request = request,
                ResultCount = resultCount,
                Results = results.Select(result=>new GetProductsByFilterQueryItemResponse
                {
                    ProductID = result.ProductID,
                    Name = result.Name,
                    Description = result.Description,
                    RegistrationDate = result.RegistrationDate,
                    Value = result.Value,
                    Ammount = result.Ammount,
                    IsPublic = result.IsPublic
                }).ToList()
            };
        }
    }
}
