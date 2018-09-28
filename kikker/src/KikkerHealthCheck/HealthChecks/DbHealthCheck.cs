using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using DbUp;
using Microsoft.Extensions.Configuration;

namespace AspNetCore2.Health.Api.QuickStart.HealthChecks
{
    public class DbHealthCheck : HealthCheck
    {

		IConfiguration _config;
		public DbHealthCheck(IConfiguration config)
            : base("Database Health Check") {
			_config = config;	
		}

        protected override ValueTask<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = default)
        {			
			var connectionString = _config.GetValue<string>("MySql:ConnectionString") ?? string.Empty;		
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
				return new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy());
			}
			return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy());			
		}
    }
}
