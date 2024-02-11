using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Services.Auth;
using MyFinance.Infra.Services.PasswordHasher;

namespace MyFinance.Application.UseCases.Users.Commands.SignIn;

public sealed class SignInCommandHandler(
    ILogger<SignInCommandHandler> logger, 
    IUserRepository userRepository, 
    IAuthService authService,
    IPasswordHasher passwordHasher) : ICommandHandler<SignInCommand>
{
    private readonly ILogger<SignInCommandHandler> _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthService _authService = authService;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<Result> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null)
        {
            _logger.LogWarning("User not registered");
            var errorMessage = string.Format("User not registered");
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var isPasswordValid = _passwordHasher.VerifyPassword(request.PlainTextPassword, user.PasswordHash);
        if (!isPasswordValid)
        {
            _logger.LogWarning("Invalid credentials");
            var errorMessage = string.Format("Invalid credentials");
            var unauthorizedError = new UnauthorizedError(errorMessage);
            return Result.Fail(unauthorizedError);
        }

        _logger.LogInformation("Signing in user");
        await _authService.SignInAsync(user.Email);
        _logger.LogInformation("User successfully signed in");

        return Result.Ok();
    }
}
