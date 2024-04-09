using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Mappers;
using MyFinance.Application.UseCases.Transfers.Commands.DeleteTransfer;
using MyFinance.Application.UseCases.Transfers.Queries.GetAnnualBalanceData;
using MyFinance.Application.UseCases.Transfers.Queries.GetBalanceDataFromPeriod;
using MyFinance.Application.UseCases.Transfers.Queries.GetTransfers;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.Transfer.Requests;
using MyFinance.Contracts.Transfer.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[SwaggerTag("Transfers management")]
[SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(ProblemResponse))]
public class TransferController(IMediator mediator) : ApiController(mediator)
{
    [HttpGet]
    [SwaggerOperation(Summary = "Lists the Transfers retrived according to query parameters. Transfers are grouped by date")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of Transfers", typeof(Paginated<TransferGroupResponse>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid query parameters", typeof(ValidationProblemResponse))]
    public async Task<IActionResult> GetTransfersAsync(
        [FromQuery][SwaggerParameter("Business Unit Id", Required = true)]
        Guid businessUnitId,
        [FromQuery][SwaggerParameter("Start date")]
        DateOnly startDate,
        [FromQuery][SwaggerParameter("End date")]
        DateOnly endDate,
        [FromQuery][SwaggerParameter("Category Id")]
        Guid categoryId,
        [FromQuery][SwaggerParameter("Account Tag Id")]
        Guid accountTagId,
        [FromQuery][SwaggerParameter("Page number", Required = true)]
        int pageNumber,
        [FromQuery][SwaggerParameter("Units per page", Required = true)]
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = new GetTransfersQuery(
            businessUnitId,
            startDate,
            endDate,
            categoryId,
            accountTagId,
            pageNumber,
            pageSize);

        return ProcessResult(await _mediator.Send(query, cancellationToken));
    }

    [HttpGet("PeriodBalance")]
    [SwaggerOperation(Summary = "Gets the income, outcome and balance of the given period according to query parameters")]
    [SwaggerResponse(StatusCodes.Status200OK, "Balance data", typeof(PeriodBalanceDataResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid query parameters", typeof(ValidationProblemResponse))]
    public async Task<IActionResult> GetPeriodBalanceAsync(
        [FromQuery][SwaggerParameter("Business Unit Id", Required = true)]
        Guid businessUnitId,
        [FromQuery][SwaggerParameter("Start date")]
        DateOnly startDate,
        [FromQuery][SwaggerParameter("End date")]
        DateOnly endDate,
        [FromQuery][SwaggerParameter("Category Id")]
        Guid categoryId,
        [FromQuery][SwaggerParameter("Account Tag Id")]
        Guid accountTagId,
        CancellationToken cancellationToken)
    {
        var query = new GetBalanceDataFromPeriodQuery(
            businessUnitId,
            startDate,
            endDate,
            categoryId,
            accountTagId);

        return ProcessResult(await _mediator.Send(query, cancellationToken));
    }

    [HttpGet("AnnualBalance")]
    [SwaggerOperation(Summary = "Gets the income, outcome and balance for each month within a given year")]
    [SwaggerResponse(StatusCodes.Status200OK, "Annual balance data", typeof(AnnualBalanceDataResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid query parameters", typeof(ValidationProblemResponse))]
    public async Task<IActionResult> GetAnnualBalanceDataAsync(
        [FromQuery][SwaggerParameter("Business Unit Id", Required = true)]
        Guid businessUnitId,
        [FromQuery][SwaggerParameter("Year", Required = true)]
        int year,
        CancellationToken cancellationToken)
    {    
        var query = new GetAnnualBalanceDataQuery(businessUnitId, year);
        return ProcessResult(await _mediator.Send(query, cancellationToken));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Registers a new Transfer")]
    [SwaggerResponse(StatusCodes.Status201Created, "Transfer registered", typeof(TransferResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Business Unit not found", typeof(ProblemResponse))]
    public async Task<IActionResult> RegisterTransfersAsync(
        [FromBody] [SwaggerRequestBody("Transfers' payload", Required = true)]
        RegisterTransferRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(TransferMapper.RTC.Map(request), cancellationToken), true);

    [HttpPut]
    [SwaggerOperation(Summary = "Updates an existing Transfer")]
    [SwaggerResponse(StatusCodes.Status200OK, "Updated Transfer", typeof(TransferResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Transfer not found", typeof(ProblemResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Transfer was updated after the request was sent",
        typeof(ProblemResponse))]
    public async Task<IActionResult> UpdateTransferAsync(
        [FromBody] [SwaggerRequestBody("Transfers' payload", Required = true)]
        UpdateTransferRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(TransferMapper.RTC.Map(request), cancellationToken));

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Deletes an existing Transfer")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Transfer successfully deleted")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid Transfer Id", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Transfer not found", typeof(ProblemResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Transfer was updated after the request was sent",
        typeof(ProblemResponse))]
    public async Task<IActionResult> DeleteTransferAsync(
        [FromRoute] [SwaggerParameter("Transfer Id", Required = true)]
        Guid id,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(new DeleteTransferCommand(id), cancellationToken));
}