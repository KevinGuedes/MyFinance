﻿using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Summary.Responses;

namespace MyFinance.Application.UseCases.Summary.Queries.GetBusinessUnitSummary;

internal sealed class GetBusinessUnitSummaryHandler(
    IBusinessUnitRepository businessUnitRepository,
    ISpreadsheetService spreadsheetService,
    ICurrentUserProvider currentUserProvider)
    : IQueryHandler<GetBusinessUnitSummaryQuery, SummaryResponse>
{
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;
    private readonly ISpreadsheetService _spreadsheetService = spreadsheetService;

    public async Task<Result<SummaryResponse>> Handle(GetBusinessUnitSummaryQuery query,
        CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        var businessUnit = await _businessUnitRepository.GetWithSummaryData(
            query.Id,
            query.Year,
            currentUserId,
            cancellationToken);

        if (businessUnit is null)
        {
            var errorMessage = $"Business Unit with Id {query.Id} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var hasMonthlyBalancesForProcessing = businessUnit.MonthlyBalances.Count is not 0;
        if (!hasMonthlyBalancesForProcessing)
        {
            var errorMessage = $"Business Unit with Id {query.Id} has no Monthly Balances with Transfers to summarize";
            var unprocessableEntityError = new UnprocessableEntityError(errorMessage);
            return Result.Fail(unprocessableEntityError);
        }

        var summaryData = _spreadsheetService.GetBusinessUnitSummary(businessUnit, query.Year);

        return Result.Ok(SummaryMapper.DTR.Map(summaryData));
    }
}