using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebTest.Services
{
	public class ValueService : BaseService, IValue
	{		
		private IConfiguration configuration;
		public ValueService(ILoggerFactory logFactory, IConfiguration config) : base(logFactory.CreateLogger<ValueService>())
		{
			configuration = config;			
		}
		public async Task<List<string>> GetValuesAsync()
		{			
			var httpRequest = new HttpRequestMessage(HttpMethod.Get, configuration.GetValue<string>("WebApiAddress") ?? string.Empty);
			var result = await Invoke<List<string>>(httpRequest);

			return result;
		}
	}
}
