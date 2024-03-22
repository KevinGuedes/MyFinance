using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.AccountTags.Commands.ArchiveAccountTag;

internal sealed class ArchiveAccountTagHandler(IAccountTagRepository accountTagRepository)
    : ICommandHandler<ArchiveAccountTagCommand>
{
    private readonly IAccountTagRepository _accountTagRepository = accountTagRepository;

    public async Task<Result> Handle(ArchiveAccountTagCommand command, CancellationToken cancellationToken)
    {
        var accountTag = await _accountTagRepository.GetByIdAsync(command.Id, cancellationToken);

        if (accountTag is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Account Tag with Id {command.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        accountTag.Archive(command.ReasonToArchive);
        _accountTagRepository.Update(accountTag);

        return Result.Ok();
    }
}