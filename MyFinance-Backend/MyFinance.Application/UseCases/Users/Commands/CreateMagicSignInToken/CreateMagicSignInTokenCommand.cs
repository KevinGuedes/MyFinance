using MyFinance.Application.Abstractions.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.Users.Commands.CreateMagicSignInToken;

public sealed record CreateMagicSignInTokenCommand(string Email) : ICommand
{
}
