using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace IRanwa.EOD.Chart.Core;

public static class Extensions
{
    /// <summary>
    /// Gets the display name of the enum.
    /// </summary>
    /// <param name="enumValue">The enum value.</param>
    /// <returns>Return enum display name.</returns>
    public static string GetEnumDisplayName(this Enum enumValue)
    {
        return enumValue.GetType()
                        .GetMember(enumValue.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>().Name;
    }
}
