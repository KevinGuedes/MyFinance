using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Services;

public interface ISpreadsheetService
{
    Tuple<string, byte[]> GenerateMonthlySummary(BusinessUnit businessUnit, int year, int month);
}