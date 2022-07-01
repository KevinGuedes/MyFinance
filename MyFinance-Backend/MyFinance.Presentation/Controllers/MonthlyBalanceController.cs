using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Generics.ApiService;
using MyFinance.Application.MonthlyBalances.ApiService;
using MyFinance.Application.MonthlyBalances.Queries.GetRecentMonthlyBalances;
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

        [HttpGet]
        [SwaggerOperation(Summary = "Lists all existing Monthly Balances according to query parameters.")]
        [SwaggerResponse(StatusCodes.Status200OK, "List of Monthly Balances", typeof(IEnumerable<MonthlyBalanceViewModel>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ApiInvalidDataResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Backend went rogue", typeof(ApiErrorResponse))]
        public async Task<IActionResult> GetBusinessUnitsAsync(
            [FromQuery, SwaggerParameter("Query parameters", Required = true)] GetMonthlyBalancesQuery query,
            CancellationToken cancellationToken)
            => ProcessResult(await _monthlyBalanceApiService.GetMonthlyBalancesAsync(query, cancellationToken));
    }
}
