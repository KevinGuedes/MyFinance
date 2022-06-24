using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.MonthlyBalances.ApiService;
using MyFinance.Application.MonthlyBalances.Queries.GetRecentMonthlyBalances;

namespace MyFinance.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonthlyBalanceController : ControllerBase
    {
        private readonly IMonthlyBalanceApiService _monthlyBalanceApiService;

        public MonthlyBalanceController(IMonthlyBalanceApiService monthlyBalanceApiService)
            => _monthlyBalanceApiService = monthlyBalanceApiService;

        [HttpGet]
        public async Task<IActionResult> GetBusinessUnitsAsync(GetMonthlyBalancesQuery query, CancellationToken cancellationToken)
            => Ok(await _monthlyBalanceApiService.GetMonthlyBalancesAsync(query, cancellationToken));
    }
}
