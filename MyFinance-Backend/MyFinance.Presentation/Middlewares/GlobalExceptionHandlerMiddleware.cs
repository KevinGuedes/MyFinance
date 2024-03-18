using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MyFinance.Presentation.Middlewares;

public class GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger) 
    : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "MyFinance API went rogue! Sorry.2222"
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
