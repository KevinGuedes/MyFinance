namespace MyFinance.Application.Services.Auth;

public interface IAuthService
{
    Task SignInAsync(string userEmail);
    Task SignOutAsync();
}
