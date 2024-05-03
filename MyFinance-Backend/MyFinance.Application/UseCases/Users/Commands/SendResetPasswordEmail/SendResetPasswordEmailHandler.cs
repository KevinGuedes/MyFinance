using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.Users.Commands.SendResetPasswordEmail;

internal sealed class SendResetPasswordEmailHandler(
    ITokenProvider tokenProvider,
    IUserRepository userRepository,
    IEmailSender emailSender)
    : ICommandHandler<SendResetPasswordEmailCommand>
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task<Result> Handle(SendResetPasswordEmailCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);

        if (user is null)
            return Result.Ok();

        if (!user.IsEmailVerified)
            return Result.Ok();

        var urlSafeResetPasswordToken = _tokenProvider.CreateUrlSafeResetPasswordToken(user.Id);
        await _emailSender.SendResetPasswordEmailAsync(user.Email, urlSafeResetPasswordToken);

        return Result.Ok();
    }
}
