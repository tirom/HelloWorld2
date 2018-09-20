using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

using Serilog;
//using Serilog.Formatting.Compact;
using Serilog.Sinks.Elasticsearch;

namespace WebApiTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
				.UseSerilog((ctx, config) =>
				{
					config
						.MinimumLevel.Information()
						.Enrich.FromLogContext()
						.WriteTo.Console(new ElasticsearchJsonFormatter());						
				})
			    //.UseUrls("http://localhost:8088")
				.UseStartup<Startup>();
    }
}
