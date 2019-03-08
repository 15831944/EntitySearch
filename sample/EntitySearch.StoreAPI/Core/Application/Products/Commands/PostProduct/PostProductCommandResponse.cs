using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntitySearch.StoreAPI.Core.Application.Products.Commands.PostProduct
{
    public class PostProductCommandResponse
    {
        public PostProductCommand Request { get; set; }
        public string Message { get; set; }
        public PostProductCommandResponseItem Result { get; set; }
    }
}
