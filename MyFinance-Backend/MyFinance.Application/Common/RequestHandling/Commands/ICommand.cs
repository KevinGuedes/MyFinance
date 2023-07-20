using FluentResults;
using MediatR;

namespace MyFinance.Application.Common.RequestHandling.Commands;

public interface ICommand : IRequest<Result>, IBaseCommand, IBaseAppRequest
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand, IBaseAppRequest
{
}
