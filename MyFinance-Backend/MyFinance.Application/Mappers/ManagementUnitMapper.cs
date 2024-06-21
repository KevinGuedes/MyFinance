using MyFinance.Application.UseCases.ManagementUnits.Commands.ArchiveManagementUnit;
using MyFinance.Application.UseCases.ManagementUnits.Commands.CreateManagementUnit;
using MyFinance.Application.UseCases.ManagementUnits.Commands.UpdateManagementUnit;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.ManagementUnit.Requests;
using MyFinance.Contracts.ManagementUnit.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.Mappers;

public class ManagementUnitMapper
{
    public static class DTR
    {
        public static Paginated<ManagementUnitResponse> Map(
            IEnumerable<ManagementUnit> managementUnits,
            int pageNumber,
            int pageSize,
            long totalCount)
            => new(Map(managementUnits), pageNumber, pageSize, totalCount);

        public static ManagementUnitResponse Map(ManagementUnit managementUnit)
            => new()
            {
                Id = managementUnit.Id,
                Name = managementUnit.Name,
                Income = managementUnit.Income,
                Outcome = managementUnit.Outcome,
                Balance = managementUnit.Balance,
                Description = managementUnit.Description,
            };

        public static IReadOnlyCollection<ManagementUnitResponse> Map(
            IEnumerable<ManagementUnit> managementUnits)
            => managementUnits.Select(Map).ToList().AsReadOnly();
    }

    public static class RTC
    {
        public static CreateManagementUnitCommand Map(CreateManagementUnitRequest request)
            => new(request.Name, request.Description);

        public static UpdateManagementUnitCommand Map(UpdateManagementUnitRequest request)
            => new(request.Id, request.Name, request.Description);

        public static ArchiveManagementUnitCommand Map(ArchiveManagementUnitRequest request)
            => new(request.Id, request.ReasonToArchive);
    }
}