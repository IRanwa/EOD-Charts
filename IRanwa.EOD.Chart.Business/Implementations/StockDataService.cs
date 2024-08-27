using IRanwa.EOD.Chart.Core;
using IRanwa.EOD.Chart.Data;
using IRanwa.EOD.Chart.Model;
using System.Reflection;

namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// Stock data service.
/// </summary>
/// <seealso cref="IStockDataService" />
public class StockDataService : IStockDataService
{
    /// <summary>
    /// Stock data helper service.
    /// </summary>
    private readonly IStockDataHelperService stockDataHelperService;

    /// <summary>
    /// The unit of work asynchronous
    /// </summary>
    private readonly IUnitOfWorkAsync unitOfWorkAsync;

    /// <summary>
    /// The row keys
    /// </summary>
    private readonly Dictionary<string, string> RowKeys = new Dictionary<string, string>()
            {
                {"marketcap", "Market Capitalization" },
                {"perate", "PE Rate" },
                {"pbrate", "PB Rate" },
                {"currentratio", "Current Ratio" },
                {"pricetosale", "Price to Sales" },
                {"evebitda", "EV/EBITDA" },
                {"evsales", "EV/Sales" },
                {"evopeningcashflow", "EV/Operating Cashflow" },
                {"earningyield", "Earnings Yield" },
                {"debittoassets", "Debt To Assets" },
                {"interestcoverage", "Interest Coverage Ratio" },
                {"payoutratio", "Payout Ratio" },
                {"roe", "ROE" },
                {"roa", "ROA" },
                {"roic", "ROIC" },
                {"quickratio", "Quick Ratio" },
                {"grossprofitmargin", "Gross Profit Margin" },
                {"dividendyield", "Dividend Yield" },
                {"pricetocashflow", "P/CF" },
                {"pricetofreecashflow", "PFCF" },
                {"freecashflowyield", "Free Cash Flow Yield" },
                {"debittoequity", "Debt to Equity" },
                {"debittoebitda", "Net Debt to EBITDA Ratio" },
                {"cashratio", "Cash Ratio" },
                {"debitratio", "Debt Ratio" },
                {"operatingprofitmargin", "Operating Profit Margin" },
                {"assetsturnoverratio", "Assets Turnover Ratio" },
                {"returnoncapitalemployed", "Return on Capital Employed(ROCE)" },
                {"ebitdamargin", "EBITDA Margin" },
                {"netincomemargin", "Net Income Margin" },
                {"ebitdainterestexpense", "EBITDA/Interest Expense" },
            };

    /// <summary>
    /// Initializes a new instance of the <see cref="StockDataService"/> class.
    /// </summary>
    /// <param name="stockDataHelperService">The stock data helper service.</param>
    /// <param name="unitOfWorkAsync">The unit of work asynchronous.</param>
    public StockDataService(
        IStockDataHelperService stockDataHelperService,
        IUnitOfWorkAsync unitOfWorkAsync)
    {
        this.stockDataHelperService = stockDataHelperService;
        this.unitOfWorkAsync = unitOfWorkAsync;
    }

