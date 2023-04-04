namespace IRanwa.EOD.Chart.Model;

/// <summary>
/// EOD data model.
/// </summary>
public class EODDataModel
{
    /// <summary>
    /// Gets or sets the date.
    /// </summary>
    public string Date { get; set; }

    /// <summary>
    /// Gets or sets the open.
    /// </summary>
    public double? Open { get; set; }

    /// <summary>
    /// Gets or sets the high.
    /// </summary>
    public double? High { get; set; }

    /// <summary>
    /// Gets or sets the low.
    /// </summary>
    public double? Low { get; set; }

    /// <summary>
    /// Gets or sets the close.
    /// </summary>
    public double? Close { get; set; }

    /// <summary>
    /// Gets or sets the adjusted close.
    /// </summary>
    public double? Adjusted_Close { get; set; }

    /// <summary>
    /// Gets or sets the volume.
    /// </summary>
    public long Volume { get; set; }

    /// <summary>
    /// Gets or sets the timestamp.
    /// </summary>
    public long? Timestamp { get; set; }
}
