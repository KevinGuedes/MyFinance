﻿using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Common.RequestHandling;

namespace MyFinance.Application.PipelineBehaviors;

public sealed class ExceptionHandlerBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseAppRequest
    where TResponse : ResultBase, new()
{
    private readonly ILogger<ExceptionHandlerBehavior<TRequest, TResponse>> _logger;

    public ExceptionHandlerBehavior(ILogger<ExceptionHandlerBehavior<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = request.GetType().Name;

        try
        {
            _logger.LogInformation("[{RequestName}] Starting to handle request", requestName);
            var result = await next();

            if (result.IsSuccess)
                _logger.LogInformation("[{RequestName}] Request handled with a success result", requestName);
            else
                _logger.LogWarning("[{RequestName}] Request handled with a failure result", requestName);

            return result;
        }
        catch (DbUpdateConcurrencyException exception)
        {
            _logger.LogError(exception, "[{RequestName}] Entity has been updated previously", requestName);

            var message = "The regarding entity has been previously updated. Try again later";
            var unprocessableEntityError = new UnprocessableEntityError(message);
            var failedResult = Result.Fail(unprocessableEntityError.CausedBy(exception));
            var response = new TResponse();
            response.Reasons.AddRange(failedResult.Reasons);

            return response;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "[{RequestName}] Failed to handle request", requestName);

            var failedResult = Result.Fail(new InternalServerError().CausedBy(exception));
            var response = new TResponse();
            response.Reasons.AddRange(failedResult.Reasons);

            return response;
        }
    }
}