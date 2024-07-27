using FluentResults;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;
using MyFinance.Contracts.Transfer.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfer;

internal sealed class RegisterTransferHandler(IMyFinanceDbContext myFinanceDbContext)
    : ICommandHandler<RegisterTransferCommand, TransferResponse>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result<TransferResponse>> Handle(RegisterTransferCommand command,
        CancellationToken cancellationToken)
    {
        var managementUnit = await _myFinanceDbContext.ManagementUnits
            .FindAsync([command.ManagementUnitId], cancellationToken);

        if (managementUnit is null)
        {
            var errorMessage = $"Management Unit with Id {command.ManagementUnitId} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var accountTag = await _myFinanceDbContext.AccountTags.FindAsync([command.AccountTagId], cancellationToken);
        if (accountTag is null)
        {
            var errorMessage = $"Account Tag with Id {command.AccountTagId} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var category = await _myFinanceDbContext.Categories.FindAsync([command.CategoryId], cancellationToken);
        if (category is null)
        {
            var errorMessage = $"Category with Id {command.AccountTagId} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var transfer = new Transfer(
            command.Value,
            command.RelatedTo,
            command.Description,
            command.SettlementDate,
            command.Type,
            command.CurrentUserId,
            managementUnit,
            accountTag,
            category);

        managementUnit.RegisterTransferValue(transfer.Value, transfer.Type);

        await _myFinanceDbContext.Transfers.AddAsync(transfer, cancellationToken);
        _myFinanceDbContext.ManagementUnits.Update(managementUnit);

        return Result.Ok(new TransferResponse()
        {
            Id = transfer.Id,
            RelatedTo = transfer.RelatedTo,
            Description = transfer.Description,
            SettlementDate = transfer.SettlementDate,
            Type = transfer.Type,
            Value = transfer.Value,
            Tag = transfer.AccountTag?.Tag!,
            CategoryName = transfer.Category?.Name!
        });
    }
}