using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Transfer.Responses;

namespace MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;

internal sealed class UpdateTransferHandler(
    ITransferRepository transferRepository,
    IManagementUnitRepository managementUnitRepository,
    IAccountTagRepository accountTagRepository,
    ICategoryRepository categoryRepository) : ICommandHandler<UpdateTransferCommand, TransferResponse>
{
    private readonly ITransferRepository _transferRepository = transferRepository;
    private readonly IManagementUnitRepository _managementUnitRepository = managementUnitRepository;
    private readonly IAccountTagRepository _accountTagRepository = accountTagRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<Result<TransferResponse>> Handle(UpdateTransferCommand command,
        CancellationToken cancellationToken)
    {
        var transfer = await _transferRepository.GetWithManagementUnitByIdAsync(command.Id, cancellationToken);

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
            var accountTag = await _accountTagRepository.GetByIdAsync(command.AccountTagId, cancellationToken);

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
            var category = await _categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);

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

        _transferRepository.Update(transfer);
        _managementUnitRepository.Update(managementUnit);

        return Result.Ok(TransferMapper.DTR.Map(transfer));
    }
}