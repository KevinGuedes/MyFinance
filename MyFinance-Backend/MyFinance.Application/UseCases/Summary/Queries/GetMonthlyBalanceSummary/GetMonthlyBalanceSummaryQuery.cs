﻿using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.Summary.Responses;

namespace MyFinance.Application.UseCases.Summary.Queries.GetMonthlyBalanceSummary;

public sealed record GetMonthlyBalanceSummaryQuery(Guid Id) :  IQuery<SummaryResponse>
{
}