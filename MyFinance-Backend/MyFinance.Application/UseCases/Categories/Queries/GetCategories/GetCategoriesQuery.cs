using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.Category.Responses;
using MyFinance.Contracts.Common;

namespace MyFinance.Application.UseCases.Categories.Queries.GetCategories;

public sealed record GetCategoriesQuery(int PageNumber, int PageSize)
    : IQuery<Paginated<CategoryResponse>>
{
}