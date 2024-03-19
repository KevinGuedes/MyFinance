using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.AccountTag.Responses;

namespace MyFinance.Application.UseCases.AccountTags.Commands.UpdateAccountTag;

internal sealed class UpdateAccountTagHandler(IAccountTagRepository accountTagRepository) 
    : ICommandHandler<UpdateAccountTagCommand, AccountTagResponse>
{
    private readonly IAccountTagRepository _accountTagRepository = accountTagRepository;

    public async Task<Result<AccountTagResponse>> Handle(UpdateAccountTagCommand command, CancellationToken cancellationToken)
    {
        var accountTag = await _accountTagRepository.GetByIdAsync(command.Id, command.CurrentUserId, cancellationToken);
       
        if (accountTag is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Account Tag with Id {command.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        accountTag.Update(command.Tag, command.Description);
        _accountTagRepository.Update(accountTag);

        return Result.Ok(AccountTagMapper.DTR.Map(accountTag));
    }
}