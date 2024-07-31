using FluentResults;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;
using MyFinance.Contracts.AccountTag.Responses;

namespace MyFinance.Application.UseCases.AccountTags.Commands.UpdateAccountTag;

internal sealed class UpdateAccountTagHandler(IMyFinanceDbContext myFinanceDbContext)
    : ICommandHandler<UpdateAccountTagCommand, AccountTagResponse>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result<AccountTagResponse>> Handle(UpdateAccountTagCommand command,
        CancellationToken cancellationToken)
    {
        var accountTag = await _myFinanceDbContext.AccountTags
            .FindAsync([command.Id], cancellationToken);

        if (accountTag is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Account Tag with Id {command.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        accountTag.Update(command.Tag, command.Description);
        _myFinanceDbContext.AccountTags.Update(accountTag);

        return Result.Ok(new AccountTagResponse
        {
            Id = accountTag.Id,
            Tag = accountTag.Tag,
            Description = accountTag.Description,
        });
    }
}