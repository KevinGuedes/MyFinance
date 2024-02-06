using FluentResults;
using MediatR;
using MyFinance.Application.Common.ApiService;
using MyFinance.Application.Mappers;
using MyFinance.Application.UseCases.Users.Commands;
using MyFinance.Application.UseCases.Users.DTOs;
namespace MyFinance.Application.UseCases.Users.ApiService;

public sealed class UserApiService(IMediator mediator) : BaseApiService(mediator), IUserApiService
{
    public async Task<Result<UserDTO>> RegisterUserAsync(
        RegisterUserCommand command, 
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return MapResult(result, DomainToDTOMapper.UserToDTO);
    }
}