    /// <summary>
    /// Gets the stock data asynchronous.
    /// </summary>
    /// <param name="symbolCode">The symbol code.</param>
    /// <param name="exchangeCode">The exchange code.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns final stock data model.</returns>
    public async Task<FinalStockDataModel> GetStockDataAsync(string symbolCode, string exchangeCode, PeriodTypes period)
    {
        var exchangeCodeModel = unitOfWorkAsync.GetGenericRepository<ExchangeCode>()
            .GetQueryable(exchange => exchange.Code == exchangeCode, null).FirstOrDefault();
        if (exchangeCodeModel == null)
            return null;

        var symbolModel = unitOfWorkAsync.GetGenericRepository<ExchangeSymbol>()
            .GetQueryable(symbol => symbol.Code == symbolCode && symbol.ExchangeCodeId == exchangeCodeModel.Id, null).FirstOrDefault();
        if (symbolModel == null)
            return null;

        var finalData = GetStockDataFromDB(period,symbolModel.Id);
        if (finalData != null)
        {
            var dates = finalData.MarketCap.Select(x => x.Key).ToList();
            finalData.Columns = dates;
            finalData.RowKeys = RowKeys;
            return finalData;
        }

        var fullSymbolCode = $"{symbolCode}.{exchangeCode}";
        var periodType = Enum.GetName(typeof(PeriodTypes), period).ToLower();

        var stockDetailsModel = await GetStockDetails(fullSymbolCode, periodType);

        var outstandingData = stockDetailsModel.OutstandingShares;
        var eodData = stockDetailsModel.EODData;
        var epsData = stockDetailsModel.EPSModels;
        var balanceSheetData = stockDetailsModel.BalanceSheets;
        var incomeStatement = stockDetailsModel.IncomeStatements;
        var cashFlow = stockDetailsModel.CashFlows;

        await stockDataHelperService.SyncEODDataAsync(eodData, symbolModel.Id);

        var marketCapData = CalculateMarketCap(outstandingData, eodData, period);
        var finalDataList = new List<FinalStockDataModel>();
        if (marketCapData != null)
        {
            var dates = marketCapData.Select(marketCap => marketCap.Key).ToList();
            var peRatios = CalculatePERatio(dates, eodData, epsData, period);
            var pbRatios = CalculatePBRatio(dates, balanceSheetData, eodData, outstandingData, period);
            var currentRatios = CalculateCurrentRatio(dates, balanceSheetData, period);
            var priceToSales = CalculatePriceToSales(dates, incomeStatement, outstandingData, eodData, epsData, period);
            var evEBITDAData = CalculateEvEBITDA(dates, marketCapData, balanceSheetData, incomeStatement, period);
            var evSalesData = CalculateEvSales(dates, marketCapData, balanceSheetData, incomeStatement, period);
            var evOpeningCashFlowData = CalculateEvOpeningCashFlow(dates, marketCapData, balanceSheetData, cashFlow, period);
            var earningYieldData = CalculateEarningYield(dates, epsData, eodData, period);
            var debitToAssetsData = CalculateDebitToAssets(dates, period, balanceSheetData);
            var interestCoverageData = CalculateInterestCoverageRatio(dates, period, incomeStatement);
            var payoutRatioData = CalculatePayoutRatio(dates, period, incomeStatement, cashFlow);
            var roeData = CalculateROE(dates, period, incomeStatement, balanceSheetData);
            var roaData = CalculateROA(dates, period, incomeStatement, balanceSheetData);
            var roicData = CalculateROIC(dates, period, incomeStatement, balanceSheetData);
            var quickRatioData = CalculateQuickRatio(dates, period, balanceSheetData);
            var grossProfitMarginData = CalculateGrossProfitMargin(dates, period, incomeStatement);
            var dividendYieldData = CalculateDividendYield(dates, period, outstandingData, cashFlow, eodData);
            var priceToCashFlowData = CalculatePriceToCashFlow(dates, period, outstandingData, cashFlow, eodData);
            var priceToFreeCashFlowData = CalculatePriceToFreeCashFlow(dates, period, outstandingData, cashFlow, eodData);
            var freeCashFlowYieldData = CalculateFreeCashFlowYield(dates, period, outstandingData, cashFlow, eodData);
            var debitToEquityData = CalculateDebtToEquity(dates, period, balanceSheetData);
            var debitToEBITDAData = CalculateDebtToEBITDARatio(dates, period, balanceSheetData, incomeStatement);
            var cashRatioData = CalculateCashRatio(dates, period, balanceSheetData);
            var debitRatioData = CalculateDebtRatio(dates, period, balanceSheetData);
            var operatingProfitMarginData = CalculateOperatingProfitMargin(dates, period, incomeStatement);
            var assetsTurnOverRatioData = CalculateAssetsTurnOverRatio(dates, period, incomeStatement, balanceSheetData);
            var returnOnCapitalEmployedData = CalculateReturnOnCapitalEmployed(dates, period, incomeStatement, balanceSheetData);
            var ebitdaMarginData = CalculateEBITDAMargin(dates, period, incomeStatement);
            var netIncomeMarginData = CalculateNetIncomeMargin(dates, period, incomeStatement);
            var ebitdaInterestExpenseData = CalculateEBITDAInterestExpense(dates, period, incomeStatement);

            finalData = new FinalStockDataModel()
            {
                Columns = dates,
                MarketCap = stockDataHelperService.FormatToCurrency(marketCapData, exchangeCodeModel),
                PERate = stockDataHelperService.ConvertToMultiply(peRatios),
                PBRate = stockDataHelperService.ConvertToMultiply(pbRatios),
                CurrentRatio = stockDataHelperService.ConvertToMultiply(currentRatios),
                PriceToSale = stockDataHelperService.ConvertToMultiply(priceToSales),
                EvEBITDA = stockDataHelperService.ConvertToString(evEBITDAData),
                EvSales = stockDataHelperService.ConvertToString(evSalesData),
                EvOpeningCashFlow = stockDataHelperService.ConvertToString(evOpeningCashFlowData),
                EarningYield = stockDataHelperService.ConvertToPercentage(earningYieldData),
                DebitToAssets = stockDataHelperService.ConvertToMultiply(debitToAssetsData),
                InterestCoverage = stockDataHelperService.ConvertToMultiply(interestCoverageData),
                PayoutRatio = stockDataHelperService.ConvertToPercentage(payoutRatioData),
                ROE = stockDataHelperService.ConvertToPercentage(roeData),
                ROA = stockDataHelperService.ConvertToPercentage(roaData),
                ROIC = stockDataHelperService.ConvertToPercentage(roicData),
                QuickRatio = stockDataHelperService.ConvertToMultiply(quickRatioData),
                GrossProfitMargin = stockDataHelperService.ConvertToPercentage(grossProfitMarginData),
                DividendYield = stockDataHelperService.ConvertToPercentage(dividendYieldData),
                PriceToCashFlow = stockDataHelperService.ConvertToMultiply(priceToCashFlowData),
                PriceToFreeCashFlow = stockDataHelperService.ConvertToMultiply(priceToFreeCashFlowData),
                FreeCashFlowYield = stockDataHelperService.ConvertToPercentage(freeCashFlowYieldData),
                DebitToEquity = stockDataHelperService.ConvertToMultiply(debitToEquityData),
                DebitToEBITDA = stockDataHelperService.ConvertToMultiply(debitToEBITDAData),
                CashRatio = stockDataHelperService.ConvertToMultiply(cashRatioData),
                DebitRatio = stockDataHelperService.ConvertToPercentage(debitRatioData),
                OperatingProfitMargin = stockDataHelperService.ConvertToPercentage(operatingProfitMarginData),
                AssetsTurnOverRatio = stockDataHelperService.ConvertToMultiply(assetsTurnOverRatioData),
                ReturnOnCapitalEmployed = stockDataHelperService.ConvertToPercentage(returnOnCapitalEmployedData),
                EBITDAMargin = stockDataHelperService.ConvertToPercentage(ebitdaMarginData),
                NetIncomeMargin = stockDataHelperService.ConvertToPercentage(netIncomeMarginData),
                EBITDAInterestExpense = stockDataHelperService.ConvertToMultiply(ebitdaInterestExpenseData),
                RowKeys = RowKeys
            };
            await SyncFinalDataAsync(finalData, period, symbolModel.Id);
            return finalData;
        }
        return null;
    }

    /// <summary>
    /// Gets the stock details.
    /// </summary>
    /// <param name="fullSymbolCode">The full symbol code.</param>
    /// <param name="periodType">Type of the period.</param>
    /// <returns>Returns stock details model.</returns>
    private async Task<StockDetailsModel> GetStockDetails(string fullSymbolCode, string periodType)
    {
        var stockDetailsModel = new StockDetailsModel();
        stockDetailsModel.OutstandingShares = (await stockDataHelperService.GetOutstandingSharesAsync(fullSymbolCode, periodType)).OrderByDescending(x => x.DateFormatted).ToList();
        stockDetailsModel.EODData = (await stockDataHelperService.GetEODDataAsync(fullSymbolCode)).OrderByDescending(x => x.Date).ToList();
        stockDetailsModel.EPSModels = (await stockDataHelperService.GetEPSAsync(fullSymbolCode)).OrderByDescending(x => x.Date).ToList();
        stockDetailsModel.BalanceSheets = (await stockDataHelperService.GetBalanceSheetAsync(fullSymbolCode, periodType)).OrderByDescending(x => x.Date).ToList();
        stockDetailsModel.IncomeStatements = (await stockDataHelperService.GetIncomeStatementAsync(fullSymbolCode, periodType)).OrderByDescending(x => x.Date).ToList();
        stockDetailsModel.CashFlows = (await stockDataHelperService.GetCashFlowAsync(fullSymbolCode, periodType)).OrderByDescending(x => x.Date).ToList();
        return stockDetailsModel;
    }

    /// <summary>
    /// Gets the stock data from database.
    /// </summary>
    /// <param name="period">The period.</param>
    /// <param name="exchangeSymbolId">The exchange symbol identifier.</param>
    /// <returns>Returns final stock model.</returns>
    private FinalStockDataModel GetStockDataFromDB(PeriodTypes period, int exchangeSymbolId)
    {
        var finalDataModel = new FinalStockDataModel();
        var properties = finalDataModel.GetType().GetProperties();
        Type modelType = typeof(FinalStockDataModel);
        if (period == PeriodTypes.Quarterly)
        {
            var querterlyData = unitOfWorkAsync.GetGenericRepository<StockQuarterly>()
                .GetQueryable(x => x.ExchangeSymbol == exchangeSymbolId, null).ToList();
            if (querterlyData == null || !querterlyData.Any() || querterlyData.Any(x => ((DateTime)x.CreatedDateTime).AddDays(Constants.SyncDates) <= DateTime.UtcNow))
                return null;

            foreach (var record in querterlyData)
                BindingQuarterlyData(properties, record, finalDataModel, modelType);
            return finalDataModel;
        }
        else
        {

            var annualData = unitOfWorkAsync.GetGenericRepository<StockAnnual>()
                .GetQueryable(x => x.ExchangeSymbol == exchangeSymbolId, null).ToList();
            if (annualData == null || !annualData.Any() || annualData.Any(x => ((DateTime)x.CreatedDateTime).AddDays(Constants.SyncDates) <= DateTime.UtcNow))
                return null;

            foreach (var record in annualData)
                BindingAnnualData(properties, record, finalDataModel, modelType);
            return finalDataModel;
        }
    }

