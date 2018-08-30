using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DownstreamService
{
	
	using SimpleService.Models;	
	//using Pivotal.Discovery.Client;
	//using Serilog;
	//using MySql.Data.MySqlClient;	
	using Swashbuckle.AspNetCore.Swagger;	

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

			// Register the Swagger generator, defining 1 or more Swagger documents
			services.AddSwaggerGen(c =>
			{
				//c.SwaggerDoc("v1", new Info { Title = "SimpleService API", Version = "v1" });
				c.SwaggerDoc("v1", new Info
				{
					Version = "v1",
					Title = "Category API",
					Description = "A simple example ASP.NET Core Web API",
					TermsOfService = "None",
					Contact = new Contact
					{
						Name = "Phong Nhu",
						Email = "nhuthanhphong@gmail.com",
						Url = "https://www.facebook.com/phong.n.thanh.9"
					},
					License = new License
					{
						Name = "Use under LICX",
						Url = "https://example.com/license"
					}
				});

			});

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "SimpleService API V1");

				//serve the Swagger UI at the app's root (http://localhost:<port>/)
				c.RoutePrefix = string.Empty;
			});


			//app.UseDiscoveryClient();
			app.UseMvc();			
		}

		
		
	}
}
