using MediatR;

namespace MyFinance.Application.Interfaces
{
    public interface ICommand<TResponse> : IRequest<TResponse>, IValidatable
    {
    }

    public interface ICommand : IRequest, IValidatable
    {
    }
}
