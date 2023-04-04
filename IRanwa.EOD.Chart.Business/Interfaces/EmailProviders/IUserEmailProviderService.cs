namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// User email provider service
/// </summary>
public interface IUserEmailProviderService
{
    /// <summary>
    /// Users the registration email.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="toEmail">To email.</param>
    /// <param name="code">The code.</param>
    void UserRegistrationEmail(string username, string toEmail, string code);

    /// <summary>
    /// Users the password change email.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="toEmail">To email.</param>
    void UserPasswordChangeEmail(string username, string toEmail);

    /// <summary>
    /// Users the password reset email.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="toEmail">To email.</param>
    /// <param name="code">The code.</param>
    void UserPasswordResetEmail(string username, string toEmail, string code);
}
