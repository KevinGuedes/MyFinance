using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.UpdateBusinessUnit;

public sealed class UpdateBusinessUnitCommand : ICommand<BusinessUnit>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public UpdateBusinessUnitCommand(Guid id, string name, string? description)
        => (Id, Name, Description) = (id, name, description);
}
