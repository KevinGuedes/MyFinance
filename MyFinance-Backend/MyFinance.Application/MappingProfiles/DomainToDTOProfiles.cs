using AutoMapper;
using MyFinance.Application.UseCases.BusinessUnits.DTOs;
using MyFinance.Application.UseCases.MonthlyBalances.DTOs;
using MyFinance.Application.UseCases.Transfers.DTOs;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.MappingProfiles;

public class DomainToDTOProfiles : Profile
{
    public DomainToDTOProfiles()
    {
        CreateMap<BusinessUnit, BusinessUnitDTO>();
        CreateMap<MonthlyBalance, MonthlyBalanceDTO>();
        CreateMap<Transfer, TransferDTO>();
    }
}
