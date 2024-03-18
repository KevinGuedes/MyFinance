using MyFinance.Application.UseCases.Users.Commands.RegisterUser;
using MyFinance.Application.UseCases.Users.Commands.SignIn;
using MyFinance.Contracts.User.Requests;

namespace MyFinance.Application.Mappers;

public static class UserMapper
{
    public static class RTC
    {
        public static RegisterUserCommand Map(RegisterUserRequest request)
            => new(request.Name, request.Email, request.PlainTextPassword, request.PlainTextConfirmationPassword);

        public static SignInCommand Map(SignInRequest request)
            => new(request.Email, request.PlainTextPassword);
    }
}
