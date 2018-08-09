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

namespace DownstreamService3
{
    using System.Text;
    using ApiGateway.Model;
    using AuthServer.Authorization;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.IdentityModel.Tokens;
	using Pivotal.Discovery.Client;

	//using Steeltoe.Discovery.Client;

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
            // Setup Options framework with DI
            services.AddOptions();
            services.Configure<ConfigServerData>(Configuration);

            //get config Auth
            string Secret = Configuration.GetValue<string>("Audience:Secret") ?? string.Empty;
            string Iss = Configuration.GetValue<string>("Audience:Iss") ?? string.Empty;
            string Aud = Configuration.GetValue<string>("Audience:Aud") ?? string.Empty;

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = Iss,
                ValidateAudience = true,
                ValidAudience = Aud,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = "TestKey";
                //register the challenge scheme handle the forbid as well
                o.DefaultForbidScheme = "TestKey";
            })
            .AddJwtBearer("TestKey", x =>
            {
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = tokenValidationParameters;
            });
            services.AddAuthorization(options =>
            {
                //options.AddPolicy("UserRole",
                //       policy => {
                //           policy.RequireRole("Customer", "Administrator");
                //       });
                options.AddPolicy("UserRole",
                           policy => policy.Requirements.Add(new RoleRequirement("customer")));
            });
            services.AddSingleton<IAuthorizationHandler, RoleHandler>();
            services.AddDiscoveryClient(Configuration);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseDiscoveryClient();
            app.UseMvc();
        }
    }
}
