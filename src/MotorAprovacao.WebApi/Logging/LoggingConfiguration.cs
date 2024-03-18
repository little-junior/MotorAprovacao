using Serilog.Events;
using Serilog.Filters;
using Serilog;
using Serilog.Exceptions;



namespace MotorAprovacao.WebApi.Logging
{
    public static class LoggingConfiguration 
    {
        public static void UseCustomLog(this IApplicationBuilder app, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)


                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithCorrelationId()
                //.Enrich.WithMachineName("x")
                .Enrich.WithThreadId()
                .Enrich.WithProperty("ApplicationName", "MotorAprovação")
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
                .WriteTo.Console();

            

            Log.Logger = loggerConfiguration.CreateLogger();

            loggerFactory.AddSerilog(Log.Logger);
        }
    }
}


