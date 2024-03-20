using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Common.RequestHandling;
using MyFinance.Contracts.Summary.Responses;

namespace MyFinance.Application.UseCases.Summary.Queries.GetBusinessUnitSummary;

public sealed record GetBusinessUnitSummaryQuery(Guid Id, int Year)
    : UserBasedRequest, IQuery<SummaryResponse>
{
}