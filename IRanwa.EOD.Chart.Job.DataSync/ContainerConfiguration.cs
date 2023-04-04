using IRanwa.EOD.Chart.Business;
using IRanwa.EOD.Chart.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using System.Security.Principal;

namespace IRanwa.EOD.Chart.Job.DataSync;

public static class ContainerConfiguration
{
    public static IHost ConfigureService()
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
                services.AddTransient<IPrincipal>(provider => new ClaimsPrincipal() { });
                services.AddTransient<IUnitOfWorkAsync, UnitOfWorkAsync>();

                var connectionString = appConfiguration.GetConnectionString("DefaultConnection");
                services.AddDbContext<EODDBContext>(options => options.UseSqlServer(connectionString));
            });
        return builder.Build();
    }
}
