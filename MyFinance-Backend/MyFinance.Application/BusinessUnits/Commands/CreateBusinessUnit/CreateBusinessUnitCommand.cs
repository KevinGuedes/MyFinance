using MyFinance.Application.Generics.Requests;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.BusinessUnits.Commands.CreateBusinessUnit
{
    public sealed class CreateBusinessUnitCommand : Command<BusinessUnit>
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public CreateBusinessUnitCommand(string name, string? description)
            => (Name, Description) = (name, description);
    }
}
