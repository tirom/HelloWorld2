using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollyWithHttpClient.Logging
{
	public class CustomLoggingFilter : IHttpMessageHandlerBuilderFilter
	{
		private readonly ILoggerFactory _loggerFactory;

		public CustomLoggingFilter(ILoggerFactory loggerFactory)
		{
			_loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
		}

		public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
		{
			if (next == null)
			{
				throw new ArgumentNullException(nameof(next));
			}

			return (builder) =>
			{
				// Run other configuration first, we want to decorate.
				next(builder);

				var outerLogger =
					_loggerFactory.CreateLogger($"System.Net.Http.HttpClient.{builder.Name}.LogicalHandler");

				builder.AdditionalHandlers.Insert(0, new CustomLoggingScopeHttpMessageHandler(outerLogger));
			};
		}
	}
}
