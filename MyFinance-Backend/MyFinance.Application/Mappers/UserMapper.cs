using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.UseCases.Users.Commands.CreateMagicSignInToken;
using MyFinance.Application.UseCases.Users.Commands.MagicSignIn;
using MyFinance.Application.UseCases.Users.Commands.RegisterUser;
using MyFinance.Application.UseCases.Users.Commands.SignIn;
using MyFinance.Application.UseCases.Users.Commands.UpdatePassword;
using MyFinance.Contracts.User.Requests;
using MyFinance.Contracts.User.Responses;

namespace MyFinance.Application.Mappers;

public static class UserMapper
{
    public static class RTC
    {
        public static RegisterUserCommand Map(RegisterUserRequest request)
            => new(
                request.Name,
                request.Email,
                request.PlainTextPassword,
                request.PlainTextPasswordConfirmation);

        public static SignInCommand Map(SignInRequest request)
            => new(request.Email, request.PlainTextPassword);

        public static UpdatePasswordCommand Map(UpdatePasswordRequest request)
            => new(
                request.PlainTextCurrentPassword,
                request.PlainTextNewPassword,
                request.PlainTextNewPasswordConfirmation);

        public static CreateMagicSignInTokenCommand Map(CreateMagicSignInTokenRequest request)
            => new(request.Email);

        public static MagicSignInCommand Map(MagicSignInRequest request)
            => new(request.Token);
    }

    public static class ETR
    {
        public static TooManyFailedSignInAttemptsResponse Map(
            ProblemDetails problemDetails,
            TooManyFailedSignInAttemptsError tooManyFailedSignInAttemptsError)
            => new(problemDetails, tooManyFailedSignInAttemptsError.LockoutEndOnUtc);
    }
}