using MyFinance.Domain.Enums;
using MyFinance.Domain.Interfaces;
using MyFinance.Domain.ValueObjects;

namespace MyFinance.Domain.Entities
{
    public class MonthlyBalance : Entity, IAggregateRoot
    {
        public List<Transfer> Transfers { get; private set; }
        public double CurrentBalance { get; private set; }
        public ReferenceData ReferenceData { get; private set; }

        public MonthlyBalance(ReferenceData referenceData)
        {
            Transfers = new List<Transfer>();
            CurrentBalance = 0;
            ReferenceData = referenceData;
        }

        public void AddTransfer(Transfer transfer)
        {
            SetUpdateDate();
            CurrentBalance += transfer.Value;
            Transfers.Add(transfer);
        }

        public void AddTransfers(IEnumerable<Transfer> transfers)
        {
            SetUpdateDate();
            CurrentBalance += transfers.Sum(transfer => transfer.Value);
            Transfers.AddRange(transfers);
        }

        public Transfer GetTransferById(Guid transferId)
            => Transfers.Single(transfer => transfer.Id == transferId);

        public Transfer PopTransferById(Guid transferId)
        {
            SetUpdateDate();
            var transferToPop = GetTransferById(transferId);
            CurrentBalance -= transferToPop.Value;
            Transfers.Remove(transferToPop);
            return transferToPop;
        }
    }
}
