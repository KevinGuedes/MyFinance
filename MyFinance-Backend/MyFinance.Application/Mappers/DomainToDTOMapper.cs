using MyFinance.Application.UseCases.BusinessUnits.DTOs;
using MyFinance.Application.UseCases.MonthlyBalances.DTOs;
using MyFinance.Application.UseCases.Transfers.DTOs;
using MyFinance.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace MyFinance.Application.MappingProfiles;

[Mapper]
public static partial class DomainToDTOMapper
{
    [MapperIgnoreSource(nameof(BusinessUnit.CreationDate))]
    [MapperIgnoreSource(nameof(BusinessUnit.UpdateDate))]
    [MapperIgnoreSource(nameof(BusinessUnit.MonthlyBalances))]
    public static partial BusinessUnitDTO BusinessUnitToDTO(BusinessUnit businessUnit);
    public static partial IEnumerable<BusinessUnitDTO> BusinessUnitToDTO(IEnumerable<BusinessUnit> businessUnits);

    [MapperIgnoreSource(nameof(MonthlyBalance.CreationDate))]
    [MapperIgnoreSource(nameof(MonthlyBalance.UpdateDate))]
    [MapperIgnoreSource(nameof(MonthlyBalance.BusinessUnit))]
    [MapperIgnoreSource(nameof(MonthlyBalance.Transfers))]
    public static partial MonthlyBalanceDTO MonthlyBalanceToDTO(MonthlyBalance monthlyBalance);
    public static partial IEnumerable<MonthlyBalanceDTO> MonthlyBalanceToDTO(IEnumerable<MonthlyBalance> monthlyBalances);

    [MapperIgnoreSource(nameof(Transfer.CreationDate))]
    [MapperIgnoreSource(nameof(Transfer.UpdateDate))]
    [MapperIgnoreSource(nameof(Transfer.UpdateDate))]
    [MapperIgnoreSource(nameof(Transfer.MonthlyBalanceId))]
    [MapperIgnoreSource(nameof(Transfer.MonthlyBalance))]
    public static partial TransferDTO TransferToDTO(Transfer transfer);
    public static partial IEnumerable<TransferDTO> TransferToDTO(IEnumerable<Transfer> transfers);

}
