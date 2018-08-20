namespace ApiGateway
{
    using System;
	using System.IO;
	using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Ocelot.DependencyInjection;
    using Ocelot.Middleware;
	//using Serilog;
	//using Serilog.Events;
	using Steeltoe.Extensions.Configuration.ConfigServer;

    public class Program
    {
        public static void Main(string[] args)
        {
			var config = new ConfigurationBuilder()
							.SetBasePath(Directory.GetCurrentDirectory())
							.AddJsonFile("appsettings.json", optional: true)
							.AddConfigServer()
							.AddCommandLine(args)
							.Build();
			//InitLogger(config);
            BuildWebHost(args).Run();
        }


        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("ocelot.json")
                        .AddEnvironmentVariables();
                })
                .AddConfigServer()
                .UseUrls("http://*:5000")
                .UseStartup<APIGateway.Startup>()
                .Build();

		//private static void InitLogger(IConfiguration config)
		//{
		//	string logstashUrl = config.GetValue<string>("Logstash:Uri") ?? string.Empty;
		//	Log.Logger = new LoggerConfiguration()
		//		.MinimumLevel.Debug()
		//		.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
		//		.Enrich.FromLogContext()
		//		.WriteTo.Console()
		//		.WriteTo.Http(logstashUrl)
		//		.CreateLogger();
		//}

	}
}
