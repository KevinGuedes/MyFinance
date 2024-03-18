using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Transfer.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;

internal sealed class UpdateTransferHandler(
    IMonthlyBalanceRepository monthlyBalanceRepository,
    IBusinessUnitRepository businessUnitRepository,
    ITransferRepository transferRepository,
    IAccountTagRepository accountTagRepository,
    ICurrentUserProvider currentUserProvider) : ICommandHandler<UpdateTransferCommand, TransferResponse>
{
    private readonly IAccountTagRepository _accountTagRepository = accountTagRepository;
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;
    private readonly IMonthlyBalanceRepository _monthlyBalanceRepository = monthlyBalanceRepository;
    private readonly ITransferRepository _transferRepository = transferRepository;

    public async Task<Result<TransferResponse>> Handle(UpdateTransferCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();
        var (transferId, accountTagId, newTransferValue, relatedTo, description, settlementDate, type) = command;

        var transfer = await _transferRepository.GetByIdAsync(transferId, currentUserId, cancellationToken);

        if (transfer is null)
        {
            var errorMessage = $"Transfer with Id {transferId} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var accountTag = await _accountTagRepository.GetByIdAsync(accountTagId, currentUserId, cancellationToken);
        if (accountTag is null)
        {
            var errorMessage = $"Account Tag with Id {accountTagId} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var currentMonthlyBalance = transfer.MonthlyBalance;
        var businessUnit = currentMonthlyBalance.BusinessUnit;

        businessUnit.CancelValue(transfer.Value, transfer.Type);
        currentMonthlyBalance.CancelValue(transfer.Value, transfer.Type);

        var shouldGoToAnotherMonthlyBalance =
            currentMonthlyBalance.ReferenceYear != settlementDate.Year ||
            currentMonthlyBalance.ReferenceMonth != settlementDate.Month;

        if (shouldGoToAnotherMonthlyBalance)
        {
            currentMonthlyBalance.Transfers.Remove(transfer);

            var existingMonthlyBalance = await _monthlyBalanceRepository.GetByReferenceDateAndBusinessUnitId(
                settlementDate,
                businessUnit.Id,
                currentUserId,
                cancellationToken);

            if (existingMonthlyBalance is null)
            {
                var newMonthlyBalance = new MonthlyBalance(settlementDate, businessUnit, currentUserId);
                transfer.Update(
                    newTransferValue,
                    relatedTo,
                    description,
                    settlementDate,
                    type,
                    newMonthlyBalance,
                    accountTag);
                newMonthlyBalance.RegisterValue(transfer.Value, transfer.Type);
                _monthlyBalanceRepository.Insert(newMonthlyBalance);
            }
            else
            {
                transfer.Update(
                    newTransferValue,
                    relatedTo,
                    description,
                    settlementDate,
                    type,
                    existingMonthlyBalance,
                    accountTag);
                existingMonthlyBalance.RegisterValue(transfer.Value, transfer.Type);
                _monthlyBalanceRepository.Update(existingMonthlyBalance);
            }
        }
        else
        {
            transfer.Update(
                newTransferValue,
                relatedTo,
                description,
                settlementDate,
                type,
                currentMonthlyBalance,
                accountTag);
            currentMonthlyBalance.RegisterValue(transfer.Value, transfer.Type);
        }

        businessUnit.RegisterValue(transfer.Value, transfer.Type);
        _transferRepository.Update(transfer);
        _businessUnitRepository.Update(businessUnit);
        _monthlyBalanceRepository.Update(currentMonthlyBalance);

        return Result.Ok(TransferMapper.DTR.Map(transfer));
    }
}