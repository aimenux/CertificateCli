using System.Diagnostics.CodeAnalysis;
using App.Commands;
using App.Configuration;
using App.Extensions;
using App.Services.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace App;

[ExcludeFromCodeCoverage]
public static class Program
{
    public static Task Main(string[] args)
    {
        return CreateHostBuilder(args).RunCommandLineApplicationAsync<ToolCommand>(args);
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, config) =>
            {
                config.AddJsonFile();
                config.AddUserSecrets();
                config.AddEnvironmentVariables();
                config.AddCommandLine(args);
            })
            .ConfigureLogging((hostingContext, loggingBuilder) =>
            {
                loggingBuilder.AddConsoleLogger();
                loggingBuilder.AddNonGenericLogger();
                loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            })
            .ConfigureServices((hostingContext, services) =>
            {
                services.Configure<Settings>(hostingContext.Configuration.GetSection("Settings"));
                services.Scan(scan =>
                {
                    scan.FromCallingAssembly()
                        .FromAssemblies(typeof(ConsoleService).Assembly)
                        .AddClasses()
                        .AsImplementedInterfaces()
                        .WithTransientLifetime();
                });
                services.AddCertificateManager();
            });
}