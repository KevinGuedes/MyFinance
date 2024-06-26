using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.Transfer.Responses;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetDiscriminatedBalanceData;

public sealed record GetDiscriminatedBalanceDataQuery(
    Guid ManagementUnitId, 
    int PastMonths,
    bool IncludeCurrentMonth)
    : IQuery<DiscriminatedBalanceDataResponse>
{
}
