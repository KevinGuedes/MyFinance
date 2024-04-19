using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Services;

public interface ISignInManager
{
    Task SignInAsync(User user);
    Task SignOutAsync();
    bool ShouldUpdatePassword(DateTime lastPasswordUpdateOnUtc);
    bool CanSignIn(DateTime? lockoutEndOnUtc);
    public bool WillLockoutOnNextAttempt(int failedSignInAttempts);
    TimeSpan GetNextLockoutDuration(int failedSignInAttempts);
    public bool ShouldLockout(int failedSignInAttempts);
    DateTime GetLockoutEndOnUtc(int failedSignInAttempts);
    public string CreateMagicSignInToken(Guid magicSignInId);
    public bool TryGetMagicSignInIdFromToken(string token, out Guid magicSignInId);
}
