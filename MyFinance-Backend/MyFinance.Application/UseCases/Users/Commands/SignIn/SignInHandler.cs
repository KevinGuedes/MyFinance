using FluentResults;
using Humanizer;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.User.Responses;

namespace MyFinance.Application.UseCases.Users.Commands.SignIn;

internal sealed class SignInHandler(
    ISignInManager signInManager,
    IPasswordManager passwordManager,
    ILockoutManager lockoutManager,
    IEmailSender emailSender,
    IUserRepository userRepository) : ICommandHandler<SignInCommand, UserResponse>
{
    private readonly ISignInManager _signInManager = signInManager;
    private readonly IPasswordManager _passwordManager = passwordManager;
    private readonly ILockoutManager _lockoutManager = lockoutManager;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<UserResponse>> Handle(SignInCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);

        if (user is null)
            return HandleInvalidCredentials();

        if (!user.IsEmailVerified)
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

            var signInResponse = UserMapper.DTR.Map(user, _passwordManager.ShouldUpdatePassword(user));

            return Result.Ok(signInResponse);
        }

        user.IncrementFailedSignInAttempts();

        if (_lockoutManager.ShouldLockout(user.FailedSignInAttempts))
        {
            var lockoutEndOnUtc = _lockoutManager.GetLockoutEndOnUtc(user.FailedSignInAttempts);
            var lockoutDuration = _lockoutManager.GetLockoutDurationFor(user.FailedSignInAttempts);

            await _emailSender.SendUserLockedEmailAsync(
                user.Email,
                lockoutDuration,
                lockoutEndOnUtc,
                cancellationToken);

            user.SetLockoutEnd(lockoutEndOnUtc);
            _userRepository.Update(user);

            return HandleLockout(lockoutEndOnUtc);
        }

        if (_lockoutManager.WillLockoutOnNextAttempt(user.FailedSignInAttempts))
        {
            var nextLockoutDuration = _lockoutManager.GetNextLockoutDurationFor(user.FailedSignInAttempts);

            _userRepository.Update(user);

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
            $"{nextLockoutDuration.Humanize()}"));

    private static Result HandleInvalidCredentials()
        => Result.Fail(new UnauthorizedError(
            "Invalid credentials. " +
            "Your account may be blocked for excessive attempts. " +
            "Also, please check your email to make sure you have verified your account."));
}