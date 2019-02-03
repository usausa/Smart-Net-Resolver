﻿namespace Example.GenericHost
{
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using Smart.Resolver;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await new HostBuilder()
                .ConfigureAppConfiguration((hostContext, app) =>
                {
                    hostContext.HostingEnvironment.EnvironmentName = System.Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

                    app.SetBasePath(Directory.GetCurrentDirectory());
                    app.AddJsonFile("appsettings.json", optional: true);
                    app.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    app.AddEnvironmentVariables(prefix: "HOST_EXAMPLE_");
                    app.AddCommandLine(args);
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Information);

                    if (hostingContext.HostingEnvironment.IsDevelopment())
                    {
                        logging.AddConsole();
                        logging.AddDebug();
                    }
                })
                .UseServiceProviderFactory(new SmartServiceProviderFactory(ConfigureServices))
                .ConfigureServices(ConfigureServices)
                .UseConsoleLifetime()
                .RunConsoleAsync();
        }

        private static void ConfigureServices(ResolverConfig config)
        {
            config.Bind<SettingService>().ToSelf().InSingletonScope();
        }

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.Configure<Settings>(hostContext.Configuration.GetSection("Settings"));

            services.AddHostedService<ExampleHostedService>();
        }
    }
}
