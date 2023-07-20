using FluentResults;
using MediatR;

namespace MyFinance.Application.Common.RequestHandling;

public abstract class QueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse>>
    where TRequest : IQuery<Result<TResponse>>
{
    public abstract Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
}
