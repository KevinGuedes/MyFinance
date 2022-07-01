using FluentResults;
using MediatR;
using MyFinance.Application.Interfaces;

namespace MyFinance.Application.Generics.Requests
{
    public abstract class CommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse>>
        where TRequest : ICommand<Result<TResponse>>
    {
        public abstract Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
    }

    public abstract class CommandHandler<TRequest> : IRequestHandler<TRequest, Result>
        where TRequest : ICommand<Result>
    {
        public abstract Task<Result> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
