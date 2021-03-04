using System;
using LoadBalancer.Domain.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoadBalancer.Web.Controllers
{
    /// <summary>
    /// Query response accessor.
    /// </summary>
    [Route("{controller}/{action}")]
    public class ResponseController : Controller
    {
        private readonly IResponseStorage _storage;

        public ResponseController(IResponseStorage storage)
        {
            _storage = storage;
        }
        
        /// <summary>
        /// Get response for request with <see cref="requestId"/>.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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