using System;
using System.Collections.Concurrent;
using LoadBalancer.Models.Entities;

namespace LoadBalancer.Domain.Storage
{
	public class ResponseStorage : IResponseStorage
	{
		private readonly ConcurrentDictionary<Guid, Response> _responses = new();

		public void Add(Response response)
		{
			_responses.TryAdd(response.RequestId, response);
		}

		public bool TryGetResponseByRequestId(Guid requestId, out object response)
		{
			if (_responses.TryGetValue(requestId, out var result))
			{
				response = result;
				return true;
			}

			response = null;
			return false;
		}
	}
}