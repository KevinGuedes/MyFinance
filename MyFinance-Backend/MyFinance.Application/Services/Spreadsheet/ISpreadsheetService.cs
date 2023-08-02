using ClosedXML.Excel;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.Services.Spreadsheet;

public interface ISpreadsheetService
{
    Tuple<string, XLWorkbook> GetBusinessUnitSummary(
        BusinessUnit businessUnit, 
        IEnumerable<MonthlyBalance> monthlyBalancesForProcessing);
    Tuple<string, XLWorkbook> GetMonthlyBalanceSummary(MonthlyBalance monthlyBalance);
}
