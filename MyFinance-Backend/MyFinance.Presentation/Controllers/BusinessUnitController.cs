using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.BusinessUnits.ApiService;
using MyFinance.Application.BusinessUnits.Commands.CreateBusinessUnit;

namespace MyFinance.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessUnitController : ControllerBase
    {
        private readonly IBusinessUnitApiService _businessUnitApiService;

        public BusinessUnitController(IBusinessUnitApiService businessUnitApiService)
            => _businessUnitApiService = businessUnitApiService;

        [HttpPost]
        public async Task<IActionResult> CreateBusinessUnitAsync(CreateBusinessUnitCommand command, CancellationToken cancellationToken)
            => Ok(await _businessUnitApiService.CreateBusinessUnitAsync(command, cancellationToken));

        [HttpGet]
        public async Task<IActionResult> GetBusinessUnits(CancellationToken cancellationToken)
            => Ok(await _businessUnitApiService.GetBusinessUnitsAsync(cancellationToken));

        [HttpDelete("{businessUnitId:guid}")]
        public async Task<IActionResult> DeleteBusinessUnitbyId(Guid businessUnitId, CancellationToken cancellationToken)
        {
            await _businessUnitApiService.DeleteBusinessUnitByIdAsync(businessUnitId, cancellationToken);
            return NoContent();
        }
    }
}
