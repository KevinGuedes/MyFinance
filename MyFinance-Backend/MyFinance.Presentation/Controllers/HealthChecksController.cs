using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.UseCases.HealthChecks.Queries.GetHealthChecksReport;
using MyFinance.Contracts.HealthCheck.Responses;
using MyFinance.Contracts.Summary.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[SwaggerTag("Application Health Checks")]
public class HealthChecksController(IMediator mediator) : ApiController(mediator)
{
    [HttpGet]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Checks the health of the application")]
    [SwaggerResponse(StatusCodes.Status200OK, "Application is Healthy or Degraded", typeof(HealthChecksReportResponse))]
    [SwaggerResponse(StatusCodes.Status503ServiceUnavailable, "Application is unhealthy", typeof(HealthChecksReportResponse))]
    public async Task<IActionResult> GetHealthChecksReportAsync(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetHealthChecksReportQuery(), cancellationToken);
        return ProcessHealthChecksReportResult(result);
    }

    private IActionResult ProcessHealthChecksReportResult(Result<HealthChecksReportResponse> result)
    {
        if(result.IsFailed)
            return HandleFailureResult(result.Errors);

        return result.Value.IsHealthy ? 
            Ok(result.Value) : 
            StatusCode(StatusCodes.Status503ServiceUnavailable, result.Value);
    }
}
