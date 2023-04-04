using IRanwa.EOD.Chart.Core;
using IRanwa.EOD.Chart.Data;
using Serilog;

namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// Data sync service.
/// </summary>
/// <seealso cref="IDataSyncService" />
public class DataSyncService : IDataSyncService
{
    /// <summary>
    /// The unit of work asynchronous
    /// </summary>
    private readonly IUnitOfWorkAsync unitOfWorkAsync;

    /// <summary>
    /// The stock data service
    /// </summary>
    private readonly IStockDataService stockDataService;

    /// <summary>
    /// The log service
    /// </summary>
    private readonly ILogService logService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataSyncService"/> class.
    /// </summary>
    /// <param name="unitOfWorkAsync">The unit of work asynchronous.</param>
    /// <param name="stockDataService">The stock data service.</param>
    /// <param name="logService">The log service.</param>
    public DataSyncService(IUnitOfWorkAsync unitOfWorkAsync, IStockDataService stockDataService, ILogService logService)
    {
        this.unitOfWorkAsync = unitOfWorkAsync;
        this.stockDataService = stockDataService;
        this.logService = logService;
    }

    /// <summary>
    /// Synchronizes the data asynchronous.
    /// </summary>
    public async Task SyncDataAsync()
    {
        await SyncHistoricalDataAsync();
    }

    /// <summary>
    /// Synchronizes the historical data asynchronous.
    /// </summary>
    private async Task SyncHistoricalDataAsync()
    {
        logService.AddInformation("Sync historical data started.");

        var totalSymbolsSync = (int)default;
        var exchangeCodes = unitOfWorkAsync.GetGenericRepository<ExchangeCode>().GetQueryable(x => x.Code == "LSE", null).ToList();
        foreach(var code in exchangeCodes)
        {
            var exchangeSymbols = unitOfWorkAsync.GetGenericRepository<ExchangeSymbol>().GetQueryable(
                x => x.ExchangeCodeId == code.Id && 
                (x.LastSyncDate == null || ((DateTime)x.LastSyncDate).AddDays(Constants.SyncDates) == DateTime.UtcNow) &&
                x.Type == StockTypes.CommonStock.GetEnumDisplayName(), 
                null).Take(Constants.MaximumSymbolSyncCount).ToList();
            totalSymbolsSync += exchangeSymbols.Count;
            foreach (var symbol in exchangeSymbols)
            {
                logService.AddInformation($"Data syncing started for {code.Code}-{symbol.Code}");
                var quarterlySync = false;
                var annualSync = false;
                try
                {
                    var quarterlyData = await stockDataService.GetStockDataAsync(symbol.Code, code.Code, PeriodTypes.Quarterly);
                    quarterlySync = true;
                    var annualData = await stockDataService.GetStockDataAsync(symbol.Code, code.Code, PeriodTypes.Annual);
                    annualSync = true;

                    symbol.DataSyncCompleted = true;
                    symbol.LastSyncDate= DateTime.UtcNow;
                    symbol.QuarterlySyncCompleted = quarterlySync;
                    symbol.AnnualSyncCompleted = annualSync;
                }
                catch(Exception ex)
                {
                    logService.AddError($"{code.Code} - {symbol.Code} symbol error : {ex.Message}");

                    symbol.DataSyncCompleted = true;
                    symbol.LastSyncDate = DateTime.UtcNow;
                    symbol.QuarterlySyncCompleted = quarterlySync;
                    symbol.AnnualSyncCompleted = annualSync;
                    symbol.SyncException = ex.Message;
                }
                unitOfWorkAsync.SaveChanges();
                logService.AddInformation($"Data syncing ended for {code.Code}-{symbol.Code}");
                Thread.Sleep(2 * 1000 );
            }
            if (totalSymbolsSync >= Constants.MaximumSymbolSyncCount)
                break;
        }
        logService.AddInformation("Sync historical data ended.");
    }
}
