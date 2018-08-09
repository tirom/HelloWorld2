namespace AuthServer
{
    using ApiGateway.Model;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
	using Pivotal.Discovery.Client;

	//using Steeltoe.Discovery.Client;

	public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<ConfigServerData>(Configuration);

            services.AddDiscoveryClient(Configuration);
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDiscoveryClient();
            app.UseMvc();
        }
    }
}