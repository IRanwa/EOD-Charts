using System.ComponentModel.DataAnnotations;

namespace IRanwa.EOD.Chart.Core;

/// <summary>
/// Ordering type.
/// </summary>
public enum OrderingType
{
    /// <summary>
    /// Ascending
    /// </summary>
    [Display(Name = "a")]
    Ascending = 1,

    /// <summary>
    /// Descending
    /// </summary>
    [Display(Name = "d")]
    Descending = 2
}
