using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.RequestHandling;
using System.Diagnostics;

namespace MyFinance.Application.RequestPipeline.Behaviors;

internal sealed class TimingBehavior<TRequest, TResponse>(ILogger<TimingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseAppRequest
    where TResponse : ResultBase
{
    private readonly ILogger<TimingBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = request.GetType().Name;
        var startTime = Stopwatch.GetTimestamp();

        _logger.LogInformation("Handling {RequestName}", requestName);
        var response = await next();

        _logger.LogInformation(
            "{RequestName} {Status} - Main flow execution time: {ElapsedTime}ms",
            requestName,
            response.IsSuccess ? "succeeded" : "failed",
            Stopwatch.GetElapsedTime(startTime));

        return response;
    }
}