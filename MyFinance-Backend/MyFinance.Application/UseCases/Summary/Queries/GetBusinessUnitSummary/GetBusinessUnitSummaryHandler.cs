using ClosedXML.Excel;
using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Common.RequestHandling.Queries;
using MyFinance.Application.UseCases.Summary.Queries.GetMonthlyBalanceSummary;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.Summary.Queries.GetBusinessUnitSummary;

internal sealed class GetBusinessUnitSummaryHandler : IQueryHandler<GetMonthlyBalanceSummaryQuery, Tuple<string, XLWorkbook>>
{
    private readonly ILogger<GetBusinessUnitSummaryHandler> _logger;
    private readonly IBusinessUnitRepository _businessUnitRepository;

    public GetBusinessUnitSummaryHandler(
        ILogger<GetBusinessUnitSummaryHandler> logger,
        IBusinessUnitRepository businessUnitRepository)
        => (_logger, _businessUnitRepository) = (logger, businessUnitRepository);

    public async Task<Result<Tuple<string, XLWorkbook>>> Handle(GetMonthlyBalanceSummaryQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving Business Unit with Id {BusinessUnitId} ", query.Id);
        var businessUnit = await _businessUnitRepository.GetWithSummaryData(query.Id, cancellationToken);

        if (businessUnit is null)
        {
            _logger.LogWarning("Business Unit with Id {BusinessUnitId} not found", query.Id);
            var errorMessage = string.Format("Business Unit with Id {0} not found", query.Id);
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var fileName = businessUnit.Name;
        var workBook = new XLWorkbook();

        return Result.Ok(new Tuple<string, XLWorkbook>(fileName, workBook));
    }
}
