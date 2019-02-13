using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntitySearch.StoreAPI.Core.Application.Products.Queries.GetProductsByFilter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EntitySearch.StoreAPI.Resources
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IMediator Mediator { get; set; }
        public ProductsController(IMediator mediator)
        {
            Mediator = mediator;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<GetProductsByFilterQueryResponse>> Get(GetProductsByFilterQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
