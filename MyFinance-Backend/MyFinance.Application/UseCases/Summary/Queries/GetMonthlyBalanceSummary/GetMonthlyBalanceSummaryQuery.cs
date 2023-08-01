using ClosedXML.Excel;
using MyFinance.Application.Common.RequestHandling.Queries;

namespace MyFinance.Application.UseCases.Summary.Queries.GetMonthlyBalanceSummary;

public sealed record GetMonthlyBalanceSummaryQuery(Guid Id) : IQuery<Tuple<string, XLWorkbook>>
{
}
