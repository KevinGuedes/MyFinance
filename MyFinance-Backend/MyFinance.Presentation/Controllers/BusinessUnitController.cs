using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.BusinessUnits.ApiService;
using MyFinance.Application.BusinessUnits.Commands.CreateBusinessUnit;
using MyFinance.Application.BusinessUnits.Commands.UpdateBusinessUnit;
using MyFinance.Application.BusinessUnits.ViewModels;
using MyFinance.Application.Generics.ApiService;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Create, read and update Business Units")]
    public class BusinessUnitController : BaseApiController
    {
        private readonly IBusinessUnitApiService _businessUnitApiService;

        public BusinessUnitController(IBusinessUnitApiService businessUnitApiService)
            => _businessUnitApiService = businessUnitApiService;

        [HttpGet]
        [SwaggerOperation(Summary = "Lists all existing Business Units")]
        [SwaggerResponse(StatusCodes.Status200OK, "List of all existing Business Units", typeof(IEnumerable<BusinessUnitViewModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Backend went rogue", typeof(ApiErrorResponse))]
        public async Task<IActionResult> GetBusinessUnitsAsync(CancellationToken cancellationToken)
            => ProcessResult(await _businessUnitApiService.GetBusinessUnitsAsync(cancellationToken));

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new Business Unit")]
        [SwaggerResponse(StatusCodes.Status200OK, "Created Business Unit", typeof(BusinessUnitViewModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ApiInvalidDataResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Backend went rogue", typeof(ApiErrorResponse))]
        public async Task<IActionResult> CreateBusinessUnitAsync(
            [FromBody, SwaggerRequestBody("Business unit payload", Required = true)] CreateBusinessUnitCommand command,
            CancellationToken cancellationToken)
            => ProcessResult(await _businessUnitApiService.CreateBusinessUnitAsync(command, cancellationToken));

        [HttpPut]
        [SwaggerOperation(Summary = "Updates an existing Business Unit")]
        [SwaggerResponse(StatusCodes.Status200OK, "Updated Business Unit", typeof(BusinessUnitViewModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ApiInvalidDataResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Backend went rogue", typeof(ApiErrorResponse))]
        public async Task<IActionResult> UpdateBusinessUnitAsync(
            [FromBody, SwaggerRequestBody("Business unit payload", Required = true)] UpdateBusinessUnitCommand command,
            CancellationToken cancellationToken)
            => ProcessResult(await _businessUnitApiService.UpdateBusinessUnitAsync(command, cancellationToken));
    }
}
