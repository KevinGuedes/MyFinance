using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;

namespace MyFinance.Application.UseCases.Users.Commands.ResendConfirmRegistrationEmail;

internal sealed class ResendConfirmRegistrationEmailHandler(
    IUserRepository userRepository,
    ITokenProvider tokenProvider,
    IEmailSender emailSender)
    : ICommandHandler<ResendConfirmRegistrationEmailCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task<Result> Handle(ResendConfirmRegistrationEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
            return Result.Ok();

        var urlSafeConfirmRegistrationToken
            = _tokenProvider.CreateUrlSafeConfirmRegistrationToken(user.Id);

        await _emailSender.SendConfirmRegistrationEmailAsync(
            user.Email,
            urlSafeConfirmRegistrationToken,
            cancellationToken);

        return Result.Ok();
    }
}
