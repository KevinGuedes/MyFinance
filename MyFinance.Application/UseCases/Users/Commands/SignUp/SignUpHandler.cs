using FluentResults;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.Users.Commands.SignUp;

internal sealed class SignUpHandler(
    ITokenProvider tokenProvider,
    IPasswordManager passwordManager,
    IEmailSender emailSender,
    IMyFinanceDbContext myFinanceDbContext)
    : ICommandHandler<SignUpCommand>
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IPasswordManager _passwordManager = passwordManager;
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task<Result> Handle(SignUpCommand command, CancellationToken cancellationToken)
    {
        var passwordHash = _passwordManager.HashPassword(command.PlainTextPassword);
        var user = new User(command.Name, command.Email, passwordHash);

        await _myFinanceDbContext.Users.AddAsync(user, cancellationToken);

        if (_emailSender.IsEmailConfirmationEnabled)
        {
            var urlSafeConfirmRegistrationToken
                = _tokenProvider.CreateUrlSafeConfirmRegistrationToken(user.Id);

            await _emailSender.SendConfirmRegistrationEmailAsync(
                user.Email,
                urlSafeConfirmRegistrationToken,
                cancellationToken);
        }

        return Result.Ok();
    }
}