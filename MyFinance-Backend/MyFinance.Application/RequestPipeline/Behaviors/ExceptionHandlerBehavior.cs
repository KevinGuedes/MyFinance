using FluentResults;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.RequestPipeline.Behaviors;

internal sealed class ExceptionHandlerBehavior<TRequest, TResponse, TException>(
    ILogger<ExceptionHandlerBehavior<TRequest, TResponse, TException>> logger)
    : IRequestExceptionHandler<TRequest, TResponse, TException>
    where TRequest : IBaseAppRequest
    where TResponse : ResultBase, new()
    where TException : Exception
{
    private readonly ILogger<ExceptionHandlerBehavior<TRequest, TResponse, TException>> _logger = logger;

    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
    {
        var errorResult = exception switch
        {
            DbUpdateConcurrencyException => BuildConflictErrorResult(exception),
            _ => BuildInternalServerErrorResult(exception, request.GetType().Name)
        };

        var response = new TResponse();
        response.Reasons.AddRange(errorResult.Reasons);
        state.SetHandled(response);

        return Task.CompletedTask;
    }

    private static Result BuildConflictErrorResult(TException exception)
    {
        var conflictMessage = "The regarding entity has already been update. Check the updated data and try again";
        var conflictError = new ConflictError(conflictMessage).CausedBy(exception);
        return Result.Fail(conflictError);
    }

    private Result BuildInternalServerErrorResult(TException exception, string requestName)
    {
        _logger.LogError(exception, "An error occurred while processing {RequestName}", requestName);
        var internalServerError = new InternalServerError().CausedBy(exception);
        return Result.Fail(internalServerError);
    }
}