using IRanwa.EOD.Chart.Business;
using IRanwa.EOD.Chart.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using System.Security.Principal;

namespace IRanwa.EOD.Chart.Job.SyncData;

/// <summary>
/// Container configuration.
/// </summary>
public static class ContainerConfiguration
{
    /// <summary>
    /// Configures the services.
    /// </summary>
    /// <returns>Returns host.</returns>
    public static IHost ConfigureServices()
    {
        var appConfiguration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .Build();

        var builder = new HostBuilder()
            .ConfigureServices(services =>
            {
                services.AddTransient<Application>();
                var businessProjectClasses = typeof(ExchangeService).Assembly.GetTypes()
                .Where(service => service.Name.EndsWith("Service")).ToList();
                var interfaces = businessProjectClasses.Where(x => x.IsInterface);
                var serviceClasses = businessProjectClasses.Where(x => !x.IsInterface);
                foreach (var serviceClass in serviceClasses)
                {
                    var interfaceClass = interfaces.Where(x => x.Name.Equals($"I{serviceClass.Name}")).FirstOrDefault();
                    if (interfaceClass != null)
                        services.AddTransient(interfaceClass, serviceClass);
                }

                services.AddSingleton<IEODHttpClient, EODHttpClient>();
                services.AddTransient<IUnitOfWorkAsync, UnitOfWorkAsync>();
                services.AddTransient<ILogService, LogService>();
                services.AddTransient<IPrincipal>(provider => new ClaimsPrincipal() { });

                services.AddDbContext<EODDBContext>(options => options.UseSqlServer(appConfiguration.GetConnectionString("DefaultConnection")));
            });
        return builder.Build();
    }
}
