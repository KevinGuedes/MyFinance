using MyFinance.Domain.Entities;

namespace MyFinance.Application.Services.Auth;

public interface IAuthService
{
    Task SignInAsync(User user);
    Task SignOutAsync();
}
