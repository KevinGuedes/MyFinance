using ClosedXML.Excel;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;

namespace MyFinance.Infrastructure.Services.Summary;

public sealed class SummaryService : ISummaryService
{
    private const int initalRowForBalanceData = 3;
    private const int inititalRowForGenerationData = 1;
    private const int inititalRowForTransfersData = 1;

    private static readonly string[] summaryColumnNames = ["Income", "Outcome", "Balance"];
    private static readonly string[] transferColumnNames =
        ["Value", "Related To", "Description", "Category", "Account Tag", "Settlement Date"];

    public (string FileName, byte[] FileContent) GenerateMonthlySummary(
        BusinessUnit businessUnit, 
        IEnumerable<Transfer> transfers,
        int year, 
        int month)
    {
        var refrenceDateHumanized = new DateOnly(year, month, 1).ToString("MMMM yyyy");
        var workbookName = $"{businessUnit.Name} Summary - {refrenceDateHumanized}.xlsx";
        var wb = new XLWorkbook();

        var balanceWorksheet = wb.AddWorksheet(businessUnit.Name, 1);
        FillGenerationData(balanceWorksheet);
        FillCurrentBalanceData(businessUnit, balanceWorksheet);
        FillMonthlyBalanceData(transfers, balanceWorksheet, refrenceDateHumanized);
        balanceWorksheet.Columns().AdjustToContents();

        var transfersWorksheet = wb.AddWorksheet(refrenceDateHumanized, 2);
        FillTransfersData(businessUnit.Transfers, transfersWorksheet, refrenceDateHumanized);
        transfersWorksheet.Columns().AdjustToContents();

        return (FileName: workbookName, FileContent: ConvertWorkbookToByteArray(wb));
    }

    private static void FillGenerationData(IXLWorksheet ws)
    {
        var summaryGenerationDateHumanized = DateTime.UtcNow.ToString("dddd, dd MMMM yyyy HH:mm:ss");
        ws.Range(inititalRowForGenerationData, 1, inititalRowForGenerationData, 7)
            .SetValue($"Summary enerated {summaryGenerationDateHumanized}")
            .Merge()
            .Style
            .Font.SetBold()
            .Fill.SetBackgroundColor(XLColor.CornflowerBlue)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
    }

    private static void FillCurrentBalanceData(BusinessUnit businessUnit, IXLWorksheet ws)
    {
          ws.Range(initalRowForBalanceData, 1, initalRowForBalanceData, 3)
            .SetValue($"Current Balance")
            .Merge()
            .Style
            .Font.SetBold()
            .Fill.SetBackgroundColor(XLColor.CornflowerBlue)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        ws.Cell(initalRowForBalanceData + 1, 1)
            .InsertData(summaryColumnNames, true)
            .Style
            .Font.SetBold()
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        var currentBalanceData = new[]
        {
           businessUnit.Income, -1 * businessUnit.Outcome, businessUnit.Balance
        };

        var currentBalanceRange = ws.Cell(initalRowForBalanceData + 2, 1).InsertData(currentBalanceData, true);
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

    private static void FillMonthlyBalanceData(IEnumerable<Transfer> transfers, IXLWorksheet ws, string refrenceDateHumanized)
    {
        decimal monthlyIncome = 0;
        decimal monthlyOutcome = 0;

        foreach (var transfer in transfers)
        {
            if (transfer.Type == TransferType.Profit)
                monthlyIncome += transfer.Value;
            else
                monthlyOutcome += transfer.Value;
        }

        ws.Range(initalRowForBalanceData, 5, initalRowForBalanceData, 7)
          .SetValue($"Monthly Balance - {refrenceDateHumanized}")
          .Merge()
          .Style
          .Font.SetBold()
          .Fill.SetBackgroundColor(XLColor.CornflowerBlue)
          .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
          .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        ws.Cell(initalRowForBalanceData + 1, 5)
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
                var monthlyBalanceRange = ws.Cell(initalRowForBalanceData + 2, 5).InsertData(monthlyBalanceData, true);
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

    private static void FillTransfersData(IEnumerable<Transfer> transfers, IXLWorksheet ws, string refrenceDateHumanized)
    {
        ws.Range(inititalRowForTransfersData, 1, inititalRowForTransfersData, transferColumnNames.Length)
            .SetValue("Transfers")
            .Merge()
            .Style
            .Font.SetBold()
            .Fill.SetBackgroundColor(XLColor.CornflowerBlue)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        ws.Cell(inititalRowForTransfersData + 1, 1)
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
                Category = transfer.Category.Name,
                AccountTag = transfer.AccountTag.Tag,
                SettlementDate = transfer.SettlementDate.ToString("dddd, dd MMMM yyyy HH:mm:ss")
            };
        });

        var transfersDataRange = ws.Cell(inititalRowForTransfersData + 2, 1).InsertData(transfersData);
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

        ws.Columns().AdjustToContents();
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