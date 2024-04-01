using MyFinance.Application.UseCases.Categories.Commands.ArchiveCategory;
using MyFinance.Application.UseCases.Categories.Commands.CreateCategory;
using MyFinance.Application.UseCases.Categories.Commands.UpdateCategory;
using MyFinance.Contracts.Category.Requests;
using MyFinance.Contracts.Category.Responses;
using MyFinance.Contracts.Common;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.Mappers;

public static class CategoryMapper
{
    public static class DTR
    {
        public static Paginated<CategoryResponse> Map(
            IEnumerable<Category> categories,
            int pageNumber,
            int pageSize)
            => new(Map(categories), pageNumber, pageSize, 0);

        public static CategoryResponse Map(Category category)
               => new()
               {
                   Id = category.Id,
                   Name = category.Name,
                   IsArchived = category.IsArchived,
                   ReasonToArchive = category.ReasonToArchive,
               };

        public static IReadOnlyCollection<CategoryResponse> Map(IEnumerable<Category> categories)
            => categories.Select(Map).ToList().AsReadOnly();
    }


    public static class RTC
    {
        public static CreateCategoryCommand Map(CreateCategoryRequest request)
            => new(request.Name);

        public static UpdateCategoryCommand Map(UpdateCategoryRequest request)
            => new(request.Id, request.Name);

        public static ArchiveCategoryCommand Map(ArchiveCategoryRequest request)
            => new(request.Id, request.ReasonToArchive);
    }
}
