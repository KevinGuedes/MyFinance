using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.Users.Commands.ResetPassword;

public sealed class ResetPasswordHandler(
    ITokenProvider tokenProvider,
    IPasswordManager passwordManager,
    IUserRepository userRepository)
    : ICommandHandler<ResetPasswordCommand>
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IPasswordManager _passwordManager = passwordManager;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var isValidToken = _tokenProvider.TryGetUserIdFromUrlSafeResetPasswordToken(
            command.UrlSafeResetPasswordToken,
            out var userId);

        if (!isValidToken)
            return HandleInvalidResetPasswordToken();

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
            return HandleInvalidResetPasswordToken();

        var newPasswordHash = _passwordManager.HashPassword(command.PlainTextNewPassword);

        user.UpdatePasswordHash(newPasswordHash);
        _userRepository.Update(user);

        return Result.Ok();
    }

    private static Result HandleInvalidResetPasswordToken()
        => Result.Fail(new UnauthorizedError("Invalid reset password token"));
}
