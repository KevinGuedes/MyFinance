using MediatR;
using MyFinance.Application.Interfaces;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.BusinessUnits.Commands.CreateBusinessUnit
{
    public class CreateBusinessUnitCommand : IRequest<BusinessUnit>, ICommand
    {
        public CreateBusinessUnitCommand(string name)
            => Name = name;

        public string Name { get; set; }
    }
}
