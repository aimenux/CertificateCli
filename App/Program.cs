using App.Commands;
using App.Extensions;
using Lib.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace App;

public static class Program
{
    public static Task Main(string[] args)
    {
        return CreateHostBuilder(args).RunCommandLineApplicationAsync<MainCommand>(args);
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, config) =>
            {
                config.AddJsonFile();
                config.AddEnvironmentVariables();
                config.AddCommandLine(args);
            })
            .ConfigureLogging((hostingContext, loggingBuilder) =>
            {
                loggingBuilder.AddConsoleLogger();
                loggingBuilder.AddNonGenericLogger();
                loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            })
            .ConfigureServices((_, services) =>
            {
                services.Scan(scan =>
                {
                    scan.FromCallingAssembly()
                        .FromAssemblies(typeof(ConsoleHelper).Assembly)
                        .AddClasses()
                        .AsImplementedInterfaces()
                        .WithTransientLifetime();
                });
                services.AddCertificateManager();
            });
}