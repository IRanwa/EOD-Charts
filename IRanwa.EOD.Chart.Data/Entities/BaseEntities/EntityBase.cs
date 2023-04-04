using System.ComponentModel.DataAnnotations;

namespace IRanwa.EOD.Chart.Data;

/// <summary>
/// Entity base.
/// </summary>
public abstract class EntityBase
{
    /// <summary>
    /// Gets or sets the created user.
    /// </summary>
    [StringLength(128)]
    public string CreatedUser { get; set; }

    /// <summary>
    /// Gets or sets the created date time.
    /// </summary>
    public DateTime? CreatedDateTime { get; set; }

    /// <summary>
    /// Gets or sets the modified user.
    /// </summary>
    [StringLength(128)]
    public string ModifiedUser { get; set; }

    /// <summary>
    /// Gets or sets the modified date time.
    /// </summary>
    public DateTime? ModifiedDateTime { get; set; }
}
