using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;

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
        {
            var random = new Random();
            var randomNumberBetween0And2Inclusive = Math.Round(random.NextDouble() * 2, 3);
            var delayInSeconds = Convert.ToInt32(randomNumberBetween0And2Inclusive * 1000);
            await Task.Delay(delayInSeconds, cancellationToken);
            return Result.Ok();
        }

        if (!user.IsEmailVerified)
            return Result.Ok();

        //0. If the email has been sent in less than 5 minutes ago, do not send a new one
        //Polly for persistence in save changes?

        var urlSafeMagicSignInToken = _tokenProvider.CreateUrlSafeMagicSignInToken(user.Id);
        await _emailSender.SendMagicSignInEmailAsync(user.Email, urlSafeMagicSignInToken);

        return Result.Ok();
    }
}
