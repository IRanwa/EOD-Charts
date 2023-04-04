namespace IRanwa.EOD.Chart.Model;

/// <summary>
/// Eod data view model.
/// </summary>
public class EODDataViewModel
{
    /// <summary>
    /// Gets or sets the time stamp.
    /// </summary>
    public long TimeStamp { get; set; }

    /// <summary>
    /// Gets or sets the data.
    /// </summary>
    public double[] Data { get; set; }
}
