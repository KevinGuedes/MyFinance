using MediatR;
using MyFinance.Application.Interfaces;

namespace MyFinance.Application.BusinessUnits.Commands.RemoveBusinessUnitById
{
    public sealed class RemoveBusinessUnitByIdCommand : IRequest, ICommand
    {
        public Guid BusinessUnitId { get; set; }

        public RemoveBusinessUnitByIdCommand(Guid businessUnitId)
            => BusinessUnitId = businessUnitId;
    }
}
