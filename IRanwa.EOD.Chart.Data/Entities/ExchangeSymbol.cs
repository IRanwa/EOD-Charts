using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRanwa.EOD.Chart.Data;

/// <summary>
/// Exchange symbol.
/// </summary>
/// <seealso cref="EntityBase" />
[Index(nameof(Code))]
[Index(nameof(ExchangeCodeId))]
[Index(nameof(Type))]
[Index(nameof(DataSyncCompleted))]
[Index(nameof(QuarterlySyncCompleted))]
[Index(nameof(AnnualSyncCompleted))]
public class ExchangeSymbol : EntityBase
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the country.
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Gets or sets the exchange.
    /// </summary>
    public string Exchange { get; set; }

    /// <summary>
    /// Gets or sets the currency.
    /// </summary>
    public string Currency { get; set; }

    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the isin.
    /// </summary>
    public string Isin { get; set; }

    /// <summary>
    /// Gets or sets the exchange code.
    /// </summary>
    public int ExchangeCodeId { get; set; }

    /// <summary>
    /// Gets or sets the last synchronize date.
    /// </summary>
    public DateTime? LastSyncDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [data synchronize completed].
    /// </summary>
    public bool DataSyncCompleted { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [quarterly synchronize completed].
    /// </summary>
    public bool QuarterlySyncCompleted { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [annual synchronize completed].
    /// </summary>
    public bool AnnualSyncCompleted { get; set; }

    /// <summary>
    /// Gets or sets the synchronize exception.
    /// </summary>
    public string SyncException { get; set; }

    /// <summary>
    /// Gets or sets the exchange code model.
    /// </summary>
    [ForeignKey(nameof(ExchangeCodeId))]
    public virtual ExchangeCode ExchangeCodeModel { get; set; }
}
