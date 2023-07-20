using FluentResults;

namespace MyFinance.Application.Common.RequestHandling;

public abstract class Command<TResponse> : ICommand<Result<TResponse>>
{
}

public abstract class Command : ICommand<Result>
{
}
