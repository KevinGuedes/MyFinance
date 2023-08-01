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
        [HttpPost]
        [SwaggerOperation(Summary = "Registers a new Transfer")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
        public async Task<IActionResult> RegisterTransfersAsync(
        [FromBody, SwaggerRequestBody("Transfers' payload", Required = true)] RegisterTransfersCommand command,
        CancellationToken cancellationToken)
        {

        }
    }
}