    /// <summary>
    /// Bindings the quarterly data.
    /// </summary>
    /// <param name="properties">The properties.</param>
    /// <param name="record">The record.</param>
    /// <param name="finalDataModel">The final data model.</param>
    /// <param name="modelType">Type of the model.</param>
    private void BindingQuarterlyData(PropertyInfo[] properties, StockQuarterly record, FinalStockDataModel finalDataModel, Type modelType)
    {
        foreach (var property in properties)
        {
            if (property.Name == "Columns" || property.Name == "RowKeys")
                continue;

            var data = (string)record.GetType().GetProperty(property.Name).GetValue(record, null);

            PropertyInfo currentPropInfo = modelType.GetProperty(property.Name);
            var currentData = (Dictionary<string, string>)currentPropInfo.GetValue(finalDataModel, null);
            if (currentData == null)
                currentPropInfo.SetValue(finalDataModel, new Dictionary<string, string> { { record.Date, data } }, null);
            else
            {
                currentData.Add(record.Date, data);
                currentPropInfo.SetValue(finalDataModel, currentData, null);
            }
        }
    }

    /// <summary>
    /// Bindings the annual data.
    /// </summary>
    /// <param name="properties">The properties.</param>
    /// <param name="record">The record.</param>
    /// <param name="finalDataModel">The final data model.</param>
    /// <param name="modelType">Type of the model.</param>
    private void BindingAnnualData(PropertyInfo[] properties, StockAnnual record, FinalStockDataModel finalDataModel, Type modelType)
    {
        foreach (var property in properties)
        {
            if (property.Name == "Columns" || property.Name == "RowKeys")
                continue;

            var data = (string)record.GetType().GetProperty(property.Name).GetValue(record, null);

            PropertyInfo currentPropInfo = modelType.GetProperty(property.Name);
            var currentData = (Dictionary<string, string>)currentPropInfo.GetValue(finalDataModel, null);
            if (currentData == null)
                currentPropInfo.SetValue(finalDataModel, new Dictionary<string, string> { { record.Date, data } }, null);
            else
            {
                if (!currentData.Keys.Contains(record.Date))
                {
                    currentData.Add(record.Date, data);
                    currentPropInfo.SetValue(finalDataModel, currentData, null);
                }
            }
        }
    }

    /// <summary>
    /// Synchronizes the final data asynchronous.
    /// </summary>
    /// <param name="finalStockDataModel">The final stock data model.</param>
    /// <param name="period">The period.</param>
    /// <param name="exchangeSymbolId">The exchange symbol identifier.</param>
    private async Task SyncFinalDataAsync(FinalStockDataModel finalStockDataModel, PeriodTypes period, int exchangeSymbolId)
    {
        RemoveCurrentRecords(period, exchangeSymbolId);
        var properties = finalStockDataModel.GetType().GetProperties();
        foreach (var date in finalStockDataModel.MarketCap.Keys)
        {
            object stockClass;
            if (period == PeriodTypes.Quarterly)
                stockClass = new StockQuarterly() { Date = date, ExchangeSymbol = exchangeSymbolId, CreatedDateTime = DateTime.UtcNow };
            else
                stockClass = new StockAnnual() { Date = date, ExchangeSymbol = exchangeSymbolId, CreatedDateTime = DateTime.UtcNow };
            Type modelType;
            if (period == PeriodTypes.Quarterly)
                modelType = typeof(StockQuarterly);
            else
                modelType = typeof(StockAnnual);
            foreach (var property in properties)
            {
                if (property.Name == "Columns" || property.Name == "RowKeys")
                    continue;

                var field = (Dictionary<string, string>)finalStockDataModel.GetType().GetProperty(property.Name).GetValue(finalStockDataModel, null);
                var value = field[date];

                PropertyInfo currentPropInfo = modelType.GetProperty(property.Name);
                currentPropInfo.SetValue(stockClass, value, null);
            }
            if (period == PeriodTypes.Quarterly)
                await unitOfWorkAsync.GetGenericRepository<StockQuarterly>().Add((StockQuarterly)stockClass);
            else
                await unitOfWorkAsync.GetGenericRepository<StockAnnual>().Add((StockAnnual)stockClass);
        }
        unitOfWorkAsync.SaveChanges();
    }

    /// <summary>
    /// Removes the current records.
    /// </summary>
    /// <param name="period">The period.</param>
    /// <param name="exchangeSymbolId">The exchange symbol identifier.</param>
    public void RemoveCurrentRecords(PeriodTypes period, int exchangeSymbolId)
    {
        if (period == PeriodTypes.Quarterly)
        {
            var querterlyData = unitOfWorkAsync.GetGenericRepository<StockQuarterly>()
                .GetQueryable(x => x.ExchangeSymbol == exchangeSymbolId, null).ToList();
            if (querterlyData == null || !querterlyData.Any() || querterlyData.Any(x => ((DateTime)x.CreatedDateTime).AddDays(Constants.SyncDates) <= DateTime.UtcNow))
                return;

            foreach (var record in querterlyData)
                unitOfWorkAsync.GetGenericRepository<StockQuarterly>().Delete(record.Id);
        }
        else
        {
            var annualData = unitOfWorkAsync.GetGenericRepository<StockAnnual>()
                .GetQueryable(x => x.ExchangeSymbol == exchangeSymbolId, null).ToList();
            if (annualData == null || !annualData.Any() || annualData.Any(x => ((DateTime)x.CreatedDateTime).AddDays(Constants.SyncDates) <= DateTime.UtcNow))
                return;

            foreach (var record in annualData)
                unitOfWorkAsync.GetGenericRepository<StockQuarterly>().Delete(record.Id);
        }
        unitOfWorkAsync.SaveChanges();
    }

    #region calculations    
    /// <summary>
    /// Calculates the market cap.
    /// </summary>
    /// <param name="outstandingSharesModels">The outstanding shares models.</param>
    /// <param name="eodDataModels">The eod data models.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns calculate market cap.</returns>
    private Dictionary<string, double> CalculateMarketCap(List<OutstandingSharesModel> outstandingSharesModels, List<EODDataModel> eodDataModels, PeriodTypes period)
    {
        var marketCapData = new Dictionary<string, double>();
        foreach (var model in outstandingSharesModels)
        {
            var eodData = stockDataHelperService.GetEODData(eodDataModels, model.DateFormatted, period);
            if (eodData != null && model.Shares != null && eodData.Adjusted_Close != null)
                marketCapData.Add(model.DateFormatted, (double)(model.Shares * eodData.Adjusted_Close));
            else
                marketCapData.Add(model.DateFormatted, 0);
        }
        return marketCapData;
    }

