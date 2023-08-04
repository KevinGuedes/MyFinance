using AutoMapper;
using FluentResults;
using MediatR;
using MyFinance.Application.Common.ApiService;
using MyFinance.Application.UseCases.MonthlyBalances.DTOs;
using MyFinance.Application.UseCases.MonthlyBalances.Queries.GetMonthlyBalances;

namespace MyFinance.Application.UseCases.MonthlyBalances.ApiService;

public class MonthlyBalanceApiService : BaseApiService, IMonthlyBalanceApiService
{
    private readonly IMapper _mapper;

    public MonthlyBalanceApiService(IMediator mediator, IMapper mapper) : base(mediator)
        => _mapper = mapper;

    public async Task<Result<IEnumerable<MonthlyBalanceDTO>>> GetMonthlyBalancesAsync(
        Guid businessUnitId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = new GetMonthlyBalancesQuery(businessUnitId, page, pageSize);
        var result = await _mediator.Send(query, cancellationToken);
        return MapResult(result, _mapper.Map<IEnumerable<MonthlyBalanceDTO>>);
    }
}
