namespace IRanwa.EOD.Chart.Model;

/// <summary>
/// EPS Model.
/// </summary>
public class EPSModel
{
    /// <summary>
    /// Gets or sets the report date.
    /// </summary>
    public string ReportDate { get; set; }

    /// <summary>
    /// Gets or sets the date.
    /// </summary>
    public string Date { get; set; }

    /// <summary>
    /// Gets or sets the before after market.
    /// </summary>
    public string BeforeAfterMarket { get; set; }

    /// <summary>
    /// Gets or sets the currency.
    /// </summary>
    public string Currency { get; set; }

    /// <summary>
    /// Gets or sets the eps actual.
    /// </summary>
    public double? EpsActual { get; set; }

    /// <summary>
    /// Gets or sets the eps estimate.
    /// </summary>
    public double? EpsEstimate { get; set; }

    /// <summary>
    /// Gets or sets the eps difference.
    /// </summary>
    public double? EpsDifference { get; set; }

    /// <summary>
    /// Gets or sets the surprise percent.
    /// </summary>
    public double? SurprisePercent { get; set; }
}
