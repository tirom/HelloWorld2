using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Steeltoe.Extensions.Configuration.ConfigServer;

namespace DownstreamService3
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
                .UseUrls("http://*:5003")
                .Build();
    }
}
