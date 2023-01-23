using AutoMapper;
using MyFinance.Application.BusinessUnits.ViewModels;
using MyFinance.Application.MonthlyBalances.ViewModels;
using MyFinance.Application.Transfers.ViewModels;
using MyFinance.Domain.Entities;
using MyFinance.Domain.ValueObjects;

namespace MyFinance.Application.Profiles
{
    public class DomainToViewModelProfiles : Profile
    {
        public DomainToViewModelProfiles()
        {
            CreateMap<BusinessUnit, BusinessUnitViewModel>();
            CreateMap<MonthlyBalance, MonthlyBalanceViewModel>();
            CreateMap<Transfer, TransferViewModel>();
            CreateMap<ReferenceData, ReferenceDataViewModel>();
        }
    }
}
