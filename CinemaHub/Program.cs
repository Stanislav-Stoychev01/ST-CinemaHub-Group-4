using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub
{
    public static class Application
    {
        public class SwitchesSection
        {
            public bool UseAccessToken { get; set; }
            public bool SeedData { get; set; }
            public bool AllowAdminRegister { get; set; }

        }

        public static IConfiguration Configuration { get; set; }
        public static SwitchesSection Switches => Configuration.GetSection("Switches").Get<SwitchesSection>();
        public static string Environment => Configuration.GetSection("Environment").Value;
        public static string BlobStorageUrl => Configuration.GetSection("BlobStorageUrl").Value;
        public static string BlobStorageConnectionString => Configuration.GetSection("BlobStorageConnectionString").Value;
        public static string MainApplicationUrl => Configuration.GetSection("MainApplicationUrl").Value;
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .ConfigureAppConfiguration((_, builder) =>
                        {
                            builder.Sources.Clear();
                            builder.SetBasePath(_.HostingEnvironment.ContentRootPath)
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                .AddJsonFile($"appsettings.{_.HostingEnvironment.EnvironmentName}.json", optional: true)
                                .AddJsonFile($"appsettings.Local.json", optional: true)
                                .AddEnvironmentVariables();
                        }).ConfigureLogging((hostingContext, logging) =>
                        {
                            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                            logging.AddConsole();
                            logging.AddDebug();
                        })
                        .UseKestrel(c => c.AddServerHeader = false);

                });

    }
}
