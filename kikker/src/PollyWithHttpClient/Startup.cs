using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Timeout;
using PollyWithHttpClient.Handlers;
using PollyWithHttpClient.Logging;

namespace PollyWithHttpClient
{
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
			var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10)); // Timeout for an individual try
			var longTimeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(30));
			var noOpPolicy = Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();

			services.AddTransient<TimingHandler>();
			services.AddTransient<ValidateHeaderHandler>();
			services.AddHttpClient<MyGitHubClient>(client =>
			{
				client.BaseAddress = new Uri("https://api.github.com/");
				client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
				client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
			})
			.AddPolicyHandler(timeoutPolicy)
			.AddHttpMessageHandler<TimingHandler>() // This handler is on the outside and executes first on the way out and last on the way in.
			.AddHttpMessageHandler<ValidateHeaderHandler>(); // This handler is on the inside, closest to the request.

			//
			//register a PolicyRegistry with DI
			//
			var registry = services.AddPolicyRegistry();
			//registry.Add("defaultretrystrategy",
			//	HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(
			//		new[]
			//		{
			//			TimeSpan.FromSeconds(1),
			//			TimeSpan.FromSeconds(5),
			//			TimeSpan.FromSeconds(10)
			//		}
			//		));
			registry.Add("defaultcircuitbreaker",
				HttpPolicyExtensions.HandleTransientHttpError().CircuitBreakerAsync(
					handledEventsAllowedBeforeBreaking: 3,
					durationOfBreak: TimeSpan.FromSeconds(30)
					));
			registry.Add("regularTimeout", timeoutPolicy);
			registry.Add("longTimeout", longTimeoutPolicy);

			var retryPolicy = HttpPolicyExtensions
				.HandleTransientHttpError()//Network failures (System.Net.Http.HttpRequestException),HTTP 5XX status codes (server errors),HTTP 408 status code (request timeout)
				.Or<TimeoutRejectedException>() // thrown by Polly's TimeoutPolicy if the inner call times out
				.WaitAndRetryAsync(new[]
				{
						TimeSpan.FromSeconds(1),
						TimeSpan.FromSeconds(5),
						TimeSpan.FromSeconds(10)
				});
			
			// Configure a client named as "GitHub", with various default properties.
			services.AddHttpClient("TestGitHub", client =>
			{
				client.BaseAddress = new Uri("https://api.github.com/");
				client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
				client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactoryTesting");
				client.Timeout = TimeSpan.FromSeconds(60); // Overall timeout across all tries
			})
			.AddPolicyHandler(request => request.Method == HttpMethod.Get ? retryPolicy : noOpPolicy)
			//.AddPolicyHandlerFromRegistry("defaultretrystrategy")
			.AddPolicyHandlerFromRegistry("defaultcircuitbreaker")  //already registered above		
			//.AddTransientHttpErrorPolicy(builder => builder.CircuitBreakerAsync(
			//	handledEventsAllowedBeforeBreaking: 3,
			//	durationOfBreak: TimeSpan.FromSeconds(30)
			//))			
			.AddPolicyHandlerFromRegistry("regularTimeout");
			//.AddPolicyHandler(timeoutPolicy);

			services.Replace(ServiceDescriptor.Singleton<IHttpMessageHandlerBuilderFilter, CustomLoggingFilter>());

			services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
			

			app.UseMvc();
        }
    }
}
