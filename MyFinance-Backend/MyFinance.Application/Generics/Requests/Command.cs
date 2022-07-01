using FluentResults;
using MyFinance.Application.Interfaces;

namespace MyFinance.Application.Generics.Requests
{
    public abstract class Command<TResponse> : ICommand<Result<TResponse>>
    {
    }

    public abstract class Command : ICommand<Result>
    {
    }
}
