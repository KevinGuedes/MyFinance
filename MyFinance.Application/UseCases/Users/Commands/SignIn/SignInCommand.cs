using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.User.Requests;
using MyFinance.Contracts.User.Responses;

namespace MyFinance.Application.UseCases.Users.Commands.SignIn;

public sealed class SignInCommand(SignInRequest request) : ICommand<UserInfoResponse>
{
    public string Email { get; init; } = request.Email;
    public string PlainTextPassword { get; init; } = request.PlainTextPassword;
}