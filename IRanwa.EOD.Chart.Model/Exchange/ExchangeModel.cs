namespace IRanwa.EOD.Chart.Model;

/// <summary>
/// Exchange model.
/// </summary>
public class ExchangeModel
{
    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the operating mic.
    /// </summary>
    public string OperatingMIC { get; set; }

    /// <summary>
    /// Gets or sets the country.
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Gets or sets the currency.
    /// </summary>
    public string Currency { get; set; }

    /// <summary>
    /// Gets or sets the country is o2.
    /// </summary>
    public string CountryISO2 { get; set; }

    /// <summary>
    /// Gets or sets the country is o3.
    /// </summary>
    public string CountryISO3 { get; set; }
}
