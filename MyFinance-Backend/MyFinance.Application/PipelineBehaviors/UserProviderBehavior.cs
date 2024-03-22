using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.PipelineBehaviors;

internal sealed class UserProviderBehavior<TRequest, TResponse>(ICurrentUserProvider currentUserProvider)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IUserRequiredRequest
    where TResponse : ResultBase, new()
{
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        if (currentUserId.HasValue)
        {
            request.CurrentUserId = currentUserId.Value;
            return await next();
        }

        var response = new TResponse();
        var failedResult = Result.Fail(new UnauthorizedError("Unable to identify user"));
        response.Reasons.AddRange(failedResult.Reasons);

        return response;
    }
}