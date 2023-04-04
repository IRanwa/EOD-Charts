using System.ComponentModel.DataAnnotations;

namespace IRanwa.EOD.Chart.Core;

public enum StockTypes
{

    [Display(Name = "FUND")]
    FUND = 1,

    [Display(Name = "ETF")]
    ETF = 2,

    [Display(Name = "ETC")]
    ETC = 3,

    [Display(Name = "Preferred Stock")]
    PreferredStock = 4,

    [Display(Name = "Mutual Fund")]
    MutualFund = 5,

    [Display(Name = "Note")]
    Note = 6,

    [Display(Name = "Common Stock")]
    CommonStock = 7
}
