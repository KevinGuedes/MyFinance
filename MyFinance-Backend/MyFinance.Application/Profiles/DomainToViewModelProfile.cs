using AutoMapper;
using MyFinance.Application.BusinessUnits.ViewModels;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.Profiles
{
    public class DomainToViewModelProfile : Profile
    {
        public DomainToViewModelProfile()
        {
            CreateMap<BusinessUnit, BusinessUnitViewModel>();
        }
    }
}
