using ClosedXML.Excel;
using MyFinance.Application.Common.RequestHandling.Queries;

namespace MyFinance.Application.UseCases.Summary.Queries.GetBusinessUnitSummary;

public sealed record GetBusinessUnitSummaryQuery(Guid Id, int Year) : IQuery<Tuple<string, XLWorkbook>>
{
}
