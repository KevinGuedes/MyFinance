using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.UseCases.MonthlyBalances.Queries.GetMonthlyBalances;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.MonthlyBalance.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[SwaggerTag("Read Monthly Balances")]
[SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
public class MonthlyBalanceController(IMediator mediator) : ApiController(mediator)
{
    [HttpGet]
    [SwaggerOperation(Summary = "Lists all existing Monthly Balances according to query parameters")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of Monthly Balances", typeof(Paginated<MonthlyBalanceResponse>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid query parameters", typeof(ValidationProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Business Unit not found", typeof(ProblemDetails))]
    public async Task<IActionResult> GetBusinessUnitsAsync(
        [FromQuery] [SwaggerParameter("Business Unit Id", Required = true)]
        Guid businessUnitId,
        [FromQuery] [SwaggerParameter("Page number", Required = true)]
        int pageNumber,
        [FromQuery] [SwaggerParameter("Units per page", Required = true)]
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = new GetMonthlyBalancesQuery(businessUnitId, pageNumber, pageSize);
        return ProcessResult(await _mediator.Send(query, cancellationToken));
    }
}