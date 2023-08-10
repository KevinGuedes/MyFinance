using FluentResults;
using MediatR;
using MyFinance.Application.Common.ApiService;
using MyFinance.Application.MappingProfiles;
using MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;
using MyFinance.Application.UseCases.AccountTags.Commands.UpdateAccountTag;
using MyFinance.Application.UseCases.AccountTags.DTOs;

namespace MyFinance.Application.UseCases.AccountTags.ApiService;

public class AccountTagApiService : BaseApiService, IAccountTagApiService
{
    public AccountTagApiService(IMediator mediator) : base(mediator)
    {
    }


    public Task<Result> BindToBusinessUnitAsync(Guid businessUnitId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<AccountTagDTO>> CreateAccountTagAsync(CreateAccountTagCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return MapResult(result, DomainToDTOMapper.AccountTagToDTO);
    }

    public async Task<Result<AccountTagDTO>> UpdateAccountTagAsync(UpdateAccountTagCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return MapResult(result, DomainToDTOMapper.AccountTagToDTO);
    }
}
