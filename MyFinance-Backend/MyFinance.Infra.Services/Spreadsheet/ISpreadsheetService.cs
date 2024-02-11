using ClosedXML.Excel;
using MyFinance.Domain.Entities;

namespace MyFinance.Infra.Services.Spreadsheet;

public interface ISpreadsheetService
{
    Tuple<string, XLWorkbook> GetBusinessUnitSummary(BusinessUnit businessUnit, int year);
    Tuple<string, XLWorkbook> GetMonthlyBalanceSummary(MonthlyBalance monthlyBalance);
}
