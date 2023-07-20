using MediatR;

namespace MyFinance.Application.Common.RequestHandling;

public interface ICommand<TResponse> : IRequest<TResponse>
{
}
