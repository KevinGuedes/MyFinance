﻿using System.Globalization;
using ClosedXML.Excel;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;

namespace MyFinance.Infrastructure.Services.Spreadsheet;

public sealed class SpreadsheetService : ISpreadsheetService
{
    private static readonly string[] summaryColumnNames = ["Income", "Outcome", "Balance"];

    private static readonly string[] transferColumnNames =
        ["Value", "Related To", "Description", "Account Tag", "Settlement Date"];

    public Tuple<string, byte[]> GetBusinessUnitSummary(BusinessUnit businessUnit, int year)
    {
        double yearlyIncome = 0;
        double yearlyOutcome = 0;
        var workbookName = string.Format("Summary - {0} - {1}.xlsx", businessUnit.Name, year);
        var wb = new XLWorkbook();

        foreach (var monthlyBalance in businessUnit.MonthlyBalances)
        {
            yearlyIncome += monthlyBalance.Income;
            yearlyOutcome += monthlyBalance.Outcome;
            var referenceDate = new DateOnly(monthlyBalance.ReferenceYear, monthlyBalance.ReferenceMonth, 1);
            var wsName = referenceDate.ToString("y", new CultureInfo("en-US"));
            GenerateMonthlyBalanceSummary(monthlyBalance, wb, wsName);
        }

        GenerateBusinessUnitSummary(businessUnit, wb, year, yearlyIncome, yearlyOutcome);

        return new Tuple<string, byte[]>(workbookName, ConvertWorkbookToByteArray(wb));
    }

    public Tuple<string, byte[]> GetMonthlyBalanceSummary(MonthlyBalance monthlyBalance)
    {
        var referenceDate = new DateOnly(monthlyBalance.ReferenceYear, monthlyBalance.ReferenceMonth, 1);
        var wsName = referenceDate.ToString("y", new CultureInfo("en-US"));
        var businessUnitName = monthlyBalance.BusinessUnit.Name;
        var workbookName = $"Summary - {businessUnitName} - {wsName}.xlsx";
        var wb = new XLWorkbook();

        GenerateMonthlyBalanceSummary(monthlyBalance, wb, wsName);

        return new Tuple<string, byte[]>(workbookName, ConvertWorkbookToByteArray(wb));
    }

    private static void GenerateBusinessUnitSummary(
        BusinessUnit businessUnit,
        XLWorkbook wb,
        int year,
        double yearlyIncome,
        double yearlyOutcome)
    {
        var ws = wb.AddWorksheet(businessUnit.Name, 1);

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

        var businessUnitCurrentData = new[]
        {
            Math.Round(businessUnit.Income, 2),
            -1 * Math.Round(businessUnit.Outcome, 2),
            Math.Round(businessUnit.Balance, 2)
        };

        var businessUnitCurrentDataRange = ws.Cell(3, 1).InsertData(businessUnitCurrentData, true);
        businessUnitCurrentDataRange.Column(1).LastCell().Style.Font.SetFontColor(XLColor.Green);
        businessUnitCurrentDataRange.Column(2).LastCell().Style.Font.SetFontColor(XLColor.Red);
        var currentBalanceColor = GetBalanceColor(businessUnit.Balance);
        businessUnitCurrentDataRange.Column(3).LastCell().Style.Font.SetFontColor(currentBalanceColor);
        businessUnitCurrentDataRange.LastRow()
            .Style
            .NumberFormat.SetFormat("R$ #,##0.00")
            .Font.SetBold();

        businessUnitCurrentDataRange.Style
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        ws.Range(1, 5, 1, 7)
            .SetValue($"{year} Yearly Balance - {businessUnit.Name}")
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

        var yearlyBalance = yearlyIncome - yearlyOutcome;
        var businessUnitYearlyData = new[]
        {
            Math.Round(yearlyIncome, 2),
            -1 * Math.Round(yearlyOutcome, 2),
            Math.Round(yearlyBalance, 2)
        };

        var businessUnitYearlyDataRange = ws.Cell(3, 5).InsertData(businessUnitYearlyData, true);
        businessUnitYearlyDataRange.Column(1).LastCell().Style.Font.SetFontColor(XLColor.Green);
        businessUnitYearlyDataRange.Column(2).LastCell().Style.Font.SetFontColor(XLColor.Red);
        var yearlyBalanceColor = GetBalanceColor(yearlyBalance);
        businessUnitYearlyDataRange.Column(3).LastCell().Style.Font.SetFontColor(yearlyBalanceColor);
        businessUnitYearlyDataRange.LastRow()
            .Style.NumberFormat.SetFormat("R$ #,##0.00")
            .Font.SetBold();

        businessUnitYearlyDataRange.Style
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        ws.ColumnsUsed().Width = 12;
    }

    private static void GenerateMonthlyBalanceSummary(MonthlyBalance monthlyBalance, XLWorkbook wb, string wsName)
    {
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

        var transfersData = monthlyBalance.Transfers.Select(transfer =>
        {
            var value = transfer.Type == TransferType.Profit
                ? Math.Round(transfer.Value, 2)
                : -1 * Math.Round(transfer.Value, 2);

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

        var monthlyBalanceData = new[]
        {
            Math.Round(monthlyBalance.Income, 2),
            -1 * Math.Round(monthlyBalance.Outcome, 2),
            Math.Round(monthlyBalance.Balance, 2)
        };

        var monthlyBalanceDataRange = ws.Cell(3, lastColumnNumberForTransferData + spaceBetweenData)
            .InsertData(monthlyBalanceData, true);

        monthlyBalanceDataRange.LastRow()
            .Style
            .NumberFormat.SetFormat("R$ #,##0.00")
            .Font.SetBold();

        monthlyBalanceDataRange.Column(1).LastCell().Style.Font.SetFontColor(XLColor.Green);
        monthlyBalanceDataRange.Column(2).LastCell().Style.Font.SetFontColor(XLColor.Red);
        var balanceColor = GetBalanceColor(monthlyBalance.Balance);
        monthlyBalanceDataRange.Column(3).LastCell().Style.Font.SetFontColor(balanceColor);

        monthlyBalanceDataRange.Style
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

    private static XLColor GetBalanceColor(double balance)
        => balance >= 0 ? XLColor.Green : XLColor.Red;
}