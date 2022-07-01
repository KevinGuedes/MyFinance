using MyFinance.Application.Generics.Requests;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.BusinessUnits.Commands.CreateBusinessUnit
{
    public sealed class CreateBusinessUnitCommand : Command<BusinessUnit>
    {
        public string Name { get; set; }

        public CreateBusinessUnitCommand(string name)
            => Name = name;
    }
}
