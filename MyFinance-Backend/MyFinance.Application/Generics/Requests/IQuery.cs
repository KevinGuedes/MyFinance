using MediatR;

namespace MyFinance.Application.Generics.Requests
{
    public interface IQuery<TResponse> : IRequest<TResponse>
    {
    }
}
