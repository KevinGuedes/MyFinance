using MyFinance.Domain.Enums;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Domain.Entities
{
    public class MonthlyBalance : Entity, IAggregateRoot
    {
        public List<Transfer> Transfers { get; private set; }
        public double CurrentBalance { get; private set; }
        public Guid BusinessUnitId { get; private set; }
        public int Month { get; private set; }
        public int Year { get; private set; }

        public MonthlyBalance(Guid businessUnitId, int month, int year)
        {
            Transfers = new List<Transfer>();
            CurrentBalance = 0;
            BusinessUnitId = businessUnitId;
            Month = month;
            Year = year;
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

        public void DeleteTransferById(Guid transferId)
        {
            SetUpdateDate();
            var transferToRemove = GetTransferById(transferId);
            CurrentBalance -= transferToRemove.Value;
            Transfers.Remove(transferToRemove);
        }

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
