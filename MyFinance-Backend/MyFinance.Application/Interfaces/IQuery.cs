using MediatR;

namespace MyFinance.Application.Interfaces
{
    public interface IQuery<TResponse> : IRequest<TResponse>, IQueryRequest, IValidatable
    {
    }

    public interface IQueryRequest
    {
    }
}
