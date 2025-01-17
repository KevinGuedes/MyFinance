﻿using FluentResults;
using MediatR;
using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.RequestPipeline.Behaviors;

internal sealed class UserProviderBehavior<TRequest, TResponse>(ICurrentUserProvider currentUserProvider)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IUserRequiredRequest
    where TResponse : ResultBase, new()
{
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_currentUserProvider.TryGetCurrentUserId(out var currentUserId))
        {
            request.CurrentUserId = currentUserId;
            return await next();
        }

        var internalServerError = new InternalServerError("Failed to identify current User");
        var internalServerErrorResponse = new TResponse();
        internalServerErrorResponse.Reasons.AddRange(internalServerError.Reasons);
        return internalServerErrorResponse;
    }
}