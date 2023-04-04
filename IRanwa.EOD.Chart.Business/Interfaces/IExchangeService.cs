using IRanwa.EOD.Chart.Core;
using IRanwa.EOD.Chart.Model;

namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// Exchange service.
/// </summary>
public interface IExchangeService
{
    /// <summary>
    /// Gets the exchange symbols asynchronous.
    /// </summary>
    /// <param name="exchangeCode">The exchange code.</param>
    /// <param name="stockType">The stock type.</param>
    /// <returns>Returns symbols model.</returns>
    Task<List<SymbolsModel>> GetExchangeSymbolsAsync(string exchangeCode, StockTypes? stockType);

    /// <summary>
    /// Gets the exchange codes asynchronous.
    /// </summary>
    /// <returns>Returns list of exchange codes.</returns>
    Task<List<ExchangeModel>> GetExchangeCodesAsync();

    /// <summary>
    /// Gets all exchange symbols.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="currentSymbolsPage">The current symbols page.</param>
    /// <param name="searchKeyword">The search keyword.</param>
    /// <returns>Returns list of symbols.</returns>
    List<SymbolsModel> GetAllExchangeSymbols(StockTypes? type, int currentSymbolsPage, string searchKeyword);
}
