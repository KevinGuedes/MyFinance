using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Services;

public interface IAuthService
{
    Task SignInAsync(User user);
    Task SignOutAsync();
}
