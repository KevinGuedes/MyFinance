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

        //0. If the email has been sent in less than 5 minutes ago, do not send a new one
        //1. Create a new token
        //2. Send the email with some resilience strategy (Polly)
        //3. If the email was sent, save the user data. If not, fail the request

        //User data: MagicSignInId and MagicSignInEmailSentOnUtc
        //Polly for persistence in save changes?

        var urlSafeMagicSignInToken 
            = _tokenProvider.CreateUrlSafeMagicSignInToken(user.Id);

        var (hasEmailBeenSent, exception) = await _emailSender.SendMagicSignInEmailAsync(
            user.Email, 
            urlSafeMagicSignInToken);

        if(!hasEmailBeenSent)
        {
            var errorMessage = "The magic sign-in email could not be sent. Please try again later.";
            var internalServerError = new InternalServerError(errorMessage).CausedBy(exception);
            return Result.Fail(internalServerError);
        }

        return Result.Ok();
    }
}
