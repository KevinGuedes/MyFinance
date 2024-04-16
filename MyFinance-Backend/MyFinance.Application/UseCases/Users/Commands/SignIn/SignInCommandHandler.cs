using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Contracts.User.Responses;

namespace MyFinance.Application.UseCases.Users.Commands.SignIn;

public sealed class SignInCommandHandler(
    ISignInManager signInManager,
    IPasswordHasher passwordHasher,
    IUserRepository userRepository) : ICommandHandler<SignInCommand, SignInResponse>
{
    private readonly ISignInManager _signInManager = signInManager;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<SignInResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null)
            return HandleInvalidCredentials();

        if (!_signInManager.CanSignIn(user.LockoutEndOnUtc))
            return HandleLockout(user.LockoutEndOnUtc!.Value);

        var isPasswordValid = _passwordHasher.VerifyPassword(request.PlainTextPassword, user.PasswordHash);
        if (isPasswordValid)
        {
            if (user.FailedSignInAttempts != 0)
            {
                user.ResetLockout();
                _userRepository.Update(user);
            }

            await _signInManager.SignInAsync(user);
            var shouldUpdatePassword = _signInManager.ShouldUpdatePassword(user.LastPasswordUpdateOnUtc);
            return Result.Ok(new SignInResponse(shouldUpdatePassword));
        }

        user.IncrementFailedSignInAttempts();

        if (_signInManager.ShouldLockout(user.FailedSignInAttempts))
        {
            var lockoutEndOnUtc = _signInManager.GetLockoutEndOnUtc(user.FailedSignInAttempts);
            user.SetLockoutEnd(lockoutEndOnUtc);
            return HandleLockout(lockoutEndOnUtc);
        }

        if (_signInManager.WillLockoutOnNextAttempt(user.FailedSignInAttempts))
        {
            var nextLockoutDuration = _signInManager.GetNextLockoutDuration(user.FailedSignInAttempts);
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