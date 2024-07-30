using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.Category.Requests;

namespace MyFinance.Application.UseCases.Categories.Commands.ArchiveCategory;

public sealed class ArchiveCategoryCommand(ArchiveCategoryRequest request) : ICommand
{
    public Guid Id { get; init; } = request.Id;
    public string? ReasonToArchive { get; init; } = request.ReasonToArchive;
}