using MyFinance.Application.Abstractions.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.Categories.Commands.UnarchiveCategory;

public sealed record UnarchiveCategoryCommand(Guid Id) : ICommand
{
}