using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Services;

public interface ISummaryService
{
    Tuple<string, byte[]> GenerateMonthlySummary(BusinessUnit businessUnit, int year, int month);
}