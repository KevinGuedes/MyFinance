using MediatR;
using Microsoft.Extensions.Logging;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Transfers.Commands.CreateTransfer
{
    internal sealed class CreateTransferRequestHandler : IRequestHandler<CreateTransferCommand, IEnumerable<Transfer>>
    {
        private readonly ILogger<CreateTransferRequestHandler> _logger;
        private readonly IMonthlyBalanceRepository _monthlyBalanceRepository;
        private readonly IBusinessUnitRepository _businessUnitRepository;

        public CreateTransferRequestHandler(
            ILogger<CreateTransferRequestHandler> logger, 
            IMonthlyBalanceRepository monthlyBalanceRepository, 
            IBusinessUnitRepository businessUnitRepository)
        {
            _logger = logger;
            _monthlyBalanceRepository = monthlyBalanceRepository;
            _businessUnitRepository = businessUnitRepository;
        }

        public Task<IEnumerable<Transfer>> Handle(CreateTransferCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Registering new transfer(s)");
            var transfersGroupedByReferenceDate = command.Transfers
                .GroupBy(transferData => new { transferData.SettlementDate.Month, transferData.SettlementDate.Year });

             
            //Processamento paralelo para executar esse loop
            //Cada group independe do outro
            foreach(var transferGroup in transfersGroupedByReferenceDate)
            {
                var month = transferGroup.Key.Month;
                var year = transferGroup.Key.Year;

                var monthlyBalance = _monthlyBalanceRepository.GetByMonthAndYearAsync(month, year, cancellationToken);
                //vai quebrar se não tiver

                foreach(var transferData in transferGroup)
                {
                    var transfer = new Transfer(
                        transferData.RelatedTo,
                        transferData.Description,
                        transferData.AbsoluteValue,
                        transferData.SettlementDate,
                        transferData.Type);

                    //monthlyBalance.AddTransfer(transfer);
                }
            };

            var x = new Transfer("abc", "descr", 100, DateTime.Now, TransferType.Profit);
            var y = new List<Transfer> { x };
            return Task.FromResult(y.AsEnumerable());
        }
    }
}
