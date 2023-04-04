namespace IRanwa.EOD.Chart.Model;

/// <summary>
/// Mail settings model.
/// </summary>
public class MailSettingsModel
{

    /// <summary>
    /// Gets or sets the mail.
    /// </summary>
    public string Mail { get; set; }

    /// <summary>
    /// Gets or sets the display name.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the host.
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// Gets or sets the port.
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [SSL enabled].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [SSL enabled]; otherwise, <c>false</c>.
    /// </value>
    public bool SSLEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [use default credentials].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [use default credentials]; otherwise, <c>false</c>.
    /// </value>
    public bool UseDefaultCredentials { get; set; }
}