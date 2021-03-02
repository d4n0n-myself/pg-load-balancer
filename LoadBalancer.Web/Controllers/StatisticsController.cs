using System.Linq;
using LoadBalancer.Domain.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoadBalancer.Web.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly IStatisticsStorage _storage;

        public StatisticsController(IStatisticsStorage storage)
        {
            _storage = storage;
        }

        [Route("statistics")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok(_storage.GetAll().Select(arg =>
            {
                var (server, statistics) = arg;
                return $"{server.Name}: Online - {statistics.IsOnline}, Sessions - {statistics.CurrentSessionsCount}";
            }));
        }
    }
}