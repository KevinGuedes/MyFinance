using FluentResults;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;

namespace MyFinance.Application.UseCases.Users.Commands.SignOut;

public sealed class SignOutCommandHandler(ISignInManager signInManager) : ICommandHandler<SignOutCommand>
{
    private readonly ISignInManager _signInManager = signInManager;

    public async Task<Result> Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        await _signInManager.SignOutAsync();
        return Result.Ok();
    }
}