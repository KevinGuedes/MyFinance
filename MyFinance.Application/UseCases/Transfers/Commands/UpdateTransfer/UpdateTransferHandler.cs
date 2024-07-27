using FluentResults;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Transfer.Responses;

namespace MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;

internal sealed class UpdateTransferHandler(IMyFinanceDbContext myFinanceDbContext)
    : ICommandHandler<UpdateTransferCommand, TransferResponse>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result<TransferResponse>> Handle(UpdateTransferCommand command,
        CancellationToken cancellationToken)
    {
        var transfer = await _myFinanceDbContext.Transfers
            .Include(transfer => transfer.ManagementUnit)
            .FirstOrDefaultAsync(transfer => transfer.Id == command.Id, cancellationToken);

        if (transfer is null)
        {
            var errorMessage = $"Transfer with Id {command.Id} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var managementUnit = transfer.ManagementUnit;
        managementUnit.CancelTransferValue(transfer.Value, transfer.Type);

        var hasAccountTagChanged = transfer.AccountTagId != command.AccountTagId;
        if (hasAccountTagChanged)
        {
            var accountTag = await _myFinanceDbContext.AccountTags.FindAsync([command.AccountTagId], cancellationToken);

            if (accountTag is null)
            {
                var errorMessage = $"Account Tag with Id {command.AccountTagId} not found";
                var entityNotFoundError = new EntityNotFoundError(errorMessage);
                return Result.Fail(entityNotFoundError);
            }

            transfer.UpdateAccountTag(accountTag);
        }

        var hasCategoryChanged = transfer.CategoryId != command.CategoryId;
        if (hasCategoryChanged)
        {
            var category = await _myFinanceDbContext.Categories.FindAsync([command.CategoryId], cancellationToken);

            if (category is null)
            {
                var errorMessage = $"Category with Id {command.AccountTagId} not found";
                var entityNotFoundError = new EntityNotFoundError(errorMessage);
                return Result.Fail(entityNotFoundError);
            }

            transfer.UpdateCategory(category);
        }

        transfer.Update(
            command.Value,
            command.RelatedTo,
            command.Description,
            command.SettlementDate,
            command.Type);

        managementUnit.RegisterTransferValue(transfer.Value, transfer.Type);

        _myFinanceDbContext.Transfers.Update(transfer);
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