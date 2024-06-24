using MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfer;
using MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.Transfer.Requests;
using MyFinance.Contracts.Transfer.Responses;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;
using System.Globalization;

namespace MyFinance.Application.Mappers;

public static class TransferMapper
{
    public static class DTR
    {
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

        public static DiscriminatedBalanceDataResponse Map(
            IEnumerable<(int Year, int Month, decimal Income, decimal Outcome)> discriminatedBalanceData,
            DateTime fromDate,
            DateTime toDate)
        {
            var existingMonthlyBalances = discriminatedBalanceData.Select(monthlyBalanceData => 
            {
                var referenceDate = new DateTime(monthlyBalanceData.Year, monthlyBalanceData.Month, 1);

                return new MonthlyBalanceDataResponse
                {
                    Reference = referenceDate.ToString("MMM/yy", CultureInfo.InvariantCulture),
                    Year = monthlyBalanceData.Year,
                    Month = monthlyBalanceData.Month,
                    Income = monthlyBalanceData.Income,
                    Outcome = monthlyBalanceData.Outcome,
                };
            });

            var filledMonthlyBalances = new Dictionary<string, MonthlyBalanceDataResponse>();

            foreach (var monthlyBalance in existingMonthlyBalances)
                filledMonthlyBalances[monthlyBalance.Reference] = monthlyBalance;

            var loopDate = fromDate;
            while (loopDate <= toDate)
            {
                var key = loopDate.ToString("MMM/yy", CultureInfo.InvariantCulture);

                if (filledMonthlyBalances.ContainsKey(key))
                    continue;

                filledMonthlyBalances[key] = new MonthlyBalanceDataResponse
                {
                    Reference = key,
                    Year = loopDate.Year,
                    Month = loopDate.Month,
                    Income = 0.0000m,
                    Outcome = 0.00000m,
                };

                loopDate = loopDate.AddMonths(1);
            }

            return new()
            {
                DiscriminatedBalanceData = filledMonthlyBalances.Values
                    .OrderBy(monthlyBalance => monthlyBalance.Year)
                    .ThenBy(monthlyBalance => monthlyBalance.Month)
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
    }

    public static class RTC
    {
        public static RegisterTransferCommand Map(RegisterTransferRequest request)
            => new(request.ManagementUnitId,
                request.AccountTagId,
                request.CategoryId,
                request.Value,
                request.RelatedTo,
                request.Description,
                request.SettlementDate,
                request.Type);

        public static UpdateTransferCommand Map(UpdateTransferRequest request)
            => new(request.Id,
                request.AccountTagId,
                request.CategoryId,
                request.Value,
                request.RelatedTo,
                request.Description,
                request.SettlementDate,
                request.Type);
    }
}