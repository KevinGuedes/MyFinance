using MyFinance.Application.Common.RequestHandling;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.BusinessUnits.Commands.UpdateBusinessUnit
{
    public sealed class UpdateBusinessUnitCommand : Command<BusinessUnit>
    {
        public Guid BusinessUnitId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public UpdateBusinessUnitCommand(Guid businessUnitId, string name, string? description)
            => (BusinessUnitId, Name, Description) = (businessUnitId, name, description);
    }
}
