using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.AccountTag.Responses;

namespace MyFinance.Application.UseCases.AccountTags.Commands.UpdateAccountTag;

internal sealed class UpdateAccountTagHandler(IAccountTagRepository accountTagRepository, ICurrentUserProvider currentUserProvider) 
    : ICommandHandler<UpdateAccountTagCommand, AccountTagResponse>
{
    private readonly IAccountTagRepository _accountTagRepository = accountTagRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

    public async Task<Result<AccountTagResponse>> Handle(UpdateAccountTagCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        var accountTag = await _accountTagRepository.GetByIdAsync(command.Id, currentUserId, cancellationToken);
       
        if (accountTag is null)
        {
            var errorMessage = $"Account Tag with Id {command.Id} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        accountTag.Update(command.Tag, command.Description);
        _accountTagRepository.Update(accountTag);

        return Result.Ok(AccountTagMapper.DTR.Map(accountTag));
    }
}