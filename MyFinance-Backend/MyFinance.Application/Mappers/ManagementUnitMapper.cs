using MyFinance.Application.UseCases.ManagementUnits.Commands.ArchiveManagementUnit;
using MyFinance.Application.UseCases.ManagementUnits.Commands.CreateManagementUnit;
using MyFinance.Application.UseCases.ManagementUnits.Commands.UpdateManagementUnit;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.ManagementUnit.Requests;
using MyFinance.Contracts.ManagementUnit.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.Mappers;

public class ManagementUnitMapper
{
    public static class DTR
    {
        private const int MONTHS_IN_ONE_YEAR = 12;

        public static Paginated<ManagementUnitResponse> Map(
            IEnumerable<ManagementUnit> managementUnits,
            int pageNumber,
            int pageSize,
            long totalCount)
            => new(Map(managementUnits), pageNumber, pageSize, totalCount);

        public static ManagementUnitResponse Map(ManagementUnit managementUnit)
            => new()
            {
                Id = managementUnit.Id,
                Name = managementUnit.Name,
                Income = managementUnit.Income,
                Outcome = managementUnit.Outcome,
                Balance = managementUnit.Balance,
                Description = managementUnit.Description,
                IsArchived = managementUnit.IsArchived,
                ReasonToArchive = managementUnit.ReasonToArchive
            };

        public static IReadOnlyCollection<ManagementUnitResponse> Map(
            IEnumerable<ManagementUnit> managementUnits)
            => managementUnits.Select(Map).ToList().AsReadOnly();

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

            for (int month = 1; month <= MONTHS_IN_ONE_YEAR; month++)
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
    }

    public static class RTC
    {
        public static CreateManagementUnitCommand Map(CreateManagementUnitRequest request)
            => new(request.Name, request.Description);

        public static UpdateManagementUnitCommand Map(UpdateManagementUnitRequest request)
            => new(request.Id, request.Name, request.Description);

        public static ArchiveManagementUnitCommand Map(ArchiveManagementUnitRequest request)
            => new(request.Id, request.ReasonToArchive);
    }
}