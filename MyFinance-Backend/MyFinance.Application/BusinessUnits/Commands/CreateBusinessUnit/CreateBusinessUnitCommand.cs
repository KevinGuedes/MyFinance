using MediatR;
using MyFinance.Application.Interfaces;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.BusinessUnits.Commands.CreateBusinessUnit
{
    public sealed class CreateBusinessUnitCommand : IRequest<BusinessUnit>, ICommand
    {
        public string Name { get; set; }

        public CreateBusinessUnitCommand(string name)
            => Name = name;
    }
}
