using System.Collections.Generic;
using LoadBalancer.Models.Entities;
using LoadBalancer.Models.Enums;

namespace LoadBalancer.Domain.Storage
{
	/// <summary>
	/// Service to store server statistics.
	/// </summary>
	public interface IStatisticsStorage
	{
		/// <summary>
		/// Get stats for server for pool of <param name="type"></param>.
		/// </summary>
		IDictionary<Server, Statistics> Get(QueryType type);
		
		/// <summary>
		/// Update statistics for server.
		/// </summary>
		void Set(QueryType type, Server server, Statistics statistics);
		
		/// <summary>
		/// Get all stats.
		/// </summary>
		/// <remarks>
		/// Intended to observe server stats from API.
		/// </remarks>
		IEnumerable<(Server, Statistics)> GetAll();
	}
}