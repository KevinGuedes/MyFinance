﻿using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.Transfer.Responses;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetBalanceDataFromPeriod;

public sealed record class GetBalanceDataFromPeriodQuery(
    Guid BusinessUnitId,
    DateOnly? StartDate,
    DateOnly? EndDate,
    Guid? CategoryId,
    Guid? AccountTagId) : IQuery<PeriodBalanceDataResponse>
{
}