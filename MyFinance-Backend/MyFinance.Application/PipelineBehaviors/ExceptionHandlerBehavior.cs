﻿using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.PipelineBehaviors;

public sealed class ExceptionHandlerBehavior<TRequest, TResponse>(
    ILogger<ExceptionHandlerBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseAppRequest
    where TResponse : ResultBase, new()
{
    private readonly ILogger<ExceptionHandlerBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = request.GetType().Name;

        try
        {
            var response = await next();
            return response;
        }
        catch (DbUpdateConcurrencyException exception)
        {
            _logger.LogError(exception, "Failed to handle {RequestName}. Entity has been updated previously", requestName);

            var conflictError =
                new ConflictError("The regarding entity has already been update. Check the updated data and try again");
            var failedResult = Result.Fail(conflictError.CausedBy(exception));
            var response = new TResponse();
            response.Reasons.AddRange(failedResult.Reasons);

            return response;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to handle {RequestName}", requestName);

            var failedResult = Result.Fail(new InternalServerError().CausedBy(exception));
            var response = new TResponse();
            response.Reasons.AddRange(failedResult.Reasons);

            return response;
        }
    }
}