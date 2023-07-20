using AutoMapper;
using FluentResults;
using MediatR;

namespace MyFinance.Application.Common.ApiService;

public abstract class EntityApiService
{
    private protected readonly IMediator _mediator;
    private protected readonly IMapper _mapper;

    public EntityApiService(IMediator mediator, IMapper mapper)
        => (_mediator, _mapper) = (mediator, mapper);

    protected Result<TTarget> MapResult<TSource, TTarget>(Result<TSource> result)
    {
        if (result.IsSuccess)
            return result.ToResult(value => _mapper.Map<TTarget>(value));

        return result.ToResult<TTarget>();
    }

    protected Result<IEnumerable<TTarget>> MapResult<TSource, TTarget>(Result<IEnumerable<TSource>> result)
    {
        if (result.IsSuccess)
            return result.ToResult(_mapper.Map<IEnumerable<TTarget>>);

        return result.ToResult<IEnumerable<TTarget>>();
    }
}
