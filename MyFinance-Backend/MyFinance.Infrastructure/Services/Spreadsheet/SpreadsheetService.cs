using System.Globalization;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;

//Data de geração do summary

namespace MyFinance.Infrastructure.Services.Spreadsheet;

public sealed class SpreadsheetService : ISpreadsheetService
{
    private static readonly string[] summaryColumnNames = ["Income", "Outcome", "Balance"];

    private static readonly string[] transferColumnNames =
        ["Value", "Related To", "Description", "Account Tag", "Settlement Date"];

    public Tuple<string, byte[]> GenerateMonthlySummary(BusinessUnit businessUnit, int year, int month)
    {
        var workbookName = $"{businessUnit.Name} Summary - {month}/{year}.xlsx";
        var wb = new XLWorkbook();

        FillBusinessUnitData(businessUnit, wb);
        FillTransfersData(businessUnit.Transfers, wb, year, month);

        return new Tuple<string, byte[]>(workbookName, ConvertWorkbookToByteArray(wb));
    }

    private static void FillBusinessUnitData(BusinessUnit businessUnit, XLWorkbook wb)
    {
        var ws = wb.AddWorksheet(businessUnit.Name, 1);
        ws.ColumnsUsed().Width = 12;

        FillCurrentBalanceData(businessUnit, ws);
        FillMonthlyBalanceData(businessUnit, ws);
    }

    private static void FillCurrentBalanceData(BusinessUnit businessUnit, IXLWorksheet ws)
    {
        ws.Range(1, 1, 1, 3)
            .SetValue($"Current Balance - {businessUnit.Name}")
            .Merge()
            .Style
            .Font.SetBold()
            .Fill.SetBackgroundColor(XLColor.CornflowerBlue)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        ws.Cell(2, 1)
            .InsertData(summaryColumnNames, true)
            .Style
            .Font.SetBold()
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        var currentBalanceData = new[]
        {
           businessUnit.Income, -1 * businessUnit.Outcome, businessUnit.Balance
        };

        var currentBalanceRange = ws.Cell(3, 1).InsertData(currentBalanceData, true);
        currentBalanceRange.Column(1).LastCell().Style.Font.SetFontColor(XLColor.Green);
        currentBalanceRange.Column(2).LastCell().Style.Font.SetFontColor(XLColor.Red);
        var currentBalanceColor = GetBalanceColor(businessUnit.Balance);
        currentBalanceRange.Column(3).LastCell().Style.Font.SetFontColor(currentBalanceColor);
        currentBalanceRange.LastRow()
            .Style
            .NumberFormat.SetFormat("R$ #,##0.00")
            .Font.SetBold();

        currentBalanceRange.Style
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
    }

    private static void FillMonthlyBalanceData(BusinessUnit businessUnit, IXLWorksheet ws)
    {
        decimal monthlyIncome = 0;
        decimal monthlyOutcome = 0;

        foreach (var transfer in businessUnit.Transfers)
        {
            if (transfer.Type == TransferType.Profit)
                monthlyIncome += transfer.Value;
            else
                monthlyOutcome += transfer.Value;
        }

        ws.Range(1, 5, 1, 7)
          .SetValue($"Monthly Balance - {businessUnit.Name}")
          .Merge()
          .Style
          .Font.SetBold()
          .Fill.SetBackgroundColor(XLColor.CornflowerBlue)
          .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
          .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        ws.Cell(2, 5)
            .InsertData(summaryColumnNames, true)
            .Style
            .Font.SetBold()
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        var monthlyBalance = monthlyIncome - monthlyOutcome;
        var monthlyBalanceData = new[]
        {
            monthlyIncome, -1 * monthlyOutcome, monthlyBalance
        };

        var monthlyBalanceRange = ws.Cell(3, 5).InsertData(monthlyBalanceData, true);
        monthlyBalanceRange.Column(1).LastCell().Style.Font.SetFontColor(XLColor.Green);
        monthlyBalanceRange.Column(2).LastCell().Style.Font.SetFontColor(XLColor.Red);
        var yearlyBalanceColor = GetBalanceColor(monthlyBalance);
        monthlyBalanceRange.Column(3).LastCell().Style.Font.SetFontColor(yearlyBalanceColor);
        monthlyBalanceRange.LastRow()
            .Style.NumberFormat.SetFormat("R$ #,##0.00")
            .Font.SetBold();

        monthlyBalanceRange.Style
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
    }

    private static void FillTransfersData(List<Transfer> transfers, XLWorkbook wb, int year, int month) {
        var wsName = $"{year}/{month}";
        var ws = wb.AddWorksheet(wsName);
        var numberOfColumnsForMonthlyBalanceData = summaryColumnNames.Length;
        var lastColumnNumberForTransferData = transferColumnNames.Length;
        const int spaceBetweenData = 2;

        ws.Range(1, 1, 1, lastColumnNumberForTransferData)
            .SetValue("Transfers")
            .Merge()
            .Style
            .Font.SetBold()
            .Fill.SetBackgroundColor(XLColor.CornflowerBlue)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        ws.Cell(2, 1)
            .InsertData(transferColumnNames, true)
            .Style
            .Font.SetBold()
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        var transfersData = transfers.Select(transfer =>
        {
            var value = transfer.Type == TransferType.Profit
                ? transfer.Value
                : -1 * transfer.Value;

            return new
            {
                Value = value,
                transfer.RelatedTo,
                transfer.Description,
                AccountTag = transfer.AccountTag.Tag,
                SettlementDate = transfer.SettlementDate.ToString("dddd, dd MMMM yyyy HH:mm:ss")
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

        ws.Range(
                1,
                lastColumnNumberForTransferData + spaceBetweenData,
                1,
                lastColumnNumberForTransferData + 1 + numberOfColumnsForMonthlyBalanceData)
            .SetValue(wsName)
            .Merge()
            .Style
            .Font.SetBold()
            .Fill.SetBackgroundColor(XLColor.CornflowerBlue)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        ws.Cell(2, lastColumnNumberForTransferData + 2)
            .InsertData(summaryColumnNames, true)
            .Style
            .Font.SetBold()
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        ws.Columns().AdjustToContents();
        ws.Column(1).Width = 12;

        var lastColumnNumber =
            lastColumnNumberForTransferData + spaceBetweenData + numberOfColumnsForMonthlyBalanceData;
        for (var i = lastColumnNumberForTransferData + spaceBetweenData; i < lastColumnNumber; i++)
            ws.Column(i).Width = 12;
    }

    private static byte[] ConvertWorkbookToByteArray(XLWorkbook workbook)
    {
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Close();
        return stream.ToArray();
    }

    private static XLColor GetBalanceColor(decimal balance)
        => balance >= 0 ? XLColor.Green : XLColor.Red;
}