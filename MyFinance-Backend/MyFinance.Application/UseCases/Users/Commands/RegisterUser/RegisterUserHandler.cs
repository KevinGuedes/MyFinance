using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.Users.Commands.RegisterUser;

internal sealed class RegisterUserHandler(
    ITokenProvider tokenProvider,
    IUserRepository userRepository,
    IPasswordManager passwordManager,
    IEmailSender emailSender)
    : ICommandHandler<RegisterUserCommand>
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IPasswordManager _passwordManager = passwordManager;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task<Result> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var passwordHash = _passwordManager.HashPassword(command.PlainTextPassword);
        var user = new User(command.Name, command.Email, passwordHash);
        _userRepository.Insert(user);

        var urlSafeConfirmRegistrationToken
            = _tokenProvider.CreateUrlSafeConfirmRegistrationToken(user.Id);

        await _emailSender.SendConfirmRegistrationEmailAsync(
            user.Email,
            urlSafeConfirmRegistrationToken,
            cancellationToken);

        return Result.Ok();
    }
}