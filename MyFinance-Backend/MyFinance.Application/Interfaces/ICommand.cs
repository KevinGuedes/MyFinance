using MediatR;

namespace MyFinance.Application.Interfaces
{
    public interface ICommand<TResponse> : IRequest<TResponse>, ICommandRequest, IValidatable
    {
    }

    public interface ICommand : IRequest, ICommandRequest, IValidatable
    {
    }

    public interface ICommandRequest
    {
    }
}
