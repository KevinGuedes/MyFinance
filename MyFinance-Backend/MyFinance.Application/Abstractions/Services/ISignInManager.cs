using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Services;

public interface ISignInManager
{
    Task SignInAsync(User user);
    Task SignOutAsync();
    string CreateMagicSignInToken(Guid magicSignInId);
    bool TryGetMagicSignInIdFromToken(string token, out Guid magicSignInId);
}
