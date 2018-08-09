using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Steeltoe.Extensions.Configuration.ConfigServer;

namespace DownstreamService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .AddConfigServer()
                .UseStartup<Startup>()
                .UseUrls($"http://{Environment.MachineName}:5007")
                .Build();
    }
}
