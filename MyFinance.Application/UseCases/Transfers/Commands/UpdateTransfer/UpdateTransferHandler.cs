using FluentResults;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;
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
            var accountTagId = await _myFinanceDbContext.AccountTags
                .Where(at =>
                    at.Id == command.AccountTagId &&
                    at.ManagementUnitId == managementUnit.Id)
                .Select(at => at.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (accountTagId == default)
            {
                var errorMessage = $"Account Tag with Id {command.AccountTagId} not found";
                var entityNotFoundError = new EntityNotFoundError(errorMessage);
                return Result.Fail(entityNotFoundError);
            }

            transfer.UpdateAccountTag(accountTagId);
        }

        var hasCategoryChanged = transfer.CategoryId != command.CategoryId;
        if (hasCategoryChanged)
        {
            var categoryId = await _myFinanceDbContext.Categories
                .Where(category =>
                    category.Id == command.CategoryId &&
                    category.Id == managementUnit.Id)
                .Select(category => category.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (categoryId == default)
            {
                var errorMessage = $"Category with Id {command.AccountTagId} not found";
                var entityNotFoundError = new EntityNotFoundError(errorMessage);
                return Result.Fail(entityNotFoundError);
            }

            transfer.UpdateCategory(categoryId);
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