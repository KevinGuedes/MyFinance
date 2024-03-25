using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace MyFinance.Presentation.Middlewares;

public class GlobalExceptionHandlerMiddleware(
    ILogger<GlobalExceptionHandlerMiddleware> logger,
    ProblemDetailsFactory problemDetailsFactory)
    : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger = logger;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            httpContext,
            instance: httpContext.Request.Path,
            statusCode: StatusCodes.Status500InternalServerError,
            detail: "MyFinance API went rogue! Sorry!");

        var response = new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails!.Status
        };

        await httpContext.Response.WriteAsJsonAsync(
            value: response.Value,
            type: response.Value!.GetType(),
            options: null, 
            contentType: "application/problem+json",
            cancellationToken: cancellationToken);
       
        return true;
    }
}