using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.UseCases.Users.Commands.ConfirmRegistration;
using MyFinance.Application.UseCases.Users.Commands.MagicSignIn;
using MyFinance.Application.UseCases.Users.Commands.ResendConfirmRegistrationEmail;
using MyFinance.Application.UseCases.Users.Commands.ResetPassword;
using MyFinance.Application.UseCases.Users.Commands.SendMagicSignInEmail;
using MyFinance.Application.UseCases.Users.Commands.SendResetPasswordEmail;
using MyFinance.Application.UseCases.Users.Commands.SignIn;
using MyFinance.Application.UseCases.Users.Commands.SignOut;
using MyFinance.Application.UseCases.Users.Commands.SignOutFromAllDevices;
using MyFinance.Application.UseCases.Users.Commands.SignUp;
using MyFinance.Application.UseCases.Users.Commands.UpdatePassword;
using MyFinance.Application.UseCases.Users.Queries.GetUserInfo;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.User.Requests;
using MyFinance.Contracts.User.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[SwaggerTag("User management")]
public class UserController(ISender sender) : ApiController(sender)
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
    {
        var result = await _sender.Send(new SignUpCommand(request), cancellationToken);
        return ProcessResult(result);
    }

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
    {
        var result = await _sender.Send(new ConfirmRegistrationCommand(request), cancellationToken);
        return ProcessResult(result);
    }

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
    {
        var result = await _sender.Send(new ResendConfirmRegistrationEmailCommand(request), cancellationToken);
        return ProcessResult(result);
    }

    [AllowAnonymous]
    [HttpPost("SignIn")]
    [SwaggerOperation(
        Summary = "Signs in an existing User",
        Description = "Password sign in for users who have already confirmed their email",
        Tags = ["User"])]
    [SwaggerResponse(StatusCodes.Status200OK, "User successfully signed in", typeof(UserInfoResponse))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials", typeof(ProblemResponse))]
    [SwaggerResponse(StatusCodes.Status429TooManyRequests, "Too many failed sign in attempts", typeof(TooManyFailedSignInAttemptsResponse))]
    public async Task<IActionResult> SignInAsync(
        [FromBody] [SwaggerRequestBody("User's sign in credentials", Required = true)]
        SignInRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new SignInCommand(request), cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        var error = result.Errors.FirstOrDefault();

        if (error is TooManyFailedSignInAttemptsError tooManyFailedSignInAttemptsError)
        {
            HttpContext.Response.Headers.RetryAfter
                = tooManyFailedSignInAttemptsError.LockoutEndOnUtc.ToString("R");

            var problemDetails = ProblemDetailsFactory.CreateProblemDetails(
                HttpContext,
                statusCode: StatusCodes.Status429TooManyRequests,
                detail: "Failed sign in attempts threshold reached",
                instance: HttpContext.Request.Path);

            problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc6585#section-4";

            var tooManyFailedSignInAttemptsResponse = new TooManyFailedSignInAttemptsResponse(
                problemDetails,
                tooManyFailedSignInAttemptsError.LockoutEndOnUtc);

            return new ObjectResult(tooManyFailedSignInAttemptsResponse)
            {
                StatusCode = problemDetails.Status
            };
        }

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
    {
        var result = await _sender.Send(new SendMagicSignInEmailCommand(request), cancellationToken);
        return ProcessResult(result);
    }

    [AllowAnonymous]
    [HttpPost("MagicSignIn")]
    [SwaggerOperation(
        Summary = "Magically signs in an existing User",
        Description = "Uses the magic sign in token to sign in the user")]
    [SwaggerResponse(StatusCodes.Status200OK, "User successfully signed in", typeof(UserInfoResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid token", typeof(ProblemResponse))]
    public async Task<IActionResult> MagicSignInAsync(
        [FromBody] [SwaggerRequestBody("Magic sign in payload", Required = true)]
        MagicSignInRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new MagicSignInCommand(request), cancellationToken);
        return ProcessResult(result);
    }

    [AllowAnonymous]
    [HttpPost("SendResetPasswordEmail")]
    [SwaggerOperation(Summary = "Sends an email to the user with a link to reset the password")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Email successfully sent to user's email")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    public async Task<IActionResult> SendResetPasswordEmailAsync(
        [FromBody] [SwaggerRequestBody("Send reset password email payload", Required = true)]
        SendResetPasswordEmailRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new SendResetPasswordEmailCommand(request), cancellationToken);
        return ProcessResult(result);
    }

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
    {
        var result = await _sender.Send(new ResetPasswordCommand(request), cancellationToken);
        return ProcessResult(result);
    }

    [HttpGet("Info")]
    [SwaggerOperation(Summary = "Gets the current user basic information")]
    [SwaggerResponse(StatusCodes.Status200OK, "User's basic information", typeof(UserInfoResponse))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not signed in", typeof(ProblemResponse))]
    public async Task<IActionResult> GetUserInfoAsync(CancellationToken cancellationToken)
    {
        var query = new GetUserInfoQuery();
        var result = await _sender.Send(query, cancellationToken);
        return ProcessResult(result);
    }

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
    {
        var result = await _sender.Send(new UpdatePasswordCommand(request), cancellationToken);
        return ProcessResult(result);
    }

    [HttpPost("SignOut")]
    [SwaggerOperation(Summary = "Signs out an existing User")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "User successfully signed out")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(ProblemResponse))]
    public async Task<IActionResult> SignOutAsync(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new SignOutCommand(), cancellationToken);
        return ProcessResult(result);
    }

    [HttpPost("SignOutFromAllDevices")]
    [SwaggerOperation(Summary = "Signs out an existing User from all devices")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "User successfully signed out from all devices")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(ProblemResponse))]
    public async Task<IActionResult> SignOutFromAllDevicesAsync(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new SignOutFromAllDevicesCommand(), cancellationToken);
        return ProcessResult(result);
    }
}