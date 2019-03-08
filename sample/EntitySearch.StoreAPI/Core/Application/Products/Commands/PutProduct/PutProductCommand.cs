using MediatR;
using System;

namespace EntitySearch.StoreAPI.Core.Application.Products.Commands.PutProduct
{
    public class PutProductCommand : IRequest<PutProductCommandResponse>
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public int Ammount { get; set; }
        public bool IsPublic { get; set; }

        public PutProductCommand()
        {

        }

        internal PutProductCommand Put(int id)
        {
            this.ProductID = id;
            return this;
        }
    }
}
