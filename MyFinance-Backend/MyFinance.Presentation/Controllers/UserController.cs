using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Application.UseCases.Users.Commands.SignOut;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.User.Requests;
using MyFinance.Contracts.User.Responses;
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
    [SwaggerResponse(StatusCodes.Status200OK, "User successfully signed in", typeof(SignInResponse))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials", typeof(ProblemResponse))]
    [SwaggerResponse(StatusCodes.Status429TooManyRequests, "Too many failed sign in attempts", typeof(TooManyFailedSignInAttemptsResponse))]
    public async Task<IActionResult> SignInAsync(
        [FromBody] [SwaggerRequestBody("User's sign in credentials", Required = true)]
        SignInRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(UserMapper.RTC.Map(request), cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        var error = result.Errors.FirstOrDefault();

        if (error is TooManyFailedSignInAttemptsError tooManyFailedSignInAttemptsError)
            return BuildTooManyFailedSignInAttemptsResponse(tooManyFailedSignInAttemptsError);

        return HandleFailureResult(error);
    }

    [HttpPost("SignOut")]
    [SwaggerOperation(Summary = "Signs out an existing User")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "User successfully signed out")]
    public async Task<IActionResult> SignOutAsync(CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(new SignOutCommand(), cancellationToken));

    private ObjectResult BuildTooManyFailedSignInAttemptsResponse(TooManyFailedSignInAttemptsError tooManyFailedSignInAttemptsError)
    {
        HttpContext.Response.Headers.RetryAfter 
            = tooManyFailedSignInAttemptsError.LockoutEndOnUtc.ToString("R");

        var statusCode = StatusCodes.Status429TooManyRequests;
       
        var problemDetails = ProblemDetailsFactory.CreateProblemDetails(
            HttpContext,
            statusCode: statusCode,
            detail: "Service(s) currently unhealthy",
            instance: HttpContext.Request.Path);

        problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc6585#section-4";
        
        var tooManyFailedSignInAttemptsResponse 
            = UserMapper.ETR.Map(problemDetails, tooManyFailedSignInAttemptsError);

        return new(tooManyFailedSignInAttemptsResponse)
        {
            StatusCode = problemDetails.Status
        };
    }
}