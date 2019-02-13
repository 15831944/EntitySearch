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
        public PostProductCommandHandler()
        {
        }

        public async Task<PostProductCommandResponse> Handle(PostProductCommand request, CancellationToken cancellationToken)
        {
            return new PostProductCommandResponse
            {

            };
        }
    }
}
