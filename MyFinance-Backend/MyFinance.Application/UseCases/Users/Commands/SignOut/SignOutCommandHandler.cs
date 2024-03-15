using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;

namespace MyFinance.Application.UseCases.Users.Commands.SignOut;

public sealed class SignOutCommandHandler(
    ILogger<SignOutCommandHandler> logger,
    IAuthService authService) : ICommandHandler<SignOutCommand>
{
    private readonly ILogger<SignOutCommandHandler> _logger = logger;
    private readonly IAuthService _authService = authService;

    public async Task<Result> Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Signing out user");
        await _authService.SignOutAsync();
        _logger.LogInformation("User successfully signed out");

        return Result.Ok();
    }
}
