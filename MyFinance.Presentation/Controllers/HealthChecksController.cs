using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.UseCases.HealthChecks.Queries.GetHealthReport;
using MyFinance.Contracts.HealthCheck.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[SwaggerTag("Application Health Checks")]
public class HealthChecksController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Checks the health of the application")]
    [SwaggerResponse(StatusCodes.Status200OK, "Application is Healthy or Degraded", typeof(HealthReportResponse))]
    [SwaggerResponse(StatusCodes.Status503ServiceUnavailable, "Application is unhealthy", typeof(UnhealthyApplicationResponse))]
    public async Task<IActionResult> GetHealthChecksReportAsync(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetHealthReportQuery(), cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        var error = result.Errors.FirstOrDefault();

        if (error is UnhealthyApplicationError unhealthyServicesError)
        {
            var problemDetails = ProblemDetailsFactory.CreateProblemDetails(
                HttpContext,
                statusCode: StatusCodes.Status503ServiceUnavailable,
                detail: "Service(s) currently unhealthy",
                instance: HttpContext.Request.Path);

            var unhealthyApplicationResponse = new UnhealthyApplicationResponse(
                unhealthyServicesError.HealthReport,
                problemDetails);

            return new ObjectResult(unhealthyApplicationResponse)
            {
                StatusCode = problemDetails.Status
            };
        }

        return HandleFailureResult(error);
    }
}
