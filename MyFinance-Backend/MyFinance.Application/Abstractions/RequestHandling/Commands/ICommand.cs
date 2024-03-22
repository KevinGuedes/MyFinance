using FluentResults;
using MediatR;

namespace MyFinance.Application.Abstractions.RequestHandling.Commands;

internal interface ICommand : IRequest<Result>, IBaseCommand, IBaseAppRequest
{
}

internal interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand, IBaseAppRequest
{
}