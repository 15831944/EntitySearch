using EntitySearch.StoreAPI.Core.Domain.Entities;
using MediatR;
using ModelWrapper;
using System;

namespace EntitySearch.StoreAPI.Core.Application.Products.Commands.PutProduct
{
    public class PutProductCommand : Wrap<Product>, IRequest<PutProductCommandResponse>
    {
        public int ID { get; set; }
        public PutProductCommand()
        {

        }

        internal PutProductCommand Put(int id)
        {
            this.ID = id;
            
            return this;
        }
    }
}
