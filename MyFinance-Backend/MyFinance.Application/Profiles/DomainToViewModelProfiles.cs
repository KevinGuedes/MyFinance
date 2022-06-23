using AutoMapper;
using MyFinance.Application.BusinessUnits.ViewModels;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.Profiles
{
    public class DomainToViewModelProfiles : Profile
    {
        public DomainToViewModelProfiles()
        {
            CreateMap<BusinessUnit, BusinessUnitViewModel>();
        }
    }
}
