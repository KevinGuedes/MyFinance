using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Generics.Requests;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Domain.ValueObjects;

namespace MyFinance.Application.Transfers.Commands.RegisterTransfers
{
    internal sealed class RegisterTransfersHandler : CommandHandler<RegisterTransfersCommand>
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

        public async override Task<Result> Handle(RegisterTransfersCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Registering new transfer(s)");
            await RegisterNewTransfers(command, cancellationToken);
            _logger.LogInformation("New transfer(s) successfully registered");

            return Result.Ok();
        }

        private async Task RegisterNewTransfers(RegisterTransfersCommand command, CancellationToken cancellationToken)
        {
            var businessUnitRevenue = 0d;
            var transfersGroupedByReferenceData = command.Transfers
                .GroupBy(transferData => new ReferenceData(
                    command.BusinessUnitId,
                    transferData.SettlementDate.Year,
                    transferData.SettlementDate.Month
                ));

            await Parallel.ForEachAsync(
                transfersGroupedByReferenceData,
                cancellationToken,
                async (transferGroup, cancellationToken) =>
                {
                    var reference = transferGroup.Key;
                    _logger.LogInformation("Registering transfers for {ReferenceData}", reference);

                    var newTransfers = new List<Transfer>();
                    foreach (var transferData in transferGroup)
                    {
                        //var transfer = new Transfer(
                        //    transferData.RelatedTo,
                        //    transferData.Description,
                        //    transferData.Value,
                        //    transferData.SettlementDate,
                        //    transferData.Type);

                        //businessUnitRevenue += transfer.Value;
                        //newTransfers.Add(transfer);
                    }

                    //await AddTransfersToMonthlyBalance(reference, newTransfers, cancellationToken);
                });

            await UpdateBusinessUnitBalance(command.BusinessUnitId, businessUnitRevenue, cancellationToken);
        }

        private async Task AddTransfersToMonthlyBalance(
            BusinessUnit businessUnit,
            ReferenceData reference,
            List<Transfer> newTransfers,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Verifying if there is an existing Monthly Balance related to the transfer(s)");
            var monthlyBalance = await _monthlyBalanceRepository.GetByReferenceData(reference, cancellationToken);

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
                //monthlyBalance = new MonthlyBalance(reference);

                _logger.LogInformation("Adding new Transfer(s) to Monthly Balance with Id {MonthlyBalanceId}", monthlyBalance.Id);
                monthlyBalance.AddTransfers(newTransfers);
                _monthlyBalanceRepository.Insert(monthlyBalance);
                _logger.LogInformation("New Monthly Balance with Id {MonthlyBalanceId} created", monthlyBalance.Id);
            }
        }

        private async Task UpdateBusinessUnitBalance(Guid businessUnitId, double businessUnitRevenue, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating balance of Business Unit with Id {BusinessUnitId}", businessUnitId);
            var businessUnit = await _businessUnitRepository.GetByIdAsync(businessUnitId, cancellationToken);
            businessUnit.AddBalance(businessUnitRevenue);
            _businessUnitRepository.Update(businessUnit);
            _logger.LogInformation("Balance of Business Unit with Id {BusinessUnitId} updated", businessUnitId);
        }
    }
}
