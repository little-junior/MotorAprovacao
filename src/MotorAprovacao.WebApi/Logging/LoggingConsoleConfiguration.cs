﻿using Serilog.Events;
using Serilog.Filters;
using Serilog;

namespace MotorAprovacao.WebApi.Logging
{
    public class LoggingConsoleConfiguration
    {
        public static void UseCustomLog(this IApplicationBuilder app, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithCorrelationId()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithProperty("ApplicationName", "MotorAprovação")
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
                .WriteTo.Console();

        }
    }
}
