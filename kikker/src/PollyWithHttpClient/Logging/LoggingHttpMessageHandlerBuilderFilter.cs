﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Http
{
	// Internal so we can change the requirements without breaking changes.
	internal class LoggingHttpMessageHandlerBuilderFilter : IHttpMessageHandlerBuilderFilter
	{
		private readonly ILoggerFactory _loggerFactory;

		public LoggingHttpMessageHandlerBuilderFilter(ILoggerFactory loggerFactory)
		{
			if (loggerFactory == null)
			{
				throw new ArgumentNullException(nameof(loggerFactory));
			}

			_loggerFactory = loggerFactory;
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

				var loggerName = !string.IsNullOrEmpty(builder.Name) ? builder.Name : "Default";

				// We want all of our logging message to show up as-if they are coming from HttpClient,
				// but also to include the name of the client for more fine-grained control.
				var outerLogger = _loggerFactory.CreateLogger($"System.Net.Http.HttpClient.{loggerName}.LogicalHandler");
				var innerLogger = _loggerFactory.CreateLogger($"System.Net.Http.HttpClient.{loggerName}.ClientHandler");

				// The 'scope' handler goes first so it can surround everything.
				builder.AdditionalHandlers.Insert(0, new LoggingScopeHttpMessageHandler(outerLogger));

				// We want this handler to be last so we can log details about the request after
				// service discovery and security happen.
				builder.AdditionalHandlers.Add(new LoggingHttpMessageHandler(innerLogger));

			};
		}
	}
}