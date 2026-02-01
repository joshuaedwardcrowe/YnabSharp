using System.Globalization;

namespace YnabSharp.Sanitisers;

public static class IdentifierSanitiser
{
    public static string SanitiseForMonth(DateTime date)
    {
        var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month);
        return $"{monthName} {date.Year}";
    }

    public static string SanitiseForMonth(DateOnly dateOnly)
    {
        var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateOnly.Month);
        return $"{monthName} {dateOnly.Year}";
    }

    public static int SanitiseForYear(DateTime date) => date.Year;
    
    public static string SanitiseForFlag(string? flagName, FlagColor? flagColour) => $"{flagName} ({flagColour})";
}