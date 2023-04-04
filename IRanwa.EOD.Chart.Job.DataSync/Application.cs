using IRanwa.EOD.Chart.Business;

namespace IRanwa.EOD.Chart.Job.DataSync;

public class Application
{
    /// <summary>
    /// The exchange service
    /// </summary>
    private readonly IExchangeService exchangeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="Application"/> class.
    /// </summary>
    /// <param name="exchangeService">The exchange service.</param>
    public Application(IExchangeService exchangeService)
    {
        this.exchangeService = exchangeService;
    }

    public void Run()
    {
        try
        {
            Console.WriteLine("Data syncing started.");
            //var status = exchangeService.SyncExchangeCodesAndSymbols().Result;
            //Console.WriteLine($"Data syncing ended {status}.");
        }catch(Exception ex)
        {
            Console.WriteLine("Data syncing failed.");
        }
    }
}
