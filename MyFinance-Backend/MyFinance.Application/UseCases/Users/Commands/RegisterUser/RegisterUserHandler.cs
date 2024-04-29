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

        var (hasEmailBeenSent, exception) = await _emailSender.SendMagicSignInEmailAsync(
            user.Email,
            urlSafeConfirmRegistrationToken);

        if (!hasEmailBeenSent)
        {
            var errorMessage = "The magic confirm registration email could not be sent. Please try again later.";
            var internalServerError = new InternalServerError(errorMessage).CausedBy(exception);
            return Result.Fail(internalServerError);
        }

        return Result.Ok();
    }
}