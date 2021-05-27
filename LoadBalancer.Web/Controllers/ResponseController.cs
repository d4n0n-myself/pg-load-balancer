using System;
using LoadBalancer.Domain.Storage.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoadBalancer.Web.Controllers
{
    /// <summary>
    /// Query response accessor.
    /// </summary>
    [UnhandledExceptionCoverage]
    public class ResponseController : Controller
    {
        private readonly IResponseStorage _storage;

        /// <inheritdoc />
        public ResponseController(IResponseStorage storage) => _storage = storage;

        /// <summary>
        /// Get response for request with <paramref name="requestId"></paramref>.
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("response")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetResponse([FromQuery] Guid requestId)
        {
            if (_storage.TryGetResponseByRequestId(requestId, out var response))
            {
                return Ok(response);
            }

            return NoContent();
        }
    }
}