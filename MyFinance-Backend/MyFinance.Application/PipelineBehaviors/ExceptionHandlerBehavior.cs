using FluentResults;
using MediatR;
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
        catch (Exception exception)
        {
            _logger.LogError(exception, "[{RequestName}] Failed to handle request", requestName);

            var error = Result.Fail(new InternalServerError().CausedBy(exception));
            var response = new TResponse();
            response.Reasons.AddRange(error.Reasons);

            return response;
        }
    }
}
