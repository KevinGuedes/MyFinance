using ClosedXML.Excel;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;
using System.Globalization;

namespace MyFinance.Application.Services.Spreadsheet;

public class SpreadsheetService : ISpreadsheetService
{
    public Tuple<string, XLWorkbook> GetBusinessUnitSummary(BusinessUnit businessUnit)
    {
        var workbookName = string.Format("Summary - {0}.xlsx", businessUnit.Name);
        var wb = new XLWorkbook();

        GenerateBusinessUnitSummary(businessUnit, wb);

        foreach (var monthlyBalance in businessUnit.MonthlyBalances)
        {
            var referenceDate = new DateOnly(monthlyBalance.ReferenceYear, monthlyBalance.ReferenceMonth, 1);
            var wsName = referenceDate.ToString("y", new CultureInfo("en-US"));
            GenerateMonthlyBalanceSummary(monthlyBalance, wb, wsName);
        }

        return new(workbookName, wb);
    }

    public Tuple<string, XLWorkbook> GetMonthlyBalanceSummary(MonthlyBalance monthlyBalance)
    {
        var referenceDate = new DateOnly(monthlyBalance.ReferenceYear, monthlyBalance.ReferenceMonth, 1);
        var wsName = referenceDate.ToString("y", new CultureInfo("en-US"));
        var businessUnitName = monthlyBalance.BusinessUnit.Name;
        var workbookName = string.Format("Summary - {0} - {1}.xlsx", businessUnitName, wsName);
        var wb = new XLWorkbook();

        GenerateMonthlyBalanceSummary(monthlyBalance, wb, wsName);

        return new(workbookName, wb);
    }

    private static void GenerateBusinessUnitSummary(BusinessUnit businessUnit, XLWorkbook wb)
    {
        var ws = wb.AddWorksheet(string.Format("Summary - {0}", businessUnit.Name));

        ws.Range(1, 1, 1, 3)
               .SetValue(string.Format("Summary - {0}", businessUnit.Name))
               .Merge()
               .Style
               .Font.SetBold()
               .Fill.SetBackgroundColor(XLColor.CornflowerBlue)
               .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
               .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        ws.Cell(2, 1)
            .InsertData(new[] { "Income", "Outcome", "Balance" }, true)
            .Style
            .Font.SetBold()
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        var businessUnitData = new double[] {
            Math.Round(businessUnit.Income, 2),
            (-1 * Math.Round(businessUnit.Outcome, 2)),
            Math.Round(businessUnit.Balance, 2)
        };

        var businessUnitDataRange = ws.Cell(3, 1).InsertData(businessUnitData, true);

        businessUnitDataRange.LastRow()
            .Style
            .NumberFormat.SetFormat("R$ #,##0.00")
            .Font.SetBold();

        businessUnitDataRange.Column(1).LastCell().Style.Font.SetFontColor(XLColor.Green);
        businessUnitDataRange.Column(2).LastCell().Style.Font.SetFontColor(XLColor.Red);
        var balanceColor = businessUnit.Balance >= 0 ? XLColor.Green : XLColor.Red;
        businessUnitDataRange.Column(3).LastCell().Style.Font.SetFontColor(balanceColor);

        businessUnitDataRange.Style
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        ws.ColumnsUsed().Width = 12;
    }

    private static void GenerateMonthlyBalanceSummary(MonthlyBalance monthlyBalance, XLWorkbook wb, string wsName)
    {
        var ws = wb.AddWorksheet(wsName);

        ws.Range(1, 1, 1, 4)
                .SetValue("Transfers")
                .Merge()
                .Style
                .Font.SetBold()
                .Fill.SetBackgroundColor(XLColor.CornflowerBlue)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        ws.Cell(2, 1)
            .InsertData(new[] { "Value", "Related To", "Description", "Settlement Date" }, true)
            .Style
            .Font.SetBold()
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        var transfersData = monthlyBalance.Transfers.Select(transfer =>
        {
            var value = transfer.Type == TransferType.Profit ?
                Math.Round(transfer.Value, 2) :
                (-1 * Math.Round(transfer.Value, 2));

            return new
            {
                Value = value,
                transfer.RelatedTo,
                transfer.Description,
                SettlementDate = transfer.SettlementDate.ToString("dddd, dd MMMM yyyy HH:mm:ss"),
            };
        });

        var transfersDataRange = ws.Cell(3, 1).InsertData(transfersData);
        transfersDataRange.Style
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
        transfersDataRange.FirstColumn().Style
            .Font.SetBold()
            .NumberFormat.SetFormat("R$ #,##0.00");
        transfersDataRange.FirstColumn()
            .AddConditionalFormat()
            .WhenEqualOrGreaterThan(0).Font.SetFontColor(XLColor.Green);
        transfersDataRange.FirstColumn()
            .AddConditionalFormat()
            .WhenLessThan(0).Font.SetFontColor(XLColor.Red);

        ws.Range(1, 6, 1, 8)
            .SetValue(string.Format("Summary - {0}", wsName))
            .Merge()
            .Style
            .Font.SetBold()
            .Fill.SetBackgroundColor(XLColor.CornflowerBlue)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        ws.Cell(2, 6)
            .InsertData(new[] { "Income", "Outcome", "Balance" }, true)
            .Style
            .Font.SetBold()
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        var monthlyBalanceData = new double[] {
            Math.Round(monthlyBalance.Income, 2),
            (-1 * Math.Round(monthlyBalance.Outcome, 2)),
            Math.Round(monthlyBalance.Balance, 2)
        };

        var monthlyBalanceDataRange = ws.Cell(3, 6)
            .InsertData(monthlyBalanceData, true);

        monthlyBalanceDataRange.LastRow()
            .Style
            .NumberFormat.SetFormat("R$ #,##0.00")
            .Font.SetBold();

        monthlyBalanceDataRange.Column(1).LastCell().Style.Font.SetFontColor(XLColor.Green);
        monthlyBalanceDataRange.Column(2).LastCell().Style.Font.SetFontColor(XLColor.Red);
        var balanceColor = monthlyBalance.Balance >= 0 ? XLColor.Green : XLColor.Red;
        monthlyBalanceDataRange.Column(3).LastCell().Style.Font.SetFontColor(balanceColor);

        monthlyBalanceDataRange.Style
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        ws.Columns().AdjustToContents();
        ws.Column(1).Width = 12;
        ws.Column(6).Width = 12;
        ws.Column(7).Width = 12;
        ws.Column(8).Width = 12;
    }
}
