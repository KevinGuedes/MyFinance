using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.ApiResponses;
using MyFinance.Application.UseCases.AccountTags.ApiService;
using MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;
using MyFinance.Application.UseCases.AccountTags.Commands.UpdateAccountTag;
using MyFinance.Application.UseCases.AccountTags.DTOs;
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
        [SwaggerResponse(StatusCodes.Status201Created, "Account Tag registered", typeof(AccountTagDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
        public async Task<IActionResult> RegisterTransfersAsync(
            [FromBody, SwaggerRequestBody("Account Tag's payload", Required = true)] CreateAccountTagCommand command,
            CancellationToken cancellationToken)
            => ProcessResult(await _accountTagApiService.CreateAccountTagAsync(command, cancellationToken), true);

        [HttpPut]
        [SwaggerOperation(Summary = "Updates an existing Account Tag")]
        [SwaggerResponse(StatusCodes.Status200OK, "Updated Account Tag", typeof(AccountTagDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Account Tag not found", typeof(EntityNotFoundResponse))]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Server currently unable to process the Account Tag", typeof(UnprocessableEntityResponse))]
        public async Task<IActionResult> UpdateBusinessUnitAsync(
            [FromBody, SwaggerRequestBody("Business Unit payload", Required = true)] UpdateAccountTagCommand command,
            CancellationToken cancellationToken)
            => ProcessResult(await _accountTagApiService.UpdateAccountTagAsync(command, cancellationToken));
    }
}
