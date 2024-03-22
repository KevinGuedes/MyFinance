using MyFinance.Application.UseCases.BusinessUnits.Commands.ArchiveBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.Commands.CreateBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.Commands.UpdateBusinessUnit;
using MyFinance.Contracts.BusinessUnit.Requests;
using MyFinance.Contracts.BusinessUnit.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.Mappers;

public class BusinessUnitMapper
{
    public static class DTR
    {
        public static BusinessUnitResponse Map(BusinessUnit businessUnit)
            => new()
            {
                Id = businessUnit.Id,
                Name = businessUnit.Name,
                Income = businessUnit.Income,
                Outcome = businessUnit.Outcome,
                Balance = businessUnit.Balance,
                Description = businessUnit.Description,
                IsArchived = businessUnit.IsArchived,
                ReasonToArchive = businessUnit.ReasonToArchive
            };

        public static IReadOnlyCollection<BusinessUnitResponse> Map(IEnumerable<BusinessUnit> businessUnits)
            => businessUnits.Select(Map).ToList().AsReadOnly();
    }

    public static class RTC
    {
        public static CreateBusinessUnitCommand Map(CreateBusinessUnitRequest request)
            => new(request.Name, request.Description);

        public static UpdateBusinessUnitCommand Map(UpdateBusinessUnitRequest request)
            => new(request.Id, request.Name, request.Description);

        public static ArchiveBusinessUnitCommand Map(ArchiveBusinessUnitRequest request)
            => new(request.Id, request.ReasonToArchive);
    }
}