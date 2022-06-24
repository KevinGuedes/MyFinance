namespace MyFinance.Domain.Entities
{
    public class MonthlyBalance : Entity
    {
        public List<Transfer> Transfers { get; private set; }
        public double CurrentBalance { get => Transfers.Select(transfer => transfer.FormattedValue).Sum(); }
        public Guid BusinessUnitId { get; private set; }
        public int Month { get; private set; }
        public int Year { get; private set; }

        public MonthlyBalance(Guid businessUnitId, int month, int year)
            => (BusinessUnitId, Month, Year, Transfers) = (businessUnitId, month, year, new List<Transfer>());
        
        public void AddTransfer(Transfer transfer)
        {
            SetUpdateDate();
            VerifyTransferSettlementDateCompatibility(transfer.SettlementDate);
            Transfers.Add(transfer);
        }

        public void AddTransfers(IEnumerable<Transfer> transfers)
        {
            SetUpdateDate();
            foreach (var transfer in transfers)
                VerifyTransferSettlementDateCompatibility(transfer.SettlementDate);

            Transfers.AddRange(transfers);
        }

        public Transfer GetTransferById(Guid transferId)
        {
            var transfer = Transfers.FirstOrDefault(transfer => transfer.Id == transferId);
            if (transfer is null) throw new InvalidOperationException("Transfer not found in this Monthly Balance");
            return transfer;
        }

        public void DeleteTransferById(Guid transferId)
        {
            SetUpdateDate();
            var transfer = GetTransferById(transferId);
            Transfers.Remove(transfer);
        }

        public void UpdateTransfer(Transfer updatedTransfer)
        {
            SetUpdateDate();
            var index = Transfers.FindIndex(transfer => transfer.Id == updatedTransfer.Id);
            if (index == -1) throw new InvalidOperationException("Transfer not found in this Monthly Balance");
            Transfers[index] = updatedTransfer;
        }

        private void VerifyTransferSettlementDateCompatibility(DateTime settlementDate)
        {
            if (settlementDate.Month != Month || settlementDate.Year != Year)
                throw new InvalidOperationException("This Transfer can not be added to this Monthly Balance");
        }
    }
}
