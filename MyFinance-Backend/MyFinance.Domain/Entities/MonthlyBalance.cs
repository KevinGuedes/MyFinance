using MyFinance.Domain.Interfaces;
using MyFinance.Domain.ValueObjects;

namespace MyFinance.Domain.Entities
{
    public class MonthlyBalance : Entity
    {
        public double CurrentBalance { get; private set; }
        public ReferenceData ReferenceData { get; private set; }
        public Guid BusinessUnitId { get; private set; }
        public virtual BusinessUnit BusinessUnit { get; private set; }
        public virtual List<Transfer> Transfers { get; private set; }
        
        protected MonthlyBalance() { }

        public MonthlyBalance(ReferenceData referenceData, BusinessUnit businessUnit)
        {
            Transfers = new List<Transfer>();
            CurrentBalance = 0;
            ReferenceData = referenceData;
            BusinessUnit = businessUnit;
            BusinessUnitId = businessUnit.Id;
        }

        public void AddTransfer(Transfer transfer)
        {
            SetUpdateDateToNow();
            CurrentBalance += transfer.Value;
            Transfers.Add(transfer);
        }

        public void AddTransfers(IEnumerable<Transfer> transfers)
        {
            SetUpdateDateToNow();
            CurrentBalance += transfers.Sum(transfer => transfer.Value);
            Transfers.AddRange(transfers);
        }

        public Transfer GetTransferById(Guid transferId)
            => Transfers.Single(transfer => transfer.Id == transferId);

        public Transfer PopTransferById(Guid transferId)
        {
            SetUpdateDateToNow();
            var transferToPop = GetTransferById(transferId);
            CurrentBalance -= transferToPop.Value;
            Transfers.Remove(transferToPop);
            return transferToPop;
        }
    }
}
