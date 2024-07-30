using FluentResults;
using Microsoft.EntityFrameworkCore;
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
        var isValidManagementUnit = await _myFinanceDbContext.ManagementUnits
            .AnyAsync(mu => mu.Id == command.ManagementUnitId, cancellationToken);

        if (!isValidManagementUnit)
        {
            var errorMessage = $"Management Unit with Id {command.ManagementUnitId} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var accountTag = new AccountTag(
            command.Tag,
            command.Description,
            command.ManagementUnitId,
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