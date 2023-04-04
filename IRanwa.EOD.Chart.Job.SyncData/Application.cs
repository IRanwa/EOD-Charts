using IRanwa.EOD.Chart.Business;
using Serilog;

namespace IRanwa.EOD.Chart.Job.SyncData;

/// <summary>
/// Application.
/// </summary>
public class Application
{
    /// <summary>
    /// The data synchronize service
    /// </summary>
    private readonly IDataSyncService dataSyncService;

    /// <summary>
    /// The log service
    /// </summary>
    private readonly ILogService logService;

    /// <summary>
    /// Initializes a new instance of the <see cref="Application"/> class.
    /// </summary>
    /// <param name="dataSyncService">The data synchronize service.</param>
    /// <param name="logService">The log service.</param>
    public Application(IDataSyncService dataSyncService, ILogService logService)
    {
        this.dataSyncService = dataSyncService;
        this.logService = logService;
    }

    /// <summary>
    /// Runs this instance.
    /// </summary>
    public void Run()
    {
        try
        {
            logService.AddInformation("Data sync started.");
            dataSyncService.SyncDataAsync().Wait();
            logService.AddInformation("Data sync ended.");
        }catch(Exception ex)
        {
            logService.AddError($"Data sync failed. {ex}");
        }
    }
}
