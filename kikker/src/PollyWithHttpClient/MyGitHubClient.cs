﻿using System;
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
		}

		public HttpClient Client { get; }
	}
}
