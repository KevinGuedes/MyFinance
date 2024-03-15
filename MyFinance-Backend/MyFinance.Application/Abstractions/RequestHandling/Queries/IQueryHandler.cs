using FluentResults;
using MediatR;

namespace MyFinance.Application.Abstractions.RequestHandling.Queries;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}