using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.Summary.Responses;

namespace MyFinance.Application.UseCases.ManagementUnits.Queries.GetMonthlySummary;

public sealed record GetMonthlySummaryQuery(
    Guid Id,
    int Year,
    int Month)
    : IQuery<SummaryResponse>
{
}