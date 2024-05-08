using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.AccountTag.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;

internal sealed class CreateAccountTagHandler(IAccountTagRepository accountTagRepository)
    : ICommandHandler<CreateAccountTagCommand, AccountTagResponse>
{
    private readonly IAccountTagRepository _accountTagRepository = accountTagRepository;

    public async Task<Result<AccountTagResponse>> Handle(CreateAccountTagCommand command, CancellationToken cancellationToken)
    {
        var accountTag = new AccountTag(command.Tag, command.Description, command.CurrentUserId);
        await _accountTagRepository.InsertAsync(accountTag, cancellationToken);

        return Result.Ok(AccountTagMapper.DTR.Map(accountTag));
    }
}