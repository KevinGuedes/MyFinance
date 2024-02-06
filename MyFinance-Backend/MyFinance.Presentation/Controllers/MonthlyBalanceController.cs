using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.ApiResponses;
using MyFinance.Application.UseCases.MonthlyBalances.ApiService;
using MyFinance.Application.UseCases.MonthlyBalances.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[SwaggerTag("Read Monthly Balances")]
public class MonthlyBalanceController(IMonthlyBalanceApiService monthlyBalanceApiService) : BaseController
{
    private readonly IMonthlyBalanceApiService _monthlyBalanceApiService = monthlyBalanceApiService;

    [HttpGet]
    [SwaggerOperation(Summary = "Lists all existing Monthly Balances according to query parameters")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of Monthly Balances", typeof(IEnumerable<MonthlyBalanceDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid query parameters", typeof(BadRequestResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Business Unit not found", typeof(EntityNotFoundResponse))]
    public async Task<IActionResult> GetBusinessUnitsAsync(
        [FromQuery, SwaggerParameter("Business Unit Id", Required = true)] Guid businessUnitId,
        [FromQuery, SwaggerParameter("Page number", Required = true)] int page,
        [FromQuery, SwaggerParameter("Units per page", Required = true)] int pageSize,
        CancellationToken cancellationToken)
        => ProcessResult(await _monthlyBalanceApiService.GetMonthlyBalancesAsync(
            businessUnitId,
            page,
            pageSize,
            cancellationToken));
}
