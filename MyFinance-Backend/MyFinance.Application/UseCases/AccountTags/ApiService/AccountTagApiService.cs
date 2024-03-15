using FluentResults;
using MediatR;
using MyFinance.Application.Abstractions.ApiServices;
using MyFinance.Application.Common.ApiService;
using MyFinance.Application.Mappers;
using MyFinance.Application.UseCases.AccountTags.Commands.ArchiveAccountTag;
using MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;
using MyFinance.Application.UseCases.AccountTags.Commands.UnarchiveAccountTag;
using MyFinance.Application.UseCases.AccountTags.Commands.UpdateAccountTag;
using MyFinance.Application.UseCases.AccountTags.DTOs;
using MyFinance.Application.UseCases.AccountTags.Queries.GetAccountTags;

namespace MyFinance.Application.UseCases.AccountTags.ApiService;

public sealed class AccountTagApiService(IMediator mediator) : BaseApiService(mediator), IAccountTagApiService
{
    public async Task<Result<IEnumerable<AccountTagDTO>>> GetAccountTagsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = new GetAccountTagsQuery(page, pageSize);
        var result = await _mediator.Send(query, cancellationToken);

        return MapResult(result, DomainToDTOMapper.AccountTagToDTO);
    }

    public Task<Result> ArchiveAccountTagAsync(ArchiveAccountTagCommand command, CancellationToken cancellationToken)
        => _mediator.Send(command, cancellationToken);

    public Task<Result> UnarchiveAccountTagAsync(Guid id, CancellationToken cancellationToken)
        => _mediator.Send(new UnarchiveAccountTagCommand(id), cancellationToken);

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
