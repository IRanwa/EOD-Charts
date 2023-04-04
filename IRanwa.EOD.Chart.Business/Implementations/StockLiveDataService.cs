using IRanwa.EOD.Chart.Core;
using IRanwa.EOD.Chart.Data;
using IRanwa.EOD.Chart.Model;
using System.Globalization;

namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// Stock live data service.
/// </summary>
/// <seealso cref="IStockLiveDataService" />
public class StockLiveDataService : IStockLiveDataService
{
    /// <summary>
    /// The unit of work asynchronous
    /// </summary>
    private readonly IUnitOfWorkAsync unitOfWorkAsync;

    /// <summary>
    /// The stock data helper service/
    /// </summary>
    private readonly IStockDataHelperService stockDataHelperService;

    /// <summary>
    /// Initializes a new instance of the <see cref="StockLiveDataService"/> class.
    /// </summary>
    /// <param name="unitOfWorkAsync">The unit of work asynchronous.</param>
    /// <param name="stockDataHelperService">The stock data helper service.</param>
    public StockLiveDataService(
        IUnitOfWorkAsync unitOfWorkAsync,
        IStockDataHelperService stockDataHelperService)
    {
        this.unitOfWorkAsync = unitOfWorkAsync;
        this.stockDataHelperService = stockDataHelperService;
    }

