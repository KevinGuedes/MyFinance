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
            double businessUnitRevenue = 0;
            var transfersGroupedByReferenceDate = command.Transfers
                .GroupBy(transferData => new { transferData.SettlementDate.Month, transferData.SettlementDate.Year });

            foreach(var transferGroup in transfersGroupedByReferenceDate)
            {
                var month = transferGroup.Key.Month;
                var year = transferGroup.Key.Year;
                var newTransfers = new List<Transfer>();

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
                var monthlyBalance = await _monthlyBalanceRepository.GetByMonthAndYearAsync(month, year, cancellationToken);
                if (monthlyBalance is not null)
                {
                    monthlyBalance.AddTransfers(newTransfers);
                    _monthlyBalanceRepository.Update(monthlyBalance);
                }
                else
                {
                    monthlyBalance = new MonthlyBalance(command.BusinessUnitId, month, year);
                    monthlyBalance.AddTransfers(newTransfers);
                    _monthlyBalanceRepository.Insert(monthlyBalance);
                }
            }

            var businessUnit = await _businessUnitRepository.GetByIdAsync(command.BusinessUnitId, cancellationToken);
            businessUnit.AddRevenue(businessUnitRevenue);
            _businessUnitRepository.Update(businessUnit);

            return Unit.Value;
        }
    }
}
