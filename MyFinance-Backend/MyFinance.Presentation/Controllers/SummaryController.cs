using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.ApiResponses;
using MyFinance.Application.UseCases.Summary.ApiService;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Gets Summary Spreadsheets for Business Units and Monthly Balances")]
    public class SummaryController(ISummaryApiService summaryApiService) : BaseController
    {
        private const string SPREADSHEET_CONTENT_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly ISummaryApiService _summaryApiService = summaryApiService;

        [HttpGet("BusinessUnit/{id:guid}")]
        [SwaggerOperation(Summary = "Get the Summary Spreadsheet for the given Business Unit")]
        [SwaggerResponse(StatusCodes.Status200OK, "Summary Spreasheet for the given Business Unit", contentTypes: SPREADSHEET_CONTENT_TYPE)]
        [SwaggerResponse(StatusCodes.Status206PartialContent, "Summary Spreasheet for the given Business Unit", contentTypes: SPREADSHEET_CONTENT_TYPE)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid Business Unit Id", typeof(BadRequestResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Business Unit not found", typeof(EntityNotFoundResponse))]
        public async Task<IActionResult> GetBusinessUnitSummary(
            [FromRoute, SwaggerParameter("Business Unit Id", Required = true)] Guid id,
            [FromQuery, SwaggerParameter("Year", Required = true)] int year,
            CancellationToken cancellationToken)
            => ProcessFileResult(await _summaryApiService.GetBusinessUnitSummaryAsync(id, year, cancellationToken), SPREADSHEET_CONTENT_TYPE);

        [HttpGet("MonthlyBalance/{id:guid}")]
        [SwaggerOperation(Summary = "Get the Summary Spreadsheet for the given Monthly Balance")]
        [SwaggerResponse(StatusCodes.Status200OK, "Summary Spreasheet for the given Monthly Balance", contentTypes: SPREADSHEET_CONTENT_TYPE)]
        [SwaggerResponse(StatusCodes.Status206PartialContent, "Summary Spreasheet for the given Monthly Balance", contentTypes: SPREADSHEET_CONTENT_TYPE)]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Monthly Balance not found", typeof(EntityNotFoundResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid Monthly Balance Id", typeof(BadRequestResponse))]
        public async Task<IActionResult> GetMonthlyBalanceSummaryAsync(
            [FromRoute, SwaggerParameter("Monthly Balance Id", Required = true)] Guid id, CancellationToken cancellationToken)
            => ProcessFileResult(await _summaryApiService.GetMonthlyBalanceSummaryAsync(id, cancellationToken), SPREADSHEET_CONTENT_TYPE);
    }
}
