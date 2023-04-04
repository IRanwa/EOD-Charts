using IRanwa.EOD.Chart.Model;
using Microsoft.Extensions.Configuration;

namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// User email provider service.
/// </summary>
/// <seealso cref="IUserEmailProviderService" />
public class UserEmailProviderService : IUserEmailProviderService
{
    /// <summary>
    /// The configuration
    /// </summary>
    private readonly IConfiguration configuration;

    /// <summary>
    /// The email service
    /// </summary>
    private readonly IEmailService emailService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserEmailProviderService"/> class.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <param name="emailService">The email service.</param>
    public UserEmailProviderService(
        IConfiguration configuration,
        IEmailService emailService)
    {
        this.configuration = configuration;
        this.emailService = emailService;
    }

    /// <summary>
    /// Users the registration email.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="toEmail">To email.</param>
    /// <param name="code">The code.</param>
    public void UserRegistrationEmail(string username, string toEmail, string code)
    {
        var filename = "UserRegistrationEmail.html";
        var filePath = Directory.GetFiles(String.Concat(AppDomain.CurrentDomain.BaseDirectory, "MailTemplates"),
            "*.html").Where(file => file.EndsWith(filename)).FirstOrDefault();
        if (filePath == null)
            return;

        var mailBody = File.ReadAllText(filePath);
        if (string.IsNullOrEmpty(mailBody))
            return;

        mailBody = mailBody.Replace("#Username", username);
        mailBody = mailBody.Replace("#EmailAddress", toEmail);
        mailBody = mailBody.Replace("#EmailVerificationUrl", string.Concat(configuration["PlatformWebURL"], "?email_verification_code=", code));

        var request = new MailRequestModel()
        {
            Body = mailBody,
            Subject = "EOD Platform Registration",
            ToEmail = toEmail
        };

        emailService.SendEmail(request);
    }

    /// <summary>
    /// Users the password change email.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="toEmail">To email.</param>
    public void UserPasswordChangeEmail(string username, string toEmail)
    {
        var filename = "UserPasswordChangeEmail.html";
        var filePath = Directory.GetFiles(String.Concat(AppDomain.CurrentDomain.BaseDirectory, "MailTemplates"),
            "*.html").Where(file => file.EndsWith(filename)).FirstOrDefault();
        if (filePath == null)
            return;

        var mailBody = File.ReadAllText(filePath);
        if (string.IsNullOrEmpty(mailBody))
            return;

        mailBody = mailBody.Replace("#Username", username);
        mailBody = mailBody.Replace("#EmailAddress", toEmail);

        var request = new MailRequestModel()
        {
            Body = mailBody,
            Subject = "EOD Platform Password Changed",
            ToEmail = toEmail
        };

        emailService.SendEmail(request);
    }

    /// <summary>
    /// Users the password reset email.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="toEmail">To email.</param>
    /// <param name="code">The code.</param>
    public void UserPasswordResetEmail(string username, string toEmail, string code)
    {
        var filename = "UserPasswordResetEmail.html";
        var filePath = Directory.GetFiles(String.Concat(AppDomain.CurrentDomain.BaseDirectory, "MailTemplates"),
            "*.html").Where(file => file.EndsWith(filename)).FirstOrDefault();
        if (filePath == null)
            return;

        var mailBody = File.ReadAllText(filePath);
        if (string.IsNullOrEmpty(mailBody))
            return;

        mailBody = mailBody.Replace("#Username", username);
        mailBody = mailBody.Replace("#EmailAddress", toEmail);
        mailBody = mailBody.Replace("#PasswordResetUrl", string.Concat(configuration["PlatformWebURL"], "?password_reset_code=", code));

        var request = new MailRequestModel()
        {
            Body = mailBody,
            Subject = "EOD Platform Password Reset",
            ToEmail = toEmail
        };

        emailService.SendEmail(request);
    }
}