using IRanwa.EOD.Chart.Job.DataSync;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Started");

var host = ContainerConfiguration.ConfigureService();
using (var serviceScope = host.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    var service = services.GetRequiredService<Application>();
    service?.Run();
}