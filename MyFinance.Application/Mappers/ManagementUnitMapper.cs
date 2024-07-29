using MyFinance.Application.UseCases.ManagementUnits.Commands.ArchiveManagementUnit;
using MyFinance.Application.UseCases.ManagementUnits.Commands.CreateManagementUnit;
using MyFinance.Application.UseCases.ManagementUnits.Commands.UpdateManagementUnit;
using MyFinance.Contracts.ManagementUnit.Requests;

namespace MyFinance.Application.Mappers;

public class ManagementUnitMapper
{
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