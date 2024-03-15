using MyFinance.Application.Abstractions.RequestHandling.Queries;

namespace MyFinance.Application.UseCases.Summary.Queries.GetMonthlyBalanceSummary;

public sealed record GetMonthlyBalanceSummaryQuery(Guid Id) : IQuery<Tuple<string, byte[]>>
{
}