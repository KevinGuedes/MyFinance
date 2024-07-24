using MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfer;
using MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.Transfer.Requests;
using MyFinance.Contracts.Transfer.Responses;
using MyFinance.Domain.Entities;
using System.Globalization;

namespace MyFinance.Application.Mappers;

public static class TransferMapper
{
    public static class DTR
    {
        public static Paginated<TransferGroupResponse> Map(
            (long TotalCount, IEnumerable<(DateOnly Date, IEnumerable<Transfer> Transfers, decimal Income, decimal Outcome)> TransferGroups) transfersGroupsData,
            int pageNumber,
            int pageSize)
        {
            var (totalCount, transferGroups) = transfersGroupsData;

            var transferGroupsMapped = transferGroups
                .Select(transferGroup =>
                {
                    var (date, transfers, income, outcome) = transferGroup;
                    return new TransferGroupResponse
                    {
                        Date = date,
                        Income = income,
                        Outcome = outcome,
                        Transfers = Map(transfers)
                    };
                })
                .ToList()
                .AsReadOnly();

            return new(
                transferGroupsMapped,
                pageNumber,
                pageSize,
                totalCount);
        }

        public static TransferResponse Map(Transfer transfer)
            => new()
            {
                Id = transfer.Id,
                RelatedTo = transfer.RelatedTo,
                Description = transfer.Description,
                SettlementDate = transfer.SettlementDate,
                Type = transfer.Type,
                Value = transfer.Value,
                Tag = transfer.AccountTag?.Tag,
                CategoryName = transfer.Category?.Name
            };

        public static IReadOnlyCollection<TransferResponse> Map(IEnumerable<Transfer> transfers)
            => transfers.Select(Map).ToList().AsReadOnly();

        public static DiscriminatedBalanceDataResponse Map(
            IEnumerable<(int Year, int Month, decimal Income, decimal Outcome)> discriminatedBalanceData)
        {
            return new()
            {
                DiscriminatedBalanceData = discriminatedBalanceData.Select(monthlyBalanceData =>
                {
                    var referenceDate = new DateTime(monthlyBalanceData.Year, monthlyBalanceData.Month, 1);
                    return new MonthlyBalanceDataResponse
                    {
                        Income = monthlyBalanceData.Income,
                        Outcome = monthlyBalanceData.Outcome,
                        Month = monthlyBalanceData.Month,
                        Year = monthlyBalanceData.Year,
                        Reference = referenceDate.ToString("MMM/yy", CultureInfo.InvariantCulture)
                    };
                })
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