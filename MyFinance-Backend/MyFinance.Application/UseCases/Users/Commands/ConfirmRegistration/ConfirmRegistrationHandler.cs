using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.Users.Commands.ConfirmRegistration;

internal sealed class ConfirmRegistrationHandler(
    ITokenProvider tokenProvider,
    IUserRepository userRepository)
    : ICommandHandler<ConfirmRegistrationCommand>
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result> Handle(ConfirmRegistrationCommand command, CancellationToken cancellationToken)
    {
        var isValidToken = _tokenProvider.TryGetUserIdFromUrlSafeConfirmRegistrationToken(
            command.UrlSafeConfirmRegistrationToken,
            out var userId);

        if (!isValidToken)
            return HandleInvalidConfirmRegistrationToken();

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
            return HandleInvalidConfirmRegistrationToken();

        user.VerifyEmail();
        _userRepository.Update(user);

        return Result.Ok();
    }

    private static Result HandleInvalidConfirmRegistrationToken()
        => Result.Fail(new InternalServerError("Invalid confirm registration token"));
}
