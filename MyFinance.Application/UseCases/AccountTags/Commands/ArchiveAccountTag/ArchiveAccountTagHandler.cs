using FluentResults;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.AccountTags.Commands.ArchiveAccountTag;

internal sealed class ArchiveAccountTagHandler(IMyFinanceDbContext myFinanceDbContext)
    : ICommandHandler<ArchiveAccountTagCommand>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result> Handle(ArchiveAccountTagCommand command, CancellationToken cancellationToken)
    {
        var accountTag = await _myFinanceDbContext.AccountTags.FindAsync([command.Id], cancellationToken);

        if (accountTag is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Account Tag with Id {command.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        accountTag.Archive(command.ReasonToArchive);
        _myFinanceDbContext.AccountTags.Update(accountTag);

        return Result.Ok();
    }
}