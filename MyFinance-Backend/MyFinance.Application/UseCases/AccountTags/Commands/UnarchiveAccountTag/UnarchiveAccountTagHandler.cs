﻿using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.AccountTags.Commands.UnarchiveAccountTag;

internal sealed class UnarchiveAccountTagHandler : ICommandHandler<UnarchiveAccountTagCommand>
{
    private readonly ILogger<UnarchiveAccountTagHandler> _logger;
    private readonly IAccountTagRepository _accountTagRepository;
    public UnarchiveAccountTagHandler(
       ILogger<UnarchiveAccountTagHandler> logger,
       IAccountTagRepository accountTagRepository)
       => (_logger, _accountTagRepository) = (logger, accountTagRepository);

    public async Task<Result> Handle(UnarchiveAccountTagCommand command, CancellationToken cancellationToken)
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

        _logger.LogInformation("Unarchiving Account Tag with Id {AccountTagId}", command.Id);
        accountTag.Unarchive();
        _accountTagRepository.Update(accountTag);
        _logger.LogInformation("Account Tag with Id {AccountTagId} successfully unarchived", command.Id);

        return Result.Ok();
    }
}
