﻿namespace APIGateway
{
    using ApiGateway.Model;
    using CacheManager.Core;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Ocelot.DependencyInjection;
    using Ocelot.Middleware;
    using System;
    using System.Text;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //public Startup(IHostingEnvironment env)
        //{
        //    var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
        //    builder.SetBasePath(env.ContentRootPath)
        //            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //           .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
        //           .AddEnvironmentVariables();
        //    Configuration = builder.Build();
        //}

        public IConfiguration Configuration { get; }

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
                ValidateIssuer = false,
                //ValidIssuer = Iss,
                ValidateAudience = false,
                //ValidAudience = Aud,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };

            //register authentication provider key 
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = "TestKey";
            })
            //add validate token 
            .AddJwtBearer("TestKey", x =>
            {
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = tokenValidationParameters;
            });


            services.AddOcelot(Configuration);
        }

        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAuthentication();
            await app.UseOcelot();
        }
    }
}
