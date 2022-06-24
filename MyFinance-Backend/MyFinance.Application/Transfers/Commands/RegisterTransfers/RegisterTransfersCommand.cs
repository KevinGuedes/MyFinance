using MediatR;
using MyFinance.Application.Interfaces;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.Transfers.Commands.RegisterTransfers
{
    public sealed class RegisterTransfersCommand : IRequest, ICommand
    {
        public Guid BusinessUnitId { get; set; }
        public IEnumerable<TransferData> Transfers { get; set; }

        public RegisterTransfersCommand(Guid businessUnitId, IEnumerable<TransferData> transfers)
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
