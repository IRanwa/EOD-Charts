using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRanwa.EOD.Chart.Data;

/// <summary>
/// Aspnet email verifications.
/// </summary>
/// <seealso cref="EntityBase" />
public class AspNetEmailVerifications : EntityBase
{

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether this instance is verified.
    /// </summary>
    [DefaultValue("false")]
    public bool IsVerified { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is active.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
    /// </value>
    [DefaultValue("false")]
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the expire date time.
    /// </summary>
    public DateTime ExpireDateTime { get; set; }

    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; }
}