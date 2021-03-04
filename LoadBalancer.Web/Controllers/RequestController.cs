using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using LoadBalancer.Models.Entities;
using LoadBalancer.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoadBalancer.Web.Controllers
{
    /// <summary>
    /// Balancing pipeline accessor.
    /// </summary>
    [Route("")]
    public class RequestController : Controller
    {
        private readonly IQueryDistributionService _service;

        public RequestController(IQueryDistributionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Balance SQL query. 
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromQuery, Required] Request request)
        {
            var result = await _service.DistributeQuery(request);
            return Ok();
            // return result.Success ? Ok(result) : Problem(result.Message, statusCode: 500);
        }
    }
}