using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.AccountTags.Commands.ArchiveAccountTag;

internal sealed class ArchiveAccountTagHandler(
    ILogger<ArchiveAccountTagHandler> logger,
    IAccountTagRepository accountTagRepository) : ICommandHandler<ArchiveAccountTagCommand>
{
    private readonly ILogger<ArchiveAccountTagHandler> _logger = logger;
    private readonly IAccountTagRepository _accountTagRepository = accountTagRepository;
  
    public async Task<Result> Handle(ArchiveAccountTagCommand command, CancellationToken cancellationToken)
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

        _logger.LogInformation("Archiving Account Tag with Id {AccountTagId}", command.Id);
        accountTag.Archive(command.ReasonToArchive);
        _accountTagRepository.Update(accountTag);
        _logger.LogInformation("Account Tag with Id {AccountTagId} successfully archived", command.Id);

        return Result.Ok();
    }
}
