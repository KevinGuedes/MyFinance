using FluentResults;

namespace MyFinance.Application.Common.RequestHandling;

public abstract class Query<TResponse> : IQuery<Result<TResponse>>
{
}
