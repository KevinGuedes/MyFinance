using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.ApiResponses;
using MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfers;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Generate Summary Spreadsheets for Business Units and Monthly Balances")]
    public class SummaryController : BaseController
    {
        [HttpPost("BusinessUnit/{id:guid}")]
        [SwaggerOperation(Summary = "Generate the Summary Spreadsheet for the given Business Unit")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Business Unit not found", typeof(EntityNotFoundResponse))]
        public IActionResult GenerateBusinessUnitSummary(
            [FromRoute, SwaggerRequestBody("Business Unit Id", Required = true)] Guid id, CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpPost("MonthlyBalance/{id:guid}")]
        [SwaggerOperation(Summary = "Generate the Summary Spreadsheet for the given Monthly Balance")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Monthly Balance not found", typeof(EntityNotFoundResponse))]
        public IActionResult GenerateMonthlyBalanceSummaryAsync(
            [FromRoute, SwaggerRequestBody("Monthly Balance Id", Required = true)] Guid id, CancellationToken cancellationToken)
        {
            return Ok();
        }
    }
}
