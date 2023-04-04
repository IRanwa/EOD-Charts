using IRanwa.EOD.Chart.Model;

namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// Email service.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends the email.
    /// </summary>
    /// <param name="model">The model.</param>
    void SendEmail(MailRequestModel model);
}
