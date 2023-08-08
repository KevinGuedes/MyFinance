using ClosedXML.Excel;
using MyFinance.Application.Common.RequestHandling.Queries;

namespace MyFinance.Application.UseCases.Summary.Queries.GetBusinessUnitSummary;

public sealed record GetBusinessUnitSummaryQuery(Guid Id) : IQuery<Tuple<string, XLWorkbook>>
{
}
