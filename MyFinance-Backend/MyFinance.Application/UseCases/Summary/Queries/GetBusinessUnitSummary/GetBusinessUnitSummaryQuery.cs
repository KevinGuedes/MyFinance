using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.Summary.Responses;

namespace MyFinance.Application.UseCases.Summary.Queries.GetBusinessUnitSummary;

public sealed record GetBusinessUnitSummaryQuery(Guid Id, int Year) 
    : IQuery<SummaryResponse>, IUserBasedRequest
{
    public Guid CurrentUserId { get; set; }
}