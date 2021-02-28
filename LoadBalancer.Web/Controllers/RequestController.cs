using System;
using System.ComponentModel.DataAnnotations;
using LoadBalancer.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoadBalancer.Web.Controllers
{
    [Route("")]
    public class RequestController : Controller
    {
        [HttpGet]
        public IActionResult Get([FromQuery, Required] Request request)
        {
            throw new NotImplementedException();
        }
    }
}