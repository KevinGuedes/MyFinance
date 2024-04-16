﻿using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.Transfer.Responses;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetDiscriminatedAnnualBalanceData;

public sealed record class GetDiscriminatedAnnualBalanceDataQuery(Guid BusinessUnitId, int Year)
    : IQuery<DiscriminatedAnnualBalanceDataResponse>
{
}
