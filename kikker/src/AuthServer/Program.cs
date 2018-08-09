namespace AuthServer
{
    using System;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Steeltoe.Extensions.Configuration.ConfigServer;

    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            
                    // Use Config Server for configuration data
                    .AddConfigServer()
                    .UseStartup<Startup>()
                    .UseUrls($"http://{Environment.MachineName}:5559")
                    .Build();
    }
}