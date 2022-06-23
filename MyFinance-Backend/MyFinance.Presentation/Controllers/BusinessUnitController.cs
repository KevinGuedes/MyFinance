using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.BusinessUnits.ApiService;
using MyFinance.Application.BusinessUnits.ViewModels;

namespace MyFinance.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessUnitController : ApiController
    {
        private readonly IBusinessUnitApiService _businessUnitApiService;

        public BusinessUnitController(IBusinessUnitApiService businessUnitApiService)
            => _businessUnitApiService = businessUnitApiService;

        [HttpPost]
        public async Task<IActionResult> CreateBusinessUnitAsync(
            BusinessUnitViewModel businessUnitViewModel,
            CancellationToken cancellationToken)
        {
            var result = await _businessUnitApiService.CreateBusinessUnitAsync(businessUnitViewModel, cancellationToken);
            return SuccessResponse(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetBusinessUnits(CancellationToken cancellationToken)
        {
            var result = await _businessUnitApiService.GetBusinessUnitsAsync(cancellationToken);
            return SuccessResponse(result);
        }

        [HttpDelete("{businessUnitId:guid}")]
        public async Task<IActionResult> RemoveBusinessUnitbyId(Guid businessUnitId, CancellationToken cancellationToken)
        {
            await _businessUnitApiService.RemoveBusinessUnitByIdAsync(businessUnitId, cancellationToken);
            return SuccessResponse();
        }
    }
}
