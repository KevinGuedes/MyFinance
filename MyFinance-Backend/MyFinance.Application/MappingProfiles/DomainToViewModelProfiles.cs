using AutoMapper;
using MyFinance.Application.UseCases.BusinessUnits.ViewModels;
using MyFinance.Application.UseCases.MonthlyBalances.ViewModels;
using MyFinance.Application.UseCases.Transfers.ViewModels;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.MappingProfiles;

public class DomainToViewModelProfiles : Profile
{
    public DomainToViewModelProfiles()
    {
        CreateMap<BusinessUnit, BusinessUnitViewModel>();
        CreateMap<MonthlyBalance, MonthlyBalanceViewModel>();
        CreateMap<Transfer, TransferViewModel>();
    }
}
