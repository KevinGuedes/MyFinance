using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Services;

public interface ISignInManager
{
    Task SignInAsync(User user);
    Task SignOutAsync();
    bool ShouldUpdatePassword(DateTime lastPasswordUpdate);
    bool CanSignIn(DateTime? lockoutEnd);
    public bool WillLockoutOnNextAttempt(int failedSignInAttempts);
    TimeSpan GetNextLockoutDuration(int failedSignInAttempts);
    public bool ShouldLockout(int failedSignInAttempts);
    DateTime GetLockoutEndOnUtc(int failedSignInAttempts);
}
