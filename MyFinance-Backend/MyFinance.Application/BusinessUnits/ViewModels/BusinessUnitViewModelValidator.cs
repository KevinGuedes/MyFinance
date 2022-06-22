using FluentValidation;

namespace MyFinance.Application.BusinessUnits.ViewModels
{
    public class BusinessUnitViewModelValidator : AbstractValidator<BusinessUnitViewModel>
    {
        public BusinessUnitViewModelValidator()
        {
            RuleFor(businessUnitViewModel => businessUnitViewModel.Name)
               .NotNull().WithMessage("{PropertyName} can not be null")
               .NotEmpty().WithMessage("{PropertyName} can not be empty")
               .Length(2, 50).WithMessage("{PropertyName} must have between 2 and 50 characters");
        }
    }
}
