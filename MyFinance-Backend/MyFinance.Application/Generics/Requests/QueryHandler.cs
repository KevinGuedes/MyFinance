using FluentResults;
using MediatR;
using MyFinance.Application.Interfaces;

namespace MyFinance.Application.Generics.Requests
{
    public abstract class QueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse>>
        where TRequest : IQuery<Result<TResponse>>
    {
        public abstract Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
