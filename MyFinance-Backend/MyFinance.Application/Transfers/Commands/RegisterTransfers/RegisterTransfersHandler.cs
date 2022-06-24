using MediatR;
using Microsoft.Extensions.Logging;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Transfers.Commands.RegisterTransfers
{
    internal sealed class RegisterTransfersHandler : IRequestHandler<RegisterTransfersCommand>
    {
        private readonly ILogger<RegisterTransfersHandler> _logger;
        private readonly IMonthlyBalanceRepository _monthlyBalanceRepository;
        private readonly IBusinessUnitRepository _businessUnitRepository;

        public RegisterTransfersHandler(
            ILogger<RegisterTransfersHandler> logger, 
            IMonthlyBalanceRepository monthlyBalanceRepository, 
            IBusinessUnitRepository businessUnitRepository)
        {
            _logger = logger;
            _monthlyBalanceRepository = monthlyBalanceRepository;
            _businessUnitRepository = businessUnitRepository;
        }

        public async Task<Unit> Handle(RegisterTransfersCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Registering new transfer(s)");
            var businessUnitRevenue = 0D;
            var transfersGroupedByReferenceDate = command.Transfers
                .GroupBy(transferData => new { 
                    transferData.SettlementDate.Month, 
                    transferData.SettlementDate.Year 
                });

            foreach(var transferGroup in transfersGroupedByReferenceDate)
            {
                var month = transferGroup.Key.Month;
                var year = transferGroup.Key.Year;
                var newTransfers = new List<Transfer>();

                _logger.LogInformation("Registering transfers for {Month}/{Year}", month, year);
                foreach(var transferData in transferGroup)
                {
                    var transfer = new Transfer(
                        transferData.RelatedTo,
                        transferData.Description,
                        transferData.AbsoluteValue,
                        transferData.SettlementDate,
                        transferData.Type);

                    businessUnitRevenue += transfer.FormattedValue;
                    newTransfers.Add(transfer);
                }

                //retorna nulo mesmo?
                _logger.LogInformation("Verifying if the is an existing Monthly Balance related to the transfer(s)");
                var monthlyBalance = await _monthlyBalanceRepository.GetByMonthAndYearAsync(month, year, cancellationToken);
                if (monthlyBalance is not null)
                {
                    _logger.LogInformation("Adding new Transfer(s) to Monthly Balance with Id {MonthlyBalanceId}", monthlyBalance.Id);
                    monthlyBalance.AddTransfers(newTransfers);
                    _monthlyBalanceRepository.Update(monthlyBalance);
                    _logger.LogInformation("Monthly Balance with Id {MonthlyBalanceId} updated", monthlyBalance.Id);
                }
                else
                {
                    _logger.LogInformation("Creating new Monthly Balance");
                    monthlyBalance = new MonthlyBalance(command.BusinessUnitId, month, year);
                    _logger.LogInformation("Adding new Transfer(s) to Monthly Balance with Id {MonthlyBalanceId}", monthlyBalance.Id);
                    monthlyBalance.AddTransfers(newTransfers);
                    _monthlyBalanceRepository.Insert(monthlyBalance);
                    _logger.LogInformation("New Monthly Balance created with Id {MonthlyBalanceId}", monthlyBalance.Id);
                }
            }

            _logger.LogInformation("Updating balance of Business Unit with Id {BusinessUnitId}", command.BusinessUnitId);
            var businessUnit = await _businessUnitRepository.GetByIdAsync(command.BusinessUnitId, cancellationToken);
            businessUnit.AddBalance(businessUnitRevenue);
            _businessUnitRepository.Update(businessUnit);
            _logger.LogInformation("Balance of Business Unit with Id {BusinessUnitId} updated", command.BusinessUnitId);

            return Unit.Value;
        }
    }
}
