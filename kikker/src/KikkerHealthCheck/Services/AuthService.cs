using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kikker.HealthCheck.Services
{
	public class AuthService : BaseService, IAuthService
	{
		private IConfiguration configuration;

		public AuthService(ILoggerFactory loggerFactory, IConfiguration config): base(loggerFactory.CreateLogger<AuthService>())
		{
			configuration = config;
		}

		public async Task<HttpResponseMessage> GetAuthenticationAsync(string username, string password)
		{
			var jsonInString = string.Format("{2}\"username\": \"{0}\", \"password\": \"{1}\"{3}", username,password,"{","}");
			var client = new HttpClient();
			var authUri = configuration.GetValue<string>("KikkerAuth:Uri") ?? string.Empty;
			var result = await client.PostAsync(authUri, new StringContent(jsonInString, Encoding.UTF8, "application/json"));

			return result;
		}
	}
}
