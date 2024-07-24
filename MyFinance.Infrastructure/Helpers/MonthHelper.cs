namespace MyFinance.Infrastructure.Helpers;

internal static class MonthHelper
{
    public static List<(int Year, int Month)> GetMonthsInRange(DateTime startDate, DateTime endDate)
    {
        var months = new List<(int Year, int Month)>();
        var date = new DateTime(startDate.Year, startDate.Month, 1);

        while (date <= endDate)
        {
            months.Add((date.Year, date.Month));
            date = date.AddMonths(1);
        }

        return months;
    }

    public static (DateTime FromDate, DateTime ToDate) GetDateRangeInUTCFromPastMonths(
        int pastMonths,
        bool includeCurrentMonth)
    {
        var today = DateTime.UtcNow;

        var startDate = today.AddMonths(-pastMonths);
        startDate = GetFirstDateOfMonthInUTC(startDate.Month, startDate.Year);

        var endDate = today.AddMonths(includeCurrentMonth ? 0 : -1);
        endDate = GetLastDateOfMonthInUTC(endDate.Month, endDate.Year);

        return (startDate, endDate);
    }

    public static DateTime GetFirstDateOfMonthInUTC(int month, int year)
         => new(year, month, 1, 0, 0, 0, DateTimeKind.Utc);

    public static DateTime GetLastDateOfMonthInUTC(int month, int year)
    {
        DateTime firstDayOfNextMonth;
        if (month == 12)
            firstDayOfNextMonth = new DateTime(year + 1, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        else
            firstDayOfNextMonth = new DateTime(year, month + 1, 1, 0, 0, 0, DateTimeKind.Utc);

        return firstDayOfNextMonth.AddSeconds(-1);
    }
}
