using EntitySearch.StoreAPI.Core.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EntitySearch.StoreAPI.Core.Application.Products.Commands.PutProduct
{
    public class PutProductCommandHandler : IRequestHandler<PutProductCommand, PutProductCommandResponse>
    {
        private IStoreContext Context { get; set; }
        public PutProductCommandHandler(IStoreContext context)
        {
            Context = context;
        }
        public async Task<PutProductCommandResponse> Handle(PutProductCommand request, CancellationToken cancellationToken)
        {
            var product = await Context.Products.SingleOrDefaultAsync(x => x.ProductID == request.ID);

            request.Put(product);
            //product.Name = request.Name;
            //product.Description = request.Description;
            //product.Ammount = request.Ammount;
            //product.Value = request.Value;
            //product.IsPublic = request.IsPublic;

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
