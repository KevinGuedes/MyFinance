namespace MyFinance.Domain.Entities
{
    public class MonthlyBalance : Entity
    {
        public MonthlyBalance(Guid businessUnitId, int month, int year)
            => (BusinessUnitId, Month, Year, _transfers) = (businessUnitId, month, year, new List<Transfer>());

        private List<Transfer> _transfers { get; set; }
        public IEnumerable<Transfer> Transfers { get => _transfers; }
        public double CurrentBalance { get => Transfers.Select(transfer => transfer.FormattedValue).Sum(); }
        public Guid BusinessUnitId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public void AddTransfer(Transfer transfer)
        {
            SetUpdateDate();
            VerifyTransferSettlementDateCompatibility(transfer.SettlementDate);
            _transfers.Add(transfer);
        }

        public Transfer GetTransferById(Guid transferId)
        {
            var transfer = _transfers.FirstOrDefault(transfer => transfer.Id == transferId);
            if (transfer is null) throw new InvalidOperationException("Transfer not found in this Monthly Balance");
            return transfer;
        }

        public void RemoveTransferById(Guid transferId)
        {
            SetUpdateDate();
            var transfer = GetTransferById(transferId);
            _transfers.Remove(transfer);
        }

        public void UpdateTransfer(Transfer updatedTransfer)
        {
            SetUpdateDate();
            var index = _transfers.FindIndex(transfer => transfer.Id == updatedTransfer.Id);
            if (index == -1) throw new InvalidOperationException("Transfer not found in this Monthly Balance");
            _transfers[index] = updatedTransfer;
        }

        private void VerifyTransferSettlementDateCompatibility(DateTime settlementDate)
        {
            if (settlementDate.Month != Month || settlementDate.Year != Year)
                throw new InvalidOperationException("This Transfer can not be added to this Monthly Balance");
        }
    }
}
