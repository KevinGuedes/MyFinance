using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfer;

internal sealed class RegisterTransferHandler : ICommandHandler<RegisterTransferCommand, Transfer>
{
    private readonly ILogger<RegisterTransferHandler> _logger;
    private readonly IMonthlyBalanceRepository _monthlyBalanceRepository;
    private readonly IBusinessUnitRepository _businessUnitRepository;
    private readonly ITransferRepository _transferRepository;
    private readonly IAccountTagRepository _accountTagRepository;

    public RegisterTransferHandler(
        ILogger<RegisterTransferHandler> logger,
        IMonthlyBalanceRepository monthlyBalanceRepository,
        IBusinessUnitRepository businessUnitRepository,
        ITransferRepository transferRepository,
        IAccountTagRepository accountTagRepository)
    {
        _logger = logger;
        _monthlyBalanceRepository = monthlyBalanceRepository;
        _businessUnitRepository = businessUnitRepository;
        _transferRepository = transferRepository;
        _accountTagRepository = accountTagRepository;   
    }

    public async Task<Result<Transfer>> Handle(RegisterTransferCommand command, CancellationToken cancellationToken)
    {
        var (businessUnitId, accountTagId, value, relatedTo, description, settlementDate, type) = command;

        _logger.LogInformation("Retriving Business Unit with Id {BusinessUnitId}", businessUnitId);
        var businessUnit = await _businessUnitRepository.GetByIdAsync(businessUnitId, cancellationToken);
        if (businessUnit is null)
        {
            _logger.LogWarning("Business Unit with Id {BusinessUnitId} not found", businessUnitId);
            var errorMessage = string.Format("Business Unit with Id {0} not found", businessUnitId);
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        _logger.LogInformation("Retriving Account Tag with Id {AccountTagId}", accountTagId);
        var accountTag = await _accountTagRepository.GetByIdAsync(accountTagId, cancellationToken);
        if (accountTag is null)
        {
            _logger.LogWarning("Account Tag with Id {AccountTagId} not found", accountTagId);
            var errorMessage = string.Format("Account Tag with Id {0} not found", accountTagId);
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        _logger.LogInformation("Updating balance of Business Unit with Id {BusinessUnitId}", businessUnitId);
        businessUnit.RegisterValue(value, type);
        _businessUnitRepository.Update(businessUnit);

        _logger.LogInformation("Checking if there is an existing Monthly Balance to register new Transfer");
        var monthlyBalance = await _monthlyBalanceRepository.GetByReferenceDateAndBusinessUnitId(
            settlementDate,
            businessUnitId,
            cancellationToken);

        if (monthlyBalance is null)
        {
            _logger.LogInformation("Creating new Monthly Balance");
            monthlyBalance = new MonthlyBalance(settlementDate, businessUnit);
            monthlyBalance.RegisterValue(value, type);
            _monthlyBalanceRepository.Insert(monthlyBalance);
        }
        else
        {
            _logger.LogInformation("Updating balance of Monthly Balance with Id {MonthlyBalanceId}", monthlyBalance.Id);
            monthlyBalance.RegisterValue(value, type);
            _monthlyBalanceRepository.Update(monthlyBalance);
        }

        _logger.LogInformation("Creating new Transfer", businessUnitId);
        var transfer = new Transfer(value, relatedTo, description, settlementDate, type, monthlyBalance, accountTag);
        _transferRepository.Insert(transfer);
        _logger.LogInformation("New transfer(s) successfully registered");

        return Result.Ok(transfer);
    }
}
