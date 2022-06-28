using MyFinance.Application.Interfaces;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.BusinessUnits.Commands.UpdateBusinessUnit
{
    public sealed class UpdateBusinessUnitCommand : ICommand<BusinessUnit>
    {
        public Guid BusinessUnitId { get; set; }
        public string Name { get; set; }
        public bool IsArchived { get; set; }

        public UpdateBusinessUnitCommand(Guid businessUnitId, string name, bool isArchived)
            => (BusinessUnitId, Name, IsArchived) = (businessUnitId, name, isArchived);
    }
}
