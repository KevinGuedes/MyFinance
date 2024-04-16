using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Application.UseCases.HealthChecks.Queries.GetHealthChecksReport;
using MyFinance.Contracts.HealthCheck.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[SwaggerTag("Application Health Checks")]
public class HealthChecksController(IMediator mediator) : ApiController(mediator)
{
    [HttpGet]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Checks the health of the application")]
    [SwaggerResponse(StatusCodes.Status200OK, "Application is Healthy or Degraded", typeof(HealthyServicesResponse))]
    [SwaggerResponse(StatusCodes.Status503ServiceUnavailable, "Application is unhealthy", typeof(UnhealthyServicesResponse))]
    public async Task<IActionResult> GetHealthChecksReportAsync(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetHealthChecksReportQuery(), cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        var error = result.Errors.FirstOrDefault();

        if (error is UnhealthyServicesError unhealthyServicesError)
            return BuildUnhealthyApplicationResponse(unhealthyServicesError);

        return HandleFailureResult(error);
    }

    private ObjectResult BuildUnhealthyApplicationResponse(UnhealthyServicesError unhealthyServicesError)
    {
        var statusCode = StatusCodes.Status503ServiceUnavailable;

        var problemDetails = ProblemDetailsFactory.CreateProblemDetails(
            HttpContext,
            statusCode: statusCode,
            detail: "Service(s) currently unhealthy",
            instance: HttpContext.Request.Path);

        problemDetails.Title ??= ReasonPhrases.GetReasonPhrase(statusCode);

        var unhealthyServicesResponse = HealthChecksMapper.ETR.Map(problemDetails, unhealthyServicesError.HealthReport);

        return new(unhealthyServicesResponse)
        {
            StatusCode = problemDetails.Status
        };
    }
}
