using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;

namespace MyFinance.Application.UseCases.Users.Commands.SendMagicSignInEmail;

internal sealed class SendMagicSignInEmailHandler(
    ISignInManager signInManager,
    IUserRepository userRepository)
    : ICommandHandler<SendMagicSignInEmailCommand>
{
    private readonly ISignInManager _signInManager = signInManager;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result> Handle(SendMagicSignInEmailCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);

        //To avoid telling hackers if the email is from an existing user or not and avoid brute force attacks
        if (user is null)
        {
            var random = new Random();
            var randomNumberBetween1And3Inclusive = Math.Round(random.NextDouble() * 2 + 1, 3);
            var delayInSeconds = Convert.ToInt32(randomNumberBetween1And3Inclusive * 1000);
            await Task.Delay(delayInSeconds, cancellationToken);
            return Result.Ok();
        }
     
        //0. If the email has been sent in less than 5 minutes ago, do not send a new one
        //1. Create a new token
        //2. Send the email with some resilience strategy (Polly)
        //3. If the email was sent, save the user data. If not, fail the request
        
        //User data: MagicSignInId and MagicSignInEmailSentOnUtc
        //Polly for persistence in save changes?

        var magicSignInId = Guid.NewGuid();
        var token = _signInManager.CreateMagicSignInToken(magicSignInId);
        user.SetMagicSignInId(magicSignInId);

        _userRepository.Update(user);

        return Result.Ok();
    }
}
