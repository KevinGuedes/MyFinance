using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfers;

internal sealed class RegisterTransfersHandler : ICommandHandler<RegisterTransfersCommand, Transfer>
{
    private readonly ILogger<RegisterTransfersHandler> _logger;
    private readonly IMonthlyBalanceRepository _monthlyBalanceRepository;
    private readonly IBusinessUnitRepository _businessUnitRepository;
    private readonly ITransferRepository _transferRepository;

    public RegisterTransfersHandler(
        ILogger<RegisterTransfersHandler> logger,
        IMonthlyBalanceRepository monthlyBalanceRepository,
        IBusinessUnitRepository businessUnitRepository,
        ITransferRepository transferRepository)
    {
        _logger = logger;
        _monthlyBalanceRepository = monthlyBalanceRepository;
        _businessUnitRepository = businessUnitRepository;
        _transferRepository = transferRepository;
    }

    public async Task<Result<Transfer>> Handle(RegisterTransfersCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retriving Business Unit with Id {BusinessUnitId}", command.BusinessUnitId);
        var businessUnit = await _businessUnitRepository.GetByIdAsync(command.BusinessUnitId, cancellationToken);

        if (businessUnit is null)
        {
            _logger.LogWarning("Business Unit with Id {BusinessUnitId} not found", command.BusinessUnitId);
            var errorMessage = string.Format("Business Unit with Id {0} not found", command.BusinessUnitId);
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        _logger.LogInformation("Updating balance of Business Unit with Id {BusinessUnitId}", command.BusinessUnitId);
        businessUnit.RegisterValue(command.Value, command.Type);

        _logger.LogInformation("Checking if there is an existing Monthly Balance to register new Transfer");
        var monthlyBalance = await _monthlyBalanceRepository.GetByReferenceDateAndBusinessUnitId(
            command.SettlementDate,
            command.BusinessUnitId,
            cancellationToken);

        if (monthlyBalance is null)
        {
            _logger.LogInformation("Creating new Monthly Balance");
            monthlyBalance = new MonthlyBalance(command.SettlementDate, businessUnit);
            monthlyBalance.RegisterValue(command.Value, command.Type);
            _monthlyBalanceRepository.Insert(monthlyBalance);
        }
        else
        {
            _logger.LogInformation("Updating balance of Monthly Balance with Id {MonthlyBalanceId}", monthlyBalance.Id);
            monthlyBalance.RegisterValue(command.Value, command.Type);
            _monthlyBalanceRepository.Update(monthlyBalance);
        }

        _logger.LogInformation("Creating new Transfer", command.BusinessUnitId);
        var transfer = new Transfer(
            command.Value,
            command.RelatedTo,
            command.Description,
            command.SettlementDate,
            command.Type,
            monthlyBalance);
        _transferRepository.Insert(transfer);

        _logger.LogInformation("New transfer(s) successfully registered");
        return Result.Ok(transfer);
    }
}
