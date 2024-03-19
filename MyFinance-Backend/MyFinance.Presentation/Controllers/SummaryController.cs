using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.UseCases.Summary.Queries.GetBusinessUnitSummary;
using MyFinance.Application.UseCases.Summary.Queries.GetMonthlyBalanceSummary;
using MyFinance.Contracts.Summary.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[SwaggerTag("Gets Summary Spreadsheets for Business Units and Monthly Balances")]
[SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
public class SummaryController(IMediator mediator) : ApiController(mediator)
{
    private const string SPREADSHEET_CONTENT_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    [HttpGet("BusinessUnit/{id:guid}")]
    [SwaggerOperation(Summary = "Get the Summary Spreadsheet for the given Business Unit")]
    [SwaggerResponse(StatusCodes.Status200OK, "Summary Spreasheet for the given Business Unit",
        contentTypes: SPREADSHEET_CONTENT_TYPE)]
    [SwaggerResponse(StatusCodes.Status206PartialContent, "Summary Spreasheet for the given Business Unit",
        contentTypes: SPREADSHEET_CONTENT_TYPE)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Business Unit not found", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid Business Unit Id", typeof(ValidationProblemDetails))]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "The Business Unit has no data to generate the summary",
        typeof(ProblemDetails))]
    public async Task<IActionResult> GetBusinessUnitSummary(
        [FromRoute] [SwaggerParameter("Business Unit Id", Required = true)]
        Guid id,
        [FromQuery] [SwaggerParameter("Year", Required = true)]
        int year,
        CancellationToken cancellationToken)
        => ProcessSummaryResult(
            await _mediator.Send(new GetBusinessUnitSummaryQuery(id, year), cancellationToken),
            SPREADSHEET_CONTENT_TYPE);

    [HttpGet("MonthlyBalance/{id:guid}")]
    [SwaggerOperation(Summary = "Get the Summary Spreadsheet for the given Monthly Balance")]
    [SwaggerResponse(StatusCodes.Status200OK, "Summary Spreasheet for the given Monthly Balance",
        contentTypes: SPREADSHEET_CONTENT_TYPE)]
    [SwaggerResponse(StatusCodes.Status206PartialContent, "Summary Spreasheet for the given Monthly Balance",
        contentTypes: SPREADSHEET_CONTENT_TYPE)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Monthly Balance not found", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid Monthly Balance Id", typeof(ValidationProblemDetails))]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity,
        "The Monthly Balance has no data to generate the summary", typeof(ProblemDetails))]
    public async Task<IActionResult> GetMonthlyBalanceSummaryAsync(
        [FromRoute] [SwaggerParameter("Monthly Balance Id", Required = true)]
        Guid id, CancellationToken cancellationToken)
        => ProcessSummaryResult(
            await _mediator.Send(new GetMonthlyBalanceSummaryQuery(id), cancellationToken),
            SPREADSHEET_CONTENT_TYPE);

    private IActionResult ProcessSummaryResult(Result<SummaryResponse> result, string contentType)
    {
        if (result.IsFailed)
            return HandleFailureResult(result.Errors);

        var (fileName, fileContent) = result.Value;
        return File(fileContent, contentType, fileName, true);
    }
}