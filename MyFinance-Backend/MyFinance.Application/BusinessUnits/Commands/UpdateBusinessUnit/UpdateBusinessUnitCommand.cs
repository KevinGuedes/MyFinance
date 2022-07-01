using MyFinance.Application.Generics.Requests;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.BusinessUnits.Commands.UpdateBusinessUnit
{
    public sealed class UpdateBusinessUnitCommand : Command<BusinessUnit>
    {
        public Guid BusinessUnitId { get; set; }
        public string Name { get; set; }
        public bool IsArchived { get; set; }

        public UpdateBusinessUnitCommand(Guid businessUnitId, string name, bool isArchived)
            => (BusinessUnitId, Name, IsArchived) = (businessUnitId, name, isArchived);
    }
}
