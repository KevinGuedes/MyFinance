using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.ApiResponses;
using MyFinance.Application.UseCases.BusinessUnits.ApiService;
using MyFinance.Application.UseCases.BusinessUnits.Commands.ArchiveBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.Commands.CreateBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.Commands.UpdateBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[SwaggerTag("Create, read and update Business Units")]
public class BusinessUnitController(IBusinessUnitApiService businessUnitApiService) : BaseController
{
    private readonly IBusinessUnitApiService _businessUnitApiService = businessUnitApiService;

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new Business Unit")]
    [SwaggerResponse(StatusCodes.Status201Created, "Created Business Unit", typeof(BusinessUnitDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
    public async Task<IActionResult> CreateBusinessUnitAsync(
        [FromBody, SwaggerRequestBody("Business unit payload", Required = true)] CreateBusinessUnitCommand command,
        CancellationToken cancellationToken)
        => ProcessResult(await _businessUnitApiService.CreateBusinessUnitAsync(command, cancellationToken), true);

    [HttpGet]
    [SwaggerOperation(Summary = "Lists all Business Units with pagination")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of all existing Business Units with pagination", typeof(IEnumerable<BusinessUnitDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid query parameters", typeof(BadRequestResponse))]
    public async Task<IActionResult> GetBusinessUnitsAsync(
        [FromQuery, SwaggerParameter("Page number", Required = true)] int page,
        [FromQuery, SwaggerParameter("Units per page", Required = true)] int pageSize,
        CancellationToken cancellationToken)
        => ProcessResult(await _businessUnitApiService.GetBusinessUnitsAsync(page, pageSize, cancellationToken));

    [HttpPut]
    [SwaggerOperation(Summary = "Updates an existing Business Unit")]
    [SwaggerResponse(StatusCodes.Status200OK, "Updated Business Unit", typeof(BusinessUnitDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Business Unit not found", typeof(EntityNotFoundResponse))]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Server currently unable to process the Business Unit", typeof(UnprocessableEntityResponse))]
    public async Task<IActionResult> UpdateBusinessUnitAsync(
        [FromBody, SwaggerRequestBody("Business Unit payload", Required = true)] UpdateBusinessUnitCommand command,
        CancellationToken cancellationToken)
        => ProcessResult(await _businessUnitApiService.UpdateBusinessUnitAsync(command, cancellationToken));

    [HttpPatch("{id:guid}")]
    [SwaggerOperation(Summary = "Unarchives an existing Business Unit")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Business Unit successfully unarchived")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid Business Unit Id", typeof(BadRequestResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Business Unit not found", typeof(EntityNotFoundResponse))]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Server currently unable to process the Business Unit", typeof(UnprocessableEntityResponse))]
    public async Task<IActionResult> UnarchiveBusinessUnitAsync(
        [FromRoute, SwaggerParameter("Id of the Business Unit to unarchive", Required = true)] Guid id,
        CancellationToken cancellationToken)
        => ProcessResult(await _businessUnitApiService.UnarchiveBusinessUnitAsync(id, cancellationToken));

    [HttpDelete]
    [SwaggerOperation(Summary = "Logically deletes (archives) an existing Business Unit")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Business Unit successfully archived")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Business Unit not found", typeof(EntityNotFoundResponse))]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Server currently unable to process the Business Unit", typeof(UnprocessableEntityResponse))]
    public async Task<IActionResult> ArchiveBusinessUnitAsync(
        [FromBody, SwaggerRequestBody("Payload to archvie a Business Unit", Required = true)] ArchiveBusinessUnitCommand command,
        CancellationToken cancellationToken)
        => ProcessResult(await _businessUnitApiService.ArchiveBusinessUnitAsync(command, cancellationToken));
}
