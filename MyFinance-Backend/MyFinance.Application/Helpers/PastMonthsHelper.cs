namespace MyFinance.Application.Helpers;

internal static class PastMonthsHelper
{
    public static (DateTime FromDate, DateTime ToDate) GetDateRangeFromPastMonths(
        int pastMonths, 
        bool includeCurrentMont)
    {
        var today = DateTime.UtcNow;

        if (pastMonths < 1)
            throw new ArgumentException("Past Months must be at least 1");

        var fromDate = today.AddMonths(-pastMonths);
        fromDate = GetFirstDateOfMonth(fromDate.Year, fromDate.Month);

        var toDate = today.AddMonths(includeCurrentMont ? 0 : -1);
        toDate = GetLastDateOfMonth(toDate.Year, toDate.Month);

        return (fromDate, toDate);
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
