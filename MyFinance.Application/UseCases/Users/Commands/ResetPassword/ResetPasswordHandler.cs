using FluentResults;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.Users.Commands.ResetPassword;

public sealed class ResetPasswordHandler(
    ITokenProvider tokenProvider,
    IPasswordManager passwordManager,
    IMyFinanceDbContext myFinanceDbContext)
    : ICommandHandler<ResetPasswordCommand>
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IPasswordManager _passwordManager = passwordManager;
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var isValidToken = _tokenProvider.TryGetUserIdFromUrlSafeResetPasswordToken(
            command.UrlSafeResetPasswordToken,
            out var userId);

        if (!isValidToken)
            return HandleInvalidResetPasswordToken();

        var user = await _myFinanceDbContext.Users.FindAsync([userId], cancellationToken);

        if (user is null)
            return HandleInvalidResetPasswordToken();

        var newPasswordHash = _passwordManager.HashPassword(command.PlainTextNewPassword);

        user.UpdatePasswordHash(newPasswordHash);
        _myFinanceDbContext.Users.Update(user);

        return Result.Ok();
    }

    private static Result HandleInvalidResetPasswordToken()
        => Result.Fail(new UnauthorizedError("Invalid reset password token"));
}
