using System;
using System.ComponentModel.DataAnnotations;
using LoadBalancer.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoadBalancer.Web
{
    [Route("")]
    public class RequestController : Controller
    {
        public IActionResult Get([FromQuery, Required] Request request)
        {
            throw new NotImplementedException();
        }
    }
}