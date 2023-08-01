using MediatR;
using MyFinance.Application.UseCases.Summary.Commands.GenerateMonthlyBalanceSummary;

namespace MyFinance.Application.UseCases.Summary.ApiService;

public class SummaryApiService : ISummaryApiService
{
    private readonly IMediator _mediator;

    public SummaryApiService(IMediator mediator)
        => _mediator = mediator;

    public Task<int> GenerateBusinessUnitSummaryAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<int> GenerateMonthlyBalanceSummaryAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GenerateMonthlyBalanceSummaryCommand(id));
        return 1;
    }
}
