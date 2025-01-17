﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Application.UseCases.Users.Commands.SignOut;
using MyFinance.Application.UseCases.Users.Commands.SignOutFromAllDevices;
using MyFinance.Application.UseCases.Users.Queries;
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
        SignUpRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(UserMapper.RTC.Map(request), cancellationToken));

    [AllowAnonymous]
    [HttpPost("ConfirmRegistration")]
    [SwaggerOperation(
        Summary = "Confirms the User's registration",
        Description = "Step required to activate user's account")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "User successfully confirmed")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    public async Task<IActionResult> ConfirmRegistrationAsync(
        [FromBody][SwaggerRequestBody("Confirm registration payload", Required = true)]
        ConfirmRegistrationRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(UserMapper.RTC.Map(request), cancellationToken));

    [AllowAnonymous]
    [HttpPost("ResendConfirmRegistrationEmail")]
    [SwaggerOperation(
        Summary = "Resends the User's confirm registration email",
        Description = "If the email provided is from an existing user, the confirm registration email will be sent")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Confirm registration email successfully sent")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    public async Task<IActionResult> ResendConfirmRegistrationEmailAsync(
       [FromBody][SwaggerRequestBody("Resend confirm registration email payload", Required = true)]
       ResendConfirmRegistrationEmailRequest request,
       CancellationToken cancellationToken)
       => ProcessResult(await _mediator.Send(UserMapper.RTC.Map(request), cancellationToken));

    [AllowAnonymous]
    [HttpPost("SignIn")]
    [SwaggerOperation(
        Summary = "Signs in an existing User",
        Description = "Password sign in for users who have already confirmed their email",
        Tags = ["User"])]
    [SwaggerResponse(StatusCodes.Status200OK, "User successfully signed in", typeof(UserResponse))]
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

    [AllowAnonymous]
    [HttpPost("SendMagicSignInEmail")]
    [SwaggerOperation(
        Summary = "Sends an email to the user with a link for magic sign in",
        Description = "If the email provided is from an existing user, the magic sign in email will be sent")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Email successfully sent to user's email")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    public async Task<IActionResult> SendMagicSignInEmailAsync(
        [FromBody] [SwaggerRequestBody("Create magic sign in token payload", Required = true)]
        SendMagicSignInEmailRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(UserMapper.RTC.Map(request), cancellationToken));

    [AllowAnonymous]
    [HttpPost("MagicSignIn")]
    [SwaggerOperation(
        Summary = "Magically signs in an existing User",
        Description = "Uses the magic sign in token to sign in the user")]
    [SwaggerResponse(StatusCodes.Status200OK, "User successfully signed in", typeof(UserResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid token", typeof(ProblemResponse))]
    public async Task<IActionResult> MagicSignInAsync(
        [FromBody] [SwaggerRequestBody("Magic sign in payload", Required = true)]
        MagicSignInRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(UserMapper.RTC.Map(request), cancellationToken));

    [AllowAnonymous]
    [HttpPost("SendResetPasswordEmail")]
    [SwaggerOperation(Summary = "Sends an email to the user with a link to reset the password")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Email successfully sent to user's email")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    public async Task<IActionResult> SendResetPasswordEmailAsync(
        [FromBody] [SwaggerRequestBody("Send reset password email payload", Required = true)]
        SendResetPasswordEmailRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(UserMapper.RTC.Map(request), cancellationToken));

    [AllowAnonymous]
    [HttpPost("ResetPassword")]
    [SwaggerOperation(Summary = "Resets the User's password")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Password successfully reseted")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid token", typeof(ProblemResponse))]
    public async Task<IActionResult> ResetPasswordAsync(
        [FromBody][SwaggerRequestBody("Reset password payload", Required = true)]
        ResetPasswordRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(UserMapper.RTC.Map(request), cancellationToken));

    [HttpGet("Info")]
    [SwaggerOperation(Summary = "Gets the current user basic information")]
    [SwaggerResponse(StatusCodes.Status200OK, "User's basic information", typeof(UserResponse))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not signed in", typeof(ProblemResponse))]
    public async Task<IActionResult> GetUserInfoAsync(CancellationToken cancellationToken)
      => ProcessResult(await _mediator.Send(new GetUserInfoQuery(), cancellationToken));

    [HttpPatch("UpdatePassword")]
    [SwaggerOperation(Summary = "Updates the User's password")]
    [SwaggerResponse(StatusCodes.Status200OK, "Password successfully updated")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(ProblemResponse))]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "The new password is similar to the current password", typeof(ProblemResponse))]
    public async Task<IActionResult> UpdatePasswordAsync(
        [FromBody] [SwaggerRequestBody("Update password payload", Required = true)]
        UpdatePasswordRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(UserMapper.RTC.Map(request), cancellationToken));

    [HttpPost("SignOut")]
    [SwaggerOperation(Summary = "Signs out an existing User")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "User successfully signed out")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(ProblemResponse))]
    public async Task<IActionResult> SignOutAsync(CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(new SignOutCommand(), cancellationToken));

    [HttpPost("SignOutFromAllDevices")]
    [SwaggerOperation(Summary = "Signs out an existing User from all devices")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "User successfully signed out from all devices")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(ProblemResponse))]
    public async Task<IActionResult> SignOutFromAllDevicesAsync(CancellationToken cancellationToken)
       => ProcessResult(await _mediator.Send(new SignOutFromAllDevicesCommand(), cancellationToken));

    private ObjectResult BuildTooManyFailedSignInAttemptsResponse(TooManyFailedSignInAttemptsError tooManyFailedSignInAttemptsError)
    {
        HttpContext.Response.Headers.RetryAfter
            = tooManyFailedSignInAttemptsError.LockoutEndOnUtc.ToString("R");

        var statusCode = StatusCodes.Status429TooManyRequests;

        var problemDetails = ProblemDetailsFactory.CreateProblemDetails(
            HttpContext,
            statusCode: statusCode,
            detail: "Failed sign in attempts threshold reached",
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