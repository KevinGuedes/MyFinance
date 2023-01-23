using FluentResults;

namespace MyFinance.Application.Generics.Requests
{
    public abstract class Query<TResponse> : IQuery<Result<TResponse>>
    {
    }
}
