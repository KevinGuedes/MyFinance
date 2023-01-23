using MediatR;

namespace MyFinance.Application.Generics.Requests
{
    public interface ICommand<TResponse> : IRequest<TResponse>
    {
    }
}
