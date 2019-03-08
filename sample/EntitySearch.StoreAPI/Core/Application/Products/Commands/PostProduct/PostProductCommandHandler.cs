using EntitySearch.StoreAPI.Core.Domain.Entities;
using EntitySearch.StoreAPI.Core.Infrastructures.Data.Contexts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EntitySearch.StoreAPI.Core.Application.Products.Commands.PostProduct
{
    public class PostProductCommandHandler : IRequestHandler<PostProductCommand, PostProductCommandResponse>
    {
        private StoreContext Context { get; set; }
        public PostProductCommandHandler(StoreContext context)
        {
            Context = context;
        }

        public async Task<PostProductCommandResponse> Handle(PostProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Ammount = request.Ammount,
                RegistrationDate = DateTime.UtcNow,
                Value = request.Value,
                IsPublic = request.IsPublic
            };

            await Context.Products.AddAsync(product);

            await Context.SaveChangesAsync();

            return new PostProductCommandResponse
            {
                Request = request,
                Message = "Product successfully registered!",
                Result = new PostProductCommandResponseItem
                {
                    ProductID = product.ProductID,
                    Name = product.Name,
                    Description = product.Description,
                    Value = product.Value,
                    Ammount = product.Ammount,
                    RegistrationDate = product.RegistrationDate,
                    IsPublic = product.IsPublic                
                }
            };
        }
    }
}
