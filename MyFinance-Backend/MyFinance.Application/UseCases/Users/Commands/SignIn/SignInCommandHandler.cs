using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.Users.Commands.SignIn;

public sealed class SignInCommandHandler(
    ILogger<SignInCommandHandler> logger,
    IUserRepository userRepository,
    IAuthService authService,
    IPasswordHasher passwordHasher) : ICommandHandler<SignInCommand>
{
    private readonly IAuthService _authService = authService;
    private readonly ILogger<SignInCommandHandler> _logger = logger;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null)
            return HandleInvalidCredentials();

        var isPasswordValid = _passwordHasher.VerifyPassword(request.PlainTextPassword, user.PasswordHash);
        if (!isPasswordValid)
            return HandleInvalidCredentials();

        _logger.LogInformation("Signing in user");
        await _authService.SignInAsync(user);
        _logger.LogInformation("User successfully signed in");

        return Result.Ok();
    }

    private Result HandleInvalidCredentials()
    {
        _logger.LogInformation("Invalid credentials");
        var unauthorizedError = new UnauthorizedError("Invalid email or password");
        return Result.Fail(unauthorizedError);
    }
}