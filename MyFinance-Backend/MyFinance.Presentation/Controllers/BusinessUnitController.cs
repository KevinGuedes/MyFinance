using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.ApiService;
using MyFinance.Application.UseCases.BusinessUnits.ApiService;
using MyFinance.Application.UseCases.BusinessUnits.Commands.ArchiveBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.Commands.CreateBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.Commands.UpdateBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[SwaggerTag("Create, read and update Business Units")]
public class BusinessUnitController : BaseController
{
    private readonly IBusinessUnitApiService _businessUnitApiService;

    public BusinessUnitController(IBusinessUnitApiService businessUnitApiService)
        => _businessUnitApiService = businessUnitApiService;

    [HttpGet]
    [SwaggerOperation(Summary = "Lists all Business Units")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of all existing Business Units", typeof(IEnumerable<BusinessUnitDTO>))]
    public async Task<IActionResult> GetBusinessUnitsAsync(CancellationToken cancellationToken)
        => ProcessResult(await _businessUnitApiService.GetBusinessUnitsAsync(cancellationToken));

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new Business Unit")]
    [SwaggerResponse(StatusCodes.Status200OK, "Created Business Unit", typeof(BusinessUnitDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
    public async Task<IActionResult> CreateBusinessUnitAsync(
        [FromBody, SwaggerRequestBody("Business unit payload", Required = true)] CreateBusinessUnitCommand command,
        CancellationToken cancellationToken)
        => ProcessResult(await _businessUnitApiService.CreateBusinessUnitAsync(command, cancellationToken));

    [HttpPut]
    [SwaggerOperation(Summary = "Updates an existing Business Unit")]
    [SwaggerResponse(StatusCodes.Status200OK, "Updated Business Unit", typeof(BusinessUnitDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
    public async Task<IActionResult> UpdateBusinessUnitAsync(
        [FromBody, SwaggerRequestBody("Business Unit payload", Required = true)] UpdateBusinessUnitCommand command,
        CancellationToken cancellationToken)
        => ProcessResult(await _businessUnitApiService.UpdateBusinessUnitAsync(command, cancellationToken));

    [HttpPatch("{id:guid}")]
    [SwaggerOperation(Summary = "Unarchives an existing Business Unit")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Business Unit successfully unarchived")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid Business Unit Id", typeof(BadRequestResponse))]
    public async Task<IActionResult> UnarchiveBusinessUnitAsync(
        [FromRoute, SwaggerParameter("Id of the Business Unit to unarchive", Required = true)] Guid id,
        CancellationToken cancellationToken)
        => ProcessResult(await _businessUnitApiService.UnarchiveBusinessUnitAsync(id, cancellationToken));

    [HttpDelete]
    [SwaggerOperation(Summary = "Logically deletes (archives) an existing Business Unit")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Business Unit successfully archived")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
    public async Task<IActionResult> ArchiveBusinessUnitAsync(
        [FromBody, SwaggerRequestBody("Payload to archvie a Business Unit", Required = true)] ArchiveBusinessUnitCommand command,
        CancellationToken cancellationToken)
        => ProcessResult(await _businessUnitApiService.ArchiveBusinessUnitAsync(command, cancellationToken));
}
