using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;

namespace MyFinance.Application.UseCases.Users.Commands.SendMagicSignInEmail;

internal sealed class SendMagicSignInEmailHandler(
    ITokenProvider tokenProvider,
    IUserRepository userRepository,
    IEmailSender emailSender)
    : ICommandHandler<SendMagicSignInEmailCommand>
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task<Result> Handle(SendMagicSignInEmailCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);

        if (user is null)
            return Result.Ok();

        if (!user.IsEmailVerified)
            return Result.Ok();

        var urlSafeMagicSignInToken = _tokenProvider.CreateUrlSafeMagicSignInToken(user.Id);
        await _emailSender.SendMagicSignInEmailAsync(user.Email, urlSafeMagicSignInToken);

        return Result.Ok();
    }
}
