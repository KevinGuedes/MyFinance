using FluentResults;
using MediatR;

namespace MyFinance.Application.Abstractions.RequestHandling.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>, IBaseQuery, IBaseAppRequest
{
}