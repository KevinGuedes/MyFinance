using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.Users.Commands.RegisterUser;

internal sealed class RegisterUserHandler(
    IUserRepository userRepository,
    IPasswordManager passwordManager)
    : ICommandHandler<RegisterUserCommand>
{
    private readonly IPasswordManager _passwordManager = passwordManager;
    private readonly IUserRepository _userRepository = userRepository;

    public Task<Result> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var passwordHash = _passwordManager.HashPassword(command.PlainTextPassword);
        var user = new User(command.Name, command.Email, passwordHash);
        _userRepository.Insert(user);

        return Task.FromResult(Result.Ok());
    }
}