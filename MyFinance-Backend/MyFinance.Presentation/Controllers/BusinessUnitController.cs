using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.BusinessUnits.ApiService;
using MyFinance.Application.BusinessUnits.Commands.CreateBusinessUnit;
using MyFinance.Application.BusinessUnits.Commands.UpdateBusinessUnit;
using MyFinance.Application.BusinessUnits.ViewModels;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Create, read and update Business Units")]
    public class BusinessUnitController : ControllerBase
    {
        private readonly IBusinessUnitApiService _businessUnitApiService;

        public BusinessUnitController(IBusinessUnitApiService businessUnitApiService)
            => _businessUnitApiService = businessUnitApiService;

        [HttpGet]
        [SwaggerOperation(Summary = "Lists all existing Business Units")]
        [SwaggerResponse(StatusCodes.Status200OK, "List of all existing Business Units", typeof(IEnumerable<BusinessUnitViewModel>))]
        public async Task<IActionResult> GetBusinessUnitsAsync(CancellationToken cancellationToken)
            => Ok(await _businessUnitApiService.GetBusinessUnitsAsync(cancellationToken));

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new Business Unit")]
        [SwaggerResponse(StatusCodes.Status200OK, "Created Business Unit", typeof(BusinessUnitViewModel))]
        public async Task<IActionResult> CreateBusinessUnitAsync(
            [FromBody, SwaggerRequestBody("Business unit payload", Required = true)] CreateBusinessUnitCommand command,
            CancellationToken cancellationToken)
            => Ok(await _businessUnitApiService.CreateBusinessUnitAsync(command, cancellationToken));

        [HttpPut]
        [SwaggerOperation(Summary = "Updates an existing Business Unit")]
        [SwaggerResponse(StatusCodes.Status200OK, "Updated Business Unit", typeof(BusinessUnitViewModel))]
        public async Task<IActionResult> UpdateBusinessUnitAsync(
            [FromBody, SwaggerRequestBody("Business unit payload")] UpdateBusinessUnitCommand command,
            CancellationToken cancellationToken)
            => Ok(await _businessUnitApiService.UpdateBusinessUnitAsync(command, cancellationToken));
    }
}
