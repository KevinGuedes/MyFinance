using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MyFinance.Contracts.Common;

namespace MyFinance.Presentation.Middlewares;

public class GlobalExceptionHandlerMiddleware(
    ILogger<GlobalExceptionHandlerMiddleware> logger,
    ProblemDetailsFactory problemDetailsFactory) 
    : IExceptionHandler
{
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var problemResponse = _problemDetailsFactory.CreateProblemDetails(
            httpContext, 
            instance: httpContext.Request.Path,
            statusCode: StatusCodes.Status500InternalServerError, 
            detail: "MyFinance API went rogue! Sorry!") as ProblemResponse;

        httpContext.Response.StatusCode = problemResponse!.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problemResponse, cancellationToken);

        return true;
    }
}
