using FluentResults;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;
using MyFinance.Contracts.AccountTag.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;

internal sealed class CreateAccountTagHandler(IMyFinanceDbContext myFinanceDbContext)
    : ICommandHandler<CreateAccountTagCommand, AccountTagResponse>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result<AccountTagResponse>> Handle(CreateAccountTagCommand command, CancellationToken cancellationToken)
    {
        var managementUnit = await _myFinanceDbContext.ManagementUnits
            .FindAsync([command.ManagementUnitId], cancellationToken);

        if (managementUnit is null)
        {
            var errorMessage = $"Management Unit with Id {command.ManagementUnitId} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var accountTag = new AccountTag(
            managementUnit,
            command.Tag,
            command.Description,
            command.CurrentUserId);

        await _myFinanceDbContext.AccountTags.AddAsync(accountTag, cancellationToken);

        return Result.Ok(new AccountTagResponse
        {
            Id = accountTag.Id,
            Tag = accountTag.Tag,
            Description = accountTag.Description,
        });
    }
}