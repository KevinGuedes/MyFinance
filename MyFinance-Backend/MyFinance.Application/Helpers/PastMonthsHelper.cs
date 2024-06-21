namespace MyFinance.Application.Helpers;

internal static class PastMonthsHelper
{
    public static (DateTime FirstDate, DateTime LastDate) GetDateRangeFromPastMonths(int pastMonths)
    {
        var today = DateTime.UtcNow;

        if (pastMonths < 1)
            throw new ArgumentException("Past Months must be at least 1");

        var firstDate = today.AddMonths(-pastMonths);
        firstDate = GetFirstDateOfMonth(firstDate.Year, firstDate.Month);

        var lastDate = today.AddMonths(-1);
        lastDate = GetLastDateOfMonth(lastDate.Year, lastDate.Month);

        return (firstDate, lastDate);
    }

    private static DateTime GetFirstDateOfMonth(int year, int month)
         => new(year, month, 1, 0, 0, 0, DateTimeKind.Utc);

    private static DateTime GetLastDateOfMonth(int year, int month)
    {
        DateTime firstDayOfNextMonth;
        if (month == 12)
            firstDayOfNextMonth = new DateTime(year + 1, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        else
            firstDayOfNextMonth = new DateTime(year, month + 1, 1, 0, 0, 0, DateTimeKind.Utc);

        return firstDayOfNextMonth.AddSeconds(-1);
    }

}
