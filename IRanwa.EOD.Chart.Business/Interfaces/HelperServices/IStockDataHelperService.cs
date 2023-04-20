using IRanwa.EOD.Chart.Core;
using IRanwa.EOD.Chart.Data;
using IRanwa.EOD.Chart.Model;

namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// Stock data helper service.
/// </summary>
public interface IStockDataHelperService
{
    /// <summary>
    /// Formats to currency.
    /// </summary>
    /// <param name="dataList">The data list.</param>
    /// <param name="exchangeCodeModel">The exchange code.</param>
    /// <returns>Returns formatted data.</returns>
    Dictionary<string, string> FormatToCurrency(Dictionary<string, double> dataList, ExchangeCode exchangeCodeModel);

    /// <summary>
    /// Converts to multiply.
    /// </summary>
    /// <param name="dataList">The data list.</param>
    /// <returns>Returns formatted data.</returns>
    Dictionary<string, string> ConvertToMultiply(Dictionary<string, double> dataList);

    /// <summary>
    /// Converts to percentage.
    /// </summary>
    /// <param name="dataList">The data list.</param>
    /// <returns>Returns formatted data.</returns>
    Dictionary<string, string> ConvertToPercentage(Dictionary<string, double> dataList);

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <param name="dataList">The data list.</param>
    /// <returns>Returns formatted data.</returns>
    Dictionary<string, string> ConvertToString(Dictionary<string, double> dataList);

    /// <summary>
    /// Gets the eod data.
    /// </summary>
    /// <param name="eodDataModels">The eod data models.</param>
    /// <param name="date">The date.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns eod data.</returns>
    EODDataModel GetEODData(List<EODDataModel> eodDataModels, string date, PeriodTypes period);

    /// <summary>
    /// Gets the eps data.
    /// </summary>
    /// <param name="epsDataModels">The eps data models.</param>
    /// <param name="date">The date.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns eps data.</returns>
    EPSModel GetEPSData(List<EPSModel> epsDataModels, string date, PeriodTypes period);

    /// <summary>
    /// Gets the balance sheet data.
    /// </summary>
    /// <param name="balanceSheetDataModels">The balance sheet data models.</param>
    /// <param name="date">The date.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns balance sheet data.</returns>
    BalanceSheetModel GetBalanceSheetData(List<BalanceSheetModel> balanceSheetDataModels, string date, PeriodTypes period);

    /// <summary>
    /// Gets the outstanding shares data.
    /// </summary>
    /// <param name="outstandingSharesDataModels">The outstanding shares data models.</param>
    /// <param name="date">The date.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns outstanding shares data.</returns>
    OutstandingSharesModel GetOutstandingSharesData(List<OutstandingSharesModel> outstandingSharesDataModels,
        string date, PeriodTypes period);

    /// <summary>
    /// Gets the income statement.
    /// </summary>
    /// <param name="incomeStatementModels">The income statement models.</param>
    /// <param name="date">The date.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns income statement.</returns>
    IncomeStatementModel GetIncomeStatement(List<IncomeStatementModel> incomeStatementModels,
        string date, PeriodTypes period);

    /// <summary>
    /// Gets the cash flow.
    /// </summary>
    /// <param name="cashFlowModels">The cash flow models.</param>
    /// <param name="date">The date.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns cash flow model.</returns>
    CashFlowModel GetCashFlow(List<CashFlowModel> cashFlowModels, string date, PeriodTypes period);

    /// <summary>
    /// Gets the outstanding shares asynchronous.
    /// </summary>
    /// <param name="fullSymbolCode">The full symbol code.</param>
    /// <param name="periodType">Type of the period.</param>
    /// <returns>Returns list of outstandings.</returns>
    Task<List<OutstandingSharesModel>> GetOutstandingSharesAsync(string fullSymbolCode, string periodType);

    /// <summary>
    /// Gets the eod data asynchronous.
    /// </summary>
    /// <param name="fullSymbolCode">The full symbol code.</param>
    /// <param name="fromDate">From date.</param>
    /// <param name="toDate">To date.</param>
    /// <param name="order">The order.</param>
    /// <returns>Returns list of eod data.</returns>
    Task<List<EODDataModel>> GetEODDataAsync(string fullSymbolCode, string fromDate = null, string toDate = null, string order = null);

    /// <summary>
    /// Gets the eps asynchronous.
    /// </summary>
    /// <param name="fullSymbolCode">The full symbol code.</param>
    /// <returns>Returns eps data.</returns>
    Task<List<EPSModel>> GetEPSAsync(string fullSymbolCode);

    /// <summary>
    /// Gets the balance sheet asynchronous.
    /// </summary>
    /// <param name="fullSymbolCode">The full symbol code.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns balance sheets.</returns>
    Task<List<BalanceSheetModel>> GetBalanceSheetAsync(string fullSymbolCode, string period);

    /// <summary>
    /// Gets the income statement asynchronous.
    /// </summary>
    /// <param name="fullSymbolCode">The full symbol code.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns income statements.</returns>
    Task<List<IncomeStatementModel>> GetIncomeStatementAsync(string fullSymbolCode, string period);

    /// <summary>
    /// Gets the cash flow asynchronous.
    /// </summary>
    /// <param name="fullSymbolCode">The full symbol code.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns list of cashflow.</returns>
    Task<List<CashFlowModel>> GetCashFlowAsync(string fullSymbolCode, string period);

    /// <summary>
    /// Synchronizes the eod data asynchronous.
    /// </summary>
    /// <param name="eodDataModels">The eod data models.</param>
    /// <param name="symbolId">The symbol identifier.</param>
    Task SyncEODDataAsync(List<EODDataModel> eodDataModels, int symbolId);

    /// <summary>
    /// Gets the eod live data asynchronous.
    /// </summary>
    /// <param name="fullSymbolCode">The full symbol code.</param>
    /// <returns>Returns eod live data.</returns>
    Task<EODDataModel> GetEODLiveDataAsync(string fullSymbolCode);

    /// <summary>
    /// Synchronizes the eod live data asynchronous.
    /// </summary>
    /// <param name="eodDataModel">The eod data model.</param>
    /// <param name="symbolId">The symbol identifier.</param>
    Task SyncEODLiveDataAsync(EODDataModel eodDataModel, int symbolId);

    /// <summary>
    /// Gets the total eps per year data.
    /// </summary>
    /// <param name="epsDataModels">The eps data models.</param>
    /// <param name="date">The date.</param>
    /// <param name="period">The period.</param>
    /// <returns>Returns TTM eps value.</returns>
    double? GetTotalEPSPerYearData(List<EPSModel> epsDataModels, string date, PeriodTypes period);
}
