using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Services;

public interface ISignInManager
{
    Task SignInAsync(User user);
    Task SignOutAsync();
}
