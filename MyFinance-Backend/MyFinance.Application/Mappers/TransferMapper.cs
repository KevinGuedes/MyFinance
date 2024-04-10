using MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfer;
using MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.Transfer.Requests;
using MyFinance.Contracts.Transfer.Responses;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.Mappers;

public static class TransferMapper
{
    public static class DTR
    {
        private const int AMOUNT_OF_MONTHS_IN_ONE_YEAR = 12;

        public static DiscriminatedAnnualBalanceDataResponse Map(
            int year,
            IEnumerable<(int Month, decimal Income, decimal Outcome)> discriminatedAnnualBalanceData)
        {
            var existingMonthlyBalances = discriminatedAnnualBalanceData.Select(monthlyBalanceData => new MonthlyBalanceDataResponse
            {
                Month = monthlyBalanceData.Month,
                Income = monthlyBalanceData.Income,
                Outcome = monthlyBalanceData.Outcome,
            });

            var filledMonthlyBalances = new Dictionary<int, MonthlyBalanceDataResponse>();

            foreach (var monthlyBalance in existingMonthlyBalances)
                filledMonthlyBalances[monthlyBalance.Month] = monthlyBalance;

            for (int month = 1; month <= AMOUNT_OF_MONTHS_IN_ONE_YEAR; month++)
            {
                var hasMonthlyBalanceForMonth = filledMonthlyBalances.ContainsKey(month);

                if (hasMonthlyBalanceForMonth)
                    continue;

                filledMonthlyBalances[month] = new MonthlyBalanceDataResponse
                {
                    Month = month,
                    Income = 0.0000m,
                    Outcome = 0.00000m,
                };
            }

            return new()
            {
                Year = year,
                MonthlyBalanceData = filledMonthlyBalances.Values
                    .OrderBy(monthlyBalance => monthlyBalance.Month)
                    .ToList()
                    .AsReadOnly()
            };
        }

        public static PeriodBalanceDataResponse Map((decimal Income, decimal Outcome) periodBalanceData)
            => new()
            {
                Income = periodBalanceData.Income,
                Outcome = periodBalanceData.Outcome,
            };

        public static Paginated<TransferGroupResponse> Map(
            (long TotalCount, IEnumerable<Transfer> Transfers) transfersData,
            int pageNumber,
            int pageSize)
        {
            var tranferGroups = transfersData.Transfers.GroupBy(
                transfer => transfer.SettlementDate.Date,
                transfer => transfer,
                (settlementDate, transfers) =>
                {
                    var income = 0m;
                    var outcome = 0m;

                    foreach (var transfer in transfers)
                    {
                        if (transfer.Type == TransferType.Profit)
                            income += transfer.Value;
                        else
                            outcome += transfer.Value;
                    }

                    return new TransferGroupResponse
                    {
                        Date = DateOnly.FromDateTime(settlementDate),
                        Transfers = Map(transfers),
                        Income = income,
                        Outcome = outcome
                    };
                });

            return new(
                tranferGroups.ToList().AsReadOnly(), 
                pageNumber, 
                pageSize, 
                transfersData.TotalCount);
        }

        public static TransferResponse Map(Transfer transfer)
            => new()
            {
                Id = transfer.Id,
                RelatedTo = transfer.RelatedTo,
                Description = transfer.Description,
                SettlementDate = transfer.SettlementDate,
                Type = transfer.Type,
                Value = transfer.Value
            };

        public static IReadOnlyCollection<TransferResponse> Map(IEnumerable<Transfer> transfers)
            => transfers.Select(Map).ToList().AsReadOnly();
    }

    public static class RTC
    {
        public static RegisterTransferCommand Map(RegisterTransferRequest request)
            => new(
                request.BusinessUnitId,
                request.AccountTagId,
                request.CategoryId,
                request.Value,
                request.RelatedTo,
                request.Description,
                request.SettlementDate,
                request.Type);

        public static UpdateTransferCommand Map(UpdateTransferRequest request)
            => new(
                request.Id,
                request.AccountTagId,
                request.CategoryId,
                request.Value,
                request.RelatedTo,
                request.Description,
                request.SettlementDate,
                request.Type);
    }
}