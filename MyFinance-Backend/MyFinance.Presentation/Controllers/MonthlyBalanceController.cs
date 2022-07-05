using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Generics.ApiService;
using MyFinance.Application.MonthlyBalances.ApiService;
using MyFinance.Application.MonthlyBalances.Queries.GetMonthlyBalances;
using MyFinance.Application.MonthlyBalances.ViewModels;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Read Monthly Balances")]
    public class MonthlyBalanceController : BaseApiController
    {
        private readonly IMonthlyBalanceApiService _monthlyBalanceApiService;

        public MonthlyBalanceController(IMonthlyBalanceApiService monthlyBalanceApiService)
            => _monthlyBalanceApiService = monthlyBalanceApiService;

        [HttpGet("{businessUnitId:guid}")]
        [SwaggerOperation(Summary = "Lists all existing Monthly Balances according to query parameters.")]
        [SwaggerResponse(StatusCodes.Status200OK, "List of Monthly Balances", typeof(IEnumerable<MonthlyBalanceViewModel>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(InvalidRequestResponse))]
        public async Task<IActionResult> GetBusinessUnitsAsync(
            [FromRoute, SwaggerParameter("Business Unit Id", Required = true)] Guid businessUnitId,
            [FromQuery, SwaggerParameter("Requested results count", Required = true)] int count,
            [FromQuery, SwaggerParameter("Skip amount", Required = true)] int skip,
            CancellationToken cancellationToken)
            => ProcessResult(await _monthlyBalanceApiService.GetMonthlyBalancesAsync(
                businessUnitId,
                count,
                skip,
                cancellationToken));
    }
}
