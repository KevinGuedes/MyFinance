using AutoMapper;
using FluentResults;
using MediatR;

namespace MyFinance.Application.Generics.ApiService
{
    public abstract class EntityApiService
    {
        private protected readonly IMediator _mediator;
        private protected readonly IMapper _mapper;

        public EntityApiService(IMediator mediator, IMapper mapper)
            => (_mediator, _mapper) = (mediator, mapper);

        protected Result<TTarget> ProcessResultAndMapIfSuccess<TSource, TTarget>(Result<TSource> result)
        {
            if (result.IsSuccess)
                return result.ToResult(value => _mapper.Map<TTarget>(value));

            return result.ToResult<TTarget>();
        }

        protected Result<IEnumerable<TTarget>> ProcessResultAndMapIfSuccess<TSource, TTarget>(Result<IEnumerable<TSource>> result)
        {
            if (result.IsSuccess)
                return result.ToResult(value => _mapper.Map<IEnumerable<TTarget>>(value));

            return result.ToResult<IEnumerable<TTarget>>();
        }
    }
}
