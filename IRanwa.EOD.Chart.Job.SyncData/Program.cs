// See https://aka.ms/new-console-template for more information

using IRanwa.EOD.Chart.Job.SyncData;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(
                    "logs/job_.txt",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day
                )
                .CreateLogger();
Log.Information("started");

var host = ContainerConfiguration.ConfigureServices();
using var serviceScope = host.Services.CreateScope();
var serviceProvider = serviceScope.ServiceProvider;
var service = serviceProvider.GetRequiredService<Application>();
service.Run();