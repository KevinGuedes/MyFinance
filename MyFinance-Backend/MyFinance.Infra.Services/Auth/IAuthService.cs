using MyFinance.Domain.Entities;

namespace MyFinance.Infra.Services.Auth;

public interface IAuthService
{
    Task SignInAsync(string userEmail);
    Task SignOutAsync();
}
