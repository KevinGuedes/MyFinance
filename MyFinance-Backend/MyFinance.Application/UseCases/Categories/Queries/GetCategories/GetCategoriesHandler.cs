using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Category.Responses;
using MyFinance.Contracts.Common;

namespace MyFinance.Application.UseCases.Categories.Queries.GetCategories;

internal sealed class GetCategoriesHandler(ICategoryRepository categoryRepository)
    : IQueryHandler<GetCategoriesQuery, Paginated<CategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<Result<Paginated<CategoryResponse>>> Handle(GetCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetPaginatedAsync(
            query.PageNumber,
            query.PageSize,
            cancellationToken);

        var response = CategoryMapper.DTR.Map(categories, query.PageNumber, query.PageSize);

        return Result.Ok(response);
    }
}