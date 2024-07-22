using FluentResults;
using MediatR;

namespace MyFinance.Application.Abstractions.RequestHandling.Queries;

internal interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}