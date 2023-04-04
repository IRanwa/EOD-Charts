using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRanwa.EOD.Chart.Data;

/// <summary>
/// EOD data
/// </summary>
/// <seealso cref="EntityBase" />
[Index(nameof(Timestamp))]
[Index(nameof(ExchangeSymbol))]
public class EODData : EntityBase
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the timestamp.
    /// </summary>
    public long Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the symbol identifier.
    /// </summary>
    public int ExchangeSymbol { get; set; }

    /// <summary>
    /// Gets or sets the open.
    /// </summary>
    public double? Open { get; set; }

    /// <summary>
    /// Gets or sets the close.
    /// </summary>
    public double? Close { get; set; }

    /// <summary>
    /// Gets or sets the hight.
    /// </summary>
    public double? High { get; set; }

    /// <summary>
    /// Gets or sets the low.
    /// </summary>
    public double? Low { get; set; }

    /// <summary>
    /// Gets or sets the adjusted close.
    /// </summary>
    public double? AdjustedClose { get; set; }

    /// <summary>
    /// Gets or sets the volume.
    /// </summary>
    public long Volume { get; set; }

    /// <summary>
    /// Gets or sets the exchange symbol data.
    /// </summary>
    [ForeignKey(nameof(ExchangeSymbol))]
    public virtual ExchangeSymbol ExchangeSymbolData { get; set; }
}
