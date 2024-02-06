using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.ApiResponses;
using MyFinance.Application.UseCases.Users.ApiService;
using MyFinance.Application.UseCases.Users.Commands;
using MyFinance.Application.UseCases.Users.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[SwaggerTag("Manages user related operations")]
public class UserController(IUserApiService userApiService) : BaseController
{
    private readonly IUserApiService _userService = userApiService;

    [HttpPost]
    [SwaggerOperation(Summary = "Registers a new User")]
    [SwaggerResponse(StatusCodes.Status201Created, "User registered", typeof(UserDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
    public async Task<IActionResult> RegisterUserAsync(
        [FromBody, SwaggerRequestBody("User's payload", Required = true)] RegisterUserCommand command,
        CancellationToken cancellationToken)
        => ProcessResult(await _userService.RegisterUserAsync(command, cancellationToken), true);
}
