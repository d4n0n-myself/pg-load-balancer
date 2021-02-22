using System;
using Microsoft.AspNetCore.Mvc;

namespace LoadBalancer.Web
{
    [Route("{controller}/{action}")]
    public class ResponseController : Controller
    {
        [HttpGet]
        public IActionResult GetResponse([FromQuery] Guid requestId)
        {
            throw new NotImplementedException();
        }
    }
}