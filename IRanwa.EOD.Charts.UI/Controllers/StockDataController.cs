using IRanwa.EOD.Chart.Business;
using IRanwa.EOD.Chart.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IRanwa.EOD.Charts.UI;

/// <summary>
/// Stock data controller.
/// </summary>
/// <seealso cref="ControllerBase" />
[Route("api/v1/stockdata")]
[ApiController]
[Authorize]
public class StockDataController : ControllerBase
{
    /// <summary>
    /// The stock data service
    /// </summary>
    private readonly IStockDataService stockDataService;

    /// <summary>
    /// The stock live data service
    /// </summary>
    private readonly IStockLiveDataService stockLiveDataService;

    /// <summary>
    /// Initializes a new instance of the <see cref="StockDataController"/> class.
    /// </summary>
    /// <param name="stockDataService">The stock data service.</param>
    public StockDataController(IStockDataService stockDataService, IStockLiveDataService stockLiveDataService)
    {
        this.stockDataService = stockDataService;
        this.stockLiveDataService = stockLiveDataService;
    }

    /// <summary>
    /// Gets the stock data asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns response.</returns>
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> GetStockDataAsync(StockDataViewModel model)
    {
        try
        {
            var data = await stockDataService.GetStockDataAsync(model.Symbol, model.ExchangeCode, model.Period);
            return Ok(data);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.GetAllMessages());
        }
    }

    /// <summary>
    /// Gets the stock live history data.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns response.</returns>
    [HttpPost]
    [Route("history")]
    public async Task<IActionResult> GetStockLiveHistoryData(StockDataViewModel model)
    {
        try
        {
            var data = await stockLiveDataService.GetStockLiveHistoryDataAsync(model);
            return Ok(data);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.GetAllMessages());
        }
    }

    /// <summary>
    /// Gets the stock live data.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Returns response.</returns>
    [HttpPost]
    [Route("live")]
    public async Task<IActionResult> GetStockLiveData(StockDataViewModel model)
    {
        try
        {
            var data = await stockLiveDataService.GetLiveStockDataAsync(model.ExchangeCode, model.Symbol);
            return Ok(data);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.GetAllMessages());
        }
    }
}
