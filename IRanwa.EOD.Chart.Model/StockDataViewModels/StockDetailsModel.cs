namespace IRanwa.EOD.Chart.Model;

/// <summary>
/// Stock details model.
/// </summary>
public class StockDetailsModel
{
    /// <summary>
    /// Gets or sets the outstanding shares.
    /// </summary>
    public List<OutstandingSharesModel> OutstandingShares { get; set; }

    /// <summary>
    /// Gets or sets the eod data.
    /// </summary>
    public List<EODDataModel> EODData { get; set; }

    /// <summary>
    /// Gets or sets the eps models.
    /// </summary>
    public List<EPSModel> EPSModels { get; set; }

    /// <summary>
    /// Gets or sets the balance sheets.
    /// </summary>
    public List<BalanceSheetModel> BalanceSheets { get; set; }

    /// <summary>
    /// Gets or sets the income statements.
    /// </summary>
    public List<IncomeStatementModel> IncomeStatements { get; set; }

    /// <summary>
    /// Gets or sets the cash flows.
    /// </summary>
    public List<CashFlowModel> CashFlows { get; set;}
}
