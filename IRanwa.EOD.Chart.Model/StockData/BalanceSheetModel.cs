namespace IRanwa.EOD.Chart.Model;

/// <summary>
/// Balance sheet model.
/// </summary>
public class BalanceSheetModel
{
    /// <summary>
    /// Gets or sets the date.
    /// </summary>
    public string Date { get; set; }

    /// <summary>
    /// Gets or sets the total assets.
    /// </summary>
    public double? TotalAssets { get; set; }

    /// <summary>
    /// Gets or sets the total liab.
    /// </summary>
    public double? TotalLiab { get; set; }

    /// <summary>
    /// Gets or sets the total current assets.
    /// </summary>
    public double? TotalCurrentAssets { get; set; }

    /// <summary>
    /// Gets or sets the total current liabilities.
    /// </summary>
    public double? TotalCurrentLiabilities { get; set; }

    /// <summary>
    /// Gets or sets the long term debt.
    /// </summary>
    public double? LongTermDebt { get; set; }

    /// <summary>
    /// Gets or sets the short term debt.
    /// </summary>
    public double? ShortTermDebt { get; set; }

    /// <summary>
    /// Gets or sets the cash and equivalents.
    /// </summary>
    public double? CashAndEquivalents { get; set; }

    /// <summary>
    /// Gets or sets the total stockholder equity.
    /// </summary>
    public double? TotalStockholderEquity { get; set; }

    /// <summary>
    /// Gets or sets the short term investments.
    /// </summary>
    public double? ShortTermInvestments { get; set; }

    /// <summary>
    /// Gets or sets the net receivables.
    /// </summary>
    public double? NetReceivables { get; set; }

    /// <summary>
    /// Gets or sets the net debt.
    /// </summary>
    public double? NetDebt { get; set; }

    /// <summary>
    /// Gets or sets the cash.
    /// </summary>
    public double? Cash { get; set; }

    /// <summary>
    /// Gets or sets the common stock shares outstanding.
    /// </summary>
    public double? CommonStockSharesOutstanding { get; set; }
}
