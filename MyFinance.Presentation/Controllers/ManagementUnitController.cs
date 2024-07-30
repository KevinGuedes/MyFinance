using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.UseCases.ManagementUnits.Commands.ArchiveManagementUnit;
using MyFinance.Application.UseCases.ManagementUnits.Commands.CreateManagementUnit;
using MyFinance.Application.UseCases.ManagementUnits.Commands.UnarchiveManagementUnit;
using MyFinance.Application.UseCases.ManagementUnits.Commands.UpdateManagementUnit;
using MyFinance.Application.UseCases.ManagementUnits.Queries.GetManagementUnit;
using MyFinance.Application.UseCases.ManagementUnits.Queries.GetManagementUnits;
using MyFinance.Application.UseCases.ManagementUnits.Queries.GetMonthlySummary;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.ManagementUnit.Requests;
using MyFinance.Contracts.ManagementUnit.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[SwaggerTag("Management Units management")]
[SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(ProblemResponse))]
public class ManagementUnitController(ISender sender) : ApiController(sender)
{
    private const string SPREADSHEET_CONTENT_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new Management Unit")]
    [SwaggerResponse(StatusCodes.Status201Created, "Created Management Unit", typeof(ManagementUnitResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    public async Task<IActionResult> CreateManagementUnitAsync(
        [FromBody] [SwaggerRequestBody("Management unit payload", Required = true)]
        CreateManagementUnitRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new CreateManagementUnitCommand(request), cancellationToken);
        return ProcessResult(result, true);
    }

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
        [FromQuery] [SwaggerParameter("Search term. It will be used get only Management Units which contain the search term in their name", Required = false)]
        string? searchTerm,
        CancellationToken cancellationToken)
    {
        var query = new GetManagementUnitsQuery(pageNumber, pageSize, searchTerm);
        var result = await _sender.Send(query, cancellationToken);
        return ProcessResult(result);
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Gets a Management Unit by it's Id")]
    [SwaggerResponse(StatusCodes.Status200OK, "The requested Management Unit", typeof(ManagementUnitResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Management Unit not found", typeof(EntityNotFoundError))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid query parameters", typeof(ValidationProblemResponse))]
    public async Task<IActionResult> GetManagementUnitByIdAsync(
       [FromRoute][SwaggerParameter("The Management Unit Id", Required = true)] Guid id,
       CancellationToken cancellationToken)
    {
        var query = new GetManagementUnitQuery(id);
        var result = await _sender.Send(query, cancellationToken);
        return ProcessResult(result);
    }

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
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailed)
            return HandleFailureResult(result.Errors);

        return File(
            result.Value.FileContent,
            SPREADSHEET_CONTENT_TYPE,
            result.Value.FileName,
            true);
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
    {
        var result = await _sender.Send(new UpdateManagementUnitCommand(request), cancellationToken);
        return ProcessResult(result);
    }

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
    {
        var result = await _sender.Send(new UnarchiveManagementUnitCommand(id), cancellationToken);
        return ProcessResult(result);
    }

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
    {
        var result = await _sender.Send(new ArchiveManagementUnitCommand(request), cancellationToken);
        return ProcessResult(result);
    }
}