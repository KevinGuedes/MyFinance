using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.Transfer.Responses;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetAnnualBalanceData;

public sealed record class GetAnnualBalanceDataQuery(Guid BusinessUnitId, int Year) 
    : IQuery<AnnualBalanceDataResponse>
{
}
