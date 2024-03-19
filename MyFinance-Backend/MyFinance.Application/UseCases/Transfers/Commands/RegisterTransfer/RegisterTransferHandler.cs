using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Transfer.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfer;

internal sealed class RegisterTransferHandler(
    IMonthlyBalanceRepository monthlyBalanceRepository,
    IBusinessUnitRepository businessUnitRepository,
    ITransferRepository transferRepository,
    IAccountTagRepository accountTagRepository) : ICommandHandler<RegisterTransferCommand, TransferResponse>
{
    private readonly IAccountTagRepository _accountTagRepository = accountTagRepository;
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly IMonthlyBalanceRepository _monthlyBalanceRepository = monthlyBalanceRepository;
    private readonly ITransferRepository _transferRepository = transferRepository;

    public async Task<Result<TransferResponse>> Handle(RegisterTransferCommand command, CancellationToken cancellationToken)
    {
        var (businessUnitId, accountTagId, value, relatedTo, description, settlementDate, type) = command;

        var businessUnit = await _businessUnitRepository.GetByIdAsync(businessUnitId, command.CurrentUserId, cancellationToken);
        if (businessUnit is null)
        {
            var errorMessage = $"Business Unit with Id {businessUnitId} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var accountTag = await _accountTagRepository.GetByIdAsync(accountTagId, command.CurrentUserId, cancellationToken);
        if (accountTag is null)
        {
            var errorMessage = $"Account Tag with Id {accountTagId} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        businessUnit.RegisterValue(value, type);
        _businessUnitRepository.Update(businessUnit);

        var monthlyBalance = await _monthlyBalanceRepository.GetByReferenceDateAndBusinessUnitId(
            settlementDate,
            businessUnitId,
            command.CurrentUserId,
            cancellationToken);

        if (monthlyBalance is null)
        {
            monthlyBalance = new MonthlyBalance(settlementDate, businessUnit, command.CurrentUserId);
            monthlyBalance.RegisterValue(value, type);
            _monthlyBalanceRepository.Insert(monthlyBalance);
        }
        else
        {
            monthlyBalance.RegisterValue(value, type);
            _monthlyBalanceRepository.Update(monthlyBalance);
        }

        var transfer = new Transfer(value, relatedTo, description, settlementDate, type, monthlyBalance, accountTag,
            command.CurrentUserId);
        _transferRepository.Insert(transfer);

        return Result.Ok(TransferMapper.DTR.Map(transfer));
    }
}