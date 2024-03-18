using MyFinance.Contracts.MonthlyBalance.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.Mappers;

public static class MonthlyBalanceMapper
{
    public static class DTR
    {
        public static MonthlyBalanceResponse Map(MonthlyBalance monthlyBalance)
            => new()
            {
                Id = monthlyBalance.Id,
                Income = monthlyBalance.Income,
                Outcome = monthlyBalance.Outcome,
                Balance = monthlyBalance.Balance,
                ReferenceMonth = monthlyBalance.ReferenceMonth,
                ReferenceYear = monthlyBalance.ReferenceYear
            };

        public static IReadOnlyCollection<MonthlyBalanceResponse> Map(IEnumerable<MonthlyBalance> monthlyBalances)
            => monthlyBalances.Select(Map).ToList().AsReadOnly();
    }
}
