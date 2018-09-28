using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kikker.HealthCheck.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kikker.HealthCheck.Controllers
{
	[Route("api/[controller]")]
	public class AuthController : Controller
    {
		readonly IAuthService AuthService;
		IConfiguration configuration;

		public AuthController(IConfiguration config, IAuthService authService)
		{
			configuration = config;
			AuthService = authService;
		}

		[HttpGet]
		public async Task<string> GetAsync()
		{
			if (AuthService != null)
			{
				var username = configuration.GetValue<string>("KikkerAuth:Username") ?? string.Empty;
				var password = configuration.GetValue<string>("KikkerAuth:Password") ?? string.Empty;
				try
				{
					var result = await AuthService.GetAuthenticationAsync(username, password);
					Console.WriteLine(result);
					return result.ToString();
				}
				catch (Exception ex)
				{
					Console.WriteLine($"KikkerAuth {ex.Message}");
				}
			}
			return string.Empty;
        }
    }
}
