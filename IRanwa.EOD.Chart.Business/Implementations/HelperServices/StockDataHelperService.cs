using IRanwa.EOD.Chart.Core;
using IRanwa.EOD.Chart.Data;
using IRanwa.EOD.Chart.Model;
using System;
using System.Globalization;
using System.Net.Http.Json;

namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// Stock data helper service.
/// </summary>
/// <seealso cref="IStockDataHelperService" />
public class StockDataHelperService : IStockDataHelperService
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
    /// Initializes a new instance of the <see cref="StockDataHelperService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="unitOfWorkAsync">The unit of work async.</param>
    public StockDataHelperService(IEODHttpClient httpClient, IUnitOfWorkAsync unitOfWorkAsync)
    {
        this.httpClient = httpClient;
        this.unitOfWorkAsync = unitOfWorkAsync;
    }

    /// <summary>
    /// Formats to currency.
    /// </summary>
    /// <param name="dataList">The data list.</param>
    /// <param name="exchangeCodeModel">The exchange code.</param>
    /// <returns>
    /// Returns formatted data.
    /// </returns>
    public Dictionary<string, string> FormatToCurrency(Dictionary<string, double> dataList, ExchangeCode exchangeCodeModel)
    {
        var newData = new Dictionary<string, string>();
        foreach (var data in dataList)
            newData.Add(data.Key, $"{exchangeCodeModel.Currency} {Math.Round(data.Value, 2).ToString("N", new CultureInfo("en-US"))}");
        return newData;
    }

    /// <summary>
    /// Converts to multiply.
    /// </summary>
    /// <param name="dataList">The data list.</param>
    /// <returns>
    /// Returns formatted data.
    /// </returns>
    public Dictionary<string, string> ConvertToMultiply(Dictionary<string, double> dataList)
    {
        var newData = new Dictionary<string, string>();
        foreach (var data in dataList)
            newData.Add(data.Key, $"{Math.Round(data.Value, 2)}x");
        return newData;
    }

    /// <summary>
    /// Converts to percentage.
    /// </summary>
    /// <param name="dataList">The data list.</param>
    /// <returns>
    /// Returns formatted data.
    /// </returns>
    public Dictionary<string, string> ConvertToPercentage(Dictionary<string, double> dataList)
    {
        var newData = new Dictionary<string, string>();
        foreach (var data in dataList)
            newData.Add(data.Key, $"{Math.Round(data.Value, 2)}%");
        return newData;
    }

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <param name="dataList">The data list.</param>
    /// <returns>
    /// Returns formatted data.
    /// </returns>
    public Dictionary<string, string> ConvertToString(Dictionary<string, double> dataList)
    {
        var newData = new Dictionary<string, string>();
        foreach (var data in dataList)
            newData.Add(data.Key, $"{Math.Round(data.Value, 2)}");
        return newData;
    }

    /// <summary>
    /// Gets the eod data.
    /// </summary>
    /// <param name="eodDataModels">The eod data models.</param>
    /// <param name="date">The date.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns eod data.</returns>
    public EODDataModel GetEODData(List<EODDataModel> eodDataModels, string date, PeriodTypes period)
    {
        var originalDate = DateTime.Parse(date);
        var currentDate = DateTime.Parse(date);
        EODDataModel eodData = null;
        do
        {
            if (period == PeriodTypes.Quarterly)
            {
                eodData = eodDataModels.FirstOrDefault(eod =>
                eod.Date == currentDate.ToString("yyyy-MM-dd"));
                if (eodData == null)
                    currentDate = currentDate.AddDays(-1);
                if (originalDate.Subtract(currentDate).TotalDays > Constants.MaxDurationDays)
                    break;
            }
            else
            {
                eodData = eodDataModels.FirstOrDefault(eod =>
                DateTime.ParseExact(eod.Date, "yyyy-MM-dd",
                CultureInfo.InvariantCulture).ToString("yyyy") == currentDate.ToString("yyyy"));
                break;
            }
        } while (eodData == null);
        return eodData;
    }

    /// <summary>
    /// Gets the eps data.
    /// </summary>
    /// <param name="epsDataModels">The eps data models.</param>
    /// <param name="date">The date.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns eps data.</returns>
    public EPSModel GetEPSData(List<EPSModel> epsDataModels, string date, PeriodTypes period)
    {
        var originalDate = DateTime.Parse(date);
        var currentDate = DateTime.Parse(date);
        EPSModel epsData = null;
        do
        {
            if (period == PeriodTypes.Quarterly)
            {
                epsData = epsDataModels.FirstOrDefault(eps =>
                eps.Date == currentDate.ToString("yyyy-MM-dd"));
                if (epsData == null)
                    currentDate = currentDate.AddDays(-1);
                if (originalDate.Subtract(currentDate).TotalDays > Constants.MaxDurationDays)
                    break;
            }
            else
            {
                epsData = epsDataModels.FirstOrDefault(eps =>
                DateTime.ParseExact(eps.Date, "yyyy-MM-dd",
                CultureInfo.InvariantCulture).ToString("yyyy") == currentDate.ToString("yyyy"));
                break;
            }
        } while (epsData == null);
        return epsData;
    }

    /// <summary>
    /// Gets the balance sheet data.
    /// </summary>
    /// <param name="balanceSheetDataModels">The balance sheet data models.</param>
    /// <param name="date">The date.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns balance sheet data.</returns>
    public BalanceSheetModel GetBalanceSheetData(List<BalanceSheetModel> balanceSheetDataModels, string date, PeriodTypes period)
    {
        var originalDate = DateTime.Parse(date);
        var currentDate = DateTime.Parse(date);
        BalanceSheetModel balanceSheetData = null;
        do
        {
            if (period == PeriodTypes.Quarterly)
            {
                balanceSheetData = balanceSheetDataModels.FirstOrDefault(balanceSheet =>
                balanceSheet.Date == currentDate.ToString("yyyy-MM-dd"));
                if (balanceSheetData == null)
                    currentDate = currentDate.AddDays(-1);
                if (originalDate.Subtract(currentDate).TotalDays > Constants.MaxDurationDays)
                    break;
            }
            else
            {
                balanceSheetData = balanceSheetDataModels.FirstOrDefault(balanceSheet =>
                DateTime.ParseExact(balanceSheet.Date, "yyyy-MM-dd",
                CultureInfo.InvariantCulture).ToString("yyyy") == currentDate.ToString("yyyy"));
                break;
            }
        } while (balanceSheetData == null);
        return balanceSheetData;
    }

    /// <summary>
    /// Gets the outstanding shares data.
    /// </summary>
    /// <param name="outstandingSharesDataModels">The outstanding shares data models.</param>
    /// <param name="date">The date.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns outstanding shares data.</returns>
    public OutstandingSharesModel GetOutstandingSharesData(List<OutstandingSharesModel> outstandingSharesDataModels,
        string date, PeriodTypes period)
    {
        var originalDate = DateTime.Parse(date);
        var currentDate = DateTime.Parse(date);
        OutstandingSharesModel outstandingSharesData = null;
        do
        {
            if (period == PeriodTypes.Quarterly)
            {
                outstandingSharesData = outstandingSharesDataModels.FirstOrDefault(outstandingShares =>
                outstandingShares.DateFormatted == currentDate.ToString("yyyy-MM-dd"));
                if (outstandingSharesData == null)
                    currentDate = currentDate.AddDays(-1);
                if (originalDate.Subtract(currentDate).TotalDays > Constants.MaxDurationDays)
                    break;
            }
            else
            {
                outstandingSharesData = outstandingSharesDataModels.FirstOrDefault(outstandingShares =>
                DateTime.ParseExact(outstandingShares.DateFormatted, "yyyy-MM-dd",
                CultureInfo.InvariantCulture).ToString("yyyy") == currentDate.ToString("yyyy"));
                break;
            }
        } while (outstandingSharesData == null);
        return outstandingSharesData;
    }

    /// <summary>
    /// Gets the income statement.
    /// </summary>
    /// <param name="incomeStatementModels">The income statement models.</param>
    /// <param name="date">The date.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns income statement.</returns>
    public IncomeStatementModel GetIncomeStatement(List<IncomeStatementModel> incomeStatementModels,
        string date, PeriodTypes period)
    {
        var originalDate = DateTime.Parse(date);
        var currentDate = DateTime.Parse(date);
        IncomeStatementModel incomeStatementData = null;
        do
        {
            if (period == PeriodTypes.Quarterly)
            {
                incomeStatementData = incomeStatementModels.FirstOrDefault(incomeStatement =>
                incomeStatement.Date == currentDate.ToString("yyyy-MM-dd"));
                if (incomeStatementData == null)
                    currentDate = currentDate.AddDays(-1);
                if (originalDate.Subtract(currentDate).TotalDays > Constants.MaxDurationDays)
                    break;
            }
            else
            {
                incomeStatementData = incomeStatementModels.FirstOrDefault(incomeStatement =>
                DateTime.ParseExact(incomeStatement.Date, "yyyy-MM-dd",
                CultureInfo.InvariantCulture).ToString("yyyy") == currentDate.ToString("yyyy"));
                break;
            }
        } while (incomeStatementData == null);
        return incomeStatementData;
    }

    /// <summary>
    /// Gets the cash flow.
    /// </summary>
    /// <param name="cashFlowModels">The cash flow models.</param>
    /// <param name="date">The date.</param>
    /// <param name="period">The period.</param>
    /// <returns>
    /// Returns cash flow model.
    /// </returns>
    public CashFlowModel GetCashFlow(List<CashFlowModel> cashFlowModels, string date, PeriodTypes period)
    {
        var originalDate = DateTime.Parse(date);
        var currentDate = DateTime.Parse(date);
        CashFlowModel cashFlowData = null;
        do
        {
            if (period == PeriodTypes.Quarterly)
            {
                cashFlowData = cashFlowModels.FirstOrDefault(cashFlow =>
                cashFlow.Date == currentDate.ToString("yyyy-MM-dd"));
                if (cashFlowData == null)
                    currentDate = currentDate.AddDays(-1);
                if (originalDate.Subtract(currentDate).TotalDays > Constants.MaxDurationDays)
                    break;
            }
            else
            {
                cashFlowData = cashFlowModels.FirstOrDefault(cashFlow =>
                DateTime.ParseExact(cashFlow.Date, "yyyy-MM-dd",
                CultureInfo.InvariantCulture).ToString("yyyy") == currentDate.ToString("yyyy"));
                break;
            }
        } while (cashFlowData == null);
        return cashFlowData;
    }

    #region APIs    
    /// <summary>
    /// Gets the outstanding shares asynchronous.
    /// </summary>
    /// <param name="fullSymbolCode">The full symbol code.</param>
    /// <param name="periodType">Type of the period.</param>
    /// <returns>
    /// Returns list of outstandings.
    /// </returns>
    public async Task<List<OutstandingSharesModel>> GetOutstandingSharesAsync(string fullSymbolCode, string periodType)
    {
        var parameters = new Dictionary<string, string>()
        {
            { "filter", $"outstandingShares::{periodType}" }
        };
        var response = await httpClient.GetAsync($"api/fundamentals/{fullSymbolCode}", parameters);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<Dictionary<string, OutstandingSharesModel>>();
            if (data != null)
                return data.Select(record => record.Value).ToList();
        }
        return null;
    }

    /// <summary>
    /// Gets the eod data asynchronous.
    /// </summary>
    /// <param name="fullSymbolCode">The full symbol code.</param>
    /// <param name="fromDate">From date.</param>
    /// <param name="toDate">To date.</param>
    /// <param name="order">The order.</param>
    /// <returns>Returns list of eod data.</returns>
    public async Task<List<EODDataModel>> GetEODDataAsync(string fullSymbolCode, string fromDate = null, string toDate = null, string order = null)
    {
        var parameters = new Dictionary<string, string>()
        {
            { "from", fromDate },
            { "to", toDate },
            { "order", order },
        };
        var response = await httpClient.GetAsync($"api/eod/{fullSymbolCode}", parameters);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<EODDataModel>>();
        return null;
    }

    /// <summary>
    /// Gets the eps asynchronous.
    /// </summary>
    /// <param name="fullSymbolCode">The full symbol code.</param>
    /// <returns>Retruns list of eps data.</returns>
    public async Task<List<EPSModel>> GetEPSAsync(string fullSymbolCode)
    {
        var parameters = new Dictionary<string, string>()
        {
            { "filter", $"Earnings::History" }
        };
        var response = await httpClient.GetAsync($"api/fundamentals/{fullSymbolCode}", parameters);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<Dictionary<string, EPSModel>>();
            if (data != null)
                return data.Select(record => record.Value).ToList();
        }
        return null;
    }

    /// <summary>
    /// Gets the balance sheet asynchronous.
    /// </summary>
    /// <param name="fullSymbolCode">The full symbol code.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns list of balance sheet</returns>
    public async Task<List<BalanceSheetModel>> GetBalanceSheetAsync(string fullSymbolCode, string period)
    {
        var periodTxt = period == "annual" ? "yearly" : period;
        var parameters = new Dictionary<string, string>()
        {
            { "filter", $"Financials::Balance_Sheet::{periodTxt}" }
        };
        var response = await httpClient.GetAsync($"api/fundamentals/{fullSymbolCode}", parameters);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<Dictionary<string, BalanceSheetModel>>();
            if (data != null)
                return data.Select(record => record.Value).ToList();
        }
        return null;
    }

    /// <summary>
    /// Gets the income statement asynchronous.
    /// </summary>
    /// <param name="fullSymbolCode">The full symbol code.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns list of income statement.</returns>
    public async Task<List<IncomeStatementModel>> GetIncomeStatementAsync(string fullSymbolCode, string period)
    {
        var periodTxt = period == "annual" ? "yearly" : period;
        var parameters = new Dictionary<string, string>()
        {
            { "filter", $"Financials::Income_Statement::{periodTxt}" }
        };
        var response = await httpClient.GetAsync($"api/fundamentals/{fullSymbolCode}", parameters);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<Dictionary<string, IncomeStatementModel>>();
            if (data != null)
                return data.Select(record => record.Value).ToList();
        }
        return null;
    }

    /// <summary>
    /// Gets the cash flow asynchronous.
    /// </summary>
    /// <param name="fullSymbolCode">The full symbol code.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns list of cash flow.</returns>
    public async Task<List<CashFlowModel>> GetCashFlowAsync(string fullSymbolCode, string period)
    {
        var periodTxt = period == "annual" ? "yearly" : period;
        var parameters = new Dictionary<string, string>()
        {
            { "filter", $"Financials::Cash_Flow::{periodTxt}" }
        };
        var response = await httpClient.GetAsync($"api/fundamentals/{fullSymbolCode}", parameters);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<Dictionary<string, CashFlowModel>>();
            if (data != null)
                return data.Select(record => record.Value).ToList();
        }
        return null;
    }
    #endregion

    /// <summary>
    /// Synchronizes the eod data asynchronous.
    /// </summary>
    /// <param name="eodDataModels">The eod data models.</param>
    /// <param name="symbolId">The symbol identifier.</param>
    public async Task SyncEODDataAsync(List<EODDataModel> eodDataModels, int symbolId)
    {
        var existingEODData = unitOfWorkAsync.GetGenericRepository<EODData>()
            .GetQueryable(eod => eod.ExchangeSymbol == symbolId, null).ToList();

        var existingTimeStamps = existingEODData.Select(eod => eod.Timestamp);
        var eodDataModelsTimeStamps = eodDataModels.Select(eodData => 
            ((DateTimeOffset)DateTime.Parse(eodData.Date).ToUniversalTime()).ToUnixTimeSeconds());

        var newEODData = eodDataModels.Where(eodData => 
            !existingTimeStamps.Contains(((DateTimeOffset)DateTime.Parse(eodData.Date).ToUniversalTime()).ToUnixTimeSeconds())).ToList();
        var existingEODDataToUpdate = existingEODData.Where(eodData => eodDataModelsTimeStamps.Contains(eodData.Timestamp)).ToList();

        foreach (var model in newEODData)
        {
            var dateTime = DateTime.Parse(model.Date).ToUniversalTime();
            var timestamp = ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
            var eodData = new EODData()
            {
                Timestamp = timestamp,
                ExchangeSymbol = symbolId,
                AdjustedClose = model.Adjusted_Close,
                Close = model.Close,
                High = model.High,
                Low = model.Low,
                Open = model.Open,
                Volume = model.Volume,
                CreatedDateTime = DateTime.UtcNow
            };
            await unitOfWorkAsync.GetGenericRepository<EODData>().Add(eodData);
        }
        unitOfWorkAsync.SaveChanges();

        foreach(var updateEODData in existingEODDataToUpdate)
        {
            var eodDataModel = eodDataModels.FirstOrDefault(eodData => 
            ((DateTimeOffset)DateTime.Parse(eodData.Date).ToUniversalTime()).ToUnixTimeSeconds() == updateEODData.Timestamp);
            if (eodDataModel == null)
                continue;

            if((updateEODData.Open != eodDataModel.Open) ||
                (updateEODData.Close != eodDataModel.Close) ||
                (updateEODData.High != eodDataModel.High) ||
                (updateEODData.Low != eodDataModel.Low) ||
                (updateEODData.AdjustedClose != eodDataModel.Adjusted_Close) ||
                (updateEODData.Volume != eodDataModel.Volume))
            {
                updateEODData.Open = eodDataModel.Open;
                updateEODData.Close = eodDataModel.Close;
                updateEODData.High = eodDataModel.High;
                updateEODData.Low = eodDataModel.Low;
                updateEODData.AdjustedClose = eodDataModel.Adjusted_Close;
                updateEODData.Volume = eodDataModel.Volume;
                updateEODData.ModifiedDateTime = DateTime.UtcNow;

                unitOfWorkAsync.GetGenericRepository<EODData>().Update(updateEODData);
            }
        }
        unitOfWorkAsync.SaveChanges();
    }

    /// <summary>
    /// Gets the eod live data asynchronous.
    /// </summary>
    /// <param name="fullSymbolCode">The full symbol code.</param>
    /// <returns>Returns eod data model.</returns>
    public async Task<EODDataModel> GetEODLiveDataAsync(string fullSymbolCode)
    {
        var response = await httpClient.GetAsync($"api/real-time/{fullSymbolCode}", null);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<EODDataModel>();
        return null;
    }

    /// <summary>
    /// Synchronizes the eod live data asynchronous.
    /// </summary>
    /// <param name="eodDataModel">The eod data model.</param>
    /// <param name="symbolId">The symbol identifier.</param>
    public async Task SyncEODLiveDataAsync(EODDataModel eodDataModel, int symbolId)
    {
        var existingEODData = unitOfWorkAsync.GetGenericRepository<EODLiveData>()
            .GetOne(eod => eod.ExchangeSymbol == symbolId);
        if(existingEODData == null)
        {
            var eodLiveData = new EODLiveData()
            {
                Timestamp = (long)eodDataModel.Timestamp,
                ExchangeSymbol = symbolId,
                AdjustedClose = eodDataModel.Adjusted_Close,
                Close = eodDataModel.Close,
                High = eodDataModel.High,
                Low = eodDataModel.Low,
                Open = eodDataModel.Open,
                Volume = eodDataModel.Volume,
                CreatedDateTime = DateTime.UtcNow
            };
            await unitOfWorkAsync.GetGenericRepository<EODLiveData>().Add(eodLiveData);
        }
        else
        {
            if ((existingEODData.Open != eodDataModel.Open) ||
                (existingEODData.Close != eodDataModel.Close) ||
                (existingEODData.High != eodDataModel.High) ||
                (existingEODData.Low != eodDataModel.Low) ||
                (existingEODData.AdjustedClose != eodDataModel.Adjusted_Close) ||
                (existingEODData.Volume != eodDataModel.Volume) ||
                (existingEODData.Timestamp != eodDataModel.Timestamp))
            {
                existingEODData.Open = eodDataModel.Open;
                existingEODData.Close = eodDataModel.Close;
                existingEODData.High = eodDataModel.High;
                existingEODData.Low = eodDataModel.Low;
                existingEODData.AdjustedClose = eodDataModel.Adjusted_Close;
                existingEODData.Volume = eodDataModel.Volume;
                existingEODData.ModifiedDateTime = DateTime.UtcNow;

                unitOfWorkAsync.GetGenericRepository<EODLiveData>().Update(existingEODData);
            }
        }
        unitOfWorkAsync.SaveChanges();
    }

    /// <summary>
    /// Gets the total eps per year data.
    /// </summary>
    /// <param name="epsDataModels">The eps data models.</param>
    /// <param name="date">The date.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns TTM eps data.</returns>
    public double? GetTotalEPSPerYearData(List<EPSModel> epsDataModels, string date, PeriodTypes period)
    {
        var originalDate = DateTime.Parse(date);
        var currentDate = DateTime.Parse(date);
        if (period == PeriodTypes.Quarterly)
        {
            var startDate = currentDate.AddYears(-1);
            var epsList = epsDataModels.Where(eps => DateTime.Parse(eps.Date) > startDate &&
            DateTime.Parse(eps.Date) <= currentDate);

            var epsSumValue = 0.0;
            foreach (var eps in epsList)
                if (eps.EpsActual != null)
                    epsSumValue += (double)eps.EpsActual;
            if (epsSumValue > 0)
                return epsSumValue;
        }
        else
        {
            var epsData = epsDataModels.FirstOrDefault(eps =>
            DateTime.ParseExact(eps.Date, "yyyy-MM-dd",
            CultureInfo.InvariantCulture).ToString("yyyy") == currentDate.ToString("yyyy"));
            if (epsData != null)
                return epsData.EpsActual;
        }
        return null;
    }

    /// <summary>
    /// Gets the total revenue per year data.
    /// </summary>
    /// <param name="incomeStatementModels">The income statement models.</param>
    /// <param name="date">The date.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns total revenue.</returns>
    public double? GetTotalRevenuePerYearData(List<IncomeStatementModel> incomeStatementModels, string date, PeriodTypes period)
    {
        var originalDate = DateTime.Parse(date);
        var currentDate = DateTime.Parse(date);
        if (period == PeriodTypes.Quarterly)
        {
            var startDate = currentDate.AddYears(-1);
            var incomeStatements = incomeStatementModels.Where(income => DateTime.Parse(income.Date) > startDate &&
            DateTime.Parse(income.Date) <= currentDate);

            var totalRevSum = 0.0;
            foreach (var incomeStatement in incomeStatements)
                if (incomeStatement.TotalRevenue != null)
                    totalRevSum += (double)incomeStatement.TotalRevenue;
            if (totalRevSum > 0)
                return totalRevSum;
        }
        else
        {
            var incomeStatement = incomeStatementModels.FirstOrDefault(income =>
            DateTime.ParseExact(income.Date, "yyyy-MM-dd",
            CultureInfo.InvariantCulture).ToString("yyyy") == currentDate.ToString("yyyy"));
            if (incomeStatement != null)
                return incomeStatement.TotalRevenue;
        }
        return null;
    }

    /// <summary>
    /// Gets the total ebitda per year data.
    /// </summary>
    /// <param name="incomeStatementModels">The income statement models.</param>
    /// <param name="date">The date.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns total ebitda.</returns>
    public double? GetTotalEbitdaPerYearData(List<IncomeStatementModel> incomeStatementModels, string date, PeriodTypes period)
    {
        var originalDate = DateTime.Parse(date);
        var currentDate = DateTime.Parse(date);
        if (period == PeriodTypes.Quarterly)
        {
            var startDate = currentDate.AddYears(-1);
            var incomeStatements = incomeStatementModels.Where(income => DateTime.Parse(income.Date) > startDate &&
            DateTime.Parse(income.Date) <= currentDate);

            var totalEbitda = 0.0;
            foreach (var incomeStatement in incomeStatements)
                if (incomeStatement.Ebitda != null)
                    totalEbitda += (double)incomeStatement.Ebitda;
            if (totalEbitda > 0)
                return totalEbitda;
        }
        else
        {
            var incomeStatement = incomeStatementModels.FirstOrDefault(income =>
            DateTime.ParseExact(income.Date, "yyyy-MM-dd",
            CultureInfo.InvariantCulture).ToString("yyyy") == currentDate.ToString("yyyy"));
            if (incomeStatement != null)
                return incomeStatement.TotalRevenue;
        }
        return null;
    }

    public double? GetTotalCashFromOperatingActivitiesPerYearData(List<CashFlowModel> cashFlowModels, string date, PeriodTypes period)
    {
        var originalDate = DateTime.Parse(date);
        var currentDate = DateTime.Parse(date);
        if (period == PeriodTypes.Quarterly)
        {
            var startDate = currentDate.AddYears(-1);
            var cashFlows = cashFlowModels.Where(cash => DateTime.Parse(cash.Date) > startDate &&
            DateTime.Parse(cash.Date) <= currentDate);

            var totalCashFromOperatingActivites = 0.0;
            foreach (var cashFlow in cashFlows)
                if (cashFlow.TotalCashFromOperatingActivities != null)
                    totalCashFromOperatingActivites += (double)cashFlow.TotalCashFromOperatingActivities;
            if (totalCashFromOperatingActivites > 0)
                return totalCashFromOperatingActivites;
        }
        else
        {
            var cashFlow = cashFlowModels.FirstOrDefault(cash =>
            DateTime.ParseExact(cash.Date, "yyyy-MM-dd",
            CultureInfo.InvariantCulture).ToString("yyyy") == currentDate.ToString("yyyy"));
            if (cashFlow != null)
                return cashFlow.TotalCashFromOperatingActivities;
        }
        return null;
    }
}
