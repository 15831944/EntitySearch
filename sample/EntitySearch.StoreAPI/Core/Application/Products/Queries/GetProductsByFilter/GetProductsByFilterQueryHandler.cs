﻿using EntitySearch.Extensions;
using EntitySearch.StoreAPI.Core.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EntitySearch.StoreAPI.Core.Application.Products.Queries.GetProductsByFilter
{
    public class GetProductsByFilterQueryHandler : IRequestHandler<GetProductsByFilterQuery, GetProductsByFilterQueryResponse>
    {
        private IStoreContext Context { get; set; }
        public GetProductsByFilterQueryHandler(IStoreContext context)
        {
            Context = context;
        }

        public async Task<GetProductsByFilterQueryResponse> Handle(GetProductsByFilterQuery request, CancellationToken cancellationToken)
        {
            int resultCount = 0;

            var results = await Context.Products
                .Filter(request)
                .Search(request)
                .Count(ref resultCount)
                .OrderBy(request)
                .Scope(request)
                .AsNoTracking()
                .ToListAsync(cancellationToken);            

            return new GetProductsByFilterQueryResponse
            {
                Request = request,
                ResultCount = resultCount,
                Results = results.Select(result=>new GetProductsByFilterQueryResponseItem
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
