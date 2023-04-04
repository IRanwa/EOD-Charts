using Serilog;

namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// Log service.
/// </summary>
/// <seealso cref="ILogService" />
public class LogService : ILogService
{
    /// <summary>
    /// Adds the information.
    /// </summary>
    /// <param name="message">The message.</param>
    public void AddInformation(string message)
    {
        Log.Information(message);
        Console.WriteLine(message);
    }

    /// <summary>
    /// Adds the error.
    /// </summary>
    /// <param name="message">The message.</param>
    public void AddError(string message)
    {
        Log.Error(message);
        Console.WriteLine(message);
    }
}
