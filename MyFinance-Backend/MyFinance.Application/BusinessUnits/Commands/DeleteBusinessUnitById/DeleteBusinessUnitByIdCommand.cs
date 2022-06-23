using MediatR;
using MyFinance.Application.Interfaces;

namespace MyFinance.Application.BusinessUnits.Commands.DeleteBusinessUnitById
{
    public sealed class DeleteBusinessUnitByIdCommand : IRequest, ICommand
    {
        public Guid BusinessUnitId { get; set; }

        public DeleteBusinessUnitByIdCommand(Guid businessUnitId)
            => BusinessUnitId = businessUnitId;
    }
}
