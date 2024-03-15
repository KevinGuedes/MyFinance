using FluentResults;
using MediatR;
using MyFinance.Application.Abstractions.RequestHandling;

namespace MyFinance.Application.Abstractions.RequestHandling.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>, IBaseQuery, IBaseAppRequest
{
}
