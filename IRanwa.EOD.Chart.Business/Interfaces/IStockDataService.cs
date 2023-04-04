using IRanwa.EOD.Chart.Core;
using IRanwa.EOD.Chart.Model;

namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// Stock data service.
/// </summary>
public interface IStockDataService
{
    /// <summary>
    /// Gets the stock data asynchronous.
    /// </summary>
    /// <param name="symbolCode">The symbol code.</param>
    /// <param name="exchangeCode">The exchange code.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns final stock data model.</returns>
    Task<FinalStockDataModel> GetStockDataAsync(string symbolCode, string exchangeCode, PeriodTypes period);
}
