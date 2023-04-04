namespace IRanwa.EOD.Chart.Model;

/// <summary>
/// Income statement model.
/// </summary>
public class IncomeStatementModel
{
    /// <summary>
    /// Gets or sets the date.
    /// </summary>
    public string Date { get; set; }

    /// <summary>
    /// Gets or sets the total revenue.
    /// </summary>
    public double? TotalRevenue { get; set; }

    /// <summary>
    /// Gets or sets the ebitda.
    /// </summary>
    public double? Ebitda { get; set; }

    /// <summary>
    /// Gets or sets the ebit.
    /// </summary>
    public double? Ebit { get; set; }

    /// <summary>
    /// Gets or sets the interest expense.
    /// </summary>
    public double? InterestExpense { get; set; }

    /// <summary>
    /// Gets or sets the net income.
    /// </summary>
    public double? NetIncome { get; set; }

    /// <summary>
    /// Gets or sets the gross profit.
    /// </summary>
    public double? GrossProfit { get; set; }

    /// <summary>
    /// Gets or sets the operating income.
    /// </summary>
    public double? OperatingIncome { get; set; }

    /// <summary>
    /// Gets or sets the net interest income.
    /// </summary>
    public double? NetInterestIncome { get; set; }
}
