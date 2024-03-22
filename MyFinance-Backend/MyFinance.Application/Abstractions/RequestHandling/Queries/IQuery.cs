using FluentResults;
using MediatR;

namespace MyFinance.Application.Abstractions.RequestHandling.Queries;

internal interface IQuery<TResponse> : IRequest<Result<TResponse>>, IBaseQuery, IBaseAppRequest
{
}