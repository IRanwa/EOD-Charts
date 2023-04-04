using IRanwa.EOD.Chart.Model;

namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// Stock live data service
/// </summary>
public interface IStockLiveDataService
{
    /// <summary>
    /// Gets the stock live history data asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns eod data view model.</returns>
    Task<List<EODDataViewModel>> GetStockLiveHistoryDataAsync(StockDataViewModel model);

    /// <summary>
    /// Gets the live stock data asynchronous.
    /// </summary>
    /// <param name="exchangeCode">The exchange code.</param>
    /// <param name="symbolCode">The symbol code.</param>
    /// <returns>Returns live EOD data.</returns>
    Task<EODDataViewModel> GetLiveStockDataAsync(string exchangeCode, string symbolCode);
}
