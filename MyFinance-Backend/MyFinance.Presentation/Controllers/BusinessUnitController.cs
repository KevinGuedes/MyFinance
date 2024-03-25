using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Mappers;
using MyFinance.Application.UseCases.BusinessUnits.Commands.UnarchiveBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.Queries.GetBusinessUnits;
using MyFinance.Contracts.BusinessUnit.Requests;
using MyFinance.Contracts.BusinessUnit.Responses;
using MyFinance.Contracts.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[SwaggerTag("Create, read and update Business Units")]
[SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(ProblemResponse))]
public class BusinessUnitController(IMediator mediator) : ApiController(mediator)
{
    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new Business Unit")]
    [SwaggerResponse(StatusCodes.Status201Created, "Created Business Unit", typeof(BusinessUnitResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    public async Task<IActionResult> CreateBusinessUnitAsync(
        [FromBody] [SwaggerRequestBody("Business unit payload", Required = true)]
        CreateBusinessUnitRequest request,
        CancellationToken cancellationToken) =>
        ProcessResult(await _mediator.Send(BusinessUnitMapper.RTC.Map(request), cancellationToken), true);

    [HttpGet]
    [SwaggerOperation(Summary = "Lists all Business Units with pagination")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of all existing Business Units with pagination",
        typeof(Paginated<BusinessUnitResponse>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid query parameters", typeof(ValidationProblemResponse))]
    public async Task<IActionResult> GetBusinessUnitsAsync(
        [FromQuery] [SwaggerParameter("Page number", Required = true)]
        int pageNumber,
        [FromQuery] [SwaggerParameter("Units per page", Required = true)]
        int pageSize,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(new GetBusinessUnitsQuery(pageNumber, pageSize), cancellationToken));

    [HttpPut]
    [SwaggerOperation(Summary = "Updates an existing Business Unit")]
    [SwaggerResponse(StatusCodes.Status200OK, "Updated Business Unit", typeof(BusinessUnitResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Business Unit not found", typeof(ProblemResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Business Unit was updated after the request was sent",
        typeof(ProblemResponse))]
    public async Task<IActionResult> UpdateBusinessUnitAsync(
        [FromBody] [SwaggerRequestBody("Business Unit payload", Required = true)]
        UpdateBusinessUnitRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(BusinessUnitMapper.RTC.Map(request), cancellationToken));

    [HttpPatch("{id:guid}")]
    [SwaggerOperation(Summary = "Unarchives an existing Business Unit")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Business Unit successfully unarchived")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid Business Unit Id", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Business Unit not found", typeof(ProblemResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Business Unit was updated after the request was sent",
        typeof(ProblemResponse))]
    public async Task<IActionResult> UnarchiveBusinessUnitAsync(
        [FromRoute] [SwaggerParameter("Id of the Business Unit to unarchive", Required = true)]
        Guid id,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(new UnarchiveBusinessUnitCommand(id), cancellationToken));

    [HttpDelete]
    [SwaggerOperation(Summary = "Logically deletes (archives) an existing Business Unit")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Business Unit successfully archived")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Business Unit not found", typeof(ProblemResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Business Unit was updated after the request was sent",
        typeof(ProblemResponse))]
    public async Task<IActionResult> ArchiveBusinessUnitAsync(
        [FromBody] [SwaggerRequestBody("Payload to archvie a Business Unit", Required = true)]
        ArchiveBusinessUnitRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(BusinessUnitMapper.RTC.Map(request), cancellationToken));
}