using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Transfer.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfer;

internal sealed class RegisterTransferHandler(
    IManagementUnitRepository managementUnitRepository,
    ITransferRepository transferRepository,
    IAccountTagRepository accountTagRepository,
    ICategoryRepository categoryRepository) : ICommandHandler<RegisterTransferCommand, TransferResponse>
{
    private readonly IManagementUnitRepository _managementUnitRepository = managementUnitRepository;
    private readonly ITransferRepository _transferRepository = transferRepository;
    private readonly IAccountTagRepository _accountTagRepository = accountTagRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<Result<TransferResponse>> Handle(RegisterTransferCommand command,
        CancellationToken cancellationToken)
    {
        var managementUnit = await _managementUnitRepository.GetByIdAsync(command.ManagementUnitId, cancellationToken);
        if (managementUnit is null)
        {
            var errorMessage = $"Management Unit with Id {command.ManagementUnitId} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var accountTag = await _accountTagRepository.GetByIdAsync(command.AccountTagId, cancellationToken);
        if (accountTag is null)
        {
            var errorMessage = $"Account Tag with Id {command.AccountTagId} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var category = await _categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);
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

        await _transferRepository.InsertAsync(transfer, cancellationToken);
        _managementUnitRepository.Update(managementUnit);

        return Result.Ok(TransferMapper.DTR.Map(transfer));
    }
}