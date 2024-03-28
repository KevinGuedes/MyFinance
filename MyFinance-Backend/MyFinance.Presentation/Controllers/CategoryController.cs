using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Mappers;
using MyFinance.Application.UseCases.Categories.Commands.UnarchiveCategory;
using MyFinance.Application.UseCases.Categories.Queries.GetCategories;
using MyFinance.Contracts.Category.Requests;
using MyFinance.Contracts.Category.Responses;
using MyFinance.Contracts.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[SwaggerTag("Categories management")]
[SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(ProblemResponse))]
public class CategoryController(IMediator mediator) : ApiController(mediator)
{
    [HttpPost]
    [SwaggerOperation(Summary = "Registers a new Category")]
    [SwaggerResponse(StatusCodes.Status201Created, "Category registered", typeof(CategoryResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    public async Task<IActionResult> RegisterCategoryAsync(
        [FromBody] [SwaggerRequestBody("Category's payload", Required = true)]
        CreateCategoryRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(CategoryMapper.RTC.Map(request), cancellationToken), true);

    [HttpGet]
    [SwaggerOperation(Summary = "Lists all Categories with pagination")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of all existing Categories with pagination",
        typeof(Paginated<CategoryResponse>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid query parameters", typeof(ValidationProblemResponse))]
    public async Task<IActionResult> GetBusinessUnitsAsync(
        [FromQuery] [SwaggerParameter("Page number", Required = true)]
        int pageNumber,
        [FromQuery] [SwaggerParameter("Units per page", Required = true)]
        int pageSize,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(new GetCategoriesQuery(pageNumber, pageSize), cancellationToken));

    [HttpPut]
    [SwaggerOperation(Summary = "Updates an existing Category")]
    [SwaggerResponse(StatusCodes.Status200OK, "Updated Category", typeof(CategoryResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Category not found", typeof(ProblemResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Category was updated after the request was sent",
        typeof(ProblemResponse))]
    public async Task<IActionResult> UpdateBusinessUnitAsync(
        [FromBody] [SwaggerRequestBody("Category payload", Required = true)]
        UpdateCategoryRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(CategoryMapper.RTC.Map(request), cancellationToken));

    [HttpPatch("{id:guid}")]
    [SwaggerOperation(Summary = "Unarchives an existing Category")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Category successfully unarchived")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid Category Id", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Category not found", typeof(ProblemResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Category was updated after the request was sent",
        typeof(ProblemResponse))]
    public async Task<IActionResult> UnarchiveBusinessUnitAsync(
        [FromRoute] [SwaggerParameter("Id of the Category to unarchive", Required = true)]
        Guid id,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(new UnarchiveCategoryCommand(id), cancellationToken));

    [HttpDelete]
    [SwaggerOperation(Summary = "Logically deletes (archives) an existing Category")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Category successfully archived")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid payload", typeof(ValidationProblemResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Category not found", typeof(ProblemResponse))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The Category was updated after the request was sent",
        typeof(ProblemResponse))]
    public async Task<IActionResult> ArchiveBusinessUnitAsync(
        [FromBody] [SwaggerRequestBody("Payload to archvie a Category", Required = true)]
        ArchiveCategoryRequest request,
        CancellationToken cancellationToken)
        => ProcessResult(await _mediator.Send(CategoryMapper.RTC.Map(request), cancellationToken));
}