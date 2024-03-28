using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Mappers;
using MyFinance.Application.UseCases.Users.Commands.SignOut;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.User.Requests;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[SwaggerTag("User management")]
public class UserController(IMediator mediator) : ApiController(mediator)
{
    [AllowAnonymous]
    [HttpPost]
    [SwaggerOperation(Summary = "Registers a new User")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "User successfully created")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    public async Task<IActionResult> RegisterUserAsync(
        [FromBody] [SwaggerRequestBody("User's payload", Required = true)]
        RegisterUserRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(UserMapper.RTC.Map(request), cancellationToken));

    [AllowAnonymous]
    [HttpPost("SignIn")]
    [SwaggerOperation(Summary = "Signs in an existing User")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "User successfully signed in")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials", typeof(ProblemResponse))]
    public async Task<IActionResult> SignInAsync(
        [FromBody] [SwaggerRequestBody("User's sign in credentials", Required = true)]
        SignInRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(UserMapper.RTC.Map(request), cancellationToken));

    [HttpPost("SignOut")]
    [SwaggerOperation(Summary = "Signs out an existing User")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "User successfully signed out")]
    public async Task<IActionResult> SignOutAsync(CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(new SignOutCommand(), cancellationToken));
}