using MediatR;

namespace MyFinance.Application.Interfaces
{
    public interface IQuery<TResponse> : IRequest<TResponse>, IValidatable
    {
    }
}
