using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Services;

public interface ISpreadsheetService
{
    Tuple<string, byte[]> GetBusinessUnitSummary(BusinessUnit businessUnit, int year);
    Tuple<string, byte[]> GetMonthlyBalanceSummary(MonthlyBalance monthlyBalance);
}
