namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// Log Service.
/// </summary>
public interface ILogService
{
    /// <summary>
    /// Adds the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    void AddInformation(string message);

    /// <summary>
    /// Adds the error.
    /// </summary>
    /// <param name="message">The message.</param>
    void AddError(string message);
}
