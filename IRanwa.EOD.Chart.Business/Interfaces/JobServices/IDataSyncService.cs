namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// Data sync service.
/// </summary>
public interface IDataSyncService
{
    /// <summary>
    /// Synchronizes the data asynchronous.
    /// </summary>
    Task SyncDataAsync();
}
