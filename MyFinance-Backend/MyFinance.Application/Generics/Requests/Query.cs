using FluentResults;
using MyFinance.Application.Interfaces;

namespace MyFinance.Application.Generics.Requests
{
    public abstract class Query<TResponse> : IQuery<Result<TResponse>>
    {
    }
}
