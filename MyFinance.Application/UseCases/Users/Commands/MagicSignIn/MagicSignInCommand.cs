using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.User.Responses;

namespace MyFinance.Application.UseCases.Users.Commands.MagicSignIn;

public sealed record MagicSignInCommand(string UrlSafeMagicSignInToken)
    : ICommand<UserResponse>
{
}
