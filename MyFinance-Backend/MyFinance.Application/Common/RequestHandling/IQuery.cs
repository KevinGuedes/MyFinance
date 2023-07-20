using MediatR;

namespace MyFinance.Application.Common.RequestHandling;

public interface IQuery<TResponse> : IRequest<TResponse>
{
}
