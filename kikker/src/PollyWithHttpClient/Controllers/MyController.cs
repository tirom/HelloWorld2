using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PollyWithHttpClient.Controllers
{
	[Route("api/my")]
	public class MyController : Controller
    {
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly MyGitHubClient _gitHubClient;

		public MyController(IHttpClientFactory httpClientFactory, MyGitHubClient gitGitHubClient)
		{
			_httpClientFactory = httpClientFactory;
			_gitHubClient = gitGitHubClient;
		}

		[HttpGet]
		public async Task<IActionResult> SomeActionAsync()
		{
			// Get an HttpClient configured to the specification you defined in StartUp.
			//var client = _httpClientFactory.CreateClient("TestGitHub");
			//var result = await client.GetStringAsync("/");
			//return Ok(result);
			//return Ok(await client.GetStringAsync("/user"));

			var result = await _gitHubClient.Client.GetStringAsync("/");
			return Ok(result);
		}
		
    }
}