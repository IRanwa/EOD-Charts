using IRanwa.EOD.Chart.Core;
using IRanwa.EOD.Chart.Model;

namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// Exchange helper service.
/// </summary>
public interface IExchangeHelperService
{
    /// <summary>
    /// Gets the exchange symbols list asynchronous.
    /// </summary>
    /// <param name="exchangeCode">The exchange code.</param>
    /// <param name="stockType">The stock type.</param>
    /// <returns>Returns list of symbols models.</returns>
    Task<List<SymbolsModel>> GetExchangeSymbolsListAsync(string exchangeCode, StockTypes? stockType);

    /// <summary>
    /// Gets the exchange list asynchronous.
    /// </summary>
    /// <returns>Returns exchange list.</returns>
    Task<List<ExchangeModel>> GetExchangeListAsync();
}
