using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.AccountTags.Commands.UpdateAccountTag;

internal sealed class UpdateAccountTagHandler(
    ILogger<UpdateAccountTagHandler> logger,
    IAccountTagRepository accountTagRepository,
    ICurrentUserProvider currentUserProvider) : ICommandHandler<UpdateAccountTagCommand, AccountTag>
{
    private readonly ILogger<UpdateAccountTagHandler> _logger = logger;
    private readonly IAccountTagRepository _accountTagRepository = accountTagRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

    public async Task<Result<AccountTag>> Handle(UpdateAccountTagCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        _logger.LogInformation("Retriving Account Tag with Id {AccountTagId}", command.Id);
        var accountTag = await _accountTagRepository.GetByIdAsync(command.Id, currentUserId, cancellationToken);
        if (accountTag is null)
        {
            _logger.LogWarning("Account Tag with Id {AccountTagId} not found", command.Id);
            var errorMessage = string.Format("Account Tag with Id {0} not found", command.Id);
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        _logger.LogInformation("Updating Account Tag with Id {AccountTagId}", command.Id);
        accountTag.Update(command.Tag, command.Description);
        _accountTagRepository.Update(accountTag);
        _logger.LogInformation("Account Tag with Id {AccountTagId} successfully updated", command.Id);

        return Result.Ok(accountTag);
    }
}
