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
                return Result.Ok(_mapper.Map<TTarget>(result.Value));

            return Result.Fail(result.Errors);
        }

        protected Result<IEnumerable<TTarget>> ProcessResultAndMapIfSuccess<TSource, TTarget>(Result<IEnumerable<TSource>> result)
        {
            if (result.IsSuccess)
                return Result.Ok(_mapper.Map<IEnumerable<TTarget>>(result.Value));

            return Result.Fail(result.Errors);
        }

        protected static Result ProcessResult(Result result)
        {
            if (result.IsSuccess)
                return Result.Ok();

            return Result.Fail(result.Errors);
        }
    }
}
