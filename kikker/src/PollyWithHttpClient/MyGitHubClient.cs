using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PollyWithHttpClient
{
	public class MyGitHubClient
	{
		public MyGitHubClient(HttpClient client)
		{
			Client = client;
			client.DefaultRequestHeaders.Add("X-Correlation-ID", Guid.NewGuid().ToString());
			client.DefaultRequestHeaders.Add("X-API-KEY", "kikker-api");
		}

		public HttpClient Client { get; }
	}
}
