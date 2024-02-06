using FluentResults;
using MediatR;
using MyFinance.Application.Common.ApiService;
using MyFinance.Application.UseCases.Summary.Queries.GetBusinessUnitSummary;
using MyFinance.Application.UseCases.Summary.Queries.GetMonthlyBalanceSummary;

namespace MyFinance.Application.UseCases.Summary.ApiService;

public sealed class SummaryApiService(IMediator mediator) : BaseApiService(mediator), ISummaryApiService
{
    public async Task<Result<Tuple<string, byte[]>>> GetBusinessUnitSummaryAsync(Guid id, int year, CancellationToken cancellationToken)
        => MapSummaryResult(await _mediator.Send(new GetBusinessUnitSummaryQuery(id, year), cancellationToken));

    public async Task<Result<Tuple<string, byte[]>>> GetMonthlyBalanceSummaryAsync(Guid id, CancellationToken cancellationToken)
        => MapSummaryResult(await _mediator.Send(new GetMonthlyBalanceSummaryQuery(id), cancellationToken));
}
