namespace MyFinance.Application.Services.TransferProcessing
{
    public static class TransferProcessingHelper
    {
        public static bool ShouldUpdateBusinessUnitBalance(double currentValue, double newValue)
            => currentValue != newValue;

        public static bool ShouldGoToAnotherMonthlyBalance(DateTime currentSettlementDate, DateTime newSettlementDate)
            => currentSettlementDate.Month != newSettlementDate.Month || currentSettlementDate.Year != newSettlementDate.Year;
    }
}
