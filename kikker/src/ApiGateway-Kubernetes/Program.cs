namespace ApiGateway
{
    using System;
	using System.IO;
	using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
   
	using Serilog;
	using Serilog.Sinks.Elasticsearch;	
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
                //.UseUrls("http://*:5000")
				.UseSerilog((ctx, conf) =>
				{
					conf
						.MinimumLevel.Information()
						.Enrich.FromLogContext()
						.WriteTo.Console(new ElasticsearchJsonFormatter());
				})
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
