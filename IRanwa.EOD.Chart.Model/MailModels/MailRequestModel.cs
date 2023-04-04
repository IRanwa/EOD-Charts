namespace IRanwa.EOD.Chart.Model;

/// <summary>
/// Mail request model.
/// </summary>
public class MailRequestModel
{
    /// <summary>
    /// Converts to email.
    /// </summary>
    public string ToEmail { get; set; }

    /// <summary>
    /// Gets or sets the subject.
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Gets or sets the body.
    /// </summary>
    public string Body { get; set; }
}