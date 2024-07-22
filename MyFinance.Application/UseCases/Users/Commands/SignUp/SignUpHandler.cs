using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.Users.Commands.SignUp;

internal sealed class SignUpHandler(
    ITokenProvider tokenProvider,
    IUserRepository userRepository,
    IPasswordManager passwordManager,
    IEmailSender emailSender)
    : ICommandHandler<SignUpCommand>
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IPasswordManager _passwordManager = passwordManager;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task<Result> Handle(SignUpCommand command, CancellationToken cancellationToken)
    {
        var passwordHash = _passwordManager.HashPassword(command.PlainTextPassword);
        var user = new User(command.Name, command.Email, passwordHash);

        //REMOVE THIS WHEN WE HAVE AN EMAIL SERVICE OR BETTER APP SETTINGS TO HANDLE THIS
        user.VerifyEmail();

        await _userRepository.InsertAsync(user, cancellationToken);

        var urlSafeConfirmRegistrationToken
            = _tokenProvider.CreateUrlSafeConfirmRegistrationToken(user.Id);

        await _emailSender.SendConfirmRegistrationEmailAsync(
            user.Email,
            urlSafeConfirmRegistrationToken,
            cancellationToken);

        return Result.Ok();
    }
}