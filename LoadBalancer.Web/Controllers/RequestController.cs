using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using LoadBalancer.Models.Entities;
using LoadBalancer.Domain.Distribution;
using LoadBalancer.Domain.Storage.Request;
using LoadBalancer.Models.Enums;
using LoadBalancer.Web.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoadBalancer.Web.Controllers
{
    /// <summary>
    /// Balancing pipeline accessor.
    /// </summary>
    [UnhandledExceptionCoverage]
    public class RequestController : Controller
    {
        private readonly IQueryDistributionService _service;
        private readonly IRequestQueue _queue;

        /// <inheritdoc />
        public RequestController(IQueryDistributionService service, IRequestQueue queue)
        {
            _service = service;
            _queue = queue;
        }

        /// <summary>
        /// Balance SQL query. 
        /// </summary>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Balance([FromBody, Required] RequestDto requestDto)
        {
            var result = await _service.DistributeQueryAsync(MapRequestFromDto(requestDto));
            return result.Result switch
            {
                QueryExecutionResult.QueryFailed => Problem(result.Message, statusCode: 400),
                QueryExecutionResult.QueryQueued => Ok(new {result.Result, result.RequestId}),
                _ => Ok(new {result.Result, result.Data})
            };
        }

        /// <summary>
        /// Purge request queue.
        /// </summary>
        /// <remarks>
        /// Test purposes only.
        /// </remarks>
        [HttpPost]
        [Route("purge")]
        public IActionResult PurgeQueue()
        {
            _queue.Purge();
            return Ok();
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