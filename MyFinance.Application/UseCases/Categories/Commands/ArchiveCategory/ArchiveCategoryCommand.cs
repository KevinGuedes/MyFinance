using MyFinance.Application.Abstractions.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.Categories.Commands.ArchiveCategory;

public sealed record ArchiveCategoryCommand(Guid Id, string? ReasonToArchive) : ICommand
{
}