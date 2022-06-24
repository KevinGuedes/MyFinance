using MediatR;
using MyFinance.Application.Interfaces;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.Transfers.Commands.CreateTransfer
{
    public sealed class CreateTransferCommand : IRequest<IEnumerable<Transfer>>, ICommand
    {
        public Guid BusinessUnitId { get; set; }
        public IEnumerable<TransferData> Transfers { get; set; }

        public CreateTransferCommand(Guid businessUnitId, IEnumerable<TransferData> transfers)
            => (BusinessUnitId, Transfers) = (businessUnitId, transfers);
    }

    public sealed class TransferData
    {
        public double AbsoluteValue { get; set; }
        public string RelatedTo { get; set; }
        public string Description { get; set; }
        public DateTime SettlementDate { get; set; }
        public TransferType Type { get; set; }

        public TransferData(
            double absoluteValue,
            string relatedTo,
            string description,
            DateTime settlementDate,
            TransferType type)
        {
            AbsoluteValue = absoluteValue;
            RelatedTo = relatedTo;
            Description = description;
            SettlementDate = settlementDate;
            Type = type;
        }
    }
}
