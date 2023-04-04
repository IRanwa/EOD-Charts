
using IRanwa.EOD.Chart.Business;
using IRanwa.EOD.Chart.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IRanwa.EOD.Charts.Jobs.SyncData;

public static class ContainerConfig
{
    public static IHost Configure()
    {
        var appConfiguration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .Build();

        var builder = new HostBuilder()
            .ConfigureServices(service =>
            {
                service.AddTransient<IExchangeService, ExchangeService>();
                service.AddTransient<IUnitOfWorkAsync, UnitOfWorkAsync>();
                service.AddTransient<IExchangeHelperService, ExchangeHelperService>();
                service.AddTransient<IEODHttpClient, EODHttpClient>();
            });
        return builder.Build();
    }
}
