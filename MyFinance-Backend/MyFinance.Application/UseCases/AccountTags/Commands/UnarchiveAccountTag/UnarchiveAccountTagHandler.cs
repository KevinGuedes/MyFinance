using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.AccountTags.Commands.UnarchiveAccountTag;

internal sealed class UnarchiveAccountTagHandler(IAccountTagRepository accountTagRepository)
    : ICommandHandler<UnarchiveAccountTagCommand>
{
    private readonly IAccountTagRepository _accountTagRepository = accountTagRepository;

    public async Task<Result> Handle(UnarchiveAccountTagCommand command, CancellationToken cancellationToken)
    {
        var accountTag = await _accountTagRepository.GetByIdAsync(command.Id, command.CurrentUserId, cancellationToken);

        if (accountTag is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Account Tag with Id {command.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        accountTag.Unarchive();
        _accountTagRepository.Update(accountTag);

        return Result.Ok();
    }
}