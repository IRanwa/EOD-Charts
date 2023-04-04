using IRanwa.EOD.Charts.Jobs.SyncData;
using Microsoft.Extensions.DependencyInjection;

var host = ContainerConfig.Configure();
var serviceScropes = host.Services.CreateScope();
var provider = serviceScropes.ServiceProvider;
var service = provider.GetRequiredService<Application>();
service.Run();