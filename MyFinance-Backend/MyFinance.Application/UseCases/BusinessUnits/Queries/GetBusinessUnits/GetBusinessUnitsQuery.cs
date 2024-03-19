using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.BusinessUnit.Responses;
using MyFinance.Contracts.Common;

namespace MyFinance.Application.UseCases.BusinessUnits.Queries.GetBusinessUnits;

public sealed record GetBusinessUnitsQuery(int PageNumber, int PageSize)
    : IQuery<Paginated<BusinessUnitResponse>>, IUserBasedRequest
{
    public Guid CurrentUserId { get; set; }
}