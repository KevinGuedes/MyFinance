using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Contracts.User.Responses;

namespace MyFinance.Application.UseCases.Users.Commands.SignIn;

internal sealed class SignInHandler(
    ISignInManager signInManager,
    IPasswordManager passwordManager,
    ILockoutManager lockoutManager,
    IUserRepository userRepository) : ICommandHandler<SignInCommand, SignInResponse>
{
    private readonly ISignInManager _signInManager = signInManager;
    private readonly IPasswordManager _passwordManager = passwordManager;
    private readonly ILockoutManager _lockoutManager = lockoutManager;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<SignInResponse>> Handle(SignInCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (user is null)
            return HandleInvalidCredentials();

        if (!_lockoutManager.CanSignIn(user.LockoutEndOnUtc))
            return HandleLockout(user.LockoutEndOnUtc!.Value);

        var isPasswordValid = _passwordManager.VerifyPassword(command.PlainTextPassword, user.PasswordHash);
        if (isPasswordValid)
        {
            if (user.FailedSignInAttempts != 0)
            {
                user.ResetLockout();
                _userRepository.Update(user);
            }

            await _signInManager.SignInAsync(user);
            var shouldUpdatePassword = _passwordManager.ShouldUpdatePassword(user.LastPasswordUpdateOnUtc);
            return Result.Ok(new SignInResponse(shouldUpdatePassword));
        }

        user.IncrementFailedSignInAttempts();

        if (_lockoutManager.ShouldLockout(user.FailedSignInAttempts))
        {
            var lockoutEndOnUtc = _lockoutManager.GetLockoutEndOnUtc(user.FailedSignInAttempts);
            user.SetLockoutEnd(lockoutEndOnUtc);
            return HandleLockout(lockoutEndOnUtc);
        }

        if (_lockoutManager.WillLockoutOnNextAttempt(user.FailedSignInAttempts))
        {
            var nextLockoutDuration = _lockoutManager.GetNextLockoutDuration(user.FailedSignInAttempts);
            return HandleNextLockout(nextLockoutDuration);
        }

        _userRepository.Update(user);

        return HandleInvalidCredentials();
    }

    private static Result HandleLockout(DateTime lockoutEndOnUtc)
        => Result.Fail(new TooManyFailedSignInAttemptsError(lockoutEndOnUtc));

    private static Result HandleNextLockout(TimeSpan nextLockoutDuration)
        => Result.Fail(new UnauthorizedError(
            "Invalid credentials. " +
            "If you fail one more time you will be locked for " +
            $"{nextLockoutDuration.Days} days {nextLockoutDuration.Hours} " +
            $"hours and {nextLockoutDuration.Minutes} minutes"));

    private static Result HandleInvalidCredentials()
        => Result.Fail(new UnauthorizedError("Invalid credentials"));
}