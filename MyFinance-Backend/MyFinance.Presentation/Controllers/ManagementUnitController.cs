﻿using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Mappers;
using MyFinance.Application.UseCases.ManagementUnits.Commands.UnarchiveManagementUnit;
using MyFinance.Application.UseCases.ManagementUnits.Queries.GetBalanceDataFromPeriod;
using MyFinance.Application.UseCases.ManagementUnits.Queries.GetDiscriminatedAnnualBalanceData;
using MyFinance.Application.UseCases.ManagementUnits.Queries.GetManagementUnits;
using MyFinance.Application.UseCases.ManagementUnits.Queries.GetMonthlySummary;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.ManagementUnit.Requests;
using MyFinance.Contracts.ManagementUnit.Responses;
using MyFinance.Contracts.Summary.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[SwaggerTag("Management Units management")]
[SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(ProblemResponse))]
public class ManagementUnitController(IMediator mediator) : ApiController(mediator)
{
    private const string SPREADSHEET_CONTENT_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new Management Unit")]
    [SwaggerResponse(StatusCodes.Status201Created, "Created Management Unit", typeof(ManagementUnitResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    public async Task<IActionResult> CreateManagementUnitAsync(
        [FromBody] [SwaggerRequestBody("Management unit payload", Required = true)]
        CreateManagementUnitRequest request,
        CancellationToken cancellationToken) =>
        ProcessResult(await _mediator.Send(ManagementUnitMapper.RTC.Map(request), cancellationToken), true);

    [HttpGet]
    [SwaggerOperation(Summary = "Lists all Management Units with pagination")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of all existing Management Units with pagination",
        typeof(Paginated<ManagementUnitResponse>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid query parameters", typeof(ValidationProblemResponse))]
    public async Task<IActionResult> GetManagementUnitsAsync(
        [FromQuery] [SwaggerParameter("Page number", Required = true)]
        int pageNumber,
        [FromQuery] [SwaggerParameter("Units per page", Required = true)]
        int pageSize,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(new GetManagementUnitsQuery(pageNumber, pageSize), cancellationToken));

    [HttpGet("{id:guid}/MonthlySummary")]
    [SwaggerOperation(Summary = "Generates a monthly summary for a Management Unit")]
    [SwaggerResponse(StatusCodes.Status200OK, "Monthly Summary Spreadsheet for the given Management Unit",
        contentTypes: SPREADSHEET_CONTENT_TYPE)]
    [SwaggerResponse(StatusCodes.Status206PartialContent, "Monthly Summary Spreadsheet for the given Management Unit",
        contentTypes: SPREADSHEET_CONTENT_TYPE)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Management Unit not found", typeof(ProblemResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid query parameters", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "The Management Unit has no data to generate the summary",
        typeof(ProblemResponse))]
    public async Task<IActionResult> GetManagementUnitSummary(
        [FromRoute] [SwaggerParameter("Management Unit Id", Required = true)]
        Guid id,
        [FromQuery] [SwaggerParameter("Year", Required = true)]
        int year,
        [FromQuery] [SwaggerParameter("Month", Required = true)]
        int month,
        CancellationToken cancellationToken)
    {
        var query = new GetMonthlySummaryQuery(id, year, month);
        var result = await _mediator.Send(query, cancellationToken);
        return ProcessSummaryResult(result, SPREADSHEET_CONTENT_TYPE);
    }

    [HttpGet("PeriodBalance")]
    [SwaggerOperation(Summary = "Gets the income, outcome and balance of the given period according to query parameters")]
    [SwaggerResponse(StatusCodes.Status200OK, "Balance data", typeof(PeriodBalanceDataResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid query parameters", typeof(ValidationProblemResponse))]
    public async Task<IActionResult> GetPeriodBalanceAsync(
        [FromQuery][SwaggerParameter("Management Unit Id", Required = true)]
        Guid managementUnitId,
        [FromQuery][SwaggerParameter("Start date")]
        DateOnly startDate,
        [FromQuery][SwaggerParameter("End date")]
        DateOnly endDate,
        [FromQuery][SwaggerParameter("Category Id")]
        Guid categoryId,
        [FromQuery][SwaggerParameter("Account Tag Id")]
        Guid accountTagId,
        CancellationToken cancellationToken)
    {
        var query = new GetBalanceDataFromPeriodQuery(
            managementUnitId,
            startDate,
            endDate,
            categoryId,
            accountTagId);

        return ProcessResult(await _mediator.Send(query, cancellationToken));
    }

    [HttpGet("DiscriminatedAnnualBalance")]
    [SwaggerOperation(Summary = "Gets the income, outcome and balance for each month within a given year")]
    [SwaggerResponse(StatusCodes.Status200OK, "Annual balance data", typeof(DiscriminatedAnnualBalanceDataResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid query parameters", typeof(ValidationProblemResponse))]
    public async Task<IActionResult> GetDiscriminatedAnnualBalanceAsync(
        [FromQuery][SwaggerParameter("Management Unit Id", Required = true)]
        Guid managementUnitId,
        [FromQuery][SwaggerParameter("Year", Required = true)]
        int year,
        CancellationToken cancellationToken)
    {
        var query = new GetDiscriminatedAnnualBalanceDataQuery(managementUnitId, year);
        return ProcessResult(await _mediator.Send(query, cancellationToken));
    }

    [HttpPut]
    [SwaggerOperation(Summary = "Updates an existing Management Unit")]
    [SwaggerResponse(StatusCodes.Status200OK, "Updated Management Unit", typeof(ManagementUnitResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Management Unit not found", typeof(ProblemResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Management Unit was updated after the request was sent",
        typeof(ProblemResponse))]
    public async Task<IActionResult> UpdateManagementUnitAsync(
        [FromBody] [SwaggerRequestBody("Management Unit payload", Required = true)]
        UpdateManagementUnitRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(ManagementUnitMapper.RTC.Map(request), cancellationToken));

    [HttpPatch("{id:guid}")]
    [SwaggerOperation(Summary = "Unarchives an existing Management Unit")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Management Unit successfully unarchived")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid Management Unit Id", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Management Unit not found", typeof(ProblemResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Management Unit was updated after the request was sent",
        typeof(ProblemResponse))]
    public async Task<IActionResult> UnarchiveManagementUnitAsync(
        [FromRoute] [SwaggerParameter("Id of the Management Unit to unarchive", Required = true)]
        Guid id,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(new UnarchiveManagementUnitCommand(id), cancellationToken));

    [HttpDelete]
    [SwaggerOperation(Summary = "Logically deletes (archives) an existing Management Unit")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Management Unit successfully archived")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Management Unit not found", typeof(ProblemResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Management Unit was updated after the request was sent",
        typeof(ProblemResponse))]
    public async Task<IActionResult> ArchiveManagementUnitAsync(
        [FromBody] [SwaggerRequestBody("Payload to archvie a Management Unit", Required = true)]
        ArchiveManagementUnitRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(ManagementUnitMapper.RTC.Map(request), cancellationToken));

    private IActionResult ProcessSummaryResult(Result<SummaryResponse> result, string contentType)
    {
        if (result.IsFailed)
            return HandleFailureResult(result.Errors);

        var (fileName, fileContent) = result.Value;
        return File(fileContent, contentType, fileName, true);
    }
}