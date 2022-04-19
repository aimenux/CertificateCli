﻿using App.Commands;
using App.Extensions;
using Lib.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static App.Extensions.PathExtensions;

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
                config.AddCommandLine(args);
                config.AddEnvironmentVariables();
                config.SetBasePath(GetDirectoryPath());
                var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
            })
            .ConfigureLogging((hostingContext, loggingBuilder) =>
            {
                loggingBuilder.AddConsoleLogger();
                loggingBuilder.AddNonGenericLogger();
                loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            })
            .ConfigureServices((_, services) =>
            {
                services.AddTransient<MainCommand>();
                services.AddTransient<JwkCommand>();
                services.AddTransient<RsaCommand>();
                services.AddTransient<IJwkHelper, JwkHelper>();
                services.AddTransient<IRsaHelper, RsaHelper>();
                services.AddTransient<IConsoleHelper, ConsoleHelper>();
                services.AddTransient<ICertificateManagerWrapper, CertificateManagerWrapper>();
                services.AddCertificateManager();
            });
}