using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Application.UseCases.BusinessUnits.Commands.UpdateBusinessUnit;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Application.UseCases.AccountTags.Commands.UpdateAccountTag;

internal sealed class UpdateAccountTagHandler : ICommandHandler<UpdateAccountTagCommand, AccountTag>
{
    private readonly ILogger<UpdateAccountTagHandler> _logger;
    private readonly IAccountTagRepository _accountTagRepository;

    public UpdateAccountTagHandler(
        ILogger<UpdateAccountTagHandler> logger,
        IAccountTagRepository accountTagRepository)
        => (_logger, _accountTagRepository) = (logger, accountTagRepository);

    public async Task<Result<AccountTag>> Handle(UpdateAccountTagCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retriving Account Tag with Id {AccountTagId}", command.Id);
        var accountTag = await _accountTagRepository.GetByIdAsync(command.Id, cancellationToken);
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
