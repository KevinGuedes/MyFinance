using MyFinance.Application.UseCases.AccountTags.DTOs;
using MyFinance.Application.UseCases.BusinessUnits.DTOs;
using MyFinance.Application.UseCases.MonthlyBalances.DTOs;
using MyFinance.Application.UseCases.Transfers.DTOs;
using MyFinance.Application.UseCases.Users.DTOs;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.Mappers;

public static class DomainToDTOMapper
{
    public static BusinessUnitDTO BusinessUnitToDTO(BusinessUnit businessUnit)
        => new()
        {
            Id = businessUnit.Id,
            Name = businessUnit.Name,
            Income = businessUnit.Income,
            Outcome = businessUnit.Outcome,
            Balance = businessUnit.Balance,
            Description = businessUnit.Description,
            IsArchived = businessUnit.IsArchived,
            ReasonToArchive = businessUnit.ReasonToArchive,
            ArchiveDate = businessUnit.ArchiveDate
        };

    public static IEnumerable<BusinessUnitDTO> BusinessUnitToDTO(IEnumerable<BusinessUnit> businessUnits)
        => businessUnits.Select(BusinessUnitToDTO);

    public static MonthlyBalanceDTO MonthlyBalanceToDTO(MonthlyBalance monthlyBalance)
        => new()
        {
            Id = monthlyBalance.Id,
            Income = monthlyBalance.Income,
            Outcome = monthlyBalance.Outcome,
            Balance = monthlyBalance.Balance,
            ReferenceMonth = monthlyBalance.ReferenceMonth,
            ReferenceYear = monthlyBalance.ReferenceYear,
        };

    public static IEnumerable<MonthlyBalanceDTO> MonthlyBalanceToDTO(IEnumerable<MonthlyBalance> monthlyBalances)
        => monthlyBalances.Select(MonthlyBalanceToDTO);

    public static TransferDTO TransferToDTO(Transfer transfer)
        => new()
        {
            Id = transfer.Id,
            RelatedTo = transfer.RelatedTo,
            Description = transfer.Description,
            SettlementDate = transfer.SettlementDate,
            Type = transfer.Type,
            Value = transfer.Value
        };

    public static AccountTagDTO AccountTagToDTO(AccountTag accountTag)
        => new()
        {
            Id = accountTag.Id,
            Tag = accountTag.Tag,
            Description = accountTag.Description,
            IsArchived = accountTag.IsArchived,
            ReasonToArchive = accountTag.ReasonToArchive,
            ArchiveDate = accountTag.ArchiveDate
        };

    public static IEnumerable<AccountTagDTO> AccountTagToDTO(IEnumerable<AccountTag> businessUnits)
        => businessUnits.Select(AccountTagToDTO);

    public static UserDTO UserToDTO(User user)
        => new()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
}
