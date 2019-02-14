using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntitySearch.StoreAPI.Core.Application.Products.Commands.PostProduct;
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
        public async Task<ActionResult<GetProductsByFilterQueryResponse>> Get([FromQuery]GetProductsByFilterQuery query)
        {
            try
            {
                return Ok(await Mediator.Send(query));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            try
            {
                return "value";
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<PostProductCommandResponse>> Post([FromBody] PostProductCommand command)
        {
            try
            {
                return Ok(await Mediator.Send(command));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            try
            {
                //return Ok("");
            }
            catch (Exception ex)
            {
                //return BadRequest(ex);
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            try
            {
                //
            }
            catch (Exception)
            {
                //
            }
        }
    }
}
