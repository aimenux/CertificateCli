﻿using System.Diagnostics.CodeAnalysis;
using App.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace App.Extensions;

[ExcludeFromCodeCoverage]
public static class LoggingBuilderExtensions
{
    public static void AddConsoleLogger(this ILoggingBuilder loggingBuilder)
    {
        if (File.Exists(PathExtensions.GetSettingFilePath()))
        {
            loggingBuilder.AddConsole();
        }
        else
        {
            loggingBuilder.AddSimpleConsole(options =>
            {
                options.SingleLine = true;
                options.IncludeScopes = true;
                options.UseUtcTimestamp = true;
                options.TimestampFormat = "[HH:mm:ss:fff] ";
                options.ColorBehavior = LoggerColorBehavior.Enabled;
            });
        }
    }

    public static void AddNonGenericLogger(this ILoggingBuilder loggingBuilder)
    {
        const string categoryName = Settings.Cli.ToolName;
        var services = loggingBuilder.Services;
        services.AddSingleton(serviceProvider =>
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            return loggerFactory.CreateLogger(categoryName!);
        });
    }
}