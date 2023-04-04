using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IRanwa.EOD.Chart.Data;

/// <summary>
/// Application user.
/// </summary>
/// <seealso cref="IdentityUser" />
public class ApplicationUser : IdentityUser
{

    /// <summary>
    /// Gets or sets the created user.
    /// </summary>
    [StringLength(128)]
    public string CreatedUser { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the created date time.
    /// </summary>
    public DateTime? CreatedDateTime { get; set; }

    /// <summary>
    /// Gets or sets the modified user.
    /// </summary>
    [StringLength(128)]
    public string ModifiedUser { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the modified date time.
    /// </summary>
    public DateTime? ModifiedDateTime { get; set; }

    /// <summary>
    /// Gets or sets the email address for this user.
    /// </summary>
    public override string Email
    {
        get { return base.Email; }
        set { base.Email = value; }
    }

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
    /// Gets or sets the content of the user image.
    /// </summary>
    public string UserImageContent { get; set; }

    /// <summary>
    /// Gets or sets the last active.
    /// </summary>
    public DateTime? LastActive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [join newsletter].
    /// </summary>
    public bool JoinNewsletter { get; set; }
}
