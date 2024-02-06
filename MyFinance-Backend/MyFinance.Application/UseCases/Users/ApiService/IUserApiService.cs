using FluentResults;
using MyFinance.Application.UseCases.Users.Commands;
using MyFinance.Application.UseCases.Users.DTOs;

namespace MyFinance.Application.UseCases.Users.ApiService;

public interface IUserApiService
{
    Task<Result<UserDTO>> RegisterUserAsync(RegisterUserCommand command, CancellationToken cancellationToken);
}
