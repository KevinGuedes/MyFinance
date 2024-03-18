using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Abstractions.ApiServices;
using MyFinance.Application.Common.ApiResponses;
using MyFinance.Application.UseCases.Users.Commands.RegisterUser;
using MyFinance.Application.UseCases.Users.Commands.SignIn;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[SwaggerTag("Manages user related operations")]
public class UserController(IUserService userApiService) : ApiController
{
    private readonly IUserService _userService = userApiService;

    [AllowAnonymous]
    [HttpPost]
    [SwaggerOperation(Summary = "Registers a new User")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "User successfully created")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(BadRequestResponse))]
    public async Task<IActionResult> RegisterUserAsync(
        [FromBody] [SwaggerRequestBody("User's payload", Required = true)]
        RegisterUserCommand command,
        CancellationToken cancellationToken)
        => ProcessResult(await _userService.RegisterUserAsync(command, cancellationToken));

    [AllowAnonymous]
    [HttpPost("SignIn")]
    [SwaggerOperation(Summary = "Signs in an existing User")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "User successfully signed in")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials", typeof(UnauthorizedResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found", typeof(EntityNotFoundResponse))]
    public async Task<IActionResult> SignInAsync(
        [FromBody] [SwaggerRequestBody("User's sign in credentials", Required = true)]
        SignInCommand command,
        CancellationToken cancellationToken)
        => ProcessResult(await _userService.SignInAsync(command, cancellationToken));

    [HttpPost("SignOut")]
    [SwaggerOperation(Summary = "Signs out an existing User")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "User successfully signed out")]
    public async Task<IActionResult> SignOutAsync(CancellationToken cancellationToken)
        => ProcessResult(await _userService.SignOutAsync(cancellationToken));
}