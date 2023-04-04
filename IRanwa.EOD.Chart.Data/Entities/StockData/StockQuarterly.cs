using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRanwa.EOD.Chart.Data;

/// <summary>
/// Stock quarterly
/// </summary>
/// <seealso cref="EntityBase" />
[Index(nameof(ExchangeSymbol))]
public class StockQuarterly : EntityBase
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the exchange symbol.
    /// </summary>
    public int ExchangeSymbol { get; set; }

    /// <summary>
    /// Gets or sets the date.
    /// </summary>
    public string Date { get; set; }

    /// <summary>
    /// Gets or sets the market cap.
    /// </summary>
    public string MarketCap { get; set; }

    /// <summary>
    /// Gets or sets the pe rate.
    /// </summary>
    public string PERate { get; set; }

    /// <summary>
    /// Gets or sets the pb rate.
    /// </summary>
    public string PBRate { get; set; }

    /// <summary>
    /// Gets or sets the current ratio.
    /// </summary>
    public string CurrentRatio { get; set; }

    /// <summary>
    /// Gets or sets the price to sale.
    /// </summary>
    public string PriceToSale { get; set; }

    /// <summary>
    /// Gets or sets the ev ebitda.
    /// </summary>
    public string EvEBITDA { get; set; }

    /// <summary>
    /// Gets or sets the ev sales.
    /// </summary>
    public string EvSales { get; set; }

    /// <summary>
    /// Gets or sets the ev opening cash flow.
    /// </summary>
    public string EvOpeningCashFlow { get; set; }

    /// <summary>
    /// Gets or sets the earning yield.
    /// </summary>
    public string EarningYield { get; set; }

    /// <summary>
    /// Gets or sets the debit to assets.
    /// </summary>
    public string DebitToAssets { get; set; }

    /// <summary>
    /// Gets or sets the interest coverage.
    /// </summary>
    public string InterestCoverage { get; set; }

    /// <summary>
    /// Gets or sets the payout ratio.
    /// </summary>
    public string PayoutRatio { get; set; }

    /// <summary>
    /// Gets or sets the roe.
    /// </summary>
    public string ROE { get; set; }

    /// <summary>
    /// Gets or sets the roa.
    /// </summary>
    public string ROA { get; set; }

    /// <summary>
    /// Gets or sets the roic.
    /// </summary>
    public string ROIC { get; set; }

    /// <summary>
    /// Gets or sets the quick ratio.
    /// </summary>
    public string QuickRatio { get; set; }

    /// <summary>
    /// Gets or sets the gross profit margin.
    /// </summary>
    public string GrossProfitMargin { get; set; }

    /// <summary>
    /// Gets or sets the dividend yield.
    /// </summary>
    public string DividendYield { get; set; }

    /// <summary>
    /// Gets or sets the price to cash flow.
    /// </summary>
    public string PriceToCashFlow { get; set; }

    /// <summary>
    /// Gets or sets the price to free cash flow.
    /// </summary>
    public string PriceToFreeCashFlow { get; set; }

    /// <summary>
    /// Gets or sets the free cash flow yield.
    /// </summary>
    public string FreeCashFlowYield { get; set; }

    /// <summary>
    /// Gets or sets the debit to equity.
    /// </summary>
    public string DebitToEquity { get; set; }

    /// <summary>
    /// Gets or sets the debit to ebitda.
    /// </summary>
    public string DebitToEBITDA { get; set; }

    /// <summary>
    /// Gets or sets the cash ratio.
    /// </summary>
    public string CashRatio { get; set; }

    /// <summary>
    /// Gets or sets the debit ratio.
    /// </summary>
    public string DebitRatio { get; set; }

    /// <summary>
    /// Gets or sets the operating profit margin.
    /// </summary>
    public string OperatingProfitMargin { get; set; }

    /// <summary>
    /// Gets or sets the assets turn over ratio.
    /// </summary>
    public string AssetsTurnOverRatio { get; set; }

    /// <summary>
    /// Gets or sets the return on capital employed.
    /// </summary>
    public string ReturnOnCapitalEmployed { get; set; }

    /// <summary>
    /// Gets or sets the ebitda margin.
    /// </summary>
    public string EBITDAMargin { get; set; }

    /// <summary>
    /// Gets or sets the net income margin.
    /// </summary>
    public string NetIncomeMargin { get; set; }

    /// <summary>
    /// Gets or sets the ebitda interest expense.
    /// </summary>
    public string EBITDAInterestExpense { get; set; }

    /// <summary>
    /// Gets or sets the exchange symbol data.
    /// </summary>
    [ForeignKey(nameof(ExchangeSymbol))]
    public virtual ExchangeSymbol ExchangeSymbolData { get; set; }
}
