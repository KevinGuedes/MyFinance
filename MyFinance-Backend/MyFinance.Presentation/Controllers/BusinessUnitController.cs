using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.BusinessUnits.ApiService;
using MyFinance.Application.BusinessUnits.Commands.CreateBusinessUnit;
using MyFinance.Application.BusinessUnits.Commands.UpdateBusinessUnit;

namespace MyFinance.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessUnitController : ControllerBase
    {
        private readonly IBusinessUnitApiService _businessUnitApiService;

        public BusinessUnitController(IBusinessUnitApiService businessUnitApiService)
            => _businessUnitApiService = businessUnitApiService;

        [HttpGet]
        public async Task<IActionResult> GetBusinessUnitsAsync(CancellationToken cancellationToken)
            => Ok(await _businessUnitApiService.GetBusinessUnitsAsync(cancellationToken));

        [HttpPost]
        public async Task<IActionResult> CreateBusinessUnitAsync(CreateBusinessUnitCommand command, CancellationToken cancellationToken)
            => Ok(await _businessUnitApiService.CreateBusinessUnitAsync(command, cancellationToken));

        [HttpPut]
        public async Task<IActionResult> UpdateBusinessUnitAsync(
            UpdateBusinessUnitCommand command, 
            CancellationToken cancellationToken)
            => Ok(await _businessUnitApiService.UpdateBusinessUnitAsync(command, cancellationToken));
    }
}
