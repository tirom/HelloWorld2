using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DownstreamService
{
	
	using SimpleService.Models;
	//using Steeltoe.Discovery.Client;
	using Pivotal.Discovery.Client;
	using Serilog;
	//using MySql.Data.MySqlClient;
	using System.Threading;
	//using MySql.Data.MySqlClient;

	public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;			
		}

        public IConfiguration Configuration { get; }
		

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
        {			
			//services.AddDiscoveryClient(Configuration);
			// Adds the configuration data POCO configured with data returned from the Spring Cloud Config Server
			services.Configure<ConfigServerData>(Configuration);
			services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
			
			//app.UseDiscoveryClient();
            app.UseMvc();			
		}

		
		
	}
}
