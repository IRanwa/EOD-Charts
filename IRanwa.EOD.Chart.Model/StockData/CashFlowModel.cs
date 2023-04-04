namespace IRanwa.EOD.Chart.Model;

/// <summary>
/// Cash flow model.
/// </summary>
public class CashFlowModel
{
    /// <summary>
    /// Gets or sets the date.
    /// </summary>
    public string Date { get; set; }

    /// <summary>
    /// Gets or sets the total cash from operating activities.
    /// </summary>
    public double? TotalCashFromOperatingActivities { get; set; }

    /// <summary>
    /// Gets or sets the dividends paid.
    /// </summary>
    public double? DividendsPaid { get; set; }

    /// <summary>
    /// Gets or sets the free cash flow.
    /// </summary>
    public double? FreeCashFlow { get; set; }
}
