using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.ApiResponses;
using MyFinance.Application.UseCases.AccountTags.ApiService;
using MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;
using MyFinance.Application.UseCases.Transfers.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Create and update Account Tags")]
    public class AccountTagController : BaseController
    {
        private readonly IAccountTagApiService _accountTagApiService;
        public AccountTagController(IAccountTagApiService accountTagApiService)
            => _accountTagApiService = accountTagApiService;

        [HttpPost]
        [SwaggerOperation(Summary = "Registers a new Account Tag")]
        [SwaggerResponse(StatusCodes.Status201Created, "Account Tag registered", typeof(TransferDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
        public async Task<IActionResult> RegisterTransfersAsync(
            [FromBody, SwaggerRequestBody("Account Tag's payload", Required = true)] CreateAccountTagCommand command,
            CancellationToken cancellationToken)
            => ProcessResult(await _accountTagApiService.CreateAccountTagAsync(command, cancellationToken), true);
    }
}
