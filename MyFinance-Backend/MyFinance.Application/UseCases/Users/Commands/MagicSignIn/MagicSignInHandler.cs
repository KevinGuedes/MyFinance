using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.User.Responses;

namespace MyFinance.Application.UseCases.Users.Commands.MagicSignIn;

internal sealed class MagicSignInHandler(
    ITokenProvider tokenProvider,
    ISignInManager signInManager,
    IPasswordManager passwordManager,
    IUserRepository userRepository)
    : ICommandHandler<MagicSignInCommand, SignInResponse>
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly ISignInManager _signInManager = signInManager;
    private readonly IPasswordManager _passwordManager = passwordManager;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<SignInResponse>> Handle(MagicSignInCommand command, CancellationToken cancellationToken)
    {
        var isValidToken = _tokenProvider.TryGetUserIdFromUrlSafeMagicSignInToken(
            command.UrlSafeMagicSignInToken,
            out var userId);

        if (!isValidToken)
            return HandleInvalidMagicSignInToken();

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
            return HandleInvalidMagicSignInToken();

        if (user.FailedSignInAttempts != 0)
        {
            user.ResetLockout();
            _userRepository.Update(user);
        }

        await _signInManager.SignInAsync(user);

        var signInResponse = UserMapper.DTR.Map(user, _passwordManager.ShouldUpdatePassword(user));
       
        return Result.Ok(signInResponse);
    }

    private static Result HandleInvalidMagicSignInToken()
        => Result.Fail(new UnauthorizedError("Invalid magic sign in token"));
}
