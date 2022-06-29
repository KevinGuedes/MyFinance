using MyFinance.Application.Interfaces;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.BusinessUnits.Commands.CreateBusinessUnit
{
    public sealed class CreateBusinessUnitCommand : ICommand<BusinessUnit>
    {
        public string Name { get; set; }

        public CreateBusinessUnitCommand(string name)
            => Name = name;
    }
}
