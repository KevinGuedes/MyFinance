using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using FluentResults;
using MediatR;
using MyFinance.Application.UseCases.Summary.Queries.GetBusinessUnitSummary;
using MyFinance.Application.UseCases.Summary.Queries.GetMonthlyBalanceSummary;

namespace MyFinance.Application.UseCases.Summary.ApiService;

public class SummaryApiService : ISummaryApiService
{
    private readonly IMediator _mediator;

    public SummaryApiService(IMediator mediator)
        => _mediator = mediator;

    public async Task<Result<Tuple<string, byte[]>>> GetBusinessUnitSummaryAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBusinessUnitSummaryQuery(id), cancellationToken);

        var (workbookName, workbook) = result.Value;
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Close();

        return Result.Ok(new Tuple<string, byte[]>(workbookName, stream.ToArray()));
    }

    public async Task<Result<Tuple<string, byte[]>>> GetMonthlyBalanceSummaryAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMonthlyBalanceSummaryQuery(id), cancellationToken);

        var (workbookName, workbook) = result.Value;
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Close();

        return Result.Ok(new Tuple<string, byte[]>(workbookName, stream.ToArray()));
    }
}