    /// <summary>
    /// Gets the stock live history data asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns eod data view model.</returns>
    public async Task<List<EODDataViewModel>> GetStockLiveHistoryDataAsync(StockDataViewModel model)
    {
        var exchangeCode = model.ExchangeCode;
        var symbolCode = model.Symbol;
        var lastTimeStamp = model.LastTimeStamp;
        var fetchCount = Constants.MaximumLiveDataFetchCount;
        switch (model.FrequencyType)
        {
            case FrequencyTypes.Monthly:
                fetchCount = Constants.MaximumLiveDataFetchCount * 30;
                break;
            case FrequencyTypes.Weekly:
                fetchCount = Constants.MaximumLiveDataFetchCount * 7;
                break;
        }

        var exchange = unitOfWorkAsync.GetGenericRepository<ExchangeCode>()
            .GetOne(e => e.Code == exchangeCode);
        if (exchange == null)
            return null;

        var symbol = unitOfWorkAsync.GetGenericRepository<ExchangeSymbol>()
            .GetOne(s => s.Code == symbolCode && s.ExchangeCodeId == exchange.Id);
        if (symbol == null)
            return null;

        Func<IQueryable<EODData>, IOrderedQueryable<EODData>> orderBy = eodData => eodData.OrderByDescending(x => x.Timestamp);
        var existingData = unitOfWorkAsync.GetGenericRepository<EODData>()
            .GetQueryable(eodData => eodData.ExchangeSymbol == symbol.Id && (lastTimeStamp == null || eodData.Timestamp < lastTimeStamp), orderBy)
            .Take(fetchCount).ToList();
        
        var eodDataList = new List<EODDataViewModel>();
        if (existingData.Any())
        {
            foreach (var existingRecord in existingData.OrderBy(x => x.Timestamp))
            {
                eodDataList.Add(new EODDataViewModel()
                {
                    TimeStamp = existingRecord.Timestamp,
                    Data = new double[] { (double)existingRecord.Open, (double)existingRecord.High, (double)existingRecord.Low, (double)existingRecord.Close }
                });
            }

            if (lastTimeStamp == null)
            {
                var lastExistingRecord = existingData.First();
                if (((((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds() - lastExistingRecord.Timestamp) / (60 * 60 * 24)) > 1)
                {
                    var fromDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    fromDate = fromDate.AddSeconds(lastExistingRecord.Timestamp).ToUniversalTime();

                    var fullSymbol = $"{symbolCode}.{exchangeCode}";
                    var newEodData = await stockDataHelperService.GetEODDataAsync(fullSymbol, fromDate.ToString("yyyy-MM-dd"), string.Empty, OrderingType.Ascending.GetEnumDisplayName());
                    await stockDataHelperService.SyncEODDataAsync(newEodData, symbol.Id);
                    foreach (var newEod in newEodData)
                    {
                        var existing = eodDataList.Any(x => x.TimeStamp == ((DateTimeOffset)DateTime.Parse(newEod.Date)).ToUnixTimeSeconds());
                        if (!existing)
                        {
                            eodDataList.Add(new EODDataViewModel()
                            {
                                TimeStamp = ((DateTimeOffset)DateTime.Parse(newEod.Date)).ToUnixTimeSeconds(),
                                Data = new double[] { (double)newEod.Open, (double)newEod.High, (double)newEod.Low, (double)newEod.Close }
                            });
                        }
                    }
                }
            }
        }
        else if(lastTimeStamp == null)
        {
            var fullSymbol = $"{symbolCode}.{exchangeCode}";
            var newEodData = await stockDataHelperService.GetEODDataAsync(fullSymbol);
            await stockDataHelperService.SyncEODDataAsync(newEodData, symbol.Id);
            foreach (var newEod in newEodData)
            {
                eodDataList.Add(new EODDataViewModel()
                {
                    TimeStamp = ((DateTimeOffset)DateTime.Parse(newEod.Date)).ToUnixTimeSeconds(),
                    Data = new double[] { (double)newEod.Open, (double)newEod.High, (double)newEod.Low, (double)newEod.Close }
                });
            }
        }
        
        eodDataList = eodDataList.OrderByDescending(x => x.TimeStamp).Take(fetchCount).ToList();
        eodDataList = GetDataByFrequency(eodDataList, (FrequencyTypes)model.FrequencyType);
        eodDataList = eodDataList.OrderBy(x => x.TimeStamp).ToList();
        return eodDataList;
    }

    /// <summary>
    /// Gets the live stock data asynchronous.
    /// </summary>
    /// <param name="exchangeCode">The exchange code.</param>
    /// <param name="symbolCode">The symbol code.</param>
    /// <returns>Returns live eod data.</returns>
    public async Task<EODDataViewModel> GetLiveStockDataAsync(string exchangeCode, string symbolCode)
    {
        var exchange = unitOfWorkAsync.GetGenericRepository<ExchangeCode>()
            .GetOne(e => e.Code == exchangeCode);
        if (exchange == null)
            return null;

        var symbol = unitOfWorkAsync.GetGenericRepository<ExchangeSymbol>()
            .GetOne(s => s.Code == symbolCode && s.ExchangeCodeId == exchange.Id);
        if (symbol == null)
            return null;

        var existingData = unitOfWorkAsync.GetGenericRepository<EODLiveData>()
            .GetOne(eodData => eodData.ExchangeSymbol == symbol.Id);

        var eodData = new EODDataViewModel();
        if (existingData != null)
        {
            if (((((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds() - existingData.Timestamp) / (60)) > 1)
                eodData = await GetSyncLiveDataAsync(symbolCode, exchangeCode, symbol);
            else
            {
                eodData = new EODDataViewModel()
                {
                    TimeStamp = existingData.Timestamp,
                    Data = new double[] { (double)existingData.Open, (double)existingData.High, (double)existingData.Low, (double)existingData.Close }
                };
            }
        }
        else
            eodData = await GetSyncLiveDataAsync(symbolCode, exchangeCode, symbol);
        return eodData;
    }

    /// <summary>
    /// Gets the synchronize live data asynchronous.
    /// </summary>
    /// <param name="symbolCode">The symbol code.</param>
    /// <param name="exchangeCode">The exchange code.</param>
    /// <param name="symbol">The symbol.</param>
    /// <returns>Returns eod data.</returns>
    private async Task<EODDataViewModel> GetSyncLiveDataAsync(string symbolCode, string exchangeCode, ExchangeSymbol symbol)
    {
        var fullSymbol = $"{symbolCode}.{exchangeCode}";
        var newEodData = await stockDataHelperService.GetEODLiveDataAsync(fullSymbol);
        if (newEodData != null)
        {
            await stockDataHelperService.SyncEODLiveDataAsync(newEodData, symbol.Id);
            return new EODDataViewModel()
            {
                TimeStamp = (long)newEodData.Timestamp,
                Data = new double[] { (double)newEodData.Open, (double)newEodData.High, (double)newEodData.Low, (double)newEodData.Close }
            };
        }
        return null;
    }

    private List<EODDataViewModel> GetDataByFrequency(List<EODDataViewModel> eodDataList, FrequencyTypes frequencyType)
    {
        if (frequencyType == FrequencyTypes.Weekly)
        {
            var yearWeekGroups = eodDataList.GroupBy(d => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
             new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(d.TimeStamp),
             CalendarWeekRule.FirstFourDayWeek,
             DayOfWeek.Sunday) + (new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(d.TimeStamp).Year * 53)).ToList();

            var newEODData = new List<EODDataViewModel>();
            foreach (var data in yearWeekGroups)
            {
                var weekData = data.OrderBy(x => x.TimeStamp).ToList();
                var weekFirstRecord = weekData.First();
                var weekLastRecord = weekData.Last();
                var high = data.Max(x => x.Data[1]);
                var low = data.Min(x => x.Data[2]);

                newEODData.Add(new EODDataViewModel()
                {
                    TimeStamp = weekFirstRecord.TimeStamp,
                    Data = new double[] { weekFirstRecord.Data[0], high, low, weekLastRecord.Data[3] }
                });
            }
            return newEODData;
        }
        else if (frequencyType == FrequencyTypes.Monthly)
        {
            var yearMonthGroup = eodDataList.GroupBy(d =>
                new
                {
                    Month = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(d.TimeStamp).Month,
                    Year = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(d.TimeStamp).Year,
                }).ToList();
            var newEODData = new List<EODDataViewModel>();
            foreach (var data in yearMonthGroup)
            {
                var weekData = data.OrderBy(x => x.TimeStamp).ToList();
                var weekFirstRecord = weekData.First();
                var weekLastRecord = weekData.Last();
                var high = data.Max(x => x.Data[1]);
                var low = data.Min(x => x.Data[2]);

                newEODData.Add(new EODDataViewModel()
                {
                    TimeStamp = weekFirstRecord.TimeStamp,
                    Data = new double[] { weekFirstRecord.Data[0], high, low, weekLastRecord.Data[3] }
                });
            }
            return newEODData;
        }
        return eodDataList;
    }
}
