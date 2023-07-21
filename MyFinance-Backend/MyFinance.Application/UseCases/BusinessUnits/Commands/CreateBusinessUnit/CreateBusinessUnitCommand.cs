using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.CreateBusinessUnit;

public sealed class CreateBusinessUnitCommand : ICommand<BusinessUnit>
{
    public string Name { get; set; }
    public string? Description { get; set; }

    public CreateBusinessUnitCommand(string name, string? description)
        => (Name, Description) = (name, description);
}
