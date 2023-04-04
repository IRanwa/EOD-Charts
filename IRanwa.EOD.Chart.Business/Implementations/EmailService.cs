using IRanwa.EOD.Chart.Model;
using Microsoft.Extensions.Options;
using Serilog;
using System.Net;
using System.Net.Mail;

namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// Email service.
/// </summary>
public class EmailService : IEmailService
{
    /// <summary>
    /// The mail settings
    /// </summary>
    private readonly MailSettingsModel mailSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailService"/> class.
    /// </summary>
    /// <param name="mailSettings">The mail settings.</param>
    public EmailService(IOptions<MailSettingsModel> mailSettings)
    {
        this.mailSettings = mailSettings.Value;
    }

    /// <summary>
    /// Sends the email asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    public void SendEmail(MailRequestModel model)
    {
        try
        {
            var email = new MailMessage();
            email.Sender = new MailAddress(mailSettings.Mail);
            email.From = new MailAddress(mailSettings.Mail, "EOD Platform");
            email.To.Add(new MailAddress(model.ToEmail));
            email.Subject = model.Subject;

            //var logoImagePath = Directory.GetFiles(String.Concat(AppDomain.CurrentDomain.BaseDirectory, "Content\\", "Images"),
            //"*.png").Where(file => file.EndsWith("logo.png")).FirstOrDefault();
            //if (logoImagePath != null)
            //{
            //    var att = new Attachment(logoImagePath);
            //    if (att != null && att.ContentDisposition != null)
            //    {
            //        att.ContentDisposition.Inline = true;
            //        model.Body = model.Body.Replace("#platformLogo", $@" src=""cid:{att.ContentId}""");
            //        email.Attachments.Add(att);
            //    }
            //}

            email.Body = model.Body;
            email.IsBodyHtml = true;

            using var smtp = new SmtpClient()
            {
                Host = mailSettings.Host,
                Port = mailSettings.Port,
                EnableSsl = mailSettings.SSLEnabled,
                Credentials = new NetworkCredential(mailSettings.Mail, mailSettings.Password),

            };

            smtp.Send(email);
            smtp.Dispose();
        }
        catch (Exception ex)
        {
            Log.Error(ex, string.Format("Send email failed : Model - {0}", model));
        }
    }
}