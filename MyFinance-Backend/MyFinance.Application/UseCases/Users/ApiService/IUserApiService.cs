using FluentResults;
using MyFinance.Application.UseCases.Users.Commands.RegisterUser;
using MyFinance.Application.UseCases.Users.Commands.SignIn;

namespace MyFinance.Application.UseCases.Users.ApiService;

public interface IUserApiService
{
    Task<Result> RegisterUserAsync(RegisterUserCommand command, CancellationToken cancellationToken);
    Task<Result> SignInAsync(SignInCommand command, CancellationToken cancellationToken);
    Task<Result> SignOutAsync(CancellationToken cancellationToken);
}
