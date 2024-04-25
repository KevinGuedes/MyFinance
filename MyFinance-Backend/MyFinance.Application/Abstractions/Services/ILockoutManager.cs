namespace MyFinance.Application.Abstractions.Services;

public interface ILockoutManager
{
    bool CanSignIn(DateTime? lockoutEndOnUtc);
    bool WillLockoutOnNextAttempt(int failedSignInAttempts);
    TimeSpan GetNextLockoutDuration(int failedSignInAttempts);
    bool ShouldLockout(int failedSignInAttempts);
    DateTime GetLockoutEndOnUtc(int failedSignInAttempts);
}
