using IRanwa.EOD.Chart.Business;

namespace IRanwa.EOD.Charts.Jobs.SyncData;

public class Application
{
    private readonly IExchangeService exchangeService;

    public Application(IExchangeService exchangeService)
    {
        this.exchangeService = exchangeService;
    }

    public void Run()
    {
        try
        {
            Console.WriteLine("Application run started");

            var exchangeCodes = exchangeService.GetExchangeCodesAsync().Result;
            foreach(var code in exchangeCodes)
            {
                try
                {
                    var symbols = exchangeService.GetExchangeSymbolsAsync(code.Code, null);
                }catch(Exception ex)
                {
                    Console.WriteLine($"symbols sync failed {ex.ToString()}");
                }
            }
            Console.WriteLine("Application run ended");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Application run failed {ex.ToString()}");
        }
    }
}
