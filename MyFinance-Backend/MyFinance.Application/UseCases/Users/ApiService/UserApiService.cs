using FluentResults;
using MediatR;
using MyFinance.Application.Abstractions.ApiServices;
using MyFinance.Application.Common.ApiService;
using MyFinance.Application.UseCases.Users.Commands.RegisterUser;
using MyFinance.Application.UseCases.Users.Commands.SignIn;
using MyFinance.Application.UseCases.Users.Commands.SignOut;

namespace MyFinance.Application.UseCases.Users.ApiService;

public sealed class UserApiService(IMediator mediator) : BaseApiService(mediator), IUserApiService
{
    public Task<Result> RegisterUserAsync(
        RegisterUserCommand command,
        CancellationToken cancellationToken)
        => _mediator.Send(command, cancellationToken);

    public Task<Result> SignInAsync(SignInCommand command, CancellationToken cancellationToken)
        => _mediator.Send(command, cancellationToken);

    public Task<Result> SignOutAsync(CancellationToken cancellationToken)
        => _mediator.Send(new SignOutCommand(), cancellationToken);
}