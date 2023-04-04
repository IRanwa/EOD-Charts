using System.ComponentModel.DataAnnotations;

namespace IRanwa.EOD.Chart.Core;

/// <summary>
/// Role types.
/// </summary>
public enum RoleTypes
{
    [Display(Name = "SuperAdmin")]
    SuperAdmin = 1,
    [Display(Name = "Admin")]
    Admin = 2,
    [Display(Name = "Guest")]
    Guest = 3
}
