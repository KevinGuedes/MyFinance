using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.User.Responses;

namespace MyFinance.Application.UseCases.Users.Commands.SignIn;

public sealed record SignInCommand(string Email, string PlainTextPassword) : ICommand<UserInfoResponse>
{
}