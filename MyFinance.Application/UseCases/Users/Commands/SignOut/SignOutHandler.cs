using FluentResults;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;

namespace MyFinance.Application.UseCases.Users.Commands.SignOut;

internal sealed class SignOutHandler(ISignInManager signInManager) : ICommandHandler<SignOutCommand>
{
    private readonly ISignInManager _signInManager = signInManager;

    public async Task<Result> Handle(SignOutCommand command, CancellationToken cancellationToken)
    {
        await _signInManager.SignOutAsync();
        return Result.Ok();
    }
}