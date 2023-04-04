using IRanwa.EOD.Chart.Core;
using IRanwa.EOD.Chart.Data;
using IRanwa.EOD.Chart.Model;
using System.Net.Http.Json;

namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// Exchange helper service.
/// </summary>
/// <seealso cref="IExchangeHelperService" />
public class ExchangeHelperService : IExchangeHelperService
{
    /// <summary>
    /// The HTTP client
    /// </summary>
    private readonly IEODHttpClient httpClient;

    /// <summary>
    /// The unit of work asynchronous
    /// </summary>
    private readonly IUnitOfWorkAsync unitOfWorkAsync;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExchangeHelperService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="unitOfWorkAsync">The unit of work..</param>
    public ExchangeHelperService(IEODHttpClient httpClient, IUnitOfWorkAsync unitOfWorkAsync)
    {
        this.httpClient = httpClient;
        this.unitOfWorkAsync = unitOfWorkAsync;
    }

    /// <summary>
    /// Gets the exchange list asynchronous.
    /// </summary>
    /// <returns>Returns exchange list.</returns>
    public async Task<List<ExchangeModel>> GetExchangeListAsync()
    {
        var response = await httpClient.GetAsync($"api/exchanges-list", null);
        if (response.IsSuccessStatusCode)
        {
            var codesList = await response.Content.ReadFromJsonAsync<List<ExchangeModel>>();
            await UpdateExchangesAsync(codesList);
            return codesList;
        }
        return null;
    }

    /// <summary>
    /// Gets the exchange symbols list asynchronous.
    /// </summary>
    /// <param name="exchangeCode">The exchange code.</param>
    /// <param name="stockType">The stock types.</param>
    /// <returns>
    /// Returns list of symbols models.
    /// </returns>
    public async Task<List<SymbolsModel>> GetExchangeSymbolsListAsync(string exchangeCode, StockTypes? stockType)
    {
        var response = await httpClient.GetAsync($"api/exchange-symbol-list/{exchangeCode}", null);
        if (response.IsSuccessStatusCode)
        {
            var symbolsList = await response.Content.ReadFromJsonAsync<List<SymbolsModel>>();
            await UpdateSymbolsAsync(symbolsList, exchangeCode);
            if(stockType != null)
            {
                symbolsList = symbolsList.Where(x => x.Type == stockType.GetEnumDisplayName()).ToList();
            }
            return symbolsList;
        }
        return null;
    }

    /// <summary>
    /// Updates the symbols asynchronous.
    /// </summary>
    /// <param name="symbolsList">The symbols list.</param>
    /// <param name="exchangeCode">The exchange code.</param>
    private async Task UpdateSymbolsAsync(List<SymbolsModel> symbolsList, string exchangeCode)
    {
        var exchangeCodeModel = unitOfWorkAsync.GetGenericRepository<ExchangeCode>().GetOne(x => x.Code == exchangeCode);
        if (exchangeCodeModel == null)
            return;

        var existingSymbolsList = unitOfWorkAsync
            .GetGenericRepository<ExchangeSymbol>().GetQueryable(x => x.ExchangeCodeId == exchangeCodeModel.Id, null).ToList();

        var currentSymbolCodes = existingSymbolsList.Select(x => x.Code);

        var newSymbolsList = symbolsList.Where(x => !currentSymbolCodes.Contains(x.Code)).ToList();
        var updateSymbolsList = symbolsList.Where(x => currentSymbolCodes.Contains(x.Code)).ToList();

        foreach(var symbol in newSymbolsList)
        {
            var newSymbol = new ExchangeSymbol()
            {
                Code= symbol.Code,
                Country= symbol.Country,
                Currency= symbol.Currency,
                Exchange = symbol.Exchange,
                Isin = symbol.Isin,
                Name= symbol.Name,
                Type= symbol.Type,
                ExchangeCodeId = exchangeCodeModel.Id,
                CreatedDateTime = DateTime.UtcNow
            };
            await unitOfWorkAsync.GetGenericRepository<ExchangeSymbol>().Add(newSymbol);
        }

        foreach (var symbol in updateSymbolsList)
        {
            var existingSymbol = existingSymbolsList.FirstOrDefault(x => x.Code == symbol.Code);
            if (existingSymbol == null)
                continue;

            existingSymbol.Country = symbol.Country;
            existingSymbol.Currency = symbol.Currency;
            existingSymbol.Exchange = symbol.Exchange;
            existingSymbol.Isin = symbol.Isin;
            existingSymbol.Name = symbol.Name;
            existingSymbol.Type = symbol.Type;
            existingSymbol.CreatedDateTime = DateTime.UtcNow;

            unitOfWorkAsync.GetGenericRepository<ExchangeSymbol>().Update(existingSymbol);
        }
        unitOfWorkAsync.SaveChanges();
    }

    /// <summary>
    /// Updates the exchanges asynchronous.
    /// </summary>
    /// <param name="exchangeCodesList">The exchange codes list.</param>
    private async Task UpdateExchangesAsync(List<ExchangeModel> exchangeCodesList)
    {
        var existingCodesList = unitOfWorkAsync
            .GetGenericRepository<ExchangeCode>().GetAll().ToList();

        var currentCodes = existingCodesList.Select(x => x.Code);

        var newCodesList = exchangeCodesList.Where(x => !currentCodes.Contains(x.Code));
        var updateCodesList = exchangeCodesList.Where(x => currentCodes.Contains(x.Code));

        foreach (var code in newCodesList)
        {
            var newCode = new ExchangeCode()
            {
                Code = code.Code,
                Country = code.Country,
                Currency = code.Currency,
                Name = code.Name,
                CountryISO2 = code.CountryISO2,
                CountryISO3 = code.CountryISO3,
                OperatingMIC = code.OperatingMIC,
                CreatedDateTime = DateTime.UtcNow
            };
            await unitOfWorkAsync.GetGenericRepository<ExchangeCode>().Add(newCode);
        }

        foreach (var code in updateCodesList)
        {
            var existingCode = existingCodesList.FirstOrDefault(x => x.Code == code.Code);
            if (existingCode == null)
                continue;

            existingCode.Country = code.Country;
            existingCode.Currency = code.Currency;
            existingCode.Name = code.Name;
            existingCode.CountryISO2 = code.CountryISO2;
            existingCode.CountryISO3 = code.CountryISO3;
            existingCode.OperatingMIC = code.OperatingMIC;
            existingCode.CreatedDateTime = DateTime.UtcNow;

            unitOfWorkAsync.GetGenericRepository<ExchangeCode>().Update(existingCode);
        }
        unitOfWorkAsync.SaveChanges();
    }
}
