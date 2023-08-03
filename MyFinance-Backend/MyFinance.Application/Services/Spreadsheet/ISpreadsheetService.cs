using ClosedXML.Excel;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.Services.Spreadsheet;

public interface ISpreadsheetService
{
    Tuple<string, XLWorkbook> GetBusinessUnitSummary(BusinessUnit businessUnit);
    Tuple<string, XLWorkbook> GetMonthlyBalanceSummary(MonthlyBalance monthlyBalance);
}
