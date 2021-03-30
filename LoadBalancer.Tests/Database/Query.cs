using System.Threading.Tasks;
using LoadBalancer.Database.Query;
using LoadBalancer.Models.Entities;
using NUnit.Framework;

namespace LoadBalancer.Tests.Database
{
    public class Query
    {
        private readonly QueryExecutor _queryRunner = new ();
        private readonly Server _server = new ();

        /// <summary>
        /// Run ExecuteAsync
        /// </summary>
        [Test]
        public async Task RunExecute()
        {
            await _queryRunner.ExecuteAsync(_server, "SELECT 1");
        }
        
        /// <summary>
        /// Run QueryAsync
        /// </summary>
        [Test]
        public async Task RunQuery()
        {
            var x = await _queryRunner.QueryAsync(_server, $@"SELECT 1 as ""SomeAlias""");
            Assert.AreEqual(x, "[{\"SomeAlias\":1}]");
        }
    }
}