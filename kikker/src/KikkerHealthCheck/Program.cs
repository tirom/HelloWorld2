using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Health;
using App.Metrics.Health;
using DbUp;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AspNetCore2.Health.Api.QuickStart
{
    public class Program
    {
        public static void Main(string[] args)
        {
			var config = new ConfigurationBuilder()
							.SetBasePath(Directory.GetCurrentDirectory())
							.AddJsonFile("appsettings.json", optional: true)						
							//.AddCommandLine(args)
							.Build();
			BuildWebHost(args, config).Run();
        }

        public static IWebHost BuildWebHost(string[] args, IConfiguration config) =>
            WebHost.CreateDefaultBuilder(args)
			.ConfigureHealthWithDefaults(
				builder =>
				{
					var kikkerFrontendUri = config.GetValue<string>("KikkerFrontendUri") ?? string.Empty;
					builder.HealthChecks.AddHttpGetCheck("KikkerFrontend", new Uri(kikkerFrontendUri), TimeSpan.FromSeconds(10));

					//var urls = config.GetSection("UrlsCheck").Get<string[]>() ?? null;
					//Console.WriteLine($"Urls Check = {urls}");
					//foreach (var url in urls)
					//{
					//	builder.HealthChecks.AddHttpGetCheck(url, new Uri(url), TimeSpan.FromSeconds(10));						
					//}

					//var urlsPing = config.GetSection("UrlsPing").Get<string[]>() ?? null;
					//Console.WriteLine($"Urls Ping = {urlsPing}");
					//foreach (var ping in urlsPing)
					//{
					//	builder.HealthChecks.AddPingCheck(ping, ping, TimeSpan.FromSeconds(10));
					//}					
				})
			.ConfigureAppHealthHostingConfiguration(options =>
			{
				options.AllEndpointsPort = 3333;
				//options.HealthEndpoint = "/my-health";
				//options.HealthEndpointPort = 1111;
				//options.PingEndpoint = "/my-ping";
				//options.PingEndpointPort = 2222;
			})
                .UseHealth()
                .UseStartup<Startup>()
                .Build();
    }
}
