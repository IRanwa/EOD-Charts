using IRanwa.EOD.Chart.Core;

namespace IRanwa.EOD.Chart.Model;

/// <summary>
/// Stock data view model.
/// </summary>
public class StockDataViewModel
{
    /// <summary>
    /// Gets or sets the symbol.
    /// </summary>
    public string Symbol { get; set; }

    /// <summary>
    /// Gets or sets the exchange code.
    /// </summary>
    public string ExchangeCode { get; set; }

    /// <summary>
    /// Gets or sets the period.
    /// </summary>
    public PeriodTypes Period { get; set; }

    /// <summary>
    /// Gets or sets the last time stamp.
    /// </summary>
    public long? LastTimeStamp { get; set; }

    /// <summary>
    /// Gets or sets the type of the frequency.
    /// </summary>
    public FrequencyTypes? FrequencyType { get; set; }
}
