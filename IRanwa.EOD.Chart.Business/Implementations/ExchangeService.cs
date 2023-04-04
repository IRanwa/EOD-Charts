using IRanwa.EOD.Chart.Core;
using IRanwa.EOD.Chart.Data;
using IRanwa.EOD.Chart.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Http.Json;

namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// Exchange service.
/// </summary>
/// <seealso cref="IExchangeService" />
public class ExchangeService : IExchangeService
{
    /// <summary>
    /// The unit of work asynchronous
    /// </summary>
    private readonly IUnitOfWorkAsync unitOfWorkAsync;

    /// <summary>
    /// The exchange helper service
    /// </summary>
    private readonly IExchangeHelperService exchangeHelperService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExchangeService"/> class.
    /// </summary>
    /// <param name="unitOfWorkAsync">The unit of work.</param>
    /// <param name="exchangeHelperService">The exchange helper service.</param>
    public ExchangeService(IUnitOfWorkAsync unitOfWorkAsync, IExchangeHelperService exchangeHelperService)
    {
        this.unitOfWorkAsync = unitOfWorkAsync;
        this.exchangeHelperService = exchangeHelperService;
    }

    /// <summary>
    /// Gets the exchange symbols asynchronous.
    /// </summary>
    /// <param name="exchangeCode">The exchange code.</param>
    /// <param name="stockType">The stock type.</param>
    /// <returns>Returns symbols model.</returns>
    public async Task<List<SymbolsModel>> GetExchangeSymbolsAsync(string exchangeCode, StockTypes? stockType)
    {
        var exchangeCodeModel = unitOfWorkAsync.GetGenericRepository<ExchangeCode>()
            .GetQueryable(exchange => exchange.Code == exchangeCode, null).FirstOrDefault();
        if (exchangeCodeModel == null)
            return null;

        var symbolsList = unitOfWorkAsync.GetGenericRepository<ExchangeSymbol>()
            .GetQueryable(symbol => symbol.ExchangeCodeId == exchangeCodeModel.Id && 
            (stockType == null || symbol.Type == stockType.GetEnumDisplayName()), null).ToList();
        if(symbolsList != null)
        {
            if (symbolsList.Any())
            {
                //symbolsList = symbolsList.Where(symbol => symbol.DataSyncCompleted && symbol.QuarterlySyncCompleted && symbol.AnnualSyncCompleted).ToList();
                //if(!symbolsList.Any())
                //    return new List<SymbolsModel>();
                var isDataOld = symbolsList.Any(symbol => (DateTime.UtcNow - (DateTime)symbol.CreatedDateTime).TotalDays > Constants.SyncDates );
                if (!isDataOld)
                    return MapSymbolsData(symbolsList);
            }
        }
        return await exchangeHelperService.GetExchangeSymbolsListAsync(exchangeCode, stockType);
    }

    private List<SymbolsModel> MapSymbolsData(List<ExchangeSymbol> symbolsList)
    {
        var mappings = new List<SymbolsModel>();
        foreach (var symbol in symbolsList)
        {
            mappings.Add(new SymbolsModel()
            {
                Code = symbol.Code,
                Name = symbol.Name,
                ExchangeCode = symbol.ExchangeCodeModel.Code,
                Exchange = symbol.Exchange,
                Country = symbol.Country,
                Currency = symbol.Currency,
                Type = symbol.Type
            });
        }
        return mappings;
    }

    /// <summary>
    /// Gets the exchange codes asynchronous.
    /// </summary>
    /// <returns>Returns list of exchange codes.</returns>
    public async Task<List<ExchangeModel>> GetExchangeCodesAsync()
    {
        var exchangeCodesList = unitOfWorkAsync.GetGenericRepository<ExchangeCode>()
            .GetAll().ToList();
        if (exchangeCodesList == null)
            return null;

        if (exchangeCodesList.Any())
        {
            var isDataOld = exchangeCodesList.Any(code => ((DateTime)code.CreatedDateTime).AddDays(Constants.SyncDates) <= DateTime.UtcNow);
            if (!isDataOld)
            {
                var mappings = new List<ExchangeModel>();
                foreach (var code in exchangeCodesList)
                {
                    mappings.Add(new ExchangeModel()
                    {
                        Code = code.Code,
                        Name = code.Name,
                    });
                }
                return mappings;
            }
        }
        return await exchangeHelperService.GetExchangeListAsync();
    }

    /// <summary>
    /// Gets all exchange symbols.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="currentSymbolsPage">The current symbols page.</param>
    /// <returns>
    /// Returns symbols list.
    /// </returns>
    public List<SymbolsModel> GetAllExchangeSymbols(StockTypes? type, int currentSymbolsPage, string searchKeyword)
    {
        var exchangeSymbols = unitOfWorkAsync.GetGenericRepository<ExchangeSymbol>()
            .GetQueryable(x=> 
            (type == null || x.Type == type.GetEnumDisplayName()) && 
            (string.IsNullOrEmpty(searchKeyword) || x.Name.Contains(searchKeyword) || x.Code.Contains(searchKeyword) 
            || x.Exchange.Contains(searchKeyword) || x.ExchangeCodeModel.Code.Contains(searchKeyword) || x.Country.Contains(searchKeyword)), 
            null)
            .Skip((currentSymbolsPage - 1) * 3)
            .Take(30)
            .Include(x => x.ExchangeCodeModel).ToList();
        return MapSymbolsData(exchangeSymbols);
    }
}