    /// <summary>
    /// Calculates the pe ratio.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="eodDataModels">The eod data models.</param>
    /// <param name="epsDataModels">The eps data models.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns calculate pe ratio.</returns>
    private Dictionary<string, double> CalculatePERatio(List<string> dates, List<EODDataModel> eodDataModels, List<EPSModel> epsDataModels, PeriodTypes period)
    {
        var peRatioData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            var eodData = stockDataHelperService.GetEODData(eodDataModels, date, period);
            if (eodData != null && eodData.Adjusted_Close != null)
            {
                double? epsValue = stockDataHelperService.GetTotalEPSPerYearData(epsDataModels, date, period);
                if (epsValue != null)
                {
                    peRatioData.Add(date, (double)(eodData.Adjusted_Close / epsValue));
                    continue;
                }
            }
            peRatioData.Add(date, 0);
        }
        return peRatioData;
    }

    /// <summary>
    /// Calculates the pb ratio.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="balanceSheetDataModels">The balance sheet data models.</param>
    /// <param name="eodDataModels">The eod data models.</param>
    /// <param name="outstandingDataModels">The outstanding data models.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns calculate pb ratio.</returns>
    private Dictionary<string, double> CalculatePBRatio(List<string> dates, List<BalanceSheetModel> balanceSheetDataModels,
        List<EODDataModel> eodDataModels, List<OutstandingSharesModel> outstandingDataModels, PeriodTypes period)
    {
        var pbRatioData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            var eodData = stockDataHelperService.GetEODData(eodDataModels, date, period);
            if (eodData != null && eodData.Adjusted_Close != null)
            {
                BalanceSheetModel balanceSheetData = stockDataHelperService.GetBalanceSheetData(balanceSheetDataModels, date, period);
                OutstandingSharesModel outstandingShareData =
                    stockDataHelperService.GetOutstandingSharesData(outstandingDataModels, date, period);

                if (balanceSheetData != null && outstandingShareData != null &&
                    balanceSheetData.TotalAssets != null && balanceSheetData.TotalLiab != null &&
                    outstandingShareData.Shares != null)
                {
                    var bookValue = (double)(balanceSheetData.TotalAssets - balanceSheetData.TotalLiab);
                    var bookValuePerShare = bookValue / (double)outstandingShareData.Shares;
                    pbRatioData.Add(date, (double)(eodData.Adjusted_Close / bookValuePerShare));
                    continue;
                }
            }
            pbRatioData.Add(date, 0);
        }
        return pbRatioData;
    }

    /// <summary>
    /// Calculates the current ratio.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="balanceSheetDataModels">The balance sheet data models.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns calculate current ratio.</returns>
    private Dictionary<string, double> CalculateCurrentRatio(List<string> dates,
        List<BalanceSheetModel> balanceSheetDataModels, PeriodTypes period)
    {
        var currentRatioData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            BalanceSheetModel balanceSheetData = stockDataHelperService.GetBalanceSheetData(balanceSheetDataModels, date, period);
            if (balanceSheetData != null && balanceSheetData.TotalCurrentAssets != null &&
                balanceSheetData.TotalCurrentLiabilities != null)
            {
                currentRatioData.Add(date,
                    (double)(balanceSheetData.TotalCurrentAssets / balanceSheetData.TotalCurrentLiabilities)
                );
                continue;
            }
            currentRatioData.Add(date, 0);
        }
        return currentRatioData;
    }

    /// <summary>
    /// Calculates the price to sales.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="incomeStatementModels">The income statement models.</param>
    /// <param name="outstandingDataModels">The outstanding data models.</param>
    /// <param name="eodDataModels">The eod data models.</param>
    /// <param name="epsDataModels">The eps data models.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns calculate price to sales.</returns>
    private Dictionary<string, double> CalculatePriceToSales(List<string> dates, List<IncomeStatementModel> incomeStatementModels,
        List<OutstandingSharesModel> outstandingDataModels, List<EODDataModel> eodDataModels, List<EPSModel> epsDataModels, PeriodTypes period)
    {
        var priceToSalesData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            var eodData = stockDataHelperService.GetEODData(eodDataModels, date, period);
            if (eodData != null && eodData.Adjusted_Close != null)
            {
                //IncomeStatementModel incomeStatementData = stockDataHelperService.GetIncomeStatement(incomeStatementModels,
                //    date, period);
                OutstandingSharesModel outstandingShareData = stockDataHelperService.GetOutstandingSharesData(outstandingDataModels,
                    date, period);
                var totalRevenue = stockDataHelperService.GetTotalRevenuePerYearData(incomeStatementModels, date, period);
                if (totalRevenue != null && outstandingShareData != null 
                    && outstandingShareData.Shares != null)
                {
                    priceToSalesData.Add(date, (double)(
                        eodData.Adjusted_Close / (totalRevenue / outstandingShareData.Shares))
                    );
                    continue;
                }
            }
            priceToSalesData.Add(date, 0);
        }
        return priceToSalesData;
    }

    /// <summary>
    /// Calculates the ev ebitda.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="marketCapData">The market cap data.</param>
    /// <param name="balanceSheetDataModels">The balance sheet data models.</param>
    /// <param name="incomeStatementModels">The income statement models.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns calculate evebitda.</returns>
    private Dictionary<string, double> CalculateEvEBITDA(List<string> dates, Dictionary<string, double> marketCapData,
        List<BalanceSheetModel> balanceSheetDataModels, List<IncomeStatementModel> incomeStatementModels, PeriodTypes period)
    {
        var evEBITDAData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            //IncomeStatementModel incomeStatementData = stockDataHelperService.GetIncomeStatement(incomeStatementModels, date, period);
            var totalEbitda = stockDataHelperService.GetTotalEbitdaPerYearData(incomeStatementModels, date, period);
            BalanceSheetModel balanceSheetData = stockDataHelperService.GetBalanceSheetData(balanceSheetDataModels, date, period);
            var marketCap = marketCapData.FirstOrDefault(cap => cap.Key == date);
            if (balanceSheetData != null && balanceSheetData.LongTermDebt != null && balanceSheetData.ShortTermDebt != null &&
                balanceSheetData.Cash != null && totalEbitda != null)
            {
                var ev = (double)(marketCap.Value + (balanceSheetData.LongTermDebt + balanceSheetData.ShortTermDebt)
                    - balanceSheetData.Cash);

                evEBITDAData.Add(date, (double)(ev / totalEbitda));
                continue;
            }
            evEBITDAData.Add(date, 0);
        }
        return evEBITDAData;
    }

    /// <summary>
    /// Calculates the ev sales.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="marketCapData">The market cap data.</param>
    /// <param name="balanceSheetDataModels">The balance sheet data models.</param>
    /// <param name="incomeStatementModels">The income statement models.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns calculate ev sales.</returns>
    private Dictionary<string, double> CalculateEvSales(List<string> dates, Dictionary<string, double> marketCapData,
        List<BalanceSheetModel> balanceSheetDataModels, List<IncomeStatementModel> incomeStatementModels, PeriodTypes period)
    {
        var evSalesData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            //IncomeStatementModel incomeStatementData = stockDataHelperService.GetIncomeStatement(incomeStatementModels, date, period);
            var totalRevenue = stockDataHelperService.GetTotalRevenuePerYearData(incomeStatementModels, date, period);
            BalanceSheetModel balanceSheetData = stockDataHelperService.GetBalanceSheetData(balanceSheetDataModels, date, period);
            var marketCap = marketCapData.FirstOrDefault(cap => cap.Key == date);

            if (balanceSheetData != null && balanceSheetData.LongTermDebt != null && balanceSheetData.ShortTermDebt != null &&
                balanceSheetData.Cash != null && totalRevenue != null)
            {
                var ev = (double)(marketCap.Value + (balanceSheetData.LongTermDebt + balanceSheetData.ShortTermDebt)
                    - balanceSheetData.Cash);
                evSalesData.Add(date, (double)(ev / totalRevenue));
                continue;
            }
            evSalesData.Add(date, 0);
        }
        return evSalesData;
    }

    /// <summary>
    /// Calculates the ev opening cash flow.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="marketCapData">The market cap data.</param>
    /// <param name="balanceSheetDataModels">The balance sheet data models.</param>
    /// <param name="cashFlowModels">The cash flow models.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns calculate ev opening cash flow</returns>
    private Dictionary<string, double> CalculateEvOpeningCashFlow(List<string> dates, Dictionary<string, double> marketCapData,
        List<BalanceSheetModel> balanceSheetDataModels, List<CashFlowModel> cashFlowModels, PeriodTypes period)
    {
        var evCFOData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            //CashFlowModel cashFlowData = stockDataHelperService.GetCashFlow(cashFlowModels, date, period);
            var totalCashFromOperatingActivites = stockDataHelperService.GetTotalCashFromOperatingActivitiesPerYearData(cashFlowModels, date, period);
            BalanceSheetModel balanceSheetData = stockDataHelperService.GetBalanceSheetData(balanceSheetDataModels, date, period);
            var marketCap = marketCapData.FirstOrDefault(cap => cap.Key == date);

            if (balanceSheetData != null && balanceSheetData.LongTermDebt != null && balanceSheetData.ShortTermDebt != null &&
                balanceSheetData.Cash != null && totalCashFromOperatingActivites != null)
            {
                var ev = (double)(marketCap.Value + (balanceSheetData.LongTermDebt + balanceSheetData.ShortTermDebt)
                    - balanceSheetData.Cash);
                evCFOData.Add(date, (double)(ev / totalCashFromOperatingActivites));
                continue;
            }
            evCFOData.Add(date, 0);
        }
        return evCFOData;
    }

    /// <summary>
    /// Calculates the earning yield.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="epsModels">The eps models.</param>
    /// <param name="eodModels">The eod models.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns calculate earning yield</returns>
    private Dictionary<string, double> CalculateEarningYield(List<string> dates,
        List<EPSModel> epsModels, List<EODDataModel> eodModels, PeriodTypes period)
    {
        var earningYieldData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            EPSModel epsData = stockDataHelperService.GetEPSData(epsModels, date, period);
            EODDataModel eodData = stockDataHelperService.GetEODData(eodModels, date, period);

            if (epsData != null && epsData.EpsActual != null && eodData != null && eodData.Adjusted_Close != null)
            {
                earningYieldData.Add(date, (double)(epsData.EpsActual / eodData.Adjusted_Close));
                continue;
            }
            earningYieldData.Add(date, 0);
        }
        return earningYieldData;
    }

    /// <summary>
    /// Calculates the debit to assets.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="balanceSheetModels">The balance sheet models.</param>
    /// <returns>Returns calculate debit to assets</returns>
    private Dictionary<string, double> CalculateDebitToAssets(List<string> dates, PeriodTypes period, List<BalanceSheetModel> balanceSheetModels)
    {
        var debitToAssetsData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            BalanceSheetModel balanceSheetData = stockDataHelperService.GetBalanceSheetData(balanceSheetModels, date, period);
            if (balanceSheetData != null && balanceSheetData.LongTermDebt != null && balanceSheetData.ShortTermDebt != null &&
                balanceSheetData.TotalAssets != null)
            {
                debitToAssetsData.Add(date, (double)((balanceSheetData.LongTermDebt + balanceSheetData.ShortTermDebt) / balanceSheetData.TotalAssets));
                continue;
            }
            debitToAssetsData.Add(date, 0);
        }
        return debitToAssetsData;
    }

    /// <summary>
    /// Calculates the interest coverage ratio.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="incomeStatementModels">The income statement models.</param>
    /// <returns>Returns calculate interest coverage ratio.</returns>
    private Dictionary<string, double> CalculateInterestCoverageRatio(List<string> dates, PeriodTypes period, List<IncomeStatementModel> incomeStatementModels)
    {
        var interestCoverageRatioData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            IncomeStatementModel incomeStatementData = stockDataHelperService.GetIncomeStatement(incomeStatementModels, date, period);
            if (incomeStatementData != null && incomeStatementData.Ebit != null && incomeStatementData.InterestExpense != null)
            {
                interestCoverageRatioData.Add(date, (double)(incomeStatementData.Ebit / incomeStatementData.InterestExpense));
                continue;
            }
            interestCoverageRatioData.Add(date, 0);
        }
        return interestCoverageRatioData;
    }

    /// <summary>
    /// Calculates the payout ratio.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="incomeStatementModels">The income statement models.</param>
    /// <param name="cashFlowModels">The cash flow models.</param>
    /// <returns>Returns calculate payout ratio</returns>
    private Dictionary<string, double> CalculatePayoutRatio(List<string> dates, PeriodTypes period,
        List<IncomeStatementModel> incomeStatementModels, List<CashFlowModel> cashFlowModels)
    {
        var payoutRatioData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            IncomeStatementModel incomeStatementData = stockDataHelperService.GetIncomeStatement(incomeStatementModels, date, period);
            CashFlowModel cashFlowData = stockDataHelperService.GetCashFlow(cashFlowModels, date, period);
            if (incomeStatementData != null && cashFlowData != null && cashFlowData.DividendsPaid != null && incomeStatementData.NetIncome != null)
            {
                payoutRatioData.Add(date, ((double)(cashFlowData.DividendsPaid / incomeStatementData.NetIncome))*100);
                continue;
            }
            payoutRatioData.Add(date, 0);
        }
        return payoutRatioData;
    }

    /// <summary>
    /// Calculates the roe.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="incomeStatementModels">The income statement models.</param>
    /// <param name="balanceSheetModels">The balance sheet models.</param>
    /// <returns>Returns calculate roe.</returns>
    private Dictionary<string, double> CalculateROE(List<string> dates, PeriodTypes period, List<IncomeStatementModel> incomeStatementModels,
        List<BalanceSheetModel> balanceSheetModels)
    {
        var roeData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            IncomeStatementModel incomeStatementData = stockDataHelperService.GetIncomeStatement(incomeStatementModels, date, period);
            BalanceSheetModel balanceSheetData = stockDataHelperService.GetBalanceSheetData(balanceSheetModels, date, period);
            if (incomeStatementData != null && balanceSheetData != null && incomeStatementData.NetIncome != null &&
                balanceSheetData.TotalStockholderEquity != null)
            {
                roeData.Add(date, ((double)(incomeStatementData.NetIncome / balanceSheetData.TotalStockholderEquity)) * 100);
                continue;
            }
            roeData.Add(date, 0);
        }
        return roeData;
    }

    /// <summary>
    /// Calculates the roa.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="incomeStatementModels">The income statement models.</param>
    /// <param name="balanceSheetModels">The balance sheet models.</param>
    /// <returns>Returns calculate roa</returns>
    private Dictionary<string, double> CalculateROA(List<string> dates, PeriodTypes period, List<IncomeStatementModel> incomeStatementModels,
        List<BalanceSheetModel> balanceSheetModels)
    {
        var roaData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            IncomeStatementModel incomeStatementData = stockDataHelperService.GetIncomeStatement(incomeStatementModels, date, period);
            BalanceSheetModel balanceSheetData = stockDataHelperService.GetBalanceSheetData(balanceSheetModels, date, period);
            if (incomeStatementData != null && balanceSheetData != null && incomeStatementData.NetIncome != null &&
                balanceSheetData.TotalAssets != null)
            {
                roaData.Add(date, ((double)(incomeStatementData.NetIncome / balanceSheetData.TotalAssets)) * 100);
                continue;
            }
            roaData.Add(date, 0);
        }
        return roaData;
    }

    /// <summary>
    /// Calculates the roic.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="incomeStatementModels">The income statement models.</param>
    /// <param name="balanceSheetModels">The balance sheet models.</param>
    /// <returns>Returns calculate roic</returns>
    private Dictionary<string, double> CalculateROIC(List<string> dates, PeriodTypes period, List<IncomeStatementModel> incomeStatementModels,
        List<BalanceSheetModel> balanceSheetModels)
    {
        var roicData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            IncomeStatementModel incomeStatementData = stockDataHelperService.GetIncomeStatement(incomeStatementModels, date, period);
            BalanceSheetModel balanceSheetData = stockDataHelperService.GetBalanceSheetData(balanceSheetModels, date, period);
            if (incomeStatementData != null && balanceSheetData != null && incomeStatementData.Ebit != null &&
                balanceSheetData.TotalAssets != null && balanceSheetData.TotalCurrentAssets != null)
            {
                roicData.Add(date, ((double)(incomeStatementData.Ebit / (balanceSheetData.TotalAssets - balanceSheetData.TotalCurrentAssets))) * 100);
                continue;
            }
            roicData.Add(date, 0);
        }
        return roicData;
    }

    /// <summary>
    /// Calculates the quick ratio.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="balanceSheetModels">The balance sheet models.</param>
    /// <returns>Returns calculate quick ratio</returns>
    private Dictionary<string, double> CalculateQuickRatio(List<string> dates, PeriodTypes period, List<BalanceSheetModel> balanceSheetModels)
    {
        var quickRatioData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            BalanceSheetModel balanceSheetData = stockDataHelperService.GetBalanceSheetData(balanceSheetModels, date, period);
            if (balanceSheetData != null && balanceSheetData.Cash != null && balanceSheetData.ShortTermInvestments != null &&
                balanceSheetData.NetReceivables != null && balanceSheetData.TotalCurrentLiabilities != null)
            {
                quickRatioData.Add(date, (double)(
                    (balanceSheetData.Cash + balanceSheetData.ShortTermInvestments + balanceSheetData.NetReceivables)
                    / balanceSheetData.TotalCurrentLiabilities));
                continue;
            }
            quickRatioData.Add(date, 0);
        }
        return quickRatioData;
    }

    /// <summary>
    /// Calculates the gross profit margin.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="incomeStatementModels">The income statement models.</param>
    /// <returns>Returns calculate gross profit margin</returns>
    private Dictionary<string, double> CalculateGrossProfitMargin(List<string> dates, PeriodTypes period, List<IncomeStatementModel> incomeStatementModels)
    {
        var grossProfitMarginData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            IncomeStatementModel incomeStatementData = stockDataHelperService.GetIncomeStatement(incomeStatementModels, date, period);
            if (incomeStatementData != null && incomeStatementData.GrossProfit != null && incomeStatementData.TotalRevenue != null)
            {
                grossProfitMarginData.Add(date, ((double)(incomeStatementData.GrossProfit / incomeStatementData.TotalRevenue))*100);
                continue;
            }
            grossProfitMarginData.Add(date, 0);
        }
        return grossProfitMarginData;
    }

    /// <summary>
    /// Calculates the dividend yield.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="outstandingSharesModels">The outstanding shares models.</param>
    /// <param name="cashFlowModels">The cash flow models.</param>
    /// <param name="eODDataModels">The e od data models.</param>
    /// <returns>Returns calculate dividend yield</returns>
    private Dictionary<string, double> CalculateDividendYield(List<string> dates, PeriodTypes period, List<OutstandingSharesModel> outstandingSharesModels,
        List<CashFlowModel> cashFlowModels, List<EODDataModel> eODDataModels)
    {
        var dividendYieldData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            OutstandingSharesModel outstandingSharesData = stockDataHelperService.GetOutstandingSharesData(outstandingSharesModels, date, period);
            CashFlowModel cashFlowData = stockDataHelperService.GetCashFlow(cashFlowModels, date, period);
            EODDataModel eodData = stockDataHelperService.GetEODData(eODDataModels, date, period);
            if (outstandingSharesData != null && cashFlowData != null && cashFlowData.DividendsPaid != null &&
                outstandingSharesData.Shares != null && eodData != null && eodData.Adjusted_Close != null)
            {
                dividendYieldData.Add(date, ((double)((cashFlowData.DividendsPaid / outstandingSharesData.Shares) / eodData.Adjusted_Close))*100);
                continue;
            }
            dividendYieldData.Add(date, 0);
        }
        return dividendYieldData;
    }

    /// <summary>
    /// Calculates the price to cash flow.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="outstandingSharesModels">The outstanding shares models.</param>
    /// <param name="cashFlowModels">The cash flow models.</param>
    /// <param name="eODDataModels">The e od data models.</param>
    /// <returns>Returns calculate price to cash flow</returns>
    private Dictionary<string, double> CalculatePriceToCashFlow(List<string> dates, PeriodTypes period, List<OutstandingSharesModel> outstandingSharesModels,
        List<CashFlowModel> cashFlowModels, List<EODDataModel> eODDataModels)
    {
        var priceToCashFlowData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            OutstandingSharesModel outstandingSharesData = stockDataHelperService.GetOutstandingSharesData(outstandingSharesModels, date, period);
            CashFlowModel cashFlowData = stockDataHelperService.GetCashFlow(cashFlowModels, date, period);
            EODDataModel eodData = stockDataHelperService.GetEODData(eODDataModels, date, period);
            if (outstandingSharesData != null && cashFlowData != null && cashFlowData.TotalCashFromOperatingActivities != null &&
                outstandingSharesData.Shares != null && eodData != null && eodData.Adjusted_Close != null)
            {
                priceToCashFlowData.Add(date, (double)(eodData.Adjusted_Close / (cashFlowData.TotalCashFromOperatingActivities / outstandingSharesData.Shares)));
                continue;
            }
            priceToCashFlowData.Add(date, 0);
        }
        return priceToCashFlowData;
    }

    /// <summary>
    /// Calculates the price to free cash flow.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="outstandingSharesModels">The outstanding shares models.</param>
    /// <param name="cashFlowModels">The cash flow models.</param>
    /// <param name="eODDataModels">The e od data models.</param>
    /// <returns>Returns calculate price to free cash flow</returns>
    private Dictionary<string, double> CalculatePriceToFreeCashFlow(List<string> dates, PeriodTypes period, List<OutstandingSharesModel> outstandingSharesModels,
        List<CashFlowModel> cashFlowModels, List<EODDataModel> eODDataModels)
    {
        var priceToFreeCashFlow = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            OutstandingSharesModel outstandingSharesData = stockDataHelperService.GetOutstandingSharesData(outstandingSharesModels, date, period);
            CashFlowModel cashFlowData = stockDataHelperService.GetCashFlow(cashFlowModels, date, period);
            EODDataModel eodData = stockDataHelperService.GetEODData(eODDataModels, date, period);
            if (outstandingSharesData != null && cashFlowData != null && cashFlowData.FreeCashFlow != null &&
                outstandingSharesData.Shares != null && eodData != null && eodData.Adjusted_Close != null)
            {
                priceToFreeCashFlow.Add(date, (double)((eodData.Adjusted_Close * outstandingSharesData.Shares) / cashFlowData.FreeCashFlow));
                continue;
            }
            priceToFreeCashFlow.Add(date, 0);
        }
        return priceToFreeCashFlow;
    }

    /// <summary>
    /// Calculates the free cash flow yield.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="outstandingSharesModels">The outstanding shares models.</param>
    /// <param name="cashFlowModels">The cash flow models.</param>
    /// <param name="eODDataModels">The e od data models.</param>
    /// <returns>Returns calculate free cash flow yield</returns>
    private Dictionary<string, double> CalculateFreeCashFlowYield(List<string> dates, PeriodTypes period, List<OutstandingSharesModel> outstandingSharesModels,
        List<CashFlowModel> cashFlowModels, List<EODDataModel> eODDataModels)
    {
        var freeCashFlowYield = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            OutstandingSharesModel outstandingSharesData = stockDataHelperService.GetOutstandingSharesData(outstandingSharesModels, date, period);
            CashFlowModel cashFlowData = stockDataHelperService.GetCashFlow(cashFlowModels, date, period);
            EODDataModel eodData = stockDataHelperService.GetEODData(eODDataModels, date, period);
            if (outstandingSharesData != null && cashFlowData != null && cashFlowData.FreeCashFlow != null &&
                outstandingSharesData.Shares != null && eodData != null && eodData.Adjusted_Close != null)
            {
                freeCashFlowYield.Add(date, (double)(cashFlowData.FreeCashFlow / (eodData.Adjusted_Close * outstandingSharesData.Shares)));
                continue;
            }
            freeCashFlowYield.Add(date, 0);
        }
        return freeCashFlowYield;
    }

    /// <summary>
    /// Calculates the debt to equity.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="balanceSheetModels">The balance sheet models.</param>
    /// <returns>Returns calculate debit to equity.</returns>
    private Dictionary<string, double> CalculateDebtToEquity(List<string> dates, PeriodTypes period, List<BalanceSheetModel> balanceSheetModels)
    {
        var debtToEquity = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            BalanceSheetModel balanceSheetData = stockDataHelperService.GetBalanceSheetData(balanceSheetModels, date, period);
            if (balanceSheetData != null && balanceSheetData.LongTermDebt != null && balanceSheetData.ShortTermDebt != null &&
                balanceSheetData.TotalStockholderEquity != null)
            {
                debtToEquity.Add(date, (double)((balanceSheetData.LongTermDebt + balanceSheetData.ShortTermDebt) / balanceSheetData.TotalStockholderEquity));
                continue;
            }
            debtToEquity.Add(date, 0);
        }
        return debtToEquity;
    }

    /// <summary>
    /// Calculates the debt to ebitda ratio.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="balanceSheetModels">The balance sheet models.</param>
    /// <param name="incomeStatementModels">The income statement models.</param>
    /// <returns>Returns calculate debit to ebitda ratio.</returns>
    private Dictionary<string, double> CalculateDebtToEBITDARatio(List<string> dates, PeriodTypes period, List<BalanceSheetModel> balanceSheetModels,
        List<IncomeStatementModel> incomeStatementModels)
    {
        var debitToEBITDARatioData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            BalanceSheetModel balanceSheetData = stockDataHelperService.GetBalanceSheetData(balanceSheetModels, date, period);
            IncomeStatementModel incomeStatementData = stockDataHelperService.GetIncomeStatement(incomeStatementModels, date, period);
            if (balanceSheetData != null && incomeStatementData != null && balanceSheetData.NetDebt != null &&
                incomeStatementData.Ebitda != null)
            {
                debitToEBITDARatioData.Add(date, (double)(balanceSheetData.NetDebt / incomeStatementData.Ebitda));
                continue;
            }
            debitToEBITDARatioData.Add(date, 0);
        }
        return debitToEBITDARatioData;
    }

    /// <summary>
    /// Calculates the cash ratio.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="balanceSheetModels">The balance sheet models.</param>
    /// <returns>Returns calculate cash ratio.</returns>
    private Dictionary<string, double> CalculateCashRatio(List<string> dates, PeriodTypes period, List<BalanceSheetModel> balanceSheetModels)
    {
        var cashRatioData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            BalanceSheetModel balanceSheetData = stockDataHelperService.GetBalanceSheetData(balanceSheetModels, date, period);
            if (balanceSheetData != null && balanceSheetData.Cash != null && balanceSheetData.TotalCurrentLiabilities != null)
            {
                cashRatioData.Add(date, (double)(balanceSheetData.Cash / balanceSheetData.TotalCurrentLiabilities));
                continue;
            }
            cashRatioData.Add(date, 0);
        }
        return cashRatioData;
    }

    /// <summary>
    /// Calculates the debt ratio.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="balanceSheetModels">The balance sheet models.</param>
    /// <returns>Returns calculate debit ratio.</returns>
    private Dictionary<string, double> CalculateDebtRatio(List<string> dates, PeriodTypes period, List<BalanceSheetModel> balanceSheetModels)
    {
        var debtRatio = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            BalanceSheetModel balanceSheetData = stockDataHelperService.GetBalanceSheetData(balanceSheetModels, date, period);
            if (balanceSheetData != null && balanceSheetData.TotalLiab != null && balanceSheetData.TotalAssets != null)
            {
                debtRatio.Add(date, (double)(balanceSheetData.TotalLiab / balanceSheetData.TotalAssets));
                continue;
            }
            debtRatio.Add(date, 0);
        }
        return debtRatio;
    }

    /// <summary>
    /// Calculates the operating profit margin.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="incomeStatementModels">The income statement models.</param>
    /// <returns>Returns calculate operating profit margin.</returns>
    private Dictionary<string, double> CalculateOperatingProfitMargin(List<string> dates, PeriodTypes period, List<IncomeStatementModel> incomeStatementModels)
    {
        var operatingProfileMarginData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            IncomeStatementModel incomeStatementData = stockDataHelperService.GetIncomeStatement(incomeStatementModels, date, period);
            if (incomeStatementData != null && incomeStatementData.TotalRevenue != null && incomeStatementData.OperatingIncome != null)
            {
                operatingProfileMarginData.Add(date, ((double)(incomeStatementData.OperatingIncome / incomeStatementData.TotalRevenue))*100);
                continue;
            }
            operatingProfileMarginData.Add(date, 0);
        }
        return operatingProfileMarginData;
    }

    /// <summary>
    /// Calculates the assets turn over ratio.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="incomeStatementModels">The income statement models.</param>
    /// <param name="balanceSheetModels">The balance sheet models.</param>
    /// <returns>Returns calculate assets turn over ratio.</returns>
    private Dictionary<string, double> CalculateAssetsTurnOverRatio(List<string> dates, PeriodTypes period, List<IncomeStatementModel> incomeStatementModels,
        List<BalanceSheetModel> balanceSheetModels)
    {
        var assetsTurnOverRatioData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            IncomeStatementModel incomeStatementData = stockDataHelperService.GetIncomeStatement(incomeStatementModels, date, period);
            BalanceSheetModel balanceSheetData = stockDataHelperService.GetBalanceSheetData(balanceSheetModels, date, period);
            if (incomeStatementData != null && incomeStatementData.TotalRevenue != null && balanceSheetData != null
                && balanceSheetData.TotalAssets != null)
            {
                assetsTurnOverRatioData.Add(date, (double)(incomeStatementData.TotalRevenue / balanceSheetData.TotalAssets));
                continue;
            }
            assetsTurnOverRatioData.Add(date, 0);
        }
        return assetsTurnOverRatioData;
    }

    /// <summary>
    /// Calculates the return on capital employed.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="incomeStatementModels">The income statement models.</param>
    /// <param name="balanceSheetModels">The balance sheet models.</param>
    /// <returns>Returns calculate return on capital employed</returns>
    private Dictionary<string, double> CalculateReturnOnCapitalEmployed(List<string> dates, PeriodTypes period, List<IncomeStatementModel> incomeStatementModels,
        List<BalanceSheetModel> balanceSheetModels)
    {
        var returnOnCapitalEmployedData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            IncomeStatementModel incomeStatementData = stockDataHelperService.GetIncomeStatement(incomeStatementModels, date, period);
            BalanceSheetModel balanceSheetData = stockDataHelperService.GetBalanceSheetData(balanceSheetModels, date, period);
            if (incomeStatementData != null && incomeStatementData.Ebit != null && balanceSheetData != null &&
                balanceSheetData.TotalAssets != null && balanceSheetData.TotalCurrentLiabilities != null)
            {
                returnOnCapitalEmployedData.Add(date, (double)(incomeStatementData.Ebit / (balanceSheetData.TotalAssets - balanceSheetData.TotalCurrentLiabilities)));
                continue;
            }
            returnOnCapitalEmployedData.Add(date, 0);
        }
        return returnOnCapitalEmployedData;
    }

    /// <summary>
    /// Calculates the ebitda margin.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="incomeStatementModels">The income statement models.</param>
    /// <returns>Returns calculate ebitda margin.</returns>
    private Dictionary<string, double> CalculateEBITDAMargin(List<string> dates, PeriodTypes period, List<IncomeStatementModel> incomeStatementModels)
    {
        var ebitdaMarginData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            IncomeStatementModel incomeStatementData = stockDataHelperService.GetIncomeStatement(incomeStatementModels, date, period);
            if (incomeStatementData != null && incomeStatementData.Ebitda != null && incomeStatementData.TotalRevenue != null)
            {
                ebitdaMarginData.Add(date, ((double)(incomeStatementData.Ebitda / incomeStatementData.TotalRevenue))*100);
                continue;
            }
            ebitdaMarginData.Add(date, 0);
        }
        return ebitdaMarginData;
    }

    /// <summary>
    /// Calculates the net income margin.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="incomeStatementModels">The income statement models.</param>
    /// <returns>Returns calculate net income margin.</returns>
    private Dictionary<string, double> CalculateNetIncomeMargin(List<string> dates, PeriodTypes period, List<IncomeStatementModel> incomeStatementModels)
    {
        var netIncomeData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            IncomeStatementModel incomeStatementData = stockDataHelperService.GetIncomeStatement(incomeStatementModels, date, period);
            if (incomeStatementData != null && incomeStatementData.GrossProfit != null && incomeStatementData.TotalRevenue != null)
            {
                netIncomeData.Add(date, (double)((incomeStatementData.GrossProfit / incomeStatementData.TotalRevenue) * 100));
                continue;
            }
            netIncomeData.Add(date, 0);
        }
        return netIncomeData;
    }

    /// <summary>
    /// Calculates the ebitda interest expense.
    /// </summary>
    /// <param name="dates">The dates.</param>
    /// <param name="period">The period.</param>
    /// <param name="incomeStatementModels">The income statement models.</param>
    /// <returns>Returns calculate ebitda interest expenses.</returns>
    private Dictionary<string, double> CalculateEBITDAInterestExpense(List<string> dates, PeriodTypes period, List<IncomeStatementModel> incomeStatementModels)
    {
        var ebitdaInterestExpenseData = new Dictionary<string, double>();
        foreach (var date in dates)
        {
            IncomeStatementModel incomeStatementData = stockDataHelperService.GetIncomeStatement(incomeStatementModels, date, period);
            if (incomeStatementData != null && incomeStatementData.InterestExpense != null && incomeStatementData.Ebitda != null)
            {
                ebitdaInterestExpenseData.Add(date, (double)((incomeStatementData.Ebitda / incomeStatementData.InterestExpense) * 100));
                continue;
            }
            ebitdaInterestExpenseData.Add(date, 0);
        }
        return ebitdaInterestExpenseData;
    }
    #endregion
}