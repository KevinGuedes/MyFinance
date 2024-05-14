using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.ManagementUnit.Responses;

namespace MyFinance.Application.UseCases.ManagementUnits.Queries.GetDiscriminatedAnnualBalanceData;

public sealed record GetDiscriminatedAnnualBalanceDataQuery(Guid ManagementUnitId, int Year)
    : IQuery<DiscriminatedAnnualBalanceDataResponse>
{
}
