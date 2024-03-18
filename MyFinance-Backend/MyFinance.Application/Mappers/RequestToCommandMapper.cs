using MyFinance.Application.UseCases.Users.Commands.RegisterUser;
using MyFinance.Contracts.User;

namespace MyFinance.Application.Mappers;

public static class RequestToCommandMapper
{
    public static RegisterUserCommand RegisterUserRequestToCommand(RegisterUserRequest request)
        => new(request.Name,
            request.Email,
            request.PlainTextPassword,
            request.PlainTextConfirmationPassword);
}
