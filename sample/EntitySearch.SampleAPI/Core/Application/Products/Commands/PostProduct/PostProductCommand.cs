using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntitySearch.StoreAPI.Core.Application.Products.Commands.PostProduct
{
    public class PostProductCommand : IRequest<PostProductCommandResponse>
    {
        public PostProductCommand()
        {
        }
    }
}
