using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Application.Services.CurrentUserProvider;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfer;

internal sealed class RegisterTransferHandler(
    ILogger<RegisterTransferHandler> logger,
    IMonthlyBalanceRepository monthlyBalanceRepository,
    IBusinessUnitRepository businessUnitRepository,
    ITransferRepository transferRepository,
    IAccountTagRepository accountTagRepository,
    ICurrentUserProvider currentUserProvider) : ICommandHandler<RegisterTransferCommand, Transfer>
{
    private readonly ILogger<RegisterTransferHandler> _logger = logger;
    private readonly IMonthlyBalanceRepository _monthlyBalanceRepository = monthlyBalanceRepository;
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly ITransferRepository _transferRepository = transferRepository;
    private readonly IAccountTagRepository _accountTagRepository = accountTagRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

    public async Task<Result<Transfer>> Handle(RegisterTransferCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();
        var (businessUnitId, accountTagId, value, relatedTo, description, settlementDate, type) = command;

        _logger.LogInformation("Retrieving Business Unit with Id {BusinessUnitId}", businessUnitId);
        var businessUnit = await _businessUnitRepository.GetByIdAsync(businessUnitId, currentUserId, cancellationToken);
        if (businessUnit is null)
        {
            _logger.LogWarning("Business Unit with Id {BusinessUnitId} not found", businessUnitId);
            var errorMessage = $"Business Unit with Id {businessUnitId} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        _logger.LogInformation("Retrieving Account Tag with Id {AccountTagId}", accountTagId);
        var accountTag = await _accountTagRepository.GetByIdAsync(accountTagId, currentUserId, cancellationToken);
        if (accountTag is null)
        {
            _logger.LogWarning("Account Tag with Id {AccountTagId} not found", accountTagId);
            var errorMessage = $"Account Tag with Id {accountTagId} not found";
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
            currentUserId,
            cancellationToken);

        if (monthlyBalance is null)
        {
            _logger.LogInformation("Creating new Monthly Balance");
            monthlyBalance = new MonthlyBalance(settlementDate, businessUnit, currentUserId);
            monthlyBalance.RegisterValue(value, type);
            _monthlyBalanceRepository.Insert(monthlyBalance);
        }
        else
        {
            _logger.LogInformation("Updating balance of Monthly Balance with Id {MonthlyBalanceId}", monthlyBalance.Id);
            monthlyBalance.RegisterValue(value, type);
            _monthlyBalanceRepository.Update(monthlyBalance);
        }

        _logger.LogInformation("Creating new Transfer");
        var transfer = new Transfer(value, relatedTo, description, settlementDate, type, monthlyBalance, accountTag, currentUserId);
        _transferRepository.Insert(transfer);
        _logger.LogInformation("New transfer successfully registered");

        return Result.Ok(transfer);
    }
}
