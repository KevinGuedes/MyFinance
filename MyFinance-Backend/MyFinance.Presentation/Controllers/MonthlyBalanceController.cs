using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.ApiService;
using MyFinance.Application.MonthlyBalances.ApiService;
using MyFinance.Application.MonthlyBalances.Queries.GetMonthlyBalances;
using MyFinance.Application.MonthlyBalances.ViewModels;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[SwaggerTag("Read Monthly Balances")]
public class MonthlyBalanceController : BaseController
{
    private readonly IMonthlyBalanceApiService _monthlyBalanceApiService;

    public MonthlyBalanceController(IMonthlyBalanceApiService monthlyBalanceApiService)
        => _monthlyBalanceApiService = monthlyBalanceApiService;

    [HttpGet]
    [SwaggerOperation(Summary = "Lists all existing Monthly Balances according to query parameters.")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of Monthly Balances", typeof(IEnumerable<MonthlyBalanceViewModel>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid query parameters", typeof(BadRequestResponse))]
    public async Task<IActionResult> GetBusinessUnitsAsync(
        [FromQuery, SwaggerParameter("Business Unit Id", Required = true)] Guid businessUnitId,
        [FromQuery, SwaggerParameter("Amount of Monthly Balancers to take", Required = true)] int take,
        [FromQuery, SwaggerParameter("Skip amount", Required = true)] int skip,
        CancellationToken cancellationToken)
        => ProcessResult(await _monthlyBalanceApiService.GetMonthlyBalancesAsync(
            businessUnitId,
            take,
            skip,
            cancellationToken));
}
