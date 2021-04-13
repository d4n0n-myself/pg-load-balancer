using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using LoadBalancer.Models.Entities;
using LoadBalancer.Domain.Distribution;
using LoadBalancer.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoadBalancer.Web.Controllers
{
    /// <summary>
    /// Balancing pipeline accessor.
    /// </summary>
    [Route("")] 
    [UnhandledExceptionCoverage]
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery, Required] Request request)
        {
            var result = await _service.DistributeQueryAsync(request);
            return result.Result switch
            {
                QueryExecutionResult.QueryFailed => Problem(result.Message, statusCode: 400),
                _ => Ok(result.Data),
            };
        }
    }
}