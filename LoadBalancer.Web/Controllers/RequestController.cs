using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using LoadBalancer.Models.Entities;
using LoadBalancer.Domain.Distribution;
using LoadBalancer.Models.Enums;
using LoadBalancer.Web.Dto;
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

        /// <inheritdoc />
        public RequestController(IQueryDistributionService service) => _service = service;

        /// <summary>
        /// Balance SQL query. 
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery, Required] RequestDto requestDto)
        {
            var result = await _service.DistributeQueryAsync(MapRequestFromDto(requestDto));
            return result.Result switch
            {
                QueryExecutionResult.QueryFailed => Problem(result.Message, statusCode: 400),
                _ => Ok(result.Data),
            };
        }

        private static Request MapRequestFromDto(RequestDto requestDto)
        {
            return new()
            {
                Priority = requestDto.Priority,
                Query = requestDto.Query,
                Type = requestDto.Type,
                AcceptRetries = requestDto.AcceptRetries,
                IsSelect = requestDto.IsSelect,
            };
        }
    }
}