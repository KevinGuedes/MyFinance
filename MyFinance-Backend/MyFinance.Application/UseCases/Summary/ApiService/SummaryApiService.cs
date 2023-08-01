using ClosedXML.Excel;
using FluentResults;
using MediatR;
using MyFinance.Application.UseCases.Summary.Queries.GetBusinessUnitSummary;
using MyFinance.Application.UseCases.Summary.Queries.GetMonthlyBalanceSummary;

namespace MyFinance.Application.UseCases.Summary.ApiService;

public class SummaryApiService : ISummaryApiService
{
    private readonly IMediator _mediator;

    public SummaryApiService(IMediator mediator)
        => _mediator = mediator;

    public Task<Result<Tuple<string, XLWorkbook>>> GetBusinessUnitSummaryAsync(Guid id, CancellationToken cancellationToken)
        => _mediator.Send(new GetBusinessUnitSummaryQuery(id), cancellationToken);

    public Task<Result<Tuple<string, XLWorkbook>>> GetMonthlyBalanceSummaryAsync(Guid id, CancellationToken cancellationToken)
        => _mediator.Send(new GetMonthlyBalanceSummaryQuery(id), cancellationToken);
}
