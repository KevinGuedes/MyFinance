using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.UseCases.Users.Commands.ConfirmRegistration;
using MyFinance.Application.UseCases.Users.Commands.MagicSignIn;
using MyFinance.Application.UseCases.Users.Commands.ResendConfirmRegistrationEmail;
using MyFinance.Application.UseCases.Users.Commands.ResetPassword;
using MyFinance.Application.UseCases.Users.Commands.SendMagicSignInEmail;
using MyFinance.Application.UseCases.Users.Commands.SendResetPasswordEmail;
using MyFinance.Application.UseCases.Users.Commands.SignIn;
using MyFinance.Application.UseCases.Users.Commands.SignUp;
using MyFinance.Application.UseCases.Users.Commands.UpdatePassword;
using MyFinance.Contracts.User.Requests;
using MyFinance.Contracts.User.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.Mappers;

public static class UserMapper
{
    public static class RTC
    {
        public static SignUpCommand Map(SignUpRequest request)
            => new(request.Name,
                request.Email,
                request.PlainTextPassword,
                request.PlainTextPasswordConfirmation);

        public static ResendConfirmRegistrationEmailCommand Map(ResendConfirmRegistrationEmailRequest request)
            => new(request.Email);

        public static ConfirmRegistrationCommand Map(ConfirmRegistrationRequest request)
            => new(request.UrlSafeConfirmRegistrationToken);

        public static SignInCommand Map(SignInRequest request)
            => new(request.Email, request.PlainTextPassword);

        public static UpdatePasswordCommand Map(UpdatePasswordRequest request)
            => new(request.PlainTextCurrentPassword,
                request.PlainTextNewPassword,
                request.PlainTextNewPasswordConfirmation);

        public static SendMagicSignInEmailCommand Map(SendMagicSignInEmailRequest request)
            => new(request.Email);

        public static MagicSignInCommand Map(MagicSignInRequest request)
            => new(request.UrlSafeMagicSignInToken);

        public static SendResetPasswordEmailCommand Map(SendResetPasswordEmailRequest request)
            => new(request.Email);

        public static ResetPasswordCommand Map(ResetPasswordRequest request)
            => new(request.UrlSafeResetPasswordToken,
                request.PlainTextNewPassword,
                request.PlainTextNewPasswordConfirmation);
    }

    public static class ETR
    {
        public static TooManyFailedSignInAttemptsResponse Map(
            ProblemDetails problemDetails,
            TooManyFailedSignInAttemptsError tooManyFailedSignInAttemptsError)
            => new(problemDetails, tooManyFailedSignInAttemptsError.LockoutEndOnUtc);
    }
}