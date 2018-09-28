using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using Kikker.HealthCheck.Services;
using Microsoft.Extensions.Configuration;

namespace AspNetCore2.Health.Api.QuickStart.HealthChecks
{
    public class KikkerAuthHealthCheck: HealthCheck
	{
		IConfiguration _config;
		readonly IAuthService AuthService;
		public KikkerAuthHealthCheck(IConfiguration config, IAuthService authService)
			: base("KikkerAuth - authenticate(username,password) Check")
		{
			_config = config;
			AuthService = authService;
		}

		protected override async ValueTask<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = default)
		{
			if (AuthService != null)
			{
				var username = _config.GetValue<string>("KikkerAuth:Username") ?? string.Empty;
				var password = _config.GetValue<string>("KikkerAuth:Password") ?? string.Empty;
				try
				{
					var result = await AuthService.GetAuthenticationAsync(username, password);
					Console.WriteLine(result);

					if (result.StatusCode == System.Net.HttpStatusCode.OK)
					{
						return HealthCheckResult.Healthy();
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"KikkerAuth {ex.Message}");
					return HealthCheckResult.Unhealthy();
				}
			}
			
			return HealthCheckResult.Unhealthy();
		}
	}
}
