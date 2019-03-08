using EntitySearch.StoreAPI.Core.Infrastructures.Data.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EntitySearch.StoreAPI.Core.Application.Products.Commands.PutProduct
{
    public class PutProductCommandHandler : IRequestHandler<PutProductCommand, PutProductCommandResponse>
    {
        private StoreContext Context { get; set; }
        public PutProductCommandHandler(StoreContext context)
        {
            Context = context;
        }
        public async Task<PutProductCommandResponse> Handle(PutProductCommand request, CancellationToken cancellationToken)
        {
            var product = await Context.Products.SingleOrDefaultAsync(x => x.ProductID == request.ProductID);

            product.Name = request.Name;
            product.Description = request.Description;
            product.Ammount = request.Ammount;
            product.Value = request.Value;
            product.IsPublic = request.IsPublic;

            await Context.SaveChangesAsync();

            return new PutProductCommandResponse
            {
                Request = request,
                Message = "Product successfully saved!",
                Result = new PutProductCommandResponseItem
                {
                    ProductID = product.ProductID,
                    Name = product.Name,
                    Description = product.Description,
                    Ammount = product.Ammount,
                    Value = product.Value,
                    RegistrationDate = product.RegistrationDate,
                    IsPublic = product.IsPublic
                }
            };
        }
    }
}
