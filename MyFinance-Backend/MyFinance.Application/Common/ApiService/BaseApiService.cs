using FluentResults;
using MediatR;

namespace MyFinance.Application.Common.ApiService;

public abstract class BaseApiService(IMediator mediator)
{
    private protected readonly IMediator _mediator = mediator;

    protected static Result<TTarget> MapResult<TSource, TTarget>(
        Result<TSource> result,
        Func<TSource, TTarget> mapper)
    {
        if (result.IsSuccess)
            return result.ToResult(mapper);
        return result.ToResult<TTarget>();
    }

    protected static Result<IEnumerable<TTarget>> MapResult<TSource, TTarget>(
        Result<IEnumerable<TSource>> result,
        Func<IEnumerable<TSource>, IEnumerable<TTarget>> mapper)
    {
        if (result.IsSuccess)
            return result.ToResult(mapper);
        return result.ToResult<IEnumerable<TTarget>>();
    }
}
