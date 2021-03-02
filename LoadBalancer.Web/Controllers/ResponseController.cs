using System;
using LoadBalancer.Domain.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoadBalancer.Web.Controllers
{
    [Route("{controller}/{action}")]
    public class ResponseController : Controller
    {
        private readonly IResponseStorage _storage;

        public ResponseController(IResponseStorage storage)
        {
            _storage = storage;
        }
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