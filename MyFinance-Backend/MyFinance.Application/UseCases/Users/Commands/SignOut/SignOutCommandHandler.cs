using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;

namespace MyFinance.Application.UseCases.Users.Commands.SignOut;

public sealed class SignOutCommandHandler(IAuthService authService) : ICommandHandler<SignOutCommand>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result> Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        await _authService.SignOutAsync();
        return Result.Ok();
    }
}