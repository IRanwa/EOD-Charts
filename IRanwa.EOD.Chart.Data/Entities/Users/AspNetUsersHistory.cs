using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRanwa.EOD.Chart.Data;

/// <summary>
/// Aspnet users history.
/// </summary>
/// <seealso cref="EntityBase" />
public class AspNetUsersHistory : EntityBase
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
    /// Gets or sets the password hash.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    [StringLength(128)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    [StringLength(128)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the content of the user image.
    /// </summary>
    public string UserImageContent { get; set; }

    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; }
}