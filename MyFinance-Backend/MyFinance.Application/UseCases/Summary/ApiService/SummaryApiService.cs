using FluentResults;
using MediatR;
using MyFinance.Application.Abstractions.ApiServices;
using MyFinance.Application.Common.ApiService;
using MyFinance.Application.UseCases.Summary.Queries.GetBusinessUnitSummary;
using MyFinance.Application.UseCases.Summary.Queries.GetMonthlyBalanceSummary;

namespace MyFinance.Application.UseCases.Summary.ApiService;

public sealed class SummaryApiService(IMediator mediator) : BaseApiService(mediator), ISummaryApiService
{
    public Task<Result<Tuple<string, byte[]>>> GetBusinessUnitSummaryAsync(Guid id, int year,
        CancellationToken cancellationToken)
        => _mediator.Send(new GetBusinessUnitSummaryQuery(id, year), cancellationToken);

    public Task<Result<Tuple<string, byte[]>>> GetMonthlyBalanceSummaryAsync(Guid id,
        CancellationToken cancellationToken)
        => _mediator.Send(new GetMonthlyBalanceSummaryQuery(id), cancellationToken);
}