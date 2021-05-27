using System.Linq;
using LoadBalancer.Domain.Storage.Statistics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoadBalancer.Web.Controllers
{
    /// <summary>
    /// Statistics accessor.
    /// </summary>
    [UnhandledExceptionCoverage]
    public class StatisticsController : Controller
    {
        private readonly IStatisticsStorage _storage;

        /// <inheritdoc />
        public StatisticsController(IStatisticsStorage storage) => _storage = storage;

        /// <summary>
        /// Get statistics for all servers. No format, pretty-print.
        /// </summary>
        [Route("statistics")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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