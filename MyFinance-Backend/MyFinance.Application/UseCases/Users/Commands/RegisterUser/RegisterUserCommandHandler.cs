using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Application.Services.PasswordHasher;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.Users.Commands.RegisterUser;

internal sealed class RegisterUserCommandHandler(
    ILogger<RegisterUserCommandHandler> logger,
    IUserRepository userRepository,
    IPasswordHasher passwordHasher) : ICommandHandler<RegisterUserCommand>
{
    private readonly ILogger<RegisterUserCommandHandler> _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Registering new User");
        var passwordHash = _passwordHasher.HashPassword(request.PlainTextPassword);
        var user = new User(request.Name, request.Email, passwordHash);
        _userRepository.Insert(user);
        _logger.LogInformation("User successfully registered");

        return Task.FromResult(Result.Ok());
    }
}
