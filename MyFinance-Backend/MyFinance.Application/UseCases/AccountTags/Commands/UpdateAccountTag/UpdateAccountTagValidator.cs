using FluentValidation;
using MyFinance.Application.Services.CurrentUserProvider;
using MyFinance.Application.UseCases.AccountTags.Commands.UpdateAccountTag;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.UpdateBusinessUnit;

public sealed class UpdateAccountTagValidator : AbstractValidator<UpdateAccountTagCommand>
{
    private readonly IAccountTagRepository _accountTagRepository;
    private readonly ICurrentUserProvider _currentUserProvider;

    public UpdateAccountTagValidator(IAccountTagRepository accountTagRepository, ICurrentUserProvider currentUserProvider)
    {
        _accountTagRepository = accountTagRepository;
        _currentUserProvider = currentUserProvider;
        ClassLevelCascadeMode = CascadeMode.Stop;

        When(command => command.Description is not null, () =>
        {
            RuleFor(command => command.Description)
                .NotNull().WithMessage("{PropertyName} must not be null")
                .NotEmpty().WithMessage("{PropertyName} must not be empty")
                .Length(10, 200).WithMessage("{PropertyName} must have between 10 and 200 characters");
        });

        RuleFor(command => command.Id)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");

        RuleFor(command => command.Tag)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .Length(2, 6).WithMessage("{PropertyName} must have between 2 and 6 characters")
            .MustAsync(async (command, tag, cancellationToken) =>
            {
                var currentUserId = _currentUserProvider.GetCurrentUserId();

                var existingBusinessUnit = await _accountTagRepository.GetByTagAsync(tag, currentUserId, cancellationToken);
                if (existingBusinessUnit is null)
                    return true;

                var isValidName = existingBusinessUnit.Id == command.Id;
                return isValidName;
            }).WithMessage("This {PropertyName} has already been taken");
    }
}
