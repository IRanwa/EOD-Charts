namespace IRanwa.EOD.Chart.Model;

/// <summary>
/// User password update model.
/// </summary>
public class UserPasswordModel
{
    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the old password.
    /// </summary>
    public string OldPassword { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string Username { get; set; }
}