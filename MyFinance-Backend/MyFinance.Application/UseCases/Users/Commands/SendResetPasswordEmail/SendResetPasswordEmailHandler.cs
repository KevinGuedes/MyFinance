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
        {
            var random = new Random();
            var randomNumberBetween0And2Inclusive = Math.Round(random.NextDouble() * 2, 3);
            var delayInSeconds = Convert.ToInt32(randomNumberBetween0And2Inclusive * 1000);
            await Task.Delay(delayInSeconds, cancellationToken);
            return Result.Ok();
        }

        var urlSafeResetPasswordToken
            = _tokenProvider.CreateUrlSafeResetPasswordToken(user.Id);

        var (hasEmailBeenSent, exception) = await _emailSender.SendResetPasswordEmailAsync(
            user.Email,
            urlSafeResetPasswordToken);

        if (!hasEmailBeenSent)
        {
            var errorMessage = "The reset password email could not be sent. Please try again later.";
            var internalServerError = new InternalServerError(errorMessage).CausedBy(exception);
            return Result.Fail(internalServerError);
        }

        return Result.Ok();
    }
}
