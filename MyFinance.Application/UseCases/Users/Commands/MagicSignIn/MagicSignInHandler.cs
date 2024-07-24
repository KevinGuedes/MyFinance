using FluentResults;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Contracts.User.Responses;

namespace MyFinance.Application.UseCases.Users.Commands.MagicSignIn;

internal sealed class MagicSignInHandler(
    ITokenProvider tokenProvider,
    ISignInManager signInManager,
    IPasswordManager passwordManager,
    IMyFinanceDbContext myFinanceDbContext)
    : ICommandHandler<MagicSignInCommand, UserInfoResponse>
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly ISignInManager _signInManager = signInManager;
    private readonly IPasswordManager _passwordManager = passwordManager;
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result<UserInfoResponse>> Handle(MagicSignInCommand command, CancellationToken cancellationToken)
    {
        var isValidToken = _tokenProvider.TryGetUserIdFromUrlSafeMagicSignInToken(
            command.UrlSafeMagicSignInToken,
            out var userId);

        if (!isValidToken)
            return HandleInvalidMagicSignInToken();

        var user = await _myFinanceDbContext.Users.FindAsync([userId], cancellationToken);

        if (user is null)
            return HandleInvalidMagicSignInToken();

        if (user.FailedSignInAttempts != 0)
        {
            user.ResetLockout();
            _myFinanceDbContext.Users.Update(user);
        }

        await _signInManager.SignInAsync(user);

        var shouldUpdatePassword = _passwordManager.ShouldUpdatePassword(
            user.CreatedOnUtc,
            user.LastPasswordUpdateOnUtc);

        return Result.Ok(new UserInfoResponse
        {
            Name = user.Name,
            ShouldUpdatePassword = shouldUpdatePassword,
        });
    }

    private static Result HandleInvalidMagicSignInToken()
        => Result.Fail(new UnauthorizedError("Invalid magic sign in token"));
}
