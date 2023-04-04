using IRanwa.EOD.Chart.Business;
using IRanwa.EOD.Chart.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IRanwa.EOD.Charts.UI;

/// <summary>
/// Exchange controller.
/// </summary>
/// <seealso cref="ControllerBase" />
[Route("api/v1/exchange")]
[ApiController]
[Authorize]
public class ExchangeController : ControllerBase
{
    /// <summary>
    /// The exchange service
    /// </summary>
    private readonly IExchangeService exchangeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExchangeController"/> class.
    /// </summary>
    /// <param name="exchangeService">The exchange service.</param>
    public ExchangeController(IExchangeService exchangeService)
    {
        this.exchangeService = exchangeService;
    }

    /// <summary>
    /// Gets the exchange symbols asynchronous.
    /// </summary>
    /// <param name="exchangeCode">The exchange code.</param>
    /// <returns>Returns response.</returns>
    [HttpGet]
    [Route("symbols/{exchangeCode}")]
    public async Task<IActionResult> GetExchangeSymbolsAsync(string exchangeCode,StockTypes? stockType)
    {
        try
        {
            var data = await exchangeService.GetExchangeSymbolsAsync(exchangeCode, stockType);
            return Ok(data);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.GetAllMessages());
        }
    }

    /// <summary>
    /// Gets the exchange codes asynchronous.
    /// </summary>
    /// <returns>Returns response.</returns>
    [HttpGet]
    [Route("exchange-codes")]
    public async Task<IActionResult> GetExchangeCodesAsync()
    {
        try
        {
            var data = await exchangeService.GetExchangeCodesAsync();
            return Ok(data);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.GetAllMessages());
        }
    }

    /// <summary>
    /// Gets all exchange symbols asynchronous.
    /// </summary>
    /// <param name="stockType">Type of the stock.</param>
    /// <param name="currentSymbolsPage">The current symbols page.</param>
    /// <param name="searchKeyword">The search keyword.</param>
    /// <returns>Returns response.</returns>
    [HttpGet]
    [Route("all-symbols")]
    public IActionResult GetAllExchangeSymbolsAsync(StockTypes? stockType, int currentSymbolsPage, string searchKeyword)
    {
        try
        {
            var data = exchangeService.GetAllExchangeSymbols(stockType, currentSymbolsPage, searchKeyword);
            return Ok(data);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.GetAllMessages());
        }
    }
}
