namespace IRanwa.EOD.Chart.Model;

/// <summary>
/// Outstanding shares model.
/// </summary>
public class OutstandingSharesModel
{
    /// <summary>
    /// Gets or sets the date.
    /// </summary>
    public string Date { get; set; }

    /// <summary>
    /// Gets or sets the date formatted.
    /// </summary>
    public string DateFormatted { get; set; }

    /// <summary>
    /// Gets or sets the shares MLN.
    /// </summary>
    public string SharesMln { get; set; }

    /// <summary>
    /// Gets or sets the shares.
    /// </summary>
    public double? Shares { get; set; }
}
