using FluentResults;
using MediatR;

namespace MyFinance.Application.Common.RequestHandling.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>, IBaseQuery, IBaseAppRequest
{
}
