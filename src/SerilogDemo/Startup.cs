using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting.Display;
using SerilogDemo.Infrastructure;

namespace SerilogDemo
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var loggerConfiguration = CreateLoggerConfiguration();

            Log.Logger = loggerConfiguration.CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static LoggerConfiguration CreateLoggerConfiguration()
        {
            var file = File.CreateText(Directory.GetCurrentDirectory() + "/serilog.log");
            SelfLog.Enable(TextWriter.Synchronized(file));

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {NewLine} {Message:lj}{NewLine} CorrelationId: {CorrelationId}{NewLine} " + SerilogProperties.MessageTemplateWithAllProperties + " {NewLine}{Exception}";
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationIdHeader("x-vermeer-correlation-id")
                .WriteTo.Console(new MessageTemplateTextFormatter(outputTemplate))
                .WriteTo.File("log.log", rollingInterval: RollingInterval.Hour, rollOnFileSizeLimit: true, fileSizeLimitBytes: 25_000_000, outputTemplate: outputTemplate)
                .WriteTo.Seq("http://localhost:5341");
            
            if (environment == Environments.Development)
            {
                loggerConfiguration = loggerConfiguration.MinimumLevel.Override("SerilogDemo", LogEventLevel.Debug);
            }

            return loggerConfiguration;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
