using AutoMapper;
using MyFinance.Application.BusinessUnits.Commands.CreateBusinessUnit;
using MyFinance.Application.BusinessUnits.ViewModels;

namespace MyFinance.Application.Profiles
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<BusinessUnitViewModel, CreateBusinessUnitCommand>()
                .ConstructUsing(businessUnitViewModel =>
                     new CreateBusinessUnitCommand(businessUnitViewModel.Name));
        }
    }
}
