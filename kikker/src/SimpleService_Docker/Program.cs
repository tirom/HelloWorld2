using System;
using System.IO;
using System.Linq;
using System.Reflection;
using DbUp;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Steeltoe.Extensions.Configuration.ConfigServer;

namespace DownstreamService
{
    public class Program
    {
		//private static string SERVICE_NAME = string.Empty; 
		public static void Main(string[] args)
        {
			Console.WriteLine(args.Length > 0 ? args[0] :"No argument");
			var config = new ConfigurationBuilder()
							.SetBasePath(Directory.GetCurrentDirectory())
							.AddJsonFile("appsettings.json", optional: true)							
							.AddConfigServer()
							.AddCommandLine(args)							
							.Build();

			//InitLogger(config);
			//SERVICE_NAME = config.GetValue<string>("Spring:Application:Name") ?? string.Empty;
			//var serverName = config.GetValue<string>("machinename");
			var port = config.GetValue<string>("Eureka:Instance:Port") ?? string.Empty;
			if (MigrateDb(config) == 0)
			{
				var listeningUrl = $"http://*:{port}";				
				//Log.Information($"Service {SERVICE_NAME} try to listen on {listeningUrl}");				

				BuildWebHost(args, config, listeningUrl).Run();				
			}							
		}

		public static IWebHost BuildWebHost(string[] args, IConfiguration config, string listeningUrl) =>
			WebHost.CreateDefaultBuilder(args)
				//.UseUrls(listeningUrl)
				.UseConfiguration(config)
				.UseSerilog((ctx, conf) =>
				{
					conf
						.MinimumLevel.Information()
						.Enrich.FromLogContext()
						.WriteTo.Console(new ElasticsearchJsonFormatter());
				})
				.UseStartup<Startup>()				
				.Build();

		private static int MigrateDb(IConfiguration config)
		{
			try
			{
				var connectionString = config.GetValue<string>("mysql:connectionString") ?? string.Empty;
				//Log.Information($"{SERVICE_NAME} {connectionString}");
				var upgrader =
					DeployChanges.To
						.MySqlDatabase(connectionString)
						.WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
						//.WithScriptsFromFileSystem(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Scripts")
						.LogToConsole()
						.Build();

				var result = upgrader.PerformUpgrade();
				if (!result.Successful)
				{
					//Log.Error($"{SERVICE_NAME} {result.Error.ToString()}");
					return -1;
				}

				if (result.Scripts.Count() > 0)
				{
					foreach (var dbScript in result.Scripts)
					{
						//Log.Information($"{SERVICE_NAME} {dbScript.Name} {dbScript.Contents}");
					}
				}
				else
				{
					//Log.Information($"{SERVICE_NAME} No new scripts need to be executed");
				}
				//Log.Information($"{SERVICE_NAME} Database migrating was successful!");
				return 0;
			}
			catch (Exception ex)
			{
				//Log.Fatal($"{SERVICE_NAME} {ex.ToString()}");
				return -1;
			}
		}

		//private static void InitLogger(IConfiguration config)
		//{			
		//	string logstashUrl = config.GetValue<string>("Logstash:Uri") ?? string.Empty;
		//	Console.WriteLine($"Logstash url {logstashUrl}");
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
